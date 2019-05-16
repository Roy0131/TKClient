public class SkillRoundAction : ActionNodeBase
{
    private ActionNodeData _attackRoundData;
    private SkillActionBase _attackAction;
    private int _actionIndex = 0;

    protected override void OnInitData<T>(T data)
    {
        base.OnInitData(data);
        _attackRoundData = data as ActionNodeData;
        _actionIndex = 0;
        DoAttackAction();
    }

    #region Event
    protected override void AddEvent()
    {
        base.AddEvent();
        GameEventMgr.Instance.mBattleDispatcher.AddEvent<SkillActionBase>(BattleEvent.BattleStepEnd, OnEndStep);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GameEventMgr.Instance.mBattleDispatcher.RemoveEvent<SkillActionBase>(BattleEvent.BattleStepEnd, OnEndStep);
    }
    #endregion

    private void OnEndStep(SkillActionBase value)
    {
        _actionIndex++;
        if (_actionIndex >= _attackRoundData.mActionItemDatas.Count)
        {
            GameEventMgr.Instance.mBattleDispatcher.DispathEvent(BattleEvent.BattleAttackRoundEnd, this);
            return;
        }
        DoAttackAction();
    }

    private void DoAttackAction()
    {
        ActionItemData attackItemData = _attackRoundData.mActionItemDatas[_actionIndex];
        if (!attackItemData.mBlInvalidNode)
        {
            LogHelper.LogError("attack invalid, do next action item!");
            OnEndStep(null);
            return;
        }
        if (_attackAction != null)
        {
            _attackAction.Dispose();
            _attackAction = null;
        }
        int skillAnimType = attackItemData.mSkillConfig.SkillAnimType;
        if (skillAnimType == 4)
            _attackAction = new ArtifactAction();
        else if (skillAnimType % 2 == 0)
            _attackAction = new StandSkillAction();
        else
            _attackAction = new MTTargetAttackAction();
        _attackAction.InitData(attackItemData);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (_attackAction != null)
            _attackAction.Update();
    }

    protected override void OnDispose()
    {
        base.OnDispose();
        if (_attackAction != null)
        {
            _attackAction.Dispose();
            _attackAction = null;
        }
        _attackRoundData = null;
    }
}