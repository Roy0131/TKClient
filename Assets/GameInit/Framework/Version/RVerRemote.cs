#region << 版 本 注 释 >>
/*
	 * ========================================================================
     * Copyright Notice  2016-2016 moyutec.com All rights reserved .
     * ========================================================================
     * 文件名	： RVerRemote 
     * 创建人	： roy
     * 创建时间	： 2016/12/20 15:24:44 
     * 描述  	： 远程服务器资源版本数据对象
*/
#endregion

using System;
using System.Collections.Generic;
using System.Xml;

public class RVerRemote : IDisposable
{
    /// <summary>
    /// 游戏资源版本
    /// </summary>
    public int m_resVer { get; private set; }
    public Dictionary<string, RVerResInfo> m_dictResInfo { get; protected set; }
    public string m_verTxt { get; protected set; }
    protected Action _finishMethod;
    public RVerRemote()
    {
        m_dictResInfo = new Dictionary<string, RVerResInfo>();
    }

    public virtual void OnInit(Action finishMethod)
    {
        Action<byte[], string> OnLoaded = (bt, name) =>
        {
            if (bt == null || bt.Length == 0)
            {
                Debuger.LogError("[MRemoteVer.OnInit() => 资源服务器的游戏资源版本文件加载出错!!!]");
                return;
            }
            string value = System.Text.UTF8Encoding.UTF8.GetString(bt);
            string txt = FileTool.TrimUnicode(value);
            ParseVersionTxt(txt);
            if (_finishMethod != null)
                _finishMethod.Invoke();
        };

        Action onLoadError = () =>
        {
            GameInitLoading.Instance.ShowLoadingError();
        };

        string fileName = FileConst.GAME_VERSION_FILE + "?" + UnityEngine.Random.Range(1f, 10000000f);
        _finishMethod = finishMethod;
        RLoadMgr.Instance.LoadFileFromRemote(fileName, OnLoaded, onLoadError);
    }

    protected virtual void ParseVersionTxt(string value)
    {
        m_verTxt = value;
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(m_verTxt);
        XmlElement root = xmlDoc.SelectSingleNode("verRoot") as XmlElement;
        m_resVer = int.Parse(root.GetAttribute("ver"));
        XmlNodeList nodelist = root.ChildNodes;
        XmlElement element;
        foreach (XmlNode node in nodelist)
        {
            if (node is XmlComment)
                continue;
            element = node as XmlElement;
            RVerResInfo info = new RVerResInfo(element.GetAttribute("fileName"), element.GetAttribute("resMD5"), int.Parse(element.GetAttribute("resSize")));
            if (m_dictResInfo.ContainsKey(info.m_filePath))
            {
                Debuger.LogWarning("[RVerRemote.FirstInitGameAssets() => 文件:" + info.m_filePath + "版本信息重复]");
                continue;
            }
            m_dictResInfo.Add(info.m_filePath, info);
        }
    }

    public virtual void Dispose()
    {
        if (m_dictResInfo != null)
        {
            m_dictResInfo.Clear();
            m_dictResInfo = null;
        }
        _finishMethod = null;
    }
}