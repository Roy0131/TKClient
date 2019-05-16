using UnityEngine.UI;

public class RecommemdItemView : FriendItemBase
{
    private Button _agreeBtn;
    protected override void ParseComponent()
    {
        base.ParseComponent();

        _agreeBtn = Find<Button>("ApplyBtn");

        _agreeBtn.onClick.Add(OnAddFriend);
    }

    private void OnAddFriend()
    {
        GameNetMgr.Instance.mGameServer.ReqAskFriend(_vo.mPlayerId);
    }
}
