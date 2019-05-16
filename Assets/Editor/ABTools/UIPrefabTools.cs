using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIPrefabTools
{
    static string UIPrefabDir = Application.dataPath + "/Art/FinallyRes/Resources/uiprefabs";
    static string EffectDir = Application.dataPath + "/Art/FinallyRes/Resources/effect";

    static string UIImageDir = Application.dataPath + "/Art/FinallyRes/UIImage";

    static string AssetsDir = "Assets/Art/FinallyRes/UIImage/";

    static string EffectTextureDir = Application.dataPath + "/Art/FinallyRes/Effect/Texture";
    static string AssetsEffectTextureDir = "Assets/Art/FinallyRes/Effect/Texture/";

    //[MenuItem("Tool/UIPrefabs/RepairMissImage")]
    //static void RepairMissImage()
    //{
    //    //DirectoryInfo abResOrignal = new DirectoryInfo(UIPrefabDir);
    //    //FileInfo[] files = abResOrignal.GetFiles();
    //    //if (files.Length == 0)
    //    //    return;
    //    //string filterExtension = ".meta";
    //    //GameObject uiObject;
    //    Image[] allImages;
    //    //EditorUtility.DisplayProgressBar("uiprefab analysising", "please waiting...", 1f);
    //    //foreach (FileInfo file in files)
    //    //{
    //    //if (file.Extension.Contains(filterExtension))
    //    //    continue;
    //    GameObject uiObject = Selection.activeGameObject as GameObject;//AssetDatabase.LoadAssetAtPath<GameObject>(AssetBundleTools.DataPathToAssetPath(file.FullName));
    //    //if (uiObject == null)
    //    //    continue;
    //    allImages = uiObject.GetComponentsInChildren<Image>(true);
    //    for (int i = 0; i < allImages.Length; i++)
    //    {
    //        SerializedObject so = new SerializedObject(allImages[i]);
    //        var iter = so.GetIterator();
    //        while (iter.NextVisible(true))
    //        {
    //            if (iter.propertyType == SerializedPropertyType.ObjectReference)
    //            {
    //                if (iter.objectReferenceValue == null && iter.objectReferenceInstanceIDValue != 0)
    //                {
    //                    //AssetDatabase.get
    //                    //Debuger.Log(iter.serializedObject.targetObject.name + ", image name:" + allImages[i].name);
    //                }
    //            }
    //        }
    //    }
    //    //}
    //            //EditorUtility.ClearProgressBar();
    //    AssetDatabase.Refresh();
    //}

    [MenuItem("Tool/UIPrefabs/AnalysisPrefab")]
    static void AnalysisUIPrefab()
    {
        DirectoryInfo abResOrignal = new DirectoryInfo(UIPrefabDir);
        FileInfo[] files = abResOrignal.GetFiles();
        if (files.Length == 0)
            return;
        string filterExtension = ".meta";
        GameObject uiObject;
        Image[] allImages;
        EditorUtility.DisplayProgressBar("uiprefab analysising", "please waiting...", 1f);
        Dictionary<string, List<string>> dictUI4Image = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> dictImage4UI = new Dictionary<string, List<string>>();
        string path;
        List<string> lstUIs;
        foreach (FileInfo file in files)
        {
            if (file.Extension.Contains(filterExtension))
                continue;
            uiObject = AssetDatabase.LoadAssetAtPath<GameObject>(AssetBundleTools.DataPathToAssetPath(file.FullName));
            if (uiObject == null)
                continue;
            allImages = uiObject.GetComponentsInChildren<Image>(true);

            List<string> images;
            if (dictUI4Image.ContainsKey(uiObject.name))
            {
                images = dictUI4Image[uiObject.name];
            }
            else
            {
                images = new List<string>();
                dictUI4Image.Add(uiObject.name, images);
            }
            for (int i = 0; i < allImages.Length; i++)
            {
                path = AssetDatabase.GetAssetPath(allImages[i].sprite);
                if (!images.Contains(path))
                    images.Add(path);
                if (dictImage4UI.ContainsKey(path))
                {
                    lstUIs = dictImage4UI[path];
                }
                else
                {
                    lstUIs = new List<string>();
                    dictImage4UI.Add(path, lstUIs);
                }
                if (!lstUIs.Contains(uiObject.name))
                    lstUIs.Add(uiObject.name);
            }
        }
        CreateAnalysisText(dictUI4Image, Application.dataPath + "/Art/uiAnalysis.txt");
        CreateAnalysisText(dictImage4UI, Application.dataPath + "/Art/imageAnalysis.txt");
        SplitObjectImage(dictImage4UI);
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }

    static void SplitObjectImage(Dictionary<string, List<string>> value)
    {
        if (value == null || value.Count == 0)
            return;
        string dir;
        string childDir;
        foreach (var kv in value)
        {
            if (kv.Key.Contains("Resources") || string.IsNullOrEmpty(kv.Key))
                continue;
            if (kv.Value.Count > 1)
            {
                childDir = "Common";
                dir = UIImageDir + "/Common";
            }
            else
            {
                childDir = kv.Value[0];
                dir = UIImageDir + "/" + kv.Value[0];
            }
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            Debug.LogWarning("image:" + kv.Key);
            dir = AssetsDir + childDir + kv.Key.Substring(kv.Key.LastIndexOf("/"));
            Debug.Log(dir);
            AssetDatabase.MoveAsset(kv.Key, dir);
        }
    }

    static void CreateAnalysisText(Dictionary<string, List<string>> value, string fileName)
    {
        if (value == null || value.Count == 0)
            return;
        StringBuilder sb = new StringBuilder();
        foreach (var kv in value)
        {
            sb.Append(kv.Key);
            sb.Append("\n");
            for (int i = 0; i < kv.Value.Count; i++)
            {
                sb.Append("\t" + kv.Value[i]);
                sb.Append("\n");
            }
            sb.Append("\n");
        }

        using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
        {
            using (TextWriter textWriter = new StreamWriter(fileStream, Encoding.GetEncoding("utf-8")))
            {
                textWriter.Write(sb.ToString());
            }
        }
    }

    [MenuItem("Tool/Effects/AnalysisEffect")]
    static void AnalysisUIEffect()
    {
        DirectoryInfo uiPrefabDir = new DirectoryInfo(UIPrefabDir);
        FileInfo[] uiFiles = uiPrefabDir.GetFiles();
        DirectoryInfo effectPrefabDir = new DirectoryInfo(EffectDir);
        FileInfo[] effectFiles = effectPrefabDir.GetFiles();
        if (uiFiles.Length == 0 && effectFiles.Length == 0)
            return;
        FileInfo[] files = new FileInfo[uiFiles.Length + effectFiles.Length];
        if (uiFiles.Length > 0)
            uiFiles.CopyTo(files, 0);
        if (effectFiles.Length > 0)
            effectFiles.CopyTo(files, uiFiles.Length);
        string filterExtension = ".meta";
        GameObject gameObject;
        Renderer[] allRenders;
        EditorUtility.DisplayProgressBar("effect object analysising", "please waiting...", 1f);
        Dictionary<string, List<string>> dictObject4Textures = new Dictionary<string, List<string>>();
        Dictionary<string, List<string>> dictTexture4Objects = new Dictionary<string, List<string>>();
        List<string> lstMats;
        List<string> lstObjects;
        string path;
        foreach (FileInfo file in files)
        {
            if (file.Extension.Contains(filterExtension))
                continue;
            gameObject = AssetDatabase.LoadAssetAtPath<GameObject>(AssetBundleTools.DataPathToAssetPath(file.FullName));
            if (gameObject == null)
                continue;
            Debug.Log(gameObject.name);
            allRenders = gameObject.GetComponentsInChildren<Renderer>(true);

            if (allRenders == null || allRenders.Length == 0)
                continue;
            if (dictObject4Textures.ContainsKey(gameObject.name))
            {
                lstMats = dictObject4Textures[gameObject.name];
            }
            else
            {
                lstMats = new List<string>();
                dictObject4Textures.Add(gameObject.name, lstMats);
            }
            for (int i = 0; i < allRenders.Length; i++)
            {
                if (allRenders[i].sharedMaterial == null || allRenders[i].sharedMaterial.mainTexture == null)
                    continue;
                path = AssetDatabase.GetAssetPath(allRenders[i].sharedMaterial.mainTexture);
                if (!lstMats.Contains(path))
                    lstMats.Add(path);
                if (dictTexture4Objects.ContainsKey(path))
                {
                    lstObjects = dictTexture4Objects[path];
                }
                else
                {
                    lstObjects = new List<string>();
                    dictTexture4Objects.Add(path, lstObjects);
                }
                if (!lstObjects.Contains(gameObject.name))
                    lstObjects.Add(gameObject.name);
            }
        }
        CreateAnalysisText(dictObject4Textures, Application.dataPath + "/Art/object4Mat.txt");
        CreateAnalysisText(dictTexture4Objects, Application.dataPath + "/Art/mat4Object.txt");
        SplitEffectText(dictTexture4Objects);
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }

    static void SplitEffectText(Dictionary<string, List<string>> value)
    {
        if (value == null || value.Count == 0)
            return;
        string dir;
        string childDir;
        foreach (var kv in value)
        {
            if (kv.Key.Contains("Resources") || string.IsNullOrEmpty(kv.Key))
                continue;
            if (kv.Value.Count > 1)
            {
                childDir = "Common";
                dir = EffectTextureDir + "/Common";
            }
            else
            {
                childDir = kv.Value[0].Contains("fx_") ? "Battle" : "UI";
                dir = EffectTextureDir + "/" + childDir;
            }
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            Debug.LogWarning("image:" + kv.Key);
            dir = AssetsEffectTextureDir + childDir + kv.Key.Substring(kv.Key.LastIndexOf("/"));
            Debug.Log(dir);
            AssetDatabase.MoveAsset(kv.Key, dir);
        }
    }
}