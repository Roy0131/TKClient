using UnityEngine;
using UnityEngine.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using NewBieGuide;
using Framework.UI;

public class EquipSelectView : UIBaseView
{
    private Button _closeBtn;
    private RectTransform _itemRoot;
    private List<ItemView> _lstItemViews;
    private EquipEventVO _vo;
    private GameObject _no;
    protected override void ParseComponent()
    {
        base.ParseComponent();
        _no = Find("No");
        _itemRoot = Find<RectTransform>("ScrollView/Content");
        _closeBtn = Find<Button>("BtnClose");

        _closeBtn.onClick.Add(Hide);

        ColliderHelper.SetButtonCollider(_closeBtn.transform);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);

        ClearItemViews();

        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.EquipItemView);
        _vo = args[0] as EquipEventVO;
        List<ItemInfo> value = BagDataModel.Instance.GetEquipItemByEquipType(_vo.mEquipType);
        if (value != null && value.Count > 0)
        {
            ItemView view;
            _lstItemViews = new List<ItemView>();
            value.Sort(OnSort);
            for (int i = 0; i < value.Count; i++)
            {
                if (GameConfigMgr.Instance.GetItemConfig(value[i].Id).ItemType == 2)
                    view = ItemFactory.Instance.CreateItemView(value[i], ItemViewType.EquipItem, OnClick);
                else
                    view = ItemFactory.Instance.CreateItemView(value[i], ItemViewType.BagItem, OnClick);
                view.mRectTransform.SetParent(_itemRoot, false);
                _lstItemViews.Add(view);
            }

            if (_lstItemViews != null)
                NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.EquipItemView, _lstItemViews[0].GetBtnTransform());
        }
        _no.SetActive(value == null || value.Count == 0);
    }

    private int OnSort(ItemInfo v1,ItemInfo v2)
    {
        ItemConfig cfg1 = GameConfigMgr.Instance.GetItemConfig(v1.Id);
        ItemConfig cfg2 = GameConfigMgr.Instance.GetItemConfig(v2.Id);
        if (cfg1.BattlePower != cfg2.BattlePower)
            return cfg1.BattlePower > cfg2.BattlePower ? -1 : 1;
        else
            return v1.Id > v2.Id ? -1 : 1;
    }

    private int _itemCfgId;
    private void OnClick(ItemView view)
    {
        LogHelper.LogWarning(view.mItemDataVO.mItemConfig.ID);
        _itemCfgId = view.mItemDataVO.mItemConfig.ID;
        ItemTipsMgr.Instance.ShowItemTips(view.mItemDataVO.mItemConfig, ItemTipsType.EquipBagTips);
    }

    private void OnWearEquip()
    {
        GameNetMgr.Instance.mGameServer.ReqWearEquip(_vo.mCardDataVO.mCardID, _itemCfgId);
        Hide();
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(EquipEvent.EquipBagTipsFun1Called, OnWearEquip);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(EquipEvent.EquipBagTipsFun1Called, OnWearEquip);
    }

    public override void Hide()
    {
        base.Hide();
        ClearItemViews();
        _vo = null;
        _itemCfgId = 0;
    }

    private void ClearItemViews()
    {
        if (_lstItemViews == null)
            return;
        for (int i = 0; i < _lstItemViews.Count; i++)
            ItemFactory.Instance.ReturnItemView(_lstItemViews[i]);
        _lstItemViews.Clear();
        _lstItemViews = null;
    }

    public override void Dispose()
    {
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.EquipItemView);
        ClearItemViews();
        base.Dispose();
    }
}