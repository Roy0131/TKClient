using UnityEngine;
using System.Collections.Generic;
using Framework.UI;

public enum RoleRTType
{
    None,
    RoleInfo,
    Lineup,
    Hangup,
    BeforeBattle,
    CTower,
    HeroCall,
}

public class RoleRTMgr : Singleton<RoleRTMgr>
{
    public Camera mCamera { get; private set; }
    private Dictionary<RoleRTType, RTLogicBase> _dictRTLogic;

    private GameObject _rtGameObject;
    private RTLogicBase _curRTLogic;
    
    public void InitRoleRTMode(GameObject gameObject)
    {
        _rtGameObject = gameObject;
        _rtGameObject.SetActive(true);

        _curRTLogic = null;
        Transform tf = _rtGameObject.transform;
        mCamera = tf.Find("Camera").GetComponent<Camera>();
        //Camera cam = _transform.Find("Camera").GetComponent<Camera>();
        mCamera.orthographicSize = GameUIMgr.Instance.blPadMode ? 5.2f : 3.9f;

        _dictRTLogic = new Dictionary<RoleRTType, RTLogicBase>();

        RTLogicBase logic;
        logic = new RoleInfoRTLogic();
        logic.Init(tf.Find("RoleInfo").gameObject);
        _dictRTLogic.Add(RoleRTType.RoleInfo, logic);

        logic = new HangupScene();
        logic.Init(tf.Find("Hangup").gameObject);
        _dictRTLogic.Add(RoleRTType.Hangup, logic);

        logic = new LineupScene();
        logic.Init(tf.Find("Lineup").gameObject);
        _dictRTLogic.Add(RoleRTType.Lineup, logic);

        logic = new BeforeRTLogic();
        logic.Init(tf.Find("BeforeBattle").gameObject);
        _dictRTLogic.Add(RoleRTType.BeforeBattle, logic);

        logic = new CTowerRTLogic();
        logic.Init(tf.Find("Tower").gameObject);
        _dictRTLogic.Add(RoleRTType.CTower, logic);

        logic = new HeroCallRTLogic();
        Transform heroCall = tf.Find("HeroCall");
        if (heroCall == null)
        {
            GameObject heroCallObj = new GameObject("HeroCall");
            heroCall = heroCallObj.transform;
            ObjectHelper.AddChildToParent(heroCallObj.transform, tf);
            heroCallObj.SetActive(false);

            GameObject tmpObject;
            for (int i = 0; i < 2; i++)
            {
                tmpObject = new GameObject("p" + (i + 1));
                ObjectHelper.AddChildToParent(tmpObject.transform, heroCall);
                tmpObject.layer = GameLayer.ModeUILayer;

                tmpObject.transform.localPosition = new Vector3(i * 3.2f, 0f, 0f);
            }

        }
        logic.Init(heroCall.gameObject);
        _dictRTLogic.Add(RoleRTType.HeroCall,logic);
        
    }

    private void AdjustCameraData(RoleRTType type)
    {
        switch(type)
        {
            case RoleRTType.Hangup:
                mCamera.orthographicSize = 2.55f;
                mCamera.farClipPlane = 1f;
                mCamera.nearClipPlane = -0.3f;
                mCamera.transform.localPosition = new Vector3(-0.4f, 0f, 0f);
                break;
            case RoleRTType.RoleInfo:
                mCamera.orthographicSize = 1.2f;
                mCamera.farClipPlane = 1f;
                mCamera.nearClipPlane = -0.3f;
                mCamera.transform.localPosition = new Vector3(0f, 0.9f, 0f);
                break;
            case RoleRTType.Lineup:
                mCamera.orthographicSize = 3.75f;
                mCamera.farClipPlane = 100f;
                mCamera.nearClipPlane = -2f;
                mCamera.transform.localPosition = new Vector3(0f, 3.32f, 0f);
                break;
            case RoleRTType.BeforeBattle:
                mCamera.orthographicSize = 4.52f;
                mCamera.farClipPlane = 1f;
                mCamera.nearClipPlane = -0.3f;
                mCamera.transform.localPosition = new Vector3(0.28f, 3.65f, 0f);
                break;
            case RoleRTType.CTower:
                mCamera.orthographicSize = 4.1f;
                mCamera.farClipPlane = 1f;
                mCamera.nearClipPlane = -0.3f;
                mCamera.transform.localPosition = new Vector3(0.28f, 3.47f, 0f);
                break;
            case RoleRTType.HeroCall:
                mCamera.orthographicSize = 2.5f;
                mCamera.farClipPlane = 1f;
                mCamera.nearClipPlane = -0.3f;
                mCamera.transform.localPosition = new Vector3(0f, 0.9f, 0f);
                break;
         
        }
    }

    private object _curData;
    private object _lastData;
    public bool _curShowHpBar;
    private bool _lastShowHpBar;

    public object GetLastData()
    {
        return _lastData;
    }

    public bool GetLastShowHpBar()
    {
        return _lastShowHpBar;
    }

    public void ShowRoleRTLogic(RoleRTType type, object data = null, bool showHpBar = false)
    {
        if (!_dictRTLogic.ContainsKey(type))
        {
            LogHelper.LogError("[RoleRTMgr.ShowRoleRTLogic() => type:" + type + " not found!!]");
            return;
        }
        AdjustCameraData(type);
        if (_curRTLogic != null)
        {
            if (_curRTLogic.mRTType == type)
            {
                _curData = _lastData = data;
                _curShowHpBar = _lastShowHpBar = showHpBar;
                _curRTLogic.Show(data, showHpBar);
                return;
            }
            _curRTLogic.Hide();
        }

        _lastData = _curData;
        _curData = data;

        _lastShowHpBar = _curShowHpBar;
        _curShowHpBar = showHpBar;


        _curRTLogic = _dictRTLogic[type];
        _curRTLogic.Show(data, showHpBar);
    }

    public T GetRoleRTLogicByType<T>(RoleRTType type) where T : RTLogicBase
    {
        if (_dictRTLogic.ContainsKey(type))
            return _dictRTLogic[type] as T;
        return default(T);
    }

    public void Hide(RoleRTType type)
    {
        if (_curRTLogic == null)
            return;
        if (_curRTLogic.mRTType != type)
            return;
        _curRTLogic.Hide();
        _curRTLogic = null;
    }

    public void Hide()
    {
        if (_curRTLogic == null)
            return;
        _curRTLogic.Hide();
        _curRTLogic = null;
    }

    public RenderTexture GetRoleRTImage()
    {
        return mCamera.targetTexture;
    }

    public void Dispose()
    {
        if (_dictRTLogic != null)
        {
            Dictionary<RoleRTType, RTLogicBase>.ValueCollection vall = _dictRTLogic.Values;
            foreach (RTLogicBase logic in vall)
                logic.Dispose();
            _dictRTLogic.Clear();
            _dictRTLogic = null;
        }
        mCamera = null;
        _rtGameObject = null;
        _curRTLogic = null;
    }
}
