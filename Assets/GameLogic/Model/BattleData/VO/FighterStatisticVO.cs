public class FighterStatisticVO
{
    public int mDamageCount { get; private set; }
    public int mHealCount { get; private set; }
    public int mRank { get; private set; }
    public int mLevel { get; private set; }

    public bool mBlHero { get; private set; }
    public int mItemConfigId { get; private set; }

    public FighterStatisticVO(bool blHero = false)
    {
        mBlHero = blHero;
    }

    public void InitData(int damage, int heal, int level, int itemConfigId)
    {
        mDamageCount = damage;
        mHealCount = heal;
        mLevel = level;
        mItemConfigId = itemConfigId;
    }
}