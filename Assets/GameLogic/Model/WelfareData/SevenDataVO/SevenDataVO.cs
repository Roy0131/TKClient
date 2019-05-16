using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Msg.ClientMessage;

public class SevenDataVO : DataBaseVO
{
    public int mHeaven { get; private set; }
    public ItemInfo mInfo { get; private set; }
    public int mCurHeaven { get; private set; }
    public int mStatus { get; private set; }
    public int mStartTime { get; private set; }



    protected override void OnInitData<T>(T value)
    {
        S2CSevenDaysDataResponse req = value as S2CSevenDaysDataResponse;
        mStatus = 0;
        mCurHeaven = req.AwardStates.Count;
        mStartTime = (int)Time.realtimeSinceStartup + req.RemainSeconds;
    }

    public int SevenTime
    {
        get { return mStartTime - (int)Time.realtimeSinceStartup; }
    }

    public void OnSevenChang(int heaven,ItemInfo info)
    {
        mHeaven = heaven;
        mInfo = info;
    }

    public void OnSevenStat(int stat)
    {
        mStatus = stat;
    }

    public void OnSevenAward()
    {
        mStatus = 1;
    }
}
