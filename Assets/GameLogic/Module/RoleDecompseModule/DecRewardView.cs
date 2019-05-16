using Framework.UI;
using Msg.ClientMessage;
using NewBieGuide;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecRewardView : UIBaseView
{
    private List<ItemView> _lstItems;
    private RectTransform _rewardRoot;
    private Button _button;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _rewardRoot = Find<RectTransform>("ScrollView/Content");
        _button = Find<Button>("OkBtn");

        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.DisassembleOk, _button.transform);

        _button.onClick.Add(Hide);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.EndCondTrigger, NewBieGuide.EndConditionConst.DeComposeReward);
        ClearAllItem();
        Dictionary<int, ItemInfo> dictReward = DecomposeDataModel.Instance.mDictReward;
        ItemView view;
        _lstItems = new List<ItemView>();
        foreach (var kv in dictReward)
        {
            view = ItemFactory.Instance.CreateItemView(kv.Value, ItemViewType.BagItem, null);
            view.mRectTransform.SetParent(_rewardRoot, false);
            _lstItems.Add(view);
        }
    }

    private void ClearAllItem()
    {
        if (_lstItems == null)
            return;
        for (int i = _lstItems.Count - 1; i >= 0; i--)
            ItemFactory.Instance.ReturnItemView(_lstItems[i]);
        _lstItems.Clear();
        _lstItems = null;
    }

    public override void Dispose()
    {
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.DisassembleOk);
        ClearAllItem();
        base.Dispose();
    }
}