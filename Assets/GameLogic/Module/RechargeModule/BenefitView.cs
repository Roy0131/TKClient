using Framework.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BenefitView : UIBaseView
{
    private Text _benefitText;
    private Text _Text1;
    private Text _Text2;
    private Text _Text3;
    private Text _Text4;
    private Text _Text5;
    private Text _Text6;
    private Text _vipText;
    private Text _monthCardText;
    private Button _leftBtn;
    private Button _righBtn;
    private GameObject _vipReward;
    private GameObject _monthCardReward;
    private RectTransform _parent;
    private RectTransform _monthCardParent;
    private int _vipId;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _benefitText = Find<Text>("Bj/CurVIP");
        _Text1 = Find<Text>("Bj/Detail/Group/Text1");
        _Text2 = Find<Text>("Bj/Detail/Group/Text2");
        _Text3 = Find<Text>("Bj/Detail/Group/Text3");
        _Text4 = Find<Text>("Bj/Detail/Group/Text4");
        _Text5 = Find<Text>("Bj/Detail/Group/Text5");
        _Text6 = Find<Text>("Bj/Detail/Group/Text6");
        _vipText = Find<Text>("Bj/Detail/Group/VipReward/Text");
        _monthCardText = Find<Text>("Bj/Detail/Group/MonthCardReward/Text");
        _leftBtn = Find<Button>("LeftBtn");
        _righBtn = Find<Button>("RightBtn");
        _parent = Find<RectTransform>("Bj/Detail/Group/VipReward/Obj");
        _monthCardParent = Find<RectTransform>("Bj/Detail/Group/MonthCardReward/Obj");
        _vipReward = Find("Bj/Detail/Group/VipReward");
        _monthCardReward = Find("Bj/Detail/Group/MonthCardReward");

        _leftBtn.onClick.Add(OnLeft);
        _righBtn.onClick.Add(OnRigh);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _vipText.text = LanguageMgr.GetLanguage(5001309);
        _monthCardText.text = LanguageMgr.GetLanguage(5001319);
        _vipId = HeroDataModel.Instance.mHeroInfoData.mVipLevel;
        if (_vipId == 0)
            _vipId = 1;
        OnBenefitChang();
    }

    private void OnBenefitChang()
    {
        DiposeChildren();
        _childrenViews = new List<UIBaseView>();
        VipConfig cfg = GameConfigMgr.Instance.GetVipConfig(_vipId);
        _leftBtn.gameObject.SetActive(_vipId > 1);
        _righBtn.gameObject.SetActive(_vipId < 11);
        _Text1.gameObject.SetActive(cfg.AccelTimes > 0);
        _Text2.gameObject.SetActive(cfg.ActiveStageBuyTimes > 0);
        _Text3.gameObject.SetActive(cfg.GoldFingerBonus > 0);
        _Text4.gameObject.SetActive(cfg.HonorPointBonus > 0);
        _Text5.gameObject.SetActive(cfg.QuickBattle > 0);
        _Text6.gameObject.SetActive(cfg.SearchTaskCount > 0);
        _benefitText.text = LanguageMgr.GetLanguage(5001303, _vipId);
        _Text1.text = LanguageMgr.GetLanguage(5001311) + cfg.AccelTimes;
        _Text2.text = LanguageMgr.GetLanguage(5001314) + cfg.ActiveStageBuyTimes;
        _Text3.text = LanguageMgr.GetLanguage(5001310) + (float)(cfg.GoldFingerBonus / 100) + "%";
        _Text4.text = LanguageMgr.GetLanguage(5001318) + (float)(cfg.HonorPointBonus / 100) + "%";
        _Text5.text = LanguageMgr.GetLanguage(5001313);
        _Text6.text = LanguageMgr.GetLanguage(5001312) + cfg.SearchTaskCount;
        _vipReward.SetActive(_vipId > 0);
        _monthCardReward.SetActive(cfg.MonthCardItemBonus != null && cfg.MonthCardItemBonus != "");
        string[] reward = cfg.VIPItemRewardShow.Split(',');
        int itemId = 0;
        int itemCount = 0;    
        ItemView view;
        for (int i = 0; i < reward.Length; i += 2)
        {
            ItemInfo itemInfo = new ItemInfo();
            view = new ItemView();
            if (reward.Length % 2 != 0)
                continue;
            itemId = int.Parse(reward[i]);
            itemCount = int.Parse(reward[i + 1]);
            itemInfo.Id = itemId;
            itemInfo.Value = itemCount;

            if (GameConfigMgr.Instance.GetItemConfig(itemInfo.Id).ItemType == 2)
                view = ItemFactory.Instance.CreateItemView(itemInfo, ItemViewType.EquipRewardItem);
            else
                view = ItemFactory.Instance.CreateItemView(itemInfo, ItemViewType.RewardItem);
            view.mRectTransform.SetParent(_parent, false);
            AddChildren(view);
        }
        if (cfg.MonthCardItemBonus != null && cfg.MonthCardItemBonus != "")
        {
            string[] rewards = cfg.MonthCardItemBonus.Split(',');
            int itemIds = 0;
            int itemCounts = 0;
            for (int i = 0; i < rewards.Length; i += 2)
            {
                ItemInfo itemInfo = new ItemInfo();
                view = new ItemView();
                if (rewards.Length % 2 != 0)
                    continue;
                itemIds = int.Parse(rewards[i]);
                itemCounts = int.Parse(rewards[i + 1]);
                itemInfo.Id = itemIds;
                itemInfo.Value = itemCounts;

                if (GameConfigMgr.Instance.GetItemConfig(itemInfo.Id).ItemType == 2)
                    view = ItemFactory.Instance.CreateItemView(itemInfo, ItemViewType.EquipRewardItem);
                else
                    view = ItemFactory.Instance.CreateItemView(itemInfo, ItemViewType.RewardItem);
                view.mRectTransform.SetParent(_monthCardParent, false);
                AddChildren(view);
            }
        }
    }

    private void OnLeft()
    {
        _vipId--;
        OnBenefitChang();
    }

    private void OnRigh()
    {
        _vipId++;
        OnBenefitChang();
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
