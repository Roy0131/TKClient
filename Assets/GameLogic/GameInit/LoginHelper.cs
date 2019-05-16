using Msg.ClientMessage;
using System;

public class LoginHelper
{
    private static string _gameLogicIp = "";

    private static string _account;
    private static string _password;
    private static int _channel;
    public static string mBoundAccount { get; private set; } = "";

    public static int ServerID { get; private set; }
    public static void DoLoginNetError(int errCode)
    {
        if (_onSetPasswordMethod != null)
        {
            ResetPasswordError();
            NetErrorHelper.DoErrorCode(errCode);
        }
        else if (_onBindAccMethod != null)
        {
            BindAccountError(errCode);
            NetErrorHelper.DoErrorCode(errCode);
        }
        else
        {
            if (_blRelogin || _needClearData)
            {
                ReloginFailed();
                if (_onReloginFailed != null)
                {
                    _onReloginFailed.Invoke();
                    _onReloginFailed = null;
                }
            }
            else
                GameLoginMgr.Instance.LoginFailed(errCode);
        }
    }


    #region reset password
    private static void ResetPasswordError()
    {
        if (_onSetPasswordMethod != null)
            _onSetPasswordMethod.Invoke(-1, null);
        _onSetPasswordMethod = null;
    }

    public static void OnSetPsdResponse(S2CSetLoginPasswordResponse value)
    {
        if (_onSetPasswordMethod != null)
            _onSetPasswordMethod.Invoke(0, value);
        _onSetPasswordMethod = null;
    }


    private static Action<int, S2CSetLoginPasswordResponse> _onSetPasswordMethod;
    public static void SetPassword(string password, string newPassword, Action<int, S2CSetLoginPasswordResponse> method)
    {
        _onSetPasswordMethod = method;
        GameNetMgr.Instance.mLoginServer.ReqSetLoginPsd(LocalDataMgr.PlayerAccount, password, newPassword);
    }
    #endregion

    #region bind new account
    private static Action<int, S2CGuestBindNewAccountResponse> _onBindAccMethod;
    public static void BindNewAccount(string oldAcc, string oldPsd, string newAcc, string newPsd, Action<int, S2CGuestBindNewAccountResponse> onMethod, int channel)
    {
        _onBindAccMethod = onMethod;
        GameNetMgr.Instance.mLoginServer.ReqBindAccount(ServerID, oldAcc, oldPsd, newAcc, newPsd, channel);
    }

    public static void OnBindAccResponse(S2CGuestBindNewAccountResponse value)
    {
        if (_onBindAccMethod != null)
            _onBindAccMethod.Invoke(0, value);
        _onBindAccMethod = null;
    }

    private static void BindAccountError(int errorCode)
    {
        if (_onBindAccMethod != null)
            _onBindAccMethod.Invoke(errorCode, null);
        _onBindAccMethod = null;
    }

    #endregion

    private static bool _blRelogin = false;
    private static Action _onReloginFailed;
    public static string mToken;
    public static void ReLogin(string account, string password, int channel, Action onReloginFailed = null, bool blShowTips = true)
    {
        _needClearData = true;
        _account = account;
        _password = password;
        _channel = channel;
        _blRelogin = true;
        if (blShowTips)
            LoadingMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001262));
        _onReloginFailed = onReloginFailed;
        GameNetMgr.Instance.mLoginServer.ReqLogin(_account, _password, _channel);
    }

    private static void ReloginFailed()
    {
        _blRelogin = false;
        LoadingMgr.Instance.CloseLoading();
        PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001263));
    }

    public static void DoGameLogin(string account, string password, int channel)
    {
        _account = account;
        _password = password;
        _channel = channel;

        GameNetMgr.Instance.mLoginServer.ReqLogin(_account, _password, _channel);
    }

    public static void OnLoginResponse(S2CLoginResponse value)
    {
        ServerDataModel.Instance.OnServerData(value);

        LocalDataMgr.PlayerAccount = _account;
        LocalDataMgr.Password = _password;
        LocalDataMgr.LoginChannel = _channel;
        _gameLogicIp = value.GameIP;
        ServerID = value.LastServerId;
        string[] ips = value.GameIP.Split(':');
        _gameLogicIp = GameNetMgr.Instance.mDomain + ":" + (ips.Length == 2 ? ips[1] : ips[0]);
        GameNetMgr.GAME_LOGIC_URL = _gameLogicIp + "/client_msg";
        TDPostDataMgr.Instance.DoSetAccount(_account, ServerID);
        mToken = value.Token;
        mBoundAccount = value.BoundAccount;
        EnterGameServer(value.Acc, value.Token);
    }

    private static void DoLoginError()
    {
        GameLoginMgr.Instance.LoginFailed();
    }
    
    private static void EnterGameServer(string acc, string token)
    {
        if (_needClearData || _blRelogin)
        {
            LocalDataMgr.DeleteLocalCache();
            GameEventMgr.Instance.mGlobalDispatcher.DispathEvent(GameEventMgr.GEventClearAllData);
        }
        else
        {
            GameLoginMgr.Instance.LoginServerSuccess();
        }
        GameStageMgr.Instance.ChangeStage(StageType.Login);
        _blRelogin = false;
        _needClearData = false;
        GameNetMgr.Instance.mGameServer.ReqLoginGameServer(acc);
    }

    #region change server logic

    private static bool _needClearData = false;
    public static void ChangeServer(string acc, string token, int idServer, string ip)
    {
        _needClearData = true;
        _gameLogicIp = GameNetMgr.Instance.mDomain + ip;
        ServerID = idServer;
        LoadingMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001266));
        GameNetMgr.Instance.mLoginServer.ReqSelectServer(acc, token, idServer);
    }

    public static void OnSelectedResponse(S2CSelectServerResponse value)
    {
        GameNetMgr.GAME_LOGIC_URL = _gameLogicIp + "/client_msg";
        LogHelper.LogWarning("game logic url:" + GameNetMgr.GAME_LOGIC_URL);

        mToken = value.Token;
        EnterGameServer(value.Acc, value.Token);
    }
    #endregion
}