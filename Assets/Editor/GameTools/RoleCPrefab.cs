//把角色spine生成prefab并存储到另一个文件夹下面
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using Spine.Unity;

public class RoleCPrefab : ScriptableWizard
{
    //原角色路径
    public string path01 = "Assets\\Art\\spine\\role";
    //转换生成的prefab的路径
    //public string path02 = "Assets\\Art\\Resources\\rolesui";
    public string path02 = "Assets\\Art\\Resources\\roles";
    //[MenuItem("Tool/RoleUICPrefab")]
    [MenuItem("Tool/RoleCPrefab")]
    static void CreatePre()
    {
        ScriptableWizard.DisplayWizard<RoleCPrefab>("Create RolePrefab", "Create");
    }
    void OnWizardCreate()
    {
        if (Directory.Exists(path01))
        {
            string[] Directorys = Directory.GetDirectories(path01);
            foreach (var directory in Directorys)
            {
                string[] files = Directory.GetFiles(directory);
                foreach (var file in files)
                {
                    if (file.EndsWith("_SkeletonData.asset"))
                    {
                        string[] fileArry = file.Split('\\');
                        string filenameSke = fileArry[fileArry.Length - 1];
                        string[] filenameArry = filenameSke.Split('_');
                        string filename = filenameArry[0];

                        #region 创建ui角色
                        //SkeletonDataAsset _skeletonDataAsset = AssetDatabase.LoadAssetAtPath(file, typeof(SkeletonDataAsset)) as SkeletonDataAsset;
                        //GameObject gameObject = new GameObject(filename, typeof(SkeletonGraphic));
                        //SkeletonGraphic m_skeletonGraphic = gameObject.GetComponent<SkeletonGraphic>();
                        //m_skeletonGraphic.skeletonDataAsset = _skeletonDataAsset;
                        #endregion
                        #region 创建角色prefab
                        SkeletonDataAsset _skeletonDataAsset = AssetDatabase.LoadAssetAtPath(file, typeof(SkeletonDataAsset)) as SkeletonDataAsset;

                        GameObject gameObject = new GameObject(filename, typeof(SkeletonAnimation));
                        SkeletonAnimation m_skelAnim = gameObject.GetComponent<SkeletonAnimation>();
                        m_skelAnim.skeletonDataAsset = _skeletonDataAsset;
                        #endregion
                        PrefabUtility.CreatePrefab(path02 + "/" + filename + ".prefab", gameObject, ReplacePrefabOptions.Default);

                        DestroyImmediate(gameObject);
                    }
                }
            }
        }
        Debug.Log("角色创建成功-------");
        //Debug.Log("ui角色创建成功-------");
    }
}