using Framework.UI;
using NewBieGuide;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipListView : UIBaseView
{
    private List<Toggle> _lstTypeToggle;
    private List<ItemView> _lstEquipViews;
    private RectTransform _itemViewRoot;
    private Button _forgeOneKeyBtn;
    private int _equipType;
    private ItemView _curItemView;

    //private Dictionary<int, List<ItemUpgradeConfig>> _dictUpgradeCfgs;
    protected override void ParseComponent()
    {
        base.ParseComponent();

        _itemViewRoot = Find<RectTransform>("ScrollView/Content");

        _lstTypeToggle = new List<Toggle>();
        RedPointEnum redPointID;
        for (int i = 1; i <= 4; i++)
        {
            _lstTypeToggle.Add(Find<Toggle>("ToggleGroup/Toggle" + i));
            redPointID = RedPointHelper.GetRedPointEnum((int)RedPointEnum.EquipFusion * 100 + i);
            RedPointTipsMgr.Instance.RedPointBindObject(redPointID, Find("ToggleGroup/Toggle" + i + "/RedDot"));
        }
        foreach(var t in _lstTypeToggle)
            t.onValueChanged.Add((bool value) => { if (value) OnToggleChange(t); });
        _equipType = 1;

        _forgeOneKeyBtn = Find<Button>("ForgeOneKeyBtn");
        _forgeOneKeyBtn.onClick.Add(OnForgeByOneKey);
    }

    private void OnForgeByOneKey()
    {
        if (_lstEquipViews == null || _lstEquipViews.Count == 0)
            return;
        IList<int> result = new List<int>();
        int id;
        int count;
        for (int i = 0; i < _lstEquipViews.Count; i++)
        {
            id = _lstEquipViews[i].mItemDataVO.mItemConfig.ID - 1;
            count = BagDataModel.Instance.GetItemCountById(id);
            if (count < 3 || result.Contains(id))
                continue;
            result.Add(id);
        }
        if (result.Count == 0)
            return;
        GameNetMgr.Instance.mGameServer.ReqUpgradeItemByOneKey(result);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        BagDataModel.Instance.AddEvent<List<int>>(BagEvent.BagItemRefresh, OnBagItemView);
        BagDataModel.Instance.AddEvent(BagEvent.BagAllItemRefresh, OnBagRefresh);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        BagDataModel.Instance.RemoveEvent<List<int>>(BagEvent.BagItemRefresh, OnBagItemView);
        BagDataModel.Instance.RemoveEvent(BagEvent.BagAllItemRefresh, OnBagRefresh);
    }

    private void OnBagRefresh()
    {
        ShowEquipItemView(true);
    }

    private void OnBagItemView(List<int> value)
    {
        ShowEquipItemView(true);
    }

    private void OnToggleChange(Toggle tog)
    {
        _equipType = _lstTypeToggle.IndexOf(tog) + 1;
        ShowEquipItemView();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        ShowEquipItemView();
        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.EquipItem, _lstEquipViews[0].GetBtnTransform());
    }

    private RedPointEnum _parentID = RedPointEnum.None;
    private int _index = 0;

    private void ShowEquipItemView(bool isRefresh = false)
    {
        ClearItemView();
        LogHelper.LogWarning(_equipType);
        List<EquipForgeDataVO> values = EquipForgeDataModel.Instance.GetEquipForeData(_equipType);
        ItemView view;
        _lstEquipViews = new List<ItemView>();
        ItemConfig itemCfg;
        _parentID = RedPointHelper.GetRedPointEnum((int)RedPointEnum.EquipFusion * 100 + _equipType);

        values.Sort((x, y) => x.mForgeItemID.CompareTo(y.mForgeItemID));
        for (int i = 0; i < values.Count; i++)
        {
            itemCfg = GameConfigMgr.Instance.GetItemConfig(values[i].mForgeItemID);
            view = ItemFactory.Instance.CreateItemView(itemCfg, ItemViewType.EquipItem, OnClick);
            view.mRectTransform.SetParent(_itemViewRoot, false);
            _lstEquipViews.Add(view);
            RedPointTipsMgr.Instance.ChildNodeBindObject(values[i].mForgeItemID, _parentID, view.mRedPointObject);
        }
        if (!isRefresh)
        {
            if (_lstEquipViews.Count > 0)
                OnClick(_lstEquipViews[0]);
            else
                OnClick(null);
        }
        else
        {
            OnClick(_lstEquipViews[_index]);
        }
    }

    private void OnClick(ItemView view)
    {
        for (int i = 0; i < _lstEquipViews.Count; i++)
        {
            if (_lstEquipViews[i] == view)
                _index = i;
        }
        if (_curItemView != null)
            _curItemView.BlSelected = false;
        _curItemView = view;
        view.BlSelected = true;
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(EquipEvent.EquipForgeSelect, view.mItemDataVO.mItemConfig.ID - 1);
    }

    private void ClearItemView()
    {
        if (_lstEquipViews == null)
            return;
        for (int i = 0; i < _lstEquipViews.Count; i++)
        {
            RedPointTipsMgr.Instance.ChildNodeUnBindObject(_lstEquipViews[i].mItemDataVO.mItemConfig.ID, _parentID, _lstEquipViews[i].mRedPointObject);
            ItemFactory.Instance.ReturnItemView(_lstEquipViews[i]);
        }
        _lstEquipViews.Clear();
        _lstEquipViews = null;
    }

    public override void Hide()
    {
        _lstTypeToggle[0].isOn = true;
        base.Hide();
    }

    public override void Dispose()
    {
        ClearItemView();
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.EquipItem);
        base.Dispose();
    }
}