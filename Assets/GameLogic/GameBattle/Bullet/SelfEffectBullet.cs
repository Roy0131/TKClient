using UnityEngine;

public class SelfEffectBullet : BulletBase
{
    private FrameTimer _damageTimer;

    protected override void OnStart()
    {
        base.OnStart();
        string[] hitTimes = mBulletDataVO.mSkillConfig.HitShowTime.Split(',');
        _damageTimer = new FrameTimer(int.Parse(hitTimes[0]), DoDamage);
        bool blHero = BattleDataModel.Instance.IsHeroFighter(mBulletDataVO.mAttacker.mData.mSide);
        if (!blHero)
            _modelObject.transform.localScale = new Vector3(-1f, 1f, 1f);
        else
            _modelObject.transform.localScale = Vector3.one;
    }

	private void DoDamage()
    {
        if (_lstTargeters != null)
        {
            for (int i = 0; i < _lstTargeters.Count; i++)
            {
                _lstTargeters[i].DoDamage(mBulletDataVO.mlstTargeters[i]);
                _lstShowingBloodFighters.Add(_lstTargeters[i]);
            }
            _lstTargeters.Clear();
        }
        GameEventMgr.Instance.mBattleDispatcher.DispathEvent(BattleEvent.BulletHitFirstDamage);
    }

	protected override void OnUpdate()
    {
        base.OnUpdate();
        if (_damageTimer != null && _damageTimer.mBlEnable)
            _damageTimer.Update();
    }

    protected override void OnDispose()
    {
        base.OnDispose();
        if (_damageTimer != null)
        {
            _damageTimer.Dispose();
            _damageTimer = null;
        }
    }
}