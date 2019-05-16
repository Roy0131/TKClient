using Framework.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskView : UIBaseView
{
    private Text _taskTitle;
    private Text _time;
    private Text _allText;
    private Text fillText;
    private Image fillImg;
    private Button drawBtn;
    private RectTransform _rewardGrod;
    private RectTransform _taskGroup;
    private RectTransform _parent;
    private RectTransform _allParent;
    private GameObject _timeObj;
    private GameObject _allTask;
    private GameObject _allTaskObj;
    private List<TaskItemView> _taskItemViews;

    private TaskDataVO _taskDataVO;
    private TaskData _taskData;
    private ItemView _view;

    private GameObject _achieveRed;
    private GameObject _dailyRed;

    private int _curTaskType;
    private int _dailyNum;

    private uint _timer = 0;
    private int _taskTime = 0;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _taskTitle = Find<Text>("Title/Text");
        _time = Find<Text>("TaskTime");
        _allText = Find<Text>("AllTask/AllText");
        fillText = Find<Text>("AllTask/FillImg/Text");
        fillImg = Find<Image>("AllTask/FillImg/Fill");
        drawBtn = Find<Button>("AllTask/DrawBtn");
        _taskGroup = Find<RectTransform>("Panel_Scroll");
        _rewardGrod = Find<RectTransform>("AllTask/RewardGrod");
        _parent = Find<RectTransform>("Panel_Scroll/KnapsackPanel");
        _allParent = Find<RectTransform>("AllTask/RewardGrod");
        _timeObj = Find("TaskTime");
        _allTask = Find("AllTask");
        _allTaskObj = Find("Panel_Scroll/KnapsackPanel/AllTaskObj");
        _achieveRed = Find("ToggleGroup/Tog2/RedPoint");
        _dailyRed = Find("ToggleGroup/Tog1/RedPoint");

        RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.Achieve_Task, _achieveRed);
        RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.DAILY_TASK, _dailyRed);

        drawBtn.onClick.Add(OnTaskDraw);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        TaskDataModel.Instance.AddEvent<int>(TaskEvent.TaskReward, OnTaskReward);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        TaskDataModel.Instance.RemoveEvent<int>(TaskEvent.TaskReward, OnTaskReward);
    }

    private void OnTaskReward(int taskId)
    {
        MissionConfig cfg = GameConfigMgr.Instance.GetMissionConfig(taskId);
        List<ItemInfo> _listItemInfo = new List<ItemInfo>();
        ItemInfo _itemInfo;
        string[] univalent = cfg.Reward.Split(',');
        if (univalent.Length % 2 != 0)
            return;
        for (int j = 0; j < univalent.Length; j += 2)
        {
            _itemInfo = new ItemInfo();
            _itemInfo.Id = int.Parse(univalent[j]);
            _itemInfo.Value = int.Parse(univalent[j + 1]);
            _listItemInfo.Add(_itemInfo);
        }
        GetItemTipMgr.Instance.ShowItemResult(_listItemInfo);
    }

    private void OnTaskValue()
    {
        if (_taskDataVO == null)
            return;
        _taskDataVO.mListTaskData.Sort(SortTask);
        if (_curTaskType == TaskTypeConst.DAILYTask)
        {
            _taskTime = _taskDataVO.TaskTime;
            if (_timer != 0)
                TimerHeap.DelTimer(_timer);
            int interval = 1000;
            _timer = TimerHeap.AddTimer(0, interval, OnAddTime);
        }
        OnTaskitemClear();
        _taskItemViews = new List<TaskItemView>();
        for (int i = 0; i < _taskDataVO.mListTaskData.Count; i++)
        {
            if (_taskDataVO.mListTaskData[i].Id != 20001)
            {
                GameObject obj = GameObject.Instantiate(_allTaskObj);
                obj.transform.SetParent(_parent, false);
                TaskItemView taskItemView = new TaskItemView();
                taskItemView.SetDisplayObject(obj);
                taskItemView.Show(_taskDataVO.mListTaskData[i]);
                _taskItemViews.Add(taskItemView);
            }
            else
            {
                OnTaskInit(_taskDataVO.mListTaskData[i]);
            }
        }
        _parent.anchoredPosition = new Vector2(0, 0);
    }

    private int SortTask(TaskData V0, TaskData V1)
    {
        MissionConfig cfg1 = GameConfigMgr.Instance.GetMissionConfig(V0.Id);
        MissionConfig cfg2 = GameConfigMgr.Instance.GetMissionConfig(V1.Id);
        if (V0.State == V1.State)
        {
            if (cfg1.Sort != 0 && cfg2.Sort != 0)
                return cfg1.Sort < cfg2.Sort ? -1 : 1;
            else
                return V0.Id > V1.Id ? 1 : -1;
        }
        else if (V0.State == 0)
            return V1.State == 1 ? 1 : -1;
        else if (V0.State == 1)
            return -1;
        else if (V0.State == 2)
            return 1;
        else
            return 0;
    }

    private void OnAddTime()
    {
        if (_taskTime > 0)
        {
            _taskTime -= 1;
            _time.text = (LanguageMgr.GetLanguage(5001316) + "<color=#C46E10>" + TimeHelper.GetCountTime(_taskTime) + "</color>");
        }
    }

    private void OnTaskInit(TaskData taskData)
    {
        _taskData = taskData;
        _dailyNum = _taskDataVO.mListTaskData.Count - 1;
        fillText.text = (taskData.Value + "/" + _dailyNum);
        fillImg.fillAmount = (float)taskData.Value / (float)_dailyNum;
        
        MissionConfig cfg = GameConfigMgr.Instance.GetMissionConfig(taskData.Id);
        string[] rewards = cfg.Reward.Split(',');
        if (rewards.Length % 2 != 0)
            return;
        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        for (int i = 0; i < rewards.Length; i += 2)
        {
            ItemInfo itemInfo = new ItemInfo();
            itemInfo.Id = int.Parse(rewards[i]);
            itemInfo.Value = int.Parse(rewards[i + 1]);
            _view = ItemFactory.Instance.CreateItemView(itemInfo, ItemViewType.HeroItem, null);
            _view.mRectTransform.SetParent(_allParent, false);
        }
        _allText.text = LanguageMgr.GetLanguage(cfg.Title);
        if (taskData.State == 1)
            drawBtn.interactable = true;
        else
            drawBtn.interactable = false;
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _taskDataVO = args[0] as TaskDataVO;
        _curTaskType = int.Parse(args[1].ToString());
        if (_curTaskType == TaskTypeConst.DAILYTask)
        {
            _taskGroup.anchoredPosition = new Vector2(-12, -107);
            _taskGroup.sizeDelta = new Vector2(836, 377);
            _taskTitle.text = LanguageMgr.GetLanguage(5001201);
        }
        else
        {
            _taskGroup.anchoredPosition = new Vector2(-12, -36);
            _taskGroup.sizeDelta = new Vector2(836, 518);
            _taskTitle.text = LanguageMgr.GetLanguage(5001205);
        }
        _timeObj.SetActive(_curTaskType == TaskTypeConst.DAILYTask);
        _allTask.SetActive(_curTaskType == TaskTypeConst.DAILYTask);
        OnTaskValue();
    }

    private void OnTaskDraw()
    {
        GameNetMgr.Instance.mGameServer.ReqTaskReward(_taskData.Id);
    }

    private void OnTaskitemClear()
    {
        if (_taskItemViews != null)
        {
            for (int i = 0; i < _taskItemViews.Count; i++)
                _taskItemViews[i].Dispose();
            _taskItemViews.Clear();
            _taskItemViews = null;
        }
    }

    public override void Dispose()
    {
        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        _view = null;
        OnTaskitemClear();
        RedPointTipsMgr.Instance.RedPointUnBindObject(RedPointEnum.Achieve_Task, _achieveRed);
        RedPointTipsMgr.Instance.RedPointUnBindObject(RedPointEnum.DAILY_TASK, _dailyRed);
        base.Dispose();
    }
}
