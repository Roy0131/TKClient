using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class BattleModule : ModuleBase
{
    private Text _roundText;
    private Button _speedBtn;
    private Button _skipBtn;
    private Text _speedLabel;
    private GameObject _addSpeedObj;

    private BattleResultView _resultView;
    private BattleInfoView _battleInfoView;
    private GameObject _restrainObject;
    private Button _restrainCloseBtn;
    private Button _restrainInfoBtn;

    private GameObject _anger;
    private Image _angerAmount;

    private Transform _shenqi1;
    private UIEffectView _effect1;
    private GameObject _shenqi2;
    private UIEffectView _effect2;
    private GameObject _shenqi3;
    private UIEffectView _effect3;


    public BattleModule()
        : base(ModuleID.Battle, UILayer.Module)
    {
        _modelResName = UIModuleResName.UI_Battle;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();

        _speedBtn = Find<Button>("TopRight/SpeedBtn");
        _skipBtn = Find<Button>("TopRight/SkipBtn");
        _addSpeedObj = Find("TopRight");
        _speedLabel = Find<Text>("TopRight/Text");
        _roundText = Find<Text>("Round/Text");
        _angerAmount = Find<Image>("Anger/Amount");
        _anger = Find("Anger");

        _restrainInfoBtn = Find<Button>("BtnInf");
        _restrainObject = Find("RestrainObject");
        _restrainCloseBtn = Find<Button>("RestrainObject/ButtonClose");

        _shenqi1 = Find<Transform>("Anger/Amount/fx_ui_sqlodinglv1");
        _effect1 = CreateUIEffect(_shenqi1.gameObject, UILayerSort.ModuleSortBeginner);
        _shenqi2 = Find("Anger/fx_ui_sqlodinglv2");
        _effect2 = CreateUIEffect(_shenqi2, UILayerSort.ModuleSortBeginner);
        _shenqi3 = Find("Anger/fx_ui_sqlodinglv3");
        _effect3 = CreateUIEffect(_shenqi3, UILayerSort.ModuleSortBeginner + 1);

        _resultView = new BattleResultView();
        _resultView.SetDisplayObject(Find("ResultObject"));
        
        _battleInfoView = new BattleInfoView();
        _battleInfoView.SetDisplayObject(Find("BattleInfoRoot"));

        _speedBtn.onClick.Add(OnAddSpeed);
        _skipBtn.onClick.Add(OnSkipBattle);
        _restrainInfoBtn.onClick.Add(OnShowRestrain);
        _restrainCloseBtn.onClick.Add(OnHideRestrain);

        ColliderHelper.SetButtonCollider(_speedBtn.transform);
        ColliderHelper.SetButtonCollider(_restrainInfoBtn.transform);
        ColliderHelper.SetButtonCollider(_restrainCloseBtn.transform);

        _skipBtn.gameObject.SetActive(GameDriver.Instance.mShowDebug);

        NewBieGuide.NewBieGuideMgr.Instance.RegistMaskTransform(NewBieGuide.NewBieMaskID.BattleInfoBtn, _restrainInfoBtn.transform);
        NewBieGuide.NewBieGuideMgr.Instance.RegistMaskTransform(NewBieGuide.NewBieMaskID.RestraintBtn, _restrainCloseBtn.transform);
    }

    private void OnShowRestrain()
    {
        _restrainObject.SetActive(true);
    }

    private void OnHideRestrain()
    {
        _restrainObject.SetActive(false);
    }

    private void OnSkipBattle()
    {
        BattleManager.Instance.SkipBattle();
    }

    private void OnAddSpeed()
    {
        if (GameDriver.Instance.mShowDebug || HeroDataModel.Instance.mHeroInfoData.mVipLevel > 0 || HeroDataModel.Instance.mHeroInfoData.mLevel >= GameConst.AccelerateLevel)
        {
            if (LocalDataMgr.SpeedLevel == 2)
                LocalDataMgr.SpeedLevel = 1;
            else
                LocalDataMgr.SpeedLevel++;
            RefreshTimeScale();
        }
        else
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(3011064));
        }
    }

    private void RefreshTimeScale()
    {
        Time.timeScale = LocalDataMgr.SpeedLevel;
        _speedLabel.text = "x" + LocalDataMgr.SpeedLevel.ToString();
       // BattleManager.Instance.mBattleScene.ChangeTimeScale(LocalDataMgr.SpeedLevel);
    }

    public override void Hide()
    {
        base.Hide();
        _resultView.Hide();
        _addSpeedObj.SetActive(true);
        OnHideRestrain();
    }

    private void OnRoundStart(RoundNodeDataVO data)
    {
        _roundText.text = data.mRoundIndex.ToString();
        float per = data.mSelfArtifStartValue / 60f;
        if (per > 1.01f)
            per = 1f;
        if (per < 1)
        {
            _effect1.PlayEffect();
            _effect2.StopEffect();
            _effect3.StopEffect();
            DGHelper.DoLocalMoveX(_shenqi1, _angerAmount.preferredWidth * per, 0.5f);
        }
        else
        {
            _effect1.StopEffect();
            _effect2.PlayEffect();
        }
        DGHelper.DoImageFillAmount(_angerAmount, per, 0.5f);
    }

    private void OnSelfArtifactEnd()
    {
        float per = BattleManager.Instance.CurRoundDataVO.mSelfArtifEndValue / 60f;
        if (per > 1.01f)
            per = 1f;
        _effect1.StopEffect();
        _effect2.StopEffect();
        _effect3.PlayEffect();
        DGHelper.DoLocalMoveX(_shenqi1, _angerAmount.preferredWidth * per, 0.5f);
        DGHelper.DoImageFillAmount(_angerAmount, per, 0.5f);
    }

    private void OnBattleEnd()
    {
        Time.timeScale = 1;
        _addSpeedObj.SetActive(false);
        _resultView.Show();
    }

    private void OnShowDetailView()
    {
        _resultView.mDisplayObject.SetActive(false);
        _battleInfoView.Show();
    }

    private void OnHideDetailView()
    {
        _resultView.mDisplayObject.SetActive(true);
        _battleInfoView.Hide();
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GameEventMgr.Instance.mBattleDispatcher.AddEvent<RoundNodeDataVO>(BattleEvent.BattleRoundStart, OnRoundStart);
        GameEventMgr.Instance.mBattleDispatcher.AddEvent(BattleEvent.BattleEnd, OnBattleEnd);
        GameEventMgr.Instance.mBattleDispatcher.AddEvent(BattleEvent.ShowBattleDetailView, OnShowDetailView);
        GameEventMgr.Instance.mBattleDispatcher.AddEvent(BattleEvent.HideBattleDetailView, OnHideDetailView);
        GameEventMgr.Instance.mBattleDispatcher.AddEvent(BattleEvent.SelfArtifactActionEnd, OnSelfArtifactEnd);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GameEventMgr.Instance.mBattleDispatcher.RemoveEvent<RoundNodeDataVO>(BattleEvent.BattleRoundStart, OnRoundStart);
        GameEventMgr.Instance.mBattleDispatcher.RemoveEvent(BattleEvent.BattleEnd, OnBattleEnd);
        GameEventMgr.Instance.mBattleDispatcher.RemoveEvent(BattleEvent.ShowBattleDetailView, OnShowDetailView);
        GameEventMgr.Instance.mBattleDispatcher.RemoveEvent(BattleEvent.HideBattleDetailView, OnHideDetailView);
        GameEventMgr.Instance.mBattleDispatcher.RemoveEvent(BattleEvent.SelfArtifactActionEnd, OnSelfArtifactEnd);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _anger.SetActive(LocalDataMgr.GetArtifactSele(LineupSceneMgr.Instance.mLineupTeamType) > 0);
        _angerAmount.fillAmount = 0f;
        _roundText.text = "1";
        RefreshTimeScale();
        _effect1.StopEffect();
        _effect2.StopEffect();
        _effect3.StopEffect();
    }
}