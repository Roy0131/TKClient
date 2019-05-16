using Msg.ClientMessage;

public class GuildBossHurtVO : DataBaseVO
{
    public GuildStageDamageItem mDamage { get; private set; }
    public ItemInfo mRewardItem { get; private set; }

    protected override void OnInitData<T>(T value)
    {
        mDamage = value as GuildStageDamageItem;
        mRewardItem = GuildBossDataModel.Instance.GetRankReward(mDamage.Rank);
    }
}
