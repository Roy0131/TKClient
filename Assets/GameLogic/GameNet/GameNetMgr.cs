using UnityEngine;

public class GameNetMgr : Singleton<GameNetMgr>
{
    #region Server Url
    public static string GAME_LOGIC_URL;
    public string mGameLoginUrl { get; private set; }
    #endregion

    public GameServer mGameServer { get; private set; }
    public LoginServer mLoginServer { get; private set; }

    public string mDomain { get; private set; }

    public void Init(ServerNameEnum svrEnum)
    {
        switch(svrEnum)
        {
            case ServerNameEnum.Singapore:
                if(GameDriver.Instance.mBlRunHot)
                {
                    mGameLoginUrl = "http://www.moyutec.com:35000/client";
                    mDomain = "http://www.moyutec.com";
                }
                else
                {
                    mGameLoginUrl = "http://www.moyutec.com:45000/client";
                    mDomain = "http://www.moyutec.com";
                }
                break;
            case ServerNameEnum.Hongkong:
                mGameLoginUrl = "http://47.74.186.77:45000/client";
                mDomain = "http://47.74.186.77";
                break;
        }
        mGameServer = new GameServer();
        mLoginServer = new LoginServer();
    }

    public void Update()
    {
        if (mLoginServer != null)
            mLoginServer.Update();
        if (mGameServer != null && mGameServer.blEnable)
            mGameServer.Update();
        if (_count > 0)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable || _blNeedRelogin)
            {
                LoadingMgr.Instance.HideRechargeMask();
                NetReconnectMgr.Instance.ShowRecconect();
            }
        }
    }

    public void Dispose()
    {
        if (mGameServer != null)
        {
            mGameServer.Dispose();
            mGameServer = null;
        }
    }

    private int _count = 0;
    private bool _blNeedRelogin = false;
    public void AddNetErrorCount()
    {
        _count++;
        if (_count >= 3)
            _blNeedRelogin = true;
    }

    public void ResetNetErrorCount()
    {
        _blNeedRelogin = false;
        _count = 0;
    }
}