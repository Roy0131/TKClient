using Msg.ClientMessage;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ExploreDataVO : DataBaseVO
{
    public int mId { get; private set; }
    public int mTaskId { get; private set; }
    public int mState { get; private set; }
    public int mRoleId4Title { get; private set; }
    public int mNameId4Title { get; private set; }
    public int mRemainSeconds { get; private set; }
    public int mRewardStageId { get; private set; }
    public bool mIsLock { get; private set; }
    public List<int> mRoleCampsCanSel { get; private set; }
    public List<int> mRoleTypesCanSel { get; private set; }
    public List<int> mConstReward { get; private set; }
    public List<int> mRoleIds { get; private set; }
    public List<int> mRandomRewards { get; private set; }
    public string mTaskName { get; private set; }
    public int mTargetTime = 0;
    public SearchTaskConfig mExploreTaskCfg { get; private set; }

    protected override void OnInitData<T>(T value)
    {
        ExploreData item = value as ExploreData;
        mId = item.Id;
        mTaskId = item.TaskId;
        mState = item.State;
        mRoleId4Title = item.RoleId4Title;
        mNameId4Title = item.NameId4Title;
        if (mState == 0)
            mRemainSeconds = item.RemainSeconds;
        else
            mRemainSeconds = item.RemainSeconds + (int)Time.realtimeSinceStartup;
        mRewardStageId = item.RewardStageId;
        mIsLock = item.IsLock;
        mExploreTaskCfg = GameConfigMgr.Instance.GetSearchTaskConfig(item.TaskId);
        //mTargetTime = item.RemainSeconds + (int)Time.realtimeSinceStartup;
        if (item.RoleCampsCanSel.Count > 0)
        {
            mRoleCampsCanSel = new List<int>();
            for (int i = 0; i < item.RoleCampsCanSel.Count; i++)
                mRoleCampsCanSel.Add(item.RoleCampsCanSel[i]);
        }
        if (item.RoleTypesCanSel.Count > 0)
        {
            mRoleTypesCanSel = new List<int>();
            for (int i = 0; i < item.RoleTypesCanSel.Count; i++)
                mRoleTypesCanSel.Add(item.RoleTypesCanSel[i]);
        }
        if (item.RoleIds.Count > 0)
        {
            if (mRoleIds == null)
                mRoleIds = new List<int>();
            for (int i = 0; i < item.RoleIds.Count; i++)
                mRoleIds.Add(item.RoleIds[i]);
        }
        if (item.RandomRewards.Count > 0)
        {
            mRandomRewards = new List<int>();
            for (int i = 0; i < item.RandomRewards.Count; i++)
                mRandomRewards.Add(item.RandomRewards[i]);
        }
        if (GameConfigMgr.Instance.GetSearchTaskConfig(item.TaskId).ConstReward.Length != 0)
        {
            string[] constReward = GameConfigMgr.Instance.GetSearchTaskConfig(item.TaskId).ConstReward.Split(',');
            mConstReward = new List<int>();
            for (int i = 0; i < constReward.Length; i++)
                mConstReward.Add(Convert.ToInt32(constReward[i]));
        }
        if (GameConfigMgr.Instance.GetSearchTaskConfig(item.TaskId).Type == 2)
            mTaskName = LanguageMgr.GetLanguage(Convert.ToInt32(GameConfigMgr.Instance.GetSearchTaskConfig(item.TaskId).TaskNameList));
    }

    public void OnRoleList(List<int> listId)
    {
        if (mRoleIds == null)
            mRoleIds = new List<int>();
        mRoleIds.AddRange(listId);
    }

    public void OnState(int state)
    {
        mState = state;
    }

    public void OnLock(bool value)
    {
        mIsLock = value;
    }

    public void OnTime()
    {
        mRemainSeconds += (int)Time.realtimeSinceStartup;
    }

    public int RemainSeconds
    {
        get{ return mRemainSeconds - (int)Time.realtimeSinceStartup; }
    }

    public void RefreshStageId(int value)
    {
        mRewardStageId = value;
    }

    public void RefreshRoleIds(List<int> value)
    {
        mRoleIds = new List<int>();
        mRoleIds.AddRange(value);
    }
}
