using Framework.UI;
using Msg.ClientMessage;
using UnityEngine;
using UnityEngine.UI;


//累计
public class LimitedRebateView : UIBaseView
{
    private Text _text1;
    private Text _text2;
    private RectTransform _parent;
    private ItemView _view;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _text1 = Find<Text>("Text1");
        _text2 = Find<Text>("Text2");
        _parent = Find<RectTransform>("ItemObj");
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        LimitedItemDataVO limitedItemDataVO = args[0] as LimitedItemDataVO;
        int eventId = int.Parse(args[1].ToString());
        SubActiveConfig cfg = GameConfigMgr.Instance.GetSubActiveConfig(limitedItemDataVO.mSubActiveID);
        _text1.text = LanguageMgr.GetLanguage(cfg.DescriptionId, cfg.Param1);
        if (limitedItemDataVO.mCurValue >= cfg.Param1)
            _text2.text = "(" + cfg.Param1 + "/" + cfg.Param1 + ")";
        else
            _text2.text = "(" + limitedItemDataVO.mCurValue + "/" + cfg.Param1 + ")";
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
            _view.mRectTransform.SetParent(_parent, false);
        }
    }

    public override void Dispose()
    {
        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        _view = null;
        base.Dispose();
    }
}
