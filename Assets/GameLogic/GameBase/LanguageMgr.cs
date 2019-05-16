using UnityEngine;
using System;
using System.Reflection;
using System.Text;

public class LanguageMgr
{
    public static SystemLanguage curLanguage;// = Application.systemLanguage;//SystemLanguage.English;

    public static string GetLanguage(int id, params object[] args)
    {
        LanguageConfig cfg = LanguageConfig.Get(id);
        string result = id.ToString();
        if (cfg != null)
        {
            Type configType = cfg.GetType();
            PropertyInfo value = configType.GetProperty(curLanguage.ToString());
            if (value == null)
                value = configType.GetProperty(SystemLanguage.English.ToString());
            if (value != null)
                result = (string)value.GetValue(cfg, null);
        }
        if (args != null && args.Length > 0)
        {
            try
            {
                result = string.Format(result, args);
            }
            catch (FormatException e)
            {

                LogHelper.LogWarning("语言表（languageConfig.xml）“{}”配对错误:语言表错误位置ID:" + id + "--->错误信息：" + e.ToString());
            }
        }
        return ProgressNewLine(result);
    }

    /// <summary>
    /// string换行处理
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    static public string ProgressNewLine(string text)
    {
        StringBuilder newtext = new StringBuilder();
        int offset = 0;
        int length = text.Length;
        for (; offset < length; offset++)
        {
            char temp = text[offset];
            if (text[offset] == '\\')
            {
                if (offset < length - 1)
                {
                    if (text[offset + 1] == 'n')
                    {
                        temp = '\n';
                        offset++;

                    }
                }
            }
            newtext.Append(temp);
        }
        return newtext.ToString();
    }
}