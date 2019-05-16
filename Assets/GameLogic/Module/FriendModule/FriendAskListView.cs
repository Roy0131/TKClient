using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class FriendAskListView : UILoopBaseView<FriendDataVO>
{
    private Button _delAllBtn;
    private GameObject _itemObject;
    private Text _countText;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _delAllBtn = Find<Button>("DeleteAllBtn");
        _itemObject = Find("ScrollView/Content/Item");
        _countText = Find<Text>("CountText");

        InitScrollRect("ScrollView");

        _delAllBtn.onClick.Add(OnDelAllAsk);
    }

    private void OnDelAllAsk()
    {
        if (_lstDatas.Count <= 0)
            return;
        int[] players = new int[_lstDatas.Count];
        for (int i = 0; i < _lstDatas.Count; i++)
            players[i] = _lstDatas[i].mPlayerId;
        GameNetMgr.Instance.mGameServer.ReqRefuseFriend(players);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        FriendDataModel.Instance.AddEvent(FriendEvent.FriendAskPlayerListRefresh, OnShowFriend);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        FriendDataModel.Instance.RemoveEvent(FriendEvent.FriendAskPlayerListRefresh, OnShowFriend);
    }

    private void OnShowFriend()
    {
        RefreshLoopView(FriendDataModel.Instance.mlstFriendAskPlayers);
        _delAllBtn.gameObject.SetActive(_lstDatas.Count > 0);
        _countText.text = _lstDatas.Count.ToString();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        OnShowFriend();
    }

    protected override UIBaseView CreateItemView()
    {
        AskFriendItemView item = new AskFriendItemView();
        item.SetDisplayObject(GameObject.Instantiate(_itemObject));
        return item;
    }
}