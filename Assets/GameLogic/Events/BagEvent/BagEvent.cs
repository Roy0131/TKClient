
public class BagEvent
{
    #region data event
    /// <summary>
    /// 背包所有数据刷新
    /// </summary>
    public static readonly string BagAllItemRefresh = "bagAllItemRefresh";

    /// <summary>
    /// 变化的道具配置id。参数：变化道具数据刷新
    /// </summary>
    public static readonly string BagItemRefresh = "bagItemRefresh";

    /// <summary>
    /// params IList<ItemInfo>,
    /// </summary>
    public static readonly string ItemFusionRefresh = "itemFusionRefresh";

    /// <summary>
    /// params int -> item id, int -> item count
    /// </summary>
    public static readonly string ItemSellResult = "itemSellResult";

    /// <summary>
    /// param int -> item id
    /// </summary>
    public static readonly string ItemUpGradeBack = "itemUpGradeBack";

    /// <summary>
    /// param roleid
    /// </summary>
    public static readonly string ItemUpGradeRefresh = "itemUpGradeRefresh";

    /// <summary>
    /// param role id
    /// </summary>
    public static readonly string ItemUpGradeSaveBack = "itemUpGradeSaveBack";

    public static readonly string Click = "clik";

    public static readonly string OneKeyUpGrade = "oneKeyUpGrade";

    public static readonly string BagNull = "bagNull";

    public static readonly string Detail = "Detail";

    public static readonly string BagJump = "bagJump";

    #endregion

    #region ui event

    #endregion
}