using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class switchText
{
    static string UIPrefabDir = "Assets\\Art\\FinallyRes\\Resources\\uiprefabs";
    static string languageXML = "Assets\\Art\\FinallyRes\\Resources\\configs\\LanguageConfig.xml";
    [MenuItem("Tool/UITools/UITextSwitch")]
    static void CreatePre()
    {
        EditorUtility.DisplayProgressBar("uiprefabText Switch", "please waiting...", 5f);
        //创建canvas
        GameObject objCanvas = new GameObject("Canvas", typeof(Canvas));
        DirectoryInfo abResOrignal = new DirectoryInfo(UIPrefabDir);
        FileInfo[] files = abResOrignal.GetFiles();
        if (files.Length == 0)
            return;
        string filterExtension = ".meta";
        //GameObject uiObject;
        foreach (FileInfo file in files)
        {
            if (file.Extension.Contains(filterExtension))
                continue;
            //创建prefab
            GameObject objPrefab = AssetDatabase.LoadAssetAtPath(UIPrefabDir + "\\" + file.FullName.Split('\\')[file.FullName.Split('\\').Length - 1], typeof(GameObject)) as GameObject;
            GameObject objInsPre = GameObject.Instantiate(objPrefab);
            objInsPre.transform.SetParent(objCanvas.transform, false);
            OnSwitch(objInsPre);

            PrefabUtility.ReplacePrefab(objInsPre, objPrefab, ReplacePrefabOptions.Default);
        }

        GameObject.DestroyImmediate(objCanvas);
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }
    static void OnSwitch(GameObject objPrefab)
    {
        //获得xml下的item集合
        XmlDocument doc = new XmlDocument();
        doc.Load(languageXML);
        XmlElement root = doc.DocumentElement;
        XmlNodeList languageNodes = root.GetElementsByTagName("item");
        if (objPrefab)
        {
            Text[] instChilds = objPrefab.GetComponentsInChildren<Text>(true);
            foreach (var value in instChilds)
            {
                GameObject obj = value.gameObject;
                //GameObject objParent = obj.transform.parent.gameObject;
                //if (objParent.GetComponent<InputField>() != null)
                //{
                //    objParent.GetComponent<InputField>().textComponent = value;
                //}
                //创建物体继承text的数值
                GameObject objTmp = GameObject.Instantiate(obj);
                //销毁原有text组件，添加locationtext组件
                Component.DestroyImmediate(obj.GetComponent<Text>());
                obj.AddComponent<LocationText>();
                //把原有text属性赋值给locationtext组件
                LocationText textNew = obj.GetComponent<LocationText>();
                Text textTmp = objTmp.GetComponent<Text>();

                textNew.text = textTmp.text;
                textNew.font = textTmp.font;
                textNew.fontStyle = textTmp.fontStyle;
                textNew.fontSize = textTmp.fontSize;
                textNew.lineSpacing = textTmp.lineSpacing;

                textNew.alignment = textTmp.alignment;
                textNew.alignByGeometry = textTmp.alignByGeometry;
                textNew.horizontalOverflow = textTmp.horizontalOverflow;
                textNew.verticalOverflow = textTmp.verticalOverflow;
                textNew.resizeTextForBestFit = textTmp.resizeTextForBestFit;
                textNew.color = textTmp.color;
                textNew.material = textTmp.material;
                textNew.raycastTarget = textTmp.raycastTarget;
                //销毁用来复制属性的物体
                GameObject.DestroyImmediate(objTmp);
                //Write(value.text+"\r\n");
                //GameObject objParent = textNew.gameObject.transform.parent.gameObject;
                //if (objParent.GetComponent<InputField>() != null)
                //{
                //    objParent.GetComponent<InputField>().textComponent = textNew;
                //    textNew.supportRichText = false;
                //}
            }

            LocationText[] locationTexts = objPrefab.GetComponentsInChildren<LocationText>(true);
            foreach (var value in locationTexts)
            {
                GameObject parent = value.gameObject.transform.parent.gameObject;
                if (parent.GetComponent<InputField>() != null)
                {
                    parent.GetComponent<InputField>().textComponent = value;
                    value.supportRichText = false;
                }
                value.supportRichText = true;

                value.IDLanguage = GetXmlEng(value.text.Trim(), languageNodes);
                value.enabled = true;
                //if (objParent.transform.Find("Placeholder") != null)
                //    GameObject.Destroy(objParent.transform.Find("Placeholder"));
                if (value.IDLanguage == -100)
                {
                    //Write(value.text + "    " + objPrefab.name + "\r\n");
                    //Debug.Log(objPrefab.name);
                }
                //Debuger.Log(textNew.IDLanguage);
            }


        }
    }
    static int GetXmlEng(string value, XmlNodeList languageNodes)
    {
        if (string.IsNullOrEmpty(value) || IsChinese(value) || IsNum(value))
        {
            return 0;
        }

        for (int i = 0; i < languageNodes.Count; i++)
        {
            if (string.Equals(languageNodes[i].Attributes["English"].Value.ToUpper(), value.ToUpper()))
            {
                return XmlConvert.ToInt32(languageNodes[i].Attributes["ID"].Value);
            }
        }

        return -100;
    }
    static void Write(string value)
    {
        FileStream fs = new FileStream("E:\\TextNotInLanguage.txt", FileMode.Append);
        //获得字节数组
        byte[] data = System.Text.Encoding.Default.GetBytes(value);
        //开始写入
        fs.Write(data, 0, data.Length);
        //清空缓冲区、关闭流
        fs.Flush();
        fs.Close();
    }
    private static bool IsChinese(string str)
    {
        if (System.Text.RegularExpressions.Regex.IsMatch(str, @"[\u4e00-\u9fa5]"))
            return true;
        else return false;
    }
    private static bool IsNum(string str)
    {
        if (System.Text.RegularExpressions.Regex.IsMatch(str, @"^[0-9]*$"))
            return true;
        else return false;
    }
}
