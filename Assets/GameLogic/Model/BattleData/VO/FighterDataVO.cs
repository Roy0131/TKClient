using Msg.ClientMessage;

public class FighterDataVO : DataBaseVO
{
    public int mId { get; private set; }
    public int mSeatIndex { get; private set; }
    public int mCurHp { get; private set; }
    public int mMaxHp { get; private set; }
    public int mEnergy { get; private set; }
    public int mSide { get; private set; }
    public int mLevel { get; private set; }
    public int mShieldMax { get; private set; }
    public int mCurShield { get; private set; }

    public float mflModelHeight { get; private set; }
    public float mflModelWidth { get; private set; }

    public CardConfig mCardConfig { get; private set; }
    public int mCardType { get; private set; }

    protected override void OnInitData<T>(T value)
    {
        BattleMemberItem itemData = value as BattleMemberItem;
        mId = itemData.Id;
        mSeatIndex = itemData.Pos;
        mCurHp = itemData.HP;
        mMaxHp = itemData.MaxHP;
        mEnergy = itemData.Energy;
        mSide = itemData.Side;
        mLevel = itemData.Level;
         mCurShield = 0;
        mShieldMax = 0;

        //Debuger.LogWarning("Fighter default value:" + mCurHp + ", " + mEnergy + ", fighter seat:" + mSide + "-" + mSeatIndex);

        int id = itemData.TableId * 100 + itemData.Rank;
        mCardConfig = GameConfigMgr.Instance.GetCardConfig(id);
        if (mCardConfig == null)
        {
            LogHelper.LogError("fighter id:" + mId + " not found cardconfig, server tableID:" + itemData.TableId + ", rank:" + itemData.Rank + ", level:" + itemData.Level);
            return;
        }
        mCardType = mCardConfig.Type;
        ModelConfig modelConfig = GameConfigMgr.Instance.GetModelConfig(mCardConfig.Model);
        if (modelConfig == null)
        {
            LogHelper.LogError("model config not found, model name:" + mCardConfig.Model);
            return;
        }
        mflModelHeight = ((float)modelConfig.Height) / 100f;
        mflModelWidth = ((float)modelConfig.Width) / 100f;
    }

    public void RefreshHpAndEnergy(int hp, int energy, int shield)
    {
        mCurHp = hp;
        mEnergy = energy;
        mCurShield = shield;
        mShieldMax = mShieldMax > mCurShield ? mShieldMax : mCurShield;
        //Debuger.LogWarning("Fighter refresh HpAnEnergy:" + mCurHp + ", " + mEnergy + ", fighter seat:" + mSide + "-" + mSeatIndex);
    }

    public override void Dispose()
    {
        base.Dispose();
        mCardConfig = null;
    }

    public string PosToString()
    {
        return mSide + "_" + mSeatIndex;
    }
}