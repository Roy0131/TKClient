using Msg.ClientMessage;
using System.Collections.Generic;

public class ShopIdConst
{
    public const int MYSTERYSHOP = 1;//神秘商店
    public const int HEROSHOP = 2;//英雄商店
    public const int TOWERSHOP= 3;//爬塔体力商店
    public const int ISLANDSHOP= 4;//空岛体力商店
    public const int GUILDSHOP = 5;//联盟商店
    public const int EXPEDITIONSHOP = 6;//远征商店
}

public class ShopDataModel : ModelDataBase<ShopDataModel>
{
    private Dictionary<int, ShopDataVO> _dictAllShopData = new Dictionary<int, ShopDataVO>();

    public void ReqShopData(int shopType)
    {
        if (!_dictAllShopData.ContainsKey(shopType))
            GameNetMgr.Instance.mGameServer.ReqShopData(shopType);
        else
            DispathEvent(ShopEvent.ShopData, shopType);
    }
    
    private void OnShopData(S2CShopDataResponse value)
    {
        ShopDataVO vo;
        if (_dictAllShopData.ContainsKey(value.ShopId))
        {
            vo = _dictAllShopData[value.ShopId];
            vo.InitData(value);
        }
        else
        {
            vo = new ShopDataVO();
            vo.InitData(value);
            _dictAllShopData.Add(value.ShopId, vo);
        }
        DispathEvent(ShopEvent.ShopData, value.ShopId);
    }
    
    private void OnShopBuy(S2CShopBuyItemResponse value)
    {
        _dictAllShopData[value.ShopId].OnShopBuyNum(value.Id,value.BuyNum);
        DispathEvent(ShopEvent.ShopBuy, value.Id);
    }
    
    private void OnShopResfresh(S2CShopRefreshResponse value)
    {
        DispathEvent(ShopEvent.Refresh, value.ShopId, value.IsFreeRefresh);
    }

    private void OnShopAuto(S2CShopAutoRefreshNotify value)
    {
        DispathEvent(ShopEvent.Autorefresh, value.ShopId);
    }

    public ShopDataVO GetShopDataByShopId(int shopId)
    {
        if (_dictAllShopData.ContainsKey(shopId))
            return _dictAllShopData[shopId];
        return null;
    }

    public static void DoShopData(S2CShopDataResponse value)
    {
        Instance.OnShopData(value);
    }

    public static void DoShopBuy(S2CShopBuyItemResponse value)
    {
        Instance.OnShopBuy(value);
    }

    public static void DoShopResfresh(S2CShopRefreshResponse value)
    {
        Instance.OnShopResfresh(value);
    }

    public static void DoShopAuto(S2CShopAutoRefreshNotify value)
    {
        Instance.OnShopAuto(value);
    }

    protected override void DoClearData()
    {
        base.DoClearData();
        if (_dictAllShopData != null)
            _dictAllShopData.Clear();
    }
}