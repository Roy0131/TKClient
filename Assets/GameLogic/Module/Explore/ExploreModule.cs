using System.Collections.Generic;
using UnityEngine.UI;


public class ExploreModule : ModuleBase
{
    private Button _btnClose;
    private Button _btnHelp;
    private Text _textRemainTime;
    private uint _timerKey = 0;
    private int _remainSeconds;
    private Text _textDiamondCount;
    private Button _btnRefresh;
    private Text _buttonRereshCost;
    private Toggle[] _toggles;
    private bool _isStory;

    private ExploreView _exploreView;
    private int _costNum;
    private int _type;

    public ExploreModule() : base(ModuleID.Explore, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_Explore;
        _soundName = UIModuleSoundName.ExplpreSoundName;
        mBlNeedBackMask = true;
    }
    protected override void ParseComponent()
    {
        base.ParseComponent();
        _textRemainTime = Find<Text>("Content01/Image/TextRemainTime");
        _btnClose = Find<Button>("ButtonBack");
        ColliderHelper.SetButtonCollider(_btnClose.transform);
        _btnHelp = Find<Button>("Content01/ButtonHelp");
        _textDiamondCount = Find<Text>("Content01/uiResGroup/Item/countText");
        _btnRefresh = Find<Button>("Content01/ButtonRefresh");
        _buttonRereshCost = Find<Text>("Content01/ButtonRefresh/TextCost");

        _toggles = new Toggle[2];
        for (int i = 0; i < 2; i++)
            _toggles[i] = Find<Toggle>("ToggleGroup/Tog" + (i + 1));
        foreach (Toggle tog in _toggles)
            tog.onValueChanged.Add((bool blSelect) => { if (blSelect) OnItemTypeChange(tog); });

        _exploreView = new ExploreView();
        _exploreView.SetDisplayObject(Find("Content02"));

        _btnRefresh.onClick.Add(ExploreResresh);
        _btnClose.onClick.Add(OnClose);
        _btnHelp.onClick.Add(OpenHelp);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        HeroDataModel.Instance.AddEvent(HeroEvent.HeroInfoChange, SpeedDiamondCost);
        ExploreDataModel.Instance.AddEvent(ExploreEvent.ExploreData, OnExploreData);
        ExploreDataModel.Instance.AddEvent(ExploreEvent.ExploreLock, DiamondCost);
        ExploreDataModel.Instance.AddEvent<List<int>>(ExploreEvent.ExploreStart, OnExploreStart);
        ExploreDataModel.Instance.AddEvent(ExploreEvent.ExploreRefresh, OnRefreshExplore);
    }
    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        HeroDataModel.Instance.RemoveEvent(HeroEvent.HeroInfoChange, SpeedDiamondCost);
        ExploreDataModel.Instance.RemoveEvent(ExploreEvent.ExploreData, OnExploreData);
        ExploreDataModel.Instance.RemoveEvent(ExploreEvent.ExploreLock, DiamondCost);
        ExploreDataModel.Instance.RemoveEvent<List<int>>(ExploreEvent.ExploreStart, OnExploreStart);
        ExploreDataModel.Instance.RemoveEvent(ExploreEvent.ExploreRefresh, OnRefreshExplore);
    }

    private void OnExploreStart(List<int> ListId)
    {
        DiamondCost();
    }

    private void OnRefreshExplore()
    {
        OnItemTypeChange(_toggles[_type]);
        DiamondCost();
    }

    private void OnExploreData()
    {
        int index;
        if (ExploreDataModel.Instance._isStoryType)
            index = 1;
        else
            index = 0;
        _toggles[index].isOn = true;
        OnItemTypeChange(_toggles[index]);
        DiamondCost();
        _remainSeconds = ExploreDataModel.Instance.TaskTime;
        if (_timerKey != 0)
            TimerHeap.DelTimer(_timerKey);
        _timerKey = TimerHeap.AddTimer(0, 1000, OnTimeCD);
    }

    private void OnTimeCD()
    {
        if (_remainSeconds < 0)
        {
            GameNetMgr.Instance.mGameServer.ReqExploreData();
        }
        else
        {
            _remainSeconds--;
            _textRemainTime.text = LanguageMgr.GetLanguage(5002211, ExploreDataModel.Instance.exploreData.Count,
                GameConfigMgr.Instance.GetVipConfig(HeroDataModel.Instance.mHeroInfoData.mVipLevel).SearchTaskCount,
                TimeHelper.GetCountTime(_remainSeconds));
        }
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        bool isStory = bool.Parse(args[0].ToString());
        ExploreDataModel.Instance.ReqExploreData(isStory);
        SpeedDiamondCost();
    }

    private void OnItemTypeChange(Toggle tog)
    {
        switch (tog.name)
        {
            case "Tog1":
                _type = 0;
                _isStory = false;
                _textRemainTime.gameObject.SetActive(true);
                _btnRefresh.gameObject.SetActive(true);
                break;
            case "Tog2":
                _type = 1;
                _isStory = true;
                _textRemainTime.gameObject.SetActive(false);
                _btnRefresh.gameObject.SetActive(false);
                break;
        }
        ExploreDataModel.Instance.OnIsStory(_isStory);
        _exploreView.Show();
    }

    private bool _blShowAlertAgain;
    private bool _blAlertValue;
    private void OnAlertBack(bool value, bool blShowAgain)
    {
        _blShowAlertAgain = blShowAgain;
        _blAlertValue = value;
        if (_blAlertValue)
        {
            TDPostDataMgr.Instance.DoCostDiamond(TDCostDiamondType.BuyExploreRefresh, 1, _costNum);
            ExploreDataModel.Instance.ReqExploreRefresh();
        }
        else
            _blShowAlertAgain = false;

    }

    private void SpeedDiamondCost()
    {
        _textDiamondCount.text = UnitChange.GetUnitNum(HeroDataModel.Instance.mHeroInfoData.mDiamond);
    }

    private void OpenHelp()
    {
        HelpTipsMgr.Instance.ShowTIps(HelpType.ExploreHelp);
    }

    private void ExploreResresh()
    {
        SoundMgr.Instance.PlayEffectSound("UI_btn_refresh");
        RefreshData();
    }

    private void RefreshData()
    {
        DiamondCost();
        if (_costNum > 0)
        {
            if (HeroDataModel.Instance.mHeroInfoData.mDiamond >= _costNum)
            {
                if (!_blShowAlertAgain)
                    ConfirmTipsMgr.Instance.ShowConfirmTips(string.Format(LanguageMgr.GetLanguage(6001168), _costNum), OnAlertBack, true);
                else
                {
                    if (_blAlertValue)
                    {
                        TDPostDataMgr.Instance.DoCostDiamond(TDCostDiamondType.BuyExploreRefresh, 1, _costNum);
                        ExploreDataModel.Instance.ReqExploreRefresh();
                    }
                }
            }
            else
                PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000022));
        }
        else
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000131));
        }
    }

    private void DiamondCost()
    {
        _costNum = 0;
        for (int i = 0; i < ExploreDataModel.Instance.exploreData.Count; i++)
        {
            if (!ExploreDataModel.Instance.exploreData[i].mIsLock)
                _costNum += 10;
        }
        _buttonRereshCost.text = _costNum.ToString();
    }

    protected override void OnClose()
    {
        base.OnClose();
    }

    public override void Hide()
    {
        if (_exploreView != null)
            _exploreView.Hide();
        base.Hide();
        StopAllEffectSound();
    }

    public override void Dispose()
    {
        if (_exploreView != null)
        {
            _exploreView.Dispose();
            _exploreView = null;
        }
        base.Dispose();
    }
}
