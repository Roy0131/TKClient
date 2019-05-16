using Plugin;
using UnityEngine;

public enum ServerNameEnum : ushort
{
    Singapore,
    Hongkong,
}

public class GameDriver : RComponent
{
    public bool mShowDebug = true;
    public bool mShowGuide = false;
    public bool mRunInIOS = false;

    public static GameDriver Instance;
    
    public bool UseAssetBundle;
    public ServerNameEnum m_serverName;
    public GameObject mInitViewObject { get; private set; }
    
    [HideInInspector]
    public GameObject mInitObject;
    [HideInInspector]
    public GameObject mLoginObject;
    public static bool ISIPHONEX = false;
    public REventDispatcher mPluginDispather { get; private set; }
    public bool mBlRunHot { get; set; }
    protected override void OnAwake()
    {
        mBlRunHot = true;
        Instance = this;
        base.OnAwake();
        Application.runInBackground = true;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 30;
        DontDestroyOnLoad(gameObject);
        Debuger.EnableLog = mShowDebug;
        if (mShowDebug)
            gameObject.AddComponent<GameLogTool>();
        gameObject.AddComponent<Loom>();
        gameObject.AddComponent<InputController>();
        Input.multiTouchEnabled = false;
        mPluginDispather = new REventDispatcher();
#if UNITY_IPHONE
        string modelStr = SystemInfo.deviceModel;
        ISIPHONEX = modelStr.Equals("iPhone10,3") || modelStr.Equals("iPhone10,6") || modelStr.Equals("iPhone11,8") || modelStr.Equals("iPhone11,2") || modelStr.Equals("iPhone11,6");
        CheckIphonexDPI();
#endif
        InitBugly();

        int tmpLanguage = PlayerPrefs.GetInt(FileConst.LanguageKey, (int)Application.systemLanguage);
        FileConst._curLanguage = (SystemLanguage)tmpLanguage;
    }


#if UNITY_IPHONE
    private void CheckIphonexDPI()
    {
        Canvas canvas;
        GameObject root = GameObject.Find("Canvas");
        canvas = root.transform.Find("UIModuleRoot").GetComponent<Canvas>();
        OpeniPhoneX(canvas);
        canvas = root.transform.Find("UIWindowRoot").GetComponent<Canvas>();
        OpeniPhoneX(canvas);
        canvas = root.transform.Find("UIPopupRoot").GetComponent<Canvas>();
        OpeniPhoneX(canvas);
        canvas = root.transform.Find("UITopRoot").GetComponent<Canvas>();
        OpeniPhoneX(canvas);
        canvas = root.transform.Find("UIBattleRoot").GetComponent<Canvas>();
        OpeniPhoneX(canvas);
        canvas = root.transform.Find("UIGuideRoot").GetComponent<Canvas>();
        OpeniPhoneX(canvas);
    }

    /// <summary>
    /// 自适应iPhoneX
    /// </summary>
    /// <param name="canvas">Canvas.</param>
    private void OpeniPhoneX(Canvas canvas)
    {
        if (ISIPHONEX)
        {
            RectTransform rectTransform = (canvas.transform as RectTransform);
            rectTransform.offsetMin = new Vector2(44f, 44F);
            rectTransform.offsetMax = new Vector2(-44f, 0f);
        }
    }
#endif

    protected override void OnStart()
    {
        base.OnStart();

        mInitViewObject = GameObject.Find("uiLoginView");
        mLoginObject = mInitViewObject.transform.Find("LoginRoot").gameObject;
        mInitObject = mInitViewObject.transform.Find("InitRoot").gameObject;
        if (UseAssetBundle)
        {
            mInitObject.SetActive(true);
            mLoginObject.SetActive(false);
            GameInitLoading.Instance.Init(mInitObject, RunGame);
            return;
        }
        RunGame();
    }

    private void RunGame()
    {
        gameObject.AddComponent<GameEntry>();
    }

    private void InitBugly()
    {
#if UNITY_IPHONE || UNITY_IOS
        BuglyAgent.InitWithAppId ("59d061c62a");
#if VRelease
        TalkingDataGA.OnStart("78B77DA4D9BE48599D6482350A9B8976", "AS_TapBattles_Armet");
#endif
#elif UNITY_ANDROID
        BuglyAgent.InitWithAppId("9cf1e5fb54");
#if VRelease
        TalkingDataGA.OnStart("78B77DA4D9BE48599D6482350A9B8976", "GooglePlay");
#endif
#endif
        //如果你确认已在对应的iOS工程或Android工程中初始化SDK，那么在脚本中只需启动C#异常捕获上报功能即可
        BuglyAgent.EnableExceptionHandler();
    }

    public string mFcmToken { get; private set; }
    public void OnFcmMessageToken(string value)
    {
        mFcmToken = value;
    }

    float lastEsccape = 0f;
    protected override void OnUpdate()
    {
        base.OnUpdate();
        RLoadMgr.Instance.Update();
#if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.time - lastEsccape < 2.5f)
                Application.Quit();
            else
                lastEsccape = Time.time; 
        }
#endif
    }
}