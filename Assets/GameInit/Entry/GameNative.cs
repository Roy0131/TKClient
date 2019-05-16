using UnityEngine;
using System.Collections.Generic;

#if UNITY_IPHONE
using System.Runtime.InteropServices;
#endif

public class GameNative : RComponent
{
    public static GameNative Instance;

#if UNITY_IPHONE
    [DllImport("__Internal")]
    private static extern void _InitIAPManager();
    [DllImport("__Internal")]
    private static extern bool _IsProductAvailable();
    [DllImport("__Internal")]
    private static extern void _RequstProductInfo(string productID);
    [DllImport("__Internal")]
    private static extern void _BuyProduct(string productID);
    [DllImport("__Internal")]
    private static extern void _InitFaceBook();
    [DllImport("__Internal")]
    private static extern void _DoFBLogin();
    [DllImport("__Internal")]
    private static extern void _DoFBLogout();
    [DllImport("__Internal")]
    private static extern void _DoFBEvent(string evtLog);
    [DllImport("__Internal")]
    private static extern void _DoFBShare();
    [DllImport("__Internal")]
    private static extern void _PasteBoard(string value);

    public void ProvideContent(string value)
    {
        GameEntry.Instance.mHotLogic.mProvideContentAction?.Invoke(value);
    }

    public void AppStoreBuyFailed()
    {
        GameEntry.Instance.mHotLogic.mAppStoreBuyFailedAction?.Invoke();
    }
#endif
    
    public void OnShopDetailResult(string detailValues)
    {
        Debug.LogWarning("[GameNative.OnShopDetailResult() => detailValues:" + detailValues + "]");
        GameEntry.Instance.mHotLogic.mShopDetailAction?.Invoke(detailValues);
    }

    #region awake
    protected override void OnAwake()
    {
        base.OnAwake();
        Instance = this;
#if UNITY_EDITOR
        return;
#elif UNITY_ANDROID
        if(_androidAcitiy == null)
        {
            AndroidJavaClass javaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            _androidAcitiy = javaClass.GetStatic<AndroidJavaObject>("currentActivity");
        }
#elif UNITY_IPHONE
        _InitIAPManager();
#endif
    }

    #endregion;

    #region pay operate
    private AndroidJavaObject _androidAcitiy = null;
    public void Pay(string bundleID)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        _androidAcitiy.Call("Pay", bundleID);
#elif UNITY_IPHONE && !UNITY_EDITOR
        _BuyProduct(bundleID);
#endif
    }

    public void ReqPayPrice(string value)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        _androidAcitiy.Call("GetPayPrice", value);
#elif UNITY_IPHONE && !UNITY_EDITOR
        _RequstProductInfo(value);
#endif
    }
    #endregion

    #region google pay back method
    public void GooglePaySuccess(string payData)
    {
        GameEntry.Instance.mHotLogic.mGooglePaySuccessAction?.Invoke(payData);
    }

    public void OnFaceBookEvent(string value)
    {
#if UNITY_EDITOR
        return;
#elif UNITY_ANDROID
        _androidAcitiy.Call("LogPurchase", value);
#elif UNITY_IPHONE
        _DoFBEvent(value);
#endif
    }

    public void GooglePayFailed(string payData)
    {
        GameEntry.Instance.mHotLogic.mGooglePayFailedAction?.Invoke();
    }
#endregion

    public void DoCopy(string value)
    {
    #if UNITY_EDITOR
        TextEditor te = new TextEditor();
        te.text = value;
        te.OnFocus();
        te.Copy();
    #elif UNITY_ANDROID
            _androidAcitiy.Call("PasteBoard", value);
    #elif UNITY_IPHONE
            _PasteBoard(value);
    #endif
    }

    public void DoFBShare()
    {
    #if UNITY_EDITOR
        return;
    #elif UNITY_ANDROID
            _androidAcitiy.Call("OnFBShare");
    #elif UNITY_IPHONE
            _DoFBShare();
    #endif
    }

    public void OnFBShareBack(string value)
    {
        int code = -1;
        int.TryParse(value, out code);
        GameEntry.Instance.mHotLogic.mFBShareBackAction?.Invoke(code);
    }
    
    public void DoFBLogin()
    {
#if UNITY_EDITOR
        return;
#elif UNITY_ANDROID
        _androidAcitiy.Call("OnFBLogin");
#elif UNITY_IPHONE
        _DoFBLogin();
#endif
    }

    public void OnFBLoginBack(string value)
    {
        GameEntry.Instance.mHotLogic.mFBLoginBackAction?.Invoke(value);
    }

    public void OnFBCancelLogin(string value)
    {
        GameEntry.Instance.mHotLogic.mFBLoginCancelAction?.Invoke();
    }

    public override void Dispose()
    {
        _androidAcitiy = null;
        Instance = null;
        base.Dispose();
    }
}