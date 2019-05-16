using Msg.ClientMessage;

public class ArenaModule : ModuleBase
{
    private ArenaMatchView _matchView;
    private ArenaShopView _arenaShopView;
    private ArenaRecordView _recordView;
    private ArenaRewardView _rewardView;
    private bool _blOpenRecord = false;
    public ArenaModule()
        : base(ModuleID.Arena, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_Arena;
        _soundName = UIModuleSoundName.AranaSoundName;
        mBlNeedBackMask = true;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();

        ArenaInfoView infoView = new ArenaInfoView();
        infoView.SetDisplayObject(Find("InfoRoot"));
        AddChildren(infoView);

        ArenaUserView userView = new ArenaUserView();
        userView.SetDisplayObject(Find("UsersRoot"));
        AddChildren(userView);

        _matchView = new ArenaMatchView();
        _matchView.SortingOrder = UILayerSort.WindowSortBeginner + 2;
        _matchView.SetDisplayObject(Find("MatchWindow"));

        _arenaShopView = new ArenaShopView();
        _arenaShopView.SetDisplayObject(Find("BuyTicketWindow"));
        _arenaShopView.SortingOrder = UILayerSort.WindowSortBeginner + 2;

        _recordView = new ArenaRecordView();
        _recordView.SetDisplayObject(Find("BattleRecordWindow"));
        _recordView.SortingOrder = UILayerSort.WindowSortBeginner + 2;

        _rewardView = new ArenaRewardView();
        _rewardView.SetDisplayObject(Find("RewardWindow"));
        _rewardView.SortingOrder = UILayerSort.WindowSortBeginner + 2;
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.EndCondTrigger, NewBieGuide.EndConditionConst.ArenaModuleOpen);
    }

    public override void Hide()
    {
        base.Hide();
        _matchView.Hide();
        _arenaShopView.Hide();
        _rewardView.Hide();
        _arenaShopView.Hide();

        StopAllEffectSound();
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        ArenaDataModel.Instance.AddEvent<S2CArenaMatchPlayerResponse>(ArenaEvent.AreanMatchPlayerBack, OnShowMatchPlayer);
        ShopDataModel.Instance.AddEvent<int>(ShopEvent.ShopData, OnShowBuyTicket);
        ArenaDataModel.Instance.AddEvent(ArenaEvent.ArenaRecordListBack, OnShowRecord);
        ArenaDataModel.Instance.AddEvent(ArenaEvent.ArenaRecordDataBack, OnHideRecord);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(ArenaEvent.HideArenaPlayerInfo, OnHidePlayerInfo);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(ArenaEvent.ArenaRewardShow, OnShowReward);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        ArenaDataModel.Instance.RemoveEvent<S2CArenaMatchPlayerResponse>(ArenaEvent.AreanMatchPlayerBack, OnShowMatchPlayer);
        ShopDataModel.Instance.RemoveEvent<int>(ShopEvent.ShopData, OnShowBuyTicket);
        ArenaDataModel.Instance.RemoveEvent(ArenaEvent.ArenaRecordListBack, OnShowRecord);
        ArenaDataModel.Instance.RemoveEvent(ArenaEvent.ArenaRecordDataBack, OnHideRecord);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(ArenaEvent.HideArenaPlayerInfo, OnHidePlayerInfo);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(ArenaEvent.ArenaRewardShow, OnShowReward);
    }

    private void OnShowReward()
    {
        _rewardView.Show();
    }

    private void OnHidePlayerInfo()
    {
        if (_blOpenRecord)
            OnShowRecord();
        _blOpenRecord = false;
    }

    private void OnShowRecord()
    {
        _recordView.Show();
    }

    private void OnHideRecord()
    {
        _recordView.Hide();
    }

    private void OnShowBuyTicket(int shopId)
    {
        if (shopId != ShopIdConst.ISLANDSHOP)
            return;
        ShopDataVO vo = ShopDataModel.Instance.GetShopDataByShopId(ShopIdConst.ISLANDSHOP);
        ShopItemDataVO itemVO = null;
        for (int i = 0; i < vo.mListItemVO.Count; i++)
        {
            if (vo.mListItemVO[i].mBuyNum > 0)
            {
                itemVO = vo.mListItemVO[i];
                break;
            }
        }
        if (itemVO == null)
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001111));
            return;
        }
        _arenaShopView.Show(itemVO);
    }

    private void OnShowMatchPlayer(S2CArenaMatchPlayerResponse value)
    {
        _matchView.Show(value);
        GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.EndCondTrigger, NewBieGuide.EndConditionConst.ArenaMatchResult);
    }

    public override void Dispose()
    {
        if (_matchView != null)
        {
            _matchView.Dispose();
            _matchView = null;
        }
        if (_arenaShopView != null)
        {
            _arenaShopView.Dispose();
            _arenaShopView = null;
        }
        if (_recordView != null)
        {
            _recordView.Dispose();
            _recordView = null;
        }
        if (_rewardView != null)
        {
            _rewardView.Dispose();
            _rewardView = null;
        }
        base.Dispose();
    }
}