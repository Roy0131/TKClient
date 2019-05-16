using UnityEngine.UI;

public class FriendListItemView : FriendItemBase
{
    private Text _timeText;
    private Button _bossBtn;
    private Button _battleBtn;
    private Button _getPointBtn;

    private Button _sendPointBtn;

    protected override void ParseComponent()
    {
        base.ParseComponent();

        _timeText = Find<Text>("TextTime");
        _bossBtn = Find<Button>("Buttons/BossBtn");
        _battleBtn = Find<Button>("Buttons/VSBtn");
        _getPointBtn = Find<Button>("Buttons/GetBtn");
        _sendPointBtn = Find<Button>("Buttons/SendBtn");

        _bossBtn.onClick.Add(OnBattleBoss);
        _battleBtn.onClick.Add(OnBattleFriend);
        _getPointBtn.onClick.Add(OnGetPoint);
        _sendPointBtn.onClick.Add(OnSendPoint);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        if (_vo.mBlOnline)
            _timeText.text = LanguageMgr.GetLanguage(5001540);//"Online";
        else
            _timeText.text = TimeHelper.FormatTimeBySecond(_vo.mOfflineTime);
        RefreshFriendBossStatus();
        RefreshPointsStatus();
    }

    public void RefreshPointsStatus()
    {
        _sendPointBtn.interactable = _vo.mGivePointsRemainTime <= 0;
        _getPointBtn.gameObject.SetActive(true);
        if (_vo.mGetPoints == -1)
            _getPointBtn.interactable = false;
        else if (_vo.mGetPoints == 0)
            _getPointBtn.gameObject.SetActive(false);
        else
            _getPointBtn.interactable = true;
    }

    public void RefreshFriendBossStatus()
    {
        _bossBtn.gameObject.SetActive(_vo.mBossId > 0);
    }

    private void OnBattleBoss()
    {
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(FriendEvent.ChallengeFriendBoss, _vo.mPlayerId);
    }

    private void OnBattleFriend()
    {
        LineupSceneMgr.Instance.ShowLineupModule(TeamType.FriendBattle, _vo.mPlayerId);
    }

    private void OnGetPoint()
    {
        if (_vo.mGetPoints > 0)
            GameNetMgr.Instance.mGameServer.ReqGetPointsFromFriend(_vo.mPlayerId);
    }

    private void OnSendPoint()
    {
        if (_vo.mGivePointsRemainTime > 0)
            return;
        GameNetMgr.Instance.mGameServer.ReqGivePointsToFriend(_vo.mPlayerId);
    }

    protected override void OnShowPlayerInfo()
    {
        PlayerInfoDataModel.Instance.ShowPlayerInfo(_vo.mPlayerId, PlayerInfoType.FriendPlayer);
    }
}
