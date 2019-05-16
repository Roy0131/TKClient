using Msg.ClientMessage;
using System.Collections.Generic;

public class ActionItemData : DataBaseVO
{
    public bool mBlInvalidNode { get; private set; } = false;
    public int mSide { get; private set; }
    public FighterDamageDataVO mAttacker { get; private set; }
    //public List<FighterDamageDataVO> mLstBeHitsFighters { get; private set; }
    //public List<FighterDamageDataVO> mLstComboBehitsFighters { get; private set; }
    public List<FighterDataVO> mSummonFighters { get; private set; }
    public SkillConfig mSkillConfig { get; private set; }
    public List<BuffDataVO> mAddBuffs { get; private set; }
    public List<BuffDataVO> mRemoveBuffs { get; private set; }
    public bool mBlSummon { get; private set; }
    public bool mBlHasCombo { get; private set; }

    private Dictionary<int, List<FighterDamageDataVO>> _dictBehitFighters;
    private Dictionary<int, List<FighterDamageDataVO>> _dictComboBehitFighters;
    private int _beHitCount;
    protected override void OnInitData<T>(T data)
    {
        BattleReportItem value = data as BattleReportItem;
        mSkillConfig = GameConfigMgr.Instance.GetSkillConfig(value.SkillId);
        if (mSkillConfig == null)
        {
            LogHelper.LogError("skill id:" + value.SkillId + " config not found, attack node is invalid!!!");
            return;
        }
        mSide = value.Side;
        mBlSummon = value.IsSummon;
        mBlHasCombo = value.HasCombo;
        mAttacker = new FighterDamageDataVO(value.SkillId);
        mAttacker.InitData(value.User);
        mAttacker.mDamage = value.User.Damage;
        //mLstBeHitsFighters = new List<FighterDamageDataVO>();
        mSummonFighters = new List<FighterDataVO>();
        //mLstComboBehitsFighters = new List<FighterDamageDataVO>();
        _dictBehitFighters = new Dictionary<int, List<FighterDamageDataVO>>();
        _dictComboBehitFighters = new Dictionary<int, List<FighterDamageDataVO>>();

        string[] hits = mSkillConfig.BulletShowTime.Split(',');
        _beHitCount = hits.Length;

        AddBehiters(value.BeHiters, _dictBehitFighters);
        AddIncBuffData(value.AddBuffs);
        AddRemoveBuffData(value.RemoveBuffs);
        AddSummonFighter(value.SummonNpcs);

        mBlInvalidNode = true;
    }

    public override void Dispose()
    {
        base.Dispose();
        if (mAttacker != null)
        {
            mAttacker.Dispose();
            mAttacker = null;
        }
        if (_dictComboBehitFighters != null)
        {
            foreach (var kv in _dictComboBehitFighters)
                RemoveChildDataVO(kv.Value);
            _dictComboBehitFighters.Clear();
            _dictComboBehitFighters = null;
        }
        if (_dictBehitFighters != null)
        {
            foreach (var kv in _dictBehitFighters)
                RemoveChildDataVO(kv.Value);
            _dictBehitFighters.Clear();
            _dictBehitFighters = null;
        }
        if (mSummonFighters != null)
        {
            RemoveChildDataVO(mSummonFighters);
            mSummonFighters = null;
        }
        if (mAddBuffs != null)
        {
            RemoveChildDataVO(mAddBuffs);
            mAddBuffs = null;
        }
        if (mRemoveBuffs != null)
        {
            RemoveChildDataVO(mRemoveBuffs);
            mRemoveBuffs = null;
        }

        mSkillConfig = null;
    }

    private void AddSummonFighter(IList<BattleMemberItem> value)
    {
        if (value != null && value.Count > 0)
        {
            for (int i = 0; i < value.Count; i++)
            {
                FighterDataVO data = new FighterDataVO();
                data.InitData(value[i]);
                mSummonFighters.Add(data);
            }
        }
    }

    private void AddBehiters(IList<BattleFighter> value, Dictionary<int, List<FighterDamageDataVO>> dictValue, bool blCombo = false)
    {
        if (value != null && value.Count > 0)
        {
            int skillId;
            List<FighterDamageDataVO> lstValue;
            int damage;
            int[] d = new int[value.Count];
            for (int k = 0; k < value.Count; k++)
                d[k] = value[k].Damage;
            int count = _beHitCount;
            for (int j = 1; j <= _beHitCount; j++)
            {
                lstValue = new List<FighterDamageDataVO>();
                for (int i = 0; i < value.Count; i++)
                {
                    skillId = blCombo ? 0 : mSkillConfig.ID;
                    FighterDamageDataVO data = new FighterDamageDataVO(skillId);
                    data.InitData(value[i]);
                    if (d[i] != 0)
                    {
                        damage = (int)((1f / (float)count) * d[i]);
                        d[i] -= damage;
                        data.mDamage = damage;
                    }
                    else
                        data.mDamage = d[i];
                    data.mHasMultiDamage = j != _beHitCount;
                    lstValue.Add(data);
                }
                count--;
                dictValue[j] = lstValue;
            }
        }
    }

    public bool HasBehitFighters
    {
        get { return _dictBehitFighters != null && _dictBehitFighters.Count > 0; }
    }

    public bool HasComboBehitFighters
    {
        get { return _dictComboBehitFighters != null && _dictComboBehitFighters.Count > 0; }
    }

    public List<FighterDamageDataVO> GetBehitFighters(int idx)
    {
        if (_dictBehitFighters.ContainsKey(idx))
            return _dictBehitFighters[idx];
        return null;
    }

    public List<FighterDamageDataVO> GetComboBehitFighters(int idx)
    {
        if (_dictComboBehitFighters.ContainsKey(idx))
            return _dictComboBehitFighters[idx];
        return null;
    }

    private void AddIncBuffData(IList<BattleMemberBuff> value)
    {
        if (value != null && value.Count > 0)
        {
            if (mAddBuffs == null)
                mAddBuffs = new List<BuffDataVO>();
            for (int i = 0; i < value.Count; i++)
            {
                BuffDataVO data = new BuffDataVO();
                data.InitData(value[i]);
                mAddBuffs.Add(data);
            }
        }
    }

    private void AddRemoveBuffData(IList<BattleMemberBuff> value)
    {
        if (value != null && value.Count > 0)
        {
            if (mRemoveBuffs == null)
                mRemoveBuffs = new List<BuffDataVO>();
            for (int i = 0; i < value.Count; i++)
            {
                BuffDataVO data = new BuffDataVO();
                data.InitData(value[i]);
                mRemoveBuffs.Add(data);
            }
        }
    }

    public void AddComboReportItem(BattleReportItem value)
    {
        AddBehiters(value.BeHiters, _dictComboBehitFighters, true);
        AddSummonFighter(value.SummonNpcs);
        AddIncBuffData(value.AddBuffs);
        AddRemoveBuffData(value.RemoveBuffs);
    }
}

public class ActionNodeData : DataBaseVO
{
    public List<ActionItemData> mActionItemDatas;
    protected override void OnInitData<T>(T data)
    {
        BattleReportItem value = data as BattleReportItem;
        mActionItemDatas = new List<ActionItemData>();
        ActionItemData actionItem = new ActionItemData();
        actionItem.InitData(value);
        mActionItemDatas.Add(actionItem);
    }

    public override void Dispose()
    {
        base.Dispose();
        if (mActionItemDatas != null)
        {
            for (int i = mActionItemDatas.Count - 1; i >= 0; i--)
                mActionItemDatas[i].Dispose();
            mActionItemDatas.Clear();
            mActionItemDatas = null;
        }
    }

    public void AddComboReportItem(BattleReportItem value)
    {
        SkillConfig config = GameConfigMgr.Instance.GetSkillConfig(value.SkillId);
        if (config == null)
        {
            LogHelper.Log("skillid:" + value.SkillId + " skillconfig not found!!");
            return;
        }
        if (config.SkillAnimType == (int)SkillAnimtionType.None)
        {
            //attach to parent ActionNodeData
            ActionItemData parent = null;
            int len = mActionItemDatas.Count;
            while (len > 0)
            {
                parent = mActionItemDatas[len - 1];
                if (parent != null && parent.mBlHasCombo)
                    break;
                len--;
            }
            if (parent == null)
            {
                LogHelper.Log("combo skill action");
                return;
            }
            parent.AddComboReportItem(value);
        }
        else
        {
            ActionItemData actionItem = new ActionItemData();
            actionItem.InitData(value);
            mActionItemDatas.Add(actionItem);
        }
    }
}