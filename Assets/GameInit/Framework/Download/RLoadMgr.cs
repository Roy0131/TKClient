#region << 版 本 注 释 >>
/*
	 * ========================================================================
     * Copyright Notice  2016-2016 moyutec.com All rights reserved .
     * ========================================================================
     * 文件名	： RLoadMgr 
     * 创建人	： roy
     * 创建时间	： 2016/12/20 15:24:44 
     * 描述  	： 资源加载管理器
*/
#endregion

using System;
using System.Collections.Generic;
using Plugin;
using UnityEngine;

public class RLoadMgr : Singleton<RLoadMgr>
{
    private List<RBaseLoader> _lstLoaders = new List<RBaseLoader>();
    private RBaseLoader _curLoader = null;

    //从cache加载assetbundle资源
    public void LoadAssetBundleFromCache(string filePath, Action<AssetBundle, string> method, Action onError, string resName = "")
    {
        RAssetBundleLoader loader = new RAssetBundleLoader(filePath, resName, method, onError);
        _lstLoaders.Add(loader);
        LoadNext();
    }

    //从streamasset加载资源，第一次启动游戏会将streamasset目录资源copy到cache
    public void LoadFileFromStreamAsset(string filePath, Action<byte[], string> method, Action onLoadErr, string resName = "")
    {
        string url = FileConst.GetStreamFilePath(filePath);
        CreateLoader(url, method, onLoadErr, resName);
    }

    public void LoadFileFromRemote(string filePath, Action<byte[], string> method, Action onLoadError, string resName = "")
    {
        string normalUrl = "";
        string timeOutUrl = "";
        FileConst.GetRemoteFilePath(filePath, ref normalUrl, ref timeOutUrl);
        CreateLoader(normalUrl, method, onLoadError, resName, true, timeOutUrl);
    }

    private void CreateLoader(string url, Action<byte[], string> method, Action onLoadError, string resName = "", bool setTimeOut = false, string timeOutPath = null)
    {
        Debuger.LogWarning("[RLoadMgr.CreateLoader() => url:" + url + "]");
        RWWWLoader loader = new RWWWLoader(url, resName, method, onLoadError, setTimeOut, timeOutPath);
        _lstLoaders.Add(loader);
        LoadNext();
    }

    public void LoadFileFromResSvr(string filePath, Action<byte[], string> method, Action onLoadError, string resName = "")
    {
        string normalUrl = FileConst.RES_PATH + filePath;
        string timeOutUrl = FileConst.RES_TIMEOUT_PATH + filePath;
        CreateLoader(normalUrl, method, onLoadError, resName, true, timeOutUrl);
    }

    private void LoadNext()
    {
        if (_curLoader != null)
            return;
        if(_lstLoaders.Count > 0)
        {
            _curLoader = _lstLoaders[0];
            _lstLoaders.RemoveAt(0);
        }
    }

    public void OnLoadFinish(RBaseLoader loader)
    {
        Debuger.Log("[RLoadMgr.OnLoadFinish() => res load finish, respath:" + _curLoader.m_resPath + "]");
        _curLoader = null;
        LoadNext();
    }

    public void OnLoadFailed(RBaseLoader loader)
    {
        _curLoader = null;
    }

    public void Update()
    {
        if (_curLoader != null)
            _curLoader.Update();
    }
}
