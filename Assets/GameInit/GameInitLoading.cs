#region << 版 本 注 释 >>
/*
	 * ========================================================================
     * Copyright Notice  2016-2017 moyutec.com All rights reserved .
     * ========================================================================
     * 文件名	： GameInitLoading 
     * 创建人	： roy
     * 创建时间	： 9/18/2017 5:39:31 PM 
     * 描述  	：   	
*/
#endregion

using System;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Plugin;

public class GameInitLoading : Singleton<GameInitLoading>
{
    enum InitAlertType
    {
        None,
        NetNotReachable,
        Download,
        DownNewVersion,
        DownloadResError,
    }

    private Action _initEndMethod;

    private Text _progressLabel;
    private Text _loadingState;
    private Image _progressSilder;

    private string _curStateTips;

    private GameObject _initObject;
    private InitLoadingAlert _alertView;

    #region alert tips
    class InitLoadingAlert
    {
        private Text _alertText;
        private Button _yesBtn;
        private Button _noBtn;
        private Text _yesBtnText;

        private InitAlertType _type;

        public InitLoadingAlert()
        {

        }

        public void SetDisplayObject(GameObject obj)
        {
            if (obj == null)
            {
                Debuger.LogWarning("display object is null!!");
                return;
            }
            _displayObject = obj;
            _transform = mDisplayObject.transform;
            if (_transform is RectTransform)
                _rectTransform = _transform as RectTransform;
            ParseComponent();
        }

        private void ParseComponent()
        {
            _alertText = Find<Text>("Text");
            _yesBtn = Find<Button>("ButtonGroup/BtnYes");
            _noBtn = Find<Button>("ButtonGroup/BtnNo");
            _yesBtnText = Find<Text>("ButtonGroup/BtnYes/Text");

            _yesBtn.onClick.AddListener(OnYesClicked);
            _noBtn.onClick.AddListener(OnNoClicked);
        }

        private void OnYesClicked()
        {
            if (_type == InitAlertType.Download)
            {
                GameInitLoading.Instance.StartDownLoad();
            }
            else if (_type == InitAlertType.NetNotReachable)
            {
                GameInitLoading.Instance.CheckNetworkStatus();
            }
            else if(_type == InitAlertType.DownNewVersion)
            {
#if UNITY_ANDROID 
                Application.OpenURL("https://play.google.com/store/apps/details?id=com.idlerpg.heroes");
               // Application.OpenURL("https://play.google.com/store/apps/details?id=com.bettergame.ib");
#elif UNITY_IPHONE
                Application.OpenURL("https://itunes.apple.com/us/app/idle-battles-heroes-vs-zombies/id1442842385?l=zh&ls=1&mt=8");
#endif
            }
            else if(_type == InitAlertType.DownloadResError)
            {
                Hide();
            }
        }

        private void OnNoClicked()
        {
#if !UNITY_EDITOR
            Application.Quit();
#endif
        }

        public void ShowAlert(InitAlertType type, float size = 0f)
        {
            _type = type;
            switch (type)
            {
                case InitAlertType.NetNotReachable:
                    _noBtn.gameObject.SetActive(false);
                    if(FileConst.IsLanguageChinese)
                    { 
                        _yesBtnText.text = "好的";
                        _alertText.text = "当前网络不可用，请检查网络";
                    }
                    else
                    {
                        _yesBtnText.text = "OK";
                        _alertText.text = "network is not available, please check the network.";
                    }
                    break;
                case InitAlertType.Download:
                    _noBtn.gameObject.SetActive(false);
                    string sizeValue = (size / 1024f / 1024f).ToString("F2") + "M";
                    if (FileConst.IsLanguageChinese)
                    {
                        _yesBtnText.text = "下载";
                        _alertText.text = "当前有" + sizeValue + "资源更新，是否现在下";
                    }
                    else
                    {
                        _yesBtnText.text = "Yes";
                        _alertText.text = "There are currently " + sizeValue + " resource updates, are you downloading now?";
                    }

                    break;
                case InitAlertType.DownNewVersion:
                    _noBtn.gameObject.SetActive(false);
                    if (FileConst.IsLanguageChinese)
                    {
                        _yesBtnText.text = "好的";
                        _alertText.text = "检测到新版本，请下载新版本";
                    }
                    else
                    {
                        _yesBtnText.text = "OK";
                        _alertText.text = "new version detected, please download new version";
                    }
                    break;
                case InitAlertType.DownloadResError:
                    _noBtn.gameObject.SetActive(false);
                    if (FileConst.IsLanguageChinese)
                    {
                        _yesBtnText.text = "好的";
                        _alertText.text = "更新资源失败，请重启游戏。";
                    }
                    else
                    {
                        _yesBtnText.text = "OK";
                        _alertText.text = "Resource update failed, please restart the game.";
                    }
                    break;
            }
            if (!mDisplayObject.activeSelf)
                mDisplayObject.SetActive(true);
        }

        public virtual void Hide()
        {
            if (mDisplayObject == null)
                return;
            if (mDisplayObject.activeSelf)
                mDisplayObject.SetActive(false);
        }

        private GameObject _displayObject;

        public GameObject mDisplayObject
        {
            get { return _displayObject; }
        }

        private RectTransform _rectTransform;
        public RectTransform mRectTransform
        {
            get { return _rectTransform; }
        }

        private Transform _transform;
        public Transform mTransform
        {
            get { return _transform; }
        }
        protected GameObject Find(string name)
        {
            Transform tf = mRectTransform.Find(name);
            if (tf != null)
                return tf.gameObject;
            return null;
        }

        protected T Find<T>(string name) where T : UnityEngine.Object
        {
            GameObject obj = Find(name);
            if (obj != null)
                return obj.GetComponent<T>();
            return null;
        }
    }
    #endregion

    private GameObject _hotBGObject;
    private GameObject _notHotBGObject;
    public void Init(GameObject initObject, Action onEndMethod)
    {
        _initEndMethod = onEndMethod;
        _initObject = initObject;
        Transform tf = _initObject.transform;
        _loadingState = tf.Find("DesLabel").GetComponent<Text>();
        _progressLabel = tf.Find("Label").GetComponent<Text>();
        _progressSilder = tf.Find("Silder").GetComponent<Image>();
        _loadingState.text = "";
        _progressLabel.text = "";

        _hotBGObject = tf.Find("HotBG").gameObject;
        _notHotBGObject = tf.Find("NotHotBG").gameObject;

        _alertView = new InitLoadingAlert();
        _alertView.SetDisplayObject(tf.Find("AlertRoot").gameObject);

        CheckNetworkStatus();
    }

    public void CheckNetworkStatus()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            _alertView.ShowAlert(InitAlertType.NetNotReachable);
            return;
        }
        else
        {
            _alertView.Hide();
        }

        GameDriver.Instance.StartCoroutine(CheckNewVersion());
    }

    private void CheckGameRes()
    {
        _curStateTips = FileConst.IsLanguageChinese ? "正在对比游戏版本" : "checking version...";
        ShowLoadingTips(_curStateTips);
        GameVerInitMgr.Instance.BeginInit(FileConst.RES_PATH, VersionCheckOver);
    }

    private IEnumerator CheckNewVersion()
    {
        String nowTime = DateTime.Now.Subtract(DateTime.Parse("1970-1-1")).TotalMilliseconds.ToString();
        string severurl = FileConst.RES_PATH + "ver.xml" + "?" + nowTime;
        WWW www = new WWW(severurl);
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(www.text);

            XmlElement ele;
            int codeVer = 0;

#if UNITY_ANDROID //|| UNITY_EDITOR
            ele = xmlDoc.SelectSingleNode("root/android") as XmlElement;
            codeVer = int.Parse(ele.GetAttribute("ver"));
            _hotBGObject.SetActive(true);
            _notHotBGObject.SetActive(false);
#elif UNITY_IPHONE
            ele = xmlDoc.SelectSingleNode("root/ios") as XmlElement;
            codeVer = int.Parse(ele.GetAttribute("ver"));
            int storeVer = int.Parse(ele.GetAttribute("storever"));
            GameDriver.Instance.mBlRunHot = storeVer != FileConst.GAME_PACKAGE_VERSION;
            _hotBGObject.SetActive(GameDriver.Instance.mBlRunHot);
            _notHotBGObject.SetActive(!GameDriver.Instance.mBlRunHot);
#endif
            if (codeVer > FileConst.GAME_PACKAGE_VERSION)
                _alertView.ShowAlert(InitAlertType.DownNewVersion);
            else
                CheckGameRes();
        }
        else
        {
            CheckGameRes();
        }
    }

    private List<RVerResInfo> _lstNeedLoadFiles;
    private int _resVer;
    //版本比对完成
    private void VersionCheckOver(List<RVerResInfo> needLoadLst, int resVer = 0)
    {
        if (needLoadLst == null || needLoadLst.Count == 0)
        {
            //没有需要从服务器下载的资源
            ResInitFinish();
        }
        else
        {
            _lstNeedLoadFiles = needLoadLst;
            _resVer = resVer;
            if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                StartDownLoad();
            }
            else
            {
                float totalSize = 0;
                foreach (RVerResInfo file in needLoadLst)
                    totalSize += file.m_fileSize;
                _alertView.ShowAlert(InitAlertType.Download, totalSize);
            }
        }
    }

    public void StartDownLoad()
    {
        _alertView.Hide();
        int totalCount = 0;
        string tips = FileConst.IsLanguageChinese ? "正在下载资源:" : "resources downloading:";
        Action<RVerResInfo> OnSingleLoaded = (info) =>
        {
            if (_lstNeedLoadFiles.Contains(info))
                _lstNeedLoadFiles.Remove(info);
            float per = (float)(totalCount - _lstNeedLoadFiles.Count) / (float)totalCount;
            int intPer = (int)(per * 100);

            ShowLoadingTips(tips + (totalCount - _lstNeedLoadFiles.Count) + "/" + totalCount, (float)intPer / 100f);

            if (_lstNeedLoadFiles.Count == 0)
            {
                GameVerInitMgr.Instance.SaveNewVersionTxt();
                ResInitFinish();
            }
        };

        Action onLoadError = () =>
        {
            _alertView.ShowAlert(InitAlertType.DownloadResError);
        };

        totalCount = _lstNeedLoadFiles.Count;
        ShowLoadingTips(tips + "0/" + totalCount, 0f);
        SetProgressValue(0f);
        foreach (RVerResInfo info in _lstNeedLoadFiles)
            info.StartDownLoad(OnSingleLoaded, onLoadError, _resVer);
    }

    private void ResInitFinish()
    {
        RAssetBundleMgr.Instance.Init(_initEndMethod, "allres/allres");
    }

    public void ShowLoadingTips(string tips, float progress = 1f)
    {
        _loadingState.text = tips;
        _progressSilder.fillAmount = progress;
        SetProgressValue(progress);
    }

    public void ShowLoadingError()
    {
        _alertView.ShowAlert(InitAlertType.DownloadResError);
    }

    public void SetProgressValue(float _value)
    {
        _progressLabel.text = (int)(_value * 100f) + "%";
    }

    public void Dispose()
    {
        _initEndMethod = null;
    }
}