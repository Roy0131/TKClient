using UnityEngine.UI;
using Framework.UI;

public class GuildSearchView : UILoopBaseView<GuildDataVO>
{
    private InputField _inputField;
    private Text _inputText;
    private Button _searchBtn;
    protected override void ParseComponent()
    {
        base.ParseComponent();

        InitScrollRect("ScrollView");
        _inputText = Find<Text>("InputField/Placeholder");
        _inputField = Find<InputField>("InputField");
        _searchBtn = Find<Button>("SearchBtn");
        _searchBtn.onClick.Add(OnSearch);

        _inputText.text = LanguageMgr.GetLanguage(5003149);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GuildDataModel.Instance.AddEvent(GuildEvent.SerachGuildRefresh, OnShowSearchView);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GuildDataModel.Instance.RemoveEvent(GuildEvent.SerachGuildRefresh, OnShowSearchView);
    }

    private void OnShowSearchView()
    {
        _lstDatas = GuildDataModel.Instance.mlstSearchGuilds;
        _loopScrollRect.ClearCells();
        if (_lstDatas.Count == 0)
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(5003152));
            return;
        }
        _loopScrollRect.totalCount = _lstDatas.Count;
        _loopScrollRect.RefillCells();
    }

    private void OnSearch()
    {
        if (string.IsNullOrWhiteSpace(_inputField.text))
            return;
        GameNetMgr.Instance.mGameServer.ReqSearchGuild(_inputField.text);
    }

    protected override UIBaseView CreateItemView()
    {
        return GuildItemFactory.Instance.CreateGuildItem(GuildItemType.Search);
    }

    public override void RetItemView(UIBaseView view)
    {
        _lstShowViews.Remove(view);
        GuildItemFactory.Instance.ReturnGuildItem(view as GuildItemView);
    }
}
