public class ArenaEvent
{
    #region net msg
    public static readonly string AreanRankRefresh = "arenaRankRefresh";

    /// <summary>
    /// param S2CArenaMatchPlayerResponse
    /// </summary>
    public static readonly string AreanMatchPlayerBack = "arenaMatchPlayerBack";

    public static readonly string ArenaRecordListBack = "arenaRecordBack";

    public static readonly string ArenaRecordDataBack = "arenaRecordDataBack";

    /// <summary>
    /// param PlayerVO
    /// </summary>
    public static readonly string ArenaPlayerDefenseBack = "arenaPlayerDefenseBack";
    #endregion

    #region ui event
    public static readonly string HideArenaPlayerInfo = "arenaPlayerInfo";

    public static readonly string ArenaRewardShow = "arenaRewardShow";
    #endregion
}