using System.Collections.Generic;
using Msg.ClientMessage;

public class DecomposeDataModel : ModelDataBase<DecomposeDataModel>
{
    public List<CardDataVO> mlstAllCardDataVO { get; private set; }
    private List<int> _lstRarityCond = new List<int>();
    private List<int> _lstDecomposeIds = new List<int>();
    
    public void PrePareData()
    {
        _lstRarityCond.Clear();
        ResetSelectRole();
    }

    public void ResetSelectRole()
    {
        _lstDecomposeIds.Clear();
        FilterCardData();
    }

    public bool CheckRaritySelected(int rarity)
    {
        return _lstRarityCond.Contains(rarity);
    }

    public void AddRarityCond(int rarity)
    {
        if (_lstRarityCond.Contains(rarity))
            return;
        _lstRarityCond.Add(rarity);
        FilterCardData();
    }

    public void RemoveRarityCond(int rarity)
    {
        if (!_lstRarityCond.Contains(rarity))
            return;
        _lstRarityCond.Remove(rarity);
        FilterCardData();
    }

    private void FilterCardData()
    {
        List<CardDataVO> allCards = HeroDataModel.Instance.mAllCards;
        if (mlstAllCardDataVO == null)
            mlstAllCardDataVO = new List<CardDataVO>();
        mlstAllCardDataVO.Clear();
        for (int i = 0; i < allCards.Count; i++)
        {
            if (_lstRarityCond.Count > 0)
            {
                if (_lstRarityCond.Contains(allCards[i].mCardConfig.Rarity))
                    mlstAllCardDataVO.Add(allCards[i]);
            }
            else
            {
                mlstAllCardDataVO.Add(allCards[i]);
            }
        }
        mlstAllCardDataVO.Sort(OnSortCardData);
        DispathEvent(DecomposeEvent.RefreshRoleList);
    }

    private int OnSortCardData(CardDataVO v1, CardDataVO v2)
    {
        if (object.ReferenceEquals(v1, v2) || v1 == v2)
        {
            LogHelper.LogWarning("v1 == v2, v1 id:" + v1.mCardID + ", v2 id:" + v2.mCardID);
            return 0;
        }
        if (v1 == null || v2 == null)
        {
            LogHelper.LogWarning("v1 or v2 is null..");
            return 0;
        }
        if (v1.mBlInBattle && !v2.mBlInBattle)
            return 1;
        else if (!v1.mBlInBattle && v2.mBlInBattle)
            return -1;
        if (v1.mCardConfig.Rarity != v2.mCardConfig.Rarity)
        {
            return v1.mCardConfig.Rarity > v2.mCardConfig.Rarity ? 1 : -1;
        }
        else if (v1.mCardConfig.Rarity == v2.mCardConfig.Rarity && v1.mCardLevel != v2.mCardLevel)
        {
            return v1.mCardLevel > v2.mCardLevel ? 1 : -1;
        }
        else if (v1.mCardConfig.Rarity == v2.mCardConfig.Rarity && v1.mCardLevel == v2.mCardLevel && v1.mCardConfig.Type != v2.mCardConfig.Type)
        {
            return v1.mCardConfig.Type > v2.mCardConfig.Type ? 1 : -1;
        }
        else if (v1.mCardConfig.Rarity == v2.mCardConfig.Rarity && v1.mCardLevel == v2.mCardLevel && v1.mCardConfig.Type == v2.mCardConfig.Type
            && v1.mCardConfig.Camp != v2.mCardConfig.Camp)
        {
            int[] campSort = { 1, 3, 6, 4, 2, 5 };
            int curCamp1 = campSort[v1.mCardConfig.Camp - 1];
            int curCamp2 = campSort[v2.mCardConfig.Camp - 1];
            return curCamp1 > curCamp2 ? 1 : -1;
        }
        else if (v1.mCardConfig.Rarity == v2.mCardConfig.Rarity && v1.mCardLevel == v2.mCardLevel && v1.mCardConfig.Type == v2.mCardConfig.Type
            && v1.mCardConfig.Camp == v2.mCardConfig.Camp && v1.mCardConfig.ID != v2.mCardConfig.ID)
        {
            return v1.mCardConfig.ID > v2.mCardConfig.ID ? 1 : -1;
        }
        else if (v1.mCardConfig.Rarity == v2.mCardConfig.Rarity && v1.mCardLevel == v2.mCardLevel && v1.mCardConfig.Type == v2.mCardConfig.Type
            && v1.mCardConfig.Camp == v2.mCardConfig.Camp && v1.mCardConfig.ID == v2.mCardConfig.ID && v1.mBattlePower != v2.mBattlePower)
        {
            return v1.mBattlePower > v2.mBattlePower ? -1 : 1;
        }
        else
        {
            return 0;
        }
    }

    public void AddVOToDecompose(CardDataVO vo)
    {
        if (_lstDecomposeIds.Contains(vo.mCardID) || vo.mState != 0)
            return;
        _lstDecomposeIds.Add(vo.mCardID);
        DispathEvent(DecomposeEvent.AddRoleToDecompose, vo.mCardID);
    }

    public void RemoveVOToDecompose(CardDataVO vo)
    {
        if (!_lstDecomposeIds.Contains(vo.mCardID))
            return;
        _lstDecomposeIds.Remove(vo.mCardID);
        DispathEvent(DecomposeEvent.RemoveRoleToDecompose, vo.mCardID);
    }

    public void FillAllToDecompose()
    {
        if (BlFullCard)
            return;
        for (int i = 0; i < mlstAllCardDataVO.Count; i++)
        {
            if (mlstAllCardDataVO[i].mBlLock ||mlstAllCardDataVO[i].mBlInBattle)
                continue;
            if (_lstDecomposeIds.Contains(mlstAllCardDataVO[i].mCardID))
                continue;
            if (BlFullCard)
                break;
            AddVOToDecompose(mlstAllCardDataVO[i]);
        }
    }

    public bool BlFullCard
    {
        get { return _lstDecomposeIds.Count >= 12; }
    }

    public bool BlCardContain(int cardId)
    {
        if (_lstDecomposeIds == null)
            return false;
        return _lstDecomposeIds.Contains(cardId);
    }

    public int mRoleTotalCount
    {
        get
        {
            if (mlstAllCardDataVO == null)
                return 0;
            return mlstAllCardDataVO.Count;
        }
    }

    public Dictionary<int, ItemInfo> mDictReward { get; private set; } = new Dictionary<int, ItemInfo>();
    public void PareseDecomposeReward()
    {
        mDictReward.Clear();
        LevelUpConfig lvConfig;
        CardDataVO vo;
        RankUpConfig rankUpConfig;
        ItemConfig itemConfig;
        for (int i = 0; i < _lstDecomposeIds.Count; i++)
        {
            vo = HeroDataModel.Instance.GetCardDataByCardId(_lstDecomposeIds[i]);//_lstDecomposeVO[i];
            ParseRes(vo.mCardConfig.DecomposeRes);
            if (vo.mCardLevel > 1)
            {
                lvConfig = GameConfigMgr.Instance.GetLevelUpConfig(vo.mCardLevel);
                ParseRes(lvConfig.CardDecomposeRes);
            }
            if (vo.mCardRank > 1)
            {
                rankUpConfig = GameConfigMgr.Instance.GetRankUpConfig(vo.mCardRank);
                if (vo.mCardConfig.Type == 1)
                    ParseRes(rankUpConfig.Type1DecomposeRes);
                else if (vo.mCardConfig.Type == 2)
                    ParseRes(rankUpConfig.Type2DecomposeRes);
                else if (vo.mCardConfig.Type == 3)
                    ParseRes(rankUpConfig.Type3DecomposeRes);
            }

            int equipId = 0;
            for (int j = 1; j <= 6; j++)
            {
                equipId = vo.GetEquipIdByEquipType(j);
                if (equipId != 0)
                {
                    if (j == 5)
                    {
                        itemConfig = GameConfigMgr.Instance.GetItemConfig(equipId);
                        ParseRes(itemConfig.SellReward);
                    }
                    else
                    {
                        ParseRes(equipId + ",1");
                    }
                }
            }
        }
        DispathEvent(DecomposeEvent.ShowDecomposeReward, false);
    }

    private void ParseRes(string value)
    {
        if (string.IsNullOrEmpty(value))
            return;
        string[] res = value.Split(',');
        if (res.Length % 2 != 0)
        {
            LogHelper.LogWarning("[DecomposeDataModel.ParseRes() => 分解返回资源格式错误, res:" + value + "]");
            return;
        }
        int itemId;
        int count;
        ItemInfo info;
        for (int i = 0; i < res.Length; i += 2)
        {
            itemId = int.Parse(res[i]);
            count = int.Parse(res[i + 1]);
            if (mDictReward.ContainsKey(itemId))
            {
                info = mDictReward[itemId];
                info.Value += count;
            }
            else
            {
                info = new ItemInfo();
                info.Id = itemId;
                info.Value = count;
                mDictReward.Add(itemId, info);
            }
        }
    }

    protected void OnRoleDecomposeBack(IList<ItemInfo> value)
    {
        mDictReward.Clear();
        for (int i = 0; i < value.Count; i++)
        {
            if (mDictReward.ContainsKey(value[i].Id))
                mDictReward[value[i].Id].Value += value[i].Value;
            else
                mDictReward.Add(value[i].Id, value[i]);
        }
        _lstDecomposeIds.Clear();
        DispathEvent(DecomposeEvent.ShowDecomposeReward, true);
    }

    public void ReqDecomposeRole()
    {
        if (_lstDecomposeIds == null || _lstDecomposeIds.Count == 0)
            return;
        IList<int> value = new List<int>();
        for (int i = 0; i < _lstDecomposeIds.Count; i++)
            value.Add(_lstDecomposeIds[i]);
        GameNetMgr.Instance.mGameServer.ReqRoleDecompose(value);
    }

    #region static function
    public static void DoRoleDecompose(S2CRoleDecomposeResponse value)
    {
        Instance.OnRoleDecomposeBack(value.GetItems);
    }
    #endregion

    protected override void DoClearData()
    {
        base.DoClearData();
        if (mDictReward != null)
            mDictReward.Clear();
        if (mlstAllCardDataVO != null)
            mlstAllCardDataVO.Clear();
        if (_lstRarityCond != null)
            _lstRarityCond.Clear();
        if (_lstDecomposeIds != null)
            _lstDecomposeIds.Clear();
    }
}