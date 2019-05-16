using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Msg.ClientMessage;
using Framework.UI;

public class MysteryShopItemView : UIBaseView
{
    private Text _buyText;
    private Image _buyImg;
    private Button _buyBtn;
    private GameObject _buyPanl;
    private RectTransform _comParent;
    private ImageGray _gray;
    private ItemView _view;
    private ShopItemConfig _cfg;

    private ShopItemDataVO mShopItem;


    protected override void ParseComponent()
    {
        base.ParseComponent();
        _buyText = Find<Text>("BuyBtn/Text");
        _buyImg = Find<Image>("BuyBtn/Img");
        _buyBtn = Find<Button>("BuyBtn");
        _comParent = Find<RectTransform>("ComObj");
        _buyPanl = Find("BuyPanl");
        _gray = Find<ImageGray>("BuyBtn/Img");
        _buyBtn.onClick.Add(OnBuy);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        ShopDataModel.Instance.AddEvent<int>(ShopEvent.ShopBuy, OnShopBuy);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        ShopDataModel.Instance.RemoveEvent<int>(ShopEvent.ShopBuy, OnShopBuy);
    }

    private void OnShopBuy(int id)
    {
        if (mShopItem.mId == id)
        {
            List<ItemInfo> _listItemInfo = new List<ItemInfo>();
            ItemInfo _itemInfo;
            string[] itemList = _cfg.ItemList.Split(',');
            for (int i = 0; i < itemList.Length; i += 2)
            {
                _itemInfo = new ItemInfo();
                if (itemList.Length % 2 != 0)
                    continue;
                _itemInfo.Id = int.Parse(itemList[i]);
                _itemInfo.Value = int.Parse(itemList[i + 1]);
                _listItemInfo.Add(_itemInfo);
            }
            GetItemTipMgr.Instance.ShowItemResult(_listItemInfo);
            OnInteractable();
        }
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        mShopItem = args[0] as ShopItemDataVO;
        _cfg = GameConfigMgr.Instance.GetShopItemConfig(mShopItem.mItemId);
        if (_cfg == null)
            return;
        OnShopItemInit();
    }

    private void OnShopItemInit()
    {
        ItemInfo itemInfo;
        string[] itemList = _cfg.ItemList.Split(',');
        if (itemList.Length % 2 != 0)
            return;
        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        for (int i = 0; i < itemList.Length; i += 2)
        {
            itemInfo = new ItemInfo();
            if (itemList.Length % 2 != 0)
                continue;
            itemInfo.Id = int.Parse(itemList[i]);
            itemInfo.Value = int.Parse(itemList[i + 1]);
            if (GameConfigMgr.Instance.GetItemConfig(itemInfo.Id).ItemType == 2)
                _view = ItemFactory.Instance.CreateItemView(itemInfo, ItemViewType.EquipItem);
            else
                _view = ItemFactory.Instance.CreateItemView(itemInfo, ItemViewType.BagItem);
            _view.mRectTransform.SetParent(_comParent, false);
        }
        _buyText.text = UnitChange.GetUnitNum(mShopItem.mInfo.Value);
        _buyImg.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(mShopItem.mInfo.Id).UIIcon);
        ObjectHelper.SetSprite(_buyImg,_buyImg.sprite);
        OnInteractable();
    }

    private void OnInteractable()
    {
        if (mShopItem.mBuyNum <= 0)
        {
            _gray.SetGray();
            _view.SetGray();
            _buyBtn.interactable = false;
        }
        else
        {
            _gray.SetNormal();
            _view.SetNormal();
            _buyBtn.interactable = true;
        }
        _buyPanl.SetActive(mShopItem.mBuyNum <= 0);
    }

    private void OnBuy()
    {
        ConfirmTipsMgr.Instance.ShowConfirmTips(LanguageMgr.GetLanguage(6001140), AlertBack);
    }

    private void AlertBack(bool result, bool blShowAgain)
    {
        string str = "";
        if (mShopItem.mInfo.Id == SpecialItemID.Gold)
            str = LanguageMgr.GetLanguage(4000097);
        else if (mShopItem.mInfo.Id == SpecialItemID.Diamond)
            str = LanguageMgr.GetLanguage(4000055);
        if (result)
        {
            if (HeroDataModel.Instance.mHeroInfoData.GetCurrencyValue(mShopItem.mInfo.Id) >= mShopItem.mInfo.Value)
            {
                GameNetMgr.Instance.mGameServer.ReqShopBuy(ShopIdConst.MYSTERYSHOP, mShopItem.mId, 1);
                if (mShopItem.mInfo.Id == SpecialItemID.Diamond)
                    TDPostDataMgr.Instance.DoCostDiamond(TDCostDiamondType.BuyMysteryShopCount, 1, mShopItem.mInfo.Value);
            }
            else
            {
                PopupTipsMgr.Instance.ShowTips(str);
            }
        }
        else
        {

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
