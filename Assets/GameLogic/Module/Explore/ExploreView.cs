using Framework.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExploreView : UIBaseView
{
    private bool _isStory;
    private GameObject _itemObj;
    private List<ExploreDataVO> exploreData = new List<ExploreDataVO>();
    private RectTransform _parent;
    private GameObject _onNoTask;
    private Scrollbar _scrollbar;


    protected override void ParseComponent()
    {
        base.ParseComponent();
        _itemObj = Find("Item");
        _parent = Find<RectTransform>("ScrollView/Viewport/Content");
        _onNoTask = Find("NoTask");
        _scrollbar = Find<Scrollbar>("ScrollView/ScrollbarHorizontal");
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        ExploreDataModel.Instance.AddEvent<List<ItemInfo>, int, int>(ExploreEvent.ExploreGetReward, OnReward);
        ExploreDataModel.Instance.AddEvent<int>(ExploreEvent.ExploreRemove, OnExploreRemove);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        ExploreDataModel.Instance.RemoveEvent<List<ItemInfo>, int, int>(ExploreEvent.ExploreGetReward, OnReward);
        ExploreDataModel.Instance.RemoveEvent<int>(ExploreEvent.ExploreRemove, OnExploreRemove);
    }

    private void OnExploreRemove(int id)
    {
        for (int i = 0; i < _lstShowViews.Count; i++)
        {
            if (_lstShowViews[i].mExploreDataVO.mId == id)
                RItemView(_lstShowViews[i]);
        }
        _onNoTask.SetActive(_lstShowViews.Count == 0);
    }

    private void OnReward(List<ItemInfo> listInfo, int id, int stageId)
    {
        if (ExploreDataModel.Instance._isStory)
        {
            List<int> listReward = new List<int>();
            listReward = ExploreDataModel.Instance.GetExploreDataStory(id).mConstReward;
            List<ItemInfo> listItemInfo = new List<ItemInfo>();
            ItemInfo info;
            for (int i = 0; i < listReward.Count; i += 2)
            {
                info = new ItemInfo();
                info.Id = listReward[i];
                info.Value = listReward[i + 1];
                listItemInfo.Add(info);
            }
            GetItemTipMgr.Instance.ShowItemResult(listItemInfo);
        }
        else
        {
            GetItemTipMgr.Instance.ShowItemResult(listInfo);
        }
    }

    private void OnExploreData()
    {
        if (_isStory)
            exploreData = ExploreDataModel.Instance.exploreDataStory;
        else
            exploreData = ExploreDataModel.Instance.exploreData;
        _onNoTask.SetActive(exploreData.Count == 0);
        if (exploreData.Count == 0)
            return;
        exploreData.Sort(OnSort);
        RemoveItemView();
        for (int i = 0; i < exploreData.Count; i++)
            CreateNewItemView(exploreData[i], _isStory);
    }

    protected Queue<ExploreItemView> _uiViewPools = new Queue<ExploreItemView>();
    protected List<ExploreItemView> _lstShowViews = new List<ExploreItemView>();

    public ExploreItemView CreateNewItemView(ExploreDataVO vo, bool isStory)
    {
        ExploreItemView view;
        if (_uiViewPools.Count > 0)
            view = _uiViewPools.Dequeue();
        else
        {
            view = new ExploreItemView();
            view.SetDisplayObject(GameObject.Instantiate(_itemObj));
        }
        view.mRectTransform.SetParent(_parent, false);
        view.Show(vo, isStory);
        _lstShowViews.Add(view);
        return view;
    }

    public void RItemView(ExploreItemView view)
    {
        _uiViewPools.Enqueue(view);
        view.Hide();
        view.mRectTransform.SetParent(mRectTransform, false);
        _lstShowViews.Remove(view);
    }

    public void RemoveItemView()
    {
        for (int i = _lstShowViews.Count - 1; i >= 0; i--)
            RItemView(_lstShowViews[i]);
    }

    private int OnSort(ExploreDataVO v1, ExploreDataVO v2)
    {
        if (v1.mState != v2.mState)
        {
            if (v1.mState == 1)
                return 1;
            else if (v2.mState == 1)
                return -1;
            else
                return v1.mState > v2.mState ? -1 : 1;
        }
        else
        {
            return v1.mTaskId < v2.mTaskId ? -1 : 1;
        }
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        RemoveItemView();
        _isStory = ExploreDataModel.Instance._isStory;
        OnExploreData();
    }

    public override void Hide()
    {
        _scrollbar.value = 0;
        base.Hide();
    }

    public override void Dispose()
    {
        RemoveItemView();
        if (_uiViewPools != null && _uiViewPools.Count > 0)
        {
            while (_uiViewPools.Count > 0)
                _uiViewPools.Dequeue().Dispose();
            _uiViewPools.Clear();
            _uiViewPools = null;
        }
        base.Dispose();
    }
}
