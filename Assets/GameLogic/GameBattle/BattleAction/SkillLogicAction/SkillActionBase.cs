using UnityEngine;
using System.Collections.Generic;

public abstract class SkillActionBase : ActionNodeBase
{
    public ActionItemData mActionItemData { get; protected set; }
    protected Fighter _attacker;

    protected AttackNodeStatus _status;

    protected int _skillTotalFrame;

    private FrameTimer _bulletCreateTimer;
    protected FrameTimer _damageTriggerTimer;

    protected List<Fighter> _allDamageFighters;
    protected List<BulletBase> _allBullets;
    protected List<Fighter> _allBuffFighters;
    protected EffectBase _castEffect;

    protected int _damageOffsetFrame = 0;
    private int _behitIndex = 1;
    protected List<FighterDamageDataVO> _curBehitFighters;
    protected List<FighterDamageDataVO> _curComboBehitFighters;
    public SkillActionBase()
    {
        _status = AttackNodeStatus.None;
    }

    #region Init Node

    protected override void OnInitData<T>(T data)
    {
        base.OnInitData(data);
        mActionItemData = data as ActionItemData;
        _behitIndex = 1;
        _curBehitFighters = mActionItemData.GetBehitFighters(_behitIndex);
        _blFirstDamage = true;
        _curComboBehitFighters = mActionItemData.GetComboBehitFighters(_behitIndex);
        _attacker = BattleManager.Instance.mBattleScene.GetFighterBySeatIndex(mActionItemData.mAttacker.mSide, mActionItemData.mAttacker.mSeatIndex);
        ParseData();
        OnStart();
    }

    protected virtual void ParseData()
    {
        //Debuger.LogWarning("ActionNode Type:" + mActionItemData.mSkillConfig.SkillAnimType + ", Attacker id:" + mActionItemData.mAttacker.mSide + "_" + mActionItemData.mAttacker.mSeatIndex + ", cast skillid:" + mActionItemData.mSkillConfig.ID);
        _skillTotalFrame = mActionItemData.mSkillConfig.CastTime;
        _allDamageFighters = new List<Fighter>();
        _allBuffFighters = new List<Fighter>();
        int bulletType = mActionItemData.mSkillConfig.BulletType;
        if (bulletType != (int)BulletType.None && bulletType != (int)BulletType.Shadow)
        {
			_allBullets = new List<BulletBase>();
            _bulletCreateTimer = new FrameTimer(mActionItemData.mSkillConfig.BulletShowTime, CreateBullet);
        }
        else
        {
            string[] hitTimes = mActionItemData.mSkillConfig.HitShowTime.Split(',');
            int frame = int.Parse(hitTimes[0]) - _damageOffsetFrame;
            _damageTriggerTimer = new FrameTimer(frame, DoDamage);
        }
    }
    #endregion

    private int _startFrame;
    protected virtual void OnStartAttack()
    {
        _castEffect = null;
        _status = AttackNodeStatus.Attacking;
        _startFrame = Time.frameCount;
        if (mActionItemData.mSkillConfig.ReverseType == 1)
            _attacker.mUnitRoot.localScale = new Vector3(_attacker.mUnitRoot.localScale.x * -1f, _attacker.mUnitRoot.localScale.y, _attacker.mUnitRoot.localScale.z);
        if (mActionItemData.mSkillConfig.CastAnimBeforeMove == 0)
        {
            if (!string.IsNullOrEmpty(mActionItemData.mSkillConfig.CastAnim))
                _attacker.PlayAction(mActionItemData.mSkillConfig.CastAnim);
            if (!string.IsNullOrWhiteSpace(mActionItemData.mSkillConfig.CastSound))
                SoundMgr.Instance.PlayEffectSound(mActionItemData.mSkillConfig.CastSound, mActionItemData.mSkillConfig.CastSoundDelay, false);
        }
        if (!string.IsNullOrEmpty(mActionItemData.mSkillConfig.SkillCastEffect))
            _castEffect = EffectMgr.Instance.CreateEffect(mActionItemData.mSkillConfig.SkillCastEffect, _attacker, OnCastEffectEnd);
    }

    private void OnCastEffectEnd(EffectBase effect)
    {
        _castEffect = null;
    }

    protected void ResetFighterAction()
    {
        //Debuger.Log("OnActionEnd Type:" + mActionItemData.mSkillConfig.SkillAnimType + ", Attacker id:" + mActionItemData.mAttacker.mSide + "_" + mActionItemData.mAttacker.mSeatIndex + ", cast skillid:" + mActionItemData.mSkillConfig.ID + ", attacker cur hp:" + mActionItemData.mAttacker.mCurHp);
        if (mActionItemData.mSkillConfig.ReverseType == 1)
            _attacker.mUnitRoot.localScale = new Vector3(_attacker.mUnitRoot.localScale.x * -1f, _attacker.mUnitRoot.localScale.y, _attacker.mUnitRoot.localScale.z);
        if(mActionItemData.mAttacker.mCurHp > 0)
            _attacker.PlayAction(ActionName.Idle, true);
    }

    protected virtual void OnAttackEnd()
    {
        ResetFighterAction();
        OnActionEnd();
    }

    protected virtual void OnActionEnd()
    {
        _attacker.ChangeSortLayerOffest(RenderLayerOffset.None);
        if (mActionItemData.mAttacker.mCurHp < 0)
            _attacker.DoDeath();
        if (!AllActionEnd)
        {
            _status = AttackNodeStatus.WaitingEndTime;
        }    
        else
        {
            GameEventMgr.Instance.mBattleDispatcher.DispathEvent<SkillActionBase>(BattleEvent.BattleStepEnd, this);
        }    
    }

    protected override void OnUpdate()
    {
        if (_status == AttackNodeStatus.Attacking)
        {
            if (_bulletCreateTimer != null && _bulletCreateTimer.mBlEnable)
                _bulletCreateTimer.Update();
            if (_damageTriggerTimer != null && _damageTriggerTimer.mBlEnable)
                _damageTriggerTimer.Update();
            _skillTotalFrame -= (int)Time.timeScale;
            if (_skillTotalFrame <= 0)
                OnAttackEnd();
        }
        else if (_status == AttackNodeStatus.WaitingEndTime)
        {
            if (!AllActionEnd)
                return;
            GameEventMgr.Instance.mBattleDispatcher.DispathEvent<SkillActionBase>(BattleEvent.BattleStepEnd, this);
        }
    }

    protected virtual bool AllActionEnd
    {
        get
        {
            if (_allDamageFighters != null)
            {
                for (int i = 0; i < _allDamageFighters.Count; i++)
                {
                    if (!_allDamageFighters[i].FighterAllEffectFinish)
                        return false;
                }
            }
            if (_allBuffFighters != null)
            {
                for (int i = 0; i < _allBuffFighters.Count; i++)
                {
                    if (!_allBuffFighters[i].FighterAllEffectFinish)
                        return false;
                }
            }
            if (_allBullets != null)
            {
                for (int i = 0; i < _allBullets.Count; i++)
                {
                    if (!_allBullets[i].AllEffectEnd)
                        return false;
                }
            }

            if (_lstSoummonFighters != null && _lstSoummonFighters.Count > 0)
            {
                for (int i = 0; i < _lstSoummonFighters.Count; i++)
                    if (!_lstSoummonFighters[i].FighterAllEffectFinish)
                        return false;
            }
            if (_castEffect != null)
                return false;
            return true;
        }
    }

    private List<Fighter> _lstSoummonFighters;
    private bool _blFirstDamage = true;
    private void DoSummonAndBuffLogic()
    {
        if (!_blFirstDamage)
            return;
        _blFirstDamage = false;
        if (_curComboBehitFighters != null)
        {
            Fighter fighter;
            for (int i = 0; i < _curComboBehitFighters.Count; i++)
            {
                fighter = BattleManager.Instance.mBattleScene.GetFighterBySeatIndex(_curComboBehitFighters[i].mSide, _curComboBehitFighters[i].mSeatIndex);
                fighter.DoDamage(_curComboBehitFighters[i]);
                _allDamageFighters.Add(fighter);
            }
        }
        if (mActionItemData.mBlSummon && mActionItemData.mSummonFighters != null)
        {
            string bornEffect = null;
            if (mActionItemData.mSkillConfig.BulletType == (int)BulletType.Shadow)
                bornEffect = mActionItemData.mSkillConfig.BulletHitEffect;
            LogHelper.Log("create soummon fighter");
            _lstSoummonFighters = BattleManager.Instance.mBattleScene.CreateFighter(mActionItemData.mSummonFighters, bornEffect);
        }
        if (_attacker != null)
        {
            if (mActionItemData.mAttacker.mDamage != 0)
            {
                mActionItemData.mAttacker.mBlInjury = true;
                _attacker.DoDamage(mActionItemData.mAttacker);
                _allDamageFighters.Add(_attacker);
            }
            else 
            {
                //Debuger.Log("id:" + mActionItemData.mAttacker.mSide + "-" + mActionItemData.mAttacker.mSeatIndex + ", old energy:" + _attacker.mData.mEnergy + ", new energy:" + mActionItemData.mAttacker.mEnergy);
                _attacker.RefreshHpAndEnery(mActionItemData.mAttacker.mCurHp, mActionItemData.mAttacker.mEnergy, mActionItemData.mAttacker.mShield);
            }
        }
        //TODO: buff logic need add..
        if (mActionItemData.mSkillConfig.ShockScreen == 1)
            BattleManager.Instance.mBattleScene.ShakeCamera();
        if (mActionItemData.mAddBuffs != null)
        {
            Fighter fighter;
            for (int i = 0; i < mActionItemData.mAddBuffs.Count; i++)
            {
                fighter = BattleManager.Instance.mBattleScene.GetFighterBySeatIndex(mActionItemData.mAddBuffs[i].mSide, mActionItemData.mAddBuffs[i].mSeatIndex);
                if (fighter == null)
                {
                    LogHelper.LogError("[SkillActionBase.DoSummonAndBuffLogic() => skill addBuff, but fighter not found, pos:" + mActionItemData.mAddBuffs[i].mSide + "-" + mActionItemData.mAddBuffs[i].mSeatIndex + "]");
                    continue;
                }
                fighter.AddBuff(mActionItemData.mAddBuffs[i]);
                _allBuffFighters.Add(fighter);
            }
        }

        if (mActionItemData.mRemoveBuffs != null)
        {
            Fighter fighter;
            for (int i = 0; i < mActionItemData.mRemoveBuffs.Count; i++)
            {
                fighter = BattleManager.Instance.mBattleScene.GetFighterBySeatIndex(mActionItemData.mRemoveBuffs[i].mSide, mActionItemData.mRemoveBuffs[i].mSeatIndex);
                if (fighter == null)
                {
                    LogHelper.LogError("[SkillActionBase.DoSummonAndBuffLogic() => skill remove buff, but fighter not found, pos:" + mActionItemData.mAddBuffs[i].mSide + "-" + mActionItemData.mAddBuffs[i].mSeatIndex + "]");
                    continue;
                }
                fighter.RemoveBuff(mActionItemData.mRemoveBuffs[i]);
            }
        }
        
    }

    protected void DoDamage()
    {
        if (_curBehitFighters != null)
        {
            Fighter targeter = null;
            FighterDamageDataVO targetData = null;
            for (int i = 0; i < _curBehitFighters.Count; i++)
            {
                targetData = _curBehitFighters[i];
                targeter = BattleManager.Instance.mBattleScene.GetFighterBySeatIndex(targetData.mSide, targetData.mSeatIndex);
                if (targeter == null)
                    continue;
                targeter.DoDamage(targetData);
                _allDamageFighters.Add(targeter);
            }
        }
        DoSummonAndBuffLogic();
    }

    private void CreateBullet()
    {
		GameEventMgr.Instance.mBattleDispatcher.AddEvent(BattleEvent.BulletHitFirstDamage, OnDoCommonLogic);
        BulletDataVO bulletVO;
        if (mActionItemData.mSkillConfig.BulletTargetType == 0)
        {
            bulletVO = new BulletDataVO(_attacker, mActionItemData.mSkillConfig);
            bulletVO.SetData(_curBehitFighters, GetBulletTargetPos());
            _allBullets.Add(BulletMgr.Instance.CreateBullet(bulletVO));
        }
        else
        {
            for (int i = 0; i < _curBehitFighters.Count; i++)
            {
                bulletVO = new BulletDataVO(_attacker, mActionItemData.mSkillConfig);
                bulletVO.SetData(_curBehitFighters[i]);
                _allBullets.Add(BulletMgr.Instance.CreateBullet(bulletVO));
            }
        }
        _behitIndex++; ;
        _curBehitFighters = mActionItemData.GetBehitFighters(_behitIndex);
        _curComboBehitFighters = mActionItemData.GetComboBehitFighters(_behitIndex);
    }

    private void OnDoCommonLogic()
    {
        GameEventMgr.Instance.mBattleDispatcher.RemoveEvent(BattleEvent.BulletHitFirstDamage, OnDoCommonLogic);
        DoSummonAndBuffLogic();
    }

    protected override void OnDispose()
    {
        GameEventMgr.Instance.mBattleDispatcher.RemoveEvent(BattleEvent.BulletHitFirstDamage, OnDoCommonLogic);
        base.OnDispose();
        _castEffect = null;
        _status = AttackNodeStatus.None;
        if (_bulletCreateTimer != null)
        {
            _bulletCreateTimer.Dispose();
            _bulletCreateTimer = null;
        }
        if (_damageTriggerTimer != null)
        {
            _damageTriggerTimer.Dispose();
            _damageTriggerTimer = null;
        }
        if (_allBullets != null)
        {
            for(int i = _allBullets.Count - 1; i >= 0; i--)
                BulletMgr.Instance.RemoveBullet(_allBullets[i]);
            _allBullets.Clear();
            _allBullets = null;
        }
        if (_allDamageFighters != null)
        {
            _allDamageFighters.Clear();
            _allDamageFighters = null;
        }
        if (_allBuffFighters != null)
        {
            _allBuffFighters.Clear();
            _allBuffFighters = null;
        }

        if (_lstSoummonFighters != null)
        {
            _lstSoummonFighters.Clear();
            _lstSoummonFighters = null;
        }
        if(_attacker != null)
        {
            _attacker.PlayAction(ActionName.Idle, true);
			_attacker = null;
        }
        mActionItemData = null;
        _curBehitFighters = null;
        _curComboBehitFighters = null;
    }

    protected virtual Vector3 GetBulletTargetPos()
    {
        Fighter targeter;
        Vector3 targetPos = Vector3.zero;
        switch (mActionItemData.mSkillConfig.RangeType)
        {
            case SkillRangeType.Single:
            case SkillRangeType.Row:
            case SkillRangeType.Multiple:
            case SkillRangeType.CrossOne:
            case SkillRangeType.CrossTwo:
                targeter = BattleManager.Instance.mBattleScene.GetFighterBySeatIndex(_curBehitFighters[0].mSide, _curBehitFighters[0].mSeatIndex);
                targetPos = targeter.mUnitRoot.position;
                break;
            case SkillRangeType.Col:
            case SkillRangeType.All:
                targetPos = BattleManager.Instance.mBattleScene.GetSkillTargetPosByRangeType(_curBehitFighters[0].mSide, _curBehitFighters[0].mSeatIndex, mActionItemData.mSkillConfig.RangeType);
                break;
        }
        return targetPos;
    }

    protected virtual Vector3 GetTargetPos()
    {
        Fighter targeter;
        Vector3 targetPos = Vector3.zero;
        switch (mActionItemData.mSkillConfig.RangeType)
        {
            case SkillRangeType.Single:
            case SkillRangeType.Row:
            case SkillRangeType.Multiple:
            case SkillRangeType.CrossOne:
            case SkillRangeType.CrossTwo:
                targeter = BattleManager.Instance.mBattleScene.GetFighterBySeatIndex(_curBehitFighters[0].mSide, _curBehitFighters[0].mSeatIndex);
                float flInterval = _attacker.mDefaultPos.x < 0.0f ? -(_attacker.mflWidth / 2f + targeter.mflWidth / 2f) : (_attacker.mflWidth / 2f + targeter.mflWidth / 2f);
                if (mActionItemData.mSkillConfig.ReverseType == 1)
                    flInterval *= -1f;
                targetPos = new Vector3(targeter.mDefaultPos.x + flInterval, targeter.mDefaultPos.y, targeter.mDefaultPos.z);
                break;
            case SkillRangeType.Col:
            case SkillRangeType.All:
                targetPos = BattleManager.Instance.mBattleScene.GetSkillTargetPosByRangeType(_curBehitFighters[0].mSide, _curBehitFighters[0].mSeatIndex, mActionItemData.mSkillConfig.RangeType);
                break;
        }
        return targetPos;
    }

    protected abstract void OnStart();
}