using Framework.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine.UI;
using IHLogic;

public class RechargeItemView : UIBaseView
{
    private List<MonthCardData> _listCardData;
    private PayConfig _cfg;
    private Image _damdicon;
    private Text _damdNum;
    private Text _present;
    private Button _buyBtn;
    private Text _priceText;
    private bool isBuy;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _damdicon = Find<Image>("Img/DamdIcon");
        _damdNum = Find<Text>("Img/DamdNum");
        _present = Find<Text>("Text");
        _buyBtn = Find<Button>("BuyBtn");
        _priceText = Find<Text>("BuyBtn/Text");

        _buyBtn.onClick.Add(OnBuy);
        isBuy = true;
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _listCardData = args[0] as List<MonthCardData>;
        _cfg = args[1] as PayConfig;
        //_icon.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(SpecialItemID.Diamond).Icon);
        _damdicon.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(SpecialItemID.Diamond).UIIcon);
        ObjectHelper.SetSprite(_damdicon,_damdicon.sprite);
        _damdNum.text = _cfg.RewardShow.ToString();
//#if UNITY_ANDROID && !UNITY_EDITOR
        string price = NativeLogicInterface.Instance.GetShowPrice(_cfg.BundleID);
        //LogHelper.LogWarning("[RechargeItemView.Refresh() => get price:" + price + ", bundle id:" + _cfg.BundleID + "]");
        if (!string.IsNullOrEmpty(price))
            _priceText.text = price;
//#endif
        if (_cfg.Name > 0)
        {

        }
        if (_cfg.ID == 1 || _cfg.ID == 2)
        {
            _present.text = LanguageMgr.GetLanguage(5001304, _cfg.MonthCardReward);
            if (_listCardData.Count > 0)
            {
                for (int i = 0; i < _listCardData.Count; i++)
                {
                    if (_listCardData[i].BundleId == _cfg.BundleID)
                    {
                        if (_listCardData[i].SendMailNum >= 30)
                            _buyBtn.interactable = true;
                        else
                            _buyBtn.interactable = false;
                        break;
                    }
                    else
                    {
                        _buyBtn.interactable = true;
                    }
                }
            }
            else
            {
                _buyBtn.interactable = true;
            }
        }
        else
        {
            _buyBtn.interactable = true;
            List<string> listStrId = RechargeDataModel.Instance.mLstStrId;
            if (listStrId.Contains(_cfg.BundleID))
            {
                if (_cfg.RewardBonusShow > 0)
                    _present.text = LanguageMgr.GetLanguage(5001320, _cfg.RewardBonusShow);
                else
                    _present.text = "";
            }
            else
            {
                _present.text = LanguageMgr.GetLanguage(5001305, (_cfg.GemRewardFirst - _cfg.RewardShow));
            }
        }
    }

    private void OnBuy()
    {
        if (isBuy)
        {
            LogHelper.Log("[RechargeItemView.OnBuy() => pay id:" + _cfg.ID + ", bundle id:" + _cfg.BundleID + "]");
            NativeLogicInterface.Instance.Pay(_cfg.BundleID);
            isBuy = false;
            DelayCall(0.3f, () => isBuy = true);
        }
    }
}
