using Framework.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class LimitedView : UIBaseView
{
    private Text _title;
    private Text _detail;
    private Text _time;

    private GameObject _offerItemObj;
    private GameObject _taskItemObj;
    private GameObject _rebateItemObj;
    private GameObject _exchangeItemObj;
    private GameObject _festivalObj;

    private Image _bjImg;
    private Image _tbImg;

    private RectTransform _parent;

    private LimitedDataVO _limitedDataVO;

    private LimitedOfferView _limitedOfferView;
    private List<UIBaseView> _listUiItemView;

    private uint _timer = 0;
    private int _limitedTime = 0;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _title = Find<Text>("Title/TitleText");
        _detail = Find<Text>("Title/Detail");
        _time = Find<Text>("Time");

        _parent = Find<RectTransform>("Scroll/Group");

        _offerItemObj = Find("RechargeItemObj");//充值Item
        _taskItemObj = Find("TaskItemObj");//任务Item
        _rebateItemObj = Find("RebateItemObj");//累计Item
        _exchangeItemObj = Find("ExchangeItemObj");//兑换Item
        _festivalObj = Find("FestivalObj");

        _bjImg = Find<Image>("Title/BJImg");
        _tbImg = Find<Image>("Title/TBImg");
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        WelfareDataModel.Instance.AddEvent<int>(WelfareEvent.ActivityDataNotify, OnDataReward);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        WelfareDataModel.Instance.RemoveEvent<int>(WelfareEvent.ActivityDataNotify, OnDataReward);
    }

    private void OnDataReward(int subId)
    {
        OnLimitedChang();
        List<ItemInfo> _listReward = new List<ItemInfo>();
        SubActiveConfig cfg = GameConfigMgr.Instance.GetSubActiveConfig(subId);
        if (!string.IsNullOrEmpty(cfg.Reward) && !string.IsNullOrEmpty(cfg.BundleID))
        {
            string[] rewards = cfg.Reward.Split(',');
            if (rewards.Length % 2 != 0)
                return;
            for (int i = 0; i < rewards.Length; i += 2)
            {
                if (i < rewards.Length - 2)
                {
                    ItemInfo itemInfo = new ItemInfo();
                    itemInfo.Id = int.Parse(rewards[i]);
                    itemInfo.Value = int.Parse(rewards[i + 1]);
                    _listReward.Add(itemInfo);
                }
            }
            GetItemTipMgr.Instance.ShowItemResult(_listReward);
            DelayCall(0.5f, () => { LoadingMgr.Instance.HideRechargeMask(); });
        }
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _parent.anchoredPosition = new Vector2(0f, 0f);
        _limitedDataVO = args[0] as LimitedDataVO;
        OnLimitedChang();

    }

    private void OnLimitedChang()
    {
        _limitedTime = _limitedDataVO.ActivityTime;
        if (_timer != 0)
            TimerHeap.DelTimer(_timer);
        int interval = 1000;
        _timer = TimerHeap.AddTimer(0, interval, OnAddTime);

        MainActiveConfig cfg = GameConfigMgr.Instance.GetMainActiveConfig(_limitedDataVO.mActivityId);
        _title.text = LanguageMgr.GetLanguage(cfg.Title);
        _detail.text = LanguageMgr.GetLanguage(cfg.Description);
        GameResMgr.Instance.LoadMapImage(cfg.BackImg, (spr) => { _bjImg.sprite = spr; });
        _tbImg.sprite = GameResMgr.Instance.LoadItemIcon("welfare/icon_mzthhd_01"); //cfg.FrontImg);
        if (cfg.EventID > 0)
        {
            _festivalObj.SetActive(false);
            OnBaseClear();
            _listUiItemView = new List<UIBaseView>();
            for (int i = 0; i < _limitedDataVO.mListLimitedItemDataVO.Count; i++)
            {
                UIBaseView uiBaseView;
                GameObject obj;
                if (cfg.EventID == EventConst.Recharge)
                {
                    obj = GameObject.Instantiate(_offerItemObj);
                    uiBaseView = new LimitedOfferView();
                }
                else if (cfg.EventID == EventConst.Exchange)
                {
                    obj = GameObject.Instantiate(_exchangeItemObj);
                    uiBaseView = new LimitedExchangeView();
                }
                else if (cfg.EventID == EventConst.Rebate)
                {
                    obj = GameObject.Instantiate(_rebateItemObj);
                    uiBaseView = new LimitedRebateView();
                }
                else
                {
                    obj = GameObject.Instantiate(_taskItemObj);
                    uiBaseView = new LimitedTaskView();
                }
                obj.transform.SetParent(_parent, false);
                uiBaseView.SetDisplayObject(obj);
                uiBaseView.Show(_limitedDataVO.mListLimitedItemDataVO[i], cfg.EventID);
                _listUiItemView.Add(uiBaseView);
            }
        }
        else
        {
            OnBaseClear();
            _festivalObj.SetActive(true);
        }
    }

    private void OnAddTime()
    {
        if (_limitedTime > 0)
        {
            _limitedTime -= 1;
            _time.text = LanguageMgr.GetLanguage(5007503, TimeHelper.GetCountTime(_limitedTime));
        }
        else
        {
            GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(WelfareEvent.ActivityEnd, _limitedDataVO.mActivityId);
        }
    }

    private void OnBaseClear()
    {
        if (_listUiItemView != null)
        {
            for (int i = 0; i < _listUiItemView.Count; i++)
                _listUiItemView[i].Dispose();
            _listUiItemView.Clear();
            _listUiItemView = null;
        }
    }

    public override void Dispose()
    {
        OnBaseClear();
        base.Dispose();
    }
}
