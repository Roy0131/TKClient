using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class LineupModule : ModuleBase
{
    private Button _closeButton;
    private Transform _root;
    private Transform _root1;

    private Button _lineupStatuBtn;
    private Button _battleStatuBtn;
    private GameObject _statusBtnsObj;
    private Text _selectText;
    private Button _artifactSelect;

    private LineupCardView _cardView;
    private LineupFighterView _fighterView;
    private bool _blLineupStatus = true;
    private RawImage _battleImage;

    private ArtifactSeleView _artifactSeleView;
    private GameObject _speciallyObj;
    private UIEffectView _effect;


    public LineupModule()
         : base(ModuleID.Lineup, UILayer.Popup)
    {
        _modelResName = UIModuleResName.UI_Lineup;
        mBlNeedBackMask = true;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _closeButton = Find<Button>("ButtonClose");
        _root = Find<Transform>("HeroCardView");
        _selectText = Find<Text>("ArtifactSelectBtn/Text");
        _artifactSelect = Find<Button>("ArtifactSelectBtn");

        _statusBtnsObj = Find("StatusBtn");
        _lineupStatuBtn = Find<Button>("StatusBtn/LineStatusBtn");
        _battleStatuBtn = Find<Button>("StatusBtn/BattleStatusBtn");
        _battleImage = Find<RawImage>("BattleRawImage");

        _speciallyObj = Find("ArtifactSelectBtn/fx_ui_daditu");
        _effect = CreateUIEffect(_speciallyObj, UILayerSort.PopupSortBeginner);

        _cardView = new LineupCardView();
        _cardView.SetDisplayObject(_root.gameObject);
        AddChildren(_cardView);

        _fighterView = new LineupFighterView();
        _fighterView.SetDisplayObject(Find("FighterView"));
        AddChildren(_fighterView);

        _artifactSeleView = new ArtifactSeleView();
        _artifactSeleView.SetDisplayObject(Find("ArtifactSelect"));
        _artifactSeleView.SortingOrder = UILayerSort.PopupSortBeginner + 1;
        _root1 = Find<Transform>("ArtifactSelect/Img");

        _closeButton.onClick.Add(OnClose);
        _artifactSelect.onClick.Add(OnArtifactSelect);
        _lineupStatuBtn.onClick.Add(OnShowLineup);
        _battleStatuBtn.onClick.Add(OnShowBattle);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _artifactSelect.gameObject.SetActive(LineupSceneMgr.Instance.mLineupTeamType != TeamType.FriendBossAssist);
        if (FunctionUnlock.IsUnlock(FunctionType.Artifact, true))
        {
            if (LocalDataMgr.GetArtifactSele(LineupSceneMgr.Instance.mLineupTeamType) == 0)
            {
                _selectText.text = LanguageMgr.GetLanguage(400013);
                _effect.PlayEffect();
            }
            else
            {
                _selectText.text = LanguageMgr.GetLanguage(400014);
                _effect.StopEffect();
            }
        }
        else
        {
            _selectText.text = LanguageMgr.GetLanguage(400013);
            _effect.StopEffect();
        }
    }

    private void OnArtifactSelect()
    {
        if (FunctionUnlock.IsUnlock(FunctionType.Artifact))
        {
            _artifactSeleView.Show();
            ObjectHelper.PopAnimationLiner(_root1);
        }
    }

    private void OnArtifactEffect()
    {
        if (LocalDataMgr.GetArtifactSele(LineupSceneMgr.Instance.mLineupTeamType) == 0)
        {
            _selectText.text = LanguageMgr.GetLanguage(400013);
            _effect.PlayEffect();
        }
        else
        {
            _selectText.text = LanguageMgr.GetLanguage(400014);
            _effect.StopEffect();
        }
    }

    private void OnShowLineup()
    {
        //_cardView.mRectTransform.DOAnchorPosX(-952f, 0.2f).SetEase(Ease.Linear).onComplete = MoveActionEnd;
        DGHelper.DoAnchorPosX(_cardView.mRectTransform, -952f, 0.2f, 0, MoveActionEnd);
        _lineupStatuBtn.interactable = false;
    }

    private void MoveActionEnd()
    {
        _blLineupStatus = !_blLineupStatus;
        _lineupStatuBtn.interactable = true;
        _battleStatuBtn.interactable = true;
        _lineupStatuBtn.gameObject.SetActive(_blLineupStatus);
        _battleStatuBtn.gameObject.SetActive(!_blLineupStatus);
        _closeButton.gameObject.SetActive(_blLineupStatus);
        if (!_blLineupStatus)
        {
            _cardView.Hide();
            _fighterView.Hide();
            _artifactSelect.gameObject.SetActive(false);
            _battleImage.gameObject.SetActive(true);
            LineupSceneMgr.Instance.ShowBattleStatus();
            _battleImage.texture = RoleRTMgr.Instance.GetRoleRTImage();
        }
        else
        {
            LineupSceneMgr.Instance.ShowLineupStatus();
            _artifactSelect.gameObject.SetActive(true);
            _battleImage.gameObject.SetActive(false);
            _fighterView.Show();
        }
    }

    private void OnShowBattle()
    {
        _cardView.Show();
        //_cardView.mRectTransform.DOAnchorPosX(-384f, 0.2f).SetEase(Ease.Linear).onComplete = MoveActionEnd;
        DGHelper.DoAnchorPosX(_cardView.mRectTransform, -384f, 0.2f, 0, MoveActionEnd);
        _battleStatuBtn.interactable = false;
    }

    public override void Hide()
    {
        base.Hide();
        LineupSceneMgr.Instance.HideLineModule();
    }

    protected override void OnClose()
    {
        base.OnClose();
        if (LineupSceneMgr.Instance.mBattleType == BattleType.Campaign || LineupSceneMgr.Instance.mBattleType == BattleType.ExploreTask
            || LineupSceneMgr.Instance.mBattleType == BattleType.ExploreStoryTask)
            HangUpMgr.Instance.ContineBattle();
    }

    protected override void OnShowAnimator()
    {
        base.OnShowAnimator();
        _cardView.mRectTransform.anchoredPosition = new Vector2(-384f, 0f);
        _blLineupStatus = true;
        ObjectHelper.AnimationMoveBack(_root,ObjectHelper.direction.right);
        if(LineupSceneMgr.Instance.mBattleType == BattleType.Campaign || LineupSceneMgr.Instance.mBattleType == BattleType.ExploreTask
           || LineupSceneMgr.Instance.mBattleType == BattleType.ExploreStoryTask)
            HangUpMgr.Instance.PasueBattle();
        if (LineupSceneMgr.Instance.mBattleType == BattleType.Campaign && !NewBieGuide.NewBieGuideMgr.Instance.mBlGuideForce)
        {
            _lineupStatuBtn.gameObject.SetActive(_blLineupStatus);
            _battleStatuBtn.gameObject.SetActive(!_blLineupStatus);
            _closeButton.gameObject.SetActive(_blLineupStatus);
            _statusBtnsObj.SetActive(true);
        }
        else
        {
            _statusBtnsObj.SetActive(false);
        }
        DelayCall(0.5f, () =>
        {
            GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.EnterCondTrigger, NewBieGuide.EnterCondConst.LineUpShow);
        });
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GameEventMgr.Instance.mGuideDispatcher.AddEvent<bool>(GuideEvent.LineupButtonStatusChange, OnRefreshBtnStatus);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(ArtifactEvent.ArtifactEffect, OnArtifactEffect);
    }

    private void OnRefreshBtnStatus(bool blEnable)
    {
        _closeButton.enabled = blEnable;
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GameEventMgr.Instance.mGuideDispatcher.RemoveEvent<bool>(GuideEvent.LineupButtonStatusChange, OnRefreshBtnStatus);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(ArtifactEvent.ArtifactEffect, OnArtifactEffect);
    }
}