using UnityEngine;

public class HomeStage : BaseStage
{
    private GameObject _sceneRootObject;
    public GameObject mTowerRolePreview { get; private set; }

    uint times;
    public HomeStage()
        : base(StageType.Home)
    {
        _sceneName = "HomeScene";
    }

    protected override void OnEnter()
    {
        if (LanternMgr.Instance._lanternView != null)
            LanternMgr.Instance._lanternView.OnShow();
        base.OnEnter();
        LoadingMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001267));
    }

    protected override void LoadSceneFinish()
    {
        base.LoadSceneFinish();
        _sceneRootObject = GameObject.Find("HomeRoot");
        RoleRTMgr.Instance.InitRoleRTMode(_sceneRootObject.transform.Find("RoleRTObject").gameObject);

        HangUpMgr.Instance.Init();
        LineupSceneMgr.Instance.Init();

        MainMapMgr.Instance.Init(_sceneRootObject.transform.Find("mainmap").gameObject);

        //if (MainMapMgr.Instance != null)
        //    MainMapMgr.Instance.enabled = false;
        MainMapMgr.Instance.Enable = false;

        GameUIMgr.Instance.OpenModule(ModuleID.Home);
        SoundMgr.Instance.PlayBackGroundSound("Music_ZC");
        DoBattleType(LineupSceneMgr.Instance.mBattleType);
        LineupSceneMgr.Instance.mBattleType = BattleType.None;
        if (times != 0)
            TimerHeap.DelTimer(times);
        times = TimerHeap.AddTimer(1000, 0, OnTime);
    }

    private void OnTime()
    {
        LoadingMgr.Instance.CloseLoading();
    }

    private void DoBattleType(BattleType type)
    {
        switch (type)
        {
            case global::BattleType.None:
                break;
            case global::BattleType.Pvp:
                ArenaDataModel.Instance.ReqArenaData();
                break;
            case global::BattleType.Campaign:
                HangupDataModel.Instance.ReqCampaignData();
                break;
            case global::BattleType.CTower:
                CTowerDataModel.Instance.ReqTowerData();
                break;
            case global::BattleType.ActivityCopy:
                GameUIMgr.Instance.OpenModule(ModuleID.ActivityCopy, true);
                break;
            case global::BattleType.FriendBoss:
                GameUIMgr.Instance.OpenModule(ModuleID.Friend);
                break;
            case global::BattleType.ExploreTask:
                GameUIMgr.Instance.OpenModule(ModuleID.Explore, false);
                break;
            case global::BattleType.ExploreStoryTask:
                GameUIMgr.Instance.OpenModule(ModuleID.Explore, true);
                break;
            case global::BattleType.FriendBattle:
                GameUIMgr.Instance.OpenModule(ModuleID.Friend);
                break;
            case global::BattleType.GuildBoss:
                GameUIMgr.Instance.OpenModule(ModuleID.HeroGuild);
                break;
            case global::BattleType.Expedition:
                GameUIMgr.Instance.OpenModule(ModuleID.Expedition);
                break;
        }
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(BattleEvent.FightJump, type);
        GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.EnterCondTrigger, NewBieGuide.EnterCondConst.BattleBackHome);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        HangUpMgr.Instance.Update();
        MainMapMgr.Instance.Update();
    }

    protected override void OnExit()
    {
        base.OnExit();
        RoleRTMgr.Instance.Hide();
        MainMapMgr.Instance.UnBindFuncRedPoint();
        HangUpMgr.Instance.Dispose();
        LineupSceneMgr.Instance.Dispose();
        RoleRTMgr.Instance.Dispose();
        MainMapMgr.Instance.Dispose();
		_sceneRootObject = null;
        mTowerRolePreview = null;
        if (LanternMgr.Instance._lanternView != null)
            LanternMgr.Instance._lanternView.OnHide();
    }
}