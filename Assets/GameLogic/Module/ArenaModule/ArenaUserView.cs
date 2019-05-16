using UnityEngine;
using Msg.ClientMessage;
using Framework.UI;

public class ArenaUserView : UILoopBaseView<RankItemInfo>
{
    private GameObject _itemObject;
    protected override void ParseComponent()
    {
        base.ParseComponent();
        _itemObject = Find("ArenaItem");
        InitScrollRect("ScrollView");

        CreateFixedEffect(Find("fx_ui_jingji"), UILayerSort.WindowSortBeginner);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        ArenaDataModel.Instance.AddEvent(ArenaEvent.AreanRankRefresh, OnRefreshRank);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        ArenaDataModel.Instance.RemoveEvent(ArenaEvent.AreanRankRefresh, OnRefreshRank);
    }

    protected override UIBaseView CreateItemView()
    {
        UIBaseView itemView = new ArenaUserItem();
        itemView.SetDisplayObject(GameObject.Instantiate(_itemObject));
        return itemView;
    }

    private void OnRefreshRank()
    {
        _loopScrollRect.ClearCells();
        _lstDatas = ArenaDataModel.Instance.mlstRankItem;
        if (_lstDatas.Count == 0)
            return;
        _loopScrollRect.totalCount = _lstDatas.Count;
        _loopScrollRect.RefillCells();
    }
}