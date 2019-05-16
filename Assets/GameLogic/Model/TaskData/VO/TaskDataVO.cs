using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;

public class TaskDataVO : DataBaseVO
{
    public List<TaskData> mListTaskData { get; private set; }
    public int mTime { get; private set; }


    protected override void OnInitData<T>(T value)
    {
        if (mListTaskData == null)
            mListTaskData = new List<TaskData>();
        mListTaskData.Clear();
        S2CTaskDataResponse req = value as S2CTaskDataResponse;
        mListTaskData.AddRange(req.TaskList);
        mTime = (int)Time.realtimeSinceStartup + req.DailyTaskRefreshRemainSeconds;
    }

    public int TaskTime
    {
        get { return mTime - (int)Time.realtimeSinceStartup; }
    }

    public TaskData GetTaskData(int taskId)
    {
        for (int i = 0; i < mListTaskData.Count; i++)
        {
            if (mListTaskData[i].Id==taskId)
                return mListTaskData[i];
        }
        return null;
    }

    public void TaskDataRefresh(TaskData taskData)
    {
        TaskData data = GetTaskData(taskData.Id);
        if (data == null)
        {
            mListTaskData.Add(taskData);
        }
        else
        {
            for (int i = 0; i < mListTaskData.Count; i++)
            {
                if (mListTaskData[i].Id == taskData.Id)
                    mListTaskData[i] = taskData;
            }
        }
    }

    public void deleteTask(int taskId)
    {
        if (GameConfigMgr.Instance.GetMissionConfig(taskId).Next > 0)
            mListTaskData.RemoveAll(s => (s.Id) == taskId);
    }
}
