using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;

public class GuildBossDataModel : ModelDataBase<GuildBossDataModel>
{
    private const string BossData = "bossData";
    private GuildCurBossHurtVO mCurBossHurtVO = new GuildCurBossHurtVO();
    private Dictionary<int, List<GuildBossHurtVO>> _dictBossHurt = new Dictionary<int, List<GuildBossHurtVO>>();
    private Dictionary<int, int> _dictHurt = new Dictionary<int, int>();
    public int stageId { get; private set; }

    public void RefershStageId(int value)
    {
        stageId = value;
    }

    public void ReqBossData()
    {
        if (CheckNeedRequest(BossData))
            GameNetMgr.Instance.mGameServer.ReqStageData();
        else
            Instance.DispathEvent(GuildEvent.ReqBossResult, mCurBossHurtVO);
    }

    private void OnBossDataResult(S2CGuildStageDataResponse value)
    {
        mCurBossHurtVO.InitData(value);
        AddLastReqTime(BossData);
        Instance.DispathEvent(GuildEvent.ReqBossResult, mCurBossHurtVO);
    }

    #region parse guild boss reward
    struct GuildBossRewardData
    {
        int min, max, itemCfgId, itemCount;
        public string rewardValue;
        public GuildBossRewardData InitData(string value, string rValue)
        {
            string[] tmp = value.Split(',');
            min = int.Parse(tmp[0]);
            max = int.Parse(tmp[1]);
            rewardValue = rValue;

            tmp = rValue.Split(',');
            itemCfgId = int.Parse(tmp[0]);
            itemCount = int.Parse(tmp[1]);

            return this;
        }

        public bool CheckInRange(int rank)
        {
            return rank >= min && rank <= max;
        }

        public ItemInfo GetRewardInfo()
        {
            ItemInfo info = new ItemInfo();
            info.Value = itemCount;
            info.Id = itemCfgId;
            return info;
        }

        public int GetMin()
        {
            return min;
        }

        public int GetMax()
        {
            return max;
        }
    }
    #endregion

    public ItemInfo GetRankReward(int rank)
    {
        for (int i = 0; i < _lstRewardData.Count; i++)
        {
            if (_lstRewardData[i].CheckInRange(rank))
                return _lstRewardData[i].GetRewardInfo();
        }
        return null;
    }

    public List<ItemInfo> GetListRankReward(int bossId)
    {
        List<ItemInfo> listItemInfo = new List<ItemInfo>();
        for (int i = 0; i < _lstRewardData.Count; i++)
            listItemInfo.Add(_lstRewardData[i].GetRewardInfo());
        return listItemInfo;
    }

    public List<int> GetListMin(int bossId)
    {
        List<int> listMin = new List<int>();
        for (int i = 0; i < _lstRewardData.Count; i++)
            listMin.Add(_lstRewardData[i].GetMin());
        return listMin;
    }

    public List<int> GetListMax(int bossId)
    {
        List<int> listMax = new List<int>();
        for (int i = 0; i < _lstRewardData.Count; i++)
            listMax.Add(_lstRewardData[i].GetMax());
        return listMax;
    }

    private void ParseRewardData(int bossID)
    {
        if (_lstRewardData == null)
            _lstRewardData = new List<GuildBossRewardData>();
        _lstRewardData.Clear();
        GuildBossConfig config = GameConfigMgr.Instance.GetGuildBossConfig(bossID);
        _lstRewardData.Add(new GuildBossRewardData().InitData(config.RankReward1Cond, config.RankReward1));
        _lstRewardData.Add(new GuildBossRewardData().InitData(config.RankReward2Cond, config.RankReward2));
        _lstRewardData.Add(new GuildBossRewardData().InitData(config.RankReward3Cond, config.RankReward3));
        _lstRewardData.Add(new GuildBossRewardData().InitData(config.RankReward4Cond, config.RankReward4));
        _lstRewardData.Add(new GuildBossRewardData().InitData(config.RankReward5Cond, config.RankReward5));
    }

    private List<GuildBossRewardData> _lstRewardData;
    private void OoBossRankResult(S2CGuildStageRankListResponse value)
    {
        List<GuildBossHurtVO> lst = new List<GuildBossHurtVO>();
        GuildBossHurtVO vo;
        ParseRewardData(value.BossId);
        for (int i = 0; i < value.DmgList.Count; i++)
        {
            vo = new GuildBossHurtVO();
            vo.InitData(value.DmgList[i]);
            lst.Add(vo);
        }
        if (lst.Count > 0)
        {
            if (_dictBossHurt.ContainsKey(value.BossId))
                _dictBossHurt[value.BossId] = lst;
            else
                _dictBossHurt.Add(value.BossId, lst);
        }
        if (value.DmgList.Count > 0)
        {
            if (_dictHurt.ContainsKey(value.BossId))
                _dictHurt[value.BossId] = value.DmgList[0].Damage;
            else
                _dictHurt.Add(value.BossId, value.DmgList[0].Damage);
        }
        Instance.DispathEvent(GuildEvent.ReqBossDmg, value.BossId);
    }

    private void OoBossPlayerRespawn(S2CGuildStagePlayerRespawnResponse value)
    {
        mCurBossHurtVO.OnRefresh(value.RemainRespawnNum);
        Instance.DispathEvent(GuildEvent.ResPawn);
    }

    private void OoBossReset(S2CGuildStageResetResponse value)
    {
        _dictBossHurt.Clear();
        _dictHurt.Clear();
        GameNetMgr.Instance.mGameServer.ReqStageData();
    }

    private void OoResetRemain(S2CGuildStageResetNotify value)
    {
        mCurBossHurtVO.OnResetTime(value.NextResetRemainSeconds);
    }

    private void OoRefreshRemain(S2CGuildStageAutoRefreshNotify value)
    {
        GameNetMgr.Instance.mGameServer.ReqStageData();
    }

    public int HurtBossId(int bossId)
    {
        if (_dictHurt.ContainsKey(bossId))
            return _dictHurt[bossId];
        return 0;
    }

    public List<GuildBossHurtVO> HurtVO(int bossId)
    {
        if (_dictBossHurt.ContainsKey(bossId))
            return _dictBossHurt[bossId];
        return null;
    }

    public static void DoBossDataResult(S2CGuildStageDataResponse value)
    {
        Instance.OnBossDataResult(value);
    }

    public static void DoBossRankResult(S2CGuildStageRankListResponse value)
    {
        Instance.OoBossRankResult(value);
    }

    public static void DoBossPlayerRespawn(S2CGuildStagePlayerRespawnResponse value)
    {
        Instance.OoBossPlayerRespawn(value);
    }

    public static void DoBossReset(S2CGuildStageResetResponse value)
    {
        Instance.OoBossReset(value);
    }

    public static void DoResetRemain(S2CGuildStageResetNotify value)
    {
        Instance.OoResetRemain(value);
    }

    public static void DoRefreshRemain(S2CGuildStageAutoRefreshNotify value)
    {
        Instance.OoRefreshRemain(value);
    }

    protected override void DoClearData()
    {
        base.DoClearData();
        if (_dictBossHurt != null)
            _dictBossHurt.Clear();
        if (_dictHurt != null)
            _dictHurt.Clear();
    }
}