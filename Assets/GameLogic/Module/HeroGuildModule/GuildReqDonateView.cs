using Framework.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuildReqDonateView : UIBaseView
{
    private Button _reqDonate;
    private Button _disBtn;
    private RectTransform _parent;
    private GameObject _debris;
    private List<DonateItemView> _listItemView;
    private DonateItemView _donateItemView;
    private int _curItemId;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _reqDonate = Find<Button>("Image/Req");
        _disBtn = Find<Button>("Dis");
        _parent = Find<RectTransform>("Image/Panel_Scroll/KnapsackPanel");
        _debris = Find("Image/Panel_Scroll/KnapsackPanel/DebrisObj");

        _reqDonate.onClick.Add(OnReq);
        _disBtn.onClick.Add(OnDis);

        ColliderHelper.SetButtonCollider(_disBtn.transform);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GameEventMgr.Instance.mGlobalDispatcher.AddEvent<DonateItemView>(GuildEvent.ReqDonate, OnReqDonate);
        GuildDataModel.Instance.AddEvent(GuildEvent.ReqDonateResult, OnResult);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GameEventMgr.Instance.mGlobalDispatcher.RemoveEvent<DonateItemView>(GuildEvent.ReqDonate, OnReqDonate);
        GuildDataModel.Instance.RemoveEvent(GuildEvent.ReqDonateResult, OnResult);
    }

    private void OnResult()
    {
        Hide();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _donateItemView = null;
        _curItemId = 0;
        OnDictDonate();
    }

    private void OnDictDonate()
    {
        ClearHeroShopItem();
        _listItemView = new List<DonateItemView>();
        Dictionary<int, GuildDonateConfig> allDatas = GuildDonateConfig.Get();
        foreach (GuildDonateConfig cfg in allDatas.Values)
        {
            ItemInfo itemInfo = new ItemInfo();
            GameObject obj = GameObject.Instantiate(_debris);
            obj.transform.SetParent(_parent, false);
            DonateItemView itemView = new DonateItemView();
            itemView.SetDisplayObject(obj);
            itemView.Show(cfg.ItemID);
            _listItemView.Add(itemView);
        }
    }

    private void ClearHeroShopItem()
    {
        if (_listItemView != null)
        {
            for (int i = 0; i < _listItemView.Count; i++)
                _listItemView[i].Dispose();
            _listItemView.Clear();
            _listItemView = null;
        }
    }

    private void OnReqDonate(DonateItemView itemView)
    {
        if (_donateItemView!=null)
            _donateItemView.BlSelected = false;
        _donateItemView = itemView;
        _donateItemView.BlSelected = true;
        _curItemId = itemView.itemId;
    }

    private void OnReq()
    {
        if (_curItemId > 0)
            GameNetMgr.Instance.mGameServer.ReqGuildAsk(_curItemId, GameConfigMgr.Instance.GetGuildDonateConfig(_curItemId).RequestNum);
    }

    private void OnDis()
    {
        Hide();
    }
}
