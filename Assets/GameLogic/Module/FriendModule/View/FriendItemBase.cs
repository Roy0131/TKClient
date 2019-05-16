
using Framework.UI;
using UnityEngine.UI;

public class FriendItemBase : UIBaseView
{
    protected Text _lvText;
    protected Text _nameText;
    protected Image _iconImage;
    protected Button _iconBtn;
    protected FriendDataVO _vo;

    protected override void ParseComponent()
    {
        base.ParseComponent();

        _lvText = Find<Text>("InfoRoot/TextLevel");
        _nameText = Find<Text>("InfoRoot/TextName");
        _iconImage = Find<Image>("InfoRoot/Icon");
        _iconBtn = Find<Button>("InfoRoot/Icon");

        _iconBtn.onClick.Add(OnShowPlayerInfo);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _vo = args[0] as FriendDataVO;
        _lvText.text = _vo.mPlayerLevel.ToString();
        _nameText.text = _vo.mPlayerName;
        if (_vo.mPlayerIcon > 0)
        {
            _iconImage.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(_vo.mPlayerIcon).Icon);
            ObjectHelper.SetSprite(_iconImage,_iconImage.sprite);
        }
        else
            _iconImage.sprite = null;
    }

    protected virtual void OnShowPlayerInfo()
    {
        PlayerInfoDataModel.Instance.ShowPlayerInfo(_vo.mPlayerId, PlayerInfoType.FriendOther);
    }

    public FriendDataVO FriendData
    {
        get { return _vo; }
    }
}