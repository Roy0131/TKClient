using UnityEngine;
using Framework.UI;

public class SceneRenderUnit : UpdateNode
{
    public float mflHeight { get; protected set; }
    public float mflWidth { get; protected set; }
    public Transform mUnitRoot { get; protected set; }
    public Vector3 mDefaultPos { get; protected set; }


    protected GameObject _container;

    protected GameObject _modelObject;
    protected string _modelName;
    protected int _sortLayer;
    protected Renderer[] _renders;

    public BattleUnitType mUnitType { get; protected set; }
    protected int _layerOffest = 0;
    protected bool _blAlive = true;

    public SceneRenderUnit(BattleUnitType type)
    {
        mUnitType = type;
        _container = new GameObject();
        mUnitRoot = _container.transform;
        _blAlive = true;
    }

    protected override void OnInitData<T>(T data)
    {
        LoadModel();
    }

    protected void OnModelLoaded(GameObject modelObject)
    {
        _modelObject = modelObject;
        if (_modelObject == null)
        {
            LogHelper.LogError("[SceneRenderUnit.OnModelLoaded() => model object is null, name:" + _modelName + "]");
            return;
        }
        if (!_blAlive)
        {
            OnDisposeModel();
            return;
        }
        ParseComponent();
    }

    protected virtual void ParseComponent()
    {
        if (_modelObject == null)
            return;
        if (_container == null || _container.transform == null)
        {
            OnDisposeModel();
            return;
        }
        ObjectHelper.AddChildToParent(_modelObject.transform, _container.transform, false);
        ParseRenders();
    }

    protected virtual void LoadModel()
    {
        if (string.IsNullOrEmpty(_modelName))
        {
            LogHelper.LogError("[SceneRenderUnit.LoadModel() => model name is invalid, unit type:" + mUnitType + "]");
            return;
        }
        _container.name = _modelName;
        ResPoolMgr.Instance.GetBattleUnitObject(mUnitType, _modelName, OnModelLoaded);
    }

    protected virtual void ParseRenders()
    {
        Transform objTransform = _modelObject.transform;
        Renderer[] rootRenders = objTransform.GetComponents<Renderer>();
        Renderer[] childRenders = objTransform.GetComponentsInChildren<Renderer>(true);

        int rootLen = rootRenders == null ? 0 : rootRenders.Length;
        int childLen = childRenders == null ? 0 : childRenders.Length;

        _renders = new Renderer[rootLen + childLen];
        if (rootLen > 0)
            rootRenders.CopyTo(_renders, 0);
        if (childLen > 0)
            childRenders.CopyTo(_renders, rootLen);
    }

    public virtual void AddToStage(Transform parent)
    {
        Vector3 defScale = _container.transform.localScale;
        _container.transform.SetParent(parent, false);
        _container.transform.localScale = defScale;
        _container.transform.localPosition = Vector3.zero;
        _container.SetActive(true);

        ObjectHelper.SetObjectLayer(_container.transform, parent.gameObject.layer);
    }
    
    public virtual void UpdatePosition(Vector3 pos)
    {
        mUnitRoot.localPosition = pos;
        SortLayer = (int)(pos.y * 10);
    }

    protected override void OnDispose()
    {
        OnDisposeModel();
		mUnitRoot = null;
		_renders = null;
        _blAlive = false;
        base.OnDispose();
    }

    protected virtual void OnDisposeModel()
    {
        if (_modelObject != null)
        {
            ResPoolMgr.Instance.ReturnBattleObject(mUnitType, _modelName, _modelObject);
            _modelObject = null;
        }
        if (_container != null)
        {
            GameObject.Destroy(_container);
            _container = null;
        }
    }

    protected virtual int SortLayer
    {
        set
        {
            value = 100 - value + _layerOffest;
            if (_renders == null || value == _sortLayer)
                return;
            _sortLayer = value;
            for (int i = 0; i < _renders.Length; i++)
                _renders[i].sortingOrder = value;
        }
        get
        {
            return _sortLayer;
        }
    }

    public void ChangeSortLayerOffest(int value)
    {
        _layerOffest = value;
        SortLayer = (int)(mDefaultPos.y * 10);
    }

}