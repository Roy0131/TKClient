using Msg.ClientMessage;
using System.Collections.Generic;

public class TaskTypeConst
{
    public const int ALLTASK = 0;//全部任务
    public const int DAILYTask = 1;//日常任务
    public const int ACHIEVETask = 2;//成就任务
}



public class TaskDataModel : ModelDataBase<TaskDataModel>
{
    private const string DAILYTask = "dAILYTask";
    private Dictionary<int, TaskDataVO> _dictAllTaskData = new Dictionary<int, TaskDataVO>();


    public void ReqTaskData(int taskType)
    {
        if (CheckNeedRequest(DAILYTask + taskType))
            GameNetMgr.Instance.mGameServer.ReqTaskData(taskType);
        else
            DispathEvent(TaskEvent.TaskData);
    }

    private void OnTaskData(S2CTaskDataResponse value)
    {
        TaskDataVO VO = new TaskDataVO();
        VO.InitData(value);
        if (_dictAllTaskData.ContainsKey(value.TaskType))
            _dictAllTaskData[value.TaskType] = VO;
        else
            _dictAllTaskData.Add(value.TaskType, VO);
        AddLastReqTime(DAILYTask + value.TaskType);
        DispathEvent(TaskEvent.TaskData);
    }

    private void OnTaskReward(S2CTaskRewardResponse value)
    {
        _dictAllTaskData[GameConfigMgr.Instance.GetMissionConfig(value.TaskId).Type].deleteTask(value.TaskId);
        DispathEvent(TaskEvent.TaskReward, value.TaskId);
    }

    private void OnTaskValue(S2CTaskValueNotify value)
    {
        if (_dictAllTaskData.ContainsKey(GameConfigMgr.Instance.GetMissionConfig(value.Data.Id).Type))
            _dictAllTaskData[GameConfigMgr.Instance.GetMissionConfig(value.Data.Id).Type].TaskDataRefresh(value.Data);
        if (value.Data.State == 1)
        {
            if (GameConfigMgr.Instance.GetMissionConfig(value.Data.Id).Type == 1)
                RedPointDataModel.Instance.SetRedPointDataState(RedPointEnum.DAILY_TASK, true);
            else
                RedPointDataModel.Instance.SetRedPointDataState(RedPointEnum.Achieve_Task, true);
        }
        TaskRedState();
        DispathEvent(TaskEvent.TaskData);
    }

    private void TaskRedState()
    {
        bool isDailyState = false;
        bool isAchieveState = false;
        foreach (TaskDataVO vo in _dictAllTaskData.Values)
        {
            for (int i = 0; i < vo.mListTaskData.Count; i++)
            {
                if (vo.mListTaskData[i].State == 1 && GameConfigMgr.Instance.GetMissionConfig(vo.mListTaskData[i].Id).Type == 1)
                {
                    isDailyState = true;
                    break;
                }
                if (vo.mListTaskData[i].State == 1 && GameConfigMgr.Instance.GetMissionConfig(vo.mListTaskData[i].Id).Type == 2)
                {
                    isAchieveState = true;
                    break;
                }
            }
        }
        if (_dictAllTaskData.ContainsKey(TaskTypeConst.DAILYTask))
            RedPointDataModel.Instance.SetRedPointDataState(RedPointEnum.DAILY_TASK, isDailyState);
        if (_dictAllTaskData.ContainsKey(TaskTypeConst.ACHIEVETask))
            RedPointDataModel.Instance.SetRedPointDataState(RedPointEnum.Achieve_Task, isAchieveState);
    }

    public TaskDataVO GetTaskDataByTaskType(int TaskType)
    {
        if (_dictAllTaskData.ContainsKey(TaskType))
            return _dictAllTaskData[TaskType];
        return null;
    }

    public static void DoTaskData(S2CTaskDataResponse value)
    {
        Instance.OnTaskData(value);
    }

    public static void DoTaskReward(S2CTaskRewardResponse value)
    {
        Instance.OnTaskReward(value);
    }

    public static void DoTaskValue(S2CTaskValueNotify value)
    {
        Instance.OnTaskValue(value);
    }

    protected override void DoClearData()
    {
        base.DoClearData();
        if (_dictAllTaskData != null)
            _dictAllTaskData.Clear();
    }
}
