using Framework.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecommemdFriendView : UILoopBaseView<FriendDataVO>
{
    private Button _addBtn;
    private InputField _inputText;
    private GameObject _itemObject;
    private Text _placeHolder;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _inputText = Find<InputField>("InputField");
        _addBtn = Find<Button>("ApplyBtn");
        _itemObject = Find("ScrollView/Content/Item");
        _itemObject.SetActive(false);

        _placeHolder = Find<Text>("InputField/Placeholder");
        InitScrollRect("ScrollView");

        _addBtn.onClick.Add(OnAddFriend);
    }

    private void OnAddFriend()
    {
        if (string.IsNullOrWhiteSpace(_inputText.text))
            return;
        GameNetMgr.Instance.mGameServer.ReqAskFriend(int.Parse(_inputText.text));
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        FriendDataModel.Instance.AddEvent(FriendEvent.RecommemdFriendRefresh, OnShowRecommemd);
        FriendDataModel.Instance.AddEvent<List<int>>(FriendEvent.AddFriend, OnAddSuccess);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        FriendDataModel.Instance.RemoveEvent(FriendEvent.RecommemdFriendRefresh, OnShowRecommemd);
        FriendDataModel.Instance.RemoveEvent<List<int>>(FriendEvent.AddFriend, OnAddSuccess);
    }

    private void OnAddSuccess(List<int> listId)
    {
       
        if (_inputText.text != "" && listId.Count > 0)
        {
            if (listId.Contains(int.Parse(_inputText.text)))
            {
                PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000104));
                _inputText.text = "";
            }
        }
        else
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000104));
        }
    }

    private void OnShowRecommemd()
    {
        _placeHolder.text = LanguageMgr.GetLanguage(6001255);
        _lstDatas = FriendDataModel.Instance.mlstRecommendFriends;
        _loopScrollRect.ClearCells();
        if (_lstDatas.Count == 0)
            return;
        _loopScrollRect.totalCount = _lstDatas.Count;
        _loopScrollRect.RefillCells();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        OnShowRecommemd();
    }

    protected override UIBaseView CreateItemView()
    {
        RecommemdItemView item = new RecommemdItemView();
        item.SetDisplayObject(GameObject.Instantiate(_itemObject));
        return item;
    }
}
