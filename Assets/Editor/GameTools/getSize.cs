using System.IO;
using UnityEditor;
using UnityEngine;

public class getSize : MonoBehaviour {

    [MenuItem("Tool/getCharacterSize")]
    static void getTheSize()
    {
        string path = "Assets/Art/Resources/roles";
        if (Directory.Exists(path))
        {
            foreach (var file in Directory.GetFiles(path))
            {
                if (file.EndsWith(".prefab"))
                {
                    string[] name = file.Split('.')[0].Split('\\');
                    string heroName = name[name.Length - 1];//角色名字

                    string heroCreate = "roles/"+ heroName;
                    GameObject instance = Instantiate(Resources.Load<GameObject>(heroCreate));
                    float width = (int)(instance.GetComponent<MeshRenderer>().bounds.size.x * 100.0f);
                    float height = (int)(instance.GetComponent<MeshRenderer>().bounds.size.y * 100.0f);
                    Debug.Log(heroName+"的宽度是"+width+",高度是"+height);
                    DestroyImmediate(instance);
                }
            }
        }
    }
}
