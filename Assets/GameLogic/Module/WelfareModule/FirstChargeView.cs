using Framework.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstChargeView : UIBaseView
{
    private Text _name;
    private Text _con1;
    private Text _con2;
    private Text _firstBuyText;
    private Button _firstBuy;
    private RectTransform _firstParent;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _name = Find<Text>("Img/Name");
        _con1 = Find<Text>("Img/Con1");
        _con2 = Find<Text>("Img/Con2");
        _firstBuyText = Find<Text>("Buy/Text");
        _firstBuy = Find<Button>("Buy");
        _firstParent = Find<RectTransform>("Reward");


        _firstBuy.onClick.Add(OnFirstBuy);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        RechargeDataModel.Instance.AddEvent<List<ItemInfo>>(RechargeEvent.FirstRecharge, OnFirstRecharge);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        RechargeDataModel.Instance.RemoveEvent<List<ItemInfo>>(RechargeEvent.FirstRecharge, OnFirstRecharge);
    }

    private void OnFirstRecharge(List<ItemInfo> listInfo)
    {
        GetItemTipMgr.Instance.ShowItemResult(listInfo);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _name.text = LanguageMgr.GetLanguage(5007302);
        _con1.text = LanguageMgr.GetLanguage(5007303);
        _con2.text = LanguageMgr.GetLanguage(5007305);
        DiposeChildren();
        _childrenViews = new List<UIBaseView>();
        ItemInfo itemInfo;
        ItemView view;
        string[] rewards = GameConst.FirstReward.Split(',');
        if (rewards.Length % 2 != 0)
            return;
        for (int i = 0; i < rewards.Length; i += 2)
        {
            itemInfo = new ItemInfo();
            view = new ItemView();
            itemInfo.Id = int.Parse(rewards[i]);
            itemInfo.Value = int.Parse(rewards[i + 1]);

            view = ItemFactory.Instance.CreateItemView(itemInfo, ItemViewType.BagItem);
            view.mRectTransform.SetParent(_firstParent, false);
            AddChildren(view);
        }
        if (RechargeDataModel.Instance.mFirstChargeState == 0)
            _firstBuyText.text = LanguageMgr.GetLanguage(5007304);
        else if (RechargeDataModel.Instance.mFirstChargeState == 1)
            _firstBuyText.text = LanguageMgr.GetLanguage(5001203);
    }

    private void OnFirstBuy()
    {
        if (RechargeDataModel.Instance.mFirstChargeState == 0)
            GameUIMgr.Instance.OpenModule(ModuleID.Recharge);
        else if (RechargeDataModel.Instance.mFirstChargeState == 1)
            GameNetMgr.Instance.mGameServer.FirstAward();
    }
}
