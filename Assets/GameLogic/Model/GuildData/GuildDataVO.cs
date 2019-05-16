using UnityEngine;
using Msg.ClientMessage;

public class GuildDataVO : DataBaseVO
{
    public int mGuildId { get; private set; }
    public string mGuildName { get; private set; }
    public int mLevel { get; private set; }
    public int mExp { get; private set; }
    public int mLogo { get; private set; }
    public int mCurMembers { get; private set; }
    public int mMaxMembers { get; private set; }
    public string mNoticeContent { get; private set; }
    public int mDonateNum { get; private set; }
    public int mDonateMaxNum { get; private set; }
    public int mPresidentID { get; private set; }
    public string mPresidentName { get; private set; }
    public string mLogoIcon { get; private set; }
    //public int mOffice { get; private set; }
    public GuildOfficeType mOfficeType { get; private set; }
    public GuildLevelUpConfig mLevelUpConfig { get; private set; }

    private int _dismissTargetTime;
    private int _signTargetTime;
    private int _askDonateTargetTime;
    private int _donateResetTargetTime;
    
    protected override void OnInitData<T>(T value)
    {
        if (value.GetType() == typeof(GuildInfo))
        {
            GuildInfo gi = value as GuildInfo;
            mGuildId = gi.Id;
            mGuildName = gi.Name;
            mLevel = gi.Level;
            mExp = gi.Exp;
            mLogo = gi.Logo;
            mNoticeContent = gi.Anouncement;
            mDonateNum = gi.DonateNum;
            mDonateMaxNum = gi.MaxDonateNum;
            mCurMembers = gi.MemberNum;
            mMaxMembers = gi.MemberNumLimit;

            mPresidentID = gi.President;
            mPresidentName = gi.PresidentName;
            mOfficeType = (GuildOfficeType)gi.Position;
            if (gi.Position == 0)
                mOfficeType = mPresidentID == HeroDataModel.Instance.mHeroPlayerId ? GuildOfficeType.President : GuildOfficeType.Member;

            int curIntTime = (int)Time.realtimeSinceStartup;
            _dismissTargetTime = curIntTime + gi.DismissRemainSeconds;
            _signTargetTime = curIntTime + gi.SignRemainSeconds;
            _askDonateTargetTime = curIntTime + gi.AskDonateRemainSeconds;
            _donateResetTargetTime = curIntTime + gi.DonateResetRemainSeconds;
        }
        else
        {
            GuildBaseInfo gb = value as GuildBaseInfo;
            mGuildId = gb.Id;
            mGuildName = gb.Name;
            mLevel = gb.Level;
            mLogo = gb.Logo;
            mCurMembers = gb.MemberNum;
            mMaxMembers = gb.MemberNumLimit;
        }
        GuildMarkConfig config = GameConfigMgr.Instance.GetGuildMarkConfig(mLogo);
        if (config == null)
        {
            LogHelper.LogWarning("[GuildDataVO.OnInitData() => guild logo id:" + mLogo + " config not found!!!]");
            return;
        }
        mLogoIcon = config.Icon;
        mLevelUpConfig = GameConfigMgr.Instance.GetGuildLevelUpConfig(mLevel);
    }

    public void AddNewMemberCount(int count)
    {
        mCurMembers = (mCurMembers + count) > mMaxMembers ? mMaxMembers : mCurMembers + count;
    }

    public int DisMissRemainTime
    {
        get { return _dismissTargetTime - (int)Time.realtimeSinceStartup; }
    }

    public int SignRemainTime
    {
        get { return _signTargetTime - (int)Time.realtimeSinceStartup; }
    }

    public int AskDonateRemainTime
    {
        get { return _askDonateTargetTime - (int)Time.realtimeSinceStartup; }
    }

    public int DonateResetRemainTime
    {
        get { return _donateResetTargetTime - (int)Time.realtimeSinceStartup; }
    }

    public void UpDateSignData(S2CGuildSignInResponse value)
    {
        _signTargetTime = (int)Time.realtimeSinceStartup + value.NextSignInRemainSeconds;
        mExp = value.GuildExp;
        mMaxMembers = value.MemberNumLimit;
        if(value.IsLevelup)
        {
            mLevel = value.GuildLevel;
            mLevelUpConfig = GameConfigMgr.Instance.GetGuildLevelUpConfig(mLevel);
            GuildDataModel.Instance.DispathEvent(GuildEvent.GuildLevelUp);
        }
    }

    public void UpDateAskDinateData()
    {
        _askDonateTargetTime = (int)Time.realtimeSinceStartup + GameConst.GuildDonateTime;
    }

    public void UpdateDismissGuild(int time)
    {
        _dismissTargetTime = (int)Time.realtimeSinceStartup + time;
    }

    public void UpdateGuildNameAndIcon(string name, int logo)
    {
        if (mGuildName != name)
        {
            mGuildName = name;
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001211));
            GuildDataModel.Instance.DispathEvent(GuildEvent.GuildNameRefresh);
        }
        if (mLogo != logo)
        {
            mLogo = logo;
            GuildMarkConfig config = GameConfigMgr.Instance.GetGuildMarkConfig(mLogo);
            if (config == null)
            {
                LogHelper.LogWarning("[GuildDataVO.UpdateGuildNameAndIcon() => guild logo id:" + mLogo + " config not found!!!]");
                return;
            }
            mLogoIcon = config.Icon;
            mLevelUpConfig = GameConfigMgr.Instance.GetGuildLevelUpConfig(mLevel);
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001212));
            GuildDataModel.Instance.DispathEvent(GuildEvent.GuildIconRefresh);
        }
    }

    public void UpdateGuildNotice(string notice)
    {
        if (mNoticeContent == notice)
            return;
        mNoticeContent = notice;
        PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001213));
        GuildDataModel.Instance.DispathEvent(GuildEvent.GuildNoticeRefresh);
    }

    public void UpdatePresidentId(int id)
    {
        mPresidentID = id;
        if (id == HeroDataModel.Instance.mHeroPlayerId)
        {
            mOfficeType = GuildOfficeType.President;
        }
        else
        {
            if (mOfficeType == GuildOfficeType.President)
                mOfficeType = GuildOfficeType.Member;
        }
    }

    private bool _blApplyed;
    public bool BlApplyed
    {
        get { return _blApplyed; }
        set { _blApplyed = value; }
    }

    public bool BlMgr
    {
        get { return mOfficeType != GuildOfficeType.Member; }
    }

    public void DonateNum(int num)
    {
        mDonateNum = num;
    }
}