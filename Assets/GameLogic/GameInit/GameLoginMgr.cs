using Framework.Core;
using UnityEngine;
using UnityEngine.UI;
using Msg.ClientMessage;

public class GameLoginMgr : Singleton<GameLoginMgr>
{
    private GameObject _uiLoginObject;
    private Transform _transform;
    private Button _startBtn;
    private Button _accountBtn;
    private Button _facebookBtn;
    private InputField _defAccount;

    private GameObject _accViewObject;
    private InputField _accInputText;
    private InputField _psdInputText;
    private Button _cancelBtn;
    private Button _okBtn;
    private Text _tipsText;

    private Text _textClick;

    private GameObject _startTipsObj;
    private Text _loginTipsText;

    private Text _textTitle;
    private Text _textAccount;
    private Text _textPassword;
    private Text _textCancel;
    private Text _textOk;
    private Text _version;

    public int mOwnerCodeVer;

    public void Init(GameObject loginObject)
    {
        mOwnerCodeVer = FileConst.GAME_PACKAGE_VERSION;
        System.Net.ServicePointManager.DefaultConnectionLimit = 50;
        _uiLoginObject = loginObject;
        _transform = _uiLoginObject.transform;

        _defAccount = Find<InputField>("DefInput");

        _startBtn = Find<Button>("StartBtn");
        _accountBtn = Find<Button>("LoginBtn");
        _facebookBtn = Find<Button>("FaceBookBtn");

        _accViewObject = _transform.Find("InputAccObject").gameObject;
        _accInputText = Find<InputField>("InputAccObject/InputAccount");
        _psdInputText = Find<InputField>("InputAccObject/InputPassword");
        _cancelBtn = Find<Button>("InputAccObject/CancelBtn");
        _okBtn = Find<Button>("InputAccObject/OkBtn");
        _tipsText = Find<Text>("InputAccObject/Tips/Text");
        _textClick = Find<Text>("StartTips/Text");

        _textTitle = Find<Text>("InputAccObject/TitleText");
        _textAccount = Find<Text>("InputAccObject/AccountText");
        _textPassword = Find<Text>("InputAccObject/PasswordText");
        _textCancel = Find<Text>("InputAccObject/CancelBtn/Text");
        _textOk = Find<Text>("InputAccObject/OkBtn/Text");

        _version = Find<Text>("Version");
        if (_version != null)
            _version.text = GameConst.Version;

        _startTipsObj = _transform.Find("StartTips").gameObject;
        _loginTipsText = Find<Text>("ConnectTipsText");

        _textTitle.text = LocalDataMgr.IsChinese ? "账户" : "Account";
        _textAccount.text = LocalDataMgr.IsChinese ? "账户" : "Account";
        _textPassword.text = LocalDataMgr.IsChinese ? "密码" : "Password";
        _textCancel.text = LocalDataMgr.IsChinese ? "取消" : "Cancel";
        _textOk.text = LocalDataMgr.IsChinese ? "是" : "OK";

        _textClick.text = LocalDataMgr.IsChinese ? "点击任意地方进入游戏" : "Tap anywhere to start the game";
        _cancelBtn.onClick.Add(OnCancel);
        _okBtn.onClick.Add(OnAccLogin);

        _accViewObject.SetActive(false);

        _startBtn.onClick.Add(OnStartGame);
        _accountBtn.onClick.Add(OnShowAccount);
        _facebookBtn.onClick.Add(OnFaceBookLogin);

        _defAccount.gameObject.SetActive(GameDriver.Instance.mShowDebug);

        _defAccount.text = LocalDataMgr.PlayerAccount;
        //_textClick.DOFade(0, 0.6f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
        DGHelper.DoTextFade(_textClick, 0f, 0.6f, DGEaseType.InOutQuad);

        GameObject hotObj = _startBtn.transform.Find("HotBG").gameObject;
        GameObject notHotObj = _startBtn.transform.Find("NotHotBG").gameObject;
        hotObj.SetActive(GameDriver.Instance.mBlRunHot);
        notHotObj.SetActive(!GameDriver.Instance.mBlRunHot);

        InitAlertLogic();
        InitGameMgr();
        InitDataModel();

        if(!string.IsNullOrEmpty(GameEntry.mNotice))
        {
            string[] notice = GameEntry.mNotice.Split('|');
            if(notice.Length < 2)
            {
                LogHelper.LogWarning("[GameLoginMgr.Init() => game notice format error!!!!]"); 
            }
            else
            {

            }
        }
        if (!string.IsNullOrEmpty(GameEntry.mENAnnouncement))
            GameUIMgr.Instance.OpenModule(ModuleID.Setting, false);

        _blBindFBAcc = false;
        //TranslateRequest req = new TranslateRequest((string obj) => { LogHelper.LogWarning("translate, value:" + obj); });
        //req.StartSend("hello everyone", "CN");
    }

    #region account input logic
    private void OnCancel()
    {
        _accViewObject.SetActive(false);
        _startTipsObj.SetActive(true);
        _loginTipsText.gameObject.SetActive(false);
    }

    private void OnAccLogin()
    {
        if (string.IsNullOrWhiteSpace(_accInputText.text) || string.IsNullOrWhiteSpace(_psdInputText.text))
            return;
        DoConnectServer(_accInputText.text, _psdInputText.text, GameLoginType.INPUTACCOUNT);
    }

    private void OnShowAccount()
    {
        _accInputText.text = "";
        _psdInputText.text = "";
        _accViewObject.SetActive(true);
    }
    #endregion

    private void OnStartGame()
    {
        _startBtn.enabled = false;
        if (LocalDataMgr.PlayerAccount != _defAccount.text)
        {
            LocalDataMgr.Password = "";
            LocalDataMgr.LoginChannel = GameLoginType.GUEST;
        }
        DoConnectServer(_defAccount.text, LocalDataMgr.Password, LocalDataMgr.LoginChannel);
    }

    public void FaceBookLoginBack(string sid, string token)
    {
        if(_blBindFBAcc)
        {
            LoginHelper.BindNewAccount(LocalDataMgr.PlayerAccount, LocalDataMgr.Password, sid, token, OnRegistResult, GameLoginType.FACEBOOK);
        }
        else
        {
            DoConnectServer(sid, token, GameLoginType.FACEBOOK);
        }
    }

    private void OnRegistResult(int code, S2CGuestBindNewAccountResponse value)
    {
        if (code == 0)
        {
            LoginHelper.ReLogin(value.NewAccount, value.NewPassword, GameLoginType.FACEBOOK);
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001232));
        }
        else
        {
            NetErrorHelper.DoErrorCode(code);
        }

    }

    public void FaceBookLoginCancel()
    {
        _startBtn.enabled = true;
        LocalDataMgr.PlayerAccount = SystemInfo.deviceUniqueIdentifier;
        LocalDataMgr.Password = "";
        LocalDataMgr.LoginChannel = 0;
        _defAccount.text = LocalDataMgr.PlayerAccount;
    }

    #region show alert
    private GameObject _alertObject;
    private Button _yesBtn;
    private Button _noBtn;
    private Text _alertTipsText;
    private Text _yesBtnText;
    private Text _noBtnText;

    private void InitAlertLogic()
    {
        _alertObject = _transform.Find("AlertRoot").gameObject;
        _alertTipsText = Find<Text>("AlertRoot/Text");
        _yesBtn = Find<Button>("AlertRoot/ButtonGroup/BtnYes");
        _yesBtnText = Find<Text>("AlertRoot/ButtonGroup/BtnYes/Text");
        _noBtn = Find<Button>("AlertRoot/ButtonGroup/BtnNo");
        _noBtnText = Find<Text>("AlertRoot/ButtonGroup/BtnNo/Text");
        _noBtnText.text = LocalDataMgr.IsChinese ? "退出" : "Quit";
        _yesBtn.onClick.Add(OnReconnect);
        _noBtn.onClick.Add(OnExit);
    }

    private void OnReconnect()
    {
        if (_blRelogin)
        {
            if (_states == 0)
                DoConnectServer(_acc, _psd, _channel);
            else if (_states == 1)
            {
                _startTipsObj.SetActive(true);
                _loginTipsText.gameObject.SetActive(false);
                _startBtn.enabled = true;

                if (_loginTimeKey != 0)
                    TimerHeap.DelTimer(_loginTimeKey);
                _loginTimeKey = 0;
            }
        }
        _alertObject.SetActive(false);
    }

    private void OnExit()
    {
#if UNITY_EDITOR
        return;
#elif UNITY_ANDROID
        Application.Quit();
#endif
    }

    private int _states;
    private void ShowAlert(string value, int states = 0)
    {
        _states = states;
        if(_states == 0)
        {
            _yesBtn.gameObject.SetActive(true);
            _noBtn.gameObject.SetActive(true);
            _yesBtnText.text = LocalDataMgr.IsChinese ? "是" : "Yes";
        }
        else
        {
            _noBtn.gameObject.SetActive(false);
            _yesBtnText.text = LocalDataMgr.IsChinese ? "是" : "OK";
        }
        _alertTipsText.text = value;
        _alertObject.SetActive(true);
        if (_loginTimeKey != 0)
            TimerHeap.DelTimer(_loginTimeKey);
        _loginTimeKey = 0;
    }
#endregion

#region start connect server
    private string _acc, _psd;
    private int _channel;
    private void DoConnectServer(string acc, string psd, int channel)
    {
        _acc = acc;
        _psd = psd;
        _channel = channel;
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            //string tipsLog = 
            ShowAlert(LanguageMgr.GetLanguage(4000128), 1);
            return;
        }
        StartCheckLogin();
        string logStr = string.Format("[GameLoginMgr.DoConnectServer() => connect server, acc:{0}  psd:{1}  channal:{2}]", _acc, _psd, _channel);
        LogHelper.Log(logStr);
        ShowConnectTips(LocalDataMgr.IsChinese ? "正在登录服务器" : "Logging in to the server");
        LoginHelper.DoGameLogin(_acc, _psd, _channel);
    }

    public void ShowConnectTips(string value)
    {
        if(!_loginTipsText.gameObject.activeSelf)
        {
            _loginTipsText.gameObject.SetActive(true);
            _startTipsObj.SetActive(false);
        }
        _loginTipsText.text = value;
    }

    private uint _loginTimeKey = 0;
    private void StartCheckLogin()
    {
        _blRelogin = true;
        if (_loginTimeKey != 0)
            TimerHeap.DelTimer(_loginTimeKey);
        _loginTimeKey = TimerHeap.AddTimer(20000, 0, OnLoginTimeout);
    }

    private void OnLoginTimeout()
    {
        if (_loginTimeKey != 0)
            TimerHeap.DelTimer(_loginTimeKey);
        _loginTimeKey = 0;
        _startBtn.enabled = true;
        if (Application.internetReachability == NetworkReachability.NotReachable)
            ShowAlert(LanguageMgr.GetLanguage(4000128), 1);
        else
            ShowAlert(LanguageMgr.GetLanguage(4000128));
    }

    #endregion

    private bool _blBindFBAcc = false;
    public void BindFBAccount()
    {
        _blBindFBAcc = true;
        GameNative.Instance.DoFBLogin();
    }


    private void OnFaceBookLogin()
    {
        if(LocalDataMgr.LoginChannel != GameLoginType.FACEBOOK)
        {
            ConfirmTipsMgr.Instance.ShowConfirmTips(LanguageMgr.GetLanguage(6001273), AlertBack);
        }
        else
        {
            AlertBack(true, true);
        }
    }

    private void AlertBack(bool result, bool blShowAgain)
    {
        if(result)
        {
            _startBtn.enabled = false;
            GameNative.Instance.DoFBLogin();
        }
    }

    private bool _blRelogin = true;
    public void LoginFailed(int code = -1)
    {
        _startBtn.enabled = true;
        if(code == -2)
        {
            ShowAlert(LocalDataMgr.IsChinese ? "账号密码错误" : "Wrong accout or password", 2);
            _blRelogin = false;
        }
        else
        {
            ShowAlert(LocalDataMgr.IsChinese ? "登陆失败，是否重新登陆" :"Login failed, reconnect again?");
            _blRelogin = true;
        }
        _accountBtn.enabled = false;
        _facebookBtn.enabled = false;
    }

    public void EnterGameSuccess()
    {
        if (_loginTimeKey != 0)
            TimerHeap.DelTimer(_loginTimeKey);
        _loginTimeKey = 0;
        GameDriver.Instance.mInitViewObject.SetActive(false);
        _startBtn.enabled = true;
        _accountBtn.enabled = true;
        _facebookBtn.enabled = true;
        _uiLoginObject.SetActive(false);
        _accViewObject.SetActive(false);
    }

#region login back,init game logic
    public void LoginServerSuccess()
    {
        if (_loginTimeKey != 0)
            TimerHeap.DelTimer(_loginTimeKey);
        _loginTimeKey = 0;
        StartCheckLogin();
        ShowConnectTips(LanguageMgr.GetLanguage(6001268));
        //GameNative.Instance.ReqPayPrice();
#if UNITY_EDITOR
        GameConsole.ConsoleCmdMgr.Instance.Init();
#endif
        //GameStageMgr.Instance.ChangeStage(StageType.Home);
    }

    private void InitDataModel()
    {
        BagDataModel.Instance.Init();
        HangupDataModel.Instance.Init();
        HeroDataModel.Instance.Init();
        RecruitDataModel.Instance.Init();
        FriendDataModel.Instance.Init();
        ArenaDataModel.Instance.Init();
        GuildDataModel.Instance.Init();
        GuildBossDataModel.Instance.Init();
        TalentDataModel.Instance.Init();
        WelfareDataModel.Instance.Init();
        RechargeDataModel.Instance.Init();

        RedPointDataModel.Instance.Init();
        EquipForgeDataModel.Instance.Init();
        RoleFusionDataModel.Instance.Init();
        ActivityCopyDataModel.Instance.Init();
        GoldDataModel.Instance.Init();
        ShopDataModel.Instance.Init();
        LineupDataModel.Instance.Init();
        ExploreDataModel.Instance.Init();
        CTowerDataModel.Instance.Init();
        DecomposeDataModel.Instance.Init();
        TaskDataModel.Instance.Init();
        ArtifactDataModel.Instance.Init();
        CarnivalDataModel.Instance.Init();
    }

    private void InitGameMgr()
    {
        GameComponent.Instance.Init();
        GameStageMgr.Instance.Init();
        GameEventMgr.Instance.Init();
        GameConfigMgr.Instance.Init();
        GameUIMgr.Instance.Init();
        SoundMgr.Instance.Init();
        RedPointTipsMgr.Instance.Init();
        NewBieGuide.NewBieGuideMgr.Instance.Init();
    }

    private T Find<T>(string name) where T : UnityEngine.Object
    {
        Transform tf = _transform.Find(name);
        if (tf == null)
            return default(T);
        return tf.GetComponent<T>();
    }
#endregion
}