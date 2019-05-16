
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public static class ColliderHelper
{

    private static Color _colColor;

    public static void InitColor()
    {
        _colColor = Color.black;
        _colColor.a = 1f / 255f;
    }

    public static void SetButtonCollider(Transform buttonTF, float w = 100, float h = 100)
    {
        Image[] maskable = buttonTF.gameObject.GetComponents<Image>();
        DisableRaycastTarget(maskable);

        maskable = buttonTF.gameObject.GetComponentsInChildren<Image>(true);
        DisableRaycastTarget(maskable);

        GameObject collider = new GameObject("collider");
        Image colImage = collider.AddComponent<Image>();
        colImage.raycastTarget = true;
        colImage.color = _colColor;
        ObjectHelper.AddChildToParent(collider.transform, buttonTF);
    }

    private static void DisableRaycastTarget(Image[] values)
    {
        if (values == null || values.Length == 0)
            return;
        for (int i = 0; i < values.Length; i++)
            values[i].raycastTarget = false;
    }

}