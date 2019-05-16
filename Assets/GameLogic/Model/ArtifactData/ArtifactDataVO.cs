using Msg.ClientMessage;
using System.Collections.Generic;

public class ArtifactDataVO : DataBaseVO
{
    public ArtifactData mArtifactData { get; private set; }
    public int mUnlockLevel { get; private set; }//解锁需求玩家等级
    public int mUnlockVIPLevel { get; private set; }//解锁需求VIP等级
    public int mMaxRank { get; private set; }//最大品阶
    public int mMaxLevel { get; private set; }//最大等级
    public int mArtifactNameId { get; private set; }//神器名字ID
    public ItemInfo mUnlockInfo { get; private set; }//解锁资源消耗
    public int mCurMaxLevel { get; private set; }//神器当前等级上限
    public int mCurSkillId { get; private set; }//神器当前技能ID
    public List<ItemInfo> mCurArtifactAtt { get; private set; }//神器当前属性
    public string mLevelUpResCost { get; private set; }//神器升级消耗
    public string mRankUpResCost { get; private set; }//神器升阶消耗
    public List<ItemInfo> mDecomposeRes { get; private set; }//神器重铸返还资源
    public string mArtifactIcon { get; private set; }//神器图标
    public string mArtifactBjIcon { get; private set; }//神器背景图
    public int mSortIndex { get; private set; }//排序ID
    
    public int mPreViewSkillId { get; private set; }//神器预览技能ID
    public List<ItemInfo> mPreViewAtt { get; private set; }//神器预览属性




    protected override void OnInitData<T>(T value)
    {
        mArtifactData = value as ArtifactData;
        ArtifactUnlockConfig artifactUnlockCfg = GameConfigMgr.Instance.GetArtifactUnlockConfig(mArtifactData.Id);
        mSortIndex = artifactUnlockCfg.Sort;
        mUnlockLevel = artifactUnlockCfg.UnLockLevel;
        mUnlockVIPLevel = artifactUnlockCfg.UnLockVIPLevel;
        mMaxRank = artifactUnlockCfg.MaxRank;
        mMaxLevel = artifactUnlockCfg.ShowLevel;
        mArtifactNameId = artifactUnlockCfg.Name;
        mArtifactBjIcon = artifactUnlockCfg.BackGroundImg;
        string[] ResCost = artifactUnlockCfg.UnLockResCost.Split(',');
        if (ResCost.Length % 2 != 0)
            return;
        mUnlockInfo = new ItemInfo();
        for (int i = 0; i < ResCost.Length; i += 2)
        {
            mUnlockInfo.Id = int.Parse(ResCost[i]);
            mUnlockInfo.Value = int.Parse(ResCost[i + 1]);
        }
        OnArtifactLevelCfg();
        OnPreView();
    }

    private void OnPreView()
    {
        ArtifactLevelConfig artifactLevelCfg = GameConfigMgr.Instance.GetArtifactLevelConfig(mArtifactData.Id * 10000 + mMaxRank * 100 + mMaxLevel);
        mPreViewSkillId = artifactLevelCfg.SkillID;
        mPreViewAtt = OnListInfo(artifactLevelCfg.ArtifactAttr.Split(','));
    }

    public void OnArtifactLevelCfg()
    {
        if (mArtifactData.Level > 0)
        {
            ArtifactLevelConfig artifactLevelCfg = GameConfigMgr.Instance.GetArtifactLevelConfig(mArtifactData.Id * 10000 + mArtifactData.Rank * 100 + mArtifactData.Level);
            mCurMaxLevel = artifactLevelCfg.MaxLevel;
            mCurSkillId = artifactLevelCfg.SkillID;
            mCurArtifactAtt = OnListInfo(artifactLevelCfg.ArtifactAttr.Split(','));
            mDecomposeRes = OnListInfo(artifactLevelCfg.DecomposeRes.Split(','));
            mLevelUpResCost = artifactLevelCfg.LevelUpResCost;
            mRankUpResCost = artifactLevelCfg.RankUpResCost;
            mArtifactIcon = artifactLevelCfg.Icon;
        }
        else
        {
            ArtifactLevelConfig artifactLevelCfg = GameConfigMgr.Instance.GetArtifactLevelConfig(mArtifactData.Id * 10000 + mArtifactData.Rank * 100 + 1);
            mArtifactIcon = artifactLevelCfg.Icon;
        }
    }

    private List<ItemInfo> OnListInfo(string[] strs)
    {
        if (strs == null || strs.Length % 2 != 0)
            return null;
        List<ItemInfo> itemInfos = new List<ItemInfo>();
         ItemInfo info;
        for (int i = 0; i < strs.Length; i += 2)
        {
            info = new ItemInfo();
            info.Id = int.Parse(strs[i]);
            info.Value = int.Parse(strs[i + 1]);
            itemInfos.Add(info);
        }
        return itemInfos;
    }

    public void OnArtifactLevel(int level)
    {
        mArtifactData.Level = level;
        OnArtifactLevelCfg();
    }

    public void OnArtifactRank(int rank)
    {
        mArtifactData.Rank = rank;
        OnArtifactLevelCfg();
    }

    public void OnArtifactReset()
    {
        mArtifactData.Level = 1;
        mArtifactData.Rank = 1;
        OnArtifactLevelCfg();
    }
}
