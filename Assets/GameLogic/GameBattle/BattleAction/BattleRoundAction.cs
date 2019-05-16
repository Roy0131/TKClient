using System.Collections.Generic;

public class BattleRoundAction : ActionNodeBase
{
    public BattleRoundActionType mRoundType { get; private set; }
    private RoundNodeDataVO _roundData;
    private int _actionIndex = 0;
    private SkillRoundAction _attackRoundNode = null;

    private List<Fighter> _lstFighter = new List<Fighter>();

    public RoundNodeDataVO RoundData
    {
        get { return _roundData; }
    }

    public void Start(RoundNodeDataVO value, BattleRoundActionType type = BattleRoundActionType.BattleRound)
    {
        _lstFighter.Clear();
        _blWaitFighterEffect = false;
        mRoundType = type;
        _roundData = value;
        _actionIndex = 0;
        _blInitialed = true;
        if (_attackRoundNode == null)
            _attackRoundNode = new SkillRoundAction();
        DoRoundAction();
        GameEventMgr.Instance.mBattleDispatcher.DispathEvent(BattleEvent.BattleRoundStart, _roundData);
    }

    private void DoRoundAction()
    {
        _attackRoundNode.InitData(_roundData.mlstActionNodes[_actionIndex]);
    }

    #region event
    protected override void AddEvent()
    {
        base.AddEvent();
        GameEventMgr.Instance.mBattleDispatcher.AddEvent<SkillRoundAction>(BattleEvent.BattleAttackRoundEnd, OnAttackRoundEnd);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GameEventMgr.Instance.mBattleDispatcher.RemoveEvent<SkillRoundAction>(BattleEvent.BattleAttackRoundEnd, OnAttackRoundEnd);
    }
    #endregion

    private void DoProccessEnergy(IDictionary<int, int> dict, bool blHero)
    {
        Fighter fighter;
        foreach(KeyValuePair<int, int> kv in dict)
        {
            fighter = BattleManager.Instance.mBattleScene.GetFighterBySeatIndex(kv.Key, blHero);
            fighter.RefreshHpAndEnery(fighter.mData.mCurHp, kv.Value, fighter.mData.mCurShield);
        }
    }

    private void OnAttackRoundEnd(SkillRoundAction node)
    {
        _actionIndex++;
        _lstFighter.Clear();
        if(_actionIndex >= _roundData.mlstActionNodes.Count)
        {
            if (_roundData.mlstChangeFighters != null && _roundData.mlstChangeFighters.Count > 0)
            {
                _blWaitFighterEffect = true;
                for (int i = 0; i < _roundData.mlstChangeFighters.Count; i++)
                {
                    FighterDamageDataVO targetData = _roundData.mlstChangeFighters[i];
                    Fighter targeter = BattleManager.Instance.mBattleScene.GetFighterBySeatIndex(targetData.mSide, targetData.mSeatIndex);
                    if (targeter == null)
                        continue;
                    targeter.DoDamage(targetData);
                    _lstFighter.Add(targeter);
                }
            }

            if(_roundData.mMyFighterEnergy != null)
                DoProccessEnergy(_roundData.mMyFighterEnergy, true);
            
            if (_roundData.mEnemyFighterEnergy != null)
                DoProccessEnergy(_roundData.mEnemyFighterEnergy, false);

            if (_roundData.mlstRemoveBuffs != null && _roundData.mlstRemoveBuffs.Count > 0)
            {
                for (int i = 0; i < _roundData.mlstRemoveBuffs.Count; i++)
                {
                    Fighter fighter = BattleManager.Instance.mBattleScene.GetFighterBySeatIndex(_roundData.mlstRemoveBuffs[i].mSide, _roundData.mlstRemoveBuffs[i].mSeatIndex);
                    if (fighter == null)
                        continue;
                    fighter.RemoveBuff(_roundData.mlstRemoveBuffs[i]);
                }
            }

            if (_lstFighter.Count > 0)
                return;
            GameEventMgr.Instance.mBattleDispatcher.DispathEvent(BattleEvent.BattleRoundEnd);
            return;
        }
        DoRoundAction();
    }

    private bool _blWaitFighterEffect = false;
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (!_blWaitFighterEffect)
        {
            if (_attackRoundNode != null)
                _attackRoundNode.Update();
        }
        else
        {
            for (int i = 0; i < _lstFighter.Count; i++)
            {
                if (!_lstFighter[i].FighterAllEffectFinish)
                    return;
            }
            GameEventMgr.Instance.mBattleDispatcher.DispathEvent(BattleEvent.BattleRoundEnd);
        }
    }

    protected override void OnDispose()
    {
        base.OnDispose();
        if (_attackRoundNode != null)
        {
            _attackRoundNode.Dispose();
            _attackRoundNode = null;
        }
        _roundData = null;
    }
}