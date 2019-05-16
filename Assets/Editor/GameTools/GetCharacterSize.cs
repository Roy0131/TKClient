using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Reflection;
using LitJson;

public class GetCharacterSize : MonoBehaviour {

    private string nameCh;
    private int widthCh;
    private int heightCh;

    //[MenuItem("Tool/GetCharacterSize")]
    static void GetSize()
    {
        string path = "Assets/Art/spine/role";


        if (Directory.Exists(path))
        {
            string[] folds = Directory.GetDirectories(path);

            foreach (var folder in folds)
            {
                //Debug.Log(folder + "-----------");
                foreach (var file in Directory.GetFiles(folder))
                {
                    if (file.EndsWith(".json"))
                    {
                        //Debug.Log(file + "-----------");
                        string[] name =  file.Split('.')[0].Split('\\');
                        Debuger.Log(name[name.Length-1]+"-----------");//角色名字
                        

                        string text = File.ReadAllText(file);
                        GetChSize(text);
                    }
                }
            }
        }
        Debug.Log("输出结束!");
    }
    static void GetChSize(string text)
    {
        //JsonData characterData = JsonMapper.ToObject(text);
        //Debuger.Log("宽度是"+characterData["skeleton"]["width"]+"    "+ "高度是" + characterData["skeleton"]["height"]);
    }
    static void CreateExcel()
    {

    }
}
     

