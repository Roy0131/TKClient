using LitJson;
using System;
using System.IO;
using Msg.ClientMessage;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ChatChannelConst
{
    public const int World = 1;
    public const int Guild = 2;
    public const int Recruit = 3;
    public const int Announcement = 4;
}

public class ChatModel : ModelDataBase<ChatModel>
{
    private const string ChatSendKey = "ChatSendKey";
    private const string ChatChannelKey = "ChatChannelKey";

    public JsonData _allMonsters { get; private set; }
    private int _announcementTime = 0;
    
    private ChatDataVO _chatDataVO;

    #region chat data cache operate
    private static string _chatFile = FileConst.CachePath + "chat/data.bin";
    private void ReadCacheChatFile()
    {
        _chatFile = FileConst.CachePath + "chat/" + HeroDataModel.Instance.mHeroPlayerId + "/data.bin";
        if (File.Exists(_chatFile))
        {
            try
            {
                using (FileStream file = File.Open(_chatFile, FileMode.Open))
                {
                    var bf = new BinaryFormatter();
                    _chatDataVO = bf.Deserialize(file) as ChatDataVO;
                    file.Close();
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogWarning("[ChatModel.ReadCacheChatFile() => read location chat data failed]");
            }
        }
        if (_chatDataVO == null)
        {
            _chatDataVO = new ChatDataVO();
            LogHelper.Log("[ChatModel.ReadCacheChatFile() => 本地聊天缓存数据为空]");
        }
    }

    public void SaveChatDataToCache()
    {
        if (_chatDataVO == null || _chatDataVO.BlEmptyData)
        {
            LogHelper.Log("[ChatModel.SaveChatDataToCache() => 聊天数据为空或者未改变,不需要缓存]");
            return;
        }
        //Debuger.Log(dirPath);
        try
        {
            string dirPath = Path.GetDirectoryName(_chatFile);
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            using (FileStream file = File.Open(_chatFile, FileMode.OpenOrCreate))
            {
                var bf = new BinaryFormatter();
                bf.Serialize(file, _chatDataVO);
                file.Close();
            }
        }
        catch (Exception ex)
        {
            //Debuger.LogWarning("");
        }
    }

    public void DeleteChatCacheData()
    {
        if (File.Exists(_chatFile))
            File.Delete(_chatFile);
    }

    #endregion

    protected override void OnInit()
    {
        base.OnInit();
        ReadCacheChatFile();
        if (_chatDataVO != null)
        {
            List<ChatItemDataVO> lstAnnouncementDatas = new List<ChatItemDataVO>();
            lstAnnouncementDatas = _chatDataVO.GetChatDataByChannel(ChatChannelConst.Announcement);
            if (lstAnnouncementDatas != null && lstAnnouncementDatas.Count > 0 && lstAnnouncementDatas[lstAnnouncementDatas.Count - 1].mContent.Length > 0)
            {
                _allMonsters = JsonMapper.ToObject(lstAnnouncementDatas[lstAnnouncementDatas.Count - 1].mContent);
                _announcementTime = lstAnnouncementDatas[lstAnnouncementDatas.Count - 1].mSendTime + lstAnnouncementDatas[lstAnnouncementDatas.Count - 1].mExtraValue - HeroDataModel.Instance.mSystemTime + (int)Time.realtimeSinceStartup;
                OnJsonDataLanguage(_allMonsters);
            }
        }
    }

    public List<ChatItemDataVO> GetChannelChatDatas(int channel)
    {
        return _chatDataVO.GetChatDataByChannel(channel);
    }

    /// <summary>
    /// 发送世界聊天消息
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="content"></param>
    public void ReqChat(int channel, string content)
    {
        if (CheckNeedRequest(ChatSendKey + channel, 10.0f))
            GameNetMgr.Instance.mGameServer.ReqChat(channel, content);
        else
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000110));
    }

    public static void DoChatData(S2CChatResponse value)
    {
        Instance.AddLastReqTime(ChatSendKey + value.Channel);
        Instance._chatDataVO.AddChatDataVO(value.Channel, value.Content.ToStringUtf8());
        Instance.DispathEvent(ChatEvent.ChatRefresh, value.Channel);
    }

    /// <summary>
    /// 向服务器请求拉取相应频道的聊天消息
    /// </summary>
    /// <param name="channel"></param>
    /// <param name="value"></param>
    public void ReqChatMsgPull(int channel)
    {
        if (CheckNeedRequest(ChatChannelKey + channel, 10f))
            GameNetMgr.Instance.mGameServer.ReqChatMsgPull(channel);
    }

    /// <summary>
    /// 处理服务器返回的相应频道的聊天消息
    /// </summary>
    /// <param name="value"></param>
    private void InitChatMsgPullData(S2CChatMsgPullResponse value)
    {
        if (value.Channel == ChatChannelConst.Announcement && value.Items.Count > 0)
        {
            for (int i = 0; i < value.Items.Count; i++)
                    _chatDataVO.AddChatDataVO(value.Channel, value.Items[i]);

            int index = 0;
            for (int i = 0; i < value.Items.Count - 1; i++)
                index = value.Items[index].SendTime > value.Items[i + 1].SendTime ? index : i + 1;
            if (HeroDataModel.Instance.mSystemTime - value.Items[index].SendTime < value.Items[index].ExtraValue)
            {
                if (value.Items[index].Content.Length > 0)
                {
                    _allMonsters = JsonMapper.ToObject(value.Items[index].Content.ToStringUtf8());
                    _announcementTime = value.Items[index].SendTime + value.Items[index].ExtraValue - HeroDataModel.Instance.mSystemTime + (int)Time.realtimeSinceStartup;
                    OnJsonDataLanguage(_allMonsters);
                }
            }
            return;
        }
        Instance.AddLastReqTime(ChatChannelKey + value.Channel);
        if (value.Items != null && value.Items.Count > 0)
        {
            for (int i = 0; i < value.Items.Count; i++)
            {
                if (value.Items[i].PlayerId == HeroDataModel.Instance.mHeroPlayerId)
                    continue;
                else
                    _chatDataVO.AddChatDataVO(value.Channel, value.Items[i]);
            }
            RedPointEnum redPointID = RedPointHelper.GetRedPointEnum((int)RedPointEnum.Chat * 100 + value.Channel);
            RedPointDataModel.Instance.SetRedPointDataState(redPointID, true);
            DispathEvent(ChatEvent.ChatRefresh, value.Channel);
        }
    }

    public void OnJsonDataLanguage(JsonData allJsonData)
    {
        if (AnnouncementTime <= 0)
            return;
        JsonData jd;
        string content = "";
        for (int j = 0; j < allJsonData.Count; j++)
        {
            jd = allJsonData[j];
            if ((SystemLanguage)int.Parse(jd["Id"].ToString()) == LocalDataMgr.CurLanguage)
                content = jd["Text"].ToString();
        }
        if (content != "")
            LanternMgr.Instance.ShowLantern(content);
    }

    public int AnnouncementTime
    {
        get { return _announcementTime - (int)Time.realtimeSinceStartup; }
    }

    public static void DoChatMsgPullData(S2CChatMsgPullResponse value)
    {
        Instance.InitChatMsgPullData(value);
    }

    protected override void DoClearData()
    {
        base.DoClearData();
        SaveChatDataToCache();
        _chatDataVO = null;
        _blInited = false;
        RemoveEvent();
    }
}
