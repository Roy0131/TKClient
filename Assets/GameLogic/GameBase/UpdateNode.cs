using Framework.Core;

public abstract class UpdateNode : IDispose
{
    protected bool _blInitialed = false;
    public UpdateNode()
    {

    }

    public virtual void InitData<T>(T data)
    {
        _blInitialed = true;
        OnInitData(data);
    }

    public virtual void Update()
    {
        if (!_blInitialed)
            return;
        OnUpdate();
    }
    
    public void Dispose()
    {
        if (!_blInitialed)
            return;
        _blInitialed = true;
        OnDispose();
    }

    protected abstract void OnInitData<T>(T data);

    protected virtual void OnUpdate()
    {
    }

    protected virtual void OnDispose()
    {
    }
}