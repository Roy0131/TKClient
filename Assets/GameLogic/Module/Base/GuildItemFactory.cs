using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Framework.UI;

public enum GuildItemType
{
    None,
    Recommemd,
    Search,
}

public class GuildItemFactory : Singleton<GuildItemFactory>
{

    private GameObject _itemPrefab;
    private RectTransform _root;
    private Queue<GuildItemView> _itemPools;
    public void InitGuildFactory(GameObject itemPrefab, RectTransform root)
    {
        _itemPrefab = itemPrefab;
        _root = root;
        _itemPools = new Queue<GuildItemView>();
    }

    public GuildItemView CreateGuildItem(GuildItemType type)
    {
        GuildItemView view;
        if (_itemPools.Count > 0)
        {
            view = _itemPools.Dequeue();
        }
        else
        {
            view = new GuildItemView();
            view.SetDisplayObject(GameObject.Instantiate(_itemPrefab));
        }
        view.SetParam(type);
        return view;
    }

    public void ReturnGuildItem(GuildItemView view)
    {
        view.Hide();
        view.mRectTransform.SetParent(_root, false);
        _itemPools.Enqueue(view);
    }

    public void Dispose()
    {
        if (_itemPools != null)
        {
            while (_itemPools.Count > 0)
                _itemPools.Dequeue().Dispose();
            _itemPools.Clear();
            _itemPools = null;
        }
        _itemPrefab = null;
        _root = null;
    }
}

public class GuildItemView : UIBaseView
{
    private Button _funBtn;
    private GuildItemType _type;

    private Image _logoIcon;
    private Text _guildNameText;
    private Text _memberText;
    private Text _levelText;

    private GuildDataVO _vo;
    protected override void ParseComponent()
    {
        base.ParseComponent();

        _logoIcon = Find<Image>("LogoIcon/Image");
        _guildNameText = Find<Text>("GuildNameText");
        _memberText = Find<Text>("MemberText");
        _levelText = Find<Text>("LevelRoot/LevelText");

        _funBtn = Find<Button>("Button");
        _funBtn.onClick.Add(OnFunction);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _vo = args[0] as GuildDataVO;
        _logoIcon.sprite = GameResMgr.Instance.LoadGuildIcon(_vo.mLogoIcon);
        ObjectHelper.SetSprite(_logoIcon,_logoIcon.sprite);
        _guildNameText.text = _vo.mGuildName;
        _memberText.text = _vo.mCurMembers + "/" + _vo.mMaxMembers;
        if (_vo.mCurMembers >= _vo.mMaxMembers)
            _memberText.text = _vo.mMaxMembers + "/" + _vo.mMaxMembers;
        else
            _memberText.text = _vo.mCurMembers + "/" + _vo.mMaxMembers;
        _levelText.text = _vo.mLevel.ToString();

        ObjectHelper.SetEnableStatus(_funBtn, !_vo.BlApplyed);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GuildDataModel.Instance.AddEvent<int>(GuildEvent.GuildAskJoinBack, OnRefreshStatus);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GuildDataModel.Instance.RemoveEvent<int>(GuildEvent.GuildAskJoinBack, OnRefreshStatus);
    }

    private void OnRefreshStatus(int guildID)
    {
        if (_vo == null || _vo.mGuildId != guildID)
            return;
        PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(5003150));
        _vo.BlApplyed = true;
        ObjectHelper.SetEnableStatus(_funBtn, !_vo.BlApplyed);
    }

    private void OnFunction()
    {
        switch (_type)
        {
            case GuildItemType.Recommemd:
            case GuildItemType.Search:
                GameNetMgr.Instance.mGameServer.ReqAskJoinGuild(_vo.mGuildId);
                break;
        }
    }

    public override void Hide()
    {
        _vo = null;
        _type = GuildItemType.None;
        base.Hide();
    }

    public void SetParam(GuildItemType type)
    {
        _type = type;
    }

    public override void Dispose()
    {
        _vo = null;
        _type = GuildItemType.None;
        base.Dispose();
    }
}