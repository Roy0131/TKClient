using System;
using UnityEngine;
using Framework.Core;

public class DamageFrameTicker : IDispose
{
    //private float _flTime;
    private Action<FighterDamageDataVO> _method;
    private FighterDamageDataVO _vo;
    public bool mBlEnable { get; private set; }
    private int _frame;
    public DamageFrameTicker(FighterDamageDataVO vo, Action<FighterDamageDataVO> method)
    {
        _vo = vo;
        _frame = vo.NextIntervalTime;
        _method = method;
        mBlEnable = false;
    }

    public void Update()
    {
        if (mBlEnable)
            return;
        if(_frame <= 0)
        {
            if (_method != null)
            {
                _method.Invoke(_vo);
                if (_vo.BlDamageEnd)
                {
                    mBlEnable = true;
                }
                else
                {
                    _frame = _vo.NextIntervalTime;
                }
            }
        }
		_frame -= (int)Time.timeScale;
    }

    public void Dispose()
    {
        _method = null;
        _vo = null;
    }
}

public class FrameTicker : IDispose
{
    public bool mBlEnable { get; private set; }

    private float _flTime;
    private Action _method;
    private float _flCurTime;

    public FrameTicker(float time, Action method)
    {
        _flTime = time;
        _flCurTime = time;
        _method = method;
        mBlEnable = true;
    }

    public void Update()
    {
        if (!mBlEnable)
            return;
        if (_flCurTime <= 0.01f)
        {
            mBlEnable = false;
            if (_method != null)
                _method.Invoke();
            return;
        }
        _flCurTime -= Time.deltaTime;// * Time.timeScale;
    }

    public void Reset()
    {
        _flCurTime = _flTime;
        mBlEnable = true;
    }

    public void Dispose()
    {
        mBlEnable = false;
        _method = null;
    }
}