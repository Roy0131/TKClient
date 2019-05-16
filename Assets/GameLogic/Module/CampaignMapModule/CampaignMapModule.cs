public class CampaignMapModule : ModuleBase
{
    private CampaignMapView _mapView;
    public CampaignMapModule()
        : base(ModuleID.CampaignMap, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_CampaignMap;
        mBlNeedBackMask = true;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _mapView = new CampaignMapView();
        _mapView.SetDisplayObject(Find("ViewObject"));
        AddChildren(_mapView);
    }
}