#region << 版 本 注 释 >>
/*
	 * ========================================================================
     * Copyright Notice  2016-2016 moyutec.com All rights reserved .
     * ========================================================================
     * 文件名	： RVerLocal 
     * 创建人	： roy
     * 创建时间	： 2016/12/20 15:40:17 
     * 描述  	： 本地版本数据结构
*/
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class RVerLocal : RVerRemote
{
    public RVerLocal()
    {
        m_dictResInfo = new Dictionary<string, RVerResInfo>();
    }

    public List<RVerResInfo> GetAllLocalVerInfo()
    {
        List<RVerResInfo> result = new List<RVerResInfo>();
        foreach (RVerResInfo info in m_dictResInfo.Values)
        {
            if (info.m_fileName == "Thumbs.db" || info.m_fileName == "thumbs.db")
                continue;
            result.Add(info);
        }
        return result;
    }

    public void ReadCachedVer(Action finishMethod)
    {
        if (!CheckCachePath())
            return;
        string filePath = FileConst.GAME_VERSION_FILE;
        byte[] bs = FileTool.ReadCacheFile(filePath);
        if (bs != null)
        {
            string txt = Encoding.UTF8.GetString(bs);
            ParseVersionTxt(txt);
        }
        if (finishMethod != null)
            finishMethod.Invoke();
    }

    private bool CheckCachePath()
    {
        string dir = FileConst.CachePath;
        if (!Directory.Exists(dir))
        {
            try
            {
                Directory.CreateDirectory(dir);
            }
            catch (Exception)
            {
                Debuger.LogError("[RVerLocal.FirstInitGameAssets() => 缓存写入错误]");
                return false;
            }
        }
        return true;
    }

    public void FirstInitGameAssets(Action finishMethod)
    {
        Action<byte[], string> onAllVerLoaded = (bt, name) =>
        {
            if (!CheckCachePath())
                return;
            if (bt == null || bt.Length == 0)
            {
                if (finishMethod != null)
                    finishMethod.Invoke();
                return;
            }
            string value = UTF8Encoding.UTF8.GetString(bt);
            string txt = FileTool.TrimUnicode(value);
            ParseVersionTxt(txt);

            FileTool.WriteFileToCache(FileConst.GAME_VERSION_FILE, txt);

            if (finishMethod != null)
                finishMethod.Invoke();
        };

        Action onLoadError = () =>
        {
            if (finishMethod != null)
                finishMethod.Invoke();
        };

        RLoadMgr.Instance.LoadFileFromStreamAsset(FileConst.GAME_VERSION_FILE, onAllVerLoaded, onLoadError);
    }
}
