
using System;
using UnityEngine;
using Plugin;

public enum RDirLoadType
{
    Type_Cache,
    Type_StreamAsset,
    Type_Remote,
}

public class RVerResInfo
{
    public string m_filePath { get; private set; }
    public string m_fileName { get; private set; }
    public string m_fileHash { get; private set; }
    public int m_fileSize { get; private set; }
    public RDirLoadType m_type { get; set; }
    public RVerResInfo(string filePath, string hash, int size)
    {
        m_filePath = filePath;
        m_fileHash = hash;
        m_fileSize = size;
        m_type = RDirLoadType.Type_Cache;

        string[] s = m_filePath.Split('/');
        m_fileName = s[s.Length - 1];
    }

    public void StartDownLoad(Action<RVerResInfo> finishMethod, Action onLoadError, int version, bool isStreamAsset = false)
    {
        string fileName = m_filePath + "_" + version.ToString();
        //Debuger.LogWarning("fileName:" + fileName);
        Action<byte[], string> OnLoadFinish = (bt, name) =>
        {
            if (bt != null)
            {
                try
                {
                    FileTool.WriteFileToCache(m_filePath, bt);
                }
                catch (Exception)
                {
                    Debuger.LogError("[RVerResInfo.StartDownLoad() => 载入资源出错,资源名字:" + fileName + "]");
                    onLoadError();
                    return;
                }
            }
            else
            {
                Debuger.LogWarning("[文件加载失败, url:" + m_filePath + "]");
                onLoadError();
                return;
            }
            if (finishMethod != null)
                finishMethod.Invoke(this);
        };

        if (isStreamAsset)
        {
            RLoadMgr.Instance.LoadFileFromStreamAsset(fileName, OnLoadFinish, onLoadError);
        }
        else
        {
            RLoadMgr.Instance.LoadFileFromRemote(fileName, OnLoadFinish, onLoadError);
        }
    }
}
