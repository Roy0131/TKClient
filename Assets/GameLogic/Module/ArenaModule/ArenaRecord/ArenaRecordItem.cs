using UnityEngine.UI;
using Msg.ClientMessage;
using Framework.UI;

public class ArenaRecordItem : UIBaseView
{
    private Text _playerNameText;
    private Text _playerLvText;
    private Text _resultText;
    private Text _scoreText;
    private Text _timeText;

    private Button _btnRecord;

    private Image _playerIcon;
    private Button _playerBtn;

    private BattleRecordData _vo;
    private int _targetId;
    protected override void ParseComponent()
    {
        base.ParseComponent();

        _playerNameText = Find<Text>("TextName");
        _playerLvText = Find<Text>("RoleIconback/LevelText");
        _resultText = Find<Text>("TextResult");
        _scoreText = Find<Text>("TextScore/Text");
        _timeText = Find<Text>("TextTime");
        _playerIcon = Find<Image>("RoleIconback/RoleIcon");
        _playerBtn = Find<Button>("RoleIconback");

        _btnRecord = Find<Button>("BtnRecord");
        _btnRecord.onClick.Add(OnPlayBattleVideo);
        _playerBtn.onClick.Add(OnShowPlayerInfo);
    }

    private void OnShowPlayerInfo()
    {
        int playerId;
        if (_vo.AttackerId == HeroDataModel.Instance.mHeroPlayerId)
            playerId = _vo.DefenserId;
        else
            playerId = _vo.AttackerId;
        PlayerInfoDataModel.Instance.ShowPlayerInfo(playerId);
    }

    private void OnPlayBattleVideo()
    {
        GameNetMgr.Instance.mGameServer.ReqBattleRecordData(_vo.RecordId);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _vo = args[0] as BattleRecordData;

        if (_vo.AttackerId == HeroDataModel.Instance.mHeroPlayerId)
        {
            _targetId = _vo.DefenserId;
            _playerNameText.text = _vo.DefenserName;
            _playerLvText.text = _vo.DefenserLevel.ToString();
            if (_vo.DefenserHead > 0)
                _playerIcon.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(_vo.DefenserHead).Icon);
        }
        else
        {
            _targetId = _vo.AttackerId;
            _playerNameText.text = _vo.AttackerName;
            _playerLvText.text = _vo.AttackerLevel.ToString();
            if (_vo.AttackerHead > 0)
            {
                _playerIcon.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(_vo.AttackerHead).Icon);
                ObjectHelper.SetSprite(_playerIcon,_playerIcon.sprite);
            }
        }
        if (HeroDataModel.Instance.mHeroPlayerId == _vo.AttackerId)
        {
            _scoreText.text = "+" + _vo.AddScore;
            _resultText.text = _vo.IsWin ? "<color=#fe7f0a>Win</color>" : "<color=#5065AA>Lose</color>";
        }
        else
        {
            _scoreText.text = "+0";
            _resultText.text = _vo.IsWin ? "<color=#5065AA>Lose</color>" : "<color=#fe7f0a>Win</color>";
        }
        _timeText.text = TimeHelper.FormatTimeByTimeStamp(_vo.RecordTime);
    }
}