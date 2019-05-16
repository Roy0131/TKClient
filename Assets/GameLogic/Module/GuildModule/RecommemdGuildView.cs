using UnityEngine.UI;
using Framework.UI;

public class RecommemdGuildView : UILoopBaseView<GuildDataVO>
{
    private Button _refreshBtn;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _refreshBtn = Find<Button>("RefreshBtn");
        InitScrollRect("ScrollView");

        _refreshBtn.onClick.Add(OnRefreshList);
    }

    private void OnRefreshList()
    {
        SoundMgr.Instance.PlayEffectSound("UI_btn_refresh");
        GameNetMgr.Instance.mGameServer.ReqRecommemdGuild();
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GuildDataModel.Instance.AddEvent(GuildEvent.RecommemdGuildRefresh, OnShowGuildList);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GuildDataModel.Instance.RemoveEvent(GuildEvent.RecommemdGuildRefresh, OnShowGuildList);
    }

    private void OnShowGuildList()
    {
        _lstDatas = GuildDataModel.Instance.mlstRecommemdGuilds;
        _loopScrollRect.ClearCells();
        _loopScrollRect.totalCount = _lstDatas.Count;
        if (_lstDatas.Count == 0)
            return;
        _loopScrollRect.RefillCells();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        GuildDataModel.Instance.ReqRecommemdGuild();
    }

    protected override UIBaseView CreateItemView()
    {
        return GuildItemFactory.Instance.CreateGuildItem(GuildItemType.Recommemd);
    }

    public override void RetItemView(UIBaseView view)
    {
        _lstShowViews.Remove(view);
        GuildItemFactory.Instance.ReturnGuildItem(view as GuildItemView);
    }
}