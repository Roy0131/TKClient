using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;

public class ExploreDataModel : ModelDataBase<ExploreDataModel>
{
    private const string ExploreDataKey = "ExploreDataKey";
    private const string ExploreRefreshKey = "ExploreRefreshKey";
    public bool _isStoryType = false;
    public bool _isStory = false;
    public List<ExploreDataVO> exploreData { get; private set; }//任务列表
    public List<ExploreDataVO> exploreDataStory { get; private set; }//剧情任务列表
    private int _taskTime = 0;//任务刷新剩余时间
    public int TaskTime
    {
        get { return _taskTime - (int)Time.realtimeSinceStartup; }
    }
    private List<int> _exploreStoryId;
    public List<int> mLstSelRoleID { get; private set; }
    public int mRewardStageId;
    public int mStageId { get; private set; }
    public void RefreshStageId(int value)
    {
        mStageId = value;
    }

    public void OnIsStory(bool isStory)
    {
        _isStory = isStory;
    }

    public void ReqExploreData(bool value)
    {
        _isStoryType = value;
        if (CheckNeedRequest(ExploreDataKey))
            GameNetMgr.Instance.mGameServer.ReqExploreData();
        else
            DispathEvent(ExploreEvent.ExploreData);
    }

    private void OnExploreData(S2CExploreDataResponse value)
    {
        //LogHelper.LogWarning("[ExploreDataModel.OnExploreData() => 探索数据返回!!!]");
        mLstSelRoleID = new List<int>();//添加探索已选择的角色
        _exploreStoryId = new List<int>();//所有的剧情任务ID
        _taskTime = value.RefreshRemainSeconds + (int)Time.realtimeSinceStartup;
        Instance.AddLastReqTime(ExploreDataKey);
        if (exploreData != null)
            exploreData.Clear();
        exploreData = new List<ExploreDataVO>();
        if (exploreDataStory != null)
            exploreDataStory.Clear();
        exploreDataStory = new List<ExploreDataVO>();
        if (value.Datas != null && value.Datas.Count > 0)
        {
            ExploreDataVO vo;
            for (int i = 0; i < value.Datas.Count; i++)
            {
                vo = new ExploreDataVO();
                vo.InitData(value.Datas[i]);
                exploreData.Add(vo);
                if (value.Datas[i].RoleIds != null && value.Datas[i].RoleIds.Count > 0)
                {
                    for (int j = 0; j < value.Datas[i].RoleIds.Count; j++)
                    {
                        if (!mLstSelRoleID.Contains(value.Datas[i].RoleIds[j]))
                            mLstSelRoleID.Add(value.Datas[i].RoleIds[j]);
                    }
                }
            }
        }
        if (value.StoryDatas != null && value.StoryDatas.Count > 0)
        {
            ExploreDataVO vo;
            for (int i = 0; i < value.StoryDatas.Count; i++)
            {
                vo = new ExploreDataVO();
                vo.InitData(value.StoryDatas[i]);
                exploreDataStory.Add(vo);
                _exploreStoryId.Add(value.StoryDatas[i].TaskId);
                if (value.StoryDatas[i].RoleIds != null && value.StoryDatas[i].RoleIds.Count > 0)
                {
                    for (int j = 0; j < value.StoryDatas[i].RoleIds.Count; j++)
                    {
                        if (!mLstSelRoleID.Contains(value.StoryDatas[i].RoleIds[j]))
                            mLstSelRoleID.Add(value.StoryDatas[i].RoleIds[j]);
                    }
                }
            }
        }
        RefreshRedPointState();
        DispathEvent(ExploreEvent.ExploreData);
    }
    //刷新红点
    private void RefreshRedPointState()
    {
        for (int i = 0; i < exploreData.Count; i++)
        {
            if (exploreData[i].mState >= 2)
                return;
        }

        for (int i = 0; i < exploreDataStory.Count; i++)
        {
            if (exploreDataStory[i].mState >= 2)
                return;
        }
        RedPointDataModel.Instance.SetRedPointDataState(RedPointEnum.Explore, false);
    }

    private void OnExploresSelRole(S2CExploreSelRoleResponse value)
    {
        List<int> listId;
        if (value.IsStory)
        {
            for (int j = 0; j < exploreDataStory.Count; j++)
            {
                if (exploreDataStory[j].mId == value.Id)
                {
                    listId = new List<int>();
                    listId.AddRange(value.RoleIds);
                    exploreDataStory[j].OnRoleList(listId);
                }
            }
        }
        else
        {
            for (int j = 0; j < exploreData.Count; j++)
            {
                if (exploreData[j].mId == value.Id)
                {
                    listId = new List<int>();
                    listId.AddRange(value.RoleIds);
                    exploreData[j].OnRoleList(listId);
                }
            }
        }
        for (int i = 0; i < value.RoleIds.Count; i++)
        {
            if (!mLstSelRoleID.Contains(value.RoleIds[i]))
                mLstSelRoleID.Add(value.RoleIds[i]);
        }
        DispathEvent(ExploreEvent.ExploreSelRole, value.IsStory);
    }

    private void OnExploreStart(S2CExploreStartResponse value)
    {
        for (int i = 0; i < value.Ids.Count; i++)
        {
            if (value.IsStory)
            {
                for (int j = 0; j < exploreDataStory.Count; j++)
                {
                    if (exploreDataStory[j].mId== value.Ids[i])
                    {
                        exploreDataStory[j].OnState(1);
                        exploreDataStory[j].OnTime();
                        exploreDataStory[j].OnLock(value.IsLock);
                    }
                }
            }
            else
            {
                for (int j = 0; j < exploreData.Count; j++)
                {
                    if (exploreData[j].mId == value.Ids[i])
                    {
                        exploreData[j].OnState(1);
                        exploreData[j].OnTime();
                        exploreData[j].OnLock(value.IsLock);
                    }
                }
            }
        }
        List<int> listId = new List<int>();
        listId.AddRange(value.Ids);
        DispathEvent(ExploreEvent.ExploreStart, listId);
    }

    private void OnExploreSpeedup(S2CExploreSpeedupResponse value)
    {
        for (int i = 0; i < value.Ids.Count; i++)
        {
            if (!value.IsStory)
            {
                for (int j = 0; j < exploreData.Count; j++)
                {
                    if (exploreData[j].mId == value.Ids[i])
                    {
                        exploreData[j].OnState(2);
                        exploreData[j].OnLock(true);
                    }
                }
            }
        }
        List<int> listId = new List<int>();
        listId.AddRange(value.Ids);
        DispathEvent(ExploreEvent.ExploreSpeedup, listId);
    }

    public void ReqExploreRefresh()
    {
        if (CheckNeedRequest(ExploreRefreshKey, 0.5f))
            GameNetMgr.Instance.mGameServer.ReqExploreRefresh();
    }
    private void OnExploreRefresh(S2CExploreRefreshResponse value)
    {
        Instance.AddLastReqTime(ExploreRefreshKey);
        if (value.Datas != null && value.Datas.Count > 0)
        {
            for (int i = exploreData.Count - 1; i >= 0; i--)
            {
                if (!exploreData[i].mIsLock)
                    exploreData.Remove(exploreData[i]);
            }
            ExploreDataVO vo;
            for (int i = 0; i < value.Datas.Count; i++)
            {
                vo = new ExploreDataVO();
                vo.InitData(value.Datas[i]);
                exploreData.Add(vo);
            }
        }
        DispathEvent(ExploreEvent.ExploreRefresh);
    }

    private void OnExploreLock(S2CExploreLockResponse value)
    {
        if (_isStory)
        {
            for (int j = 0; j < exploreDataStory.Count; j++)
            {
                for (int i = 0; i < value.Ids.Count; i++)
                {
                    if (exploreDataStory[j].mId == value.Ids[i])
                        exploreDataStory[j].OnLock(value.IsLock);
                }
            }
        }
        else
        {
            for (int j = 0; j < exploreData.Count; j++)
            {
                for (int i = 0; i < value.Ids.Count; i++)
                {
                    if (exploreData[j].mId == value.Ids[i])
                        exploreData[j].OnLock(value.IsLock);
                }
            }
        }
        DispathEvent(ExploreEvent.ExploreLock);
    }

    private void OnExploreGetReward(S2CExploreGetRewardResponse value)
    {
        List<ItemInfo> listInfo = new List<ItemInfo>();
        listInfo.AddRange(value.RandomItems);
        mRewardStageId = value.RewardStageId;
        if (value.IsStory)
        {
            for (int i = exploreDataStory.Count - 1; i >= 0; i--)
            {
                if (exploreDataStory[i].mId == value.Id)
                {
                    for (int k = 0; k < exploreDataStory[i].mRoleIds.Count; k++)
                    {
                        if (mLstSelRoleID.Contains(exploreDataStory[i].mRoleIds[k]))
                            mLstSelRoleID.Remove(exploreDataStory[i].mRoleIds[k]);
                    }
                    if (value.RewardStageId > 0)
                    {
                        exploreDataStory[i].OnState(3);
                        exploreDataStory[i].RefreshStageId(value.RewardStageId);
                    }
                }
            }
        }
        else
        {
            for (int i = exploreData.Count - 1; i >= 0; i--)
            {
                if (exploreData[i].mId == value.Id)
                {
                    for (int k = 0; k < exploreData[i].mRoleIds.Count; k++)
                    {
                        if (mLstSelRoleID.Contains(exploreData[i].mRoleIds[k]))
                            mLstSelRoleID.Remove(exploreData[i].mRoleIds[k]);
                    }
                    if (value.RewardStageId > 0)
                    {
                        exploreData[i].OnState(3);
                        exploreData[i].RefreshStageId(value.RewardStageId);
                    }
                }
            }
        }
        DispathEvent(ExploreEvent.ExploreGetReward, listInfo, value.Id, value.RewardStageId);
    }

    private void OnExploreRemove(S2CExploreCancelResponse value)
    {
        if (value.IsStory)
        {
            for (int i = exploreDataStory.Count - 1; i >= 0; i--)
            {
                if (exploreDataStory[i].mId == value.Id)
                    exploreDataStory.Remove(exploreDataStory[i]);
            }
        }
        else
        {
            for (int i = exploreData.Count - 1; i >= 0; i--)
            {
                if (exploreData[i].mId == value.Id)
                    exploreData.Remove(exploreData[i]);
            }
        }
        DispathEvent(ExploreEvent.ExploreRemove, value.Id);
    }


    //探索数据
    public static void DoExploreData(S2CExploreDataResponse value)
    {
        Instance.OnExploreData(value);
    }
    //选择探索角色
    public static void DoExploresSelRole(S2CExploreSelRoleResponse value)
    {
        Instance.OnExploresSelRole(value);
    }
    //开始探索
    public static void DoExploreStart(S2CExploreStartResponse value)
    {
        Instance.OnExploreStart(value);
    }
    //加速
    public static void DoExploreSpeedup(S2CExploreSpeedupResponse value)
    {
        Instance.OnExploreSpeedup(value);
    }
    //刷新探索任务
    public static void DoExploreRefresh(S2CExploreRefreshResponse value)
    {
        Instance.OnExploreRefresh(value);
    }
    //解锁或锁定探索任务
    public static void DoExploreLock(S2CExploreLockResponse value)
    {
        Instance.OnExploreLock(value);
    }
    //探索奖励
    public static void DoExploreGetReward(S2CExploreGetRewardResponse value)
    {
        Instance.OnExploreGetReward(value);
    }
    //取消探索任务
    public static void DoExploreTaskRemove(S2CExploreCancelResponse value)
    {
        Instance.OnExploreRemove(value);
    }
    //剧情探索任务通知
    public static void DoExploreStoryNew(S2CExploreStoryNewNotify value)
    {
        if (Instance._exploreStoryId != null)
        {
            if (!Instance._exploreStoryId.Contains(value.TaskId))
                Instance._exploreStoryId.Add(value.TaskId);
        }
    }

    public static void DoExploreRemove(S2CExploreRemoveNotify value)
    {
        if (value.IsStory)
        {
            for (int i = Instance.exploreDataStory.Count - 1; i >= 0; i--)
            {
                if (Instance.exploreDataStory[i].mId == value.Id)
                    Instance.exploreDataStory.Remove(Instance.exploreDataStory[i]);
            }
        }
        else
        {
            for (int i = Instance.exploreData.Count - 1; i >= 0; i--)
            {
                if (Instance.exploreData[i].mId == value.Id)
                    Instance.exploreData.Remove(Instance.exploreData[i]);
            }
        }
        Instance.DispathEvent(ExploreEvent.ExploreRemove, value.Id);
    }

    public ExploreDataVO GetExploreDataStory(int id)
    {
        for (int i = 0; i < exploreDataStory.Count; i++)
        {
            if (exploreDataStory[i].mId == id)
                return exploreDataStory[i];
        }
        return null;
    }

    public bool IsExplore(int id)
    {
        if (_exploreStoryId.Contains(id))
            return true;
        return false;
    }

    protected override void DoClearData()
    {
        base.DoClearData();
        if (exploreData != null)
            exploreData.Clear();
        if (exploreDataStory != null)
            exploreDataStory.Clear();
        if (_exploreStoryId != null)
            _exploreStoryId.Clear();
        if (mLstSelRoleID != null)
            mLstSelRoleID.Clear();
    }
}
