using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Reflection;

public class GetRGBSetETC
{

    public static float sizeScale = 0.5f;   //the size decrease scale for alphaTexture  
    public static Dictionary<string, bool> texturesAlphaDic = new Dictionary<string, bool>();

    //[MenuItem("Tool/SetPicEtc")]
    private static void SetPicEtc()
    {
        Debug.Log("开始转换!");
        string path = "Assets\\Art\\spine\\role";
        
        if (Directory.Exists(path))
        {
            string[] folds = Directory.GetDirectories(path);

            foreach (var folder in folds)
            {
                //Debug.Log(folder + "-----------");
                foreach (var file in Directory.GetFiles(folder))
                {
                    if (file.EndsWith(".png"))
                    {
                        //Debug.Log(file+"-----------");
                        SeperateRGBAandlphaChannel(file);
                    }
                }
            }
        }
        Debug.Log("转换成功!");
    }

    //修改提取RGB
    #region

    static void SeperateRGBAandlphaChannel(string _texPath)
    {
        string assetRelativePath = GetRelativeAssetPath(_texPath);
        SetTextureReadable(assetRelativePath);
        Texture2D sourcetex = AssetDatabase.LoadAssetAtPath(assetRelativePath, typeof(Texture2D)) as Texture2D;  //not just the textures under Resources file  
        if (!sourcetex)
        {
            Debug.Log("Load Texture Failed : " + assetRelativePath);
            return;
        }
        if (!HasAlphaChannel(sourcetex))
        {
            Debug.Log("Texture does not have Alpha channel : " + assetRelativePath);
            return;
        }

        Texture2D rgbTex = new Texture2D(sourcetex.width, sourcetex.height, TextureFormat.RGB24, true);
        
        Texture2D alphaTex = new Texture2D((int)(sourcetex.width * sizeScale), (int)(sourcetex.height * sizeScale), TextureFormat.RGB24, true);

        for (int i = 0; i < sourcetex.width; ++i)
            for (int j = 0; j < sourcetex.height; ++j)
            {
                Color color = sourcetex.GetPixel(i, j);
                Color rgbColor = color;
                Color alphaColor = color;
                alphaColor.r = color.a;
                alphaColor.g = color.a;
                alphaColor.b = color.a;
                rgbTex.SetPixel(i, j, rgbColor);
                alphaTex.SetPixel((int)(i * sizeScale), (int)(j * sizeScale), alphaColor);
            }

        rgbTex.Apply();
        alphaTex.Apply();

        byte[] bytes = rgbTex.EncodeToPNG();
        File.WriteAllBytes(GetRGBTexPath(_texPath), bytes);
        bytes = alphaTex.EncodeToPNG();
        File.WriteAllBytes(GetAlphaTexPath(_texPath), bytes);
        Debug.Log("Succeed to seperate RGB and Alpha channel for texture : " + assetRelativePath);
    }

    static bool HasAlphaChannel(Texture2D _tex)
    {
        for (int i = 0; i < _tex.width; ++i)
            for (int j = 0; j < _tex.height; ++j)
            {
                Color color = _tex.GetPixel(i, j);
                float alpha = color.a;
                if (alpha < 1.0f - 0.001f)
                {
                    return true;
                }
            }
        return false;
    }

    static void SetTextureReadable(string _relativeAssetPath)
    {
        string postfix = GetFilePostfix(_relativeAssetPath);
        if (postfix == ".dds")    // no need to set .dds file.  Using TextureImporter to .dds file would get casting type error.  
        {
            return;
        }

        TextureImporter ti = (TextureImporter)TextureImporter.GetAtPath(_relativeAssetPath);
        ti.isReadable = true;
        AssetDatabase.ImportAsset(_relativeAssetPath);
    }

    #endregion
    static string GetRGBTexPath(string _texPath)
    {
        return GetTexPath(_texPath, "_RGB.");
    }

    static string GetAlphaTexPath(string _texPath)
    {
        return GetTexPath(_texPath, "_Alpha.");
    }

    static string GetTexPath(string _texPath, string _texRole)
    {
        string result = _texPath.Replace(".", _texRole);
        string postfix = GetFilePostfix(_texPath);
        return result.Replace(postfix, ".png");
    }

    static string GetRelativeAssetPath(string _fullPath)
    {
        _fullPath = GetRightFormatPath(_fullPath);
        int idx = _fullPath.IndexOf("Assets");
        string assetRelativePath = _fullPath.Substring(idx);
        return assetRelativePath;
    }

    static string GetRightFormatPath(string _path)
    {
        return _path.Replace("\\", "/");
    }

    static string GetFilePostfix(string _filepath)   //including '.' eg ".tga", ".dds"  
    {
        string postfix = "";
        int idx = _filepath.LastIndexOf('.');
        if (idx > 0 && idx < _filepath.Length)
            postfix = _filepath.Substring(idx, _filepath.Length - idx);
        return postfix;
    }

}