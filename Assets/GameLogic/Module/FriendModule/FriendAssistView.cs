using Framework.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FriendAssistView : UIBaseView
{
    private RectTransform _rewardItemRoot;
    private ItemView _pointRewardItem;
    private RectTransform _assistCardRoot;
    private CardView _assistCardItem;
    private Button _assistRewardBtn;

    private RectTransform _bossDiamondRoot;
    private ItemView _bossDiamondItem;
    private RectTransform _bossCardRoot;
    private CardView _bossCardItem;
    private Text _hpPreText;
    private Image _hpSlider;
    private GameObject _bossRoot;

    private Text _searchCDText;
    private Button _funBtn;
    private Text _funBtnText;
    private Button _setAssitCardBtn;
    private GameObject _sidePanle;

    private Text _helpFriend;

    private Button _help;

    private GameObject _skeleton;

    private int _lastSearchTime;
    private uint _searchKey;

    private Text _textDes;
    protected override void ParseComponent()
    {
        base.ParseComponent();
        _rewardItemRoot = Find<RectTransform>("RightView/RewardRoot");
        _assistCardRoot = Find<RectTransform>("RightView/AssistCardRoot");
        _assistRewardBtn = Find<Button>("RightView/HeartRewardBtn");

        _bossDiamondRoot = Find<RectTransform>("LeftView/BossRoot/BossRewardRoot");
        _bossCardRoot = Find<RectTransform>("LeftView/BossRoot/BossCardRoot");
        _hpPreText = Find<Text>("LeftView/BossRoot/BossHpBar/HpPerText");
        _hpSlider = Find<Image>("LeftView/BossRoot/BossHpBar/Slider");
        _bossRoot = Find("LeftView/BossRoot");

        _textDes = Find<Text>("TextDes");
        _searchCDText = Find<Text>("SeachTimeText");
        _helpFriend = Find<Text>("RightView/HelpFriend");
        _funBtn = Find<Button>("SearchBtn");
        _funBtnText = Find<Text>("SearchBtn/Text");
        _setAssitCardBtn = Find<Button>("SetRoleBtn");
        _help = Find<Button>("BtnHelp");
        _skeleton = Find("LeftView/SkeletonGraphic (tansuo)");
        _sidePanle = Find("SidePanle");

        _assistRewardBtn.onClick.Add(OnGetAssistReward);
        _funBtn.onClick.Add(OnFunClick);
        _setAssitCardBtn.onClick.Add(OnSetAssistCard);
        _help.onClick.Add(OnHelp);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        if (FriendDataModel.Instance.mFriendAssistVO != null)
            OnRefreshAssistData();
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        FriendDataModel.Instance.AddEvent(FriendEvent.FriendAssistDataRefresh, OnRefreshAssistData);
        FriendDataModel.Instance.AddEvent(FriendEvent.FriendAssistBossRefresh, InitBossView);
        FriendDataModel.Instance.AddEvent<int>(FriendEvent.FriendAssistPointRefresh, InitAssistReward);
        FriendDataModel.Instance.AddEvent(FriendEvent.FriendAssistCardRefresh, InitAssistCardview);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        FriendDataModel.Instance.RemoveEvent(FriendEvent.FriendAssistDataRefresh, OnRefreshAssistData);
        FriendDataModel.Instance.RemoveEvent(FriendEvent.FriendAssistBossRefresh, InitBossView);
        FriendDataModel.Instance.RemoveEvent<int>(FriendEvent.FriendAssistPointRefresh, InitAssistReward);
        FriendDataModel.Instance.RemoveEvent(FriendEvent.FriendAssistCardRefresh, InitAssistCardview);
    }

    private void InitAssistReward(int rewardNum)
    {
        List<ItemInfo> _listItemInfo = new List<ItemInfo>();
        ItemInfo _itemInfo = new ItemInfo();
        _itemInfo.Id = SpecialItemID.FriendShipPoint;
        _itemInfo.Value = rewardNum;
        _listItemInfo.Add(_itemInfo);
        GetItemTipMgr.Instance.ShowItemResult(_listItemInfo);
        InitAssistPointsView();
    }

    private void OnRefreshAssistData()
    {
        FriendAssistDataVO vo = FriendDataModel.Instance.mFriendAssistVO;
        InitBossView();
        InitAssistPointsView();
        InitAssistCardview();
    }

    private void InitAssistCardview()
    {
        _textDes.text = LanguageMgr.GetLanguage(5001537);
        FriendAssistDataVO vo = FriendDataModel.Instance.mFriendAssistVO;
        if (vo.mAssistRoleId > 0)
        {
            CardDataVO cardVO = HeroDataModel.Instance.GetCardDataByCardId(vo.mAssistRoleId);
            if (_assistCardItem == null)
            {
                _assistCardItem = CardViewFactory.Instance.CreateCardView(cardVO, CardViewType.Common);
                _assistCardItem.mRectTransform.SetParent(_assistCardRoot, false);
            }
            else
            {
                if (_assistCardItem.mCardDataVO.mCardID == vo.mAssistRoleId)
                    return;
                _assistCardItem.Show(cardVO);
            }
        }
        else
        {
            if (_assistCardItem != null)
            {
                CardViewFactory.Instance.ReturnCardView(_assistCardItem);
                _assistCardItem = null;
            }
        }
    }

    private void InitAssistPointsView()
    {
        FriendAssistDataVO vo = FriendDataModel.Instance.mFriendAssistVO;
        if (_pointRewardItem == null)
        {
            ItemInfo data = new ItemInfo();
            data.Id = SpecialItemID.FriendShipPoint;
            data.Value = vo.mAssistPointsReward;
            _pointRewardItem = ItemFactory.Instance.CreateItemView(data, ItemViewType.BagItem);
            _pointRewardItem.mRectTransform.SetParent(_rewardItemRoot, false);
        }
        else
        {
            _pointRewardItem.RefreshCount(vo.mAssistPointsReward);
        }
        _assistRewardBtn.gameObject.SetActive(vo.mAssistPointsReward > 0);
        _helpFriend.text = LanguageMgr.GetLanguage(6001144) + vo.mTotaGetPoint + "/" + GameConst.MaxFriendship;
    }

    private void InitBossView()
    {
        _skeleton.SetActive(false);
        FriendAssistDataVO vo = FriendDataModel.Instance.mFriendAssistVO;
        if (vo.mBossDataVO == null)
        {
            _bossRoot.SetActive(false);
            _funBtnText.text = LanguageMgr.GetLanguage(5001520);
        }
        else
        {
            if (_bossCardItem == null)
            {
                _bossCardItem = CardViewFactory.Instance.CreateCardView(vo.mBossDataVO.mBossCardVO, CardViewType.Common);
                _bossCardItem.mRectTransform.SetParent(_bossCardRoot, false);
            }
            else
                _bossCardItem.Show(vo.mBossDataVO.mBossCardVO);
            if (_bossDiamondItem == null)
            {
                ItemInfo info = new ItemInfo();
                info.Id = SpecialItemID.Diamond;
                info.Value = vo.mBossDataVO.mDiamondReward;
                _bossDiamondItem = ItemFactory.Instance.CreateItemView(info, ItemViewType.BagItem);
                _bossDiamondItem.mRectTransform.SetParent(_bossDiamondRoot, false);
            }
            else
            {
                _bossDiamondItem.RefreshCount(vo.mBossDataVO.mDiamondReward);
            }

            _bossRoot.SetActive(true);

            _hpPreText.text = vo.mBossDataVO.mBossHpPercent.ToString() + "%";
            _hpSlider.fillAmount = vo.mBossDataVO.mBossHpPercent / 100f;
            _funBtnText.text = LanguageMgr.GetLanguage(5001506);
        }

        if (vo.SearchBossRemainTime > 0)
        {
            if (!_searchCDText.gameObject.activeSelf)
                _searchCDText.gameObject.SetActive(true);
            _lastSearchTime = vo.SearchBossRemainTime;
            if (_searchKey != 0)
                TimerHeap.DelTimer(_searchKey);
            _searchKey = TimerHeap.AddTimer(0, 1000, OnSearchTimeCD);
        }
        else
        {
            if (_searchCDText.gameObject.activeSelf)
                _searchCDText.gameObject.SetActive(false);
        }
    }

    private void ClearCDTimer()
    {
        if (_searchKey != 0)
            TimerHeap.DelTimer(_searchKey);
        _searchKey = 0;
    }

    private void OnSearchTimeCD()
    {
        if (_lastSearchTime < 0)
        {
            ClearCDTimer();
            _searchCDText.gameObject.SetActive(false);
            return;
        }
        _searchCDText.text = LanguageMgr.GetLanguage(5001519) + " " + TimeHelper.GetCountTime(_lastSearchTime);
        _lastSearchTime--;
    }

    public override void Hide()
    {
        ClearCDTimer();
        base.Hide();
    }

    private void OnGetAssistReward()
    {
        GameNetMgr.Instance.mGameServer.ReqFriendAssistPoint();
    }

    private void OnSetAssistCard()
    {
        LineupSceneMgr.Instance.ShowLineupModule(TeamType.FriendBossAssist);
    }
    
    private void OnFunClick()
    {
        Action OnSend = () =>
        {
            _sidePanle.SetActive(false);
            GameNetMgr.Instance.mGameServer.ReqSearchFriendBoos();
        };

        FriendAssistDataVO vo = FriendDataModel.Instance.mFriendAssistVO;
        if (vo.mBossDataVO == null)
        {
            if (vo.SearchBossRemainTime <= 0)
            {
                _sidePanle.SetActive(true);
                _skeleton.SetActive(true);
                DelayCall(2f, OnSend);
            }
        }
        else
        {
            GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(FriendEvent.ChallengeFriendBoss, HeroDataModel.Instance.mHeroPlayerId);
            //int curStrCount = BagDataModel.Instance.GetItemCountById(SpecialItemID.FriendStrength);
            //if (curStrCount == 0)
            //{
            //    Debuger.LogWarning("体力不足");
            //    return;
            //}
            //LineupSceneMgr.Instance.ShowLineupModule(TeamType.FriendBoss, HeroDataModel.Instance.mHeroPlayerId, FriendDataModel.Instance.mFriendAssistVO.mBossDataVO.mBossConfigID);
        }
    }

    private void OnHelp()
    {
        HelpTipsMgr.Instance.ShowTIps(HelpType.FriendHelp);
    }

    public override void Dispose()
    {
        if (_pointRewardItem != null)
        {
            ItemFactory.Instance.ReturnItemView(_pointRewardItem);
            _pointRewardItem = null;
        }
        if (_bossDiamondItem != null)
        {
            ItemFactory.Instance.ReturnItemView(_bossDiamondItem);
            _bossDiamondItem = null;
        }
        if (_assistCardItem != null)
        {
            CardViewFactory.Instance.ReturnCardView(_assistCardItem);
            _assistCardItem = null;
        }
        if (_bossCardItem != null)
        {
            CardViewFactory.Instance.ReturnCardView(_bossCardItem);
            _bossCardItem = null;
        }
        base.Dispose();
    }
}