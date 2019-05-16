public class BattleStage : BaseStage
{
    private bool _blPreloading = false;
    public BattleStage()
        : base(StageType.Battle)
    {
        _sceneName = "BattleScene";
    }

    protected override void OnEnter()
    {
        LoadingMgr.Instance.ShowTips("Initialize battle...");
        base.OnEnter();
        GameEventMgr.Instance.mGlobalDispatcher.DispathEvent(GameEventMgr.GEventEnterBattle);
    }

    protected override void LoadSceneFinish()
    {
        ResPoolMgr.Instance.Init();
        BattleManager.Instance.Init();
        BulletMgr.Instance.Init(); //must be initialize battlemgr first
        EffectMgr.Instance.Init();
        ResPoolMgr.Instance.PrepareBattleRes();

        _blPreloading = true;
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(UIEventDefines.CloseConsoleModule);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (_blLoadingScene)
            return;
        if(_blPreloading)
        {
            _blPreloading = ResPoolMgr.Instance.PreloadEffect();
            if (!_blPreloading)
            {
                GameUIMgr.Instance.OpenModule(ModuleID.Battle);
                GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.EnterCondTrigger, NewBieGuide.EnterCondConst.EnterBattleScene);
                LoadingMgr.Instance.CloseLoading();
            }
        }else
        {
            BattleManager.Instance.Update();
            BulletMgr.Instance.Update();
            EffectMgr.Instance.Update();
        }
    }

    protected override void OnExit()
    {
        base.OnExit();
        BattleManager.Instance.Dispose();
        BulletMgr.Instance.Dispose();
        ResPoolMgr.Instance.Dispose();
        GameUIMgr.Instance.CloseModule(ModuleID.Battle);
        UnityEngine.Resources.UnloadUnusedAssets();
        BattleDataModel.Instance.DisposeBattleData();
        System.GC.Collect();
    }
}
