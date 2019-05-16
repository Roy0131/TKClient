using UnityEngine.UI;
using System.Collections.Generic;
using Framework.UI;

public class ArenaInfoView : UIBaseView
{
    private Button _closeBtn;
    private Button _rewardBtn;
    private Button _recordBtn;
    private Button _helpBtn;
    private Button _defenseBtn;
    private Button _addTicketsBtn;
    private Button _BattleBtn;

    private Text _endTimeText;
    private Text _ticketsCountText;
    private Text _userLevelText;
    private Text _battlePowerText;
    private Text _scoreText;
    private Text _rankNameText;
    private Text _rankLevelText;

    private Image _rankIcon;
    private Image _userIcon;
    private int _seasonRemainTime;
    private uint _timerKey = 0;

    protected override void ParseComponent()
    {
        base.ParseComponent();

        _endTimeText = Find<Text>("EndTimeText");
        _ticketsCountText = Find<Text>("TicketsCountText");
        _userLevelText = Find<Text>("HeroInfo/LevelText");
        _battlePowerText = Find<Text>("BattlePower/Text");
        _scoreText = Find<Text>("ArenaScore/Text");
        _rankNameText = Find<Text>("RankInfo/RankNameText");
        _rankLevelText = Find<Text>("RankInfo/RankLevelText");

        _rankIcon = Find<Image>("RankInfo/RankIcon");
        _userIcon = Find<Image>("HeroInfo");

        _closeBtn = Find<Button>("Buttons/BtnBack");
        ColliderHelper.SetButtonCollider(_closeBtn.transform);
        _rewardBtn = Find<Button>("Buttons/BtnReward");
        _recordBtn = Find<Button>("Buttons/BtnRecord");
        _helpBtn = Find<Button>("Buttons/BtnHelp");
        ColliderHelper.SetButtonCollider(_helpBtn.transform);
        _defenseBtn = Find<Button>("Buttons/BtnDefensive");
        _addTicketsBtn = Find<Button>("Buttons/TicketsAddBtn");
        _BattleBtn = Find<Button>("Buttons/BattleBtn");

        _closeBtn.onClick.Add(OnClose);
        _addTicketsBtn.onClick.Add(OnAddTickets);
        _BattleBtn.onClick.Add(OnMatchPlayer);
        _rewardBtn.onClick.Add(OnShowReward);
        _recordBtn.onClick.Add(OnShowRecord);
        _defenseBtn.onClick.Add(OnShowDefense);
        _helpBtn.onClick.Add(OnShowHelp);

        NewBieGuide.NewBieGuideMgr.Instance.RegistMaskTransform(NewBieGuide.NewBieMaskID.ArenaDefenseBtn, _defenseBtn.transform);
        NewBieGuide.NewBieGuideMgr.Instance.RegistMaskTransform(NewBieGuide.NewBieMaskID.ArenaMatchBtn, _BattleBtn.transform);
        NewBieGuide.NewBieGuideMgr.Instance.RegistMaskTransform(NewBieGuide.NewBieMaskID.ArenaDisBtn, _closeBtn.transform);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        BagDataModel.Instance.AddEvent<List<int>>(BagEvent.BagItemRefresh, OnItemChange);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        BagDataModel.Instance.RemoveEvent<List<int>>(BagEvent.BagItemRefresh, OnItemChange);
    }

    private void OnItemChange(List<int> value)
    {
        if (value.Contains(SpecialItemID.Arena_Ticket))
            OnRefreshTickets();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        OnRefreshTickets();
        _battlePowerText.text = HeroDataModel.Instance.GetBattlePowerByTeamType(TeamType.Arena).ToString();
        ArenaDataVO vo = ArenaDataModel.Instance.mArenaDataVO;
        _rankLevelText.text = vo.mRank.ToString();
        ArenaDivisionConfig config = GameConfigMgr.Instance.GetArenaDivisionConfig(vo.mGrade);
        if (config != null)
            _rankNameText.text = LanguageMgr.GetLanguage(config.Name);
        _userLevelText.text = HeroDataModel.Instance.mHeroInfoData.mLevel.ToString();
        _battlePowerText.text = HeroDataModel.Instance.GetBattlePowerByTeamType(TeamType.Arena).ToString();
        _scoreText.text = vo.mScore.ToString();
        _seasonRemainTime = vo.SeasonRemainTime;
        if (_seasonRemainTime > 0)
            _timerKey = TimerHeap.AddTimer(0, 1000, OnSeasonCD);
        if (HeroDataModel.Instance.mHeroInfoData.mIcon > 0)
            _userIcon.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(HeroDataModel.Instance.mHeroInfoData.mIcon).Icon);
        _rankIcon.sprite = GameResMgr.Instance.LoadItemIcon(config.Icon);
    }

    private void OnSeasonCD()
    {
        if (_seasonRemainTime <= 0)
        {
            ClearRemainTimer();
            return;
        }
        _seasonRemainTime--;
        _endTimeText.text = TimeHelper.GetCountTime(_seasonRemainTime);
    }

    private void ClearRemainTimer()
    {
        if (_timerKey != 0)
            TimerHeap.DelTimer(_timerKey);
        _timerKey = 0;
    }

    public override void Hide()
    {
        base.Hide();
        ClearRemainTimer();
    }

    private void OnRefreshTickets()
    {
        int tickCount = BagDataModel.Instance.GetItemCountById(SpecialItemID.Arena_Ticket);
        _ticketsCountText.text = tickCount.ToString();
        _addTicketsBtn.gameObject.SetActive(tickCount == 0);
    }

    private void OnMatchPlayer()
    {
        int tickCount = BagDataModel.Instance.GetItemCountById(SpecialItemID.Arena_Ticket);
        if (tickCount == 0)
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001108));
            return;
        }
        ArenaDataModel.Instance.ReqMatchPlayer();
    }

    private void OnAddTickets()
    {
        ShopDataModel.Instance.ReqShopData(ShopIdConst.ISLANDSHOP);
    }

    private void OnShowReward()
    {
        if (ArenaDataModel.Instance.mArenaDataVO.mRank == 0)
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001109));
            return;
        }
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(ArenaEvent.ArenaRewardShow);
    }

    private void OnShowRecord()
    {
        ArenaDataModel.Instance.ReqArenaRecordData();
    }

    private void OnShowDefense()
    {
        LineupSceneMgr.Instance.ShowLineupModule(TeamType.Defense);
    }

    private void OnShowHelp()
    {
        HelpTipsMgr.Instance.ShowTIps(HelpType.ArenaHelp);
    }

    private void OnClose()
    {
        GameUIMgr.Instance.CloseModule(ModuleID.Arena);
    }
}