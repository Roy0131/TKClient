using Msg.ClientMessage;

public class BuffDataVO : DataBaseVO
{
    public int mSide { get; private set; }
    public int mSeatIndex { get; private set; }
    public StatusConfig mStatusConfig { get; private set; }
    public int mBuffId { get; private set; }
    public BuffDataVO()
    {

    }

    protected override void OnInitData<T>(T value)
    {
        BattleMemberBuff data = value as BattleMemberBuff;
        mSide = data.Side;
        mSeatIndex = data.Pos;
        mBuffId = data.BuffId;
        
        mStatusConfig = GameConfigMgr.Instance.GetStatusConfig(data.BuffId);
        if (mStatusConfig == null)
            LogHelper.LogError("buff id:" + data.BuffId + " config not found!!!");
    }

    public override void Dispose()
    {
        base.Dispose();
        mStatusConfig = null;
    }
}