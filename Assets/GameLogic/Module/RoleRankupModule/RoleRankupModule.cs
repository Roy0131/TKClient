public class RoleRankupModule : ModuleBase
{
    private RoleRankupView _view;
    public RoleRankupModule()
        : base(ModuleID.RoleRankup, UILayer.Popup)
    {
        _modelResName = UIModuleResName.UI_RoleRankup;
    }

	protected override void ParseComponent()
	{
        base.ParseComponent();

        _view = new RoleRankupView();
        _view.SetDisplayObject(Find("AttriRoot"));

        AddChildren(_view);
	}
}