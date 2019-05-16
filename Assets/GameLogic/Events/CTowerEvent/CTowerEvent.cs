
public class CTowerEvent  {

    public static readonly string CloseTicket = "CloseTicket";

    #region 刷新爬塔数据
    public static readonly string RefreshTowerData = "RefreshTowerData";
    #endregion
    #region 刷新爬塔某曾录像数据
    public static readonly string RefreshTowerRecordsInfoData = "RefreshTowerRecordsInfoData";
    #endregion
    #region 刷新爬塔某个录像具体数据
    public static readonly string RefreshTowerRecordData = "RefreshTowerRecordData";
    #endregion
    #region 刷新爬塔排行榜数据
    public static readonly string RefreshTowerRankingListData = "RefreshTowerRankingListData";
    #endregion
    #region 关闭界面的时候移除定时器
    public static readonly string ClearTowerTimeHeap = "ClearTowerTimeHeap";
    #endregion
    #region 关闭界面的时候设置刷新位置是false
    public static readonly string SetTowerPos = "SetTowerPos";
    #endregion
}
