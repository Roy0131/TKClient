using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class MouseEventListener : EventTrigger
{
    public Action<GameObject> mMouseClick;
    public Action<GameObject> mMouseDown;
    public Action<GameObject> mMouseUp;

    public static MouseEventListener Get(GameObject go)
    {
        MouseEventListener listener = go.GetComponent<MouseEventListener>();
        if (listener == null)
            listener = go.AddComponent<MouseEventListener>();
        return listener;
    }

	public override void OnPointerUp(PointerEventData eventData)
	{
        if (mMouseUp != null)
            mMouseUp.Invoke(gameObject);
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
        if (mMouseDown != null)
            mMouseDown.Invoke(gameObject);
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
        if (mMouseClick != null)
            mMouseClick.Invoke(gameObject);
	}
}