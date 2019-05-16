using UnityEngine.UI;

public class GuildBossModule : ModuleBase
{
    private Button _disBtn;
    private GuildBossView _heroShopView;
    public GuildBossModule()
        : base(ModuleID.GuildBoss, UILayer.Popup)
    {
        _modelResName = UIModuleResName.UI_GuildBoss;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();

        _disBtn = Find<Button>("Btn_Back");

        _heroShopView = new GuildBossView();
        _heroShopView.SetDisplayObject(Find("BossObj"));

        _disBtn.onClick.Add(OnClose);

        ColliderHelper.SetButtonCollider(_disBtn.transform, 120, 120);
    }

    public override void Hide()
    {
        base.Hide();
        _heroShopView.Hide();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        if (args.Length > 0)
            _heroShopView.Show(bool.Parse(args[0].ToString()));
        else
            _heroShopView.Show(true);
    }
}
