using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Msg.ClientMessage;
using Framework.UI;

public class RechargeView : UIBaseView
{
    private Text _curVIPGrade;
    private Text _expNum;
    private Text _vipBuyText;
    private Text _reachNex;
    private Text _maxVIPText;
    private Image _fill;
    private Button _vipBtn;
    private RectTransform _rectRecharge;
    private GameObject _rechargeObj;
    private GameObject _nextBuyObj;
    private List<RechargeItemView> _listRechargeItemView;
    private List<MonthCardData> _listCardData;
    private bool isVIP = true;

    private BenefitView _benefitView;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _curVIPGrade = Find<Text>("VipShow/CurVIPGrade");
        _expNum = Find<Text>("VipShow/Exp/ExpNum");
        _vipBuyText = Find<Text>("VipShow/VIPBtn/Text");
        _reachNex = Find<Text>("VipShow/NextBuy/ReachNex");
        _maxVIPText = Find<Text>("VipShow/MaxVIP");
        _fill = Find<Image>("VipShow/Exp/Fill");
        _vipBtn = Find<Button>("VipShow/VIPBtn");
        _rectRecharge = Find<RectTransform>("RechargeBtn/Group");
        _rechargeObj = Find("RechargeBtn");
        _nextBuyObj = Find("VipShow/NextBuy");

        _benefitView = new BenefitView();
        _benefitView.SetDisplayObject(Find("Privilege"));

        _vipBtn.onClick.Add(OnVIPBtn);
        OnSignInit();
    }

    private void OnSignInit()
    {
        _listRechargeItemView = new List<RechargeItemView>();
        for (int i = 0; i < 8; i++)
        {
            GameObject obj = Find("RechargeBtn/Group/Recharge" + (i + 1));
            RechargeItemView rechargeItemView = new RechargeItemView();
            rechargeItemView.SetDisplayObject(obj);
            _listRechargeItemView.Add(rechargeItemView);
        }
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        BagDataModel.Instance.AddEvent<List<int>>(BagEvent.BagItemRefresh, OnBagRefresh);
        RechargeDataModel.Instance.AddEvent<bool,string>(RechargeEvent.Recharge, OnRechargeReward);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        BagDataModel.Instance.RemoveEvent<List<int>>(BagEvent.BagItemRefresh, OnBagRefresh);
        RechargeDataModel.Instance.RemoveEvent<bool,string>(RechargeEvent.Recharge, OnRechargeReward);
    }

    private void OnRechargeReward(bool isFirst,string bundleId)
    {
        List<ItemInfo> listInfo = new List<ItemInfo>();
        ItemInfo info;
        foreach (PayConfig cfg in PayConfig.Get().Values)
        {
            if (bundleId == cfg.BundleID)
            {
                info = new ItemInfo();
                info.Id = SpecialItemID.Diamond;
                if (isFirst)
                    info.Value = cfg.GemRewardFirst;
                else
                    info.Value = cfg.GemReward;
                listInfo.Add(info);
            }
        }
        GetItemTipMgr.Instance.ShowItemResult(listInfo);
        GameNetMgr.Instance.mGameServer.ReqRechargeData();
        DelayCall(0.5f, () => { LoadingMgr.Instance.HideRechargeMask(); });
    }

    private void OnBagRefresh(List<int> listItemId)
    {
        OnVIPShow();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _listCardData = args[0] as List<MonthCardData>;
        isVIP = true;
        OnVIPShow();
        RechargeChang();
    }
     
    private void RechargeChang()
    {
        for (int i = 0; i < _listRechargeItemView.Count; i++)
        {
            PayConfig cfg = GameConfigMgr.Instance.GetPayConfig(i + 1);
            _listRechargeItemView[i].Show(_listCardData, cfg);
        }
    }

    private void OnVIPShow()
    {
        if (isVIP)
        {
            _rechargeObj.SetActive(true);
            _benefitView.Hide();
        }
        else
        {
            _rechargeObj.SetActive(false);
            _benefitView.Show();
        }
        if (isVIP)
            _vipBuyText.text = LanguageMgr.GetLanguage(5001324);
        else
            _vipBuyText.text = LanguageMgr.GetLanguage(5001325);

        Find<Text>("VipShow/NextBuy/Buy").text = LanguageMgr.GetLanguage(5002509);
        _maxVIPText.text = LanguageMgr.GetLanguage(5001321);
        if (HeroDataModel.Instance.mHeroInfoData != null)
        {
            _nextBuyObj.SetActive(HeroDataModel.Instance.mHeroInfoData.mVipLevel < VipConfig.Get().Count - 1);
            _maxVIPText.gameObject.SetActive(HeroDataModel.Instance.mHeroInfoData.mVipLevel >= VipConfig.Get().Count - 1);
            _curVIPGrade.text = "VIP " + HeroDataModel.Instance.mHeroInfoData.mVipLevel;
            if (HeroDataModel.Instance.mHeroInfoData.mVipLevel < VipConfig.Get().Count - 1)
            {
                _expNum.text = BagDataModel.Instance.GetItemCountById(SpecialItemID.VIPExp) + "/" + GameConfigMgr.Instance.GetVipConfig(HeroDataModel.Instance.mHeroInfoData.mVipLevel + 1).Money;
                _fill.fillAmount = (float)BagDataModel.Instance.GetItemCountById(SpecialItemID.VIPExp) / (float)GameConfigMgr.Instance.GetVipConfig(HeroDataModel.Instance.mHeroInfoData.mVipLevel + 1).Money;
                _reachNex.text = "<color=#FFF46C>" + ((GameConfigMgr.Instance.GetVipConfig(HeroDataModel.Instance.mHeroInfoData.mVipLevel + 1).Money - BagDataModel.Instance.GetItemCountById(SpecialItemID.VIPExp))) + "<color=#FFFFFF>" + LanguageMgr.GetLanguage(5001326)+ "</color>" + "VIP" + (HeroDataModel.Instance.mHeroInfoData.mVipLevel + 1 + "</color>");
            }
            else
            {
                _expNum.text = BagDataModel.Instance.GetItemCountById(SpecialItemID.VIPExp) + "/" + GameConfigMgr.Instance.GetVipConfig(HeroDataModel.Instance.mHeroInfoData.mVipLevel).Money;
                _fill.fillAmount = 1;
            }
        }
    }

    private void OnVIPBtn()
    {
        isVIP = !isVIP;
        if (isVIP)
        {
            _rechargeObj.SetActive(true);
            _benefitView.Hide();
        }
        else
        {
            _rechargeObj.SetActive(false);
            _benefitView.Show();
        }
        if (isVIP)
            _vipBuyText.text = LanguageMgr.GetLanguage(5001324);
        else
            _vipBuyText.text = LanguageMgr.GetLanguage(5001325);
    }

    public override void Hide()
    {
        _rectRecharge.anchoredPosition = new Vector3(0f, 0f, 0f);
        base.Hide();
    }
}
