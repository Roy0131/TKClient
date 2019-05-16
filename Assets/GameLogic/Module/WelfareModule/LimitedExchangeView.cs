using Framework.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//兑换
public class LimitedExchangeView : UIBaseView
{
    private Text _num;
    private Button _buy;
    private RectTransform _itemObj1;
    private RectTransform _itemObj2;
    private ItemView _view;


    protected override void ParseComponent()
    {
        base.ParseComponent();
        _num = Find<Text>("Num");
        _buy = Find<Button>("Buy");
        _itemObj1 = Find<RectTransform>("ItemObj1");
        _itemObj2 = Find<RectTransform>("ItemObj2");

        _buy.onClick.Add(OnBuy);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        LimitedItemDataVO limitedItemDataVO = args[0] as LimitedItemDataVO;
        int eventId = int.Parse(args[1].ToString());
        SubActiveConfig cfg = GameConfigMgr.Instance.GetSubActiveConfig(limitedItemDataVO.mSubActiveID);
        string[] rewards = cfg.Reward.Split(',');
        if (rewards.Length % 2 != 0)
            return;
        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        for (int i = 0; i < rewards.Length; i += 2)
        {
            ItemInfo itemInfo = new ItemInfo();
            itemInfo.Id = int.Parse(rewards[i]);
            itemInfo.Value = int.Parse(rewards[i + 1]);
            if (GameConfigMgr.Instance.GetItemConfig(itemInfo.Id).ItemType == 2)
                _view = ItemFactory.Instance.CreateItemView(itemInfo, ItemViewType.EquipHeroItem);
            else
                _view = ItemFactory.Instance.CreateItemView(itemInfo, ItemViewType.HeroItem);
            _view.mRectTransform.SetParent(_itemObj2, false);
        }
        List<ItemInfo> listInfo = new List<ItemInfo>();
        ItemInfo info1;
        ItemInfo info2;
        if (cfg.Param3 > 0)
        {
            info1 = new ItemInfo();
            info2 = new ItemInfo();
            info1.Id = cfg.Param1;
            info1.Value = cfg.Param2;
            info2.Id = cfg.Param3;
            info2.Value = cfg.Param4;
            listInfo.Add(info1);
            listInfo.Add(info2);
        }
        else
        {
            info1 = new ItemInfo();
            info1.Id = cfg.Param1;
            info1.Value = cfg.Param2;
            listInfo.Add(info1);
        }
        for (int i = 0; i < listInfo.Count; i++)
        {
            if (GameConfigMgr.Instance.GetItemConfig(listInfo[i].Id).ItemType == 2)
                _view = ItemFactory.Instance.CreateItemView(listInfo[i], ItemViewType.EquipHeroItem);
            else
                _view = ItemFactory.Instance.CreateItemView(listInfo[i], ItemViewType.HeroItem);
            _view.mRectTransform.SetParent(_itemObj1, false);
        }
        _num.text = LanguageMgr.GetLanguage(5007505, cfg.EventCount - limitedItemDataVO.mCurValue);
    }

    private void OnBuy()
    {

    }

    public override void Dispose()
    {
        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        _view = null;
        base.Dispose();
    }
}
