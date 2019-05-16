using Msg.ClientMessage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventConst
{
    public const int Recharge = 301;//充值
    public const int Exchange = 302;//兑换
    public const int Rebate = 303;//充值返利
    public const int GetHero = 304;//获得英雄
    public const int Consume = 305;//消耗钻石
    public const int Explore = 306;//探索
    public const int DrawCard = 307;//抽卡
    public const int Arena = 308;//竞技场
}


public class LimitedDataVO : DataBaseVO
{
    public int mActivityId { get; private set; }
    public int mActivityTime { get; private set; }
    public List<LimitedItemDataVO> mListLimitedItemDataVO { get; private set; }

    protected override void OnInitData<T>(T value)
    {
        ActivityData activityData = value as ActivityData;
        mActivityId = activityData.Id;
        mActivityTime = (int)Time.realtimeSinceStartup + activityData.RemainSeconds;
        MainActiveConfig cfg = GameConfigMgr.Instance.GetMainActiveConfig(mActivityId);
        string[] activeList = cfg.SubActiveList.Split(',');
        if (mListLimitedItemDataVO != null)
            mListLimitedItemDataVO.Clear();
        mListLimitedItemDataVO = new List<LimitedItemDataVO>();
        for (int i = 0; i < activeList.Length; i++)
        {
            if (activeList[i] == "")
                return;
            LimitedItemDataVO vo = new LimitedItemDataVO();
            vo.OnSubActiveId(int.Parse(activeList[i].ToString()));
            if (activityData.SubDatas != null && activityData.SubDatas.Count > 0)//&& activityData.SubDatas.Count > i)
            {
                for (int j = 0; j < activityData.SubDatas.Count; j++)
                {
                    if (int.Parse(activeList[i]) == activityData.SubDatas[j].SubId)
                        vo.OnValue(activityData.SubDatas[j].Value);
                }
            }
            else
            {
                vo.OnValue(0);
            }
            mListLimitedItemDataVO.Add(vo);
        }
    }

    public int ActivityTime
    {
        get { return mActivityTime - (int)Time.realtimeSinceStartup; }
    }
}

public class LimitedItemDataVO
{
    public int mSubActiveID { get; private set; }
    public int mCurValue { get; private set; }


    public void OnSubActiveId(int id)
    {
        mSubActiveID = id;
    }

    public void OnValue(int value)
    {
        mCurValue = value;
    }
}
