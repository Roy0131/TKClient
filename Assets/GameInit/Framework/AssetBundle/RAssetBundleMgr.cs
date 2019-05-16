#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice  2016-2017 moyutec.com All rights reserved .
     * ========================================================================
     * 文件名	： RAssetBundleMgr
     * 创建人	： roy
     * 创建时间	： 9/19/2017 1:42:38 AM 
     * 描述  	：   	
*/
#endregion

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Plugin;
using System;

public class RAssetBundleMgr : Singleton<RAssetBundleMgr>
{
    private AssetBundleManifest _abManifest;

    private Action _initMethod;
    public void Init(Action InitMethod, string manifrestName)
    {
        _initMethod = InitMethod;

        Action<AssetBundle, string> OnManifrestLoaded = (AssetBundle ab, string name) =>
        {
            if (ab == null)
            {
                Debuger.LogWarning("[RAssetBundleMgr.Init() => manifest不存在!!]");
                return;
            }
            _abManifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            ab.Unload(false);
            if (_abManifest == null)
            {
                Debuger.LogWarning("[RAssetBundleMgr.Init() => manifest加载失败]");
                return;
            }
            else
                Debuger.Log("[RAssetBundleMgr.Init() => manifest加载成功]");
            if (_initMethod != null)
                _initMethod.Invoke();
        };

        Action OnLoadError = () =>
        {
            Debuger.LogError("[RAssetBundleMgr.Init() => manifest加载失败]");
        };


        RLoadMgr.Instance.LoadAssetBundleFromCache(manifrestName, OnManifrestLoaded, OnLoadError);
    }

    private string[] GetDepends(string abName)
    {
        string[] depends = _abManifest.GetAllDependencies(abName);
        if (depends.Length > 0)
            RAssetBundleCache.SetDependCache(abName, depends);
        return depends;
    }

    #region 同步加载AssetBundle
    public RAssetBundle LoadAssetBundle(string abName)
    {
        RAssetBundle rab = RAssetBundleCache.GetBundleCache(abName);
        if (rab != null)
            return rab;
        return null;
    }

    #endregion 

    #region 异步加载AssetBundle
    private Dictionary<string, List<Action<RAssetBundle>>> _dictLoadings = new Dictionary<string, List<Action<RAssetBundle>>>();
    public void LoadAssetBundleAsyn(string abName, Action<RAssetBundle> callBack)
    {
        RAssetBundle rab = RAssetBundleCache.GetBundleCache(abName);
        if (rab != null)
        {
            rab.Retain();
            callBack.Invoke(rab);
            return;
        }

        Action<RAssetBundle> OnLoadEnd = (ab) =>
        {
            List<Action<RAssetBundle>> lst = _dictLoadings[abName];
            for (int i = 0; i < lst.Count; i++)
            {
                ab.Retain();
                lst[i].Invoke(ab);
            }
            ab.Release();
            //Debuger.LogWarning("abName:" + abName + ", count:" + ab.RefCount);
            _dictLoadings.Remove(abName);
        };

        if (_dictLoadings.ContainsKey(abName))
        {
            _dictLoadings[abName].Add(callBack);
        }
        else
        {
            List<Action<RAssetBundle>> lst = new List<Action<RAssetBundle>>();
            lst.Add(callBack);
            _dictLoadings.Add(abName, lst);
            Loom.Current.StartCoroutine(DoLoadAssetBundleAsyn(abName, OnLoadEnd));
        }
    }

    private IEnumerator DoLoadAssetBundleAsyn(string abName, Action<RAssetBundle> callBack)
    {
        string[] depends = GetDepends(abName);
        if (depends.Length > 0)
        {
            int idx = 0;
            Action<RAssetBundle> OnDependLoaded = (bundle) =>
            {
                idx++;
                //Debuger.LogWarning("idx:" + idx);
            };
            for (int i = 0; i < depends.Length; i++)
            {
                //Debuger.LogWarning("客户端加载Depend AB:" + depends[i]);
                DoLoadAssetBundleFromeCache(depends[i], OnDependLoaded);
            }
            while (idx < depends.Length)
                yield return null;
        }
        yield return null;
        //Debuger.LogWarning("run ab...");
        DoLoadAssetBundleFromeCache(abName, callBack);
    }

    private void DoLoadAssetBundleFromeCache(string abName, Action<RAssetBundle> OnLoaded)
    {
        RAssetBundle rab = RAssetBundleCache.GetBundleCache(abName);
        if (rab != null)
        {
            rab.Retain();
            OnLoaded.Invoke(rab);
            return;
        }

        Action<AssetBundle, string> OnAssetBundleLoaded = (bundle, name) =>
        {
            rab = new RAssetBundle(bundle, abName);
            RAssetBundleCache.SetBundleToCache(abName, rab);
            OnLoaded.Invoke(rab);
        };

        Action onLoadError = () =>
        {
            OnLoaded.Invoke(null);
        };

        string filePath = "allres/" + abName;
        RLoadMgr.Instance.LoadAssetBundleFromCache(filePath, OnAssetBundleLoaded, onLoadError, abName);
    }
    #endregion
}
