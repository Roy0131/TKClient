using UnityEngine;
using Spine;
using Spine.Unity;
using System.Collections.Generic;
using Framework.UI;
using System;

public enum HangupFighterStatus
{
    None,
    MoveToTarget,
    CastSkill,
    MoveBack,
    BeHit,
    Death,
}

public class HangupFighter : AnimatorFighter
{
    private CardConfig _cardConfig;
    private SkillConfig _skillConfig;
    private HangupFighter _targeter;

    private bool _blMove = false;

    public HangupFighterStatus mFighterStatus { get; private set; }
    private float _flMoveTime;
    private bool _blOwner;

    //private HangupBullet _bullet;
    private List<HangupBullet> _lstBullets;
    private FrameTicker _beHitEffectTicker;

    private int _skillFrame;
    private FrameTimer _damageTimer;
    private FrameTimer _bulletTimer;

    public bool mBlDeath { get; private set; }

    public int mBeHitCount { get; set; } = -1;

    private List<GameObject> _lstEffectObject = new List<GameObject>();

    public HangupFighter(bool blOwner)
        :base(BattleUnitType.HangupFighter)
    {
        _blOwner = blOwner;
        mFighterStatus = HangupFighterStatus.None;
        mBlDeath = false;
        mBeHitCount = UnityEngine.Random.Range(4, 7);
        _lstBullets = new List<HangupBullet>();
    }

    public bool BlHero
    {
        get { return _blOwner; }
    }

    protected override void OnInitData<T>(T data)
    {
        _cardConfig = data as CardConfig;
        _modelName = _cardConfig.Model;
        _skillConfig = GameConfigMgr.Instance.GetSkillConfig(_cardConfig.NormalSkillID);
        ModelConfig modelConfig = GameConfigMgr.Instance.GetModelConfig(_cardConfig.Model);
        mflWidth = (float)modelConfig.Width / 100f;
        base.OnInitData(data);
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _beHitEffect = new FighterMaterialEffect(_modelObject.transform);
    }

    protected override void LoadModel()
    {
        if (string.IsNullOrEmpty(_modelName))
            return;
        _container.name = _modelName;
        RoleResPool.Instance.GetRole(_modelName, OnModelLoaded);
    }

    public override void AddToStage(Transform parent)
    {
        base.AddToStage(parent);
        mUnitRoot = parent;
        mDefaultPos = parent.localPosition;
        _container.layer = GameLayer.ModeUILayer;
    }

	protected override void OnPlayEnd(TrackEntry entry)
	{
        base.OnPlayEnd(entry);
        if (_animator.AnimationName == ActionName.Idle)
            return;
        if (_animator.AnimationName == ActionName.Death)
        {
            mBlDeath = true;
            return;
        }    
        if (entry.Animation.Name == _animator.AnimationName)
        {
            PlayAction(ActionName.Idle, true);
            if (_beHitEffect != null)
                _beHitEffect.ResetBeHitEffect();
        }   
	}


    protected override void ParseAnimator()
    {
        _animator = _modelObject.GetComponent<SkeletonAnimation>();
        if (_animator == null)
            return;
        _animator.state.Complete += OnPlayEnd;
    }

	protected override void OnUpdate()
    {
        base.OnUpdate();
        if (_lstBullets != null && _lstBullets.Count > 0)
        {
            for (int i = 0; i < _lstBullets.Count; i++)
                _lstBullets[i].Update();
        }
        if (_beHitEffectTicker != null)
            _beHitEffectTicker.Update();
        if (mFighterStatus == HangupFighterStatus.None)
            return;
        switch (mFighterStatus)
        {
            case HangupFighterStatus.MoveToTarget:
            case HangupFighterStatus.MoveBack:
                DoMove();
                break;
            case HangupFighterStatus.CastSkill:
                _skillFrame--;
                if (_skillFrame <= 0)
                    DoSKillActionEnd();
                if (_damageTimer != null && _damageTimer.mBlEnable)
                    _damageTimer.Update();
                if (_bulletTimer != null && _bulletTimer.mBlEnable)
                    _bulletTimer.Update();
                break;
        }
    }

    #region fighter move logic;
    private Vector3 _direction;
    private float _leftDistance;
    private float _flSpeed;
    private float _flDeltaDistance = 0f;
    private Vector3 _curPos;
    private Vector3 _targetPos;
    float _flTmpDistance = 0f;

    private void MoveEnd()
    {
        if (mFighterStatus == HangupFighterStatus.MoveToTarget)
            OnStartSkill();
        else
            OnSkillEnd();
    }

    private void DoMove()
    {
        _flDeltaDistance = Time.deltaTime * _flSpeed;
        if (_flDeltaDistance > _leftDistance)
        {
            mUnitRoot.localPosition = _targetPos;
            MoveEnd();
            return;
        }
        else
        {
            _flTmpDistance = Vector3.Distance(_curPos, _targetPos);
            if (_flTmpDistance <= _flSpeed * Time.deltaTime)
            {
                _curPos = _targetPos;
                mUnitRoot.localPosition = _curPos;
                MoveEnd();
                return;
            }
            _curPos += _direction * _flDeltaDistance;
            _leftDistance -= _flDeltaDistance;
        }
        mUnitRoot.localPosition = _curPos;
    }

    private void Move(Vector3 targetPos)
    {
        //Debuger.LogWarning("move TargetPos:" + targetPos + ", defalutPos:" + mDefaultPos);
        _targetPos = targetPos;
        _direction = (targetPos - mUnitRoot.localPosition).normalized;
        _leftDistance = Vector3.Distance(targetPos, mUnitRoot.localPosition);
        _curPos = mUnitRoot.localPosition;
        _flSpeed = _leftDistance / _flMoveTime;
        _flDeltaDistance = Time.deltaTime * _flSpeed;
    }

    #endregion

    private void OnSkillEnd()
    {
        SortLayer = 0;
        _targeter = null;
        PlayAction(ActionName.Idle, true);
        mFighterStatus = HangupFighterStatus.None;
    }
    
    private void DoSKillActionEnd()
    {
        if (_blMove)
        {
            mFighterStatus = HangupFighterStatus.MoveBack;
            Move(mDefaultPos);
        }
        else
        {
            OnSkillEnd();
        }
    }

    public void DoSkill(HangupFighter targeter)
    {
        SortLayer = -1;
        _targeter = targeter;
        _blMove = _skillConfig.SkillAnimType % 2 != 0;
        _flMoveTime = 0.2f;
        _skillFrame = _skillConfig.CastTime;
        if (_blMove)
        {
            mFighterStatus = HangupFighterStatus.MoveToTarget;
            float flInterval = _blOwner ? -(mflWidth / 2f + _targeter.mflWidth / 2f) : mflWidth / 2f + _targeter.mflWidth / 2f;
            if (_skillConfig.ReverseType == 1)
                flInterval *= -1f;
            Vector3 targetPos = new Vector3(targeter.mDefaultPos.x + flInterval, targeter.mDefaultPos.y, targeter.mDefaultPos.z);
            if (_skillConfig.CastAnimBeforeMove != 0)
                PlayAction(_skillConfig.CastAnim);
            Move(targetPos);
        }
        else
        {
            OnStartSkill();
        }
    }

    private void OnStartSkill()
    {
        mFighterStatus = HangupFighterStatus.CastSkill;
        if (_skillConfig.CastAnimBeforeMove == 0)
            PlayAction(_skillConfig.CastAnim);
        if (_skillConfig.BulletType != (int)BulletType.None)
        {
            //BulletLogic
            if (_bulletTimer == null)
                _bulletTimer = new FrameTimer(_skillConfig.BulletShowTime, CreateBullet);
            else
                _bulletTimer.Reset(_skillConfig.BulletShowTime);
        }
        else
        {
            string[] hitTimes = _skillConfig.HitShowTime.Split(',');
            int frame = int.Parse(hitTimes[0]);
            if (_damageTimer == null)
                _damageTimer = new FrameTimer(frame, OnShowDamage);
            else
                _damageTimer.Reset(frame);
        }
        if (!string.IsNullOrEmpty(_skillConfig.CastSound))
            SoundMgr.Instance.PlayEffectSound(_skillConfig.CastSound, _skillConfig.CastSoundDelay);
    }

    private void OnShowDamage()
    {
        _targeter.DoBehit(_skillConfig.ChaHitEffect, _skillConfig.ChaHitSound);
    }

    public void DoBehit(string beHitEffectName, string soundName)
    {
        if (!_blOwner)
        {
            mBeHitCount--;
            if(mBeHitCount <= 0)
            {
                PlayAction(ActionName.Death);
            }
            else
            {
                PlayAction(ActionName.Hit);
            }
        }else
        {
            PlayAction(ActionName.Hit);
        }

        _beHitEffect.StartEffect();
        GameObject effectObj = null;
        Action OnBeHitEffectEnd = () =>
        {
            _lstEffectObject.Remove(effectObj);
            GameObject.Destroy(effectObj);
            effectObj = null;
            if(_beHitEffectTicker != null)
            {        
				_beHitEffectTicker.Dispose();
				_beHitEffectTicker = null;
            }
        };

        if(!string.IsNullOrEmpty(beHitEffectName))
        {
            EffectConfig cfg = GameConfigMgr.Instance.GetEffectConfig(beHitEffectName);

            Action<GameObject> OnEffectLoaded = (effObject) =>
            {
                effectObj = effObject;
                effectObj.transform.SetParent(HangUpMgr.Instance.BulletRoot, false);
                effectObj.transform.position = mHitWorldPosition;
                effectObj.layer = GameLayer.ModeUILayer;

                Transform objTransform = effectObj.transform;
                Renderer[] rootRenders = objTransform.GetComponents<Renderer>();
                Renderer[] childRenders = objTransform.GetComponentsInChildren<Renderer>();

                int rootLen = rootRenders == null ? 0 : rootRenders.Length;
                int childLen = childRenders == null ? 0 : childRenders.Length;

                Renderer[] renders = new Renderer[rootLen + childLen];
                if (rootLen > 0)
                    rootRenders.CopyTo(renders, 0);
                if (childLen > 0)
                    childRenders.CopyTo(renders, rootLen);

                for (int i = 0; i < renders.Length; i++)
                {
                    renders[i].gameObject.layer = GameLayer.ModeUILayer;
                    renders[i].sortingOrder = _sortLayer + 1;
                }

                _lstEffectObject.Add(effectObj);

                _beHitEffectTicker = new FrameTicker((float)cfg.Duration / 1000f, OnBeHitEffectEnd);
            };
            GameResMgr.Instance.LoadEffect(cfg.Name, OnEffectLoaded);
            
        }
        if (!string.IsNullOrEmpty(soundName))
            SoundMgr.Instance.PlayEffectSound(soundName);
    }

    private void CreateBullet()
    {
        HangupBullet bullet = new HangupBullet();
        bullet.targeter = _targeter;
        bullet.attacker = this;
        bullet.InitData(_skillConfig);
        bullet.AddToStage(HangUpMgr.Instance.BulletRoot);
        _lstBullets.Add(bullet);
    }

	protected override void OnDisposeModel()
	{
        if (_modelObject != null)
            RoleResPool.Instance.ReturnRoleObject(_modelName, _modelObject);
        if (_container != null)
            GameObject.Destroy(_container);
        _container = null;
        _modelObject = null;
	}

	protected override void OnDispose()
    {
        if (_damageTimer != null)
        {
            _damageTimer.Dispose();
            _damageTimer = null;
        }

        if (_bulletTimer != null)
        {
            _bulletTimer.Dispose();
            _bulletTimer = null;
        }

        if (_beHitEffectTicker != null)
        {
            _beHitEffectTicker.Dispose();
            _beHitEffectTicker = null;
        }

        if (_lstBullets != null)
        {
            for (int i = 0; i < _lstBullets.Count; i++)
                _lstBullets[i].Dispose();
            _lstBullets.Clear();
            _lstBullets = null;
        }

        if (_beHitEffect != null)
        {
            _beHitEffect.Dispose();
            _beHitEffect = null;
        }

        if (_lstEffectObject != null)
        {
            for (int i = 0; i < _lstEffectObject.Count; i++)
                GameObject.Destroy(_lstEffectObject[i]);
            _lstEffectObject.Clear();
            _lstEffectObject = null;
        }

        _cardConfig = null;
        _targeter = null;
        _skillConfig = null;
        mFighterStatus = HangupFighterStatus.None;
        _animator.state.SetAnimation(0, ActionName.Idle, true);
        base.OnDispose();
    }

    public void ResetStatu()
    {
        if (_lstBullets != null)
        {
            for (int i = 0; i < _lstBullets.Count; i++)
                _lstBullets[i].Dispose();
            _lstBullets.Clear();
        }
    }

    public bool BlIdleStatus
    {
        get
        {
            if (_lstBullets != null)
            {
                for (int i = 0; i < _lstBullets.Count; i++)
                {
                    if (!_lstBullets[i].BulletFinish)
                        return false;
                }
            }
            //if (_bullet != null && !_bullet.BulletFinish)
            //    return false;
            if (mFighterStatus != HangupFighterStatus.None)
                return false;
            if (_animator.AnimationName != ActionName.Idle)
                return false;
            if (_targeter != null && !_targeter.BlIdleStatus)
                return false;
            if (_beHitEffectTicker != null)
                return false;
            return true;
        }
    }
}