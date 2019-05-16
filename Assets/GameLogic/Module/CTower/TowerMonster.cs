using UnityEngine;
using System;

public class TowerMonster : AnimatorFighter
{
    public CardDataVO mCardDataVO { get; private set; }
    public TowerMonster()
    : base(BattleUnitType.AnimatorFighter)
    {

    }
    protected override void OnInitData<T>(T data)
    {
        mCardDataVO = data as CardDataVO;
        _modelName = mCardDataVO.mCardConfig.Model;
        base.OnInitData(data);
    }
    protected override void LoadModel()
    {
        if (string.IsNullOrEmpty(_modelName))
            return;

        Action<GameObject> OnLoaded = (roleObject) =>
        {
            _modelObject = roleObject;
            if (_modelObject == null)
                return;
            _modelObject.layer = GameLayer.ModeUILayer;
            ParseRenders();
            ParseAnimator();
        };

        RoleResPool.Instance.GetRole(_modelName, OnLoaded);
    }
    public override void AddToStage(Transform parent)
    {
        base.AddToStage(parent);
        mUnitRoot = parent;
        mDefaultPos = parent.localPosition;

        PlayAction(ActionName.Idle, true);
        SortLayer = (int)(mDefaultPos.y * 10);
        mUnitRoot.gameObject.SetActive(true);
    }
    protected override void OnDispose()
    {
        mCardDataVO = null;
        base.OnDispose();
    }

    protected override void OnDisposeModel()
    {
        if (_modelObject != null)
            RoleResPool.Instance.ReturnRoleObject(_modelName, _modelObject);
        _modelObject = null;
    }

    public void OnDrag()
    {
        _layerOffest = RenderLayerOffset.Max;
    }

    public void OnDragEnd()
    {
        _layerOffest = RenderLayerOffset.None;
    }

    public override void UpdatePosition(Vector3 pos)
    {
        mUnitRoot.position = pos;
        SortLayer = (int)(pos.y * 10);
    }
}
