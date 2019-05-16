using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Msg.ClientMessage;

public class GuildCurBossHurtVO : DataBaseVO
{
    public int mCurBossId { get; private set; }
    public int mHpPercent { get; private set; }
    public int mRespawnNum { get; private set; }
    public int mTotalRespawnNum { get; private set; }
    public int mRefreshTime { get; private set; }
    public int mStageState { get; private set; }
    public List<int> mListRespawnNeedCost { get; private set; }
    public int mResetTime { get; private set; }

    protected override void OnInitData<T>(T value)
    {
        if (mListRespawnNeedCost==null)
            mListRespawnNeedCost = new List<int>();
        mListRespawnNeedCost.Clear();
        S2CGuildStageDataResponse req = value as S2CGuildStageDataResponse;
        mCurBossId = req.CurrBossId;
        mHpPercent = req.HpPercent;
        mRespawnNum = req.RespawnNum;
        mTotalRespawnNum = req.TotalRespawnNum;
        mRefreshTime = (int)Time.realtimeSinceStartup + req.RefreshRemainSeconds;
        mStageState = req.StageState;
        mListRespawnNeedCost.AddRange(req.RespawnNeedCost);
        mResetTime = req.CanResetRemainSeconds + (int)Time.realtimeSinceStartup;
    }

    public int ResetTime
    {
        get { return mResetTime - (int)Time.realtimeSinceStartup; }
    }

    public int RefreshTime
    {
        get { return mRefreshTime - (int)Time.realtimeSinceStartup; }
    }

    public void OnRefresh(int resPawnNum)
    {
        mRespawnNum = mTotalRespawnNum - resPawnNum;
        mStageState = 0;
    }

    public void OnResetTime(int time)
    {
        mResetTime = time + (int)Time.realtimeSinceStartup;
    }
}
