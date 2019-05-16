using UnityEngine.UI;

public class AskJoinMemberItem : MemberBaseView
{
    private Button _agreeBtn;
    private Button _delBtn;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _timeText.gameObject.SetActive(false);
        _agreeBtn = Find<Button>("AgreeBtn");
        _delBtn = Find<Button>("DelBtn");

        _agreeBtn.onClick.Add(OnAgreeAskJoin);
        _delBtn.onClick.Add(OnDelAskJoin);
    }

    private void OnAgreeAskJoin()
    {
        GameNetMgr.Instance.mGameServer.ReqAgreeJoinGuild(false, _vo.mPlayerId);
    }

    private void OnDelAskJoin()
    {
        GameNetMgr.Instance.mGameServer.ReqAgreeJoinGuild(true, _vo.mPlayerId);
    }

    protected override void OnShowPlayerInfo()
    {
        
    }
}