using System.Collections.Generic;
using UnityEngine;

public class HangUpMgr : Singleton<HangUpMgr>
{
    enum HangupStatus
    {
        None,
        Attack,
        AttackEnd,
        CreateInterval,
    }

    private HangupScene _scene;
    private List<HangupFighter> _lstOwnerFighters;
    private HangupFighter _enemyFighter;

    private HangupFighter _curAttacker;
    private float _flIntervalTime = 1f;
    private bool _blInterval = false;
    private HangupStatus _status = HangupStatus.None;
    private float _flNextBattleIntervalTime = 1f;

    private HangDataVO _dataVO;

    public void Init()
    {
        _scene = RoleRTMgr.Instance.GetRoleRTLogicByType<HangupScene>(RoleRTType.Hangup);
    }

    private void CreateNewEnemy()
    {
        if (_enemyFighter != null)
            _enemyFighter.Dispose();
        CardConfig enemyCfg = _dataVO.GetRandomBossCard();

        _enemyFighter = new HangupFighter(false);
        _enemyFighter.InitData(enemyCfg);
        _enemyFighter.AddToStage(_scene.mTargeterParent);

        _blInited = true;
        _blInterval = true;
        _flIntervalTime = 0.8f;
        _curAttacker = null;
        _status = HangupStatus.Attack;

        _flNextBattleIntervalTime = 1f;
    }

    private void CreateBattle()
    {
        _scene.ResetLocalPosition(_dataVO.StageID);

        _lstOwnerFighters = new List<HangupFighter>();
        HangupFighter fighter;
        Transform parent;
        int len = Mathf.Min(_dataVO.mlstRoleCards.Count, 3);
        for (int i = 0; i < len; i++)
        {
            parent = _scene.GetOwnerParentByIndex(i);
            fighter = new HangupFighter(true);
            fighter.InitData(_dataVO.mlstRoleCards[i]);
            fighter.AddToStage(parent);
            _lstOwnerFighters.Insert(0, fighter);
        }

        CreateNewEnemy();
    }

    private void ChangeAttacker()
    {
        HangupFighter targeter;
        if (_enemyFighter.mBeHitCount <= 0)
        {
            _status = HangupStatus.AttackEnd;
            return;
        }
        if (_lstOwnerFighters.Count == 0)
            return;
        int idx = _lstOwnerFighters.IndexOf(_curAttacker);
        if (idx < 0)
        {
            _curAttacker = _lstOwnerFighters[0];
            targeter = _enemyFighter;
        }
        else
        {
            if (idx >= _lstOwnerFighters.Count - 1)
            {
                _curAttacker = _enemyFighter;
                targeter = _lstOwnerFighters[0];
            }
            else
            {
                _curAttacker = _lstOwnerFighters[idx + 1];
                targeter = _enemyFighter;
            }
        }
        _curAttacker.DoSkill(targeter);
    }

    private void ClearBattleFighter()
    {
        if (_lstOwnerFighters != null)
        {
            for (int i = 0; i < _lstOwnerFighters.Count; i++)
                _lstOwnerFighters[i].Dispose();
            _lstOwnerFighters.Clear();
            _lstOwnerFighters = null;
        }
        if (_enemyFighter != null)
        {
            _enemyFighter.Dispose();
            _enemyFighter = null;
        }
        _curAttacker = null;
    }

    public void Update()
    {
        if (!_blInited || _blPause)
            return;
        if (_lstOwnerFighters != null)
        {
            for (int i = 0; i < _lstOwnerFighters.Count; i++)
                _lstOwnerFighters[i].Update();
            if (_enemyFighter != null)
                _enemyFighter.Update();
        }
        if (_status == HangupStatus.Attack)
        {
            if (_blInterval)
            {
                _flIntervalTime -= Time.deltaTime;
                if (_flIntervalTime <= 0.01f)
                {
                    _blInterval = false;
                    ChangeAttacker();
                }
            }
            else
            {
                if (_curAttacker != null)
                {
                    if (!_curAttacker.BlIdleStatus)
                        return;
                    _flIntervalTime = 0.8f;
                    _blInterval = true;
                    _curAttacker.ResetStatu();
                }
            }
        }
        else if (_status == HangupStatus.AttackEnd)
        {
            if (_enemyFighter.mBlDeath)
            {
                _status = HangupStatus.CreateInterval;
                if (_enemyFighter != null)
                {
                    _enemyFighter.Dispose();
                    _enemyFighter = null;
                }
            }
        }
        else if (_status == HangupStatus.CreateInterval)
        {
            _flNextBattleIntervalTime -= Time.deltaTime;
            if (_flNextBattleIntervalTime <= 0.01f)
                CreateNewEnemy();
        }
    }

    private bool _blPause;

    public void PasueBattle()
    {
        _blPause = true;
        RoleRTMgr.Instance.Hide(RoleRTType.Hangup);
    }

    public void ContineBattle()
    {
        _blPause = false;
        RoleRTMgr.Instance.ShowRoleRTLogic(RoleRTType.Hangup);
    }

    public void Dispose()
    {
        ClearBattleFighter();
        _scene = null;
        _dataVO = null;
        _status = HangupStatus.None;
        _blInited = false;
        _blPause = true;
    }

    public void ShowHangup(HangDataVO vo)
    {
        _dataVO = vo;
        ClearBattleFighter();
        RoleRTMgr.Instance.ShowRoleRTLogic(RoleRTType.Hangup);
        _blPause = false;
        CreateBattle();
    }

    public void HideHangup()
    {
        ClearBattleFighter();
        _blInited = false;
        _status = HangupStatus.None;
        _blPause = true;
        RoleRTMgr.Instance.Hide(RoleRTType.Hangup);
    }

    public Transform BulletRoot
    {
        get { return _scene.mBulletParent; }
    }
}