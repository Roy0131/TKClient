using UnityEngine.UI;

public class AskFriendItemView : FriendItemBase
{
    private Button _agreeBtn;
    private Button _delBtn;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _agreeBtn = Find<Button>("AgreeBtn");
        _delBtn = Find<Button>("DeleteBtn");

        _agreeBtn.onClick.Add(OnAgree);
        _delBtn.onClick.Add(OnDelete);
    }

    private void OnAgree()
    {
        GameNetMgr.Instance.mGameServer.ReqAgreeFriend(_vo.mPlayerId);
    }

    private void OnDelete()
    {
        GameNetMgr.Instance.mGameServer.ReqRefuseFriend(_vo.mPlayerId);
    }
}