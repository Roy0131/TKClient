using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public class ActivityCopyFriendView : UILoopBaseView<CardDataVO>
{
    private ActiveStageConfig _activeStageCfg;
    private Text _tips;
    private Button _btnBattle;
    private GameObject _rankItem;
    private int _playerId;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        InitScrollRect("ScrollView");
        _tips = Find<Text>("Tips");
        _btnBattle = Find<Button>("ButtonBattle");
        _rankItem = Find("RankItem");

        _btnBattle.onClick.Add(CloseBattle);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        ActivityCopyDataModel.Instance.AddEvent<List<CardDataVO>>(ActivityCopyEvent.FriendData, OnFriendData);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<int>(ActivityCopyEvent.FriendSele, OnFriendSele);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        ActivityCopyDataModel.Instance.RemoveEvent<List<CardDataVO>>(ActivityCopyEvent.FriendData, OnFriendData);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<int>(ActivityCopyEvent.FriendSele, OnFriendSele);
    }

    private void OnFriendSele(int id)
    {
        _playerId = id;
        for (int i = 0; i < _lstShowViews.Count; i++)
            (_lstShowViews[i] as ActivityCopyFriendItemView).BlSelected = (_lstShowViews[i] as ActivityCopyFriendItemView)._vo.mPlayerId == id;
    }

    private void OnFriendData(List<CardDataVO> listVO)
    {
        _loopScrollRect.ClearCells();
        _lstDatas = listVO;
        _tips.gameObject.SetActive(_lstDatas.Count == 0);
        if (_lstDatas.Count == 0)
            return;
        _lstDatas.Sort(SortCard);
        _playerId = _lstDatas[0].mPlayerId;
        _loopScrollRect.totalCount = _lstDatas.Count;
        _loopScrollRect.RefillCells();
    }

    private int SortCard(CardDataVO v1,CardDataVO v2)
    {
        return v1.mCardLevel > v2.mCardLevel ? -1 : 1;
    }

    protected override UIBaseView CreateItemView()
    {
        ActivityCopyFriendItemView item = new ActivityCopyFriendItemView();
        item.SetDisplayObject(GameObject.Instantiate(_rankItem));
        return item;
    }

    protected override void SetItemData(UIBaseView view, int idx)
    {
        base.SetItemData(view, idx);
        (view as ActivityCopyFriendItemView).BlSelected = _lstDatas[idx].mPlayerId == _playerId;
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        ActivityCopyDataModel.Instance.ReqFriendData();
        _activeStageCfg = args[0] as ActiveStageConfig;
    }

    private void CloseBattle()
    {
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(ActivityCopyEvent.ActivityCopyFriendClose);
        ActivityCopyDataModel.Instance.RefreshStageId(_activeStageCfg.StageID);
        LineupSceneMgr.Instance.ShowLineupModule(TeamType.ActiveCopy, _activeStageCfg.ID, _activeStageCfg.StageID);
    }
}