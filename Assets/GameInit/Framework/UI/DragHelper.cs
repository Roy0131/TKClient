using UnityEngine;
using UnityEngine.EventSystems;
using Plugin;
using System;

public enum DragEventType
{
    None,
    BeginDrag,
    Dragging,
    EndDrag,
    MouseDown,
    MouseUp,
    DoubleClick,
}

public class DragHelper : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    
    public Action<DragEventType, PointerEventData, DragHelper> mDragMethod;

    public int mRoleId { get; set; }
    private void OnDragEvent(DragEventType type, PointerEventData evtData)
    {
        if (mDragMethod == null)
            return;
        mDragMethod.Invoke(type, evtData, this);
    }

    public void OnPointerUp(PointerEventData evtData)
    {
        OnDragEvent(DragEventType.MouseUp, evtData);
    }

    public void OnPointerDown(PointerEventData evtData)
    {
        OnDragEvent(DragEventType.MouseDown, evtData);
    }

    public void OnDrag(PointerEventData evtData)
    {
        OnDragEvent(DragEventType.Dragging, evtData);
    }

    public void OnEndDrag(PointerEventData evtData)
    {
        OnDragEvent(DragEventType.EndDrag, evtData);
    }

    public void OnBeginDrag(PointerEventData evtData)
    {
        OnDragEvent(DragEventType.BeginDrag, evtData);
    }

    private void OnDestroy()
    {
        mDragMethod = null;
    }
}
