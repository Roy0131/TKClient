#region << 版 本 注 释 >>
/*
     * ========================================================================
     * Copyright Notice  2016-2017 moyutec.com All rights reserved .
     * ========================================================================
     * 文件名	： GameResVerWindow
     * 创建人	： roy
     * 创建时间	： 9/17/2017 10:17:47 PM 
     * 描述  	：   	
*/
#endregion

using UnityEditor;
using UnityEngine;

public class GameResVerWindow : EditorWindow
{
    private GUIContent _title = new GUIContent("资源版本工具");

    [MenuItem("Tool/资源版本工具/打开面板", false, 10)]
    static void ShowGameResWindow()
    {
        EditorWindow.GetWindow<GameResVerWindow>().Show();
    }

    #region location version value
    private static int _androidVer;
    public static int AnroidResVer
    {
        get { return _androidVer; }
        set
        {
            _androidVer = value;
            PlayerPrefs.SetInt("IBAndroidVer", _androidVer);
            PlayerPrefs.Save();
        }
    }

    private static int _iosVer;
    public static int IOSResVer
    {
        get { return _iosVer; }
        set
        {
            _iosVer = value;
            PlayerPrefs.SetInt("TTBIOSVer", _iosVer);
            PlayerPrefs.Save();
        }
    }


    private void Awake()
    {
        _androidVer = PlayerPrefs.GetInt("IBAndroidVer", 1);
        _iosVer = 0;//PlayerPrefs.GetInt("TTBIOSVer", 1);
    }
    #endregion

    private void OnGUI()
    {
        titleContent = _title;
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Android资源:");
        EditorGUILayout.LabelField("旧版本号:" + _androidVer);
        EditorGUILayout.LabelField("新版本号:" + (_androidVer + 1));
        SetAndroidVer();
        EditorGUILayout.LabelField("IOS资源:");
        EditorGUILayout.LabelField("旧版本号:" + _iosVer);
        EditorGUILayout.LabelField("新版本号:" + (_iosVer + 1));
        SetIOSVer();
        
        EditorGUILayout.EndVertical();
    }

    void SetAndroidVer()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("生成Android资源版本"))
            GameResVerTools.GenAndroidResVer();
        if (GUILayout.Button("清除Anroid版本号后缀"))
            GameResVerTools.ClearAndroidExportVersion();
        if (GUILayout.Button("导出Anroid发布资源"))
            GameResVerTools.ExporeAndroidResVersion(_androidVer + 1);
        EditorGUILayout.EndHorizontal();
    }

    void SetIOSVer()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("生成IOS资源版本"))
            GameResVerTools.GenIOSResVer();
        if (GUILayout.Button("清除IOS版本号后缀"))
            GameResVerTools.ClearIOSExporeVersion();
        if (GUILayout.Button("导出IOS发布资源"))
            GameResVerTools.ExporeIOSResVersion(_iosVer + 1);
        EditorGUILayout.EndHorizontal();
    }
}
