public class EquipFuncModule : ModuleBase
{
    public EquipFuncModule()
        : base(ModuleID.EquipFunc, UILayer.Popup)
    {
        _modelResName = UIModuleResName.UI_EquipFunc;
        mBlStack = false;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();

        EquipFuncView view = new EquipFuncView();
        view.SetDisplayObject(mDisplayObject);
        AddChildren(view);
    }
}