using UnityEngine;
using NewBieGuide;

public class MainMapMgr : Singleton<MainMapMgr>
{

    public float Drag_Gap = 10f;
    private bool _blPressed;
    private bool _blClick;
    private bool _blTmpPressed;
    private Vector2 _oldMousePos;
    private Vector2 _tmpMousePos;

    private float _curTopX = 0f;
    private float _curMiddleX = 0f;
    private float _curBottomX = 0f;

    private Transform _topTransform;
    private Transform _middleTransform;
    private Transform _bottonTransfrom;

    private GameObject _yingxiongjiuguanTitle01;
    private GameObject _yingxiongjiuguanTitle02;
    private GameObject _shuijingshuTitle01;
    private GameObject _shuijingshuTitle02;
    private GameObject _tansuodidaiTitle01;
    private GameObject _tansuodidaiTitle02;
    private GameObject _mofajitanTitle01;
    private GameObject _mofajitanTitle02;
    private GameObject _shenmishangdianTitle01;
    private GameObject _shenmishangdianTitle02;
    private GameObject _wuqishiyanshiTitle01;
    private GameObject _wuqishiyanshiTitle02;
    private GameObject _yuanzhengTitle01;
    private GameObject _yuanzhengTitle02;
    private GameObject _jingjichangTitle01;
    private GameObject _jingjichangTitle02;
    private GameObject _hangupTitle01;
    private GameObject _hangupTitle02;
    private GameObject _caihongqiaoTitle01;
    private GameObject _caihongqiaoTitle02;
    private GameObject _taikongdiantiTitle01;
    private GameObject _taikongdiantiTitle02;
    private GameObject _fukongdaoTitle01;
    private GameObject _fukongdaoTitle02;

    public void Init(GameObject mapObject)
    {
        Transform _transform = mapObject.transform;
        _yingxiongjiuguanTitle01 = _transform.Find("Second/Map_Bg_yingxiongjiuguan/panel_jainzhumingchengdi_all/GameObject").gameObject;
        _yingxiongjiuguanTitle02 = _transform.Find("Second/Map_Bg_yingxiongjiuguan/panel_jainzhumingchengdi_all/GameObject (1)").gameObject;

        _shuijingshuTitle01 = _transform.Find("Second/Map_Bg_shuijingshu/panel_jainzhumingchengdi_all/GameObject").gameObject;
        _shuijingshuTitle02 = _transform.Find("Second/Map_Bg_shuijingshu/panel_jainzhumingchengdi_all/GameObject (1)").gameObject;

        _tansuodidaiTitle01 = _transform.Find("Second/Map_Bg_tansuodidai/panel_jainzhumingchengdi_all/GameObject").gameObject;
        _tansuodidaiTitle02 = _transform.Find("Second/Map_Bg_tansuodidai/panel_jainzhumingchengdi_all/GameObject (1)").gameObject;

        _mofajitanTitle01 = _transform.Find("Second/Map_Bg_mofajitan/panel_jainzhumingchengdi_all/GameObject").gameObject;
        _mofajitanTitle02 = _transform.Find("Second/Map_Bg_mofajitan/panel_jainzhumingchengdi_all/GameObject (1)").gameObject;

        _shenmishangdianTitle01 = _transform.Find("Second/Map_Bg_shenmishangdian/panel_jainzhumingchengdi_all/GameObject").gameObject;
        _shenmishangdianTitle02 = _transform.Find("Second/Map_Bg_shenmishangdian/panel_jainzhumingchengdi_all/GameObject (1)").gameObject;

        _wuqishiyanshiTitle01 = _transform.Find("Second/Map_Bg_wuqishiyanshi/panel_jainzhumingchengdi_all/GameObject").gameObject;
        _wuqishiyanshiTitle02 = _transform.Find("Second/Map_Bg_wuqishiyanshi/panel_jainzhumingchengdi_all/GameObject (1)").gameObject;

        _yuanzhengTitle01 = _transform.Find("Second/Map_Bg_yuanzheng/panel_jainzhumingchengdi_all/GameObject").gameObject;
        _yuanzhengTitle02 = _transform.Find("Second/Map_Bg_yuanzheng/panel_jainzhumingchengdi_all/GameObject (1)").gameObject;

        _jingjichangTitle01 = _transform.Find("Second/Map_Bg_jingjichang/panel_jainzhumingchengdi_all/GameObject").gameObject;
        _jingjichangTitle02 = _transform.Find("Second/Map_Bg_jingjichang/panel_jainzhumingchengdi_all/GameObject (1)").gameObject;

        _hangupTitle01 = _transform.Find("Second/Map_hangup/panel_jainzhumingchengdi_all/GameObject").gameObject;
        _hangupTitle02 = _transform.Find("Second/Map_hangup/panel_jainzhumingchengdi_all/GameObject (1)").gameObject;

        _caihongqiaoTitle01 = _transform.Find("Second/Map_Bg_caihongqiao/panel_jainzhumingchengdi_all/GameObject").gameObject;
        _caihongqiaoTitle02 = _transform.Find("Second/Map_Bg_caihongqiao/panel_jainzhumingchengdi_all/GameObject (1)").gameObject;

        _taikongdiantiTitle01 = _transform.Find("Second/Map_Bg_taikongdianti/panel_jainzhumingchengdi_all/GameObject").gameObject;
        _taikongdiantiTitle02 = _transform.Find("Second/Map_Bg_taikongdianti/panel_jainzhumingchengdi_all/GameObject (1)").gameObject;

        _fukongdaoTitle01 = _transform.Find("Second/Map_Bg_fukongdao/panel_jainzhumingchengdi_all/GameObject").gameObject;
        _fukongdaoTitle02 = _transform.Find("Second/Map_Bg_fukongdao/panel_jainzhumingchengdi_all/GameObject (1)").gameObject;

        _topTransform = _transform.Find("First");
        _middleTransform = _transform.Find("Second");
        _bottonTransfrom = _transform.Find("Third");
        _curTopX = _flTmpTopX = _topTransform.localPosition.x;
        _curMiddleX = _flTmpMidX = _middleTransform.localPosition.x;
        _curBottomX = _flTmpBottomX = _bottonTransfrom.localPosition.x;
        Camera cam = _transform.Find("Camera").GetComponent<Camera>();
        cam.orthographicSize = GameUIMgr.Instance.blPadMode ? 5f : 4f;
        if (GameDriver.ISIPHONEX)
        {
            _leftMax = 3.7f;
            _rightMax = -2.5f;
        }
        else
        {
            _leftMax = 4.5f;
            _rightMax = -3f;
        }

        BindFuncRedPoint();
        RegisteGuideMaskObject();
        SetBuildName();
        _blEnable = false;

        NameObject();
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(HeroEvent.UpGrade, NameObject);
    }

    private void BindFuncRedPoint()
    {
        RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.EquipFusion, _middleTransform.Find("Map_Bg_wuqishiyanshi/icon_tishi_01").gameObject);
        RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.RoleFusion, _middleTransform.Find("Map_Bg_caihongqiao/icon_tishi_01").gameObject);
        RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.Explore, _middleTransform.Find("Map_Bg_tansuodidai/icon_tishi_01").gameObject);
        RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.Draw, _middleTransform.Find("Map_Bg_yingxiongjiuguan/icon_tishi_01").gameObject);
    }

    public void UnBindFuncRedPoint()
    {
        //Debuger.LogError("[MainMapMgr.UnBindFuncRedPoint() => unBunding RedPoint object]");
        RedPointTipsMgr.Instance.RedPointUnBindObject(RedPointEnum.EquipFusion, _middleTransform.Find("Map_Bg_wuqishiyanshi/icon_tishi_01").gameObject);
        RedPointTipsMgr.Instance.RedPointUnBindObject(RedPointEnum.RoleFusion, _middleTransform.Find("Map_Bg_caihongqiao/icon_tishi_01").gameObject);
        RedPointTipsMgr.Instance.RedPointUnBindObject(RedPointEnum.Explore, _middleTransform.Find("Map_Bg_tansuodidai/icon_tishi_01").gameObject);
        RedPointTipsMgr.Instance.RedPointUnBindObject(RedPointEnum.Draw, _middleTransform.Find("Map_Bg_yingxiongjiuguan/icon_tishi_01").gameObject);
    }

    private void RegisteGuideMaskObject()
    {
        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.RecruitBuild, _middleTransform.Find("Map_Bg_yingxiongjiuguan/MaskPoint"));
        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.HangupBuild, _middleTransform.Find("Map_hangup/MaskPoint"));
        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.DeComposeModule, _middleTransform.Find("Map_Bg_mofajitan/MaskPoint"));
        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.EquipmentModule, _middleTransform.Find("Map_Bg_wuqishiyanshi/MaskPoint"));
    }

    private void UnRegisteGuideMaskObject()
    {
        //Debuger.LogError("[MainMapMgr.UnRegisteGuideMaskObject() => unRegiste guide mask object!!]");
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.RecruitBuild);
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.HangupBuild);
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.DeComposeModule);
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.EquipmentModule);
    }

    private void NameObject()
    {
        _middleTransform.Find("Map_Bg_shuijingshu/panel_jainzhumingchengdi_all").gameObject.SetActive(false);
        _middleTransform.Find("Map_Bg_fukongdao/panel_jainzhumingchengdi_all").gameObject.SetActive(false);
        _middleTransform.Find("Map_Bg_yuanzheng/panel_jainzhumingchengdi_all").gameObject.SetActive(false);
        if (HeroDataModel.Instance.mHeroInfoData != null)
        {
            _middleTransform.Find("Map_Bg_tansuodidai/panel_jainzhumingchengdi_all").gameObject.SetActive(HeroDataModel.Instance.mHeroInfoData.mLevel >= GameConst.GetFeatureType(FunctionType.Explore));
            _middleTransform.Find("Map_Bg_jingjichang/panel_jainzhumingchengdi_all").gameObject.SetActive(HeroDataModel.Instance.mHeroInfoData.mLevel >= GameConst.GetFeatureType(FunctionType.Arena));
            _middleTransform.Find("Map_Bg_taikongdianti/panel_jainzhumingchengdi_all").gameObject.SetActive(HeroDataModel.Instance.mHeroInfoData.mLevel >= GameConst.GetFeatureType(FunctionType.Tower));
            _middleTransform.Find("Map_Bg_shuijingshu/panel_jainzhumingchengdi_all").gameObject.SetActive(HeroDataModel.Instance.mHeroInfoData.mLevel >= GameConst.GetFeatureType(FunctionType.HeroCall));
            _middleTransform.Find("Map_Bg_yuanzheng/panel_jainzhumingchengdi_all").gameObject.SetActive(HeroDataModel.Instance.mHeroInfoData.mLevel >= GameConst.GetFeatureType(FunctionType.Expedition));
        }
    }

    private void OnEnable()
    {
        _blTmpPressed = false;
        _blPressed = false;
    }

    private void OnDisable()
    {
        _blTmpPressed = false;
        _blPressed = false;
    }

    private void DoMapFunction(string funName)
    {
        LogHelper.LogWarning("funName:" + funName);
        switch(funName)
        {
            case "Map_hangup":
                HangupDataModel.Instance.ReqCampaignData();
                break;
            case "Map_Bg_taikongdianti":
                //GameUIMgr.Instance.OpenModule(ModuleID.CTower);
                if (FunctionUnlock.IsUnlock(FunctionType.Tower))
                    CTowerDataModel.Instance.ReqTowerData();
                break;
            case "Map_Bg_yingxiongjiuguan":
                RecruitDataModel.Instance.ReqDrawCardList();
                break;
            case "Map_Bg_caihongqiao":
                GameUIMgr.Instance.OpenModule(ModuleID.RoleFusion);
                break;
            case "Map_Bg_mofajitan":
                GameUIMgr.Instance.OpenModule(ModuleID.RoleDecompose);
                break;
            case "Map_Bg_shenmishangdian":
                GameUIMgr.Instance.OpenModule(ModuleID.MysteryShop);
                break;
            case "Map_Bg_wuqishiyanshi":
                GameUIMgr.Instance.OpenModule(ModuleID.Equipment);
                break;
            case "Map_Bg_jingjichang":
                if (FunctionUnlock.IsUnlock(FunctionType.Arena))
                    ArenaDataModel.Instance.ReqArenaData();
                break;
            case "Map_Bg_tansuodidai":
                if (FunctionUnlock.IsUnlock(FunctionType.Explore))
                {
                    GameUIMgr.Instance.OpenModule(ModuleID.Explore, false);
                }
                break;
            case "Map_Bg_shuijingshu":
                if (FunctionUnlock.IsUnlock(FunctionType.HeroCall))
                    GameUIMgr.Instance.OpenModule(ModuleID.HeroCall);
                break;
            case "Map_Bg_yuanzheng":
                if (FunctionUnlock.IsUnlock(FunctionType.Expedition))
                    GameUIMgr.Instance.OpenModule(ModuleID.Expedition);
                break;
            case "Map_Bg_fukongdao":
                PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001155));
                break;
        }
    }

    public void SetBuildName()
    {
        _yingxiongjiuguanTitle01.GetComponent<TextMesh>().text = LanguageMgr.GetLanguage(5002508);
        _yingxiongjiuguanTitle02.GetComponent<TextMesh>().text = LanguageMgr.GetLanguage(5002508);

        _shuijingshuTitle01.GetComponent<TextMesh>().text = LanguageMgr.GetLanguage(5003301);
        _shuijingshuTitle02.GetComponent<TextMesh>().text = LanguageMgr.GetLanguage(5003301);

        _tansuodidaiTitle01.GetComponent<TextMesh>().text = LanguageMgr.GetLanguage(5002210);
        _tansuodidaiTitle02.GetComponent<TextMesh>().text = LanguageMgr.GetLanguage(5002210);

        _mofajitanTitle01.GetComponent<TextMesh>().text = LanguageMgr.GetLanguage(5001407);
        _mofajitanTitle02.GetComponent<TextMesh>().text = LanguageMgr.GetLanguage(5001407);

        _shenmishangdianTitle01.GetComponent<TextMesh>().text = LanguageMgr.GetLanguage(5002101);
        _shenmishangdianTitle02.GetComponent<TextMesh>().text = LanguageMgr.GetLanguage(5002101);

        _wuqishiyanshiTitle01.GetComponent<TextMesh>().text = LanguageMgr.GetLanguage(5003025);
        _wuqishiyanshiTitle02.GetComponent<TextMesh>().text = LanguageMgr.GetLanguage(5003025);

        _yuanzhengTitle01.GetComponent<TextMesh>().text = LanguageMgr.GetLanguage(5003401);
        _yuanzhengTitle02.GetComponent<TextMesh>().text = LanguageMgr.GetLanguage(5003401);

        _jingjichangTitle01.GetComponent<TextMesh>().text = LanguageMgr.GetLanguage(5001701);
        _jingjichangTitle02.GetComponent<TextMesh>().text = LanguageMgr.GetLanguage(5001701);

        _hangupTitle01.GetComponent<TextMesh>().text = LanguageMgr.GetLanguage(5002901);
        _hangupTitle02.GetComponent<TextMesh>().text = LanguageMgr.GetLanguage(5002901);

        _caihongqiaoTitle01.GetComponent<TextMesh>().text = LanguageMgr.GetLanguage(5002601);
        _caihongqiaoTitle02.GetComponent<TextMesh>().text = LanguageMgr.GetLanguage(5002601);

        _taikongdiantiTitle01.GetComponent<TextMesh>().text = LanguageMgr.GetLanguage(5001901);
        _taikongdiantiTitle02.GetComponent<TextMesh>().text = LanguageMgr.GetLanguage(5001901);

        _fukongdaoTitle01.GetComponent<TextMesh>().text = LanguageMgr.GetLanguage(5003501);
        _fukongdaoTitle02.GetComponent<TextMesh>().text = LanguageMgr.GetLanguage(5003501);
    }
    
    public void Update()
    {
        if (!_blEnable)
            return;
        //if ((Time.deltaTime * 2 - (Time.realtimeSinceStartup - _enableTime)) > 0.0f)
        //    return;
        if (HeroDataModel.Instance.mHeroPlayerId == 0)
            return;
        _blTmpPressed = Input.GetMouseButton(0); 
        _tmpMousePos = Input.mousePosition;

        if (_blTmpPressed != _blPressed)
        {
            if (_blTmpPressed)
            {
                _blClick = true;
                //Debuger.LogWarning("MouseDown!!!");
            }
            else
            {
                //Debuger.Log("MouseUp ~~~~");
                if (_blClick)
                {
                    //Debuger.LogWarning("MouseClick...");
                    Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
                    if (hit.transform != null)
                        DoMapFunction(hit.transform.name);
                }
                _blClick = false;
            }
        }
        else if (_blClick && CheckMoved(_oldMousePos, _tmpMousePos))
        {
            _blClick = false;
        }
        else if (_blTmpPressed && !_blClick)
        {
            //Debuger.LogError("MouseDrag@@@@@" + ", delta:" + (_tmpMousePos - _oldMousePos));
            MoveCamera(_tmpMousePos - _oldMousePos);
            //DispatchInputEvent(InputEventType.MouseDrag, _tmpMousePos - _oldMousePos);
        }
        _blPressed = _blTmpPressed;
        _oldMousePos = _tmpMousePos;
    }

    private float _flTmpTopX = 0f;
    private float _flTmpMidX = 0f;
    private float _flTmpBottomX = 0f;

    private float _leftMax = 4.5f; //3.7f
    private float _rightMax = -3f; //-2.5f;
    private void MoveCamera(Vector2 delta)
    {
        if (delta.x == 0.0f)
        {
            //Debuger.LogWarning("no move!");
            return;
        }        
        _flTmpTopX += delta.x * 0.03f;
        _flTmpMidX += delta.x * 0.02f;
        _flTmpBottomX += delta.x * 0.015f;
        if (delta.x < 0.01f)
        {
            if (_flTmpBottomX <= _rightMax)
            {
                _flTmpTopX = _curTopX;
                _flTmpMidX = _curMiddleX;
                _flTmpBottomX = _curBottomX;
                return;
            }
        }
        else
        {
            if (_flTmpBottomX >= _leftMax)
            {
                _flTmpTopX = _curTopX;
                _flTmpMidX = _curMiddleX;
                _flTmpBottomX = _curBottomX;
                return;
            }
        }
        _curTopX = _flTmpTopX;
        _curMiddleX = _flTmpMidX;
        _curBottomX = _flTmpBottomX;
        _topTransform.localPosition = Vector3.Lerp(_topTransform.localPosition, new Vector3(_curTopX, 0f, 0f), 10f);
        _middleTransform.localPosition = Vector3.Lerp(_middleTransform.localPosition, new Vector3(_curMiddleX, 0f, 0f), 10f);
        _bottonTransfrom.localPosition = Vector3.Lerp(_bottonTransfrom.localPosition, new Vector3(_curBottomX, 0f, 0f), 10f);
    }

    public void SetGuideCameraType(int type)
    {
        if(type == 1)
            MoveCamera(new Vector2(120f, 0f));
        else if(type == 2)
            MoveCamera(new Vector2(-120f, 0f));
    }

    private bool CheckMoved(Vector2 p1, Vector2 p2)
    {
        return Mathf.Abs(p1.x - p2.x) > Drag_Gap || Mathf.Abs(p1.y - p2.y) > Drag_Gap;
    }

    private bool _blEnable;
    //private float _enableTime = 0f;
    public bool Enable
    {
        get { return _blEnable; }
        set
        {
            if (value)
            {
                if (!GameUIMgr.Instance.CanEnableMainMap())
                    return;
            }
            _blEnable = value;
            if (_blEnable)
            {
                //_enableTime = Time.realtimeSinceStartup;
                //Debuger.Log("time:" + _enableTime);
            }
            else
            {
                _blTmpPressed = false;
                _blPressed = false;
            }
        }
    }

    public void Dispose()
    {
        _blEnable = false;
        UnRegisteGuideMaskObject();
        _topTransform = null;
        _middleTransform = null;
        _bottonTransfrom = null;
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(HeroEvent.UpGrade, NameObject);
    }
}