using Framework.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroShopItemView : UIBaseView
{
    private Text _buyText;
    private Text _limit;
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
        _limit = Find<Text>("Limit");
        _buyImg = Find<Image>("BuyBtn/Img");
        _buyBtn = Find<Button>("BuyBtn");
        _comParent = Find<RectTransform>("ComObj");
        _gray = Find<ImageGray>("BuyBtn/Img");
        _buyPanl = Find("BuyPanl");

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
            _limit.text = string.Format(LanguageMgr.GetLanguage(5002110), mShopItem.mBuyNum, _cfg.StockNum);
        }
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        mShopItem = args[0] as ShopItemDataVO;
        _cfg = GameConfigMgr.Instance.GetShopItemConfig(mShopItem.mItemId);
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
        _limit.gameObject.SetActive(_cfg.StockNum > 0);
        _limit.text =string.Format(LanguageMgr.GetLanguage(5002110), mShopItem.mBuyNum, _cfg.StockNum);
        _buyImg.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(mShopItem.mInfo.Id).UIIcon);
        ObjectHelper.SetSprite(_buyImg,_buyImg.sprite);
        OnInteractable();
    }

    private void OnInteractable()
    {
        if (_cfg.StockNum > 0)
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
        else
        {
            _gray.SetNormal();
            _view.SetNormal();
            _buyBtn.interactable = true;
            _buyPanl.SetActive(false);
        }
    }

    private void OnBuy()
    {
        ConfirmTipsMgr.Instance.ShowConfirmTips(LanguageMgr.GetLanguage(6001140), AlertBack);
    }

    private void AlertBack(bool result, bool blShowAgain)
    {
        string str = "";
        if (_cfg.ShopID == ShopIdConst.HEROSHOP)
            str = LanguageMgr.GetLanguage(6001162);
        else if (_cfg.ShopID == ShopIdConst.GUILDSHOP)
            str = LanguageMgr.GetLanguage(6001192);
        else if (_cfg.ShopID == ShopIdConst.EXPEDITIONSHOP)
            str = LanguageMgr.GetLanguage(5003405);
        if (result)
        {
            if (BagDataModel.Instance.GetItemCountById(mShopItem.mInfo.Id) >= mShopItem.mInfo.Value)
                GameNetMgr.Instance.mGameServer.ReqShopBuy(_cfg.ShopID, mShopItem.mId, 1);
            else
                PopupTipsMgr.Instance.ShowTips(str);
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
