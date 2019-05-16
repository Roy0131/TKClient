
using UnityEngine;
using System.Collections.Generic;

public class BulletDataVO
{
    public Fighter mAttacker { get; private set; }
    public Vector3 mTargetPos { get; private set; }
    public SkillConfig mSkillConfig { get; private set; }
    public List<FighterDamageDataVO> mlstTargeters { get; private set; }

    public FighterDamageDataVO mAttackerData { get; private set; }

    public BulletDataVO(Fighter attacker, SkillConfig skillCfg)
    {
        mAttacker = attacker;
        mSkillConfig = skillCfg;
        mlstTargeters = new List<FighterDamageDataVO>();
    }

    public void SetData(List<FighterDamageDataVO> targeters, Vector3 targetPos)
    {
        mlstTargeters = targeters;
        mTargetPos = targetPos;
    }

    public void SetData(FighterDamageDataVO targeter)
    {
        mlstTargeters.Add(targeter);
        Fighter tFighter = BattleManager.Instance.mBattleScene.GetFighterBySeatIndex(targeter.mSide, targeter.mSeatIndex);
        mTargetPos = tFighter.mUnitRoot.position;
    }

    public void Dispose()
    {
        mAttacker = null;
        mSkillConfig = null;
        if(mlstTargeters != null)
        {
            mlstTargeters.Clear();
            mlstTargeters = null;
        }
    }
}