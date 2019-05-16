using Framework.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroShopView : UIBaseView
{
    private Text _shopName;
    private Text _coinsAmount;
    private Text _coinsRefresh;
    private Image _coinsImg;
    private Image _coinsRefreshImg;
    private Button _leftBtn;
    private Button _rightBtn;
    private Button _refreshBtn;
    private GameObject _leftObj;
    private GameObject _rightObj;
    private List<Toggle> _listTog;
    private List<HeroShopItemView> _heroShopItemView;
    private ShopDataVO _shopVO;

    private int _bookmarkNum;
    private int _bookmark = 0;
    private int _curShopType;
    private int shopNum;

    private int _itemId = 0;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _shopName = Find<Text>("ShopName");
        _coinsAmount = Find<Text>("Refresh/HeroCoins/CoinsAmount");
        _coinsRefresh = Find<Text>("Refresh/RefreshBtn/CoinsImg/CoinsNum");
        _coinsImg = Find<Image>("Refresh/HeroCoins/Conins");
        _coinsRefreshImg = Find<Image>("Refresh/RefreshBtn/CoinsImg");
        _leftBtn = Find<Button>("LeftBtn");
        _rightBtn = Find<Button>("RightBtn");
        _refreshBtn = Find<Button>("Refresh/RefreshBtn");
        _leftObj = Find("LeftBtn");
        _rightObj = Find("RightBtn");

        _heroShopItemView = new List<HeroShopItemView>();
        for (int i = 0; i < 8; i++)
        {
            GameObject obj = Find("Panel_Scroll/KnapsackPanel/Com" + (i + 1));
            HeroShopItemView heroItemView = new HeroShopItemView();
            heroItemView.SetDisplayObject(obj);
            _heroShopItemView.Add(heroItemView);
        }

        _leftBtn.onClick.Add(OnLeft);
        _rightBtn.onClick.Add(OnRight);
        _refreshBtn.onClick.Add(OnRefresh);
    }

    private void OnMarkNum()
    {
        shopNum = 0;
        Dictionary<int, ShopItemConfig> AllDatas = ShopItemConfig.Get();
        foreach (ShopItemConfig cfg in AllDatas.Values)
        {
            if (cfg.ShopID == _curShopType)
                shopNum += 1;
        }
        if (shopNum % 8 == 0)
            _bookmarkNum = shopNum / 8;
        else
            _bookmarkNum = (shopNum / 8) + 1;
        _listTog = new List<Toggle>();
        for (int i = 0; i < _bookmarkNum; i++)
        {
            GameObject obj = Find("GameObject/Toggle" + i);
            obj.SetActive(true);
            _listTog.Add(Find<Toggle>("GameObject/Toggle" + i));
        }
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        BagDataModel.Instance.AddEvent<List<int>>(BagEvent.BagItemRefresh, OnBagItemRefresh);
        ShopDataModel.Instance.AddEvent<int, bool>(ShopEvent.Refresh, OnShopRefresh);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        BagDataModel.Instance.RemoveEvent<List<int>>(BagEvent.BagItemRefresh, OnBagItemRefresh);
        ShopDataModel.Instance.RemoveEvent<int, bool>(ShopEvent.Refresh, OnShopRefresh);
    }

    private void OnShopRefresh(int id,bool isFree)
    {
        if (id == _curShopType)
        {
            _bookmark = 0;
            OnBookmark(_bookmark);
        }
    }

    private void OnBagItemRefresh(List<int> itemId)
    {
        if (itemId.Contains(SpecialItemID.HeroCoins) || itemId.Contains(SpecialItemID.GuildCoin) || itemId.Contains(SpecialItemID.ExpeditionGold))
            OnHeroShop();
    }

    private void OnBookmark(int book)
    {
        _leftObj.SetActive(book > 0);
        _rightObj.SetActive(book < (_bookmarkNum-1));
        for (int i = 0; i < _bookmarkNum; i++)
        {
            if (i == book)
                _listTog[i].isOn = true;
            else
                _listTog[i].isOn = false;
        }
        OnShopChange();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _curShopType = int.Parse(args[0].ToString());
        if (_curShopType == ShopIdConst.HEROSHOP)
        {
            _refreshBtn.gameObject.SetActive(true);
            _shopName.text = LanguageMgr.GetLanguage(5002108);
            _itemId = SpecialItemID.HeroCoins;
        }
        else if (_curShopType == ShopIdConst.GUILDSHOP)
        {
            _refreshBtn.gameObject.SetActive(true);
            _shopName.text = LanguageMgr.GetLanguage(5002105);
            _itemId = SpecialItemID.GuildCoin;
        }
        else if (_curShopType == ShopIdConst.EXPEDITIONSHOP)
        {
            _refreshBtn.gameObject.SetActive(false);
            _shopName.text = LanguageMgr.GetLanguage(5002113);
            _itemId = SpecialItemID.ExpeditionGold;
        }
        _shopVO = ShopDataModel.Instance.GetShopDataByShopId(_curShopType);
        if (_shopVO == null)
        {
            ShopDataModel.Instance.ReqShopData(_curShopType);
            return;
        }
        OnHeroShop();
        OnMarkNum();
        OnBookmark(_bookmark);
    }

    private void OnHeroShop()
    {
        _coinsAmount.text = UnitChange.GetUnitNum(BagDataModel.Instance.GetItemCountById(_itemId));
        _coinsImg.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(_itemId).UIIcon);

        _coinsRefresh.text = UnitChange.GetUnitNum(ShopDataModel.Instance.GetShopDataByShopId(_curShopType).mReFreshNum);
        _coinsRefreshImg.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(_itemId).UIIcon);

        ObjectHelper.SetSprite(_coinsImg,_coinsImg.sprite);
        ObjectHelper.SetSprite(_coinsRefreshImg,_coinsRefreshImg.sprite);
    }

    private void OnShopChange()
    {
        for (int i = 0; i < _heroShopItemView.Count; i++)
            _heroShopItemView[i].Hide();
        if (_shopVO.mListItemVO.Count == 0)
            return;
        for (int i = 0; i < _heroShopItemView.Count; i++)
        {
            if (_bookmark == _bookmarkNum - 1 && i >= shopNum % 8 && shopNum % 8 != 0)
                _heroShopItemView[i].Hide();
            else
                _heroShopItemView[i].Show(_shopVO.mListItemVO[_bookmark * 8 + i]);
        }
    }

    private void OnLeft()
    {
        _bookmark -= 1;
        OnBookmark(_bookmark);
    }

    private void OnRight()
    {
        _bookmark += 1;
        OnBookmark(_bookmark);
    }

    private void OnRefresh()
    {
        string str = "";
        if (_curShopType == ShopIdConst.HEROSHOP)
            str = LanguageMgr.GetLanguage(6001163);
        else if (_curShopType == ShopIdConst.GUILDSHOP)
            str = LanguageMgr.GetLanguage(6001164);
        ConfirmTipsMgr.Instance.ShowConfirmTips(LanguageMgr.GetLanguage(6001168, ShopDataModel.Instance.GetShopDataByShopId(_curShopType).mReFreshNum) + str + "?", ShopRefresh);
    }

    private void ShopRefresh(bool result, bool blShowAgain)
    {
        string str = "";
        if (_curShopType == ShopIdConst.HEROSHOP)
            str = LanguageMgr.GetLanguage(6001162);
        else if (_curShopType == ShopIdConst.GUILDSHOP)
            str = LanguageMgr.GetLanguage(6001192);
        if (result)
        {
            if (BagDataModel.Instance.GetItemCountById(_itemId) >= _shopVO.mReFreshNum)
            {
                SoundMgr.Instance.PlayEffectSound("UI_btn_refresh");
                GameNetMgr.Instance.mGameServer.ReqShopRefresh(_curShopType);
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

    public override void Hide()
    {
        _bookmark = 0;
        base.Hide();
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
