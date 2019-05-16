using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public enum Dis
{
    None = 0,
    disCamp = 1,
    disType = 2,
    disBack = 3,
}
public class HeroCallModule : ModuleBase
{
    private Button _btnBack;
    private HeroCallView _heroCallView;
    private HeroReplaceView _heroReplaceView;
    private UIBaseView _uiShowView;

    private GameObject _summon;
    private GameObject _replacement;
    private Toggle[] _toggles;
    private Button _tips01;
    private Button _tips02;
    private ItemResGroup _resGroup;

    private bool _blShowAlertAgain;
    private bool _blAlertValue;

    private GameObject _imgBack01;
    private GameObject _imgBack02;

    private GameObject _colider;
    private Button _objColider01;
    private Button _objColider02;
    private Button _objColider03;
    private Button _objColider04;

    private Dis _curType = Dis.None;
    private Dis _btnType;

    public HeroCallModule() : base(ModuleID.HeroCall, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_HeroCall;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _btnBack = Find<Button>("Btn_Back");

        _summon = Find("Summon");
        _replacement = Find("Replacement");
        _btnBack.onClick.Add(OnClose);

        _resGroup = new ItemResGroup();
        _resGroup.SetDisplayObject(Find("uiResGroup"));
        _resGroup.Show(SpecialItemID.FateKey, SpecialItemID.EnergyStone);

        _imgBack01 = Find("ImgBack");
        _imgBack02 = Find("ImgBack01");

        _heroCallView = new HeroCallView();
        _heroCallView.SetDisplayObject(_summon);

        _heroReplaceView = new HeroReplaceView();
        _heroReplaceView.SetDisplayObject(_replacement);

        _tips01 = Find<Button>("ProbabHelp");
        _tips02 = Find<Button>("DrawHelp");

        _tips01.onClick.Add(OnProbabHelp);
        _tips02.onClick.Add(OnDrawHelp);

        _toggles = new Toggle[3];
        for (int i = 0; i < 3; i++)
            _toggles[i] = Find<Toggle>("ToggleGroup/Tog" + (i + 1));
        foreach (Toggle tog in _toggles)
            tog.onValueChanged.Add((bool blSelect) => { if (blSelect) OnTaskTypeChange(tog); });

        _colider = Find("Colider");
        _objColider01 = Find<Button>("Colider/BtnBackColider01");
        _objColider02 = Find<Button>("Colider/BtnBackColider02");
        _objColider03 = Find<Button>("Colider/BtnBackColider03");
        _objColider04 = Find<Button>("Colider/BtnBackColider04");

        _objColider01.onClick.Add(() => { RefreshData(Dis.disBack); });
        _objColider02.onClick.Add(() => { RefreshData(Dis.None); });
        _objColider03.onClick.Add(() => { RefreshData(Dis.disCamp); });
        _objColider04.onClick.Add(() => { RefreshData(Dis.disType); });
    }

    private void OnTaskTypeChange(Toggle tog)
    {
        if (_uiShowView != null)
            _uiShowView.Hide();
        switch (tog.name)
        {
            case "Tog1":
                _imgBack01.SetActive(true);
                _imgBack02.SetActive(false);
                _curType = Dis.None;
                _uiShowView = _heroCallView;
                break;
            case "Tog2":
                _imgBack01.SetActive(false);
                _imgBack02.SetActive(true);
                _curType = Dis.disCamp;
                _uiShowView = _heroReplaceView;
                break;
            case "Tog3":
                _imgBack01.SetActive(false);
                _imgBack02.SetActive(true);
                _curType = Dis.disType;
                _uiShowView = _heroReplaceView;
                break;
        }
        _uiShowView.Show(_curType);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(HeroCallEvent.heroreplacesuccess, OnReplaceSuccess);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(HeroCallEvent.herosavesuccess, OnSaveSuccess);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(HeroCallEvent.herocancel,OnCancel);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(HeroCallEvent.heroreplacesuccess, OnReplaceSuccess);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(HeroCallEvent.herosavesuccess, OnSaveSuccess);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _blShowAlertAgain = false;
        OnTaskTypeChange(_toggles[0]);
    }

    /// <summary>
    /// 是否进行下一步
    /// </summary>
    private void RefreshData(Dis type)
    {
        if (_curType == type) return;
        _btnType = type;
        if (!_blShowAlertAgain)
            ConfirmTipsMgr.Instance.ShowConfirmTips(LanguageMgr.GetLanguage(4000129), OnAlertBackCamp, true);
        else
        {
            if (_blAlertValue)
            {
                if (_btnType == Dis.disBack)
                    OnClose();
                else
                    _toggles[(int)_btnType].isOn = true;
                _colider.SetActive(false);
            }
        }
    }

    private void OnAlertBackCamp(bool value, bool blShowAgain)
    {
        _blShowAlertAgain = blShowAgain;
        _blAlertValue = value;
        if (_blAlertValue)
        {
            if (_btnType == Dis.disBack)
                OnClose();
            else
                _toggles[(int)_btnType].isOn = true;
            _colider.SetActive(false);
        }
        else
        {
            _blShowAlertAgain = false;
        }
    }

    /// <summary>
    /// 置换成功
    /// </summary>
    private void OnReplaceSuccess()
    {
        _colider.SetActive(true);
    }

    /// <summary>
    /// 置换取消
    /// </summary>
    private void OnCancel()
    {
        _colider.SetActive(false);
    }

    /// <summary>
    /// 保存成功
    /// </summary>
    private void OnSaveSuccess()
    {
        _colider.SetActive(false);
    }

    private void OnProbabHelp()
    {
        HelpTipsMgr.Instance.ShowTIps(HelpType.DrawLifeProHelp);
    }

    private void OnDrawHelp()
    {
        HelpTipsMgr.Instance.ShowTIps(HelpType.DrawLifeHelp);
    }

    public override void Hide()
    {
        _toggles[0].isOn = true;
        if (_heroCallView != null)
            _heroCallView.Hide();
        if (_heroReplaceView != null)
            _heroReplaceView.Hide();
        if (_uiShowView != null)
            _uiShowView = null;
        base.Hide();
    }

    public override void Dispose()
    {
        if (_heroCallView != null)
        {
            _heroCallView.Dispose();
            _heroCallView = null;
        }
        if (_heroReplaceView != null)
        {
            _heroReplaceView.Dispose();
            _heroReplaceView = null;
        }
        if (_uiShowView != null)
            _uiShowView = null;
        base.Dispose();
    }
}
