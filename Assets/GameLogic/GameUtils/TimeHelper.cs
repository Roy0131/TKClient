#region << 版 本 注 释 >>
/*
	 * ========================================================================
     * Copyright Notice  2016-2017 moyutec.com All rights reserved .
     * ========================================================================
     * 文件名	： TimeHelper 
     * 创建人	： roy
     * 创建时间	： 2017/3/15 10:47:36 
     * 描述  	：   	
*/
#endregion

using System;
using System.Text.RegularExpressions;

public class TimeHelper
{
    /// <summary>
    /// 从1970年至今时间时间戳转换成xx前
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static string FormatTimeByTimeStamp(int time)
    {
        int seconds = Convert.ToInt32(DateTime.UtcNow.Subtract(DateTime.Parse("1970-1-1")).TotalSeconds);
        return FormatTimeBySecond(seconds - time);
    }

    public static string FormatTimeBySecond(int time)
    {
        if (time == 0)
            return " "+LanguageMgr.GetLanguage(5001540);
        if (time < 0)
            return " " + LanguageMgr.GetLanguage(5001541);
        if (time <= 60)
            return time + " " + LanguageMgr.GetLanguage(5001541);
        else if (time <= 3600)
            return Math.Ceiling((double)time / 60) + " " + LanguageMgr.GetLanguage(5001542);
        else if (time <= 86400)
            return Math.Ceiling((double)time / 3600) + " " + LanguageMgr.GetLanguage(5001543);
        else
            return Math.Ceiling((double)time / 86400) + " " + LanguageMgr.GetLanguage(5001544);
    }
    
    /// <summary>
    /// 时间戳转换成时间
    /// </summary>
    /// <param name="time">时间戳</param>
    /// <param name="format">时间格式，默认为yyyy-MM-dd</param>
    /// <param name="startTime"></param>
    /// <returns></returns>
    public static string GetTime(int time, string format = "yyyy-MM-dd HH:mm", params int[] startTime)
    {
        return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970,1,1,0,1,1).AddSeconds(time)).ToString(format);
    }
    public static string GetTimeS(int time, string format = "yyyy-MM-dd HH:mm:ss", params int[] startTime)
    {
        return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 1, 1).AddSeconds(time)).ToString(format);
    }
    public static string GetCountTime(int time)
    {
        string timeStr = "";
        timeStr += (time / 3600).ToString("#00") + ":";
        timeStr += (time % 3600 / 60).ToString("#00") + ":";
        timeStr += (time % 60).ToString("#00");
        return timeStr;
    }


    public static long Time2ServerTime(DateTime data)
    {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); 
        return  (long)(data - startTime).TotalSeconds;
    }
    
    //删除string中数字
    public static string RemoveNumber(string value)
    {
        return Regex.Replace(value, @"\d", "");
    }

    public static string RemoveNotNumber(string value)
    {
        return Regex.Replace(value, @"[^\d]", "");
    }

    /// <summary>   
    /// 得到字符串的长度，一个汉字算2个字符   
    /// </summary>   
    /// <param name="str">字符串</param>   
    /// <returns>返回字符串长度</returns>   
    public static int GetLength(string str)
    {
        if (str.Length == 0) return 0;
        
        System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
        int tempLen = 0;
        byte[] s = ascii.GetBytes(str);
        for (int i = 0; i < s.Length; i++)
        {
            if ((int)s[i] == 63)
            {
                tempLen += 2;
            }
            else
            {
                tempLen += 1;
            }
        }

        return tempLen;
    }
}