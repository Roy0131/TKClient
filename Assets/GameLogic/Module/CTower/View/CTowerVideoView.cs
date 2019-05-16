using Framework.UI;
using Msg.ClientMessage;
using UnityEngine.UI;

public class CTowerVideoView : UIBaseView
{
    private Text _name;
    private Image _roleIcon;
    private Text _level;
    private Button _btnVideo;

    private TowerFightRecord _towerFightRecord;
    

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _name = Find<Text>("TextName");
        _roleIcon = Find<Image>("ImageRoleBack/ImageRole");
        _level = Find<Text>("ImageLevelBack/TextLevel");
        _btnVideo = Find<Button>("ButtonVideo");

        _btnVideo.onClick.Add(GoVideo);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        CTowerDataModel.Instance.AddEvent(CTowerEvent.RefreshTowerRecordData, GoVideo);
    }
    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        CTowerDataModel.Instance.RemoveEvent(CTowerEvent.RefreshTowerRecordData, GoVideo);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _towerFightRecord = args[0] as TowerFightRecord;

        _name.text = _towerFightRecord.AttackerName;//角色名字
        _level.text = _towerFightRecord.AttackerLevel.ToString();
        int _headID = _towerFightRecord.AttackerHead;
        if (_headID != 0)
        {
            _roleIcon.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(_headID).Icon);
            ObjectHelper.SetSprite(_roleIcon, _roleIcon.sprite);
        }
        else
        {
            _roleIcon.sprite = null;
        }
    }
    
    private void GoVideo()
    {
        CTowerDataModel.Instance.ReqTowerRecordData(_towerFightRecord.TowerFightId);
        LogHelper.Log("打开录像回放");
    }

}
