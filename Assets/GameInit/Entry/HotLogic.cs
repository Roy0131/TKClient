using System;
using UnityEngine;
using System.Reflection;

public class HotLogic : System.Object
{
#if ILRuntime && !UNITY_EDITOR
    private ILRuntime.Runtime.Enviorment.AppDomain _appDomain;
#endif
    private int len = 15;
    private Assembly _assembly;

    public Action mUpdateAction;
    public Action mLateUpteAction;
    public Action mAppQuitAction;
    public Action<bool> mAppPauseAction;
    public Action mKeyCodeAction;
    public Action mSoundAction;

    public Action<string> mProvideContentAction;
    public Action mAppStoreBuyFailedAction;
    public Action<string> mGooglePaySuccessAction;
    public Action mGooglePayFailedAction;
    public Action<string> mFBLoginBackAction;
    public Action mFBLoginCancelAction;
    public Action<string> mShopDetailAction;
    public Action<int> mFBShareBackAction;
    public void InitHotLogic()
    {
        byte[] dllBytes = null;
        Action<RAssetBundle> OnHotBundleLoaded = (rab) =>
        {
            TextAsset ta = rab.ABAssets.LoadAsset<TextAsset>("ttbres");
            if (ta == null)
                return;
            dllBytes = ta.bytes;
            OnHotResLoaded(dllBytes, true);
        };
#if UNITY_EDITOR
        dllBytes = FileTool.ReadHotDllFile();
        OnHotResLoaded(dllBytes);
#else
        RAssetBundleMgr.Instance.LoadAssetBundleAsyn(FileConst.HOT_RES, OnHotBundleLoaded);
#endif
    }

    private void OnHotResLoaded(byte[] dllBytes, bool blDecompress = false)
    {
        byte[] result;
        if (blDecompress)
        {
            SnappyDecompressor sd = new SnappyDecompressor();
            byte[] t = sd.Decompress(dllBytes, 0, dllBytes.Length);
            result = new byte[t.Length - len];
            Array.Copy(t, len, result, 0, t.Length - len);
        }
        else
        {
            result = dllBytes;
        }

#if ILRuntime && !UNITY_EDITOR
        _appDomain = new ILRuntime.Runtime.Enviorment.AppDomain();
        using (System.IO.MemoryStream msDll = new System.IO.MemoryStream(result))
            _appDomain.LoadAssembly(msDll, null, new Mono.Cecil.Pdb.PdbReaderProvider());
        ILHelper.InitILRunTime(_appDomain);

        //_appDomain.DebugService.StartDebugService(56000);
        _appDomain.Invoke("IHLogic.LogicMain", "RunGame", null, null);

#else
        _assembly = Assembly.Load(result);

        Type mainLogic = _assembly.GetType("IHLogic.LogicMain");
        object ins = Activator.CreateInstance(mainLogic);
        MethodInfo mi = mainLogic.GetMethod("RunGame");
        mi.Invoke(ins, null);
#endif
    }
}