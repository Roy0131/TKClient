using Framework.UI;
using Msg.ClientMessage;
using UnityEngine;
using UnityEngine.UI;
using IHLogic;

//充值
public class LimitedOfferView : UIBaseView
{
    private Text _vipExp;
    private Text _number;
    private Button _buy;
    private RectTransform _parent;
    private ItemView _view;
    private Text _priceText;
    private SubActiveConfig _config;
    private bool isBuy;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _vipExp = Find<Text>("VIPEXP");
        _number = Find<Text>("Number");
        _buy = Find<Button>("Buy");
        _priceText = Find<Text>("Buy/Text");
        _parent = Find<RectTransform>("ItemObj");

        _buy.onClick.Add(OnBuy);
        isBuy = true;
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        LimitedItemDataVO limitedItemDataVO = args[0] as LimitedItemDataVO;
        int eventId = int.Parse(args[1].ToString());
        int vipEXP = 0;
        _config = GameConfigMgr.Instance.GetSubActiveConfig(limitedItemDataVO.mSubActiveID);
        _number.text = LanguageMgr.GetLanguage(5002110, _config.EventCount - limitedItemDataVO.mCurValue, _config.EventCount);
        if (_config.Reward != null && _config.Reward != "")
        {
            string[] rewards = _config.Reward.Split(',');
            if (rewards.Length % 2 != 0)
                return;
            if (_view != null)
                ItemFactory.Instance.ReturnItemView(_view);
            for (int i = 0; i < rewards.Length; i += 2)
            {
                if (i < rewards.Length - 2)
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
                else
                {
                    vipEXP = int.Parse(rewards[rewards.Length - 1]);
                }
            }
            _vipExp.text = LanguageMgr.GetLanguage(21) + "\n" + vipEXP;
        }
        else
        {
            _vipExp.text = "";
        }
        if (limitedItemDataVO.mCurValue < _config.EventCount)
            _buy.interactable = true;
        else
            _buy.interactable = false;

        string showPrice = NativeLogicInterface.Instance.GetShowPrice(_config.BundleID);
        if (!string.IsNullOrEmpty(showPrice))
            _priceText.text = showPrice;
    }

    private void OnBuy()
    {
        if (isBuy)
        {
            NativeLogicInterface.Instance.Pay(_config.BundleID);
            isBuy = false;
            DelayCall(0.3f, () => isBuy = true);
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
