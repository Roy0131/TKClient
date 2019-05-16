using UnityEngine;
using UnityEditor;
using System.IO;

public class UIImageTools
{
    [MenuItem("Tool/UITools/设置UI图片TAG")]
    static void SetUIImageTag()
    {
        string imagePath = Application.dataPath + "/Art/FinallyRes/UIImage";
        DirectoryInfo dir = new DirectoryInfo(imagePath);
        DirectoryInfo[] childDirs = dir.GetDirectories();
        if(childDirs == null || childDirs.Length == 0)
        {
            Debuger.LogError("[UIImage.SetUIImageTag() => uiimage children directory was null..]");
            return; 
        }
        EditorUtility.DisplayProgressBar("设置UI图片TAG", "请稍等...", 1f);
        FileInfo[] files;
        foreach(DirectoryInfo childDir in childDirs)
        {
            files = childDir.GetFiles("*.png", SearchOption.AllDirectories);
            if (files == null || files.Length == 0)
                continue;
            ProccessByFiles(files, childDir.Name);
        }
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }

    static void ProccessByFiles(FileInfo[] files, string tag)
    {
        foreach (FileInfo file in files)
        {
            Texture2D texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetBundleTools.DataPathToAssetPath(file.FullName));
            if (texture2D == null)
                continue;
            TextureImporter importer = RoleTextureTool.GetTextureSetting(texture2D);
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.spritePackingTag = tag;
            importer.SaveAndReimport();
        }
    }

}
