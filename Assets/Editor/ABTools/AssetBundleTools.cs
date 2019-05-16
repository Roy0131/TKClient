#region << 版 本 注 释 >>
/*
	 * ========================================================================
     * Copyright Notice  2016-2017 moyutec.com All rights reserved .
     * ========================================================================
     * 文件名	： AssetBundleTools 
     * 创建人	： roy
     * 创建时间	： 2017/2/20 15:35:20 
     * 描述  	：   	
*/
#endregion

using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;
public class AssetBundleTools
{
    static string GameResOrignalPath = Application.dataPath + "/Art/FinallyRes/Resources";
    static string UIImagePath = Application.dataPath + "/Art/FinallyRes/UIImage";
    static string EffectPath = Application.dataPath + "/Art/FinallyRes/Effect";
    private static List<string> _lstOneBundleDirs = new List<string>() { "bufficon", "configs", "guildicon", "systemicon", "campicon", "guildboss", "roleicon", "itemicon", "sound", "skillicon", "levelicon", "effect", "artifacticon", "welfare", "carnival" };

    [MenuItem("Tool/AB资源工具/生成All ab名字")]
    static void SetAllResABName()
    {
        SetLogicABName();
        SetPrefabABName();
        SetUIIMageABName();
        SetEffectABName();
    }

    [MenuItem("Tool/AB资源工具/拷贝HotDll")]
    static void CopyLogicDll()
    {
        EditorUtility.DisplayProgressBar("正在复制gamelogic代码文件", "请稍等...", 1f);
#if UNITY_ANDROID || UNITY_EDITOR
        string oriPath = Application.dataPath + "/../Library/ScriptAssemblies/GameLogic.dll";
#elif UNITY_IPHONE
        string oriPath = Application.dataPath + "/../Library/PlayerDataCache/Data/Managed/GameLogic.dll";
#endif
        string tPath = Application.dataPath + "/Art/FinallyRes/HotRes/";
        if (!Directory.Exists(tPath))
            Directory.CreateDirectory(tPath);
        string tFile = tPath + "ttbres.bytes";
        byte[] dbs = FileTool.ReadHotDllFile();
        byte[] keys = Encoding.UTF8.GetBytes("what fuck logic");
        Debuger.Log("key len:" + keys.Length);
        int len = keys.Length;
        byte[] dllBytes = new byte[dbs.Length + keys.Length];
        keys.CopyTo(dllBytes, 0);
        dbs.CopyTo(dllBytes, len);

        var target = new SnappyCompressor();
        var compressed = new byte[target.MaxCompressedLength(dllBytes.Length)];
        int compressedSize = target.Compress(dllBytes, 0, dllBytes.Length, compressed);
        var result = new byte[compressedSize];
        Array.Copy(compressed, result, compressedSize);

        if (File.Exists(tFile))
            File.Delete(tFile);
        using (FileStream fs = new FileStream(tFile, FileMode.OpenOrCreate, FileAccess.Write))
        {
            fs.Write(result, 0, result.Length);
        } 
        EditorUtility.ClearProgressBar();
    }

    [MenuItem("Tool/AB资源工具/生成HotDll ab名字")]
    static void SetLogicABName()
    {
        EditorUtility.DisplayProgressBar("正在设置hot logic名字", "请稍等...", 1f);
        string tPath = Application.dataPath + "/Art/FinallyRes/HotRes/";
        DirectoryInfo dir = new DirectoryInfo(tPath);
        FileInfo[] files = dir.GetFiles();
        if (files != null || files.Length >= 0)
        {
           string name = dir.Name + "/" + dir.Name + ".bytes";
            foreach (FileInfo fileInfo in files)
            {
                if (fileInfo.Extension.Contains(".meta"))
                    continue;
                string path = DataPathToAssetPath(fileInfo.FullName);
                string fileName = fileInfo.Name;
                AssetImporter import = AssetImporter.GetAtPath(path);
                if (import != null)
                {
                    name = dir.Name + "/" + fileName;
                    import.assetBundleName = name;
                }
            }
        }
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }

    [MenuItem("Tool/AB资源工具/生成Prefab ab名字")]
    static void SetPrefabABName()
    {
        EditorUtility.DisplayProgressBar("正在设置prefab ab名字", "请稍等...", 1f);
        SetAssetBundleName(GameResOrignalPath, true);
        EditorUtility.ClearProgressBar(); 
    }

    [MenuItem("Tool/AB资源工具/生成UI ab名字")]
    static void SetUIIMageABName()
    {
        EditorUtility.DisplayProgressBar("正在设置UI ab名字", "请稍等...", 1f);
        SetAssetBundleName(UIImagePath);
        EditorUtility.ClearProgressBar();
    }

    [MenuItem("Tool/AB资源工具/生成Effect ab名字")]
    static void SetEffectABName()
    {
        EditorUtility.DisplayProgressBar("正在设置UI ab名字", "请稍等...", 1f);
        SetAssetBundleName(EffectPath);
        SetAssetBundleName(EffectPath + "/Texture");
        EditorUtility.ClearProgressBar();
    }

    static void SetAssetBundleName(string resDir, bool blCheckOneBundle = false)
    {
        DirectoryInfo abResOrignal = new DirectoryInfo(resDir);
        DirectoryInfo[] dirInfos = abResOrignal.GetDirectories();
        string filterExtension = ".meta";
        string path;
        string name;
        bool blOneBundle = false;
        string fileName = "";
        int idx;
        foreach (DirectoryInfo dir in dirInfos)
        {
            FileInfo[] files = dir.GetFiles();
            if (blCheckOneBundle)
            {
                blOneBundle = _lstOneBundleDirs.Contains(dir.Name);
            }
            else
            {
                blOneBundle = true;
            }
            if (files != null || files.Length >= 0)
            {
                name = dir.Name + "/" + dir.Name + ".bytes";
                if (resDir == UIImagePath)
                    name = "uiimage/" + name;
                else if (resDir == GameResOrignalPath)
                    name = "prefabs/" + name;
                foreach (FileInfo fileInfo in files)
                {
                    if (fileInfo.Extension.Contains(filterExtension))
                        continue;
                    path = DataPathToAssetPath(fileInfo.FullName);
                    fileName = fileInfo.Name;
                    idx = fileName.LastIndexOf(".");
                    if (idx > 0)
                        fileName = fileName.Substring(0, idx);
                    AssetImporter import = AssetImporter.GetAtPath(path);
                    if (import != null)
                    {
                        if (blOneBundle)
                        {
                            import.assetBundleName = name;
                        }
                        else
                        {
                            name = dir.Name + "/" + fileName + ".bytes";
                            import.assetBundleName = name;
                        }
                    }
                }
            }
        }
    }


    [MenuItem("Tool/AB资源工具/删除Bundle名字")]
    static void RemoveAssetBundleName()
    {
        EditorUtility.DisplayProgressBar("Remove All Resource AssetBundleName", "Please wait...", 1f);

        int len = AssetDatabase.GetAllAssetBundleNames().Length;
        string[] assetBundleNames = new string[len];
        for (int i = 0; i < len; i++)
            assetBundleNames[i] = AssetDatabase.GetAllAssetBundleNames()[i];

        for (int j = 0; j < assetBundleNames.Length; j++)
            AssetDatabase.RemoveAssetBundleName(assetBundleNames[j], true);
        EditorUtility.ClearProgressBar();
    }


    static void BuildAssetBundle(BuildTarget target)
    {
        EditorUtility.DisplayProgressBar("Build All Resource to AssetBundle", "Please wait...", 1f);
        string abOutPath = Application.dataPath + "/../res/" + FileConst.RunPlatform + "/allres";
        if (!Directory.Exists(abOutPath))
            Directory.CreateDirectory(abOutPath);

        Debuger.LogWarning("[AssetBundleTools.BuildAssetBundle() => AB输出目录:" + abOutPath + "]");
        BuildPipeline.BuildAssetBundles(abOutPath, BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle, target);
        EditorUtility.ClearProgressBar();
    }

    [MenuItem("Tool/AB资源工具/生成Android资源")]
    static void BuildAndroidAssetBundle()
    {
        BuildAssetBundle(BuildTarget.Android);
    }

    [MenuItem("Tool/AB资源工具/生成ios资源")]
    static void BuildIOSAssetBundle()
    {
        BuildAssetBundle(BuildTarget.iOS);
    }

    [MenuItem("Tool/AB资源工具/生成win资源")]
    static void BuildWinAssetBundle()
    {
        BuildAssetBundle(BuildTarget.StandaloneWindows64);
    }

    public static string DataPathToAssetPath(string path)
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
            return path.Substring(path.IndexOf("Assets\\"));
        else
            return path.Substring(path.IndexOf("Assets/"));
    }

    static string UIImageDir = Application.dataPath + "/Art/UIImage/";
    [MenuItem("Tool/AB资源工具/UI资源设置Tag")]
    static void SetUIImageTag()
    {
        DirectoryInfo dir = new DirectoryInfo(UIImageDir);
        if (dir == null)
            return;
        DirectoryInfo[] dirs = dir.GetDirectories();
        if (dirs == null || dirs.Length == 0)
            return;
        EditorUtility.DisplayProgressBar("setting ui image packing tag", "Please wait...", 1f);
        FileInfo[] files;
        foreach (DirectoryInfo dirInfo in dirs)
        {
            files = dirInfo.GetFiles("*.png");
            ProcessPngTag(files, dirInfo.Name);
        }
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }

    static void ProcessPngTag(FileInfo[] files, string tag)
    {
        foreach (FileInfo info in files)
        {
            Texture2D texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(DataPathToAssetPath(info.FullName));
            if (texture2D == null)
                continue;
            TextureImporter importer = GetTextureSetting(texture2D);
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.spritePackingTag = tag;
            importer.SaveAndReimport();
        }
    }

    static TextureImporter GetTextureSetting(Texture2D texture)
    {
        string path = AssetDatabase.GetAssetPath(texture);
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        return importer;
    }
}