
public class GuildEvent
{
    #region net msg event
    public static readonly string GuildDataRefresh = "guildDataRefresh";

    public static readonly string RecommemdGuildRefresh = "recommemdGuildRefresh";

    public static readonly string SerachGuildRefresh = "searchGuildRefresh";

    public static readonly string GuildAgreeJoinNotify = "guildAgreeJoinNotify";

    public static readonly string GuildSignUpdate = "guildSignUpdate";

    public static readonly string GuildLevelUp = "guildLevelUp";

    public static readonly string GuildDonateListRefresh = "guildDonateListRefresh";

    public static readonly string GuildMemberDataRefresh = "guildMemberDataRefresh";

    public static readonly string AskJoinMemberDataRefresh = "askJoinMemberDataRefresh";

    public static readonly string GuildRecruitSendBack = "guildRecruitSendBack";

    public static readonly string GuildDismissTimeRefresh = "guildDismissTimeRefresh";

    public static readonly string GuildNameRefresh = "guildNameRefresh";

    public static readonly string GuildMemberChange = "guildMemberRefresh";

    public static readonly string GuildIconRefresh = "guildIconRefresh";

    public static readonly string GuildNoticeRefresh = "guildNoticeRefresh";

    public static readonly string GuildLogsRefresh = "guildLogsRefresh";

    public static readonly string GuildDonateRefresh = "guildDonateRefresh";

    public static readonly string GuildDonateReward = "GuildDonateReward";

    /// <summary>
    /// param guildId
    /// </summary>
    public static readonly string GuildAskJoinBack = "guildAskJoinBack";

    public static readonly string ReqDonateResult = "reqDonateResult";

    public static readonly string ReqBossResult = "reqBossResult";
    public static readonly string ReqBossDmg = "reqBossDmg";
    public static readonly string ReqCurBoss = "reqCurBoss";
    public static readonly string ShowHurt = "showHurt";
    public static readonly string ResPawn = "resPawn";
    #endregion

    #region ui event
    public static readonly string ShowDonateView = "showDonateView";
    public static readonly string ReqDonate = "reqDonate";
    public static readonly string ShowModifyInfoView = "showModifyInfoView";
    public static readonly string ShowGuildLogView = "showGuildLogView";
    public static readonly string ShowGuildMapView = "showGuildMapView";

    public static readonly string ShowAskJoinMemberView = "showAskJoinMemberView";
    public static readonly string ShowRecruitView = "showRecruitView";

    public static readonly string ShowGuildMap = "showGuildMap";
    public static readonly string ShowGuildBoss = "showGuildBoss";
    #endregion
}