using UnityEngine;

public class LinearBullet : MoveBulletBase
{
    private bool _blFirstDamage = false;

    protected override void OnStart()
    {
        base.OnStart();
        _blFirstDamage = true;
        //Debuger.LogWarning("start, time:" + Time.realtimeSinceStartup + ", movespeed:" + _flBulletSpeed);
    }

	protected override void CheckMoveEnd()
	{
        float distance;
        for (int i = _lstTargeters.Count - 1; i >= 0; i--)
        {
            distance = Vector3.Distance(mUnitRoot.position, _lstTargeters[i].mHitWorldPosition);
            if (distance <= _flFrameSpeed)
            {
                CreateHitEffect(_lstTargeters[i].mHitWorldPosition);
                _lstTargeters[i].DoDamage(mBulletDataVO.mlstTargeters[i]);
                _lstShowingBloodFighters.Add(_lstTargeters[i]);
                //if (_blFirstDamage)
                //{
                    GameEventMgr.Instance.mBattleDispatcher.DispathEvent(BattleEvent.BulletHitFirstDamage);
                //    _blFirstDamage = false;
                //}
                _lstTargeters.RemoveAt(i);
                mBulletDataVO.mlstTargeters.RemoveAt(i);
                if (mBulletDataVO.mSkillConfig.BulletPenetrate == 0)
                {
                    OnEnd();
                    return;
                }
            }
        }
	}

    protected void CreateHitEffect(Vector3 pos)
    {
        if (!string.IsNullOrWhiteSpace(mBulletDataVO.mSkillConfig.BulletHitSound))
            SoundMgr.Instance.PlayEffectSound(mBulletDataVO.mSkillConfig.BulletHitSound);
        if (string.IsNullOrEmpty(mBulletDataVO.mSkillConfig.BulletHitEffect))
            return;
        EffectBase effect = EffectMgr.Instance.CreateEffect(mBulletDataVO.mSkillConfig.BulletHitEffect, pos, OnEffectEnd);
        _lstHitEffects.Add(effect);
    }

	protected override Vector3 GetTargetPos()
	{
        return _lstTargeters[0].mHitWorldPosition;
	}
}