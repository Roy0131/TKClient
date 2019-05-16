using Msg.ClientMessage;

public class GuildDonateVO : DataBaseVO
{
    public int mPlayerID { get; private set; }
    public string mPlayerName { get; private set; }
    public int mPlayerIcon { get; private set; }
    public int mPlayerLevel { get; private set; }
    public int mDonateItemID { get; private set; }
    public int mDonateItemNum { get; private set; }
    public int mDonateItemMax { get; private set; }
    public int mAskTime { get; private set; }
    public bool mIsDonate = false;
    private int _targetAskTime;

    
    protected override void OnInitData<T>(T value)
    {
        GuildAskDonateInfo gd = value as GuildAskDonateInfo;
        mPlayerID = gd.PlayerId;
        mPlayerName = gd.PlayerName;
        mPlayerIcon = gd.PlayerHead;
        mPlayerLevel = gd.PlayerLevel;
        mDonateItemID = gd.ItemId;
        mDonateItemNum = gd.ItemNum;
        mAskTime = gd.AskTime;
        GuildDonateConfig config = GameConfigMgr.Instance.GetGuildDonateConfig(mDonateItemID);
        _targetAskTime = (int)UnityEngine.Time.realtimeSinceStartup + gd.RemainExistSeconds;
        if (config == null)
        {
            LogHelper.LogWarning("[GuildDonateVO.OnInitData() => donateId:" + mDonateItemID + " config not found!!]");
            return;
        }
        mDonateItemMax = config.RequestNum;
    }

    public int RemainCDTime
    {
        get { return _targetAskTime - (int)UnityEngine.Time.realtimeSinceStartup; }
    }

    public void OnDonate(int itemNum)
    {
        mDonateItemNum = itemNum;
    }

    public void OnIsDonate(bool isDonate)
    {
        mIsDonate = isDonate;
    }
}