using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class ServerItemView : UIBaseView
{
    private ServerDataVO _curServerDataVO;
    private Text _serverName;
    private Text _playerName;
    private Text _playerLevel;
    private Image _playerHead;
    private Button _seleBtn;
    private GameObject _KindObj;
    private GameObject _img;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _serverName = Find<Text>("ServerName");
        _playerName = Find<Text>("Kind/PlayerName");
        _playerLevel = Find<Text>("Kind/LevelImg/Level");
        _playerHead = Find<Image>("Kind/PlayerHead");
        _seleBtn = Find<Button>("ImgSele");
        _KindObj = Find("Kind");
        _img = Find("Img");

        _seleBtn.onClick.Add(OnSeleServer);
    }
    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _curServerDataVO = args[0] as ServerDataVO;

        _serverName.text = _curServerDataVO.mServerName;
        _KindObj.SetActive(_curServerDataVO.mPlayerLevel != 0);
        _seleBtn.gameObject.SetActive(_curServerDataVO.mServerId != LoginHelper.ServerID);
        _img.SetActive(_curServerDataVO.mServerId == LoginHelper.ServerID);
        if (_curServerDataVO.mPlayerLevel != 0)
        {
            _playerName.text = _curServerDataVO.mPlayerName;
            _playerLevel.text = _curServerDataVO.mPlayerLevel.ToString();
            _playerHead.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(_curServerDataVO.mPlayerHead).Icon);
            ObjectHelper.SetSprite(_playerHead, _playerHead.sprite);
        }
    }

    private void OnSeleServer()
    {
        if (_curServerDataVO.mServerId != LoginHelper.ServerID)
            ConfirmTipsMgr.Instance.ShowConfirmTips(LanguageMgr.GetLanguage(6001226), AlertBack);
    }

    private void AlertBack(bool result, bool blShowAgain)
    {
        if (result)
            LoginHelper.ChangeServer(ServerDataModel.Instance.mAcc, ServerDataModel.Instance.mToken, _curServerDataVO.mServerId, _curServerDataVO.mServerIp);
    }
}
