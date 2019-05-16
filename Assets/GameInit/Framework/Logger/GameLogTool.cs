#region << 版 本 注 释 >>
/*
	 * ========================================================================
     * Copyright Notice  2016-2016 moyutec.com All rights reserved .
     * ========================================================================
     * 文件名	： GameLogTool 
     * 创建人	： roy
     * 创建时间	： 2016/12/20 16:25:56 
     * 描述  	： 日志和FPS显示
*/
#endregion

using System;
using System.Collections.Generic;
using UnityEngine;

public class GameLogTool : RComponent
{
    public bool m_enableLog { get; set; }
    private Dictionary<string, LogInfoData> _dictLogDatas;
    private bool _blIsInited = false;
    private bool _isShowContent = false;
    private GUIStyle guiStyle;
    private GUIStyle guiStyleText;
    protected override void OnAwake()
    {
        base.OnAwake();
        Debuger.EnableLog = false;
    }

    protected override void OnStart()
    {
        if (_blIsInited)
            return;
        _blIsInited = true;
        base.OnStart();
        //m_enableLog = true;
        Debuger.EnableLog = true;
        _dictLogDatas = new Dictionary<string, LogInfoData>();
        Application.logMessageReceived += OnLogAdded;
        _timeleft = _updateInterval;
    }

    private void OnLogAdded(string logText, string stackTrace, LogType type)
    {
        if (_dictLogDatas.Count > 300)
            _dictLogDatas.Clear();
        string content = "{0}:{1}";
        if (type == LogType.Error)
            content = string.Format(content, type, logText + "\n" + stackTrace);
        else
            content = string.Format(content, type, logText);

        if (_dictLogDatas.ContainsKey(content))
            _dictLogDatas[content].m_logCount += 1;
        else
            _dictLogDatas.Add(content, new LogInfoData(content, type));
    }

    void OnGUI()
    {
        if (guiStyle == null)
        {
            guiStyle = GUI.skin.button;
            guiStyleText = GUI.skin.label;
            guiStyle.fontSize = 20;
            guiStyleText.fontSize = 20;
        }
        OnStart();
        if (_isShowContent)
            ShowLogContent();
        if (GUI.Button(new Rect(5, 150, 150, 60), _fps, guiStyle))
            _isShowContent = !_isShowContent;

    }
    private Vector2 _scrollPos;
    void ShowLogContent()
    {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width * 0.8f, Screen.height * 0.8f));
        _scrollPos = GUILayout.BeginScrollView(_scrollPos, GUILayout.Width(Screen.width * 0.8f), GUILayout.Height(Screen.height * 0.8f));
        Dictionary<string, LogInfoData>.Enumerator rator = _dictLogDatas.GetEnumerator();
        
        while(rator.MoveNext())
        {
            switch(rator.Current.Value.m_logType)
            {
                case LogType.Log:
                    GUI.contentColor = Color.black;
                    break;
                case LogType.Warning:
                    GUI.contentColor = Color.white;
                    break;
                case LogType.Error:
                case LogType.Exception:
                    GUI.contentColor = Color.red;
                    GUILayout.Label(rator.Current.Key + "------" + rator.Current.Value.m_logText, guiStyleText);
                    break;
                default:
                    GUI.contentColor = Color.grey;
                    break;
            }

            GUILayout.Label(rator.Current.Key + " --- Count:" + rator.Current.Value.m_logCount, guiStyleText);
        }

        if (GUILayout.Button("clear"))
            _dictLogDatas.Clear();
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    #region FPS
    private float _updateInterval = 0.5f;
    private float _accum = 0.0f;
    private float _frames = 0f;
    private float _timeleft;
    private string _fps = "";
    protected override void OnUpdate()
    {
        _timeleft -= Time.deltaTime;
        _accum += Time.timeScale / Time.deltaTime;
        ++_frames;
        if (_timeleft <= 0.0f)
        {
            _fps = "FPS:" + (_accum / _frames).ToString("F2");
            _timeleft = _updateInterval;
            _accum = 0.0f;
            _frames = 0f;
        }
    }
    #endregion
}

public class LogInfoData
{
    public LogType m_logType { private set; get; }
    public string m_logText { private set; get; }
    public int m_logCount { set; get; }

    public LogInfoData(string value, LogType type)
    {
        m_logType = type;
        m_logText = value;
    }
}