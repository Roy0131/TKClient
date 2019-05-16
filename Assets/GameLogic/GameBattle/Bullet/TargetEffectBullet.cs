using System.Collections.Generic;
using UnityEngine;

public class TargetEffectBullet : BulletBase
{
    private FrameTimer _damageTimer;
    protected override void OnInitData<T>(T data)
	{
        _lstTargeters = new List<Fighter>();
        _lstShowingBloodFighters = new List<Fighter>();
        mBulletDataVO = data as BulletDataVO;
        OnStart();
        mblComplete = false;
        CreateBulletHitEffect();
        string[] hitTimes = mBulletDataVO.mSkillConfig.HitShowTime.Split(',');
        _damageTimer = new FrameTimer(int.Parse(hitTimes[0]), OnShowDamage);
        bool blHero = BattleDataModel.Instance.IsHeroFighter(mBulletDataVO.mAttacker.mData.mSide);
        Vector3 scale = blHero ? Vector3.one : new Vector3(-1f, 1f, 1f);
        if (_lstHitEffects != null)
        {
            for (int i = 0; i < _lstHitEffects.Count; i++)
                _lstHitEffects[i].mUnitRoot.localScale = scale;
        }
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (_damageTimer != null && _damageTimer.mBlEnable)
            _damageTimer.Update();
    }

    private void OnShowDamage()
    {
        for (int i = 0; i < _lstTargeters.Count; i++)
        {
            _lstTargeters[i].DoDamage(mBulletDataVO.mlstTargeters[i]);
            _lstShowingBloodFighters.Add(_lstTargeters[i]);
        }
        _lstTargeters.Clear();
        GameEventMgr.Instance.mBattleDispatcher.DispathEvent(BattleEvent.BulletHitFirstDamage);
    }


	protected override void OnEffectEnd(EffectBase effect)
    {
        base.OnEffectEnd(effect);
        OnEnd();
    }
}