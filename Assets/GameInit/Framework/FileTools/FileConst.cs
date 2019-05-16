#region << 版 本 注 释 >>
/*
	 * ========================================================================
     * Copyright Notice  2016-2016 moyutec.com All rights reserved .
     * ========================================================================
     * 文件名	： FileConst 
     * 创建人	： roy
     * 创建时间	： 2016/12/20 15:24:44 
     * 描述  	： 文件操作常量
*/
#endregion

using UnityEngine;

public class PlatformEnum
{
    public static string PLATFORM_WIN = "win";
    public static string PLATFORM_ANDROID = "android";
    public static string PLATFORM_IPHONE = "ios";
    public static string PLATFORM_NONE = "none";
}

public class FileConst
{
    //安卓平台下，主代码DLL文件目录
    public static string HOT_RES = "hotres/ttbres.bytes";
    //资源服务器地址
    public static string RES_PATH = "http://ihdownload.moyuplay.com/app/gameres5/ttbgame/";//"http://ihdownload.moyuplay.com/app/gameres/";//"http://192.168.0.16:8000/gameres/"//;
    //超时地址目录
    public static string RES_TIMEOUT_PATH = "http://ihdownload.moyuplay.com/app/gameres5/ttbgame/";//"http://ihdownload.moyuplay.com/app/gameres/";//"http://192.168.0.16:8000/gameres/";
    //整包更新版本号,服务器版本大于此版本时，强制更新提示
    public static int GAME_PACKAGE_VERSION = 1;
    public static int GAME_RES_VERSION = 1;
    //版本资源配置文件
    public static string GAME_VERSION_FILE = "ver.xml";

    //沙箱资源根目录
    public static string CachePath
    {
        get { return Application.persistentDataPath + "/cache/res/" + RunPlatform + "/"; }
    }

    //streamasset目录资源
    public static string StreamPath
    {
        get
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return Application.streamingAssetsPath + "/res/" + RunPlatform + "/";
#else
            return "file://" + Application.streamingAssetsPath + "/res/" + RunPlatform + "/";
#endif
        }
    }

    //streamasset目录资源
    public static string GetStreamFilePath(string filePath)
    {
        return StreamPath + filePath;
    }

    //沙箱目录资源
    public static string GetCacheFilePath(string filePath)
    {
        return CachePath + filePath;
    }
    
    //资源服务器资源地址
    public static void GetRemoteFilePath(string filePath, ref string normalPath, ref string timeOutPath)
    {
        filePath = RunPlatform + "/" + filePath;
        normalPath = RES_PATH + filePath;
        timeOutPath = RES_TIMEOUT_PATH + filePath;
        //Debug.Log("[FileConst.GetRemoteFilePath() => normalPath:" + normalPath + ", timeoutPath:" + timeOutPath + "]");
    }

    public static string RunPlatform
    {
        get
        {
            //if (GameDriver.Instance.mRunInIOS)
                //return PlatformEnum.PLATFORM_IPHONE;
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
            return PlatformEnum.PLATFORM_WIN;
#elif UNITY_ANDROID
            return PlatformEnum.PLATFORM_ANDROID;
#elif UNITY_IPHONE
            return PlatformEnum.PLATFORM_IPHONE;
#else
            return PlatformEnum.PLATFORM_NONE;
#endif
        }
    }

    public static bool IsLanguageChinese
    {
        get
        {
            return _curLanguage == SystemLanguage.ChineseSimplified || _curLanguage == SystemLanguage.Chinese;
        }
    }

    /// <summary>
    /// 当前版本游戏是否是第一次进入游戏
    /// </summary>
    public static bool IsFirstGame
    {
        get
        {
            string key = "ttp" + GAME_RES_VERSION.ToString();
            if (PlayerPrefs.HasKey(key))
            {
                return false;
            }
            else
            {
                PlayerPrefs.SetString(key, key);
                return true;
            }
        }
    }

    public const string LanguageKey = "languageKey";
    #region language

    public static SystemLanguage _curLanguage;
    public static SystemLanguage CurLanguage
    {
        set
        {
            if (_curLanguage == value)
                return;
            _curLanguage = value;
            int language = (int)_curLanguage;
            PlayerPrefs.SetInt(LanguageKey, language);
            PlayerPrefs.Save();
        }
    }
    #endregion
}