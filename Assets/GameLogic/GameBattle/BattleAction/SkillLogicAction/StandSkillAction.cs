public class StandSkillAction : SkillActionBase
{
    protected override void OnStart()
    {
        if (mActionItemData.mSkillConfig.SkillAnimType == 0)
        {
            _status = AttackNodeStatus.Attacking;
        }
        else
        {
            _attacker.ChangeSortLayerOffest(RenderLayerOffset.FighterAttack);
            OnStartAttack();
        }
    }
}