#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice  2016-2017 moyutec.com All rights reserved .
     * ========================================================================
     * 文件名	： RAssetBundle
     * 创建人	： roy
     * 创建时间	： 9/19/2017 2:10:26 AM 
     * 描述  	：   	
*/
#endregion

using System.Collections.Generic;
using UnityEngine;

public sealed class RAssetBundle
{
    private AssetBundle _assetBundle;
    private string _assetBundleName;

    public RAssetBundle(AssetBundle ab, string name)
    {
        _assetBundle = ab;
        _assetBundleName = name;
        _refCount = 1;
    }

    public AssetBundle ABAssets
    {
        get { return _assetBundle; }
    }

    public void Retain()
    {
        _refCount++;
    }

    public void Release()
    {
        _refCount--;
        if (_refCount == 0)
        {
            _assetBundle.Unload(true);
            RAssetBundleCache.FreeBundle(_assetBundleName);
        }
    }

    private int _refCount;
    public int RefCount
    {
        get { return _refCount; }
    }
}

public sealed class RAssetBundleCache
{
    private static Dictionary<string, RAssetBundle> _dictABCaches;
    public static Dictionary<string, RAssetBundle> DictABCaches
    {
        get
        {
            if (_dictABCaches == null)
                _dictABCaches = new Dictionary<string, RAssetBundle>();
            return _dictABCaches;
        }
    }

    private static Dictionary<string, string[]> _dictDependCaches;

    public static Dictionary<string, string[]> DictDependCaches
    {
        get
        {
            if (_dictDependCaches == null)
                _dictDependCaches = new Dictionary<string, string[]>();
            return _dictDependCaches;
        }
    }

    public static RAssetBundle GetBundleCache(string abName)
    {
        RAssetBundle rab;
        DictABCaches.TryGetValue(abName, out rab);
        return rab;
    }

    public static void SetBundleToCache(string abName, RAssetBundle rab)
    {
        if (DictABCaches.ContainsKey(abName))
        {
            Debuger.LogWarning("[RAssetBundleCache.SetBundleToCache() => ab:" + abName + "重复添加到缓存!!]");
            return;
        }
        DictABCaches.Add(abName, rab);
    }

    public static string[] GetDependCache(string key)
    {
        string[] depends;

        DictDependCaches.TryGetValue(key, out depends);
        return depends;
    }

    public static void SetDependCache(string key, string[] value)
    {
        if (DictDependCaches.ContainsKey(key))
        {
            Debuger.LogWarning("[RAssetBundleCache.SetDependCache() => key:" + key + "重复添加到缓存!!]");
            return;
        }
        DictDependCaches.Add(key, value);
    }

    public static void FreeBundle(string abName)
    {
        if (DictABCaches.ContainsKey(abName))
            DictABCaches.Remove(abName);
    }

    public static void UnloadAssetBundle(string abName)
    {
        UnloadABInternal(abName);
    }

    //释放AssetBundle资源, 依赖同时释放掉
    private static void UnloadDependBundles(string abName)
    {
        string[] depends = null;
        DictDependCaches.TryGetValue(abName, out depends);
        if (depends == null)
            return;
        for (int i = 0; i < depends.Length; i++)
            UnloadABInternal(depends[i]);
        DictDependCaches.Remove(abName);
    }

    private static void UnloadABInternal(string abName)
    {
        RAssetBundle rab = null;
        DictABCaches.TryGetValue(abName, out rab);
        if (rab != null)
            rab.Release();
    }
}