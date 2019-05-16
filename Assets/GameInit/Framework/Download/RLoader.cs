#region << 版 本 注 释 >>
/*
	 * ========================================================================
     * Copyright Notice  2016-2016 moyutec.com All rights reserved .
     * ========================================================================
     * 文件名	： RBaseLoader 
     * 创建人	： roy
     * 创建时间	： 2016/12/20 15:20:44 
     * 描述  	： 资源加载器，
     *             RBaseLoader：基类
     *             RWWWLoader： WWW加载器
     *             RAssetBundleLoader：ab加载器, 游戏ab资源都是从cache目录获取, remote资源用RWWWLoader加载
*/
#endregion

using UnityEngine;
using System;
using UnityEngine.Networking;

public class RBaseLoader
{
    //资源目录加载url
    public string m_resPath { get; protected set; }
    public string m_resName { get; protected set; }

    protected Action _onLoadError;


    public RBaseLoader(string path, string name, Action onError)
    {
        m_resPath = path;
        m_resName = name;
        _onLoadError = onError;
    }

    public virtual void Update()
    {
        
    }

    protected virtual void LoadFinsh()
    {
        RLoadMgr.Instance.OnLoadFinish(this);
    }

    protected virtual void LoadFailed()
    {
        if (_onLoadError != null)
            _onLoadError.Invoke();
        RLoadMgr.Instance.OnLoadFailed(this);
        _onLoadError = null;
    }
}

public class RWWWLoader : RBaseLoader
{
    //超时后，资源加载目录
    private string _resTimeOutPath;
    //超时时间
    protected float _timeOut;
    //是否开启
    private bool _blIsTimeOut = false;

    private Action<byte[], string> _method;
    //private WWW _www = null;
    private int _step;
    //private UnityWebRequest _webRequest = null;
    private UnityWebRequestAsyncOperation _asyncOperat = null;
    public RWWWLoader(string path, string name, Action<byte[], string> method, Action onError, bool setTimeout = false, string timeOutPath = null, float timeOut = 60f)
        : base(path, name, onError)
    {
        _method = method;
        _blIsTimeOut = setTimeout;
        _resTimeOutPath = timeOutPath;
        _timeOut = timeOut;
        _step = 1;
    }

    public override void Update()
    {
        base.Update();
        if (_blIsTimeOut)
        {
            _timeOut -= Time.deltaTime;
            if(_timeOut <= 0.01f)
            {
                if(_step == 1)
                {
                    if(_asyncOperat != null && _asyncOperat.webRequest != null)
                        _asyncOperat.webRequest.Abort();
                    _asyncOperat = null;
                    _asyncOperat = UnityWebRequest.Get(m_resPath).SendWebRequest();
                    _step = 2;
                    _timeOut = 60f;
                }
                else
                {
                    Debuger.LogWarning("[RWWWLoader.Update() => res load failed, respath:" + m_resPath + "]");
                    LoadFailed();
                    return;
                }
            }
        }
        if(_asyncOperat == null)
            _asyncOperat = UnityWebRequest.Get(m_resPath).SendWebRequest();

        if (_asyncOperat.isDone)
        {
            if (_asyncOperat.webRequest.isNetworkError || _asyncOperat.webRequest.isHttpError)
            {
                LoadFailed();
                return;
            }
            else
            {
                LoadFinsh();
            }
        }
    }

    protected override void LoadFailed()
    {
        if (_asyncOperat != null && _asyncOperat.webRequest != null)
            _asyncOperat.webRequest.Abort();
        _asyncOperat = null;
        base.LoadFailed();
    }

    protected override void LoadFinsh()
    {
        try
        {
            if (_method != null)
                _method.Invoke(_asyncOperat.webRequest.downloadHandler.data, m_resName);
            _method = null;
            if (_asyncOperat != null)
            {
                _asyncOperat.webRequest.Abort();
                _asyncOperat = null;
            }
            base.LoadFinsh();
        }
        catch(Exception ex)
        {
            Debuger.LogError("[RWWWLoader.LoadFinsh() => load res failed, ex:" + ex + "]");
            LoadFailed();
        }
    }
}

public class RAssetBundleLoader : RBaseLoader
{
    private Action<AssetBundle, string> _method;
    private AssetBundleCreateRequest _abRequest;
    public RAssetBundleLoader(string path, string name, Action<AssetBundle, string> method, Action onError)
        : base(path, name, onError)
    {
        _method = method;
    }

    public override void Update()
    {
        base.Update();
        if (_abRequest == null)
        {
            Debuger.Log("[RAssetBundleLoader.Update() => m_resPath:" + m_resPath + "]");
            byte[] bs = FileTool.ReadCacheFile(m_resPath);
            if(bs == null)
            {
                LoadFailed();
                return;
            }
            _abRequest = AssetBundle.LoadFromMemoryAsync(bs);
            return;
        }
        if (_abRequest.isDone)
            LoadFinsh();
    }

    protected override void LoadFinsh()
    {
        if (_method != null)
            _method.Invoke(_abRequest.assetBundle, m_resName);
        base.LoadFinsh();
        _method = null;
        _abRequest = null;
    }
}