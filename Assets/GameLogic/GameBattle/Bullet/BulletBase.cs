using UnityEngine;
using System.Collections.Generic;
public abstract class BulletBase : SceneRenderUnit
{
    public bool mblComplete { get; protected set; }
    public BulletDataVO mBulletDataVO { get; protected set; }

    protected List<Fighter> _lstTargeters;
    protected List<Fighter> _lstShowingBloodFighters;
    
    protected Vector3 _direction;
    protected float _flBulletSpeed;
    protected List<EffectBase> _lstHitEffects;

    protected int _bulletAliveFrame;

    public BulletBase()
        : base(BattleUnitType.Bullet)
    {
        mblComplete = false;
        _lstHitEffects = new List<EffectBase>();
    }

    protected override void OnInitData<T>(T data)
    {
        mBulletDataVO = data as BulletDataVO;
        _modelName = mBulletDataVO.mSkillConfig.BulletAnim.Trim();
        _bulletAliveFrame = mBulletDataVO.mSkillConfig.BulletContinueTime;
        _flBulletSpeed = mBulletDataVO.mSkillConfig.BulletSpeed;
        ChangeSortLayerOffest(RenderLayerOffset.Bullet);
        base.OnInitData(data);
        mUnitRoot = _modelObject.transform;
        _lstTargeters = new List<Fighter>();
        _lstShowingBloodFighters = new List<Fighter>();
    }


    protected virtual void OnStart()
    {
        _lstTargeters = new List<Fighter>();
        if (mBulletDataVO.mlstTargeters != null)
        {
            for (int i = 0; i < mBulletDataVO.mlstTargeters.Count; i++)
            {
                Fighter targeter = BattleManager.Instance.mBattleScene.GetFighterBySeatIndex(mBulletDataVO.mlstTargeters[i].mSide, mBulletDataVO.mlstTargeters[i].mSeatIndex);
                if (targeter == null)
                    continue;
                _lstTargeters.Add(targeter);
            }
        }
    }

    public override void AddToStage(Transform parent)
    {
        base.AddToStage(parent);
        UpdatePosition(mBulletDataVO.mAttacker.FireDummyWorldPosition);
        OnStart();
    }

    protected override void OnDispose()
    {
        base.OnDispose();
        if (mBulletDataVO != null)
        {
            mBulletDataVO.Dispose();
            mBulletDataVO = null;
        }

        if (_lstTargeters != null)
        {
            _lstTargeters.Clear();
            _lstTargeters = null;
        }

        if (_lstShowingBloodFighters != null)
        {
            _lstShowingBloodFighters.Clear();
            _lstShowingBloodFighters = null;
        }

        if (_lstHitEffects != null)
        {
            _lstHitEffects.Clear();
            _lstHitEffects = null;
        }
    }


    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (mblComplete)
            return;
        if (mBulletDataVO.mSkillConfig.BulletContinueTime != 0)
        {
            if (_bulletAliveFrame <= 0)
            {
                OnEnd();
                return;
            }
            _bulletAliveFrame --;
        }
    }

    protected virtual void OnEnd()
    {
        //_blInitialed = false;
        mblComplete = true;
        if (_modelObject != null)
            _modelObject.SetActive(false);
    }

	public override void UpdatePosition(Vector3 pos)
	{
        mUnitRoot.position = pos;
        SortLayer = (int)(pos.y * 10);
	}

	public virtual bool AllEffectEnd
    {
        get
        {
            if (!mblComplete)
                return false;
            if (_lstTargeters != null && _lstTargeters.Count > 0)
                return false;
            if (_lstShowingBloodFighters != null && _lstShowingBloodFighters.Count > 0)
            {
                for (int i = 0; i < _lstShowingBloodFighters.Count; i++)
                {
                    if (!_lstShowingBloodFighters[i].FighterAllEffectFinish)
                        return false;
                }
            }
            if (_lstHitEffects.Count > 0)
            {
                for (int i = 0; i < _lstHitEffects.Count; i++)
                {
                    if (!_lstHitEffects[i].mBlEffectEnd)
                        return false;
                }
            }    
            return true;
        }
    }

    protected virtual void CreateBulletHitEffect()
    {
        if (!string.IsNullOrWhiteSpace(mBulletDataVO.mSkillConfig.BulletHitSound))
            SoundMgr.Instance.PlayEffectSound(mBulletDataVO.mSkillConfig.BulletHitSound);
        if (!string.IsNullOrEmpty(mBulletDataVO.mSkillConfig.BulletHitEffect))
        {
            EffectBase effect = EffectMgr.Instance.CreateEffect(mBulletDataVO.mSkillConfig.BulletHitEffect, GetBulletEffectPos(), OnEffectEnd);
            _lstHitEffects.Add(effect);
        }
    }

    protected virtual Vector3 GetBulletEffectPos()
    {
        return mBulletDataVO.mTargetPos;
    }

    protected virtual void OnEffectEnd(EffectBase effect)
    {
        _lstHitEffects.Remove(effect);
    }
}