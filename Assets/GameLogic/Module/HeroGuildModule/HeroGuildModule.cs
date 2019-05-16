using UnityEngine.UI;

public class HeroGuildModule : ModuleBase
{
    private Button _closeBtn;
    private GuildReqDonateView _reqDonateView;
    private GuildInfoModifyView _modifyView;
    private HeroGuildLogView _guildLogView;
    private GuildMapView _guildMapView;

    public HeroGuildModule()
        : base(ModuleID.HeroGuild, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_HeroGuild;
        mBlNeedBackMask = true;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();

        HeroGuildView guildView = new HeroGuildView();
        guildView.SetDisplayObject(Find("Content/GuildInfoRoot"));
        AddChildren(guildView);
        
        GuildDonateView donateView = new GuildDonateView();
        donateView.SetDisplayObject(Find("Content/DonateRoot"));
        AddChildren(donateView);

        _reqDonateView = new GuildReqDonateView();
        _reqDonateView.SetDisplayObject(Find("Content/DonateAskRoot"));

        _modifyView = new GuildInfoModifyView();
        _modifyView.SetDisplayObject(Find("Content/GuildInfoModifyRoot"));

        _guildLogView = new HeroGuildLogView();
        _guildLogView.SetDisplayObject(Find("Content/GuildLogRoot"));

        _guildMapView = new GuildMapView();
        _guildMapView.SetDisplayObject(Find("Content/GuildMapRoot"));

        _closeBtn = Find<Button>("Content/GuildInfoRoot/Buttons/CloseBtn");
        _closeBtn.onClick.Add(OnClose);

        ColliderHelper.SetButtonCollider(_closeBtn.transform, 120, 120);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GameEventMgr.Instance.mGlobalDispatcher.AddEvent(GuildEvent.ShowDonateView, OnReqDonate);
        GameEventMgr.Instance.mGlobalDispatcher.AddEvent(GuildEvent.ShowModifyInfoView, OnShowModifyView);
        GameEventMgr.Instance.mGlobalDispatcher.AddEvent(GuildEvent.ShowGuildLogView, OnShowLogView);
        GameEventMgr.Instance.mGlobalDispatcher.AddEvent(GuildEvent.ShowGuildMapView, OnShowGuildMapView);
        GameEventMgr.Instance.mGlobalDispatcher.AddEvent(GuildEvent.ShowGuildMap, OnShowGuildMap);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GameEventMgr.Instance.mGlobalDispatcher.RemoveEvent(GuildEvent.ShowDonateView, OnReqDonate);
        GameEventMgr.Instance.mGlobalDispatcher.RemoveEvent(GuildEvent.ShowModifyInfoView, OnShowModifyView);
        GameEventMgr.Instance.mGlobalDispatcher.RemoveEvent(GuildEvent.ShowGuildLogView, OnShowLogView);
        GameEventMgr.Instance.mGlobalDispatcher.RemoveEvent(GuildEvent.ShowGuildMapView, OnShowGuildMapView);
        GameEventMgr.Instance.mGlobalDispatcher.RemoveEvent(GuildEvent.ShowGuildMap, OnShowGuildMap);
    }

    private void OnShowGuildMap()
    {
        _guildMapView.Show();
        GameEventMgr.Instance.mGlobalDispatcher.DispathEvent(GuildEvent.ShowGuildBoss);
    }

    private void OnShowGuildMapView()
    {
        _guildMapView.Show();
    }

    private void OnShowModifyView()
    {
        _modifyView.Show();
    }

    private void OnShowLogView()
    {
        _guildLogView.Show();
    }

    public override void Dispose()
    {
        if (_reqDonateView != null)
        {
            _reqDonateView.Dispose();
            _reqDonateView = null;
        }
        if (_modifyView != null)
        {
            _modifyView.Dispose();
            _modifyView = null;
        }
        if (_guildLogView != null)
        {
            _guildLogView.Dispose();
            _guildLogView = null;
        }
        if (_guildMapView != null)
        {
            _guildMapView.Dispose();
            _guildMapView = null;
        }
        base.Dispose();
    }

    private void OnReqDonate()
    {
        _reqDonateView.Show();
    }

    public override void Hide()
    {
        if (_reqDonateView != null)
            _reqDonateView.Hide();
        if (_modifyView != null)
            _modifyView.Hide();
        if (_guildLogView != null)
            _guildLogView.Hide();
        if (_guildMapView != null)
            _guildMapView.Hide();
        base.Hide();
    }
}