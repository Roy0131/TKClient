using System.Collections;
using System.Xml;
using UnityEngine;

public class GameEntry : RComponent
{
    public static string mCNAnnouncement { get; private set; }
    public static string mENAnnouncement { get; private set; }
    public static string mNotice { get; private set; }


    public static GameEntry Instance;

    public HotLogic mHotLogic;
    protected override void OnAwake()
    {
        base.OnAwake();
        Instance = this;
        mHotLogic = new HotLogic();
        gameObject.AddComponent<GameNative>();
    }

    protected override void OnStart()
    {
        base.OnStart();
        StartCoroutine(LoadNoticContent());
    }

    private IEnumerator LoadNoticContent()
    {
        System.String nowTime = System.DateTime.Now.Subtract(System.DateTime.Parse("1970-1-1")).TotalMilliseconds.ToString();
        string severurl = "";
        Debuger.Log(FileConst.CachePath);
#if UNITY_ANDROID || UNITY_EDITOR
        severurl = FileConst.RES_PATH + "android/notice.xml" + "?" + nowTime;
#elif UNITY_IPHONE
        severurl = FileConst.RES_PATH + "ios/notice.xml" + "?" + nowTime;
#endif
        WWW www = new WWW(severurl);
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(www.text);

            XmlElement ele = xmlDoc.SelectSingleNode("root/notice") as XmlElement;
            mNotice = ele.GetAttribute("value");
            ele = xmlDoc.SelectSingleNode("root/announcement") as XmlElement;
            mENAnnouncement = ele.GetAttribute("value").Replace("\\n", "\n");
            mCNAnnouncement = ele.GetAttribute("cn").Replace("\\n", "\n");
        }
        RunGame();
    }

    private void RunGame()
    {
        mHotLogic.InitHotLogic();
    }

    private void OnApplicationQuit()
    {
        mHotLogic.mAppQuitAction?.Invoke();
    }

    private void OnApplicationPause(bool pause)
    {
        mHotLogic.mAppPauseAction?.Invoke(pause);
    }

    private void LateUpdate()
    {
        mHotLogic.mLateUpteAction?.Invoke();
    }

    protected override void OnUpdate()
    {
        mHotLogic.mUpdateAction?.Invoke();
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.BackQuote))
            mHotLogic.mKeyCodeAction?.Invoke();
#endif
    }

    public void PlayButtonSound()
    {
        mHotLogic.mSoundAction?.Invoke();
    }
}
