using Framework.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonthlyCardView : UIBaseView
{
    private Text _firstBuyText;
    private Text _monthlyCardCon;
    private Text _monthlyCardMailNum;
    private Text _monthlyCardTime;
    private Text _monthlyCardPrice;
    private Text _monthlyCardText2;
    private Button _monthlyCardBtn;
    private GameObject _img1Obj;
    private GameObject _img2Obj;
    private GameObject _monthlyObj;

    private int _curWelfareType;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _monthlyCardCon = Find<Text>("Img/Con");
        _firstBuyText = Find<Text>("Buy/Text");
        _monthlyCardMailNum = Find<Text>("Reward/MailNum");
        _monthlyCardTime = Find<Text>("Reward/Time");
        _monthlyCardPrice = Find<Text>("Img/Text1");
        _monthlyCardText2 = Find<Text>("Img/Text2");
        _monthlyCardBtn = Find<Button>("Buy");
        _monthlyObj = Find("Reward");
        _img1Obj = Find("Img/Img1");
        _img2Obj = Find("Img/Img2");

        _firstBuyText.text = LanguageMgr.GetLanguage(5001301);
        _monthlyCardText2.text = LanguageMgr.GetLanguage(5007407);

        _monthlyCardBtn.onClick.Add(OnMonthlyCardBuy);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        RechargeDataModel.Instance.AddEvent<List<MonthCardData>>(RechargeEvent.RechargeData, OnRechargeData);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        RechargeDataModel.Instance.RemoveEvent<List<MonthCardData>>(RechargeEvent.RechargeData, OnRechargeData);
    }

    private void OnRechargeData(List<MonthCardData> listCard)
    {
        if (listCard.Count > 0)
        {
            for (int i = 0; i < listCard.Count; i++)
            {
                if (listCard[i].BundleId == GameConfigMgr.Instance.GetPayConfig(1).BundleID && _curWelfareType == (int)WelfareType.MonthlyCard)
                {
                    if (listCard[i].SendMailNum >= 30)
                    {
                        _monthlyObj.SetActive(false);
                        _monthlyCardBtn.interactable = true;
                    }
                    else
                    {
                        _monthlyObj.SetActive(true);
                        _monthlyCardBtn.interactable = false;
                        _monthlyCardMailNum.text = LanguageMgr.GetLanguage(5007405) + " " + listCard[i].SendMailNum + "/30";
                        _monthlyCardTime.text = LanguageMgr.GetLanguage(6001117, TimeHelper.GetTime(listCard[i].EndTime, "yyyy-MM-dd"));
                    }
                    break;
                }
                else if (listCard[i].BundleId == GameConfigMgr.Instance.GetPayConfig(2).BundleID && _curWelfareType == (int)WelfareType.StarMonthlyCard)
                {
                    if (listCard[i].SendMailNum >= 30)
                    {
                        _monthlyObj.SetActive(false);
                        _monthlyCardBtn.interactable = true;
                    }
                    else
                    {
                        _monthlyObj.SetActive(true);
                        _monthlyCardBtn.interactable = false;
                        _monthlyCardMailNum.text = LanguageMgr.GetLanguage(5007405) + " " + listCard[i].SendMailNum + "/30";
                        _monthlyCardTime.text = LanguageMgr.GetLanguage(6001117, TimeHelper.GetTime(listCard[i].EndTime, "yyyy-MM-dd"));
                    }
                    break;
                }
                else
                {
                    _monthlyObj.SetActive(false);
                    _monthlyCardBtn.interactable = true;
                }
            }
        }
        else
        {
            _monthlyObj.SetActive(false);
            _monthlyCardBtn.interactable = true;
        }
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _curWelfareType = int.Parse(args[0].ToString());
        _img1Obj.SetActive(_curWelfareType == (int)WelfareType.MonthlyCard);
        _img2Obj.SetActive(_curWelfareType == (int)WelfareType.StarMonthlyCard);
        PayConfig cfg1 = GameConfigMgr.Instance.GetPayConfig(1);
        PayConfig cfg2 = GameConfigMgr.Instance.GetPayConfig(2);
        if (_curWelfareType == (int)WelfareType.MonthlyCard)
        {
            _monthlyCardCon.text = LanguageMgr.GetLanguage(6001120);
            _monthlyCardPrice.text = (cfg1.GemRewardFirst + cfg1.MonthCardReward * cfg1.MonthCardDay).ToString();
        }
        else
        {
            _monthlyCardCon.text = LanguageMgr.GetLanguage(6001118);
            _monthlyCardPrice.text = (cfg2.GemRewardFirst + cfg2.MonthCardReward * cfg2.MonthCardDay).ToString();
        }
    }

    private void OnMonthlyCardBuy()
    {
        GameUIMgr.Instance.OpenModule(ModuleID.Recharge);
    }
}
