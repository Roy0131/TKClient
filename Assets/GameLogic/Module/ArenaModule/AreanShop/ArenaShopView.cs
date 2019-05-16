using UnityEngine;
using UnityEngine.UI;
using Msg.ClientMessage;
using Framework.UI;

public class ArenaShopView : UIBaseView
{
    private Button _closeBtn;
    private Button _buyBtn;

    private Image _costResIcon;
    private Text _costCount;

    private ItemView _itemView;
    private ShopItemDataVO _shopItemData;

    private int _costValue;
    private Transform _root;
    protected override void ParseComponent()
    {
        base.ParseComponent();
        _root = Find<Transform>("Root");
        ItemInfo info = new ItemInfo();
        info.Id = SpecialItemID.Arena_Ticket;
        info.Value = 5;
        _itemView = ItemFactory.Instance.CreateItemView(info, ItemViewType.BagItem);
        GameUIMgr.Instance.ChildAddToParent(_itemView.mRectTransform, Find<RectTransform>("Root/ItemRoot"));

        _closeBtn = Find<Button>("Root/BtnClose");
        _buyBtn = Find<Button>("Root/ButtonBuy");
        ColliderHelper.SetButtonCollider(_buyBtn.transform);
        ColliderHelper.SetButtonCollider(_closeBtn.transform);
        _costCount = Find<Text>("Root/CostRoot/TextNum");
        _costResIcon = Find<Image>("Root/CostRoot/CostResIcon");

        _closeBtn.onClick.Add(OnClose);
        _buyBtn.onClick.Add(OnBuyTicket);
    }
    
    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _shopItemData = args[0] as ShopItemDataVO;
        ShopItemConfig config = GameConfigMgr.Instance.GetShopItemConfig(_shopItemData.mItemId);
        string[] resCost = config.BuyCost.Split(',');
        _costValue = int.Parse(resCost[1]);
        ShowCost();
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        HeroDataModel.Instance.AddEvent(HeroEvent.HeroInfoChange, ShowCost);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        HeroDataModel.Instance.RemoveEvent(HeroEvent.HeroInfoChange, ShowCost);
    }

    private void ShowCost()
    {
        int value = HeroDataModel.Instance.mHeroInfoData.mDiamond;
        _costCount.color = value > _costValue ? Color.white : Color.red;
        _costCount.text = value + "/" + _costValue;
    }

    private void OnClose()
    {
        Hide();
    }

    private void OnBuyTicket()
    {
        int value = HeroDataModel.Instance.mHeroInfoData.mDiamond;
        if (value < _costValue)
        {
            LogHelper.LogWarning("钻石不足!!!");
            return;
        }
        GameNetMgr.Instance.mGameServer.ReqShopBuy(ShopIdConst.ISLANDSHOP, _shopItemData.mId);
        TDPostDataMgr.Instance.DoCostDiamond(TDCostDiamondType.BuyArenaCount, 1, _costValue);
        Hide();
    }

    public override void Hide()
    {
        _shopItemData = null;
        base.Hide();
    }

    public override void Dispose()
    {
        _shopItemData = null;
        if (_itemView != null)
        {
            ItemFactory.Instance.ReturnItemView(_itemView);
            _itemView = null;
        }
        base.Dispose();
    }
    protected override void OnShowViewAnimation()
    {
        base.OnShowViewAnimation();
        ObjectHelper.PopAnimationLiner(_root);
    }
}