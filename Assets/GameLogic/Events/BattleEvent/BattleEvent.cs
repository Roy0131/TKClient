public class BattleEvent
{
    public static readonly string BattleSweepEnd = "battleSweepEnd";

    /// <summary>
    /// params SkillActionBase object
    /// </summary>
    public static readonly string BattleStepEnd = "battleStepEnd";

    public static readonly string SelfArtifactActionEnd = "selfArtifactActionEnd";
    /// <summary>
    /// params SkillRoundAction object
    /// </summary>
    public static readonly string BattleAttackRoundEnd = "battleAttackRoundEnd";
    public static readonly string BattleRoundEnd = "battleRoundEnd";
    /// <summary>
    /// params RoundNodeDataVO
    /// </summary>
    public static readonly string BattleRoundStart = "battleRoundStart";
    /// <summary>
    /// no params
    /// </summary>
    public static readonly string BattleEnd = "battleEnd";

    /// <summary>
    /// no params, bullet first show damage and skill action node start to check summom fighter or buff logic
    /// </summary>
    public static readonly string BulletHitFirstDamage = "bulletHitFirstDamage";

    public static readonly string ShowBattleDetailView = "showBattleDetailView";

    public static readonly string HideBattleDetailView = "hideBattleDetailView";

    public static readonly string FightJump = "FightJump";
}