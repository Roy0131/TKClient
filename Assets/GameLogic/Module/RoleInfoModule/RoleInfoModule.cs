public class RoleInfoModule : ModuleBase
{
    private RoleView _roleView;
    private RoleFuncView _funcView;
    private EquipSelectView _equipSelectView;
    public RoleInfoModule()
        :base(ModuleID.RoleInfo, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_RoleInfo;
        mBlNeedBackMask = true;
    }

	protected override void ParseComponent()
	{
        base.ParseComponent();
        _roleView = new RoleView();
        _roleView.SetDisplayObject(Find("LeftSide"));

        _funcView = new RoleFuncView();
        _funcView.SetDisplayObject(Find("RightSide"));

        AddChildren(_roleView);
        AddChildren(_funcView);

        _equipSelectView = new EquipSelectView();
        _equipSelectView.SetDisplayObject(Find("SelectEquip"));
        _equipSelectView.SortingOrder = UILayerSort.WindowSortBeginner + 1;
    }

    private void OnShowEquipSelect(EquipEventVO evtVO)
    {
        _equipSelectView.Show(evtVO);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<EquipEventVO>(EquipEvent.ShowEquipSelectView, OnShowEquipSelect);
    }
    
    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<EquipEventVO>(EquipEvent.ShowEquipSelectView, OnShowEquipSelect);
    }

    public override void Dispose()
    {
        if (_equipSelectView != null)
        {
            _equipSelectView.Dispose();
            _equipSelectView = null;
        }
        base.Dispose();
    }
}