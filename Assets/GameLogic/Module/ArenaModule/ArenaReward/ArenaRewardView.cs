using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Framework.UI;

public class ArenaRewardView : UIBaseView
{
    private Toggle _dailyTog;
    private Toggle _seasonTog;
    private Toggle _rankTog;
    private Button _closeBtn;
    private RectTransform _rectRewardRoot;
    private GameObject _rewardItemObj;

    private Queue<ArenaRewardItem> _rewardPools;
    private RectTransform _rewardPoolRoot;

    private ArenaRewardType _rewardType = ArenaRewardType.None;

    private Transform _root;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _root = Find<Transform>("Root");
        _dailyTog = Find<Toggle>("Root/ToggleGroup/DailyReward");
        _seasonTog = Find<Toggle>("Root/ToggleGroup/SeasonReward");
        _rankTog = Find<Toggle>("Root/ToggleGroup/RankReward");
        _rectRewardRoot = Find<RectTransform>("Root/ScrollView/Content");
        _rewardItemObj = Find("Root/RewardItem");
        _rewardPoolRoot = Find<RectTransform>("Root/ItemPoolRoot");

        _closeBtn = Find<Button>("Root/BtnClose");
        ColliderHelper.SetButtonCollider(_closeBtn.transform);
        _closeBtn.onClick.Add(Hide);

        _rewardPools = new Queue<ArenaRewardItem>();
        _dailyTog.onValueChanged.Add((bool value) => { if (value) OnRewardTypeChange(ArenaRewardType.DailyReward); });
        _seasonTog.onValueChanged.Add((bool value) => { if (value) OnRewardTypeChange(ArenaRewardType.SeasonReward); });
        _rankTog.onValueChanged.Add((bool value) => { if (value) OnRewardTypeChange(ArenaRewardType.RankReward); });
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        if (_rewardType == ArenaRewardType.None)
            OnRewardTypeChange(ArenaRewardType.DailyReward);
        else
            OnRewardTypeChange(_rewardType);
    }

    private void OnRewardTypeChange(ArenaRewardType type)
    {
        _rewardType = type;
        ArenaRewardItem view;
        ClearRewardItem();
        int idx = 0;
        if (_rewardType == ArenaRewardType.DailyReward)
        {
            view = GetRewardItem();
            view.SetRewardType(_rewardType, idx);
            idx = 1;
            view.Show(ArenaDataModel.Instance.GetSelfDailyRewardVO(ArenaRewardType.DailyReward));
            AddChildren(view);
        }
        List<ArenaRewardVO> lst = ArenaDataModel.Instance.GetArenaReward(type);
        int i = 0;
        for (i = 0; i < lst.Count; i++)
        {
            view = GetRewardItem();
            view.SetRewardType(_rewardType, idx);
            view.Show(lst[i]);
            AddChildren(view);
            idx++;
        }

        _rectRewardRoot.anchoredPosition = new Vector2(0, 0);
    }

    private ArenaRewardItem GetRewardItem()
    {
        ArenaRewardItem view;
        if (_rewardPools.Count > 0)
        {
            view = _rewardPools.Dequeue();
        }
        else
        {
            view = new ArenaRewardItem();
            view.SetDisplayObject(GameObject.Instantiate(_rewardItemObj));
        }
        view.mRectTransform.SetParent(_rectRewardRoot, false);
        return view;
    }

    private void ClearRewardItem()
    {
        if (_childrenViews.Count > 0)
        {
            ArenaRewardItem item;
            for (int i = _childrenViews.Count - 1; i >= 0; i--)
            {
                item = _childrenViews[i] as ArenaRewardItem;
                item.Hide();
                item.mRectTransform.SetParent(_rewardPoolRoot, false);
                _rewardPools.Enqueue(item);
            }
            _childrenViews.Clear();
        }
    }

    public override void Dispose()
    {
        ClearRewardItem();
        if (_rewardPools != null)
        {
            while (_rewardPools.Count > 0)
                _rewardPools.Dequeue().Dispose();
            _rewardPools.Clear();
            _rewardPools = null;
        }
        base.Dispose();
    }
    protected override void OnShowViewAnimation()
    {
        base.OnShowViewAnimation();
        ObjectHelper.PopAnimationLiner(_root);
    }
}