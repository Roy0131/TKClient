
using UnityEngine;
using System;

public class MoveBulletBase : BulletBase
{
    protected float _flFrameSpeed;
    protected float _flDistance;
    protected Vector3 _targetPos;
    protected bool _blMoveEnd;
    protected override void OnStart()
    {
        base.OnStart();
        _targetPos = GetTargetPos();
        _direction = (_targetPos - mBulletDataVO.mAttacker.FireDummyWorldPosition).normalized;

        float temp = Mathf.Atan2(_direction.y, _direction.x);
        float flDegress = temp * 180f / (float)Math.PI;
        mUnitRoot.transform.localEulerAngles = Vector3.zero;
        mUnitRoot.transform.Rotate(0f, 0f, flDegress);
        _layerOffest = RenderLayerOffset.Max;
        _blMoveEnd = false;
    }

    protected virtual Vector3 GetTargetPos()
    {
        return mBulletDataVO.mTargetPos;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (_blMoveEnd)
            return;
        DoMove();
    }

    protected virtual void DoMove()
    {
        _flFrameSpeed = Time.deltaTime * _flBulletSpeed;
        UpdatePosition(mUnitRoot.position + _direction * _flFrameSpeed);
        CheckMoveEnd();
    }

    protected virtual void CheckMoveEnd()
    {
        _flDistance = Vector3.Distance(_targetPos, mUnitRoot.position);
        if (_flDistance <= _flFrameSpeed)
            OnMoveEnd();
    }

    protected virtual void OnMoveEnd()
    {
        _blMoveEnd = true;
        mUnitRoot.position = _targetPos;
        if (mBulletDataVO.mSkillConfig.BulletContinueTime == 0)
            OnEnd();
    }
}