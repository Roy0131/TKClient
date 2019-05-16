using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Msg.ClientMessage;
using Framework.UI;
using System;

public class RewardView : UIBaseView
{
    private GameObject _pvpRewardObject;
    private Button _pvpRewardBGBtn;
    private List<RectTransform> _lstPvpRewardSlot;
    private List<Button> _lstPvpBtn;

    private Button _exitBtn;
    private Text _resultText;
    private Button _infoBtn;

    #region battle player info
    private GameObject _pvpPlayerRoot;

    private Text _heroName;
    private Text _heroLevel;
    private Text _heroScoreText;
    private Text _heroScoreAddText;
    private Image _heroIcon;

    private Text _targetName;
    private Text _targetLevel;
    private Text _targetScoreText;
    private Text _targetScoreAddText;
    private Image _targetIcon;
    #endregion

    private RectTransform _rewardRoot;

    private List<UIEffectView> _lstEffect1;
    private List<UIEffectView> _lstEffect2;

    protected override void ParseComponent()
    {
        base.ParseComponent();

        _rewardRoot = Find<RectTransform>("RewardGrid");
        _pvpRewardObject = Find("PvpReward");
        _pvpRewardBGBtn = Find<Button>("PvpReward");
        _exitBtn = Find<Button>("ButtonConfirm");
        _resultText = Find<Text>("TextResult");
        _infoBtn = Find<Button>("InfoBtn");

        _pvpPlayerRoot = Find("PvpPlayerRoot");

        _heroName = Find<Text>("PvpPlayerRoot/HeroIcon/TextName");
        _heroLevel = Find<Text>("PvpPlayerRoot/HeroIcon/ImageBack/Level");
        _heroScoreText = Find<Text>("PvpPlayerRoot/HeroIcon/TextScore");
        _heroScoreAddText = Find<Text>("PvpPlayerRoot/HeroIcon/TextScoreAdd");
        _heroIcon = Find<Image>("PvpPlayerRoot/HeroIcon/ImageBack/heroDeta");

        _targetName = Find<Text>("PvpPlayerRoot/TargetIcon/TextName");
        _targetLevel = Find<Text>("PvpPlayerRoot/TargetIcon/ImageBack/Level");
        _targetScoreText = Find<Text>("PvpPlayerRoot/TargetIcon/TextScore");
        _targetScoreAddText = Find<Text>("PvpPlayerRoot/TargetIcon/TextScoreAdd");
        _targetIcon = Find<Image>("PvpPlayerRoot/TargetIcon/ImageBack/heroDeta");

        _lstPvpRewardSlot = new List<RectTransform>();
        Button btn;
        _lstPvpBtn = new List<Button>();
        _lstEffect1 = new List<UIEffectView>();
        _lstEffect2 = new List<UIEffectView>();
        UIEffectView eff;
        for (int i = 0; i < 3; i++)
        {
            _lstPvpRewardSlot.Add(Find<RectTransform>("PvpReward/SlotItem" + i));
            btn = Find<Button>("PvpReward/SlotItem" + i);
            eff = CreateUIEffect(Find("PvpReward/SlotItem" + i + "/fx_ui_jiangli01"), UILayerSort.ModuleSortBeginner + 3);
            _lstEffect1.Add(eff);
            eff = CreateUIEffect(Find("PvpReward/SlotItem" + i + "/fx_ui_jiangli02"), UILayerSort.ModuleSortBeginner + 3);
            _lstEffect2.Add(eff);
            _lstPvpBtn.Add(btn);
            int tmpIdx = i;
            btn.onClick.Add(() => { ShowReward(_lstPvpBtn[tmpIdx]); });
        }

        _pvpRewardBGBtn.onClick.Add(OnClosePvpReward);
        _exitBtn.onClick.Add(OnExit);
        _infoBtn.onClick.Add(OnShowBattleInfo);

        NewBieGuide.NewBieGuideMgr.Instance.RegistMaskTransform(NewBieGuide.NewBieMaskID.BattleExitBtn, _exitBtn.transform);
    }

    private void ShowReward(Button btn)
    {
        for (int i = 0; i < 3; i++)
        {
            _lstEffect1[i].StopEffect();
            _lstEffect2[i].PlayEffect();
            _lstPvpBtn[i].interactable = false;
        }

        Action OnShowReward = () =>
        {
            int idx = _lstPvpBtn.IndexOf(btn);
            IList<ItemInfo> lstRewards = BattleDataModel.Instance.mRandomRewards;
            lstRewards.Insert(idx, BattleDataModel.Instance.mBattleRewards[0]);
            int i = 0;
            for (i = 0; i < _lstPvpRewardSlot.Count; i++)
            {
                ItemView view = ItemFactory.Instance.CreateItemView(lstRewards[i], ItemViewType.RewardItem);
                view.mRectTransform.SetParent(_lstPvpRewardSlot[i], false);
                _lstPvpBtn[i].interactable = false;
                _lstEffect2[i].StopEffect();
                
                AddChildren(view);
            }
            _exitBtn.enabled = true;
        };

        DelayCall(0.5f, OnShowReward);
    }

    private void OnClosePvpReward()
    {
        if (_childrenViews.Count == 0)
            return;
        _pvpRewardObject.SetActive(false);
        GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.EndCondTrigger, NewBieGuide.EndConditionConst.ArenaCardRewardEnd);
    }

    public void DisableExitButton()
    {
        if (!BattleDataModel.Instance.mBlRecord)
            _exitBtn.enabled = false;
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        DiposeChildren();
        if (BattleDataModel.Instance.mBattleType == BattleType.Pvp)
        {
            if(BattleDataModel.Instance.mBlRecord)
            {
                _pvpPlayerRoot.SetActive(false);
                _pvpRewardObject.SetActive(false);
                _exitBtn.enabled = true;
            }
            else
            {
                _pvpPlayerRoot.SetActive(true);
                _pvpRewardObject.SetActive(true);

                S2CArenaScoreNotify mArenaScore = BattleDataModel.Instance.mArenaScore;
                _heroName.text = HeroDataModel.Instance.mHeroInfoData.mHeroName;
                _heroLevel.text = "Lv" + HeroDataModel.Instance.mHeroInfoData.mLevel.ToString();
                _heroScoreText.text = mArenaScore.SelfScore.ToString();
                _heroScoreAddText.text = "(+" + mArenaScore.AddScore + ")";
                if (HeroDataModel.Instance.mHeroInfoData.mIcon > 0)
                {
                    _heroIcon.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(HeroDataModel.Instance.mHeroInfoData.mIcon).Icon);
                    ObjectHelper.SetSprite(_heroIcon, _heroIcon.sprite);
                }
                else
                    _heroIcon.sprite = null;

                S2CArenaMatchPlayerResponse mArenaPlayer = BattleDataModel.Instance.mArenaPlayer;
                _targetName.text = mArenaPlayer.PlayerName;
                _targetLevel.text = "Lv" + mArenaPlayer.PlayerLevel.ToString();
                _targetScoreText.text = mArenaScore.TargetScore.ToString();
                _targetScoreAddText.text = "(+" + mArenaScore.TargetAddScore + ")";
                if (mArenaPlayer.PlayerHead > 0)
                {
                    _targetIcon.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(mArenaPlayer.PlayerHead).Icon);
                    ObjectHelper.SetSprite(_targetIcon,_targetIcon.sprite);
                }
                else
                    _targetIcon.sprite = null;

                for (int i = 0; i < _lstPvpBtn.Count; i++)
                {
                    _lstPvpBtn[i].interactable = true;
                    _lstEffect1[i].PlayEffect();
                    _lstEffect2[i].StopEffect();
                }    
                _exitBtn.enabled = false;
            }
        }
        else
        {
            _exitBtn.enabled = true;
            _pvpRewardObject.SetActive(false);
            _pvpPlayerRoot.SetActive(false);
            IList<ItemInfo> lstRewards = BattleDataModel.Instance.mBattleRewards;
            if (lstRewards == null)
                return;
            ItemView view;
            for (int i = 0; i < lstRewards.Count; i++)
            {
                view = ItemFactory.Instance.CreateItemView(lstRewards[i], ItemViewType.BagItem);
                view.mRectTransform.SetParent(_rewardRoot, false);
                AddChildren(view);
            }
        }
    }

    private void OnExit()
    {
        DiposeChildren(); 
        _pvpPlayerRoot.SetActive(false);
        _pvpRewardObject.SetActive(false);
        GameStageMgr.Instance.ChangeStage(StageType.Home);
    }

    private void OnShowBattleInfo()
    {
        GameEventMgr.Instance.mBattleDispatcher.DispathEvent(BattleEvent.ShowBattleDetailView);
    }

    protected override void DiposeChildren()
    {
        for (int i = _childrenViews.Count - 1; i >= 0; i--)
            ItemFactory.Instance.ReturnItemView(_childrenViews[i] as ItemView);
        _childrenViews.Clear();
    }
}
