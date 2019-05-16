using Msg.ClientMessage;
using NewBieGuide;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public class BagItemViewMgr : UILoopBaseView<ItemInfo>
{
    private Text _tips;
    private GameObject _itemObj;
    private GameObject _kindObj;
    private KindGroup _kindGroup;
    private int _curItemType;
    private int _equipeType;

    private int _itemId = 0;
    private bool _isBagRefresh;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _tips = Find<Text>("Tips");
        _itemObj = Find("DebrisObj");
        _kindObj = Find("Kind");
        InitScrollRect("Panel_Scroll");
        _kindGroup = new KindGroup(OnKindChang);
        _kindGroup.SetDisplayObject(Find("Kind"));
    }

    private void OnKindChang(int kind)
    {
        _equipeType = kind;
        OnBagChange();
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        BagDataModel.Instance.AddEvent<List<int>>(BagEvent.BagItemRefresh, OnBagItemRefresh);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<ItemView>(BagEvent.Click, OnItemClick);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        BagDataModel.Instance.RemoveEvent<List<int>>(BagEvent.BagItemRefresh, OnBagItemRefresh);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<ItemView>(BagEvent.Click, OnItemClick);
    }

    private void OnItemClick(ItemView view)
    {
        _itemId = view.mItemDataVO.mItemConfig.ID;
        for (int i = 0; i < _lstShowViews.Count; i++)
            (_lstShowViews[i] as BagItemView)._view.BlSelected = (_lstShowViews[i] as BagItemView)._itemInfo.Id == _itemId;
    }

    private void OnBagItemRefresh(List<int> list)
    {
        OnBagChange();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _curItemType = int.Parse(args[0].ToString());
        OnKindChang(0);
        _kindGroup.OnKindReset();
        _kindObj.SetActive(_curItemType == 1 || _curItemType == 4);
        if (_lstShowViews.Count > 0)
            NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.FragmentItemView, _lstShowViews[0].mTransform);
    }

    private void OnBagChange()
    {
        KindType kintype = (KindType)_equipeType;
        _lstDatas = BagDataModel.Instance.GetBagItemDataByType(_curItemType, kintype);
        _loopScrollRect.ClearCells();
        if (_lstDatas.Count == 0)
        {
            GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(BagEvent.BagNull, true);
            _tips.gameObject.SetActive(true);
            if (_curItemType == 1)
                _tips.text = LanguageMgr.GetLanguage(6001113);
            else if (_curItemType == 2)
                _tips.text = LanguageMgr.GetLanguage(6001114);
            else if (_curItemType == 3)
                _tips.text = LanguageMgr.GetLanguage(6001115);
            else if (_curItemType == 4)
                _tips.text = LanguageMgr.GetLanguage(6001116);
            return;
        }
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(BagEvent.BagNull, false);
        _tips.gameObject.SetActive(false);
        _isBagRefresh = true;
        _itemId = _lstDatas[0].Id;
        _loopScrollRect.totalCount = _lstDatas.Count;
        _loopScrollRect.RefillCells();
    }

    protected override UIBaseView CreateItemView()
    {
        BagItemView item = new BagItemView();
        item.SetDisplayObject(GameObject.Instantiate(_itemObj));
        return item;
    }

    protected override void SetItemData(UIBaseView view, int idx)
    {
        view.Show(_lstDatas[idx], _isBagRefresh);
        (view as BagItemView)._view.BlSelected = (view as BagItemView)._itemInfo.Id == _itemId;
        _isBagRefresh = false;
    }

    public override void Dispose()
    {
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.FragmentItemView);
        base.Dispose();
    }
}
