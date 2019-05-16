using Framework.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MysteryShopView : UIBaseView
{
    private Text _glodNum;
    private Text _diamondNum;
    private Text _time;
    private Text _amdNum;
    private Image _glodImg;
    private Image _diamondImg;
    private Image _amdImg;
    private Button _goldBtn;
    private Button _diamondBtn;
    private Button _refreshBtn;
    private GameObject _commodityObj;
    private GameObject _refreshsObj;
    private GameObject _amdImgObj;
    private GameObject _timeObj;
    private RectTransform _parent;
    private ShopDataVO _shopVO;
    private List<MysteryShopItemView> listMysteryShopItemViews;

    private uint _timer = 0;
    private int _shopTime = 0;


    protected override void ParseComponent()
    {
        base.ParseComponent();
        _glodNum = Find<Text>("TotalGlod/TotalGlodLabel");
        _diamondNum = Find<Text>("HeroDiamond/TotalHeroExpLabel");
        _time = Find<Text>("TimeText");
        _amdNum = Find<Text>("RefreshBtn/AmdImg/AmdNum");
        _glodImg = Find<Image>("TotalGlod/TotalGlod");
        _diamondImg = Find<Image>("HeroDiamond/Diamond");
        _amdImg = Find<Image>("RefreshBtn/AmdImg");
        _goldBtn = Find<Button>("TotalGlod/BtnGoldAdd");
        _diamondBtn = Find<Button>("HeroDiamond/BtnDiamondAdd");
        _refreshBtn = Find<Button>("RefreshBtn");
        _parent = Find<RectTransform>("Panel_Scroll/KnapsackPanel");
        _commodityObj = Find("Panel_Scroll/KnapsackPanel/Commodity");
        _refreshsObj = Find("RefreshBtn/Refreshs");
        _amdImgObj = Find("RefreshBtn/AmdImg");
        _timeObj = Find("TimeText");

        _goldBtn.onClick.Add(OnGold);
        _diamondBtn.onClick.Add(OnDiamond);
        _refreshBtn.onClick.Add(OnRefresh);

        CreateFixedEffect(Find("fx_ui_shangcheng"), UILayerSort.WindowSortBeginner);

        OnShopInit();
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        HeroDataModel.Instance.AddEvent(HeroEvent.HeroInfoChange, MysteryShop);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        HeroDataModel.Instance.RemoveEvent(HeroEvent.HeroInfoChange, MysteryShop);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _shopTime = ShopDataModel.Instance.GetShopDataByShopId(ShopIdConst.MYSTERYSHOP).LastTime;
        if (_timer != 0)
            TimerHeap.DelTimer(_timer);
        int interval = 1000;
        _timer = TimerHeap.AddTimer(0, interval, OnAddTime);

        OnShopChange();
        MysteryShop();
    }

    private void OnAddTime()
    {
        if (_shopTime <= 0)
        {
            _timeObj.SetActive(false);
            _amdImgObj.SetActive(false);
            _refreshsObj.SetActive(true);
        }
        else
        {
            _timeObj.SetActive(true);
            _amdImgObj.SetActive(true);
            _refreshsObj.SetActive(false);
            _shopTime -= 1;
            _time.text = ("<color=#A5FD47>" + TimeHelper.GetCountTime(_shopTime) + "</color> "+LanguageMgr.GetLanguage(5002112));
        }
    }

    private void MysteryShop()
    {
        _glodNum.text= UnitChange.GetUnitNum(HeroDataModel.Instance.mHeroInfoData.mGold);
        _diamondNum.text= UnitChange.GetUnitNum(HeroDataModel.Instance.mHeroInfoData.mDiamond);
        _amdNum.text = ShopDataModel.Instance.GetShopDataByShopId(ShopIdConst.MYSTERYSHOP).mReFreshNum.ToString();
        _amdImg.sprite= GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(SpecialItemID.Diamond).UIIcon);
        //_glodImg.sprite= GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(SpecialItemID.Gold).Icon);
        //_diamondImg.sprite= GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(SpecialItemID.Diamond).Icon);
        ObjectHelper.SetSprite(_amdImg,_amdImg.sprite);
    }

    private void OnShopInit()
    {
        listMysteryShopItemViews = new List<MysteryShopItemView>();
        for (int i = 0; i < 8; i++)
        {
            GameObject obj = GameObject.Instantiate(_commodityObj);
            obj.transform.SetParent(_parent, false);
            MysteryShopItemView mysteryItemView = new MysteryShopItemView();
            mysteryItemView.SetDisplayObject(obj);
            listMysteryShopItemViews.Add(mysteryItemView);
        }
    }

    private void OnShopChange()
    {
        _shopVO = ShopDataModel.Instance.GetShopDataByShopId(ShopIdConst.MYSTERYSHOP);
        if (_shopVO == null)
        {
            ShopDataModel.Instance.ReqShopData(ShopIdConst.MYSTERYSHOP);
            return;
        }
        for (int j = 0; j < _shopVO.mListItemVO.Count; j++)
            listMysteryShopItemViews[j].Show(_shopVO.mListItemVO[j]);
    }

    private void OnGold()
    {
        GameUIMgr.Instance.OpenModule(ModuleID.Gold);
    }

    private void OnDiamond()
    {
        GameUIMgr.Instance.OpenModule(ModuleID.Recharge);
    }

    private void OnRefresh()
    {
        if (_shopTime <= 0)
        {
            SoundMgr.Instance.PlayEffectSound("UI_btn_refresh");
            GameNetMgr.Instance.mGameServer.ReqShopRefresh(ShopIdConst.MYSTERYSHOP);
        }
        else
        {
            ConfirmTipsMgr.Instance.ShowConfirmTips(LanguageMgr.GetLanguage(6001168, ShopDataModel.Instance.GetShopDataByShopId(ShopIdConst.MYSTERYSHOP).mReFreshNum) + LanguageMgr.GetLanguage(6001205) + "?", ShopRefresh);
        }
    }

    private void ShopRefresh(bool result, bool blShowAgain)
    {
        if (result)
        {
            if (HeroDataModel.Instance.mHeroInfoData.mDiamond >= _shopVO.mReFreshNum)
            {
                SoundMgr.Instance.PlayEffectSound("UI_btn_refresh");
                GameNetMgr.Instance.mGameServer.ReqShopRefresh(ShopIdConst.MYSTERYSHOP);
                TDPostDataMgr.Instance.DoCostDiamond(TDCostDiamondType.BuyMysteryRefreshCount, 1, _shopVO.mReFreshNum);
            }
            else
            {
                PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000055));
            }
        }
        else
        {

        }
    }
}
