using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public class GuildCreateView : UIBaseView
{
    private Image _logoIcon;
    private InputField _nameInput;
    private Text _nameInputText;
    private InputField _noticeInput;
    private Text _noticeInputText;
    private Button _createBtn;
    private Button _logoSelectBtn;
    private Text _costText;

    private GuildIconSelectView _iconSelectView;
    private int _logoIconId = 1;
    
    protected override void ParseComponent()
    {
        base.ParseComponent();

        _logoIcon = Find<Image>("LogoIcon/Image");
        _nameInput = Find<InputField>("NameInput");
        _nameInputText = Find<Text>("NameInput/Placeholder");
        _noticeInput = Find<InputField>("NoticeInput");
        _noticeInputText = Find<Text>("NoticeInput/Placeholder");
        _createBtn = Find<Button>("CreateBtn");
        _logoSelectBtn = Find<Button>("LogoBtn");
        _costText = Find<Text>("CreateBtn/NumText");
        _costText.text = GameConst.GuildCreateCost.ToString();

        _iconSelectView = new GuildIconSelectView(OnChooseLogo);
        _iconSelectView.SetDisplayObject(Find("LogoView"));
        
        _logoSelectBtn.onClick.Add(OnShowLogos);

        _createBtn.onClick.Add(OnCreate);

        _logoIconId = Random.Range(1, 13);
        GuildMarkConfig defGuildMarkConfig = GameConfigMgr.Instance.GetGuildMarkConfig(_logoIconId);
        _logoIcon.sprite = GameResMgr.Instance.LoadGuildIcon(defGuildMarkConfig.Icon);
        ObjectHelper.SetSprite(_logoIcon,_logoIcon.sprite);
        
        _nameInputText.text = LanguageMgr.GetLanguage(5003105);
        _noticeInputText.text = LanguageMgr.GetLanguage(5003148);
    }

    private void OnChooseLogo(GuildMarkConfig config)
    {
        _logoIcon.sprite = GameResMgr.Instance.LoadGuildIcon(config.Icon);
        _logoIconId = config.ID;
        _iconSelectView.Hide();
    }

    private void OnShowLogos()
    {
        _iconSelectView.Show();
    }

    private void OnCreate()
    {
        if (string.IsNullOrWhiteSpace(_nameInput.text))
            return;
        if (HeroDataModel.Instance.mHeroInfoData.mDiamond < GameConst.GuildCreateCost)
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000055));
            return;
        }
        string notice = string.IsNullOrWhiteSpace(_noticeInput.text) ? "" : _noticeInput.text;
        GameNetMgr.Instance.mGameServer.ReqCreateGuild(_nameInput.text, _logoIconId, notice);
    }

    public override void Dispose()
    {
        if (_iconSelectView != null)
        {
            _iconSelectView.Dispose();
            _iconSelectView = null;
        }
        base.Dispose();
    }
}