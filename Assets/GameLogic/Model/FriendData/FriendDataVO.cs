using Msg.ClientMessage;

public class FriendDataVO : DataBaseVO
{
    public int mPlayerId { get; private set; }
    public int mPlayerLevel { get; private set; }
    public int mPlayerIcon { get; private set; }
    public string mPlayerName { get; private set; }
    public bool mBlOnline { get; private set; }
    public int mOfflineTime { get; private set; }
    public int mGivePointsRemainTime { get; private set; }
    public int mBossId { get; private set; }
    public int mBossHpPer { get; private set; }
    public int mBattlePower { get; private set; }
    public int mGetPoints { get; private set; } //-1 已收取  0 还未赠送  >0 可收取的友情点
    public FriendBossDataVO mFriendBossVO { get; private set; }
    protected override void OnInitData<T>(T value)
    {
        if (value.GetType() == typeof(FriendInfo))
        {
            FriendInfo fd = value as FriendInfo;
            mPlayerId = fd.Id;
            mPlayerName = fd.Name;
            mPlayerLevel = fd.Level;
            mPlayerIcon = fd.Head;
            mBlOnline = fd.IsOnline;
            mOfflineTime = fd.OfflineSeconds;
            mGivePointsRemainTime = fd.RemainGivePointsSeconds;
            mBossId = fd.BossId;
            mBossHpPer = fd.BossHpPercent;
            mBattlePower = fd.Power;
            mGetPoints = fd.GetPoints;
            if (mBossId > 0)
            {
                mFriendBossVO = new FriendBossDataVO();
                mFriendBossVO.InitBossData(mBossId, mBossHpPer);
            }
        }
        else
        {
            PlayerInfo info = value as PlayerInfo;
            mPlayerId = info.Id;
            mPlayerName = info.Name;
            mPlayerLevel = info.Level;
            mPlayerIcon = info.Head;
        }
        if (string.IsNullOrEmpty(mPlayerName))
            mPlayerName = mPlayerId.ToString();
    }

    public bool BlGetOrSendPoint
    {
        get
        {
            if (mGetPoints > 0)
                return true;
            if (mGivePointsRemainTime <= 0)
                return true;
            return false;
        }
    }

    public void RefreshGivePointStatus(bool value)
    {
        if (mGivePointsRemainTime > 0)
        {
            if (!value)
                return;
        }
        mGivePointsRemainTime = value ? 1 : 0;
    }

    public void RefreshGetPointStatus(int points)
    {
        if (mGetPoints > 0)
            mGetPoints = -1;
    }

    public void RefreshBossHp(int hp)
    {
        mBossHpPer = hp;
        if (mBossHpPer == 0)
        {
            mBossId = 0;
            mFriendBossVO = null;
        }
        else
        {
            mFriendBossVO.RefreshBossHp(hp);
        }
    }
}