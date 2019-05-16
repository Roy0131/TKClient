using Msg.ClientMessage;
using System.Collections.Generic;

public class ActivityCopyVO:DataBaseVO
{
    public int mStageType { get; private set; }
    public int mRemainChallengeNum { get; private set; }//挑战次数
    public int mRemainBuyChallengeNum { get; private set; }//购买次数
    public string mTitle { get; private set; }
    public List<ActiveStageConfig> mListActiveCfg { get; private set; }

    protected override void OnInitData<T>(T value)
    {
        ActiveStageData item = value as ActiveStageData;
        mStageType = item.StageType;
        mRemainChallengeNum = item.RemainChallengeNum;
        mRemainBuyChallengeNum = item.RemainBuyChallengeNum;
        GetTitle(mStageType);
        if (mListActiveCfg != null)
            mListActiveCfg.Clear();
        mListActiveCfg = new List<ActiveStageConfig>();
        foreach (ActiveStageConfig cfg in ActiveStageConfig.Get().Values)
        {
            if (cfg.Type == mStageType)
                mListActiveCfg.Add(cfg);
        }

    }
    private void GetTitle(int type)
    {
        switch (type)
        {
            case ActivityCopyConst.GoldCopy:
                mTitle = LanguageMgr.GetLanguage(5001602);
                break;
            case ActivityCopyConst.ExpCopy:
                mTitle =LanguageMgr.GetLanguage(5001603);
                break;
            case ActivityCopyConst.PieceCopy:
                mTitle =LanguageMgr.GetLanguage(5001604);
                break;
        }
    }

    public void RefreshRemainChallengeNum(int num)
    {
        mRemainBuyChallengeNum = num;
    }

    public void OnRemainChallengeNum()
    {
        mRemainChallengeNum -= 1;
    }
}