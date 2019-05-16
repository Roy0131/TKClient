using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class RoleSelectModule : ModuleBase
{
    private RoleSelectView _roleSelectView;

    public RoleSelectModule()
        : base(ModuleID.RoleSelect, UILayer.Popup)
    {
        _modelResName = UIModuleResName.UI_RoleSelect;
        mBlStack = false;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _roleSelectView = new RoleSelectView();
        _roleSelectView.SetDisplayObject(Find("RoleSeleObj"));
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<int>(UIEventDefines.FusionMatSelectOK, OnMatSelectOK);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<int>(UIEventDefines.FusionMatSelectOK, OnMatSelectOK);
    }

    private void OnMatSelectOK(int idx)
    {
        OnClose();
    }

    protected override void OnClose()
    {
        base.OnClose();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _roleSelectView.Show(args[0], args[1]);
    }

    public override void Hide()
    {
        base.Hide();
        if (_roleSelectView != null)
            _roleSelectView.Hide();
    }

    public override void Dispose()
    {
        if (_roleSelectView != null)
        {
            _roleSelectView.Dispose();
            _roleSelectView = null;
        }
        base.Dispose();
    }
}