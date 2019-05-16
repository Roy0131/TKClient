using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class uiQuickBtn
{
    private static GameObject CheckSelection(MenuCommand menuCommand)
    {
        GameObject selectedObj = menuCommand.context as GameObject;
        if (selectedObj == null)
            selectedObj = Selection.activeGameObject;
        if (selectedObj == null || selectedObj != null && selectedObj.GetComponentInParent<Canvas>() == null)
            return null;
        return selectedObj;
    }

    [MenuItem("GameObject/UGUI/Image #&z", false, 6)] 
    static void CreateImage(MenuCommand menuCommand)
    {
        GameObject selectedObj = CheckSelection(menuCommand);
        if (selectedObj == null)
            return;
        GameObject go = new GameObject("Image");
        GameObjectUtility.SetParentAndAlign(go, selectedObj);
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
        go.AddComponent<Image>();
    }

    [MenuItem("GameObject/UGUI/Text #&x", false, 6)]
    static void CreateText(MenuCommand menuCommand)
    {
        GameObject selectedObj = CheckSelection(menuCommand);
        if (selectedObj == null)
            return;
        GameObject go = new GameObject("Text");
        GameObjectUtility.SetParentAndAlign(go, selectedObj);
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;

        Text t = go.AddComponent<Text>();
        Font font = AssetDatabase.LoadAssetAtPath("Assets/Art/Font/IH.ttf", typeof(Font)) as Font;
        t.font = font;
        t.fontSize = 24;
        t.alignment = TextAnchor.MiddleCenter;
        t.color = Color.white;
        t.text = "New Text";
        t.rectTransform.sizeDelta = new Vector2(150f, 30f);
    }

    [MenuItem("GameObject/UGUI/Image #&b", false, 6)] 
 
    static void CreateButton(MenuCommand menuCommand)
    {
        GameObject selectedObj = CheckSelection(menuCommand);
        if (selectedObj == null)
            return;
        GameObject go = new GameObject("Button");
        GameObjectUtility.SetParentAndAlign(go, selectedObj);
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
        go.AddComponent<Image>();
        go.AddComponent<Button>();
    }

    [MenuItem("GameObject/UGUI/Image #&d", false, 6)]

    static void CreateOutline(MenuCommand menuCommand)
    {
        GameObject selectedObj = CheckSelection(menuCommand);
        if (selectedObj == null)
            return;
        Undo.AddComponent<Outline>( selectedObj);
        selectedObj.GetComponent<Outline>().effectColor = new Color(0f,0f,0f,0.5f);
    }

    [MenuItem("GameObject/UGUI/Image #&f", false, 6)]

    static void CreateShadow(MenuCommand menuCommand)
    {
        GameObject selectedObj = CheckSelection(menuCommand);
        if (selectedObj == null)
            return;
        Undo.AddComponent<Shadow>(selectedObj);
        selectedObj.GetComponent<Shadow>().effectColor = Color.black;
    }
}