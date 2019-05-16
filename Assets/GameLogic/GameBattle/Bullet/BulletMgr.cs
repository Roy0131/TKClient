using UnityEngine;
using System.Collections.Generic;

public class BulletMgr : Singleton<BulletMgr>
{
    private List<BulletBase> _lstBullets;
    private Transform _bulletRoot;

    public void Init()
    {
        _lstBullets = new List<BulletBase>();
        _blInited = true;
        _bulletRoot = BattleManager.Instance.mBattleScene.mBulletRoot;
    }

    public BulletBase CreateBullet(BulletDataVO value)
    {
        BulletBase bullet = null;
        if (value.mSkillConfig == null)
        {
            LogHelper.LogError("create bullet error, skillConfig is null!!!" + value.mAttacker.mData.mSide + "_" + value.mAttacker.mData.mSeatIndex);
            return null;
        }

        BulletType bulletType = (BulletType)value.mSkillConfig.BulletType;
        switch (bulletType)
        {
            case BulletType.Linear:
                bullet = new LinearBullet();
                break;
            case BulletType.FiexedEffect:
                bullet = new TargetEffectBullet();
                break;
            case BulletType.LineEffect:
                bullet = new LinkBullet();
                break;
            case BulletType.SelfEffect:
                bullet = new SelfEffectBullet();
                break;
            case BulletType.Bomb:
                bullet = new BombBullet();
                break;
        }
        if (bullet != null)
        {
            bullet.InitData(value);
            if (bulletType != BulletType.FiexedEffect)
                bullet.AddToStage(_bulletRoot);
            _lstBullets.Add(bullet);
        }
        return bullet;
    }

    public void RemoveBullet(BulletBase bullet)
    {
        if (_lstBullets.Contains(bullet))
            _lstBullets.Remove(bullet);
        bullet.Dispose();
    }

    public void Update()
    {
        if (!_blInited)
            return;
        for (int i = _lstBullets.Count - 1; i >= 0; i--)
            _lstBullets[i].Update();
    }

    public void Dispose()
    {
        if (_lstBullets != null)
        {
            for (int i = _lstBullets.Count - 1; i >= 0; i--)
            {
                _lstBullets[i].Dispose();
                _lstBullets[i] = null;
            }
            _lstBullets.Clear();
            _lstBullets = null;
        }
        _blInited = false;
        _bulletRoot = null;
    }
}