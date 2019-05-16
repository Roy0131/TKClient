public class DecomposeEvent
{
    #region net message
    /// <summary>
    /// no Param
    /// </summary>
    public static readonly string RefreshRoleList = "refreshRoleList";

    #endregion

    #region ui message
    /// <summary>
    /// param cardId
    /// </summary>
    public static readonly string AddRoleToDecompose = "addRoleToDecompose";
    /// <summary>
    /// param cardId
    /// </summary>
    public static readonly string RemoveRoleToDecompose = "removeRoleToDecompose";
    /// <summary>
    /// no param
    /// </summary>
    public static readonly string FillAllRoleToDecompose = "fillAllRoleToDecompose";
    /// <summary>
    /// no param
    /// </summary>
    public static readonly string ShowDecomposeReward = "showDecomposeReward";
    #endregion

}