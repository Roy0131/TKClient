using Msg.ClientMessage;
using UnityEngine;

public class ArenaDataVO : DataBaseVO
{
    public int mScore { get; private set; }
    public int mGrade { get; private set; }
    public int mRank { get; private set; }
    public int mTopRank { get; private set; }

    private float _flSeasonRemainEndTime;
    private float _flDayRemainEndTime;
    
    protected override void OnInitData<T>(T value)
    {
        S2CArenaDataResponse data = value as S2CArenaDataResponse;
        mScore = data.Score;
        mGrade = data.Grade;
        mRank = data.Rank;
        mTopRank = data.TopRank;
        _flSeasonRemainEndTime = Time.realtimeSinceStartup + data.SeasonRemainSeconds;
        _flDayRemainEndTime = Time.realtimeSinceStartup + data.DayRemainSeconds;
        //Debuger.LogWarning("[ArenaDataVO.OnInitData() => dayRemainTime:" + _flDayRemainEndTime + ", seasonRemainTime:" + _flSeasonRemainEndTime + "]");
    }

    public int DayRemainTime
    {
        get { return (int)(_flDayRemainEndTime - Time.realtimeSinceStartup); }
    }

    public int SeasonRemainTime
    {
        get { return (int)(_flSeasonRemainEndTime - Time.realtimeSinceStartup); }
    }
}