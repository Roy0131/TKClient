#region << 版 本 注 释 >>
/*
	 * ========================================================================
     * Copyright Notice  2016-2016 moyutec.com All rights reserved .
     * ========================================================================
     * 文件名	： FileTool 
     * 创建人	： roy
     * 创建时间	： 2016/12/19 16:20:17 
     * 描述  	： 文件操作工具
*/
#endregion

using System;
using System.IO;
using System.Text;

public class FileTool
{

    public static byte[] ReadHotDllFile()
    {
#if UNITY_ANDROID ||UNITY_EDITOR
        string filePath = UnityEngine.Application.dataPath + "/../Library/ScriptAssemblies/GameLogic.dll";//"/Art/FinallyRes/HotRes/ttbres.bytes";
#elif UNITY_IPHONE
        string filePath = UnityEngine.Application.dataPath + "/../Library/PlayerDataCache/Data/Managed/GameLogic.dll";
#endif
        if (!File.Exists(filePath))
        {
            Debuger.LogError("[FileTool.ReadHotDllFile() => read hot game logic dll failed!!]");
            return null;
        }
        try
        {
            using (Stream st = File.OpenRead(filePath))
            {
                byte[] result = new byte[st.Length];
                st.Read(result, 0, (int)st.Length);
                return result;
            }
        }
        catch (Exception)
        {
            Debuger.LogWarning("[FileTool.ReadHotDllFile() => read hot game logic dll failed, file:" + filePath + "]");
            return null;
        }
    }

    //read file from cache
    public static byte[] ReadCacheFile(string filePath)
    {
        filePath = FileConst.GetCacheFilePath(filePath);
        if (!File.Exists(filePath))
        {
            Debuger.LogWarning("[FileTool.ReadCacheFile() => file does not exist:" + filePath + "]");
            return null;
        }
        try
        {
            using (Stream st = File.OpenRead(filePath))
            {
                byte[] result = new byte[st.Length];
                st.Read(result, 0, (int)st.Length);
                return result;
            }
        }
        catch(Exception)
        {
            Debuger.LogWarning("[FileTool.ReadCacheFile() => read file from cache failed, file:" + filePath + "]");
            return null;
        }
    }

    //write file to cache
    public static void WriteFileToCache(string filePath, byte[] value)
    {
        filePath = FileConst.GetCacheFilePath(filePath);
        string dirPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);
        try
        {
            using (FileStream fs = File.Create(filePath))
                fs.Write(value, 0, value.Length);
        }
        catch(Exception)
        {
            Debuger.LogWarning("[FileTool.WriteFileToCache() => write file failed, file path:" + filePath + "]");
        }
    }

    //write file to cache
    public static void WriteFileToCache(string filePath, string value)
    {
        byte[] bs = Encoding.UTF8.GetBytes(value);
        WriteFileToCache(filePath, bs);
    }

    public static string TrimUnicode(string value)
    {
        if (value.Length > 0 && value[0] == 0xFEFF)
            return value.Substring(1);
        return value;
    }

    public static string[] SpliteVerTxt(string value)
    {
        return value.Split(new string[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
    }
}