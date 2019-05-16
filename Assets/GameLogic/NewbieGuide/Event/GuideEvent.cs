
public class GuideEvent
{
    #region 引导启动、结束事件

    public static readonly string EnterCondTrigger = "enterConditionTrigger";

    public static readonly string EndCondTrigger = "endConditionTrigger";

    #endregion

    public static readonly string GuideStepComplete = "guideStepComplete";

    public static readonly string GuideDragFighterSuccess = "guideDragFighterSuccess";

    //param index 
    public static readonly string GuideLineupFighter = "guideLineupFighter";

    public static readonly string GuideChangeFighter = "guideChangeFighter";

    //params type: bool -> enable
    public static readonly string LineupButtonStatusChange = "lineupButtonStatusChange";
}