using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : UILoopBaseView<int>
{
    private Text _playerName;
    private Text _playerID;
    private Text _playerLV;
    private Text _guildName;
    private Text _curExp;
    private Text _curVIP;
    private Text _amendNum;
    private Image _avatarIcon;
    private Image _expImg;
    private Image _guildIcon;
    private Button _amendName;
    private Button _jumpVIP;
    private Button _avatarBtn;
    private Button _avatarDisBtn;
    private Button _playerNameDisBtn;
    private Button _conName;
    private GameObject _avatar;
    private GameObject _avatarObj;
    private GameObject _PlayerNameObj;
    private InputField _inputField;
    private Text _inputText;
    private GameObject _guild;
    private string _playerNames;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _playerName = Find<Text>("Info/NameImg/Name");
        _playerID = Find<Text>("Info/PlayerID");
        _playerLV = Find<Text>("Info/Rank");
        _guildName = Find<Text>("Info/Guild/GuildName");
        _curExp = Find<Text>("Info/EXP/FillImg/EXPNum");
        _curVIP = Find<Text>("Info/VIP/VIPNum");
        _amendNum = Find<Text>("Name/BackImg/ConBtn/Amend");
        _inputText = Find<Text>("Name/BackImg/InputField/Placeholder");
        _avatarIcon = Find<Image>("Info/Avatar/AmendAvatar");
        _expImg = Find<Image>("Info/EXP/FillImg/Fill");
        _guildIcon = Find<Image>("Info/Guild/Imageback/Image");
        _amendName = Find<Button>("Info/AmendName");
        _jumpVIP = Find<Button>("Info/VIP");
        _avatarBtn = Find<Button>("Info/Avatar/AmendAvatar");
        _avatarDisBtn = Find<Button>("Avatar/Btn_Back");
        _playerNameDisBtn = Find<Button>("Name/Btn_Back");
        _conName = Find<Button>("Name/BackImg/ConBtn");
        _inputField = Find<InputField>("Name/BackImg/InputField");
        _avatarObj = Find("Avatar/Image/Avatar");
        _avatar = Find("Avatar");
        _PlayerNameObj = Find("Name");
        _guild = Find("Info/Guild");

        InitScrollRect("Avatar/Image/Panel_Scroll");

        _amendName.onClick.Add(OnAmendName);
        _jumpVIP.onClick.Add(OnJumpPrivileged);
        _avatarBtn.onClick.Add(OnAvatar);
        _avatarDisBtn.onClick.Add(OnAvatarDis);
        _playerNameDisBtn.onClick.Add(OnPlayerNameDis);
        _conName.onClick.Add(OnConName);

        _inputText.text = LanguageMgr.GetLanguage(5002421);

        _inputField.onValueChanged.Add(delegate { OnDele(); });

        ColliderHelper.SetButtonCollider(_amendName.transform, 80, 80);
        ColliderHelper.SetButtonCollider(_playerNameDisBtn.transform);
        ColliderHelper.SetButtonCollider(_avatarDisBtn.transform, 120, 120);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        HeroDataModel.Instance.AddEvent(HeroEvent.AmendName, OnName);
        HeroDataModel.Instance.AddEvent(HeroEvent.AmendHead, OnHead);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        HeroDataModel.Instance.RemoveEvent(HeroEvent.AmendName, OnName);
        HeroDataModel.Instance.RemoveEvent(HeroEvent.AmendHead, OnHead);
    }

    private void OnHead()
    {
        _avatar.SetActive(false);
        _avatarIcon.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(HeroDataModel.Instance.mHeroInfoData.mIcon).Icon);
        ObjectHelper.SetSprite(_avatarIcon,_avatarIcon.sprite);
    }

    private void OnName()
    {
        _inputField.text = "";
        _PlayerNameObj.SetActive(false);
        _playerName.text = HeroDataModel.Instance.mHeroInfoData.mHeroName;
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _inputField.text = "";
        OnPlayerChang();
    }

    private void OnPlayerChang()
    {
        _guild.gameObject.SetActive(HeroDataModel.Instance.mHeroInfoData.mGuildId > 0);
        _playerName.text = HeroDataModel.Instance.mHeroInfoData.mHeroName;
        _playerID.text = HeroDataModel.Instance.mHeroPlayerId.ToString();
        _playerLV.text = HeroDataModel.Instance.mHeroInfoData.mLevel.ToString();
        _guildName.text = HeroDataModel.Instance.mHeroInfoData.mGuildName;
        if (HeroDataModel.Instance.mHeroInfoData.mGuildLogo > 0)
        {
            _guildIcon.sprite = GameResMgr.Instance.LoadGuildIcon(GameConfigMgr.Instance.GetGuildMarkConfig(HeroDataModel.Instance.mHeroInfoData.mGuildLogo).Icon);
            ObjectHelper.SetSprite(_guildIcon,_guildIcon.sprite);
        }
        if (HeroDataModel.Instance.mHeroInfoData.mLevel >= 200)
            _curExp.text = "Max";
        else
            _curExp.text = HeroDataModel.Instance.mHeroInfoData.mExp + "/" + HeroDataModel.Instance.mHeroInfoData.mExpMax;
        _curVIP.text = "VIP " + HeroDataModel.Instance.mHeroInfoData.mVipLevel.ToString();
        _expImg.fillAmount = (float)HeroDataModel.Instance.mHeroInfoData.mExp / (float)HeroDataModel.Instance.mHeroInfoData.mExpMax;
        if (HeroDataModel.Instance.mHeroInfoData.mIcon > 0)
        {
            _avatarIcon.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(HeroDataModel.Instance.mHeroInfoData.mIcon).Icon);
            ObjectHelper.SetSprite(_avatarIcon,_avatarIcon.sprite);
        }
        if (HeroDataModel.Instance.mHeroInfoData.mDiamond >= GameConst.PlayerNameNum)
            _amendNum.text = GameConst.PlayerNameNum.ToString();
        else
            _amendNum.text = "<color=#FF0000>" + GameConst.PlayerNameNum + "</color>";
    }

    private void OnDele()
    {
        if (_inputField.text != "" && _inputField != null)
            _playerNames = _inputField.text;
    }

    private void OnPlayerNameDis()
    {
        _inputField.text = "";
        _PlayerNameObj.SetActive(false);
    }

    private void OnAmendName()
    {
        _PlayerNameObj.SetActive(true);
    }

    private void OnConName()
    {
        if (_playerNames != "" && _playerNames != null)
        {
            if (HeroDataModel.Instance.mHeroInfoData.mDiamond >= GameConst.PlayerNameNum && _playerNames != HeroDataModel.Instance.mHeroInfoData.mHeroName
                && TimeHelper.GetLength(_playerNames) >= 4 && TimeHelper.GetLength(_playerNames) <= 14)
            {
                GameNetMgr.Instance.mGameServer.ReqPlayerChangeName(_playerNames);
                TDPostDataMgr.Instance.DoCostDiamond(TDCostDiamondType.BuyPlayerModifyName,1, GameConst.PlayerNameNum);
            }
            else if (HeroDataModel.Instance.mHeroInfoData.mDiamond < GameConst.PlayerNameNum)
                PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000055));
            else if (_playerNames == "")
                PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001174));
            else if (_playerNames == HeroDataModel.Instance.mHeroInfoData.mHeroName)
                PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001175));
            else if (TimeHelper.GetLength(_playerNames) < 4)
                PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001176));
            else if (TimeHelper.GetLength(_playerNames) > 14)
                PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001177));
        }
        else
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001174));
        }
    }

    private void OnJumpPrivileged()
    { 
        GameUIMgr.Instance.OpenModule(ModuleID.Recharge);
    }

    private void OnAvatarDis()
    {
        _avatar.SetActive(false);
    }

    private void OnAvatar()
    {
        //打开头像列表 更换头像
        _avatar.SetActive(true);
        _lstDatas = BagDataModel.Instance.GetAvatarItem();
        _lstDatas.Sort((a, b) => a.CompareTo(b));
        _loopScrollRect.ClearCells();
        if (_lstDatas.Count == 0)
            return;
        _loopScrollRect.totalCount = _lstDatas.Count;
        _loopScrollRect.RefillCells();
    }

    protected override UIBaseView CreateItemView()
    {
        PlayerAvatarItemView item = new PlayerAvatarItemView();
        item.SetDisplayObject(GameObject.Instantiate(_avatarObj));
        return item;
    }
}
