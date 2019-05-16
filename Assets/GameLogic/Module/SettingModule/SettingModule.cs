using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class SettingTypeConst
{
    public const int Setting = 1;//设置
    public const int Announcement = 2;//公告
    public const int Server = 3;//服务器
    public const int Feedback = 4;//反馈
}

public class SettingModule : ModuleBase
{
    private Button _disBtn;
    private Toggle[] _toggles;

    private SettingView _settingView;
    private AnnouncementView _announcementView;
    private ServerView _serverView;
    private FeedbackView _feedbackView;

    private UIBaseView _uiShowView;
    private Transform _root;
    private GameObject _togObj;

    public SettingModule()
        : base(ModuleID.Setting, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_Setting;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();

        _disBtn = Find<Button>("Root/Btn_Back");
        _root = Find<Transform>("Root");
        _togObj = Find("Root/ToggleGroup");

        _settingView = new SettingView();
        _settingView.SetDisplayObject(Find("Root/SettingObj/Setting"));
        _settingView.mDisplayObject.SetActive(false);

        _announcementView = new AnnouncementView();
        _announcementView.SetDisplayObject(Find("Root/SettingObj/Announcement"));

        _serverView = new ServerView();
        _serverView.SetDisplayObject(Find("Root/SettingObj/Server"));

        _feedbackView = new FeedbackView();
        _feedbackView.SetDisplayObject(Find("Root/SettingObj/Feedback"));

        _toggles = new Toggle[4];
        for (int i = 0; i < 4; i++)
            _toggles[i] = Find<Toggle>("Root/ToggleGroup/Tog" + (i + 1));
        foreach (Toggle tog in _toggles)
            tog.onValueChanged.Add((bool blSelect) => { if (blSelect) OnItemTypeChange(tog); });

        _disBtn.onClick.Add(OnClose);

        ColliderHelper.SetButtonCollider(_disBtn.transform);
    }

    private void OnItemTypeChange(Toggle tog)
    {
        if (_uiShowView != null)
            _uiShowView.Hide();
        switch (tog.name)
        {
            case "Tog1":
                _uiShowView = _settingView;
                break;
            case "Tog2":
                _uiShowView = _announcementView;
                break;
            case "Tog3":
                _uiShowView = _serverView;
                break;
            case "Tog4":
                _uiShowView = _feedbackView;
                break;
        }
        _uiShowView.Show();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        if (args.Length == 0 || args == null || bool.Parse(args[0].ToString()))
        {
            _togObj.SetActive(true);
            OnItemTypeChange(_toggles[0]);
            GameNetMgr.Instance.mGameServer.ReqAccountPlayer();
        }
        else
        {
            _togObj.SetActive(false);
            OnItemTypeChange(_toggles[1]);
        }
    }
    public override void Hide()
    {
        base.Hide();
        for (int i = 0; i < _toggles.Length; i++)
        {
            if (i == 0)
                _toggles[i].isOn = true;
            else
                _toggles[i].isOn = false;
        }
        if (_uiShowView != null)
            _uiShowView.Hide();
        _settingView.Hide();
    }

    public override void Dispose()
    {
        if (_settingView != null)
        {
            _settingView.Dispose();
            _settingView = null;
        }
        if (_announcementView != null)
        {
            _announcementView.Dispose();
            _announcementView = null;
        }
        if (_serverView != null)
        {
            _serverView.Dispose();
            _serverView = null;
        }
        if (_feedbackView != null)
        {
            _feedbackView.Dispose();
            _feedbackView = null;
        }
        _uiShowView = null;
        base.Dispose();
    }
    protected override void OnShowAnimator()
    {
        base.OnShowAnimator();
        ObjectHelper.PopAnimationLiner(_root);
    }
}
