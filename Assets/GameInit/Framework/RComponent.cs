#region << 版 本 注 释 >>
/*
	 * ========================================================================
     * Copyright Notice  2016-2016 moyutec.com All rights reserved .
     * ========================================================================
     * 文件名	： RComponent 
     * 创建人	： roy
     * 创建时间	： 2016/12/19 10:20:17 
     * 描述  	： 游戏组件基类，封装接口
*/
#endregion

using System;
using UnityEngine;

public class RComponent : MonoBehaviour, IDisposable
{
    #region monobehaviour原生接口
    void Awake()
    {
        OnAwake();
    }

    void Start()
    {
        OnStart();
    }

    void Update()
    {
        OnUpdate();
    }

    void FixedUpdate()
    {
        OnFixedUpdate();
    }

    void OnDestroy()
    {
        Dispose();
    }
    #endregion

    protected Transform _transform;

    protected virtual void OnAwake()
    {
        _transform = this.transform;
    }

    protected virtual void OnStart()
    {

    }

    protected virtual void OnUpdate()
    {

    }

    protected virtual void OnFixedUpdate()
    {

    }

    public virtual void Dispose()
    {
        _transform = null;
    }
}