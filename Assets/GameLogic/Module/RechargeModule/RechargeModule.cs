using Framework.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RechargeModule : ModuleBase
{
    private Button _disBtn;
    private RechargeView _rechargeView;

    private Transform _root;
    public RechargeModule()
       : base(ModuleID.Recharge, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_Rechatge;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _disBtn = Find<Button>("Root/Btn_Back");

        _rechargeView = new RechargeView();
        _rechargeView.SetDisplayObject(Find("Root/InterfaceObj"));
        _root = Find<Transform>("Root");
        _disBtn.onClick.Add(OnClose);

        ColliderHelper.SetButtonCollider(_disBtn.transform, 120, 120);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        RechargeDataModel.Instance.AddEvent<List<MonthCardData>>(RechargeEvent.RechargeData, OnRechargeData);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        RechargeDataModel.Instance.RemoveEvent<List<MonthCardData>>(RechargeEvent.RechargeData, OnRechargeData);
    }

    private void OnRechargeData(List<MonthCardData> listCardData)
    {
        _rechargeView.Show(listCardData);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        RechargeDataModel.Instance.ReqRechargeData();
    }

    public override void Hide()
    {
        if (_rechargeView != null)
            _rechargeView.Hide();
        base.Hide();
    }

    public override void Dispose()
    {
        if (_rechargeView != null)
        {
            _rechargeView.Dispose();
            _rechargeView = null;
        }
        base.Dispose();
    }
    protected override void OnShowAnimator()
    {
        base.OnShowAnimator();
        ObjectHelper.PopAnimationBack(_root);
    }
}
