using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class FriendModule : ModuleBase
{
    private FriendListView _friendListView;
    private RecommemdFriendView _recommendView;
    private FriendAskListView _askFriendView;
    private FriendAssistView _assistView;
    private FriendBossView _friendBossView;

    private Text _titleText;
    private UIFriendType _type;
    private UIBaseView _uiShowView;
    private Button _closeBtn;
    private GameObject _imgBack2Obj;
    private Transform _root;

    private Toggle[] _toggles;
    private Button _but;

    public FriendModule()
        : base(ModuleID.Friend, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_Friend;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _type = UIFriendType.None;
        _titleText = Find<Text>("Root/Content/TextTitle");
        _imgBack2Obj = Find("Root/Content/ImageBack02");

        _toggles = new Toggle[4];
        for (int i = 0; i < 4; i++)
            _toggles[i] = Find<Toggle>("Root/ToggleGroup/Tog" + (i + 1));
        foreach (Toggle tog in _toggles)
            tog.onValueChanged.Add((bool blSelect) => { if (blSelect) OnToggleChange(tog); });

        _friendListView = new FriendListView();
        _friendListView.SetDisplayObject(Find("Root/Content/FriendListView"));

        _recommendView = new RecommemdFriendView();
        _recommendView.SetDisplayObject(Find("Root/Content/RecommendView"));

        _askFriendView = new FriendAskListView();
        _askFriendView.SetDisplayObject(Find("Root/Content/AskFriendListView"));

        _assistView = new FriendAssistView();
        _assistView.SetDisplayObject(Find("Root/Content/FriendAssistView"));

        _friendBossView = new FriendBossView();
        _friendBossView.SetDisplayObject(Find("Root/BossFightView"));

        _but = Find<Button>("Root/But");
        _but.onClick.Add(OnBut);
        _closeBtn = Find<Button>("Root/Content/CloseBtn");
        _closeBtn.onClick.Add(OnClose);
        _root = Find<Transform>("Root");
        RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.FriendAssit, Find("Root/ToggleGroup/Tog4/RedPoint"));
        RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.FriendApply, Find("Root/ToggleGroup/Tog3/RedPoint"));
        
        ColliderHelper.SetButtonCollider(_closeBtn.transform);
    }

    private void OnBut()
    {
        PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001123, GameConst.GetFeatureType(FunctionType.FriendBoss)));
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<string>(FriendEvent.RefreshFriendTitle, OnRefreshTitle);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<int>(FriendEvent.ChallengeFriendBoss, OnChallengeFriendBoss);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<string>(FriendEvent.RefreshFriendTitle, OnRefreshTitle);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<int>(FriendEvent.ChallengeFriendBoss, OnChallengeFriendBoss);
    }

    private void OnChallengeFriendBoss(int playerId)
    {
        _friendBossView.Show(playerId);
    }

    private void OnRefreshTitle(string content)
    {
        _titleText.text = content;
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _but.gameObject.SetActive(!FunctionUnlock.IsUnlock(FunctionType.FriendBoss, true));
        OnToggleChange(_toggles[(int)UIFriendType.FriendList - 1]);
    }

    private void OnToggleChange(Toggle tog)
    {
        if (_uiShowView != null)
            _uiShowView.Hide();
        switch (tog.name)
        {
            case "Tog1":
                _type = UIFriendType.FriendList;
                _uiShowView = _friendListView;
                FriendDataModel.Instance.ReqFriendList();
                break;
            case "Tog2":
                _type = UIFriendType.RecommemdFriend;
                _uiShowView = _recommendView;
                FriendDataModel.Instance.ReqRecommemdFriend();
                OnRefreshTitle(LanguageMgr.GetLanguage(5001538));
                break;
            case "Tog3":
                _type = UIFriendType.FriendAskList;
                FriendDataModel.Instance.ReqFriendAskPlayerList();
                _uiShowView = _askFriendView;
                RedPointDataModel.Instance.SetRedPointDataState(RedPointEnum.FriendApply, false);
                OnRefreshTitle(LanguageMgr.GetLanguage(5001514));
                break;
            case "Tog4":
                _type = UIFriendType.FriendBoss;
                FriendDataModel.Instance.ReqFriendAssistData();
                _uiShowView = _assistView;
                OnRefreshTitle(LanguageMgr.GetLanguage(5001539));
                break;
        }
        _imgBack2Obj.SetActive(_type != UIFriendType.FriendBoss);
        _uiShowView.Show();
    }

    public override void Hide()
    {
        _toggles[(int)UIFriendType.FriendList - 1].isOn = true;
        base.Hide();
        if (_uiShowView != null)
            _uiShowView.Hide();
        _friendBossView.Hide();
    }

    public override void Dispose()
    {
        if (_friendListView != null)
        {
            _friendListView.Dispose();
            _friendListView = null;
        }

        if (_recommendView != null)
        {
            _recommendView.Dispose();
            _recommendView = null;
        }

        if (_askFriendView != null)
        {
            _askFriendView.Dispose();
            _askFriendView = null;
        }

        if (_assistView != null)
        {
            _assistView.Dispose();
            _assistView = null;
        }

        if (_friendBossView != null)
        {
            _friendBossView.Dispose();
            _friendBossView = null;
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