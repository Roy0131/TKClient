using Msg.ClientMessage;
using System.Collections.Generic;

public enum ArenaRewardType
{
    None,
    DailyReward,
    SeasonReward,
    RankReward,
}

public class ArenaDataModel : ModelDataBase<ArenaDataModel>
{
    private const string _arenaDataKey = "arenaDataKey";
    private const string _arenaRecordKey = "arenaRecordKey";

    public ArenaDataVO mArenaDataVO { get; private set; } = new ArenaDataVO();
    public List<RankItemInfo> mlstRankItem { get; private set; } = new List<RankItemInfo>();
    public List<BattleRecordData> mlstRecordData { get; private set; } = new List<BattleRecordData>();

    public void ReqArenaData()
    {
        if (CheckNeedRequest(_arenaDataKey))
            GameNetMgr.Instance.mGameServer.ReqArenaData();
        else
            GameUIMgr.Instance.OpenModule(ModuleID.Arena);
    }

    public void ReqArenaRecordData()
    {
        if (CheckNeedRequest(_arenaRecordKey))
            GameNetMgr.Instance.mGameServer.ReqBattleRecord();
        else
            Instance.DispathEvent(ArenaEvent.ArenaRecordListBack);
    }

    public void ReqMatchPlayer()
    {
        GameNetMgr.Instance.mGameServer.ReqMatchPlayer();
    }

    public ArenaRewardVO GetSelfDailyRewardVO(ArenaRewardType type)
    {
        List<ArenaRewardVO> lst = GetArenaReward(type);

        for (int i = 0; i < lst.Count; i++)
        {
            if (lst[i].InRange(mArenaDataVO.mRank))
                return lst[i];
        }
        return null;
    }

    private Dictionary<ArenaRewardType, List<ArenaRewardVO>> _dictRewards;
    public List<ArenaRewardVO> GetArenaReward(ArenaRewardType type)
    {
        if (_dictRewards == null)
            ParseArenaReward();
        return _dictRewards[type];
    }

    private void ParseArenaReward()
    {
        _dictRewards = new Dictionary<ArenaRewardType, List<ArenaRewardVO>>();
        Dictionary<int, ArenaRankingBonusConfig>.ValueCollection dict = ArenaRankingBonusConfig.Get().Values;
        List<ArenaRewardVO> lstDailyRewards = new List<ArenaRewardVO>();
        List<ArenaRewardVO> lstSeasonRewards = new List<ArenaRewardVO>();
        List<ArenaRewardVO> lstRankRewards = new List<ArenaRewardVO>();
        ArenaRewardVO vo;
        foreach (ArenaRankingBonusConfig cfg in dict)
        {
            vo = new ArenaRewardVO(ArenaRewardType.DailyReward);
            vo.InitData(cfg);
            lstDailyRewards.Add(vo);

            vo = new ArenaRewardVO(ArenaRewardType.SeasonReward);
            vo.InitData(cfg);
            lstSeasonRewards.Add(vo);
        }

        Dictionary<int, ArenaDivisionConfig>.ValueCollection dict1 = ArenaDivisionConfig.Get().Values;
        foreach (ArenaDivisionConfig cfg in dict1)
        {
            if (cfg.Division == 1)
                continue;
            vo = new ArenaRewardVO(ArenaRewardType.RankReward);
            vo.InitData(cfg);
            lstRankRewards.Add(vo);
        }
        lstDailyRewards.Sort(SortArenaReward);
        lstSeasonRewards.Sort(SortArenaReward);
        lstRankRewards.Sort(SortArenaReward);
        _dictRewards.Add(ArenaRewardType.DailyReward, lstDailyRewards);
        _dictRewards.Add(ArenaRewardType.SeasonReward, lstSeasonRewards);
        _dictRewards.Add(ArenaRewardType.RankReward, lstRankRewards);
    }

    private int SortArenaReward(ArenaRewardVO a, ArenaRewardVO b)
    {
        return a.RewardIndex > b.RewardIndex ? 1 : -1;
    }


    #region netmsg recive method
    public static void DoArenaData(S2CArenaDataResponse value)
    {
        Instance.AddLastReqTime(_arenaDataKey);
        Instance.mArenaDataVO.InitData(value);
        GameUIMgr.Instance.OpenModule(ModuleID.Arena);
        GameNetMgr.Instance.mGameServer.ReqRankData(1);
    }

    public static void DoMatchPlayer(S2CArenaMatchPlayerResponse value)
    {
        BattleDataModel.Instance.mArenaPlayer = value;
        Instance.DispathEvent(ArenaEvent.AreanMatchPlayerBack, value);
    }

    public static void DoRankListData(S2CRankListResponse value)
    {
        Instance.mlstRankItem.Clear();
        Instance.mlstRankItem.AddRange(value.RankItems);
        Instance.mlstRankItem.Sort(SortRankData);
        Instance.DispathEvent(ArenaEvent.AreanRankRefresh);
    }

    private static int SortRankData(RankItemInfo a, RankItemInfo b)
    {
        return a.Rank > b.Rank ? 1 : -1;
    }

    public static void DoBattleRecordData(S2CBattleRecordListResponse value)
    {
        Instance.mlstRecordData.Clear();
        Instance.mlstRecordData.AddRange(value.Records);
        Instance.AddLastReqTime(_arenaRecordKey);
        Instance.DispathEvent(ArenaEvent.ArenaRecordListBack);
    }

    public static void DoArenaRecordResponse(S2CBattleRecordResponse value)
    {
        Instance.DispathEvent(ArenaEvent.ArenaRecordDataBack);
        S2CBattleResultResponse recordData = S2CBattleResultResponse.Parser.ParseFrom(value.RecordData);
        BattleDataModel.DoBattleResult(recordData);
        if (value.AttackerId != HeroDataModel.Instance.mHeroPlayerId)
            BattleDataModel.Instance.mBlWin = !BattleDataModel.Instance.mBlWin;
        BattleDataModel.Instance.mBlRecord = true;
    }
    #endregion

    protected override void DoClearData()
    {
        base.DoClearData();
        if (mlstRankItem != null)
            mlstRankItem.Clear();
        if (mlstRecordData != null)
            mlstRecordData.Clear();
        if (_dictRewards != null)
            _dictRewards.Clear();
    }
}