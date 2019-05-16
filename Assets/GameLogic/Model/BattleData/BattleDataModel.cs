using Msg.ClientMessage;
using System.Collections.Generic;

public enum BattleType
{
    None = 0,
    Pvp = 1,//竞技场
    Campaign,//战役
    CTower,//爬塔
    ActivityCopy,//活动副本
    FriendBoss,//好友Boss
    ExploreTask,//探索任务
    ExploreStoryTask,//剧情任务
    FriendBattle,//好友切磋
    GuildBoss,//公会Boss
    Expedition,//远征
}

public class BattleDataModel : ModelDataBase<BattleDataModel>
{
    public bool mBlWin { get; set; }

    public List<FighterDataVO> mlstHeroFighterDatas;
    public List<FighterDataVO> mlstTargetFighterDatas;
    public RoundNodeDataVO mEnterRound;
    public List<RoundNodeDataVO> mlstActionRounds;

    public List<FighterStatisticVO> mlstHeroStatistDatas { get; private set; }
    public List<FighterStatisticVO> mlstTargetStatistDatas { get; private set; }

    public IList<ItemInfo> mBattleRewards { get; private set; } //战斗奖励
    public IList<ItemInfo> mRandomRewards { get; private set; } //竞技场与好友Boss奖励辅助道具

    private int _heroSideInServer;//主角在服务器的站位, 0 左边   1 右边
    public int mHeroTotalDamage { get; private set; } = 0;
    public int mHeroTotalHeal { get; private set; } = 0;

    public int mTargetTotalDamage { get; private set; } = 0;
    public int mTargetTotalHeal { get; private set; } = 0;
    public BattleType mBattleType { get; private set; }
    public int mBattleParam { get; private set; }
    public int mBattleExtParam { get; private set; }

    public int mSelfArtifactId { get; private set; }
    public int mTargetArtifactId { get; private set; }

    public S2CArenaMatchPlayerResponse mArenaPlayer;
    public S2CArenaScoreNotify mArenaScore;
    public bool mBlRecord;


    public bool IsHeroFighter(int side)
    {
        return _heroSideInServer == side;
    }

    private bool _blSweep = false;

    #region parse battle report
    private void OnBattleReport(S2CBattleResultResponse value)
    {
        GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.EndCondTrigger, NewBieGuide.EndConditionConst.BattleRequestBack);
        //value.BattleType
        mBattleType = (BattleType)value.BattleType;
        mBattleParam = value.BattleParam;
        mBattleExtParam = value.ExtraValue;

        mBlWin = value.IsWin;
        LineupSceneMgr.Instance.SaveBattleTeam();
        if (_blSweep)
        {
            DispathEvent(BattleEvent.BattleSweepEnd);
            return;
        }
        #region 解析战斗数据
        mlstHeroFighterDatas = new List<FighterDataVO>();
        mlstTargetFighterDatas = new List<FighterDataVO>();

        int i = 0;
        if (value.MyTeam != null)
            _heroSideInServer = ParseFighterData(value.MyTeam, mlstHeroFighterDatas);
        if (value.TargetTeam != null)
            ParseFighterData(value.TargetTeam, mlstTargetFighterDatas);
        if (value.EnterReports != null && value.EnterReports.Count > 0)
        {
            mEnterRound = new RoundNodeDataVO();
            mEnterRound.InitData(value.EnterReports);
        }
        if (value.Rounds != null && value.Rounds.Count > 0)
        {
            mlstActionRounds = new List<RoundNodeDataVO>();
            RoundNodeDataVO node;
            for (i = 0; i < value.Rounds.Count; i++)
            {
                node = new RoundNodeDataVO();
                node.SetRoundNodeData(value.Rounds[i]);
                mlstActionRounds.Add(node);
            }
        }

        mlstHeroStatistDatas = new List<FighterStatisticVO>();
        mlstTargetStatistDatas = new List<FighterStatisticVO>();

        FighterStatisticVO vo;
        int damage, heal;
        int seatIndex;
        for (i = 0; i < mlstHeroFighterDatas.Count; i++)
        {
            vo = new FighterStatisticVO(true);
            damage = 0;
            heal = 0;
            seatIndex = mlstHeroFighterDatas[i].mSeatIndex;
            if(seatIndex < value.MyMemberDamages.Count)
                damage = value.MyMemberDamages[seatIndex];
            if (seatIndex < value.MyMemberCures.Count)
                heal = -value.MyMemberCures[seatIndex];
            mHeroTotalDamage += damage;
            mHeroTotalHeal += heal;
            vo.InitData(damage, heal, mlstHeroFighterDatas[i].mLevel, mlstHeroFighterDatas[i].mCardConfig.ClientID);
            mlstHeroStatistDatas.Add(vo);
        }

        for (i = 0; i < mlstTargetFighterDatas.Count; i++)
        {
            vo = new FighterStatisticVO();
            damage = 0;
            heal = 0;
            seatIndex = mlstTargetFighterDatas[i].mSeatIndex;
            if (seatIndex < value.TargetMemberDamages.Count)
                damage = value.TargetMemberDamages[seatIndex];
            if (seatIndex < value.TargetMemberCures.Count)
                heal = -value.TargetMemberCures[seatIndex];
            mTargetTotalDamage += damage;
            mTargetTotalHeal += heal;
            vo.InitData(damage, heal, mlstTargetFighterDatas[i].mLevel, mlstTargetFighterDatas[i].mCardConfig.ClientID);
            mlstTargetStatistDatas.Add(vo);
        }

        if (LineupSceneMgr.Instance.mLineupTeamType == TeamType.Expedition && mBlWin)
            mBattleRewards = ExpeditionDataModel.Instance.mExoeditionDataVO.mListInfo;
        mSelfArtifactId = value.MyArtifactId;
        mTargetArtifactId = value.TargetArtifactId;
        #endregion
        GameStageMgr.Instance.ChangeStage(StageType.Battle);
    }

    private int ParseFighterData(IList<BattleMemberItem> value, List<FighterDataVO> lst)
    {
        FighterDataVO vo;
        int side = -1;
        for (int i = 0; i < value.Count; i++)
        {
            vo = new FighterDataVO();
            vo.InitData(value[i]);
            lst.Add(vo);
            if (side < 0)
                side = vo.mSide;
        }
        return side;
    }
    #endregion

    #region battle reward
    public void SetBattleReward(S2CCampaignHangupIncomeResponse value)
    {
        if (_blSweep)
        {
            GetItemTipMgr.Instance.ShowItemResult(value.Rewards);
            return;
        }
        mBattleRewards = value.Rewards;
    }
    #endregion

    #region request start battle
    public void ReqStartBattle(BattleType battleType, int battleParam, int sweepNum = 0, int assistFriendId = 0, int assistRoleId = 0, int assistPos = 0, int artifactId = 0)
    {
        C2SBattleResultRequest req = new C2SBattleResultRequest();
        req.BattleType = (int)battleType;
        req.BattleParam = battleParam;
        if (LineupSceneMgr.Instance.TeamMembers != null)
            req.AttackMembers.AddRange(LineupSceneMgr.Instance.TeamMembers);
        if (sweepNum > 0)
            req.SweepNum = sweepNum;
        _blSweep = sweepNum > 0;
        if (assistFriendId > 0)
        {
            req.AssistFriendId = assistFriendId;
            req.AssistRoleId = assistRoleId;
            req.AssistPos = assistPos;
        }
        req.AritfactId = artifactId;
        GameNetMgr.Instance.mGameServer.ReqBattle(req);
    }
    #endregion

    public void DisposeBattleData()
    {
        int i;
        if (mlstHeroFighterDatas != null)
        {
            for (i = mlstHeroFighterDatas.Count - 1; i >= 0; i--)
                mlstHeroFighterDatas[i].Dispose();
            mlstHeroFighterDatas.Clear();
            mlstHeroFighterDatas = null;
        }
        if (mlstTargetFighterDatas != null)
        {
            for (i = mlstTargetFighterDatas.Count - 1; i >= 0; i--)
                mlstTargetFighterDatas[i].Dispose();
            mlstTargetFighterDatas.Clear();
            mlstTargetFighterDatas = null;
        }
        if (mEnterRound != null)
        {
            mEnterRound.Dispose();
            mEnterRound = null;
        }
        if (mlstActionRounds != null)
        {
            for (i = mlstActionRounds.Count - 1; i >= 0; i--)
                mlstActionRounds[i].Dispose();
            mlstActionRounds.Clear();
            mlstActionRounds = null;
        }
        mHeroTotalDamage = 0;
        mHeroTotalHeal = 0;
        mTargetTotalDamage = 0;
        mTargetTotalHeal = 0;
        mBattleRewards = null;
        mRandomRewards = null;
        mArenaScore = null;
        mBlRecord = false;
        mArenaPlayer = null;
    }

    #region receive data proccess method (static method)
    public static void DoBattleResult(S2CBattleResultResponse result)
    {
        Instance.OnBattleReport(result);
    }

    public static void DoBattleRandomReward(S2CBattleRandomRewardNotify value)
    {
        if (Instance._blSweep)
        {
            GetItemTipMgr.Instance.ShowItemResult(value.Items);
        }
        else
        {
            Instance.mBattleRewards = value.Items;
            Instance.mRandomRewards = new List<ItemInfo>();
            ItemInfo info;
            for (int i = 0; i < value.FakeItems.Count; i++)
            {
                info = new ItemInfo();
                info.Id = value.FakeItems[i];
                info.Value = 0;
                Instance.mRandomRewards.Add(info);
            }
        }
    }

    public static void DoArenaBattleResult(S2CArenaScoreNotify value)
    {
        Instance.mArenaScore = value;
    }
    #endregion

    protected override void DoClearData()
    {
        base.DoClearData();
        if (mlstHeroFighterDatas != null)
            mlstHeroFighterDatas.Clear();
        if (mlstTargetFighterDatas != null)
            mlstTargetFighterDatas.Clear();
        if (mlstActionRounds != null)
            mlstActionRounds.Clear();
        if (mlstHeroStatistDatas != null)
            mlstHeroStatistDatas.Clear();
        if (mlstTargetStatistDatas != null)
            mlstTargetStatistDatas.Clear();
    }
}