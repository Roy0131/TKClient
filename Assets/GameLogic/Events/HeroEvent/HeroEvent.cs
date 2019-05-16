public class HeroEvent
{
    #region data event

    /// <summary>
    /// no params
    /// </summary>
    public static readonly string HeroEnterGameFinished = "heroEnterGameFinish"; 
    /// <summary>
    /// no params
    /// </summary>
    public static readonly string HeroCardChange = "heroRoleChange"; 
    /// <summary>
    /// no params
    /// </summary>
    public static readonly string HeroInfoChange = "heroInfoChange";
    /// <summary>
    /// no param, The card fight status change.
    /// </summary>
    public static readonly string CardFightStatusChange = "cardFightStatusChange";

    ///// <summary>
    ///// param CardId  card level up 
    ///// </summary>
    public static readonly string CardLevelUp = "cardLevelUp";

    public static readonly string RoleLevelUp = "roleLevelUp";

    ///// <summary>
    ///// param CardId, card rank up
    ///// </summary>
    //public static readonly string CardRankUp = "cardRankUp";

    ///// <summary>
    ///// param CardId, The card lock status change.
    ///// </summary>
    //public static readonly string CardLockStatusChange = "cardLockStatusChange";

    public static readonly string HeroGoldChange = "heroGoldChange";

    /// <summary>
    /// param CardId
    /// </summary>
    public static readonly string CardFusionBack = "cardFusionBack";

    public static readonly string CardDataRefresh = "cardDataRefresh";
    public static readonly string HeroAddCard = "heroAddCard";
    public static readonly string HeroRemoveCard = "heroRemoveCard";

    #endregion

    #region ui event

    #endregion

    /// <summary>
    /// 修改名字
    /// </summary>
    public static readonly string AmendName = "amendName";

    /// <summary>
    /// 换头像
    /// </summary>
    public static readonly string AmendHead = "amendHead";

    /// <summary>
    /// 英雄详情
    /// </summary>
    public static readonly string HeroDetail = "heroDetail";

    /// <summary>
    /// 打开英雄商店
    /// </summary>
    public static readonly string HeroShop = "heroShop";
    
    /// <summary>
    /// 升级
    /// </summary>
    public static readonly string UpGrade = "upGrade";

    /// <summary>
    /// 进阶
    /// </summary>
    public static readonly string Advanced = "advanced";

    /// <summary>
    /// 突破
    /// </summary>
    public static readonly string Surmount = "surmount";

    /// <summary>
    /// 切换语言
    /// </summary>
    public static readonly string SwitchLanguage = "switchLanguage";
}
