using System;
using UnityEngine;
using Framework.Core;

public class RTLogicBase : IDispose
{
    protected GameObject _rtRootObject;
    
    public RoleRTType mRTType { get; protected set; }
    protected bool _blShowHpbar;
    public RTLogicBase(RoleRTType type)
    {
        mRTType = type;
    }

    public GameObject RTRootObject
    {
        get { return _rtRootObject; }
    }

    public void Init(GameObject gameObject)
    {
        _rtRootObject = gameObject;
        OnInit();
    }

    protected virtual void OnInit()
    {

    }

    public virtual void Show<T>(T data, bool showHpBar = false)
    {
        _blShowHpbar = showHpBar;
        _rtRootObject.SetActive(true);
    }

    public virtual void Hide()
    {
        _rtRootObject.SetActive(false);
    }


    public virtual void Dispose()
    {
        _rtRootObject = null;
    }
}
