using Msg.ClientMessage;
using UnityEngine.UI;
using UnityEngine;
using Framework.UI;

public class ArenaUserItem : UIBaseView
{
    private Text _rankLevelText;
    private Image _userIcon;
    private Text _battlePowerText;
    private Text _nameText;
    private Text _scoreText;
    private Text _playerLvText;
    private Text _rankNameText;
    private Image _rankIcon;
    private RankItemInfo _vo;

    private Button _playerIconBtn;
    private GameObject _firstObject;
    private GameObject _secondObject;
    private GameObject _thirdObject;
    private GameObject _normalObject;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _firstObject = Find("FirstRoot");
        _secondObject = Find("SecondRoot");
        _thirdObject = Find("ThirdRoot");
        _normalObject = Find("NormalRoot");
        _rankLevelText = Find<Text>("NormalRoot/RankText");
        _userIcon = Find<Image>("PlayerIconback/PlayerIcon");
        _battlePowerText = Find<Text>("Textbattle");
        _nameText = Find<Text>("TextName");
        _scoreText = Find<Text>("ScoreText");
        _playerLvText = Find<Text>("PlayerIconback/PlayerLevelText");
        _rankIcon = Find<Image>("RankIcon");
        _rankNameText = Find<Text>("RankIcon/RankNameText");
        _playerIconBtn = Find<Button>("PlayerIconback/PlayerIcon");
        _playerIconBtn.onClick.Add(OnShowPlayerInfo);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _vo = args[0] as RankItemInfo;
        _firstObject.SetActive(false);
        _secondObject.SetActive(false);
        _thirdObject.SetActive(false);
        _normalObject.SetActive(false);
        if (_vo.Rank == 1)
            _firstObject.SetActive(true);
        else if (_vo.Rank == 2)
            _secondObject.SetActive(true);
        else if (_vo.Rank == 3)
            _thirdObject.SetActive(true);
        else
            _normalObject.SetActive(true);
        _rankLevelText.text = _vo.Rank.ToString();
        _battlePowerText.text = _vo.PlayerPower.ToString();
        _nameText.text = _vo.PlayerName;
        _scoreText.text = _vo.PlayerArenaScore.ToString();
        _playerLvText.text = _vo.PlayerLevel.ToString();
        
        ArenaDivisionConfig config = GameConfigMgr.Instance.GetArenaDivisionConfig(_vo.PlayerArenaGrade);
        if (config != null)
            _rankNameText.text = LanguageMgr.GetLanguage(config.Name);
        mDisplayObject.name = _vo.Rank.ToString();
        if (_vo.PlayerHead > 0)
        {
            _userIcon.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(_vo.PlayerHead).Icon);
            ObjectHelper.SetSprite(_userIcon, _userIcon.sprite);
        }
        else
        {
            _userIcon.sprite = null;
            ObjectHelper.SetSprite(_userIcon,_userIcon.sprite);
        }
        _rankIcon.sprite = GameResMgr.Instance.LoadItemIcon(config.Icon);
        ObjectHelper.SetSprite(_rankIcon,_rankIcon.sprite);
    }

    private void OnShowPlayerInfo()
    {
        if (_vo.PlayerId == HeroDataModel.Instance.mHeroPlayerId)
            return;
        PlayerInfoDataModel.Instance.ShowPlayerInfo(_vo.PlayerId);
    }
}