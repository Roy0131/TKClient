using Msg.ClientMessage;

public class GuildMemberVO : DataBaseVO
{
    public int mPlayerId { get; private set; }
    public string mPlayerName { get; private set; }
    public int mPlayerLevel { get; private set; }
    public int mIcon { get; private set; }
    public int mLastOnlineTime { get; private set; }
    public int mJointTime { get; private set; }
    public GuildOfficeType mOfficeType { get; private set; }
    protected override void OnInitData<T>(T value)
    {
        if (value.GetType() == typeof(GuildMember))
        {
            GuildMember gm = value as GuildMember;
            mPlayerId = gm.Id;
            mPlayerName = gm.Name;
            mPlayerLevel = gm.Level;
            mIcon = gm.Head;
            mOfficeType = (GuildOfficeType)gm.Position;
            mLastOnlineTime = gm.LastOnlineTime;
            mJointTime = gm.JoinTime;
        }
        else
        {
            PlayerInfo pi = value as PlayerInfo;
            mPlayerId = pi.Id;
            mPlayerName = pi.Name;
            mPlayerLevel = pi.Level;
            mIcon = pi.Head;
        }
    }

    public string OfficeTitle
    {
        get
        {
            switch (mOfficeType)
            {
                case GuildOfficeType.Member:
                    return LanguageMgr.GetLanguage(6001214);
                case GuildOfficeType.President:
                    return LanguageMgr.GetLanguage(6001215);
                case GuildOfficeType.Office:
                    return LanguageMgr.GetLanguage(6001216);
            }
            return LanguageMgr.GetLanguage(6001214);
        }
    }

    public void UpdateOfficer(int officer)
    {
        mOfficeType = (GuildOfficeType)officer;
    }
}