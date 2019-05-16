using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public class HangupOutputView : UIBaseView
{
    private Text _customsText;
    private Text _goldText;
    private Text _soulText;
    private Text _expText;
    private Image _goldImg;
    private Image _soulImg;
    private Image _exoImg;
    private Button _disBtn;
    private Button _aufoBtn;
    private Button _exploreBtn;
    private CampaignConfig _cfg;
    private RectTransform _parent;

    private int _curCampaignID = 0;
    public List<int> mlstRandomRewards { get; private set; } = new List<int>();

    private Transform _root;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _customsText = Find<Text>("HangupOutputObj/Stage");
        _goldText = Find<Text>("HangupOutputObj/OutputImg/GoldImg/Text");
        _soulText = Find<Text>("HangupOutputObj/OutputImg/SoulImg/Text");
        _expText = Find<Text>("HangupOutputObj/OutputImg/ExpImg/Text");
        _goldImg = Find<Image>("HangupOutputObj/OutputImg/GoldImg/Image");
        _soulImg = Find<Image>("HangupOutputObj/OutputImg/SoulImg/Image");
        _exoImg = Find<Image>("HangupOutputObj/OutputImg/ExpImg/Image");
        _disBtn = Find<Button>("HangupOutputObj/HideBtn");
        _aufoBtn = Find<Button>("HangupOutputObj/Button/AufoBtn");
        _exploreBtn = Find<Button>("HangupOutputObj/Button/Explore");

        _parent = Find<RectTransform>("HangupOutputObj/Panel_Scroll/KnapsackPanel");
        _root = Find<Transform>("HangupOutputObj");
        _disBtn.onClick.Add(Hide);
        _aufoBtn.onClick.Add(OnSetHangup);
        _exploreBtn.onClick.Add(OnExplore);

        NewBieGuide.NewBieGuideMgr.Instance.RegistMaskTransform(NewBieGuide.NewBieMaskID.HangupSetCampaignBtn, _aufoBtn.transform);
    }

    private void CreateRewardItemView()
    {
        DiposeChildren();

        _childrenViews = new List<UIBaseView>();
        //List<int> itemRewards = HangupDataModel.Instance.mlstRandomRewards;
        ItemConfig itemCfg;
        for (int index = 0; index < mlstRandomRewards.Count; index++)
        {
            ItemView view;
            itemCfg = GameConfigMgr.Instance.GetItemConfig(mlstRandomRewards[index]);
            if (itemCfg.ItemType == 2)
                view = ItemFactory.Instance.CreateItemView(itemCfg, ItemViewType.EquipRewardItem);
            else
                view = ItemFactory.Instance.CreateItemView(itemCfg, ItemViewType.RewardItem);
            view.mRectTransform.SetParent(_parent, false);
            AddChildren(view);
        }
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        ExploreDataModel.Instance.AddEvent(ExploreEvent.ExploreData, OnExploreData);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        ExploreDataModel.Instance.RemoveEvent(ExploreEvent.ExploreData, OnExploreData);
    }

    private void OnExploreData()
    {
        _exploreBtn.gameObject.SetActive(_curCampaignID != HangupDataModel.Instance.mIntHangupCampaignId && ExploreDataModel.Instance.IsExplore(_cfg.CampainTask));
    }

    //刷新数据
    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        int campaignID = int.Parse(args[0].ToString());
        _cfg = GameConfigMgr.Instance.GetCampaignByCampaignId(campaignID);
        _aufoBtn.gameObject.SetActive(campaignID != HangupDataModel.Instance.mIntHangupCampaignId);
        if (HeroDataModel.Instance.mHeroInfoData.mLevel >= GameConst.GetFeatureType(FunctionType.Explore))
        {
            if (ExploreDataModel.Instance.exploreDataStory != null)
                _exploreBtn.gameObject.SetActive(campaignID != HangupDataModel.Instance.mIntHangupCampaignId && ExploreDataModel.Instance.IsExplore(_cfg.CampainTask));
            else
                GameNetMgr.Instance.mGameServer.ReqExploreData();
        }
        //if (_curCampaignID == campaignID)
        //    return;
        //获取当前关卡的固定收益
        _curCampaignID = campaignID;
        ParseCampaignConfig();
        _goldText.text = GetStaticRewardByItemId(SpecialItemID.Gold).ToString();
        _soulText.text = GetStaticRewardByItemId(SpecialItemID.HeroExp).ToString();
        _expText.text = GetStaticRewardByItemId(SpecialItemID.RoleExp).ToString();
        _customsText.text =LanguageMgr.GetLanguage(5002908) + ((_hangupConfig.Difficulty - 1) * 8 + _hangupConfig.ChapterMap + "-" + _hangupConfig.ChildMapID);

        CreateRewardItemView();
    }

    public string GetStaticRewardByItemId(int id)
    {
        if (_dictStaticRewards.ContainsKey(id))
            return "+" + _dictStaticRewards[id] + "/" + _hangupConfig.StaticRewardSec + "s";
        return "0/s";
    }

    private Dictionary<int, int> _dictStaticRewards = new Dictionary<int, int>();
    private CampaignConfig _hangupConfig;
    private void ParseCampaignConfig()
    {
        _hangupConfig = GameConfigMgr.Instance.GetCampaignByCampaignId(_curCampaignID);
        string[] rewards = _hangupConfig.StaticRewardItem.Split(',');
        if (rewards.Length % 2 != 0)
        {
            LogHelper.LogError("HangupDataModel.OnStartRewardTimer() => hangup campaign id:" + _curCampaignID + " staticreward format was invalid!!");
            return;
        }
        _dictStaticRewards.Clear();
        int itemCfgId;
        int itemCount;
        for (int i = 0; i < rewards.Length; i += 2)
        {
            itemCfgId = int.Parse(rewards[i]);
            itemCount = int.Parse(rewards[i + 1]);
            _dictStaticRewards.Add(itemCfgId, itemCount);
        }

        string[] randdrop = _hangupConfig.RandomDropIDList.Split(',');
        if (randdrop.Length % 2 != 0)
        {
            return;
        }
        mlstRandomRewards.Clear();
        int dropId;
        for (int i = 0; i < randdrop.Length; i += 2)
        {
            dropId = int.Parse(randdrop[i]);

            mlstRandomRewards.AddRange(GameConfigMgr.Instance.GetDropConfig(dropId));
            mlstRandomRewards.Sort((x, y) => x.CompareTo(y));
        }
    }


    protected override void DiposeChildren()
    {
        if (_childrenViews != null)
        {
            for (int i = _childrenViews.Count - 1; i >= 0; i--)
                ItemFactory.Instance.ReturnItemView(_childrenViews[i] as ItemView);
            _childrenViews.Clear();
            _childrenViews = null;
        }
    }

    private void OnSetHangup()
    {
        HangupDataModel.Instance.ReqHangup(_curCampaignID);
        Hide();
    }

    private void OnExplore()
    {
        GameUIMgr.Instance.OpenModule(ModuleID.Explore, true);
        Hide();
    }

    public override void Dispose()
    {
        base.Dispose();
    }
    protected override void OnShowViewAnimation()
    {
        base.OnShowViewAnimation();
        ObjectHelper.PopAnimationLiner(_root);
    }
}
