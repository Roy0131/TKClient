using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Msg.ClientMessage;

public class TalentVO : DataBaseVO
{
    public int mTalentType { get; private set; }
    public int mTalentBaseID { get; private set; }
    public int mTalentMaxLevel { get; private set; }
    public int mTalentLevel { get; private set; }
    public int mTalengPreSkill { get; private set; }
    public int mTalentPreSkillLevel { get; private set; }
    public bool mBlLearned { get; private set; }
    public int mUpConId { get; private set; }
    public int mUpConNum { get; private set; }
    public int mTalentNameId { get; private set; }
    public string mTalentIcon { get; private set; }

    public TalentVO(int baseID)
    {
        mTalentBaseID = baseID;
        ParseTalentData(1);
        mBlLearned = false;
    }

    private void ParseTalentData(int level)
    {
        TalentConfig cfg = GameConfigMgr.Instance.GetTalentConfig(mTalentBaseID * 100 + level);
        mTalentType = cfg.PageLabel;
        mTalentBaseID = cfg.TalentBaseID;
        mTalentMaxLevel = cfg.MaxLevel;
        mTalentLevel = cfg.Level;
        mTalengPreSkill = cfg.PreSkillCond;
        mTalentPreSkillLevel = cfg.PreSkillLevCond;
        mTalentNameId = cfg.NameID;
        mTalentIcon = cfg.Icon;
        TalentConfig cfg1;
        if (mTalentLevel < mTalentMaxLevel)
        {
            if (mBlLearned)
                cfg1 = GameConfigMgr.Instance.GetTalentConfig(mTalentBaseID * 100 + level + 1);
            else
                cfg1 = GameConfigMgr.Instance.GetTalentConfig(mTalentBaseID * 100 + level);
            string[] talent = cfg1.UpgradeCost.Split(',');
            if (talent.Length % 2 != 0)
                return;
            for (int i = 0; i < talent.Length; i += 2)
            {
                mUpConId = int.Parse(talent[i]);
                mUpConNum = int.Parse(talent[i + 1]);
            }
        }
    }

    protected override void OnInitData<T>(T value)
    {
        TalentInfo talengInfo = value as TalentInfo;
        mBlLearned = true;
        ParseTalentData(talengInfo.Level);
    }

    public void OnTalentLevelUp(int level)
    {
        mBlLearned = true;
        ParseTalentData(level);
    }

    public void OnReset(int level)
    {
        mBlLearned = false;
        ParseTalentData(level);
    }
}
