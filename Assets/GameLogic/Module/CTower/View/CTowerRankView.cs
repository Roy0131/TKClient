using Framework.UI;
using Msg.ClientMessage;
using UnityEngine;

public class CTowerRankView : UILoopBaseView<TowerRankInfo>
{
    private GameObject _itemObj;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _itemObj = Find("Item");
        InitScrollRect("ScrollView");
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _lstDatas = CTowerDataModel.Instance.mListTowerRankInfo;
        _loopScrollRect.ClearCells();
        if (_lstDatas.Count == 0)
            return;
        _loopScrollRect.totalCount = _lstDatas.Count;
        _loopScrollRect.RefillCells();
    }

    protected override UIBaseView CreateItemView()
    {
        CTowerRankItemView item = new CTowerRankItemView();
        item.SetDisplayObject(GameObject.Instantiate(_itemObj));
        return item;
    }

    protected override void SetItemData(UIBaseView view, int idx)
    {
        view.Show(_lstDatas[idx], idx);
    }
}
