using UnityEngine;
using System.Collections.Generic;

public class BuffExec : UpdateNode
{
    private int _buffCount;
    private BuffDataVO _buffVO;
    private Fighter _fighter;
    private EffectBase _buffEffect;

    private List<BuffTipsView> _lstTipsView;
    public BuffExec(Fighter fighter)
    {
        _fighter = fighter;
        _lstTipsView = new List<BuffTipsView>();
    }

    protected override void OnInitData<T>(T data)
    {
        _buffCount = 1;
        _buffVO = data as BuffDataVO;
        StartBuff();
    }

    protected virtual void StartBuff()
    {
        if (!string.IsNullOrEmpty(_buffVO.mStatusConfig.ShowEffect))
            _buffEffect = EffectMgr.Instance.CreateEffect(_buffVO.mStatusConfig.ShowEffect, _fighter.mUnitRoot.position);
        if (_buffVO.mStatusConfig.ClientSpecialEffect == 1)
            _fighter.SetControlStatus(true);
        if (string.Equals(_buffVO.mStatusConfig.Effect, "4"))
            _fighter.SetAnimatorSpeed(0f);
        CreateBuffTips();
    
        
    }

    private void CreateBuffTips()
    {
        BuffTipsView view = BattleManager.Instance.mBattleUIMgr.CreateBuffTips(_buffVO.mStatusConfig);

        Vector3 pos = new Vector3(_fighter.mDefaultPos.x, _fighter.mDefaultPos.y + 1f, _fighter.mDefaultPos.z);
        Vector3 p1 = GameUIMgr.Instance.WorldToUIPoint(pos);
        Vector3 p2 = new Vector3(p1.x, p1.y - _fighter.mlstBuffTipsView.Count * 30f, p1.z);
        view.PlayAnimation(p2);
        _lstTipsView.Add(view);
        _fighter.mlstBuffTipsView.Add(view);

        if (_buffVO.mStatusConfig.Icon != "")
        {
            _fighter.SetBuffState(_buffVO.mStatusConfig, true, _buffCount);
        }
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (_lstTipsView.Count > 0)
        {
            for (int i = _lstTipsView.Count - 1; i >= 0; i--)
            {
                if (_lstTipsView[i].mBlMoveEnd)
                {
                    _fighter.mlstBuffTipsView.Remove(_lstTipsView[i]);
                    BattleManager.Instance.mBattleUIMgr.ReturnBuffTips(_lstTipsView[i]);
                    _lstTipsView.RemoveAt(i);
                }
            }
        }
    }

    public void UpdatePosition(Vector3 pos)
    {
        if (_buffEffect == null)
            return;
        _buffEffect.UpdatePosition(pos);
    }

    protected virtual void EndBuff()
    {
        if (_buffEffect != null)
            EffectMgr.Instance.DisposeEffect(_buffEffect);
        if (_buffVO.mStatusConfig.ClientSpecialEffect == 1)
            _fighter.SetControlStatus(false);
        if (string.Equals(_buffVO.mStatusConfig.Effect, "4"))
            _fighter.SetAnimatorSpeed(1f);
        if (_buffVO.mStatusConfig.Icon != "")
        {
            _fighter.SetBuffState(_buffVO.mStatusConfig, false, _buffCount);
        }
       
    }

    protected override void OnDispose()
    {
        EndBuff();
        if (_lstTipsView != null)
        {
            for (int i = _lstTipsView.Count - 1; i >= 0; i--)
            {
                _fighter.mlstBuffTipsView.Remove(_lstTipsView[i]);
                BattleManager.Instance.mBattleUIMgr.ReturnBuffTips(_lstTipsView[i]);
            }
            _lstTipsView.Clear();
            _lstTipsView = null;
        }
        _buffVO = null;
        _buffEffect = null;
        _buffCount = 1;
        base.OnDispose();
    }

    public void AddBuffCount()
    {
        _buffCount++;
        CreateBuffTips();
    }

    public void RemoveBuffCount()
    {
        _buffCount--;
    }

    public int BuffCount
    {
        get { return _buffCount; }
    }

    public bool BlBuffEffectEnd
    {
        get
        {
            if (_lstTipsView.Count > 0)
                return false;
            return true;
        }
    }
}