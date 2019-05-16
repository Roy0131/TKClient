using UnityEngine.UI;
using Framework.UI;

public class MemberBaseView : UIBaseView
{
    protected Image _userIcon;
    protected Text _levelText;
    protected Text _nameText;
    protected Text _timeText;
    protected Button _iconBtn;
    protected Text _officeText;

    protected GuildMemberVO _vo;
    protected override void ParseComponent()
    {
        base.ParseComponent();
        _userIcon = Find<Image>("UserIcon");
        _levelText = Find<Text>("LevelText");
        _nameText = Find<Text>("Name");
        _iconBtn = Find<Button>("UserIcon");
        _timeText = Find<Text>("TimeText");
        _officeText = Find<Text>("OfficeText");

        _iconBtn.onClick.Add(OnShowPlayerInfo);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _vo = args[0] as GuildMemberVO;
        _levelText.text = _vo.mPlayerLevel.ToString();
        _nameText.text = _vo.mPlayerName;
        _timeText.text = TimeHelper.FormatTimeBySecond(_vo.mLastOnlineTime);
        _officeText.text = _vo.OfficeTitle;
        if (_vo.mIcon > 0)
        {
            _userIcon.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(_vo.mIcon).Icon);
            ObjectHelper.SetSprite(_userIcon,_userIcon.sprite);
        }
        else
            _userIcon.sprite = null;
    }

    protected virtual void OnShowPlayerInfo()
    {
        if (_vo.mPlayerId == HeroDataModel.Instance.mHeroPlayerId)
            return;
        PlayerVO vo = new PlayerVO(_vo.mPlayerId, PlayerInfoType.GuildMember);
        vo.mGuildOfficeType = _vo.mOfficeType;
        PlayerInfoDataModel.Instance.ShowPlayerInfo(vo);
    }
}
