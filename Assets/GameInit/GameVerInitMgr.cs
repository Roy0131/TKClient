#region << 版 本 注 释 >>
/*
	 * ========================================================================
     * Copyright Notice  2016-2017 moyutec.com All rights reserved .
     * ========================================================================
     * 文件名	： GameVerInitMgr 
     * 创建人	： roy
     * 创建时间	： 9/18/2017 7:49:28 PM 
     * 描述  	：   	
*/
#endregion

using System.Collections.Generic;
using Plugin;
using System;

public class GameVerInitMgr : Singleton<GameVerInitMgr>
{
    private string _remoteUrl;
    private RVerLocal _localVer;
    private RVerRemote _remoteVer;

    private Action<List<RVerResInfo>, int> _initFinishMethod;

    public void BeginInit(string url, Action<List<RVerResInfo>, int> method)
    {
        tips = FileConst.IsLanguageChinese ? "正在解压资源，不消耗流量" : "decompressing resources without consuming traffic";

        _remoteUrl = url;
        _localVer = new RVerLocal();
        _initFinishMethod = method;

        if (FileConst.IsFirstGame)
            _localVer.FirstInitGameAssets(FirstCopyGameRes);
        else
            _localVer.ReadCachedVer(LoadRemoteVersion);
    }

    /// <summary>
    /// 下载远程服务器版本文件
    /// </summary>
    private void LoadRemoteVersion()
    {
        if (!GameDriver.Instance.mBlRunHot)
        {
            if (_initFinishMethod != null)
                _initFinishMethod.Invoke(null, 0);
        }
        else
        {
            _remoteVer = new RVerRemote();
            _remoteVer.OnInit(OnRemoteVerFinished);
        }
    }

    private void OnRemoteVerFinished()
    {
        if (_localVer.m_resVer != _remoteVer.m_resVer)
        {
            Dictionary<string, RVerResInfo> dictRemoteResInfo = _remoteVer.m_dictResInfo;
            Dictionary<string, RVerResInfo> dictLocalResInfo = _localVer.m_dictResInfo;
            List<RVerResInfo> needHttpDownInfo = new List<RVerResInfo>();
            RVerResInfo info;
            RVerResInfo addInfo;
            Dictionary<string, bool> tmp = new Dictionary<string, bool>();
            foreach (var f in dictRemoteResInfo)
            {
                if (dictLocalResInfo.ContainsKey(f.Key))
                {
                    info = dictLocalResInfo[f.Key];
                    if (info.m_fileHash != f.Value.m_fileHash || info.m_fileSize != f.Value.m_fileSize)
                    {
                        info.m_type = RDirLoadType.Type_Remote;
                        if (tmp.ContainsKey(info.m_filePath))
                            continue;
                        tmp.Add(info.m_filePath, true);
                        needHttpDownInfo.Add(info);
                        
                    }
                }
                else
                {
                    addInfo = new RVerResInfo(f.Value.m_filePath, f.Value.m_fileHash, f.Value.m_fileSize);
                    addInfo.m_type = RDirLoadType.Type_Remote;
                    if (tmp.ContainsKey(addInfo.m_filePath))
                        continue;
                    tmp.Add(addInfo.m_filePath, true);
                    needHttpDownInfo.Add(addInfo);
                }
            }
            if (_initFinishMethod != null)
                _initFinishMethod.Invoke(needHttpDownInfo, _remoteVer.m_resVer);
        }
        else
        {
            if (_initFinishMethod != null)
                _initFinishMethod.Invoke(null, 0);
        }
    }

    private int _downLoadCount;
    private List<RVerResInfo> _downList;
    /// <summary>
    /// 第一次进入游戏，需要将streamasset目录下的游戏资源复制到persistentDataPath目录下
    /// </summary>
    public void FirstCopyGameRes()
    {
        _downList = _localVer.GetAllLocalVerInfo();
        if (_downList.Count == 0)
        {
            if (_initFinishMethod != null)
                _initFinishMethod.Invoke(null, 0);
            return;
        }
        _downLoadCount = _downList.Count;
        foreach (RVerResInfo info in _downList)
        {
            info.StartDownLoad(CheckAllLoadFinish, OnLoadError, _localVer.m_resVer, true);
        }
        
        GameInitLoading.Instance.ShowLoadingTips(tips, (float)(_downLoadCount - _downList.Count) / (float)_downLoadCount);
    }
    private string tips = "";
    private void CheckAllLoadFinish(RVerResInfo info)
    {
        lock (_downList)
        {
            _downList.Remove(info);

            GameInitLoading.Instance.ShowLoadingTips(tips, (float)(_downLoadCount - _downList.Count) / (float)_downLoadCount);
            if (_downList.Count == 0)
                LoadRemoteVersion();
        }
    }

    private void OnLoadError()
    {
        GameInitLoading.Instance.ShowLoadingError();
    }

    public void SaveNewVersionTxt()
    {
        FileTool.WriteFileToCache(FileConst.GAME_VERSION_FILE, _remoteVer.m_verTxt);
        Debuger.LogWarning("[MVerInitMgr.SaveNewVersionTxt() => SaveNewVersionTxt]");
    }
}