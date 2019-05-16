public class FriendEvent
{
    #region net msg
    public static readonly string FriendListRefresh = "friendListRefresh";
    public static readonly string RecommemdFriendRefresh = "recommemdFriendRefresh";
    public static readonly string FriendAskPlayerListRefresh = "askFriendListRefresh";
    public static readonly string FriendAssistDataRefresh = "friendAssistDataRefresh";
    public static readonly string FriendAssistCardRefresh = "friendAssistCardRefresh";
    public static readonly string FriendAssistPointRefresh = "friendAssistPointRefresh";
    public static readonly string FriendAssistBossRefresh = "friendAssistBossRefresh";

    /// <summary>
    /// param List<int> players
    /// </summary>
    public static readonly string FriendRefreshGivePoints = "friendRefreshGivePoints";
    /// <summary>
    /// param List<int> players
    /// </summary>
    public static readonly string FriendRefreshGetPoints = "friendRefreshGetPoint";
    public static readonly string FriendRefreshBossHp = "friendRefreshBossHp";
    #endregion

    #region ui msg
    /// <summary>
    /// parma title conten -> string
    /// </summary>
    public static readonly string RefreshFriendTitle = "refreshFriendTitle";
    /// <summary>
    /// param FriendDataVO
    /// </summary>
    public static readonly string ChallengeFriendBoss = "ChallengeFriendBoss";
    #endregion

    public static readonly string AddFriend = "addFriend";

    public static readonly string RemoveFriend = "removeFriend";
}
