using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 自适应iPhoneX背景
/// </summary>
public class UIRectLayout : MonoBehaviour
{
#if UNITY_IPHONE
	void Awake ()
    {
		if (GameDriver.ISIPHONEX)
        {
			RectTransform rectTransform = transform as RectTransform;
			if (rectTransform.anchorMax.x == 1f)
            {
				rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x - 44f,rectTransform.offsetMin.y - 44f);
				rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x + 44f,rectTransform.offsetMax.y);
			}
		}
	}
#endif
}