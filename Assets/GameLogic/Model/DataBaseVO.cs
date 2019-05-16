using Framework.Core;
using System.Collections.Generic;

public interface IGameDataVO : IDispose
{
    void InitData<T>(T value);
    void RefreshData<T>(T value);
}

public abstract class DataBaseVO : IGameDataVO
{
    public void InitData<T>(T value)
    {
        OnInitData(value);
    }

    public void RefreshData<T>(T value)
    {
        OnRefreshData(value);
    }

    protected virtual void OnRefreshData<T>(T value)
    {

    }

    protected abstract void OnInitData<T>(T value);

    protected virtual void RemoveChildDataVO<T>(List<T> childs) where T : IDispose
    {
        if (childs == null)
            return;
        for (int i = childs.Count - 1; i >= 0; i--)
            childs[i].Dispose();
        childs.Clear();
    }

    public virtual void Dispose()
    {

    }
}