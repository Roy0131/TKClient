using Msg.ClientMessage;
using System.Collections.Generic;

public class CardDataVO : DataBaseVO
{
    public CardConfig mCardConfig { get; private set; }
    public int mCardID { get; private set; }
    public int mCardRank { get; set; } //品阶
    public int mCardLevel { get; private set; }
    public int mCardCfgId { get; private set; }
    public int mBattlePower { get; private set; }
    public bool mBlLock { get; set; }
    public int mState { get; private set; }
    public int mCardTableId { get; private set; }
    public int mPlayerId { get; private set; }
    public List<string> mConditionLst = new List<string>();

    private Dictionary<int, int> _dictEquips;
    private Dictionary<int, int> _dictAttris;
    private Dictionary<int, int> _dictSuits; //套装， key 套装id, value 套装数量

    private Dictionary<int, int> _dictOldAttri;

    public int mCardCount { get; set; } = 1; //TODO:ItemInfo接口逻辑用

    public CardDataVO()
    {
        mCardID = 0;
        _dictEquips = new Dictionary<int, int>();
        _dictAttris = new Dictionary<int, int>();
        _dictOldAttri = new Dictionary<int, int>();
        _dictSuits = new Dictionary<int, int>();
    }

    public CardDataVO(int tableId, int rank = 1, int level = 1, Dictionary<int, int> dictEquips = null)
    {
        mCardID = 0;
        mBlLock = false;
        _dictEquips = dictEquips == null ? new Dictionary<int, int>() : dictEquips;
        _dictAttris = new Dictionary<int, int>();
        _dictSuits = new Dictionary<int, int>();
        ParseData(tableId, rank, level);
    }

    protected override void OnInitData<T>(T value)
    {
        Role role = value as Role;
        mCardID = role.Id;
        mPlayerId = role.PlayerId;
        mBlLock = role.IsLock;
        mState = role.State;

        _dictEquips.Clear();
        for (int i = 0; i < role.Equips.Count; i++)
            _dictEquips.Add(i, role.Equips[i]);

        ParseData(role.TableId, role.Rank, role.Level);
    }

    protected override void OnRefreshData<T>(T value)
    {
        _dictOldAttri.Clear();
        if (_dictAttris != null && _dictAttris.Count > 0)
        {
            foreach (var kv in _dictAttris)
                _dictOldAttri[kv.Key] = kv.Value;
        }

        OnInitData(value);
    }

    private void ParseData(int tableId, int rank = 1, int level = 1)
    {
        mCardCfgId = tableId * 100 + rank;
        mCardConfig = GameConfigMgr.Instance.GetCardConfig(mCardCfgId);

        mCardRank = rank;
        mCardLevel = level;
        mCardTableId = mCardConfig.ID;
        CalcRoleAttr();
    }

    #region calc role attribute
    private void CalcRoleAttr()
    {
        _dictAttris.Clear();
        _dictSuits.Clear();
        mBattlePower = mCardConfig.BattlePower + (mCardLevel - 1) * mCardConfig.BattlePowerGrowth / 100;
        _dictAttris.Add(AttributesType.HP, mCardConfig.BaseHP + (mCardLevel - 1) * mCardConfig.GrowthHP / 100);
        _dictAttris.Add(AttributesType.ATTACK, mCardConfig.BaseAttack + (mCardLevel - 1) * mCardConfig.GrowthAttack / 100);
        _dictAttris.Add(AttributesType.DEFENSE, mCardConfig.BaseDefence + (mCardLevel - 1) * mCardConfig.GrowthDefence / 100);

        if (!string.IsNullOrEmpty(mCardConfig.PassiveSkillID))
        {
            string[] skills = mCardConfig.PassiveSkillID.Split(',');
            int skillId;
            SkillConfig skillConfig;
            for (int i = 0; i < skills.Length; i++)
            {
                skillId = int.Parse(skills[i]);
                skillConfig = GameConfigMgr.Instance.GetSkillConfig(skillId);
                if (skillConfig != null)
                    ParseAttr(skillConfig.SkillAttr);
            }
        }

        if (_dictEquips != null && _dictEquips.Count > 0)
        {
            int equipPower = 0;
            ItemConfig itemConfig;
            for (int i = 1; i <= 6; i++)
            {
                if (_dictEquips[i] != 0)
                {
                    itemConfig = GameConfigMgr.Instance.GetItemConfig(_dictEquips[i]);
                    if (itemConfig == null)
                        continue;
                    equipPower += itemConfig.BattlePower;
                    ParseAttr(itemConfig.EquipAttr);
                    if (itemConfig.SuitID > 0)
                    {
                        if (_dictSuits.ContainsKey(itemConfig.SuitID))
                            _dictSuits[itemConfig.SuitID]++;
                        else
                            _dictSuits[itemConfig.SuitID] = 1;
                    }
                }
            }
            if (_dictSuits.Count > 0)
            {
                SuitConfig suitCfg;
                foreach (var kv in _dictSuits)
                {
                    if (kv.Value < 2)
                        continue;
                    suitCfg = GameConfigMgr.Instance.GetSuitConfig(kv.Key);
                    if (suitCfg == null)
                    {
                        LogHelper.LogError("suit id:" + kv.Key + " suitconfig not found!!!!");
                        continue;
                    }
                    switch (kv.Value)
                    {
                        case 2:
                            equipPower += suitCfg.BpowerSuit2;
                            ParseAttr(suitCfg.AttrSuit2);
                            break;
                        case 3:
                            equipPower += suitCfg.BpowerSuit2;
                            ParseAttr(suitCfg.AttrSuit2);
                            equipPower += suitCfg.BpowerSuit3;
                            ParseAttr(suitCfg.AttrSuit3);
                            break;
                        case 4:
                            equipPower += suitCfg.BpowerSuit2;
                            ParseAttr(suitCfg.AttrSuit2);
                            equipPower += suitCfg.BpowerSuit3;
                            ParseAttr(suitCfg.AttrSuit3);
                            equipPower += suitCfg.BpowerSuit4;
                            ParseAttr(suitCfg.AttrSuit4);
                            break;
                    }
                }
            }
            mBattlePower += equipPower;
        }
        if (_dictAttris.ContainsKey(AttributesType.HP_PERCENT))
            _dictAttris[AttributesType.HP] = _dictAttris[AttributesType.HP] * (10000 + _dictAttris[AttributesType.HP_PERCENT]) / 10000;
        if (_dictAttris.ContainsKey(AttributesType.ATTACK_PERCENT))
            _dictAttris[AttributesType.ATTACK] = _dictAttris[AttributesType.ATTACK] * (10000 + _dictAttris[AttributesType.ATTACK_PERCENT]) / 10000;
        if (_dictAttris.ContainsKey(AttributesType.DEFENSE_PERCENT))
            _dictAttris[AttributesType.DEFENSE] = _dictAttris[AttributesType.DEFENSE] * (10000 + _dictAttris[AttributesType.DEFENSE_PERCENT]) / 10000;
    }

    private void ParseAttr(string attrValue)
    {
        if (string.IsNullOrEmpty(attrValue))
            return;
        string[] attrs = attrValue.Split(',');
        if (attrs.Length % 2 != 0)
        {
            LogHelper.LogWarning("[CardDataVO.ParseAttr() => attrvalue:" + attrValue + " format error!!!]");
            return;
        }

        int attrType;
        int value;
        for (int i = 0; i < attrs.Length; i += 2)
        {
            attrType = int.Parse(attrs[i]);
            value = int.Parse(attrs[i + 1]);
            if (_dictAttris.ContainsKey(attrType))
                _dictAttris[attrType] += value;
            else
                _dictAttris[attrType] = value;
        }
    }
    #endregion

    public override void Dispose()
    {
        mCardConfig = null;
        base.Dispose();
    }

    public int GetEquipIdByEquipType(int equipType)
    {
        if (_dictEquips.ContainsKey(equipType))
            return _dictEquips[equipType];
        return 0;
    }

    public int GetAttriByType(int attrType)
    {
        if (_dictAttris.ContainsKey(attrType))
            return _dictAttris[attrType];
        return 0;
    }

    public Dictionary<int, int> DictEquipment
    {
        get { return _dictEquips; }
    }

    public int GetSuitCount(int suitID)
    {
        if (_dictSuits.ContainsKey(suitID))
            return _dictSuits[suitID];
        return 0;
    }

    public bool BlTopRank
    {
        get { return mCardConfig.BreakShow == 0; }
    }

    public bool mBlInBattle
    {
        get
        {
            if (mBlLock || mState != 0)
                return true;
            return LocalDataMgr.CheckCardInBattle(mCardID);
        }
    }
   

    public bool BlEntityCard
    {
        get { return mCardID > 0; }
    }

    public void RefreshCondition(string value)
    {
        mConditionLst.Add(value);
    }

    public int Comparison(int type)
    {
        if (_dictAttris.ContainsKey(type))
        {
            if (_dictOldAttri.ContainsKey(type))
                return _dictAttris[type] - _dictOldAttri[type];
            else
                return _dictAttris[type];
        }
        else
        {
            if (_dictOldAttri.ContainsKey(type))
                return -_dictOldAttri[type];
            else
                return 0;
        }
    }

    public void OnBattlePower(int power)
    {
        mBattlePower = power;
    }

    public void OnDictAttris(int attKey,int attValue)
    {
        if (_dictAttris.ContainsKey(attKey))
            _dictAttris[attKey] = attValue;
    }

    public void OnLevel(int level)
    {
        mCardLevel = level;
    }
}