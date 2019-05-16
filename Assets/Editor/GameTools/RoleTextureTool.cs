using System.IO;
using UnityEditor;
using UnityEngine;

public class RoleTextureTool
{
    [MenuItem("Tool/角色工具/设置角色贴图")]
    static void AdjustRoleTexture()
    {
        string path = Application.dataPath + "/Art/spine/role";
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] imgs = dir.GetFiles("*.png", SearchOption.AllDirectories);
        if (imgs.Length == 0)
            return;
        EditorUtility.DisplayProgressBar("设置角色贴图中", "请稍等...", 1f);
        foreach(FileInfo file in imgs)
        {
            Texture2D texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetBundleTools.DataPathToAssetPath(file.FullName));
            if (texture2D == null)
                continue;
            TextureImporter importer = GetTextureSetting(texture2D);
            importer.textureCompression = TextureImporterCompression.Compressed;
            //importer.textureFormat = TextureImporterFormat.RGBA16;
            importer.SaveAndReimport();
        }
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }

    public static TextureImporter GetTextureSetting(Texture2D texture)
    {
        string path = AssetDatabase.GetAssetPath(texture);
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        return importer;
    }
}