using System;
using System.Collections.Generic;
using UnityEngine;
public class BuffManager : UpdateNode
{
    private Dictionary<int, BuffExec> _dictBuffs;
    private Fighter _fighter;
    private List<BuffExec> _lstBuffExec;
    public BuffManager(Fighter fighter)
    {
        _fighter = fighter;
        _dictBuffs = new Dictionary<int, BuffExec>();
        _lstBuffExec = new List<BuffExec>();
        _blInitialed = true;
    }

    protected override void OnInitData<T>(T data)
    {
    }

    public void AddBuff(BuffDataVO vo)
    {
        if (vo.mStatusConfig.BuffTextID == 0 || _dictBuffs == null)
            return;
        if (_dictBuffs.ContainsKey(vo.mBuffId))
        {
            _dictBuffs[vo.mBuffId].AddBuffCount();
            //Debuger.Log("[BuffManager.AddBuff() => repeat add buff, buffId:" + vo.mBuffId + ", buff count:" + _dictBuffs[vo.mBuffId].BuffCount + "]");
        }
        else
        {
            BuffExec exec = new BuffExec(_fighter);
            exec.InitData(vo);
            _dictBuffs.Add(vo.mBuffId, exec);
            _lstBuffExec.Add(exec);
            //Debuger.Log("[BuffManager.AddBuff() => add new buff, buff id:" + vo.mBuffId + "]");
        }
    }

    public void RemoveBuff(BuffDataVO vo)
    {
        if (vo.mStatusConfig.BuffTextID == 0 || _dictBuffs == null)
            return;
        if (!_dictBuffs.ContainsKey(vo.mBuffId))
        {
            LogHelper.LogError("[BuffManager.RemoveBuff() => buff id:" + vo.mBuffId + " not found!!!]");
            return;
        }
        BuffExec exec = _dictBuffs[vo.mBuffId];
        exec.RemoveBuffCount();
        if (exec.BuffCount == 0)
        {
            if (_lstBuffExec != null)
                _lstBuffExec.Remove(exec);
            exec.Dispose();
            _dictBuffs.Remove(vo.mBuffId);
        }
    }

    public void UpdatePosition(Vector3 pos)
    {
        if (_lstBuffExec == null)
            return;
        for (int i = 0; i < _lstBuffExec.Count; i++)
            _lstBuffExec[i].UpdatePosition(pos);
    }


    protected override void OnUpdate()
    {
        if (_lstBuffExec == null)
            return;
        for (int i = 0; i < _lstBuffExec.Count; i++)
            _lstBuffExec[i].Update();
        base.OnUpdate();
    }

    public void ClearAllBuff()
    {
        if (_dictBuffs != null)
        {
            Dictionary<int, BuffExec>.ValueCollection valColl = _dictBuffs.Values;
            foreach (BuffExec exec in valColl)
                exec.Dispose();
            _dictBuffs.Clear();
            _dictBuffs = null;
        }
        if (_lstBuffExec != null)
        {
            _lstBuffExec.Clear();
            _lstBuffExec = null;
        }
    }

    protected override void OnDispose()
    {
        _fighter = null;
        ClearAllBuff();
        base.OnDispose();
    }

    public bool BlBuffEffectEnd
    {
        get
        {
            if (_lstBuffExec == null || _lstBuffExec.Count == 0)
                return true;
            for (int i = 0; i < _lstBuffExec.Count; i++)
            {
                if (!_lstBuffExec[i].BlBuffEffectEnd)
                    return false;
            }
            return true;
        }
    }
}