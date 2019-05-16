using Framework.UI;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class BattleResultView : UIBaseView
{
    private UIEffectView _victoryEffect;
    private UIEffectView _failedEffect;
    private SkeletonGraphic _graphic;

    private UIEffectView _victoryParticle;
    private UIEffectView _failedParticle;

    private RewardView _rewardView;
    private RectTransform _winBackGround;
    private RectTransform _loseBackGround;
    private RectTransform _curBackGround;
    private Button _btnEquipUp;
    private RewardLoseView _rewardLoseView;
    protected override void ParseComponent()
    {
        base.ParseComponent();
        _winBackGround = Find<RectTransform>("ResultBG");
        _loseBackGround = Find<RectTransform>("ResultBGLose");
        _victoryEffect = CreateUIEffect(Find("Victory/VictorySpine"), UILayerSort.ModuleSortBeginner + 1, false, SortObjType.Canvas);
        _victoryParticle = CreateUIEffect(Find("Victory/VictoryEffect"), UILayerSort.ModuleSortBeginner);

        _failedEffect = CreateUIEffect(Find("Failed/FailedSpine"), UILayerSort.ModuleSortBeginner + 1, false, SortObjType.Canvas);
        _failedParticle = CreateUIEffect(Find("Failed/FailedEffect"), UILayerSort.ModuleSortBeginner);

        _rewardView = new RewardView();
        _rewardView.SetDisplayObject(Find("RewardRoot"));
        _rewardView.SortingOrder = UILayerSort.ModuleSortBeginner + 2;

        _rewardLoseView = new RewardLoseView();
        _rewardLoseView.SetDisplayObject(Find("LoseGrid"));
        _rewardLoseView.SortingOrder = UILayerSort.ModuleSortBeginner+2;

        _rewardLoseView.Hide();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _winBackGround.gameObject.SetActive(false);
        _loseBackGround.gameObject.SetActive(false);
        SoundMgr.Instance.StopAllBGSound();
        _victoryEffect.StopEffect();
        _failedEffect.StopEffect();
        _victoryParticle.StopEffect();
        _failedParticle.StopEffect();
        if (BattleDataModel.Instance.mBlWin)
        {
            _graphic = _victoryEffect.mDisplayObject.GetComponent<SkeletonGraphic>();
            _victoryEffect.PlayEffect();
            _victoryParticle.PlayEffect();
            SoundMgr.Instance.PlayEffectSound("UI_battle_victory");
            _curBackGround = _winBackGround;
            _rewardLoseView.Hide();
        }
        else
        {
            _failedEffect.PlayEffect();
            _failedParticle.PlayEffect();
            _graphic = _failedEffect.mDisplayObject.GetComponent<SkeletonGraphic>();
            SoundMgr.Instance.PlayEffectSound("UI_battle_failed");
            _curBackGround = _loseBackGround;

            if (BattleDataModel.Instance.mBattleType == BattleType.Campaign||
                BattleDataModel.Instance.mBattleType == BattleType.ActivityCopy||
                BattleDataModel.Instance.mBattleType == BattleType.CTower||
                BattleDataModel.Instance.mBattleType == BattleType.ExploreStoryTask||
                BattleDataModel.Instance.mBattleType == BattleType.ExploreTask)
                DelayCall(0.5f, () => { _rewardLoseView.Show(); });
            else
                _rewardLoseView.Hide();
        }
        _curBackGround.gameObject.SetActive(true);
        _curBackGround.localScale = Vector3.zero;
        //_curBackGround.DOScale(Vector3.one, 0.2f);
        DGHelper.DoScale(_curBackGround, Vector3.one, 0.2f);
        _graphic.AnimationState.Complete += OnEnd;
        PlayAnimation();
        _rewardView.DisableExitButton();
        DelayCall(0.5f, RewardAnimationEnd);
    }

    private void RewardAnimationEnd()
    {
        _rewardView.Show();
        GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.EnterCondTrigger, NewBieGuide.EnterCondConst.BattleRewardShow);
    }

    private void OnEnd(TrackEntry obj)
    {
        if (obj.Animation.Name == "animation")
            _graphic.AnimationState.SetAnimation(0, "animation2", true);
    }

    private void PlayAnimation()
    {
        _graphic.AnimationState.SetAnimation(0, "animation", false);
    }

    public override void Hide()
    {
        if (_graphic != null)
            _graphic.AnimationState.Complete -= OnEnd;
        base.Hide();
    }

    public override void Dispose()
    {
        if (_graphic != null)
            _graphic.AnimationState.Complete -= OnEnd;
        if (_victoryEffect != null)
        {
            _victoryEffect.Dispose();
            _victoryEffect = null;
        }
        if (_victoryParticle != null)
        {
            _victoryParticle.Dispose();
            _victoryParticle = null;
        }

        if (_failedEffect != null)
        {
            _failedEffect.Dispose();
            _failedEffect = null;
        }
        if (_failedParticle != null)
        {
            _failedParticle.Dispose();
            _failedParticle = null;
        }
        if (_rewardView != null)
        {
            _rewardView.Dispose();
            _rewardView = null;
        }
        _graphic = null;
        base.Dispose();
    }
}