using UnityEngine;

public class EffectDataVO : DataBaseVO
{
    public EffectConfig mEffectConfig { get; private set; }
    public Vector3 mEffectPos { get; private set; }

    public EffectDataVO(Vector3 pos)
    {
        mEffectPos = pos;
    }

    protected override void OnInitData<T>(T value)
    {
        mEffectConfig = value as EffectConfig;
    }

    public override void Dispose()
    {
        base.Dispose();
        mEffectConfig = null;
    }
}