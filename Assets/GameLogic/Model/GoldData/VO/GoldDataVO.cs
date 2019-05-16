using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldDataVO
{
    public float mGold { get; private set; }
    public int mCon { get; private set; }
    public int mLeftNum { get; private set; }
    public int mGoldIndex { get; private set; }

    public void GetGoldID(int index,int leftNum)
    {
        mGoldIndex = index;
        mLeftNum = leftNum;
        GoldHandConfig cfg = GameConfigMgr.Instance.GetGoldHandConfig(HeroDataModel.Instance.mHeroInfoData.mLevel);
        VipConfig vipCfg = GameConfigMgr.Instance.GetVipConfig(HeroDataModel.Instance.mHeroInfoData.mVipLevel);
        if (index==1)
        {
            mGold = cfg.GoldReward1 * ((float)vipCfg.GoldFingerBonus / 10000 + 1);
            mCon = 0;
        }
        if (index==2)
        {
            mGold = cfg.GoldReward2 * ((float)vipCfg.GoldFingerBonus / 10000 + 1);
            mCon = cfg.GemCost2;
        }
        if (index==3)
        {
            mGold = cfg.GoldReward3 * ((float)vipCfg.GoldFingerBonus / 10000 + 1);
            mCon = cfg.GemCost3;
        }
    }

    public void OnLeftNum()
    {
        mLeftNum -= 1;
    }
}
