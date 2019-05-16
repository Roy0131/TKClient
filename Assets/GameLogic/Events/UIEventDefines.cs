public class UIEventDefines
{
    #region Debug, about console message 
    public static readonly string ConsoleLogChange = "consoleLogChange";
    public static readonly string CloseConsoleModule = "forceCloseConsoleModule";
    public static readonly string OpenConsoleModule = "forceOpenConsoleModule";
    #endregion

    #region line up ui message
    public static readonly string LineupFighterDrop = "lineupFighterDrop";
    /// <summary>
    /// param fighter tableId
    /// </summary>
    public static readonly string LineupFighterRemove = "lineupFighterRemoved";
    #endregion

    #region roleinfo ui message
    /// <summary>
    /// param CardDataVO 
    /// The role info change role.
    /// </summary>
    public static readonly string RoleInfoChangeRole = "roleinfoChangeRole";

    /// <summary>
    /// param skillid, The show skill tips.
    /// </summary>
    public static readonly string ShowSkillTips = "showSkillTips";
    /// <summary>
    /// no param, The hide skill tips.
    /// </summary>
    public static readonly string HideSkillTips = "hideSkillTips";

    #endregion

    #region role fusion ui message
    /// <summary>
    /// no param, role fusion back, role info module switch role basic view toggle
    /// </summary>
    public static readonly string RoleInfoFusion = "roleInfoFusion";
    /// <summary>
    /// param matIdx -> int, open role fusion material module
    /// </summary>
    public static readonly string OpenFusionMatSelect = "openFusionMatSelect";
    /// <summary>
    /// param matIdx -> int, The fusion select material ok
    /// </summary>
    public static readonly string FusionMatSelectOK = "fusionMatSelectOk";
    #endregion
    ///// <summary>
    ///// param pos
    ///// </summary>
    //public static readonly string LineUpAddCardToFight = "lineupAddCardToFight";
    ///// <summary>
    ///// param pos
    ///// </summary>
    //public static readonly string LineUpRemoveFight = "lineupRemoveFight";
    ///// <summary>
    ///// param oldIndex, newIndex
    ///// </summary>
    //public static readonly string LineUpChangeFightIndex = "lineupChangeFightIndex";

    public static readonly string LineupChangeIndex = "lineupChangeIndex";

    public static readonly string SkillType = "skillType";
    
}