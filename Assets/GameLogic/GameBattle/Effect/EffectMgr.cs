using UnityEngine;
using System.Collections.Generic;
using System;

public class EffectMgr : Singleton<EffectMgr>
{
    private List<EffectBase> _lstAllEffects;
    private Transform _effectRoot;

    public void Init()
    {
        _blInited = true;
        _lstAllEffects = new List<EffectBase>();
        _effectRoot = BattleManager.Instance.mBattleScene.mBulletRoot;
    }

    public EffectBase CreateEffect(string effectName, Vector3 pos, Action<EffectBase> effectEndMethod = null)
    {
        EffectDataVO vo = new EffectDataVO(pos);
        EffectConfig cfg = GameConfigMgr.Instance.GetEffectConfig(effectName);
        vo.InitData(cfg);
        EffectBase effect = new EffectBase(effectEndMethod);
        effect.InitData(vo);
        _lstAllEffects.Add(effect);
        effect.AddToStage(_effectRoot);
        return effect;
    }

    public EffectBase CreateEffect(string effectName, AnimatorFighter fighter, Action<EffectBase> effectEndMethod = null)
    {
        return CreateEffect(effectName, fighter.mHitWorldPosition, effectEndMethod);
    }

    public void DisposeEffect(EffectBase effect)
    {
        if (_lstAllEffects.IndexOf(effect) != -1)
            _lstAllEffects.Remove(effect);
        effect.Dispose();
    }

    public void Update()
    {
        if (!_blInited)
            return;
        if (_lstAllEffects != null && _lstAllEffects.Count > 0)
        {
            for (int i = _lstAllEffects.Count - 1; i >= 0; i--)
                _lstAllEffects[i].Update();
        }
    }

    public void Dispose()
    {
        if (_lstAllEffects.Count > 0)
        {
            for (int i = _lstAllEffects.Count - 1; i >= 0; i--)
                _lstAllEffects[i].Dispose();
            _lstAllEffects.Clear();
            _lstAllEffects = null;
        }
        _blInited = false;
        _effectRoot = null;
    }
}