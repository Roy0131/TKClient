using UnityEngine;
using System.Collections.Generic;

public class ModelDataBase<T> : REventDispatcher where T : new()
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
                _instance = new T();
            return _instance;
        }
    }

    protected bool _blInited = false;
    public virtual void Init()
    {
        if (_blInited)
            return;
        _blInited = true;
        OnInit();
    }

    protected virtual void OnInit()
    {
        AddEvent();
    }

    protected virtual void AddEvent()
    {
        GameEventMgr.Instance.mGlobalDispatcher.AddEvent(GameEventMgr.GEventEnterBattle, ResetRequestTime);
        GameEventMgr.Instance.mGlobalDispatcher.AddEvent(GameEventMgr.GEventClearAllData, OnClearAllData);
    }

    protected virtual void RemoveEvent()
    {
        GameEventMgr.Instance.mGlobalDispatcher.RemoveEvent(GameEventMgr.GEventEnterBattle, ResetRequestTime);
        GameEventMgr.Instance.mGlobalDispatcher.RemoveEvent(GameEventMgr.GEventClearAllData, OnClearAllData);
    }

    private void OnClearAllData()
    {
        DoClearData();
        ResetRequestTime();
    }

    protected virtual void DoClearData()
    {

    }

    protected Dictionary<string, float> _dictLastReqTime = new Dictionary<string, float>();
    protected void AddLastReqTime(string key)
    {
        _dictLastReqTime[key] = Time.realtimeSinceStartup;
    }

    protected bool CheckNeedRequest(string key, float distTime = 30f)
    {
        if (_dictLastReqTime.ContainsKey(key))
        {
            float dt = Time.realtimeSinceStartup - _dictLastReqTime[key];
            return (distTime - dt) < 0.01f;
        }
        return true;
    }

    public void ResetRequestTime()
    {
        _dictLastReqTime.Clear();
    }
}