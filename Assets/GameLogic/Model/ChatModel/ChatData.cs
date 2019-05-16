using System;
using Msg.ClientMessage;
using System.Collections.Generic;

[Serializable]
public class ChatDataVO
{
    private List<ChatItemDataVO> _lstWorldChatDatas;
    private List<ChatItemDataVO> _lstGuildChatDatas;
    private List<ChatItemDataVO> _lstRecruitChatDatas;
    private List<ChatItemDataVO> _lstAnnouncementDatas;
    private bool _blDataChange;

    public ChatDataVO()
    {
        _lstWorldChatDatas = new List<ChatItemDataVO>();
        _lstGuildChatDatas = new List<ChatItemDataVO>();
        _lstRecruitChatDatas = new List<ChatItemDataVO>();
        _lstAnnouncementDatas = new List<ChatItemDataVO>();
        _blDataChange = false;
    }

    public void AddChatDataVO(int channel, string content, int extraValue = 0)
    {
        ChatItemDataVO vo = new ChatItemDataVO();
        vo.InitData(channel, content, extraValue);
        ProccessChatItemVO(channel, vo);
    }

    public void AddChatDataVO(int channel, ChatItem value)
    {
        ChatItemDataVO vo = new ChatItemDataVO();
        vo.InitData(channel, value);
        ProccessChatItemVO(channel, vo);
    }

    private void ProccessChatItemVO(int channel, ChatItemDataVO vo)
    {
        List<ChatItemDataVO> tmp = null;
        switch (channel)
        {
            case ChatChannelConst.World:
                tmp = _lstWorldChatDatas;
                break;
            case ChatChannelConst.Guild:
                tmp = _lstGuildChatDatas;
                break;
            case ChatChannelConst.Recruit:
                tmp = _lstRecruitChatDatas;
                break;
            case ChatChannelConst.Announcement:
                tmp = _lstAnnouncementDatas;
                break;
        }
        if (tmp == null)
            return;
        tmp.Add(vo);
        tmp.Sort((x,y)=>(x.mSendTime.CompareTo(y.mSendTime)));//按时间从低到高排序
        if (tmp.Count > 60)
            tmp.RemoveRange(0, tmp.Count - 60);
        _blDataChange = true;
    }

    public List<ChatItemDataVO> GetChatDataByChannel(int channel)
    {
        if (channel == ChatChannelConst.World)
            return _lstWorldChatDatas;
        else if (channel == ChatChannelConst.Guild)
            return _lstGuildChatDatas;
        else if (channel == ChatChannelConst.Recruit)
            return _lstRecruitChatDatas;
        else
            return _lstAnnouncementDatas;
    }

    public bool BlEmptyData
    {
        get
        {
            if (!_blDataChange)
                return true;
            return _lstGuildChatDatas.Count == 0 && _lstRecruitChatDatas.Count == 0 && _lstWorldChatDatas.Count == 0 && _lstAnnouncementDatas.Count == 0;
        }
    }
}


[Serializable]
public class ChatItemDataVO
{
    public string mContent { get; private set; }
    public int mPlayerId { get; private set; }
    public string mPlayerName { get; private set; }
    public int mPlayerHead { get; private set; }
    public int mPlayerLevel { get; private set; }
    public int mIsFriend { get; private set; }
    public int mExtraValue { get; private set; }
    public int mSendTime { get; private set; }
    public int mChatChannel { get; private set; }

    public void InitData(int channel, ChatItem value)
    {
        mContent = value.Content.ToStringUtf8();
        mPlayerId = value.PlayerId;
        mPlayerName = value.PlayerName;
        mPlayerHead = value.PlayerHead;
        mPlayerLevel = value.PlayerLevel;
        mIsFriend = value.IsFriend;
        mSendTime = value.SendTime;
        mExtraValue = value.ExtraValue;
        mChatChannel = channel;
    }

    public void InitData(int channel, string content, int extraValue = 0)
    {
        mContent = content;
        mPlayerId = HeroDataModel.Instance.mHeroPlayerId;
        mPlayerName = HeroDataModel.Instance.mHeroInfoData.mHeroName;
        mPlayerHead = HeroDataModel.Instance.mHeroInfoData.mIcon;
        mPlayerLevel = HeroDataModel.Instance.mHeroInfoData.mLevel;
        mIsFriend = 0;

        DateTime DateStart = new DateTime(1970, 1, 1, 8, 0, 0);
        mSendTime = HeroDataModel.Instance.mSystemTime;
        mExtraValue = extraValue;
        mChatChannel = channel;
    }
}
