public class MemberMgrModule : ModuleBase
{
    private GuildAskJoinView _askJoinView;
    private MemberRecruitView _recruitView;
    public MemberMgrModule()
        : base(ModuleID.MemberMgr, UILayer.Popup)
    {
        _modelResName = UIModuleResName.UI_MemberMgr;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        MemberView memberView = new MemberView();
        memberView.SetDisplayObject(Find("MemeberView"));
        AddChildren(memberView);

        _askJoinView = new GuildAskJoinView();
        _askJoinView.SetDisplayObject(Find("AskListView"));

        _recruitView = new MemberRecruitView();
        _recruitView.SetDisplayObject(Find("RecruitView"));
    }

    private void OnShowAskJoinView()
    {
        _askJoinView.Show();
    }

    private void OnShowRecruitView()
    {
        _recruitView.Show();
    }

    private void OnHideRecruitView()
    {
        _recruitView.Hide();
        PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000082));
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GameEventMgr.Instance.mGlobalDispatcher.AddEvent(GuildEvent.ShowAskJoinMemberView, OnShowAskJoinView);
        GameEventMgr.Instance.mGlobalDispatcher.AddEvent(GuildEvent.ShowRecruitView, OnShowRecruitView);
        GuildDataModel.Instance.AddEvent(GuildEvent.GuildRecruitSendBack, OnHideRecruitView);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GameEventMgr.Instance.mGlobalDispatcher.RemoveEvent(GuildEvent.ShowAskJoinMemberView, OnShowAskJoinView);
        GameEventMgr.Instance.mGlobalDispatcher.RemoveEvent(GuildEvent.ShowRecruitView, OnShowRecruitView);
        GuildDataModel.Instance.RemoveEvent(GuildEvent.GuildRecruitSendBack, OnHideRecruitView);
    }

    public override void Dispose()
    {
        if (_askJoinView != null)
        {
            _askJoinView.Dispose();
            _askJoinView = null;
        }
        if (_recruitView != null)
        {
            _recruitView.Dispose();
            _recruitView = null;
        }
        base.Dispose();
    }
}