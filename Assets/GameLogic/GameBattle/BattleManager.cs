using UnityEngine;
using System.Collections.Generic;
public class BattleManager : Singleton<BattleManager>
{
    public BattleScene mBattleScene { get; private set; }
    public BattleUIMgr mBattleUIMgr { get; private set; }

    private List<RoundNodeDataVO> _allRoundDatas;
    private BattleRoundAction _curRoundAction;

    private float _flRoundInterval = 0.5f;
    private bool _blRunInterval = false;
    private int _roundIndex = 0;

    public RoundNodeDataVO CurRoundDataVO
    {
        get { return _curRoundAction.RoundData; }
    }

    public void Init()
    {
        mBattleUIMgr = new BattleUIMgr();
        mBattleScene = new BattleScene();

        _blInited = true;
        _allRoundDatas = BattleDataModel.Instance.mlstActionRounds;
        _blRunInterval = true;
        GameEventMgr.Instance.mBattleDispatcher.AddEvent(BattleEvent.BattleRoundEnd, OnEndRound);
    }

    public void PauseBattle()
    {
        _blInited = false;
    }

    public void StartBattle()
    {
        _blInited = true;
    }

    private void EndBattle()
    {
        _blInited = false;
        GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.EndCondTrigger, NewBieGuide.EndConditionConst.BattleEnd);
        GameEventMgr.Instance.mBattleDispatcher.DispathEvent(BattleEvent.BattleEnd);
    }

    private void OnEndRound()
    {
        if (_curRoundAction.mRoundType == BattleRoundActionType.EnterRound)
        {
            _roundIndex = 0;
        }
        else
        {
            _roundIndex++;
            if (_roundIndex >= _allRoundDatas.Count)
            {
                //Debuger.LogWarning("11111");
                EndBattle();
                return;
            }
        }
        _blRunInterval = true;
    }

    public void StartRound(RoundNodeDataVO data = null)
    {
        _blRunInterval = false;
        if (_curRoundAction == null)
        {
            _curRoundAction = new BattleRoundAction();
            if (BattleDataModel.Instance.mEnterRound != null)
            {
                _curRoundAction.Start(BattleDataModel.Instance.mEnterRound, BattleRoundActionType.EnterRound);
                return;
            }            
        }
        _curRoundAction.Start(_allRoundDatas[_roundIndex]);
    }

    public void Update()
    {
        if (!_blInited)
            return;
        mBattleScene.Update();
        if (_blRunInterval)
        {
            _flRoundInterval -= Time.deltaTime;
            if (_flRoundInterval <= 0.01f)
            {
                _flRoundInterval = 0.5f;
                StartRound();
                return;
            }
        }
        else
        {
            if (_curRoundAction != null)
                _curRoundAction.Update();
        }
    }

    public void SkipBattle()
    {
        EndBattle();
    }

    public void Dispose()
    {
        GameEventMgr.Instance.mBattleDispatcher.RemoveEvent(BattleEvent.BattleRoundEnd, OnEndRound);
        if (mBattleScene != null)
        {
            mBattleScene.Dispose();
            mBattleScene = null;
        }
        if (mBattleUIMgr != null)
        {
            mBattleUIMgr.Dispose();
            mBattleUIMgr = null;
        }
        if (_curRoundAction != null)
        {
            _curRoundAction.Dispose();
            _curRoundAction = null;
        }
        _allRoundDatas = null;
        _blInited = false;
        _roundIndex = 0;
    }
}