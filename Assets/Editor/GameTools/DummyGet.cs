using System.IO;
using UnityEditor;
using UnityEngine;

public class DummyGet : MonoBehaviour
{
    [MenuItem("Tool/DummyGet")]
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
                        string[] name = file.Split('.')[0].Split('\\');
                        //Debug.Log(name[name.Length - 1] + "-----------");//角色名字
                        string text = File.ReadAllText(file);
                        GetDummy(text, name[name.Length - 1]);
                }
            }
        }
        Debug.Log("输出结束!");
    }
    }
    /// <summary>
    /// 判断是否存在dummy
    /// </summary>
    /// <param name="text"></param>
    /// <param name="name"></param>
    static void GetDummy(string text,string name)
    {
        int index01 = text.IndexOf("hit_dummy");
        if (index01 > -1)
        {
            //Debug.Log(name+ "找到了hit_dummy");
        }
        else
        {
            Debug.LogWarning(name+ "______未找到____________hit_dummy");
        }

        int index02 = text.IndexOf("fire_dummy_attack_01");
        if (index02 > -1)
        {
            //Debug.Log(name+ "找到了fire_dummy_01");
        }
        else
        {
            Debug.LogWarning(name + "______未找到______fire_dummy_attack_01");
        }
    }
}


