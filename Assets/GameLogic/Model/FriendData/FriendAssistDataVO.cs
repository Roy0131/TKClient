using Msg.ClientMessage;

public class FriendBossDataVO
{
    public int mBossHpPercent { get; private set; }
    public int mDiamondReward { get; private set; }
    public CardDataVO mBossCardVO { get; private set; }
    public int mBossConfigID { get; private set; }
    public void InitBossData(int id, int hpPercent = 100)
    {
        mBossConfigID = id;
        FriendBossConfig config = GameConfigMgr.Instance.GetFriendBossConfig(id);
        if (config == null)
        {
            LogHelper.LogWarning("[FriendBossDataVO.InitBossData() => Boss id:" + id + " config not found!!!]");
            return;
        }
        mBossCardVO = new CardDataVO(config.BossIDShow, config.BossRankShow, config.BossLevelShow);
        mBossHpPercent = hpPercent;

        string[] itemList = config.RewardOwner.Split(',');
        if (itemList.Length % 2 != 0)
            return;
        for (int i = 0; i < itemList.Length; i += 2)
            mDiamondReward = int.Parse(itemList[i + 1]);
    }

    public void RefreshBossHp(int hpPercent)
    {
        mBossHpPercent = hpPercent;
    }
}

public class FriendAssistDataVO : DataBaseVO
{
    public FriendBossDataVO mBossDataVO { get; private set; }

    public int mAssistPointsReward { get; private set; }
    public int mAssistRoleId { get; private set; }
    public int mTotaGetPoint { get; private set; }
    public int mTotaGiveGetPoint { get; private set; }


    private int _nextRefreshBossTime;
    private int _nextRefreshStrengthTime;

    public int mStrengthCostTime { get; private set; }
    protected override void OnInitData<T>(T value)
    {
        S2CFriendDataResponse data = value as S2CFriendDataResponse;
        mAssistPointsReward = data.AssistGetPoints;

        RefreshAssistCardId(data.AssistRoleId);
        RefreshBossData(data.BossId, data.BossHpPercent);
        _nextRefreshBossTime = (int)UnityEngine.Time.realtimeSinceStartup + data.SearchBossRemainSeconds;
        mStrengthCostTime = data.StaminaResumeOneCostTime * 60 * 60;
        if (data.RemainSecondsNextStamina > 0)
            _nextRefreshStrengthTime = (int)UnityEngine.Time.realtimeSinceStartup + data.RemainSecondsNextStamina + 2;
        mTotaGetPoint = data.TotalAssistGetPoints;
        mTotaGiveGetPoint = data.TotalFriendGiveGetPoints;
    }

    public int SearchBossRemainTime
    {
        get { return _nextRefreshBossTime - (int)UnityEngine.Time.realtimeSinceStartup; }
    }

    public int NextStrengthAddTime
    {
        get { return _nextRefreshStrengthTime - (int)UnityEngine.Time.realtimeSinceStartup; }
    }

    public void RefreshBossData(int id, int hpPercent = 100)
    {
        if (id != 0)
        {
            if (mBossDataVO == null)
                mBossDataVO = new FriendBossDataVO();
            mBossDataVO.InitBossData(id, hpPercent);
        }
        else
        {
            mBossDataVO = null;
        }
        _nextRefreshBossTime = (int)UnityEngine.Time.realtimeSinceStartup + GameConst.FriendBossCDTime;
    }

    public void RefreshAssistPoint(int value)
    {
        mAssistPointsReward = mAssistPointsReward - value < 0 ? 0 : mAssistPointsReward - value;
    }

    public void RefreshAssistCardId(int cardId)
    {
        mAssistRoleId = cardId;
        LocalDataMgr.FriendAssistCardId = cardId;
    }

    public void RefreshBossHp(int hp)
    {
        if (hp == 0)
            mBossDataVO = null;
        else
            mBossDataVO.RefreshBossHp(hp);
    }

    public void OnGetPoint(int num)
    {
        mTotaGetPoint += num;
    }

    public void OnGiveGetPoint(int num)
    {
        mTotaGiveGetPoint += num;
    }
}