using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class GuildModule : ModuleBase
{
    private UIGuildType _type;

    private Button _closeBtn;
    private UIBaseView _uiShowView;

    private RecommemdGuildView _recommemdGuildView;
    private GuildCreateView _createView;
    private GuildSearchView _searchView;

    private Toggle[] _guildTypeTog;
    private Image _imgBack;

    private Transform _root;

    public GuildModule()
        : base(ModuleID.Guild, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_Guild;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();

        _imgBack = Find<Image>("Root/ImageBack02");
        _imgBack.gameObject.SetActive(true);
        _type = UIGuildType.None;
        _guildTypeTog = new Toggle[3];
        Toggle tog;
        tog = Find<Toggle>("Root/TogGroup/Tog1");
        tog.onValueChanged.Add((bool value) => { if (value) OnToggleChange(UIGuildType.Recommemd); });
        _guildTypeTog[0] = tog;
        tog = Find<Toggle>("Root/TogGroup/Tog2");
        tog.onValueChanged.Add((bool value) => { if (value) OnToggleChange(UIGuildType.Create); });
        _guildTypeTog[1] = tog;
        tog = Find<Toggle>("Root/TogGroup/Tog3");
        tog.onValueChanged.Add((bool value) => { if (value) OnToggleChange(UIGuildType.Search); });
        _guildTypeTog[2] = tog;

        _recommemdGuildView = new RecommemdGuildView();
        _recommemdGuildView.SetDisplayObject(Find("Root/ContentRoot/Recommemd"));

        _createView = new GuildCreateView();
        _createView.SetDisplayObject(Find("Root/ContentRoot/CreateRoot"));

        _searchView = new GuildSearchView();
        _searchView.SetDisplayObject(Find("Root/ContentRoot/SearchRoot"));

        _closeBtn = Find<Button>("CloseBtn");
        _closeBtn.onClick.Add(OnClose);
        _root = Find<Transform>("Root");
        GuildItemFactory.Instance.InitGuildFactory(Find("Root/GuildItem"), mRectTransform);

        ColliderHelper.SetButtonCollider(_closeBtn.transform, 120, 120);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GuildDataModel.Instance.AddEvent(GuildEvent.GuildDataRefresh, OnCreateBack);
        GuildDataModel.Instance.AddEvent(GuildEvent.GuildAgreeJoinNotify, OnCreateBack);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GuildDataModel.Instance.RemoveEvent(GuildEvent.GuildDataRefresh, OnCreateBack);
        GuildDataModel.Instance.RemoveEvent(GuildEvent.GuildAgreeJoinNotify, OnCreateBack);
    }

    private void OnCreateBack()
    {
        GameUIMgr.Instance.CloseModule(ModuleID.Guild);
        GameUIMgr.Instance.OpenModule(ModuleID.HeroGuild);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        if (args != null && args.Length != 0)
        {
            //for (int i = 0; i < _guildTypeTog.Length; i++)
            //    _guildTypeTog[i].isOn = false;
            _type = (UIGuildType)args[0];
            int idx = (int)_type;
            _guildTypeTog[idx - 1].isOn = true;
        }
        else
        {
            if (_type == UIGuildType.None)
                _type = UIGuildType.Recommemd;
        }
        OnToggleChange(_type);
    }

    private void OnToggleChange(UIGuildType type)
    {
        _type = type;
        if (_uiShowView != null)
            _uiShowView.Hide();
        switch (_type)
        {
            case UIGuildType.Recommemd:
                _uiShowView = _recommemdGuildView;
                _imgBack.color = new Color(0.3f, 0.38f, 0.56f, 1.0f);
                _imgBack.gameObject.SetActive(true);
                break;
            case UIGuildType.Create:
                _uiShowView = _createView;
                _imgBack.color = new Color(0.3f, 0.38f, 0.56f, 1.0f);
                _imgBack.gameObject.SetActive(true);
                break;
            case UIGuildType.Search:
                _uiShowView = _searchView;
                _imgBack.gameObject.SetActive(false);
                break;
        }
        _uiShowView.Show();
    }

    public override void Hide()
    {
        base.Hide();
        if (_uiShowView != null)
            _uiShowView.Hide();
    }

    public override void Dispose()
    {
        if (_recommemdGuildView != null)
        {
            _recommemdGuildView.Dispose();
            _recommemdGuildView = null;
        }

        if (_createView != null)
        {
            _createView.Dispose();
            _createView = null;
        }

        if (_searchView != null)
        {
            _searchView.Dispose();
            _searchView = null;
        }
        _uiShowView = null;
        GuildItemFactory.Instance.Dispose();
        base.Dispose();
    }
    protected override void OnShowAnimator()
    {
        base.OnShowAnimator();
        ObjectHelper.PopAnimationLiner(_root);
    }
}