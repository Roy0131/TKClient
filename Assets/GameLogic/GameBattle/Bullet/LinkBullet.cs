
using UnityEngine;
using System;

public class LinkBullet : BulletBase
{
	protected override void OnStart()
	{
        base.OnStart();
        if (_lstTargeters.Count == 0)
            return;
        Vector3 targetPos = _lstTargeters[0].mHitWorldPosition;
        Vector3 firePos = mBulletDataVO.mAttacker.FireDummyWorldPosition;
        _direction = (targetPos - firePos).normalized;
        ChangeSortLayerOffest(RenderLayerOffset.TopBullet);

        float temp = Mathf.Atan2(_direction.y, _direction.x);
        float flDegress = temp * 180f / (float)Math.PI;
        mUnitRoot.transform.localEulerAngles = Vector3.zero;
        mUnitRoot.transform.Rotate(0f, 0f, flDegress);

        float value = Vector3.Distance(targetPos, firePos);

        Transform line = _modelObject.transform.Find("line");
        line.localScale = new Vector3(value, line.localScale.y, line.localScale.z);

        CreateBulletHitEffect();

        for (int i = 0; i < _lstTargeters.Count; i++)
        {
           _lstTargeters[i].DoDamage(mBulletDataVO.mlstTargeters[i]);
           _lstShowingBloodFighters.Add(_lstTargeters[i]);
        }
        _lstTargeters.Clear();
        GameEventMgr.Instance.mBattleDispatcher.DispathEvent(BattleEvent.BulletHitFirstDamage);
	}

	protected override Vector3 GetBulletEffectPos()
	{
        return _lstTargeters[0].mHitWorldPosition;
	}
}