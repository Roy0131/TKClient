using System;
using UnityEngine;
using Framework.Core;

public class MoveActionDataVO : IDispose
{
    public SceneRenderUnit mMoveUnit { get; private set; }
    public Vector3 mTargetPos { get; private set; }
    public int mDelayFrame { get; private set; }
    public int mMoveFrame { get; private set; }
    public MoveActionDataVO()
    {
        
    }

    public void InitData(SceneRenderUnit mover, Vector3 targetPos, int moveFrame, int delayFrame = 0)
    {
        mMoveUnit = mover;
        mTargetPos = targetPos;
        mDelayFrame = delayFrame;
        mMoveFrame = moveFrame;
    }

    public void Dispose()
    {
        mMoveUnit = null;
    }
}

public class MoveAction : UpdateNode
{
    public bool mblMoveEnd { get; private set; }

    private MoveActionDataVO _dataVO;
    private Vector3 _curPos;
    private Vector3 _direction;
    private float _leftDistance;
    private float _flSpeed;
    private float _flDeltaDistance = 0f;
    private int _delayFrame;

    protected override void OnInitData<T>(T data)
    {
        _dataVO = data as MoveActionDataVO;
        _curPos = _dataVO.mMoveUnit.mUnitRoot.localPosition;
        _direction = (_dataVO.mTargetPos - _curPos).normalized;
        _leftDistance = Vector3.Distance(_dataVO.mTargetPos, _curPos);
        _flSpeed = _leftDistance / ((float)_dataVO.mMoveFrame * Time.deltaTime);
        _blInitialed = false;
        _flDeltaDistance = Time.deltaTime * _flSpeed;
        mblMoveEnd = false;
        _blInitialed = true;
        _delayFrame = _dataVO.mDelayFrame;
    }

    float _flTmpDistance = 0f;
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (mblMoveEnd)
            return;
        if (_delayFrame > 0)
        {
            _delayFrame -= (int)Time.timeScale;
        }
        else
        {
            _flDeltaDistance = Time.deltaTime * _flSpeed * Time.timeScale;
            //Debuger.LogError("move:" + _flDeltaDistance + ", frame:" + frame);
            if (_flDeltaDistance > _leftDistance)
            {
                _curPos = _dataVO.mTargetPos;
                mblMoveEnd = true;
            }
            else
            {
                _flTmpDistance = Vector3.Distance(_curPos, _dataVO.mTargetPos);
                if (_flTmpDistance <= _flSpeed * Time.deltaTime)
                {
                    _curPos = _dataVO.mTargetPos;
                    mblMoveEnd = true;
                    _dataVO.mMoveUnit.UpdatePosition(_curPos);
                    return;
                }
                _curPos += _direction * _flDeltaDistance;
                _leftDistance -= _flDeltaDistance;
            }
            _dataVO.mMoveUnit.UpdatePosition(_curPos);
        }
    }

    protected override void OnDispose()
    {
        base.OnDispose();
        if(_dataVO != null)
        {
            _dataVO.Dispose();
            _dataVO = null;
        }
        mblMoveEnd = true;
    }
}