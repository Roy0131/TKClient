//把sprite生成ui image prefab并存储到另一个文件夹下面
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

public class UICPrefab : ScriptableWizard
{
    //原图片路径
    public string path01 = "Assets\\Art\\Ui\\heroicon";
    //转换生成的prefab的路径
    public string path02 = "Assets\\Art\\Resources\\herocard";
    [MenuItem("Tool/UiCPrefab")]
    static void CreatePre()
    {
        ScriptableWizard.DisplayWizard<UICPrefab>("Create UiPrefab", "Create");
    }
    void OnWizardCreate()
    {
        //创建canvas
        GameObject objCanvas = new GameObject("Canvas", typeof(Canvas));
        //获取文件夹下面的png图片
        if (Directory.Exists(path01))
        {
            string[] files = Directory.GetFiles(path01);
            foreach (var file in files)
            {
                if (file.EndsWith(".png"))
                {
                    //得到名字
                    string[] fileArry = file.Split('\\');
                    string filenamepng = fileArry[fileArry.Length - 1];
                    string[] filenameArry = filenamepng.Split('.');
                    string filename = filenameArry[0];

                    //创建image 并设置父级是Canvas
                    GameObject objImage = new GameObject(filename, typeof(Image));
                    objImage.transform.SetParent(objCanvas.transform);

                    //对image大小，图片等等的设置
                    Image objimg = objImage.GetComponent<Image>();
                    objimg.sprite = AssetDatabase.LoadAssetAtPath(file, typeof(Sprite)) as Sprite;
                    objimg.preserveAspect = true;
                    objimg.SetNativeSize();

                    //创建prefab
                    PrefabUtility.CreatePrefab(path02 + "/" + filename + ".prefab", objImage);
                }

            }
            Debug.Log("创建prefab完成");
            //销毁创建的canvas
            DestroyImmediate(objCanvas);
        }
    }
    }

