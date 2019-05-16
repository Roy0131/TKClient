
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CTowerRankModule : ModuleBase
{
    private Button _btnClose;
    private CTowerRankView _cTowerRankView;

    public CTowerRankModule() : base(ModuleID.TowerRank, UILayer.Popup)
    {
        _modelResName = UIModuleResName.UI_TowerRank;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _btnClose = Find<Button>("BtnClose");
        ColliderHelper.SetButtonCollider(_btnClose.transform);

        _cTowerRankView = new CTowerRankView();
        _cTowerRankView.SetDisplayObject(Find("Rank"));

        _btnClose.onClick.Add(OnClose);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        CTowerDataModel.Instance.ReqTowerRankingData();
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        CTowerDataModel.Instance.AddEvent(CTowerEvent.RefreshTowerRankingListData, Refresh);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        CTowerDataModel.Instance.RemoveEvent(CTowerEvent.RefreshTowerRankingListData, Refresh);
    }

    private void Refresh()
    {
        _cTowerRankView.Show();
    }

    public override void Hide()
    {
        if (_cTowerRankView != null)
            _cTowerRankView.Hide();
        base.Hide();
    }

    public override void Dispose()
    {
        if (_cTowerRankView != null)
        {
            _cTowerRankView.Dispose();
            _cTowerRankView = null;
        }
        base.Dispose();
    }
}
