using UnityEngine;
using UnityEngine.UI;
public class LoadingMgr : Singleton<LoadingMgr>
{
    private Text _tipsText;
    private GameObject _uiLoadObject;
    private Transform _img;
    private bool _blShow = true;
    private Image _loadingImg01;
    private Image _loadingImg02;
    private int _num;


    private GameObject _normalRoot;
    private GameObject _rechargeRoot;


    private void CreateUILoad()
    {
        _uiLoadObject = GameResMgr.Instance.LoadUIObjectSync(SingletonResName.UILoading);

        _normalRoot = _uiLoadObject.transform.Find("NormalRoot").gameObject;

        _tipsText = _uiLoadObject.transform.Find("NormalRoot/Label").GetComponent<Text>();
        _img = _uiLoadObject.transform.Find("NormalRoot/Image").GetComponent<Transform>();
        //_img.DOLocalRotate(new Vector3(0, 0, -360), 3, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
        DGHelper.DoRotateCircle(_img);

        _loadingImg01 = _uiLoadObject.transform.Find("NormalRoot/Loading01").GetComponent<Image>();
        _uiLoadObject.transform.SetParent(GameUIMgr.Instance.mGuideRoot, false);

        _rechargeRoot = _uiLoadObject.transform.Find("RechargeMask").gameObject;
        Transform _maskAniationObj = _rechargeRoot.transform.Find("Image");
        //_maskAniationObj.DOLocalRotate(new Vector3(0, 0, -360), 3, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
        DGHelper.DoRotateCircle(_maskAniationObj);

        _rechargeRoot.SetActive(false);
    }
    public void ShowTips(string value)
    {
        if (_uiLoadObject == null)
            CreateUILoad();
        if (!_blShow)
        {
            _normalRoot.SetActive(true);
            _blShow = true;
            _rechargeRoot.SetActive(false);
            _uiLoadObject.SetActive(true);
            _uiLoadObject.transform.SetAsLastSibling();
        }
      
        _tipsText.text = value;
    }

    public void CloseLoading()
    {
        if (_uiLoadObject == null)
            return;
        _uiLoadObject.SetActive(false);
        _blShow = false;
    }

    public void ShowRechargeMask()
    {
        if (_uiLoadObject == null || _blShow) 
            return;
        if (!_uiLoadObject.activeInHierarchy)
        {
            _uiLoadObject.SetActive(true);
            _normalRoot.SetActive(false);
            _uiLoadObject.transform.SetAsLastSibling();
        }

        _rechargeRoot.SetActive(true);
        _blShowRechargeMask = true;
    }

    private bool _blShowRechargeMask;
    public void HideRechargeMask()
    {
        if (!_blShowRechargeMask || _blShow)
            return;
        if (_uiLoadObject == null)
            return;
        if (_uiLoadObject.activeInHierarchy)
            _uiLoadObject.SetActive(false);
        _rechargeRoot.SetActive(false);
        _blShowRechargeMask = false;
    }
}