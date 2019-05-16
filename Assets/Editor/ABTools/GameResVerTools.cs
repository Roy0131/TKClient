#region << 版 本 注 释 >>
/*
	 * ========================================================================
     * Copyright Notice  2016-2017 moyutec.com All rights reserved .
     * ========================================================================
     * 文件名	： GameResVerTools 
     * 创建人	： roy
     * 创建时间	： 2017/8/19 14:26:25 
     * 描述  	：   	
*/
#endregion

using System;
using System.IO;
using System.Text;
using UnityEngine;

public class GameResVerTools
{
    static string abResPath = Application.dataPath + "/../res/";

    static void DoResVersion(DirectoryInfo dir, int ver)
    {
        DirectoryInfo resDir = dir.GetDirectories()[0];
        FileInfo[] files = resDir.GetFiles("*.*", SearchOption.AllDirectories);
        string resMD5 = "";
        long resSize = 0;
        //创建一个StringBuilder存储数据
        StringBuilder sb = new StringBuilder();
        //int ver = GameResVerWindow.CurVers + 1;
        //创建Xml文件头
        sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        sb.Append("\r\n");
        //创建根节点
        sb.Append("<verRoot");
        sb.Append(" ver" + "=\"" + ver + "\"");
        sb.Append(" >");
        sb.Append("\r\n");
        string resName;
        for (int i = 0; i < files.Length; i++)
        {
            resMD5 = md5file(files[i].FullName);
            resSize = files[i].Length;
#if UNITY_ANDROID || UNITY_EDITOR
            resName = files[i].FullName.Substring(files[i].FullName.IndexOf("allres/"));
#elif UNITY_IPHONE
            resName = files[i].FullName.Substring(files[i].FullName.IndexOf("allres/"));
#endif
            resName = System.Security.SecurityElement.Escape(resName);
            resName = resName.Replace("\\", "/");
            sb.Append("     <item");
            sb.Append(" fileName" + "=\"" + resName + "\"");
            sb.Append(" resMD5" + "=\"" + resMD5 + "\"");
            sb.Append(" resSize" + "=\"" + resSize + "\"");
            sb.Append(" />");
            sb.Append("\n");
        }
        sb.Append("</verRoot>");
        string xmlFiles = dir.FullName + "/ver.xml";
        Debug.LogWarning("xmlFiles:" + xmlFiles);
        ////写入文件
        using (FileStream fileStream = new FileStream(xmlFiles, FileMode.Create, FileAccess.Write))
        {
            using (TextWriter textWriter = new StreamWriter(fileStream, Encoding.GetEncoding("utf-8")))
            {
                textWriter.Write(sb.ToString());
            }
        }
    }

#region android res version
    public static void GenAndroidResVer()
    {
        string androidResPath = abResPath + "android/";
        DirectoryInfo dir = new DirectoryInfo(androidResPath);
        DoResVersion(dir, GameResVerWindow.AnroidResVer + 1);
    }

    public static void ExporeAndroidResVersion(int ver)
    {
        string resPath = abResPath + "android/allres";
        DoExporeResVersion(resPath, ver);
        GameResVerWindow.AnroidResVer = ver;
    }

    public static void ClearAndroidExportVersion()
    {
        string path = abResPath + "android/allres";
        DoClearResVersion(path);
    }

#endregion

#region ios res version
    public static void GenIOSResVer()
    {
        string iosResPath = abResPath + "ios/";
        DirectoryInfo dir = new DirectoryInfo(iosResPath);
        DoResVersion(dir, GameResVerWindow.IOSResVer + 1);
    }

    public static void ExporeIOSResVersion(int ver)
    {
        string path = abResPath + "ios/allres";
        DoExporeResVersion(path, ver);
        GameResVerWindow.IOSResVer = ver;
    }

    public static void ClearIOSExporeVersion()
    {
        string path = abResPath + "ios/allres";
        DoClearResVersion(path);
    }
#endregion


    private static void DoExporeResVersion(string dirPath, int ver)
    {
        DirectoryInfo dir = new DirectoryInfo(dirPath);
        FileInfo[] files = dir.GetFiles("*.*", SearchOption.AllDirectories);
        string fileFullName;
        for (int i = 0; i < files.Length; i++)
        {
            fileFullName = files[i].FullName;
            File.Move(fileFullName, fileFullName + "_" + ver);
        }
    }

    private static void DoClearResVersion(string dirPath)
    {
        DirectoryInfo dir = new DirectoryInfo(dirPath);
        FileInfo[] files = dir.GetFiles("*.*", SearchOption.AllDirectories);
        string fileFullName;
        string newFileName;
        for (int i = 0; i < files.Length; i++)
        {
            fileFullName = files[i].FullName;
            if (fileFullName.Contains(".DS_Store") || fileFullName.Contains(".DS"))
                continue;
            if (!fileFullName.Contains("_"))
                continue;
            newFileName = fileFullName.Substring(0, fileFullName.LastIndexOf("_"));
            File.Move(fileFullName, newFileName);
        }
    }

    /// <summary>
    /// 计算文件的MD5值
    /// </summary>
    static string md5file(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
    }
}