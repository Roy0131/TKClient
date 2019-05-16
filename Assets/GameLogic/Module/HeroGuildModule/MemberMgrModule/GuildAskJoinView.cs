using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public class GuildAskJoinView : UILoopBaseView<GuildMemberVO>
{
    private GameObject _itemObject;
    private Button _agreeBtn;
    private Button _closeBtn;

    private Text _askJoinCountText;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        InitScrollRect("ScrollView");
        _itemObject = Find("ScrollView/Content/UserItem");
        _agreeBtn = Find<Button>("AgreeBtn");
        _closeBtn = Find<Button>("CloseBtn");
        _askJoinCountText = Find<Text>("AskCountText");

        _agreeBtn.onClick.Add(OnAgreeJoin);
        _closeBtn.onClick.Add(Hide);

        ColliderHelper.SetButtonCollider(_closeBtn.transform);
    }

    private void OnAgreeJoin()
    {
        if (_lstDatas.Count == 0)
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001158));
            return;
        }
        int[] players = new int[_lstDatas.Count];
        for (int i = 0; i < _lstDatas.Count; i++)
            players[i] = _lstDatas[i].mPlayerId;
        GameNetMgr.Instance.mGameServer.ReqAgreeJoinGuild(false, players);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        GuildDataModel.Instance.ReqAskJoinMemberData();
    }

    private void OnShowAskJoinView()
    {
        _lstDatas = GuildDataModel.Instance.mlstAskJoinDatas;
        _loopScrollRect.ClearCells();
        _askJoinCountText.text = _lstDatas.Count.ToString();
        if (_lstDatas.Count == 0)
            return;
        _loopScrollRect.totalCount = _lstDatas.Count;
        _loopScrollRect.RefillCells();
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GuildDataModel.Instance.AddEvent(GuildEvent.AskJoinMemberDataRefresh, OnShowAskJoinView);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GuildDataModel.Instance.RemoveEvent(GuildEvent.AskJoinMemberDataRefresh, OnShowAskJoinView);
    }

    protected override UIBaseView CreateItemView()
    {
        AskJoinMemberItem item = new AskJoinMemberItem();
        item.SetDisplayObject(GameObject.Instantiate(_itemObject));
        return item;
    }
}