public class BombBullet : MoveBulletBase
{

    protected override void OnMoveEnd()
    {
        _modelObject.gameObject.SetActive(false);
        _blMoveEnd = true;
        if (mBulletDataVO.mSkillConfig.BulletContinueTime == 0)
            OnEnd();
        CreateBulletHitEffect();
    }

    protected override void OnEnd()
    {
        for (int i = 0; i < _lstTargeters.Count; i++)
        {
            _lstTargeters[i].DoDamage(mBulletDataVO.mlstTargeters[i]);
            _lstShowingBloodFighters.Add(_lstTargeters[i]);
        }
        _lstTargeters.Clear();
        GameEventMgr.Instance.mBattleDispatcher.DispathEvent(BattleEvent.BulletHitFirstDamage);
        base.OnEnd();
    }
}