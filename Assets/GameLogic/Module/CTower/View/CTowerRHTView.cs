using Framework.UI;
using System.Collections.Generic;
using UnityEngine.UI;

public class CTowerRHTView : UIBaseView
{

    private Button _btnHelp;//帮助按钮
    private Button _btnRank;//排行榜按钮
    private Button _btnBuyTicket;//购买门票按钮

    private Text _textTicket;
    private Text _textTime;
    private int _timeRemain;
    //private int _timeNeed;
    private int _ticketNum=0 ;

    private CTowerBuyTicketView _cTowerBuyTicketView;
    private uint _timerKey = 0;
    //private bool _isOpen = false;

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        RefreshTicket();
    }
    protected override void ParseComponent()
    {
        base.ParseComponent();
        _btnHelp = Find<Button>("BtnHelp");//帮助按钮

        _btnRank = Find<Button>("BtnRank");//排行榜按钮
        _btnBuyTicket = Find<Button>("TotalGold/BtnBuy");//购买门票按钮

        _textTicket = Find<Text>("TotalGold/TotalGoldLabel");
        _textTime = Find<Text>("TotalGold/TextTime");
        #region
        _cTowerBuyTicketView = new CTowerBuyTicketView();
        _cTowerBuyTicketView.SetDisplayObject(Find("BuyTicketWindow"));
        _cTowerBuyTicketView.SortingOrder = UILayerSort.WindowSortBeginner + 2;
        #endregion
        _btnHelp.onClick.Add(OnOpenHelp);//打开帮助面板
        _btnRank.onClick.Add(OnOpenRank);//打开排行榜
        _btnBuyTicket.onClick.Add(OnBuyTicket);//打开购买门票面板

        RefreshTicket();
    }
    protected override void AddEvent()
    {
        base.AddEvent();
        CTowerDataModel.Instance.AddEvent(CTowerEvent.RefreshTowerData, RefreshData);
        BagDataModel.Instance.AddEvent<List<int>>(BagEvent.BagItemRefresh, ValueChange);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(CTowerEvent.ClearTowerTimeHeap, ClearRemainTimer);
    }
    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        CTowerDataModel.Instance.RemoveEvent(CTowerEvent.RefreshTowerData, RefreshData);
        BagDataModel.Instance.RemoveEvent<List<int>>(BagEvent.BagItemRefresh, ValueChange);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(CTowerEvent.ClearTowerTimeHeap, ClearRemainTimer);
    }
    private void ValueChange(List<int> value)
    {
        if (value.Contains(SpecialItemID.CTowerTicket))
        {
            RefreshTicket();
            //CTowerDataModel.Instance.ReqTowerData();
            //InitTimeData();
        }
        else
        {
            LogHelper.Log("爬塔门票没有变化");
        }
    }

    private void RefreshData()
    {
        //RefreshTicket();
        _textTicket.text = CTowerDataModel.Instance.towerKeys.ToString();
        _ticketNum = CTowerDataModel.Instance.towerKeys;
        _timeRemain = CTowerDataModel.Instance.mRemainSeconds;//剩余的秒数
        InitTimeData();
    }
    private void InitTimeData()
    {
        if (_ticketNum < 10)
        {
            //CTowerDataModel.Instance.ReqTowerData();
            Find("TotalGold/TextTime").SetActive(true);
            TImeHeap();
        }
        else
        {
            Find("TotalGold/TextTime").SetActive(false);
            ClearRemainTimer();
        }
    }
    /// <summary>
    /// 倒计时刷新
    /// </summary>
    private void TImeHeap()
    {
        LogHelper.Log("添加定时器----------------------------");
        _timerKey = TimerHeap.AddTimer(0, 1000, OnTimeCD);
    }
    private void OnTimeCD()
    {
        _timeRemain--;
        if (_timeRemain < 0)
        {
            ClearRemainTimer();
            CTowerDataModel.Instance.ReqTowerData();
            //RefreshData();
            LogHelper.Log("时间小于0，移除定时器");
        }
        //Debuger.Log(_timeRemain);
        _textTime.text = TimeHelper.GetCountTime(_timeRemain);
    }

    private void ClearRemainTimer()
    {
        //Debuger.Log("移除定时器");
        if (_timerKey != 0)
            TimerHeap.DelTimer(_timerKey);
        _timerKey = 0;
    }
    /// <summary>
    /// 打开帮助界面
    /// </summary>
    private void OnOpenHelp()
    {
        HelpTipsMgr.Instance.ShowTIps(HelpType.TowerHelp);
    }
    /// <summary>
    /// 打开排行榜界面
    /// </summary>
    private void OnOpenRank()
    {
        GameUIMgr.Instance.OpenModule(ModuleID.TowerRank);
       
    }
    /// <summary>
    /// 打开购买门票面板
    /// </summary>
    private void OnBuyTicket()
    {
        _cTowerBuyTicketView.Show();
    }
    /// <summary>
    /// 刷新门票数量
    /// </summary>
    private void RefreshTicket()
    {
        _textTicket.text = BagDataModel.Instance.GetItemCountById(SpecialItemID.CTowerTicket).ToString();
        _ticketNum = BagDataModel.Instance.GetItemCountById(SpecialItemID.CTowerTicket);
        if (_ticketNum > 9) _textTime.gameObject.SetActive(false);
    }
    public override void Hide()
    {
        base.Hide();
        ClearRemainTimer();
    }
    public override void Dispose()
    {
        base.Dispose();
        if (_cTowerBuyTicketView != null)
        {
            _cTowerBuyTicketView.Dispose();
            _cTowerBuyTicketView = null;
        }
    }
}
