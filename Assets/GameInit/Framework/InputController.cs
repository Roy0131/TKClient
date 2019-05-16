using System;
using UnityEngine;
using System.Collections.Generic;

public enum InputEventType
{
    MouseDown,
    MouseUp,
    MouseDrag,
    MouseClick,
}

public class InputController : RComponent
{
    public static InputController Instance { get; private set; }
    private Dictionary<InputEventType, Delegate> _dictAllInputEvent;
    public const float Drag_Gap = 20f;

    public void AddInputEvent(InputEventType eventType, Action<Vector2> method)
    {
        Delegate eventDele = null;
        if (_dictAllInputEvent.ContainsKey(eventType))
            eventDele = _dictAllInputEvent[eventType];
        _dictAllInputEvent[eventType] = (Action<Vector2>)Delegate.Combine((Action<Vector2>)eventDele, method);
    }

    public void RemoveInputEvent(InputEventType eventType, Action<Vector2> method)
    {
        if (!HasEvent(eventType))
            return;
        _dictAllInputEvent[eventType] = (Action<Vector2>)Delegate.Remove((Action<Vector2>)_dictAllInputEvent[eventType], method);
    }

    private bool HasEvent(InputEventType type)
    {
        if (!_dictAllInputEvent.ContainsKey(type))
            return false;
        return _dictAllInputEvent[type] != null;
    }

    private void DispatchInputEvent(InputEventType eventType, Vector2 p1)
    {
        if (!HasEvent(eventType))
            return;
        Delegate[] allDele = _dictAllInputEvent[eventType].GetInvocationList();
        Action<Vector2> method;
        for (int i = 0; i < allDele.Length; i++)
        {
            if (allDele[i].GetType() != typeof(Action<Vector2>))
                continue;
            method = (Action<Vector2>)allDele[i];
            if (method != null)
                method.Invoke(p1);
        }
    }

    private bool _blPressed;
    private bool _blClick;
    private bool _blTmpPressed;
    private Vector2 _oldMousePos;
    private Vector2 _tmpMousePos;

    protected override void OnAwake()
    {
        base.OnAwake();
        _dictAllInputEvent = new Dictionary<InputEventType, Delegate>();
        Instance = this;
    }

    private void OnEnable()
    {
        ResetInit();
    }
    private void OnDisable()
    {
        ResetInit();
    }

    private void ResetInit()
    {
        _blPressed = false;
        _blClick = false;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        _blTmpPressed = Input.GetMouseButton(0);
        _tmpMousePos = Input.mousePosition;

        if (_blTmpPressed != _blPressed)
        {
            if (_blTmpPressed)
            {
                _blClick = true;
                DispatchInputEvent(InputEventType.MouseDown, _tmpMousePos);
            }
            else
            {
                //Debug.Log("mouse up or mouse click, _blick:" + _blClick);
                DispatchInputEvent(InputEventType.MouseUp, _tmpMousePos);
                if (_blClick)
                    DispatchInputEvent(InputEventType.MouseClick, _tmpMousePos);
                _blClick = false;
            }
        }
        else if (_blClick && CheckMoved(_oldMousePos, _tmpMousePos))
            _blClick = false;
        else if (_blTmpPressed && !_blClick)
            DispatchInputEvent(InputEventType.MouseDrag, _tmpMousePos - _oldMousePos);
        _blPressed = _blTmpPressed;
        _oldMousePos = _tmpMousePos;
    }

    private static bool CheckMoved(Vector2 p1, Vector2 p2)
    {
        return Mathf.Abs(p1.x - p2.x) > Drag_Gap || Mathf.Abs(p1.y - p2.y) > Drag_Gap;
    }
}