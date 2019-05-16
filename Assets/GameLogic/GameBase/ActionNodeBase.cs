using System;

public abstract class ActionNodeBase : UpdateNode
{
    public ActionNodeBase()
    {
        AddEvent();
    }

    protected override void OnInitData<T>(T data)
    {
    }

    protected virtual void AddEvent()
    {

    }

    protected virtual void RemoveEvent()
    {

    }

    protected override void OnDispose()
    {
        RemoveEvent();
        base.OnDispose();
    }
}