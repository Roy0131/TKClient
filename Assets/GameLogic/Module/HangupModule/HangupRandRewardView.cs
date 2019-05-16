using Framework.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HangupRandRewardView : UIBaseView
{
    private Button _disbtn;

    private RectTransform _parent;

    protected override void ParseComponent()
    {
        base.ParseComponent();

        _disbtn = Find<Button>("HangupRandObj/DisBtn");
        _parent = Find<RectTransform>("HangupRandObj/Panel_Scroll/KnapsackPanel");

        _disbtn.onClick.Add(OnDis);
    }
    
    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        IList<ItemInfo> rewards = args[0] as IList<ItemInfo>;
        DiposeChildren();
        ItemView view;
        _childrenViews = new List<UIBaseView>();
        for (int i = 0; i < rewards.Count; i++)
        {
            if (GameConfigMgr.Instance.GetItemConfig(rewards[i].Id).ItemType == 2)
                view = ItemFactory.Instance.CreateItemView(rewards[i], ItemViewType.EquipRewardItem);
            else
                view = ItemFactory.Instance.CreateItemView(rewards[i], ItemViewType.RewardItem);
            view.mRectTransform.SetParent(_parent, false);
            AddChildren(view);
        }
    }
    protected override void DiposeChildren()
    {
        if (_childrenViews != null)
        {
            for (int i = _childrenViews.Count - 1; i >= 0; i--)
                ItemFactory.Instance.ReturnItemView(_childrenViews[i] as ItemView);
            _childrenViews.Clear();
            _childrenViews = null;
        }
    }
    
    private void OnDis()
    {
        //关闭随机奖励面板
        Hide();
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
