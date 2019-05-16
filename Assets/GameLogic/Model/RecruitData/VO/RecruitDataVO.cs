using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Msg.ClientMessage;

public class RecruitDataVO
{
    public int mArticleId { get; private set; }
    public int mOneId { get; private set; }
    public int mOneCont { get; private set; }
    public int mTenId { get; private set; }
    public int mTenCont { get; private set; }
    public int mRecruitIndex { get; private set; }
    public int mTime { get; private set; }

    public void GetRecruitID(int index)
    {
        mRecruitIndex = index;
        int id = mRecruitIndex * 2 + 1;
        ExtractConfig onceConfig = GameConfigMgr.Instance.GetExtractConfig(id);
        ExtractConfig tenConfig = GameConfigMgr.Instance.GetExtractConfig(id + 1);
        string[] oncCond1 = onceConfig.ResCondition1.Split(',');
        int oneId = 0;
        int oneCount = 0;
        for (int i = 0; i < oncCond1.Length; i += 2)
        {
            if (oncCond1.Length % 2 != 0)
                continue;
            oneId = int.Parse(oncCond1[i]);
            oneCount = int.Parse(oncCond1[i + 1]);
        }

        string[] oncCond2 = onceConfig.ResCondition2.Split(',');
        int twoId = 0;
        int twoCount = 0;
        for (int i = 0; i < oncCond2.Length; i += 2)
        {
            if (oncCond2.Length % 2 != 0)
                continue;
            twoId = int.Parse(oncCond2[i]);
            twoCount = int.Parse(oncCond2[i + 1]);
        }

        string[] tenCond1 = tenConfig.ResCondition1.Split(',');
        int oneTenCount = 0;
        for (int i = 0; i < tenCond1.Length; i += 2)
        {
            if (tenCond1.Length % 2 != 0)
                continue;
            oneTenCount = int.Parse(tenCond1[i + 1]);
        }

        string[] tenCond2 = tenConfig.ResCondition2.Split(',');
        int twoTenCount = 0;
        for (int i = 0; i < tenCond2.Length; i += 2)
        {
            if (tenCond2.Length % 2 != 0)
                continue;
            twoTenCount = int.Parse(tenCond2[i + 1]);
        }

        mArticleId = oneId;

        if (index == 1)
        {
            if (BagDataModel.Instance.GetItemCountById(oneId) >= 10)
            {
                mOneId = oneId;
                mOneCont = oneCount;

                mTenId = oneId;
                mTenCont = oneTenCount;
            }
            else if(BagDataModel.Instance.GetItemCountById(oneId) > 0)
            {
                mOneId = oneId;
                mOneCont = oneCount;

                mTenId = twoId;
                mTenCont = twoTenCount;
            }
            else
            {
                mOneId = twoId;
                mOneCont = twoCount;

                mTenId = twoId;
                mTenCont = twoTenCount;
            }
            //if (BagDataModel.Instance.GetItemCountById(oneId) > 0)
            //{
            //    mOneId = oneId;
            //    mOneCont = oneCount;
            //}
            //else
            //{
            //    mOneId = twoId;
            //    mOneCont = twoCount;
            //}
            //if (BagDataModel.Instance.GetItemCountById(oneId) >= 10)
            //{
            //    mTenId = oneId;
            //    mTenCont = oneTenCount;
            //}
            //else
            //{
            //    mTenId = twoId;
            //    mTenCont = twoTenCount;
            //}
        }
        else
        {
            mOneId = oneId;
            mOneCont = oneCount;
            mTenId = oneId;
            mTenCont = oneTenCount;
        }
    }

    public void GetTime(int time)
    {
        mTime = (int)Time.realtimeSinceStartup + time;
    }

    public int LastTime
    {
        get { return mTime - (int)Time.realtimeSinceStartup; }
    }
}
