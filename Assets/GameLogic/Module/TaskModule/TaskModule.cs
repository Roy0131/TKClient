using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class TaskModule : ModuleBase
{
    private Button _disBtn;
    private Toggle[] _toggles;
    private TaskView _taskView;
    private Transform _root;
    private int _curTaskType;

    public TaskModule()
        : base(ModuleID.Task, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_Task;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _disBtn = Find<Button>("Btn_Back");

        _toggles = new Toggle[2];
        for (int i = 0; i < 2; i++)
            _toggles[i] = Find<Toggle>("Root/ToggleGroup/Tog" + (i + 1));
        foreach (Toggle tog in _toggles)
            tog.onValueChanged.Add((bool blSelect) => { if (blSelect) OnTaskTypeChange(tog); });

        _taskView = new TaskView();
        _taskView.SetDisplayObject(Find("Root"));

        _root = Find<Transform>("Root");
        _disBtn.onClick.Add(OnClose);

        _curTaskType = TaskTypeConst.DAILYTask;

        ColliderHelper.SetButtonCollider(_disBtn.transform, 120, 120);
    }

    private void OnTaskTypeChange(Toggle tog)
    {
        switch (tog.name)
        {
            case "Tog1":
                _curTaskType = TaskTypeConst.DAILYTask;
                break;
            case "Tog2":
                _curTaskType = TaskTypeConst.ACHIEVETask;
                break;
        }
        TaskDataModel.Instance.ReqTaskData(_curTaskType);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        TaskDataModel.Instance.AddEvent(TaskEvent.TaskData, OnTaskValue);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        TaskDataModel.Instance.RemoveEvent(TaskEvent.TaskData, OnTaskValue);
    }

    private void OnTaskValue()
    {
        _taskView.Show(TaskDataModel.Instance.GetTaskDataByTaskType(_curTaskType), _curTaskType);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        OnTaskTypeChange(_toggles[TaskTypeConst.DAILYTask - 1]);
    }

    public override void Hide()
    {
        _toggles[TaskTypeConst.DAILYTask - 1].isOn = true;
        base.Hide();
    }

    public override void Dispose()
    {
        if (_taskView != null)
        {
            _taskView.Dispose();
            _taskView = null;
        }

        base.Dispose();
    }
    protected override void OnShowAnimator()
    {
        base.OnShowAnimator();
        ObjectHelper.PopAnimationLiner(_root);
    }
}
