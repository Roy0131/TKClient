using UnityEngine;
using System;

public class EffectBase : SceneRenderUnit
{
    public EffectDataVO mEffectData { get; private set; }
    public bool mBlEffectEnd { get; private set; }

    private float _flDurationTime;
    private FrameTicker _timeTicker;
    private Action<EffectBase> _effectEndMethod;

    public EffectBase(Action<EffectBase> method = null)
        : base(BattleUnitType.Effection)
    {
        _effectEndMethod = method;
    }

	protected override void OnInitData<T>(T data)
	{
        mEffectData = data as EffectDataVO;
        _modelName = mEffectData.mEffectConfig.Name;
        _flDurationTime = (float)mEffectData.mEffectConfig.Duration / 1000f;
        if (mEffectData.mEffectConfig.Duration > 0)
            _timeTicker = new FrameTicker(_flDurationTime, OnFinished);
        base.OnInitData(data);
        if (mEffectData.mEffectConfig.Layer == -1)
            _layerOffest = RenderLayerOffset.EffectBottom;
        else
            _layerOffest = RenderLayerOffset.EffectTop;
        mBlEffectEnd = mEffectData.mEffectConfig.Duration == 0;
    }

    protected override void LoadModel()
    {
        _container.name = _modelName;
        ResPoolMgr.Instance.GetBattleUnitObject(BattleUnitType.Bullet, _modelName, OnModelLoaded);
    }

    private void OnFinished()
    {
        mBlEffectEnd = true;
        if (_effectEndMethod != null)
            _effectEndMethod.Invoke(this);
        EffectMgr.Instance.DisposeEffect(this);
    }

    public override void UpdatePosition(Vector3 pos)
    {
        mUnitRoot.position = pos;
        SortLayer = (int)(mEffectData.mEffectPos.y * 10);
    }

    public override void AddToStage(Transform parent)
    {
        base.AddToStage(parent);
        mUnitRoot = _container.transform;
        UpdatePosition(mEffectData.mEffectPos);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (_timeTicker != null)
            _timeTicker.Update();
    }

    protected override void OnDisposeModel()
    {
        if (_modelObject != null)
        {
            ResPoolMgr.Instance.ReturnBattleObject(BattleUnitType.Bullet, _modelName, _modelObject);
            _modelObject = null;
        }
        if (_container != null)
        {
            GameObject.Destroy(_container);
            _container = null;
        }
    }

    protected override void OnDispose()
    {
        mEffectData = null;
        _effectEndMethod = null;
        base.OnDispose();
    }
}