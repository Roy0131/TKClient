using Msg.ClientMessage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarnivalDataVO : DataBaseVO
{
    public int mId { get; private set; }
    public int mValue { get; private set; }
    public int mValues { get; private set; }
    public int mActiveType { get; private set; }
    public int mEventCount { get; private set; }
    public int mDescriptionId { get; private set; }
    public List<ItemInfo> mRewardInfo { get; private set; }
    public int mParam1 { get; private set; }
    public int mParam2 { get; private set; }
    public int mParam3 { get; private set; }
    public int mParam4 { get; private set; }
    public ItemInfo mExchangeInfo1 { get; private set; }
    public ItemInfo mExchangeInfo2 { get; private set; }


    protected override void OnInitData<T>(T value)
    {
        CarnivalTaskData taskData = value as CarnivalTaskData;
        mId = taskData.Id;
        mValue = taskData.Value;
        mValues = taskData.Value2;
    }

    public void OnCarnivalSubConfig(CarnivalSubConfig cfg)
    {
        mDescriptionId = cfg.DescriptionId;
        mActiveType = cfg.ActiveType;
        mEventCount = cfg.EventCount;
        mParam1 = cfg.Param1;
        mParam2 = cfg.Param2;
        mParam3 = cfg.Param3;
        mParam4 = cfg.Param4;
        string[] strs = cfg.Reward.Split(',');
        if (strs == null || strs.Length % 2 != 0)
            return;
        ItemInfo info;
        mRewardInfo = new List<ItemInfo>();
        for (int i = 0; i < strs.Length; i += 2)
        {
            info = new ItemInfo();
            info.Id = int.Parse(strs[i]);
            info.Value = int.Parse(strs[i + 1]);
            mRewardInfo.Add(info);
        }
        if (cfg.ActiveType == 405)
        {
            if (mParam1 == 0)
            {
                return;
            }
            else
            {
                mExchangeInfo1 = new ItemInfo();
                if (mParam3 == 0)
                {
                    mExchangeInfo1.Id = mParam1;
                    mExchangeInfo1.Value = mParam2;
                }
                else
                {
                    mExchangeInfo2 = new ItemInfo();
                    mExchangeInfo1.Id = mParam1;
                    mExchangeInfo1.Value = mParam2;
                    mExchangeInfo2.Id = mParam3;
                    mExchangeInfo2.Value = mParam4;
                }
            }
        }
    }

    public void OnValue()
    {
        mValue += 1;
    }
}
