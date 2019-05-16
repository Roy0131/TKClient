using Framework.UI;
using Msg.ClientMessage;
using UnityEngine.UI;

public class CTowerRankItemView : UIBaseView
{
    private TowerRankInfo _towerRankInfo;
    private int _rankNum;

    private Text _rankNumText;
    private Image _roleIcon;

    private Text _levelText;
    private Text _floorID;
    private Text _playerName;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _rankNumText = Find<Text>("ImageRankBack/TextRankNum");
        _levelText = Find<Text>("ImageLevelBack/TextLevel");
        _floorID = Find<Text>("TextFloor/TextFloorNum");
        _roleIcon = Find<Image>("ImageIcon");
        _playerName = Find<Text>("TextName");
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _towerRankInfo = args[0] as TowerRankInfo;
        _rankNum = int.Parse(args[1].ToString());
        RankNumDIs();
    }

    private void RankNumDIs()
    {
        _rankNumText.text =(_rankNum + 1).ToString();
        _floorID.text = _towerRankInfo.TowerId.ToString();
        _playerName.text = _towerRankInfo.PlayerName;
        _levelText.text = _towerRankInfo.PlayerLevel.ToString();
        if (_towerRankInfo.PlayerHead > 0)
        {
            _roleIcon.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(_towerRankInfo.PlayerHead).Icon);
            ObjectHelper.SetSprite(_roleIcon,_roleIcon.sprite);
        }
        else
            _roleIcon.sprite = null;
    }
}
