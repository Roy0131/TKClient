using UnityEngine;
using UnityEngine.UI;
public class ChatModule : ModuleBase
{
    private Button _BtnClose;
    private Button _BtnClose02;
    private ChatView _chatView;
    private Transform _root;
    private Toggle[] _toggles;

    private Button _guildBtn;
    private Button _recruitingBtn;
    private Button _discordBtn;
    public ChatModule() : base(ModuleID.Chat, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_Chat;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _BtnClose = Find<Button>("Root/BtnClose");
        _BtnClose02 = Find<Button>("Image");
        _guildBtn = Find<Button>("Root/ToggleGroup/Tog2/GuildBtn");
        _recruitingBtn = Find<Button>("Root/ToggleGroup/Tog3/RecruitingBtn");
        _discordBtn = Find<Button>("Root/DiscordBtn");

        _chatView = new ChatView();
        _chatView.SetDisplayObject(Find("Root/ChatContent"));
        _root = Find<Transform>("Root");

        _toggles = new Toggle[3];
        for (int i = 0; i < 3; i++)
            _toggles[i] = Find<Toggle>("Root/ToggleGroup/Tog" + (i + 1));
        foreach (Toggle tog in _toggles)
            tog.onValueChanged.Add((bool blSelect) => { if (blSelect) OnValueChanged(tog); });

        RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.WorldChat, Find("Root/ToggleGroup/Tog1/RedPoint"));
        RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.GuildChat, Find("Root/ToggleGroup/Tog2/RedPoint"));
        RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.RecruitChat, Find("Root/ToggleGroup/Tog3/RedPoint"));

        _BtnClose.onClick.Add(OnClose);
        _BtnClose02.onClick.Add(OnClose);
        _guildBtn.onClick.Add(OnGuildBtn);
        _recruitingBtn.onClick.Add(OnRecruitingBtn);
        _discordBtn.onClick.Add(OnGotoDiscord);
    }

    private void OnGotoDiscord()
    {
        //PopupTipsMgr.Instance.ShowTips("Open Discord!");
        Application.OpenURL("https://discord.gg/WteMFX8");
    }
    
    private void OnGuildBtn()
    {
        PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001122));
    }

    private void OnRecruitingBtn()
    {
        PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001123, GameConst.GetFeatureType(FunctionType.Guild)));
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        OnValueChanged(_toggles[0]);
        _guildBtn.gameObject.SetActive(HeroDataModel.Instance.mHeroInfoData.mGuildId <= 0);
        _recruitingBtn.gameObject.SetActive(HeroDataModel.Instance.mHeroInfoData.mLevel < GameConst.GetFeatureType(FunctionType.Guild));
    }

    private void OnValueChanged(Toggle tog)
    {
        int channel = 0;
        switch (tog.name)
        {
            case "Tog1":
                channel = ChatChannelConst.World;
                break;
            case "Tog2":
                channel = ChatChannelConst.Guild;
                break;
            case "Tog3":
                channel = ChatChannelConst.Recruit;
                break;
        }
        _chatView.Show(channel);
    }

    public override void Hide()
    {
        _toggles[0].isOn = true;
        if (_chatView != null)
            _chatView.Hide();
        base.Hide();
    }

    public override void Dispose()
    {
        if (_chatView != null)
        {
            _chatView.Dispose();
            _chatView = null;
        }
        base.Dispose();
    }
    protected override void OnShowAnimator()
    {
        DGHelper.DoLocalMoveX(_root, 0f, 0.5f, DGEaseType.OutQuad);
        base.OnShowAnimator();
    }
    protected override void OnClose()
    {
        DGHelper.DoLocalMoveX(_root, -1000f, 0.5f, DGEaseType.OutQuad);
        DelayCall(0.5f,base.OnClose);
    }
}
