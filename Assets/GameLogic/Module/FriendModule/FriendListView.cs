using Framework.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendListView : UILoopBaseView<FriendDataVO>
{
    private Text _tips;
    private Text _friendShipPointCount;
    private Text _strengthCount;
    private Text _giftFriendNum;
    private Button _onekeyBtn;
    private GameObject _itemObject;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _tips = Find<Text>("Tips");
        _friendShipPointCount = Find<Text>("FriendShipPoint/Count");
        _strengthCount = Find<Text>("Strength/Count");
        _giftFriendNum = Find<Text>("GiftFriendNum");
        _onekeyBtn = Find<Button>("OneKeyBtn");
        _itemObject = Find("ScrollView/Content/Item");

        InitScrollRect("ScrollView");

        _onekeyBtn.onClick.Add(OnGetAndGivePoint);
    }

    private void OnGetAndGivePoint()
    {
        if (_lstDatas.Count <= 0)
            return;
        IList<int> lst = new List<int>();
        for (int i = 0; i < _lstDatas.Count; i++)
        {
            if (_lstDatas[i].BlGetOrSendPoint)
                lst.Add(_lstDatas[i].mPlayerId);
        }
        GameNetMgr.Instance.mGameServer.ReqOnKeyGetAndSendPoints(lst);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        FriendDataModel.Instance.AddEvent(FriendEvent.FriendListRefresh, OnFriendListRefresh);
        BagDataModel.Instance.AddEvent<List<int>>(BagEvent.BagItemRefresh, OnRefreshItem);
        FriendDataModel.Instance.AddEvent<List<int>>(FriendEvent.FriendRefreshGetPoints, OnRefreshFriendPoints);
        FriendDataModel.Instance.AddEvent<List<int>>(FriendEvent.FriendRefreshGivePoints, OnRefreshFriendPoints);
        FriendDataModel.Instance.AddEvent<int>(FriendEvent.FriendRefreshBossHp, OnRefreshFriendBossHp);
        FriendDataModel.Instance.AddEvent(FriendEvent.FriendAssistDataRefresh, OnRefreshAssistData);
        FriendDataModel.Instance.AddEvent<List<int>>(FriendEvent.RemoveFriend, OnRemoveFriend);
    }

    private void OnRefreshFriendPoints(List<int> value)
    {
        if (_lstShowViews == null || _lstShowViews.Count == 0)
            return;
        _onekeyBtn.gameObject.SetActive(FriendDataModel.Instance.BlFriendListGetOrSendPoint);
        FriendListItemView view;
        for (int i = 0; i < _lstShowViews.Count; i++)
        {
            view = _lstShowViews[i] as FriendListItemView;
            if (value.Contains(view.FriendData.mPlayerId))
                view.RefreshPointsStatus();
        }
        OnRefreshAssistData();
    }

    private void OnRefreshFriendBossHp(int friendId)
    {
        if (_lstShowViews == null || _lstShowViews.Count == 0)
            return;
        FriendListItemView view;
        for (int i = 0; i < _lstShowViews.Count; i++)
        {
            view = _lstShowViews[i] as FriendListItemView;
            if (view.FriendData.mPlayerId == friendId)
                view.RefreshFriendBossStatus();
        }
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        FriendDataModel.Instance.RemoveEvent(FriendEvent.FriendListRefresh, OnFriendListRefresh);
        BagDataModel.Instance.RemoveEvent<List<int>>(BagEvent.BagItemRefresh, OnRefreshItem);
        FriendDataModel.Instance.RemoveEvent<List<int>>(FriendEvent.FriendRefreshGetPoints, OnRefreshFriendPoints);
        FriendDataModel.Instance.RemoveEvent<List<int>>(FriendEvent.FriendRefreshGivePoints, OnRefreshFriendPoints);
        FriendDataModel.Instance.RemoveEvent<int>(FriendEvent.FriendRefreshBossHp, OnRefreshFriendBossHp);
        FriendDataModel.Instance.RemoveEvent(FriendEvent.FriendAssistDataRefresh, OnRefreshAssistData);
        FriendDataModel.Instance.AddEvent<List<int>>(FriendEvent.RemoveFriend, OnRemoveFriend);
    }

    private void OnRemoveFriend(List<int> listId)
    {
        //for (int i = _lstShowViews.Count - 1; i >= 0; i--)
        //{
        //    for (int j = 0; j < listId.Count; j++)
        //    {
        //        if ((_lstShowViews[i] as FriendListItemView).FriendData.mPlayerId == listId[j])
        //        {
        //            //int index = _lstDatas.Count >= 30 ? 30 : _lstDatas.Count;
        //            //GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(FriendEvent.RefreshFriendTitle, LanguageMgr.GetLanguage(5001501) + index + "/30");
        //            RetItemView(_lstShowViews[i]);
        //        }
        //    }
        //}
        OnFriendListRefresh();
    }

    private void OnRefreshAssistData()
    {
        _giftFriendNum.text = FriendDataModel.Instance.mFriendAssistVO.mTotaGiveGetPoint + "/" + GameConst.MaxFriendship;
    }

    private void OnRefreshItem(List<int> value)
    {
        if (value == null || value.Contains(SpecialItemID.FriendShipPoint) || value.Contains(SpecialItemID.FriendStrength))
        {
            _friendShipPointCount.text = BagDataModel.Instance.GetItemCountById(SpecialItemID.FriendShipPoint).ToString();
            _strengthCount.text = BagDataModel.Instance.GetItemCountById(SpecialItemID.FriendStrength) + "/" + GameConst.FriendBossStrengthMax;
        }
    }

    private void OnFriendListRefresh()
    {
        _onekeyBtn.gameObject.SetActive(FriendDataModel.Instance.BlFriendListGetOrSendPoint);
        _lstDatas = FriendDataModel.Instance.mlstAllFriends;
        int index = _lstDatas.Count >= 30 ? 30 : _lstDatas.Count;
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(FriendEvent.RefreshFriendTitle, LanguageMgr.GetLanguage(5001501) + index + "/30");
        _loopScrollRect.ClearCells();
        if (_lstDatas.Count == 0)
        {
            _tips.gameObject.SetActive(true);
            return;
        }
        _tips.gameObject.SetActive(false);
        _loopScrollRect.totalCount = _lstDatas.Count;
        _loopScrollRect.RefillCells();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        OnFriendListRefresh();
        OnRefreshItem(null);
    }

    protected override UIBaseView CreateItemView()
    {
        FriendListItemView item = new FriendListItemView();
        item.SetDisplayObject(GameObject.Instantiate(_itemObject));
        return item;
    }

    public override void RetItemView(UIBaseView view)
    {
        _uiViewPools.Enqueue(view);
        view.Hide();
        view.mRectTransform.SetParent(mRectTransform, false);
        _lstShowViews.Remove(view);
    }
}
