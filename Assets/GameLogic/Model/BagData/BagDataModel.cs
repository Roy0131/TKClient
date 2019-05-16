using Msg.ClientMessage;
using System.Collections.Generic;

//public enum ItemResType
//{
//    EQUIPMENT = 1,//装备
//    PROPERTY,//道具
//    DEBRIS,//碎片
//    ARTIFACT//神器
//}
public class ItemTypeConst
{
    public const int EQUIPMENT = 1;//装备
    public const int PROPERTY = 2;//道具
    public const int DEBRIS = 3;//碎片
    public const int ARTIFACT = 4;//神器
    public const int AVATAR = 5;//头像
}
public enum KindType
{
    KONG=0,//null
    GREEN,//绿装
    BLUE,//蓝装
    PURPLE,//紫装
    YELLOW,//黄装
    RED,//红装
    ORANGE,//橙装
}

public class BagDataModel : ModelDataBase<BagDataModel>
{
    private Dictionary<int, ItemInfo> _dictAllItems = new Dictionary<int, ItemInfo>();

    private void OnUpdateItem(S2CItemsUpdate value)
    {
        if (value.ItemsAdd == null || value.ItemsAdd.Count == 0)
            return;
        List<int> changeIds = new List<int>();
        ItemInfo info;
        for (int i = 0; i < value.ItemsAdd.Count; i++)
        {
            info = value.ItemsAdd[i];
            if (!changeIds.Contains(info.Id))
                changeIds.Add(info.Id);
            if (_dictAllItems.ContainsKey(info.Id))
            {
                _dictAllItems[info.Id].Value += info.Value;
                if (_dictAllItems[info.Id].Value <= 0)
                    _dictAllItems.Remove(info.Id);
            }
            else
                _dictAllItems.Add(info.Id, info);
        }
        BagRedState();
        DispathEvent(BagEvent.BagItemRefresh, changeIds);
    }
    
    private void OnItemSync(S2CItemsSync value)
    {
        _dictAllItems.Clear();
        for (int i = 0; i < value.Items.Count; i++)
            _dictAllItems.Add(value.Items[i].Id, value.Items[i]);
        BagRedState();
        DispathEvent(BagEvent.BagAllItemRefresh);
    }

    private void BagRedState()
    {
        foreach (ItemInfo info in _dictAllItems.Values)
        {
            if (GameConfigMgr.Instance.GetItemConfig(info.Id).ItemType == 4 && info.Value >= GameConfigMgr.Instance.GetItemConfig(info.Id).ComposeNum)
            {
                RedPointDataModel.Instance.SetRedPointDataState(RedPointEnum.BagFragment, true);
                return;
            }
            RedPointDataModel.Instance.SetRedPointDataState(RedPointEnum.BagFragment, false);
        }
    }

    public List<ItemInfo> GetEquipItemByEquipType(int equipType)
    {
        List<ItemInfo> result = new List<ItemInfo>();
        Dictionary<int, ItemInfo>.ValueCollection valColl = _dictAllItems.Values;
        ItemConfig config;
        foreach (ItemInfo info in valColl)
        {
            config = GameConfigMgr.Instance.GetItemConfig(info.Id);
            if (config == null)
                continue;
            if (config.ItemType != ItemType.Equip)
                continue;
            if (config.EquipType == equipType)
                result.Add(info);
        }
        return result;
    }

    public List<int> GetAvatarItem()
    {
        List<int> avatar = new List<int>();
        Dictionary<int, ItemInfo>.ValueCollection valColl = _dictAllItems.Values;
        ItemConfig config;
        foreach (ItemInfo info in valColl)
        {
            config = GameConfigMgr.Instance.GetItemConfig(info.Id);
            if (config == null)
                continue;
            if (config.ItemType == ItemType.Avatar)
                avatar.Add(info.Id);
        }
        return avatar;
    }

    //背包分类
    public List<ItemInfo> GetBagItemDataByType(int restype, KindType kindType)
    {
        List<ItemInfo> result = new List<ItemInfo>();
        Dictionary<int, ItemInfo>.ValueCollection valColl = _dictAllItems.Values;
        
        ItemConfig cfg;
        //装备分类
        if (restype == ItemTypeConst.EQUIPMENT)
        {
            foreach (ItemInfo info in valColl)
            {
                cfg = GameConfigMgr.Instance.GetItemConfig(info.Id);
                if (cfg.ItemType == 2 && cfg.EquipType <= 4)
                {
                    if (kindType == KindType.KONG && cfg.Quality <= 6)
                        result.Add(info);
                    if (kindType == KindType.ORANGE && cfg.Quality == 6)
                        result.Add(info);
                    if (kindType == KindType.RED && cfg.Quality == 5)
                        result.Add(info);
                    if (kindType == KindType.YELLOW && cfg.Quality == 4)
                        result.Add(info);
                    if (kindType == KindType.PURPLE && cfg.Quality == 3)
                        result.Add(info);
                    if (kindType == KindType.BLUE && cfg.Quality == 2)
                        result.Add(info);
                    if (kindType == KindType.GREEN && cfg.Quality == 1)
                        result.Add(info);
                }
            }
        }
        //神器分类
        if (restype == ItemTypeConst.ARTIFACT)
        {
            foreach (ItemInfo info in valColl)
            {
                cfg = GameConfigMgr.Instance.GetItemConfig(info.Id);
                if (cfg.ItemType == 2 && cfg.EquipType == 6)
                {
                    if (kindType == KindType.KONG && cfg.Quality <= 6)
                        result.Add(info);
                    if (kindType == KindType.ORANGE && cfg.Quality == 6)
                        result.Add(info);
                    if (kindType == KindType.RED && cfg.Quality == 5)
                        result.Add(info);
                    if (kindType == KindType.YELLOW && cfg.Quality == 4)
                        result.Add(info);
                    if (kindType == KindType.PURPLE && cfg.Quality == 3)
                        result.Add(info);
                    if (kindType == KindType.BLUE && cfg.Quality == 2)
                        result.Add(info);
                    if (kindType == KindType.GREEN && cfg.Quality == 1)
                        result.Add(info);
                }
            }
        }
        //道具
        if (restype== ItemTypeConst.PROPERTY && kindType== KindType.KONG)
        {
            foreach (ItemInfo info in valColl)
            {
                cfg = GameConfigMgr.Instance.GetItemConfig(info.Id);
                if (cfg.ItemType == 3)
                    result.Add(info);
            }
        }
        //碎片
        if (restype == ItemTypeConst.DEBRIS && kindType == KindType.KONG)
        {
            foreach (ItemInfo info in valColl)
            {
                cfg = GameConfigMgr.Instance.GetItemConfig(info.Id);
                if (cfg.ItemType == 4)
                    result.Add(info);
            }
        }
        result.Sort(OnSortInfo);
        return result;
    }

    public int OnSortInfo(ItemInfo v1, ItemInfo v2)
    {
        ItemConfig cfg1 = GameConfigMgr.Instance.GetItemConfig(v1.Id);
        ItemConfig cfg2 = GameConfigMgr.Instance.GetItemConfig(v2.Id);
        if (cfg1.ItemType != 4)
        {
            if (cfg1.Quality != cfg2.Quality)
                return cfg1.Quality > cfg2.Quality ? -1 : 1;
            else if (cfg1.Quality == cfg2.Quality && cfg1.ShowStar != cfg2.ShowStar)
                return cfg1.ShowStar > cfg2.ShowStar ? -1 : 1;
            else if (cfg1.Quality == cfg2.Quality && cfg1.EquipType != cfg2.EquipType)
                return cfg1.EquipType < cfg2.EquipType ? -1 : 1;
            else
                return v1.Id > v2.Id ? -1 : 1;
        }
        else
        {
            if (cfg1.ComposeDropID < 10000 && cfg2.ComposeDropID > 10000)
            {
                return -1;
            }
            else if(cfg1.ComposeDropID > 10000 && cfg2.ComposeDropID < 10000)
            {
                return 1;
            }
            else if(cfg1.ComposeDropID < 10000 && cfg2.ComposeDropID < 10000)
            {
                CardConfig config1 = GameConfigMgr.Instance.GetCardConfig(cfg1.ComposeDropID * 100 + 1);
                CardConfig config2 = GameConfigMgr.Instance.GetCardConfig(cfg2.ComposeDropID * 100 + 1);
                if (config1.Rarity != config2.Rarity)
                    return config1.Rarity > config2.Rarity ? -1 : 1;
                else if (config1.Rarity == config2.Rarity && config1.Camp != config2.Camp)
                    return config1.Camp < config2.Camp ? -1 : 1;
                else
                    return config1.ClientID < config2.ClientID ? -1 : 1;
            }
            else
            {
                return cfg1.ComposeDropID > cfg2.ComposeDropID ? -1 : 1;
            }
        }
    }

    public int GetItemCountById(int itemCfgId)
    {
        if (_dictAllItems.ContainsKey(itemCfgId))
            return _dictAllItems[itemCfgId].Value;
        return 0;
    }

    public ItemInfo GetItemById(int itemCfgId)
    {
        if (_dictAllItems.ContainsKey(itemCfgId))
            return _dictAllItems[itemCfgId];
        return null;
    }

    public ItemInfo GetEquipFighting(int equipType,int fighting)
    {
        ItemInfo itemInfo = null;
        int tmp;
        ItemConfig cfg;
        foreach (ItemInfo info in _dictAllItems.Values)
        {
            cfg = GameConfigMgr.Instance.GetItemConfig(info.Id);
            if (cfg.EquipType != equipType)
                continue;
            tmp = cfg.BattlePower;
            if (tmp > fighting)
            {
                fighting = tmp;
                itemInfo = info;
            }
        }
        return itemInfo;
    }

    private void OnItemFusion(S2CItemFusionResponse value)
    {
        List<ItemInfo> items = new List<ItemInfo>();
        items.AddRange(value.Items);
        items.Sort((x, y) => x.Id.CompareTo(y.Id));
        DispathEvent(BagEvent.ItemFusionRefresh, items);
    }

    private void OnItemSell(S2CItemSellResponse value)
    {
        DispathEvent(BagEvent.ItemSellResult, value.ItemId, value.ItemNum);
    }

    #region receive data proccess method (static method)
    public static void DoUpdateItem(S2CItemsUpdate value)
    {
        Instance.OnUpdateItem(value);
    }

    public static void DoItemSync(S2CItemsSync value)
    {
        Instance.OnItemSync(value);
    }

    public static void DoFusionItem(S2CItemFusionResponse value)
    {
        Instance.OnItemFusion(value);
    }

    public static void DoSellItem(S2CItemSellResponse value)
    {
        Instance.OnItemSell(value);
    }

    public static void DoItemUpgradeResponse(S2CItemUpgradeResponse value)
    {
        int id = 0;
        for (int i = 0; i < value.NewItems.Count; i++)
            id = value.NewItems[i].Id;
        IList<ItemInfo> listNewItem = value.NewItems;
        Instance.DispathEvent(BagEvent.ItemUpGradeBack, id, value.RoleId, listNewItem);
    }

    public static void DoItemUpgradeResponse(S2CRoleLeftSlotResultSaveResponse value)
    {
        Instance.DispathEvent(BagEvent.ItemUpGradeSaveBack, value.RoleId);
    }

    public static void DoItemUpgradeByOneKey(S2CItemOneKeyUpgradeResponse value)
    {
        IList<ItemInfo> listResultItem = value.ResultItems;
        for (int i = listResultItem.Count - 1; i >= 0; i--)
        {
            for (int j = 0; j < value.CostItems.Count; j++)
            {
                if (listResultItem[i].Id == value.CostItems[j].Id)
                {
                    listResultItem[i].Value -= value.CostItems[j].Value;
                    if (listResultItem[i].Value <= 0)
                        listResultItem.RemoveAt(i);
                    break;
                }
            }
        }
        Instance.DispathEvent(BagEvent.OneKeyUpGrade, listResultItem);
        LogHelper.LogWarning("[BagDataModel.DoItemUpgradeByOneKey() => itemIds:" + value.ItemIds + "]");
    }
    #endregion

    protected override void DoClearData()
    {
        base.DoClearData();
        if (_dictAllItems != null)
            _dictAllItems.Clear();
    }
}