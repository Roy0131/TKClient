public class HangupEvent
{
    #region data event

    public static readonly string CampaignDataChange = "campaignDataChange";

    public static readonly string HangupCampaignChange = "hangupCampaignChange";

    public static readonly string FixedRewardChange = "hangupFixedRewardChange";

    /// <summary>
    /// params ItemInfo
    /// </summary>
    public static readonly string RandomRewardChange = "randomRewardChange";

    #endregion

    #region ui event
    /// <summary>
    /// param campaign id
    /// </summary>
    public static readonly string MapSetHangupStage = "mapSetHangupStage";
    #endregion

    public static readonly string AccelerateReward = "accelerateReward";
}