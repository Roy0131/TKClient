using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;

public class ShopDataVO : DataBaseVO
{
    public int mShopId { get; private set; }
    public int mReFreshId { get; private set; }
    public int mReFreshNum { get; private set; }
    public int mTargTime { get; private set; }
    public int mFreeTime { get; private set; }
    public List<ShopItemDataVO> mListItemVO { get; private set; }


    protected override void OnInitData<T>(T value)
    {
        S2CShopDataResponse shopData = value as S2CShopDataResponse;
        mShopId = shopData.ShopId;
        mTargTime = (int)Time.realtimeSinceStartup + shopData.NextFreeRefreshRemainSeconds;
        ShopConfig cfg = GameConfigMgr.Instance.GetShopConfig(mShopId);
        if (mListItemVO != null)
            mListItemVO.Clear();
        mListItemVO = new List<ShopItemDataVO>();
        ShopItemDataVO vo;
        if (cfg.ShopType == 1)
        {
            if (shopData.Items != null && shopData.Items.Count > 0)
            {
                for (int i = 0; i < shopData.Items.Count; i++)
                {
                    vo = new ShopItemDataVO();
                    ShopItemConfig itemCfg = GameConfigMgr.Instance.GetShopItemConfig(shopData.Items[i].ItemId);
                    vo.OnItemData(shopData.Items[i].Id, shopData.Items[i].ItemId, shopData.Items[i].BuyNum, itemCfg.StockNum, shopData.Items[i].CostResource);
                    mListItemVO.Add(vo);
                }
            }
            else
            {
                LogHelper.Log("数据为空");
            }
        }
        else
        {
            foreach (var item in ShopItemConfig.Get().Values)
            {
                if (item.ShopID == shopData.ShopId)
                {
                    vo = new ShopItemDataVO();
                    ItemInfo info = new ItemInfo();
                    string[] bycost = item.BuyCost.Split(',');
                    if (bycost.Length % 2 != 0)
                        return;
                    for (int i = 0; i < bycost.Length; i += 2)
                    {
                        info.Id = int.Parse(bycost[i]);
                        info.Value = int.Parse(bycost[i + 1]);
                    }
                    vo.OnItemData(item.GoodID, item.GoodID, 0, item.StockNum, info);
                    if (shopData.Items != null && shopData.Items.Count > 0)
                    {
                        for (int i = 0; i < shopData.Items.Count; i++)
                        {
                            if (shopData.Items[i].ItemId == item.GoodID)
                                vo.OnItemData(shopData.Items[i].Id, shopData.Items[i].ItemId, shopData.Items[i].BuyNum, item.StockNum, shopData.Items[i].CostResource);
                        }
                    }
                    mListItemVO.Add(vo);
                }
            }
        }
        mListItemVO.Sort(OnSort);


        string[] reFresh = cfg.RefreshRes.Split(',');
        for (int i = 0; i < reFresh.Length; i += 2)
        {
            if (reFresh.Length % 2 != 0)
                continue;
            mReFreshId = int.Parse(reFresh[i]);
            mReFreshNum = int.Parse(reFresh[i + 1]);
        }
        mFreeTime = cfg.FreeRefreshTime;
    }

    private int OnSort(ShopItemDataVO v1, ShopItemDataVO v2)
    {
        return v1.mId < v2.mId ? -1 : 1;
    }

    public int LastTime
    {
        get { return mTargTime - (int)Time.realtimeSinceStartup; }
    }

    public void OnShopBuyNum(int id,int num)
    {
        for (int i = 0; i < mListItemVO.Count; i++)
        {
            if (mListItemVO[i].mId == id)
                mListItemVO[i].OnBuyNum(num);
        }
    }

}

public class ShopItemDataVO
{
    public int mId { get; private set; }
    public int mItemId { get; private set; }
    public int mMaxNum { get; private set; }
    public int mBuyNum { get; private set; }
    public ItemInfo mInfo { get; private set; }


    public void OnItemData(int id, int itemId, int num, int maxNum, ItemInfo info)
    {
        mId = id;
        mItemId = itemId;
        mMaxNum = maxNum;
        mBuyNum = mMaxNum - num;
        mInfo = info;
    }

    public void OnBuyNum(int num)
    {
        mBuyNum = mMaxNum - num;
    }
}