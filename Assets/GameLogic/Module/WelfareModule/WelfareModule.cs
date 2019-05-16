using Framework.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WelfareType
{
    MonthlyCard = 1,//月卡
    StarMonthlyCard = 2,//大月卡
    FirstCharge = 3,//首充
    SevenDays = 4,//七天乐
    LimitTime = 5,//限时活动
}

public class WelfareConst
{
    public const int Activity = 1;//活动
    public const int Welfare = 2;//限时活动
}

public class WelfareModule : ModuleBase
{
    private Text _tips;
    private Transform _root01;
    private Transform _root02;
    private Button _disBtn;
    private List<Toggle> _listTog;

    private MonthlyCardView _monthlyCardView;
    private FirstChargeView _firstChargeView;
    private SevenView _sevenView;
    private LimitedView _limitTimeView;
    private UIBaseView _uiShowView;

    private RectTransform _rectMove;

    private List<LimitedDataVO> _listLimitedDataVO;
    private int _curWelfareType;

    private bool isActivity;
    private bool isWelfare;
    private HomeModule _home;

    public WelfareModule()
       : base(ModuleID.Welfare, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_Welfare;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _home = new HomeModule();
        _tips = Find<Text>("Tips");
        _root01 = Find<Transform>("Left");
        _root02 = Find<Transform>("Right");
        _disBtn = Find<Button>("Btn_Back");
        _rectMove = Find<RectTransform>("Left/Move/ToggleGroup");

        _monthlyCardView = new MonthlyCardView();
        _monthlyCardView.SetDisplayObject(Find("Right/MonthlyCardObj"));

        _firstChargeView = new FirstChargeView();
        _firstChargeView.SetDisplayObject(Find("Right/FirstChargeObj"));

        _sevenView = new SevenView();
        _sevenView.SetDisplayObject(Find("Right/SevenDaysObj"));

        _limitTimeView = new LimitedView();
        _limitTimeView.SetDisplayObject(Find("Right/LimitedObj"));

        _disBtn.onClick.Add(OnClose);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(WelfareEvent.SevenTime, OnSevenTime);
        RechargeDataModel.Instance.AddEvent<List<ItemInfo>>(RechargeEvent.FirstRecharge, OnFirstRecharge);
        RechargeDataModel.Instance.AddEvent<List<MonthCardData>>(RechargeEvent.RechargeData, OnRechargeData);
        WelfareDataModel.Instance.AddEvent<List<LimitedDataVO>>(WelfareEvent.LimitedData, OnLimitedData);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<int>(WelfareEvent.ActivityEnd, OnActivityEnd);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(WelfareEvent.SevenTime, OnSevenTime);
        RechargeDataModel.Instance.RemoveEvent<List<ItemInfo>>(RechargeEvent.FirstRecharge, OnFirstRecharge);
        RechargeDataModel.Instance.RemoveEvent<List<MonthCardData>>(RechargeEvent.RechargeData, OnRechargeData);
        WelfareDataModel.Instance.RemoveEvent<List<LimitedDataVO>>(WelfareEvent.LimitedData, OnLimitedData);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<int>(WelfareEvent.ActivityEnd, OnActivityEnd);
    }

    private void OnActivityEnd(int activeityId)
    {
        OnTogDestroy();
        OnInitWelfare();
    }

    private void OnLimitedData(List<LimitedDataVO> listLimitedDataVO)
    {
        if (listLimitedDataVO.Count == 0)
        {
            _tips.gameObject.SetActive(true);
            return;
        }
        _tips.gameObject.SetActive(false);
        _listLimitedDataVO = listLimitedDataVO;
        _listTog = new List<Toggle>();
        for (int i = 0; i < listLimitedDataVO.Count; i++)
        {
            GameObject obj = GameObject.Instantiate(Find("Left/Move/ToggleGroup/Tog"));
            obj.transform.SetParent(_rectMove, false);
            obj.SetActive(true);
            Toggle tog = obj.transform.GetComponent<Toggle>();
            MainActiveConfig cfg = GameConfigMgr.Instance.GetMainActiveConfig(listLimitedDataVO[i].mActivityId);
            tog.transform.Find("Text").gameObject.GetComponent<Text>().text = LanguageMgr.GetLanguage(cfg.Title);
            int j = i;
            tog.onValueChanged.Add((bool blSelect) => { if (blSelect) OnWelfareType(j + 5); });
            _listTog.Add(tog);
        }
        _listTog[0].isOn = true;
    }

    private void OnWelfareType(int id)
    {
        if (_uiShowView != null)
            _uiShowView.Hide();
        switch (id)
        {
            case 1:
                _curWelfareType = (int)WelfareType.MonthlyCard;
                _uiShowView = _monthlyCardView;
                break;
            case 2:
                _curWelfareType = (int)WelfareType.StarMonthlyCard;
                _uiShowView = _monthlyCardView;
                break;
            case 3:
                _curWelfareType = (int)WelfareType.FirstCharge;
                _uiShowView = _firstChargeView;
                break;
            case 4:
                _curWelfareType = (int)WelfareType.SevenDays;
                _uiShowView = _sevenView;
                break;
            default:
                _curWelfareType = (int)WelfareType.LimitTime;
                _uiShowView = _limitTimeView;
                break;
        }
        if (_curWelfareType == (int)WelfareType.LimitTime)
            _uiShowView.Show(_listLimitedDataVO[id - 5]);
        else
            _uiShowView.Show(_curWelfareType);
        if (isActivity)
        {
            WelfareDataModel.Instance.ReqSevenData();
            RechargeDataModel.Instance.ReqRechargeData();
        }
    }

    private void OnRechargeData(List<MonthCardData> listCard)
    {
        if (RechargeDataModel.Instance.mFirstChargeState == 2)
        {
            RedPointDataModel.Instance.SetRedPointDataState(RedPointEnum.FirstCharge, false);
            _listTog[(int)WelfareType.FirstCharge - 1].gameObject.SetActive(false);
            if (_curWelfareType == (int)WelfareType.FirstCharge)
                _listTog[0].isOn = true;
        }
        else
        {
            RedPointDataModel.Instance.SetRedPointDataState(RedPointEnum.FirstCharge, true);
        }
    }

    private void OnFirstRecharge(List<ItemInfo> listInfo)
    {
        RedPointDataModel.Instance.SetRedPointDataState(RedPointEnum.FirstCharge, false);
        _listTog[(int)WelfareType.FirstCharge - 1].gameObject.SetActive(false);
        if (_curWelfareType == (int)WelfareType.FirstCharge)
            _listTog[0].isOn = true;
    }

    private void OnSevenTime()
    {
        _listTog[(int)WelfareType.SevenDays - 1].gameObject.SetActive(false);
        if (_curWelfareType == (int)WelfareType.SevenDays)
            _listTog[0].isOn = true;
    }

    private bool _blLinkFirstRecharge = false;
    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _blLinkFirstRecharge = false;
        _tips.text = LanguageMgr.GetLanguage(5007524);
        if (args != null && args.Length > 0)
        {
            if (int.Parse(args[0].ToString()) == WelfareConst.Activity)
            {
                isActivity = true;
                isWelfare = true;
                if (args.Length == 2)
                    _blLinkFirstRecharge = true;
            }
            else
            {
                isActivity = false;
                isWelfare = false;
            }
        }
        OnInitWelfare();
    }

    private void OnInitWelfare()
    {
        if (isWelfare)
        {
            _tips.gameObject.SetActive(false);
            _listTog = new List<Toggle>();
            for (int i = 0; i < 4; i++)
            {
                GameObject obj = GameObject.Instantiate(Find("Left/Move/ToggleGroup/Tog"));
                obj.transform.SetParent(_rectMove, false);
                obj.SetActive(true);
                Toggle tog = obj.transform.GetComponent<Toggle>();
                tog.transform.Find("Text").gameObject.GetComponent<Text>().text = LanguageMgr.GetLanguage(GameConst.GetLimitedLanguageId(i + 100));
                int j = i;
                tog.onValueChanged.Add((bool blSelect) => { if (blSelect) OnWelfareType(j + 1); });
                _listTog.Add(tog);
                if (i == (int)WelfareType.SevenDays - 1)
                    RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.Seven, tog.transform.Find("RedPoint").gameObject);
                if (i == (int)WelfareType.FirstCharge - 1)
                    RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.FirstCharge, tog.transform.Find("RedPoint").gameObject);
            }

            if (_blLinkFirstRecharge)
                _listTog[2].isOn = true;
            else
                _listTog[0].isOn = true;
        }
        else
        {
            _home.RefreshWelfareRedPoint(false);
            WelfareDataModel.Instance.ReqLimitedData();
        }
    }

    public override void Hide()
    {
        _root01.transform.localPosition = new Vector3(371, 0, 0);
        _rectMove.anchoredPosition = new Vector2(0f, 0f);
        if (_uiShowView != null)
            _uiShowView.Hide();
        OnTogDestroy();
        base.Hide();
    }

    private void OnTogDestroy()
    {
        if (_listTog != null)
        {
            for (int i = 0; i < _listTog.Count; i++)
            {
                if (isActivity)
                {
                    if (i == (int)WelfareType.SevenDays - 1)
                        RedPointTipsMgr.Instance.RedPointUnBindObject(RedPointEnum.Seven, _listTog[i].transform.Find("RedPoint").gameObject);
                    if (i == (int)WelfareType.FirstCharge - 1)
                        RedPointTipsMgr.Instance.RedPointUnBindObject(RedPointEnum.FirstCharge, _listTog[i].transform.Find("RedPoint").gameObject);
                }
                GameObject.Destroy(_listTog[i].gameObject);
            }
            _listTog.Clear();
        }
    }

    public override void Dispose()
    {
        if (_monthlyCardView != null)
        {
            _monthlyCardView.Dispose();
            _monthlyCardView = null;
        }
        if (_firstChargeView != null)
        {
            _firstChargeView.Dispose();
            _firstChargeView = null;
        }
        if (_sevenView != null)
        {
            _sevenView.Dispose();
            _sevenView = null;
        }
        if (_limitTimeView != null)
        {
            _limitTimeView.Dispose();
            _limitTimeView = null;
        }
        _uiShowView = null;
        base.Dispose();
    }

    protected override void OnShowAnimator()
    {
        base.OnShowAnimator();
        DelayCall(0.25f, () => ObjectHelper.AnimationMove(_root01, new Vector3(371, 0, 0), new Vector3(0, 0, 0), 0.3f));
        //ObjectHelper.AnimationMoveBack(_root02, ObjectHelper.direction.left);

    }
}
