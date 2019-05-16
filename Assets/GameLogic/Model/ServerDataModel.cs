using Msg.ClientMessage;
using System.Collections.Generic;

public class ServerDataVO
{
    public int mServerId { get; private set; }
    public string mServerName { get; private set; }
    public string mServerIp { get; private set; }
    public string mPlayerName { get; private set; }
    public int mPlayerLevel { get; private set; }
    public int mPlayerHead { get; private set; }


    public void OnServerInfo(int id,string name,string ip)
    {
        mServerId = id;
        mServerName = name;
        mServerIp = ip;
    }

    public void OnPlayerInfo(string playerName,int playerLevel,int playerHead)
    {
        mPlayerName = playerName;
        mPlayerLevel = playerLevel;
        mPlayerHead = playerHead;
    }
}

public class ServerDataModel : Singleton<ServerDataModel>
{
    public List<ServerDataVO> mListServerDataVO;
    public string mAcc { get; private set; }
    public string mToken { get; private set; }

    public void OnServerData(S2CLoginResponse Server)
    {
        mAcc = Server.Acc;
        mToken = Server.Token;
        mListServerDataVO = new List<ServerDataVO>();
        ServerDataVO vo;
        for (int i = 0; i < Server.Servers.Count; i++)
        {
            vo = new ServerDataVO();
            vo.OnServerInfo(Server.Servers[i].Id, Server.Servers[i].Name, Server.Servers[i].IP);
            mListServerDataVO.Add(vo);
            for (int j = 0; j < Server.InfoList.Count; j++)
            {
                if (Server.Servers[i].Id == Server.InfoList[j].ServerId)
                {
                    if (Server.Servers[i].Id == LoginHelper.ServerID)
                        vo.OnPlayerInfo(HeroDataModel.Instance.mHeroInfoData.mHeroName, HeroDataModel.Instance.mHeroInfoData.mLevel, HeroDataModel.Instance.mHeroInfoData.mIcon);
                    else
                        vo.OnPlayerInfo(Server.InfoList[j].PlayerName, Server.InfoList[j].PlayerLevel, Server.InfoList[j].PlayerHead);
                }
            }
        }
    }

    public static void DOAccountPlayer(S2CAccountPlayerListResponse value)
    {
        for (int i = 0; i < Instance.mListServerDataVO.Count; i++)
        {
            for (int j = 0; j < value.InfoList.Count; j++)
            {
                if (Instance.mListServerDataVO[i].mServerId == value.InfoList[j].ServerId)
                    if (Instance.mListServerDataVO[i].mServerId == LoginHelper.ServerID)
                        Instance.mListServerDataVO[i].OnPlayerInfo(HeroDataModel.Instance.mHeroInfoData.mHeroName, HeroDataModel.Instance.mHeroInfoData.mLevel, HeroDataModel.Instance.mHeroInfoData.mIcon);
                    else
                        Instance.mListServerDataVO[i].OnPlayerInfo(value.InfoList[j].PlayerName, value.InfoList[j].PlayerLevel, value.InfoList[j].PlayerHead);
            }
        }
    }
}
