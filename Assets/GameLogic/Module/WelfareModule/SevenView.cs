using Framework.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SevenView : UIBaseView
{
    private List<SevenItemView> _listSevenItemView;
    private List<SevenDataVO> _listSevenDataVO;
    private Text _name1;
    private Text _name2;
    private Text _sevenText;
    private Text _sevenTimeText;
    private Button _sevenBtn;
    private Outline _outline;

    private uint _timer = 0;
    private int _sevenTime = 0;

    private uint _sjTimer = 0;
    private int _sjSevenTime = 0;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _name1 = Find<Text>("Text1");
        _name2 = Find<Text>("Text2");
        _sevenText = Find<Text>("Seven/Text");
        _sevenTimeText = Find<Text>("Time");
        _sevenBtn = Find<Button>("Seven");
        _outline = Find<Outline>("Text1");


        _sevenBtn.onClick.Add(OnSeven);

        _listSevenItemView = new List<SevenItemView>();
        for (int i = 0; i < 7; i++)
        {
            GameObject obj = Find("RewardObj/Reward" + (i + 1));
            SevenItemView sevenItemView = new SevenItemView();
            sevenItemView.SetDisplayObject(obj);
            _listSevenItemView.Add(sevenItemView);
        }
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        WelfareDataModel.Instance.AddEvent<List<SevenDataVO>>(WelfareEvent.SevenData, OnSevenData);
        WelfareDataModel.Instance.AddEvent<List<ItemInfo>>(WelfareEvent.SevenAward, OnSevenAward);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        WelfareDataModel.Instance.RemoveEvent<List<SevenDataVO>>(WelfareEvent.SevenData, OnSevenData);
        WelfareDataModel.Instance.RemoveEvent<List<ItemInfo>>(WelfareEvent.SevenAward, OnSevenAward);
    }
    private void OnSevenData(List<SevenDataVO> listSevenVO)
    {
        _sjSevenTime = WelfareDataModel.Instance.mSevenTime;
        if (_sjTimer != 0)
            TimerHeap.DelTimer(_sjTimer);
        int interval = 1000;
        _sjTimer = TimerHeap.AddTimer(0, interval, OnAddSjTime);

        _listSevenDataVO = listSevenVO;
        OnSevenChang();
    }

    private void OnAddSjTime()
    {
        if (_sjSevenTime > 0)
            _sjSevenTime -= 1;
        else
            GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(WelfareEvent.SevenTime);
    }

    private void OnSevenAward(List<ItemInfo> listInfo)
    {
        GetItemTipMgr.Instance.ShowItemResult(listInfo);
        OnSevenChang();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        if (LocalDataMgr.CurLanguage == SystemLanguage.English)
            _outline.effectDistance = new Vector2(3f, -3f);
        else
            _outline.effectDistance = new Vector2(1f, -1f);
        _name1.text = LanguageMgr.GetLanguage(5007103);
        _name2.text = LanguageMgr.GetLanguage(5007116);
    }

    private void OnSevenChang()
    {
        for (int i = 0; i < _listSevenDataVO.Count; i++)
            _listSevenItemView[i].Show(_listSevenDataVO[i]);
        if (_listSevenDataVO[_listSevenDataVO[0].mCurHeaven - 1].mStatus == 0)
            _sevenText.text = LanguageMgr.GetLanguage(5001203);
        else
            _sevenText.text = LanguageMgr.GetLanguage(5007115);

        _sevenTime = _listSevenDataVO[0].SevenTime;
        if (_timer != 0)
            TimerHeap.DelTimer(_timer);
        int interval = 1000;
        _timer = TimerHeap.AddTimer(0, interval, OnAddTime);
    }

    private void OnAddTime()
    {
        if (_sevenTime > 0)
        {
            _sevenTime -= 1;
            _sevenTimeText.text = LanguageMgr.GetLanguage(5007104, TimeHelper.GetCountTime(_sevenTime));
        }
        
    }

    private void OnSeven()
    {
        if (_listSevenDataVO[_listSevenDataVO[0].mCurHeaven - 1].mStatus == 1)
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000059));
        else
            GameNetMgr.Instance.mGameServer.ReqSevenAward();
    }
}
