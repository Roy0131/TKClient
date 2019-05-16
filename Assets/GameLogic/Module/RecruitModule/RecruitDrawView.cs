using Framework.UI;
using NewBieGuide;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecruitDrawView : UISoundViewBase
{
    private Text _callOneNumText;
    private Text _callTwoNumText;
    private Text _callText;
    private Image _callOneImg;
    private Image _callTwoImg;
    private Button _callOneBtn;
    private Button _callTwoBtn;
    private Button _disBtn;
    private GameObject _disObj;
    private GameObject _callOneObj;
    private GameObject _callTwoObj;
    private Image _drawImg;
    private RectTransform _parent;
    private RecruitDataVO _RecruitVO;

    private GameObject _speciallyObj;
    private UIEffectView _effect;
    private SkeletonGraphic _skeleSpine;

    private GameObject _rewardItemObject;

    private int drawType;
    private int drawId;
    private List<int> tableId;
    private bool _isFreeDraw;
    private int cindCount;


    protected override void ParseComponent()
    {
        base.ParseComponent();
        _callOneNumText = Find<Text>("CallOneBtn/Image/Text");
        _callTwoNumText = Find<Text>("CallTwoBtn/Image/Text");
        _callText = Find<Text>("CallOneBtn/Text");

        _callOneImg = Find<Image>("CallOneBtn/Image");
        _callTwoImg = Find<Image>("CallTwoBtn/Image");

        _callOneBtn = Find<Button>("CallOneBtn");
        _callTwoBtn = Find<Button>("CallTwoBtn");
        _disBtn = Find<Button>("DisBtn");

        _parent = Find<RectTransform>("Panel_Scroll/KnapsackPanel");

        _disObj = Find("DisBtn");
        _callOneObj = Find("CallOneBtn");
        _callTwoObj = Find("CallTwoBtn");
        _drawImg = Find<Image>("DrawImg");

        _speciallyObj = Find("fx_ui_chouka");
        _effect = CreateUIEffect(_speciallyObj, UILayerSort.WindowSortBeginner);
        _skeleSpine = Find<SkeletonGraphic>("shilianchou");

        _rewardItemObject = Find("Panel_Scroll/KnapsackPanel/RewardItem");

        _callOneBtn.onClick.Add(OnCallOne);
        _callTwoBtn.onClick.Add(OnCallTen);
        _disBtn.onClick.Add(OnDis);

        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.RecruitResultOkBtn, _disBtn.transform);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        RecruitDataModel.Instance.AddEvent(RecruitEvent.BagRefresh, OnRefresh);
        RecruitDataModel.Instance.AddEvent(RecruitEvent.HeroRefreshs, OnRefresh);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        RecruitDataModel.Instance.RemoveEvent(RecruitEvent.BagRefresh, OnRefresh);
        RecruitDataModel.Instance.RemoveEvent(RecruitEvent.HeroRefreshs, OnRefresh);
    }

    private void OnRefresh()
    {
        _RecruitVO = RecruitDataModel.Instance.GetDrawCardIdVO(drawId);
        OnDrawInit();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _callText.text = LanguageMgr.GetLanguage(5002505);
        GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.EndCondTrigger, EndConditionConst.RecriutResult);
        SoundMgr.Instance.PlayEffectSound("UI_jiuguan_buy");
        drawType = int.Parse(args[0].ToString());
        tableId = args[1] as List<int>;
        _isFreeDraw = (bool) args[2];
        drawId = (drawType - 1) / 2;
        _RecruitVO = RecruitDataModel.Instance.GetDrawCardIdVO(drawId);
        OnDrawInit();
        int id = 0;
        if (drawType == 3 || drawType == 4)
            id = 1;

        DelayCall(1.7f, id, CreateRewardItem);
        _effect.PlayEffect();
        _skeleSpine.AnimationState.SetAnimation(0, "animation", false);
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(RecruitEvent.HonorDraw);
    }

    private void CreateRewardItem(int id)
    {
        Dictionary<int, int> items = new Dictionary<int, int>();
        for (int i = 0; i < tableId.Count - id; i++)
            if (tableId[i] > 1000 && tableId[i] < 10000)
            {
                if (!items.ContainsKey(tableId[i]))
                    items.Add(tableId[i], 1);
                else
                    items[tableId[i]]++;
            }
        RecruitRewardItem view;
        List<int> itemId = new List<int>();
        ClearShowRewardItem();
        for (int i = 0; i < tableId.Count - id; i++)
        {
            if (itemId.Contains(tableId[i]))
                continue;
            itemId.Add(tableId[i]);
            for (int j = 0; j < items[tableId[i]]; j++)
            {
                view = GetRewardItem();
                view.mRectTransform.SetParent(_parent, false);
                view.Show(tableId[i]);
                _lstCurShowItems.Add(view);
            }
        }

        DelayCall(1.3f, () => _effect.StopEffect());
        DelayCall(0.6f, () => _drawImg.gameObject.SetActive(true));
        DelayCall(1.3f, OnBtn);
    }

    private void OnBtn()
    {
        _callOneObj.SetActive(drawType != 11);
        _callTwoObj.SetActive(drawType != 11);
        _disObj.SetActive(true);
        GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.EnterCondTrigger, NewBieGuide.EnterCondConst.DrawShow);
    }

    #region reward item pool

    private Queue<RecruitRewardItem> _lstRewardItemPools = new Queue<RecruitRewardItem>();
    private List<RecruitRewardItem> _lstCurShowItems = new List<RecruitRewardItem>();

    public RecruitRewardItem GetRewardItem()
    {
        if (_lstRewardItemPools.Count > 0)
            return _lstRewardItemPools.Dequeue();
        RecruitRewardItem view = new RecruitRewardItem();
        view.SetDisplayObject(Object.Instantiate(_rewardItemObject));
        return view;
    }

    private void ReturnRewardItem(RecruitRewardItem item)
    {
        item.Hide();
        ObjectHelper.AddChildToParent(item.mTransform, mTransform);
        _lstRewardItemPools.Enqueue(item);
    }

    public void ClearShowRewardItem()
    {
        for (int i = 0; i < _lstCurShowItems.Count; i++)
            ReturnRewardItem(_lstCurShowItems[i]);
        _lstCurShowItems.Clear();
    }

    #endregion

    public override void Dispose()
    {
        if (_lstCurShowItems != null)
        {
            ClearShowRewardItem();
            _lstCurShowItems = null;
        }
        if (_lstRewardItemPools != null)
        {
            while (_lstRewardItemPools.Count > 0)
                _lstRewardItemPools.Dequeue().Dispose();
            _lstRewardItemPools.Clear();
            _lstRewardItemPools = null;
        }
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.RecruitResultOkBtn);
        base.Dispose();
    }

    private void OnDrawInit()
    {
        ExtractConfig eXConfig = GameConfigMgr.Instance.GetExtractConfig(11);
        string[] Cond1 = eXConfig.ResCondition1.Split(',');
        int cindId = 0;

        for (int i = 0; i < Cond1.Length; i += 2)
        {
            if (Cond1.Length % 2 != 0)
                continue;
            cindId = int.Parse(Cond1[i]);
            cindCount = int.Parse(Cond1[i + 1]);
        }

        if (drawType == 11)
        {
            _disObj.transform.localPosition = new Vector3(0f, -80f, 0f);
        }
        else
        {
            _disObj.transform.localPosition = new Vector3(-250f, -80f, 0f);
            _callOneNumText.text = _RecruitVO.mOneCont.ToString();
            _callTwoNumText.text = _RecruitVO.mTenCont.ToString();
            _callOneImg.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(_RecruitVO.mOneId).UIIcon);
            _callTwoImg.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(_RecruitVO.mTenId).UIIcon);

            ObjectHelper.SetSprite(_callOneImg,_callOneImg.sprite);
            ObjectHelper.SetSprite(_callTwoImg,_callTwoImg.sprite);
        }
    }


    private void OnCallOne()
    {
        string str = "";
        if (_RecruitVO.mOneId == SpecialItemID.Diamond)
            str = LanguageMgr.GetLanguage(4000055);
        else if (_RecruitVO.mOneId == SpecialItemID.FriendShipPoint)
            str = LanguageMgr.GetLanguage(6001181);
        else if (_RecruitVO.mOneId == SpecialItemID.Vulgar)
            str = LanguageMgr.GetLanguage(6001182);
        else if (_RecruitVO.mOneId == SpecialItemID.High)
            str = LanguageMgr.GetLanguage(6001183);
        if (_RecruitVO.mOneId == 3)
        {
            if (HeroDataModel.Instance.mHeroInfoData.mDiamond >= _RecruitVO.mOneCont)
            {
                GameNetMgr.Instance.mGameServer.ReqDrawCard(_RecruitVO.mRecruitIndex * 2 + 1);
                TDPostDataMgr.Instance.DoCostDiamond(TDCostDiamondType.BuyRecruitOneCount, 1, _RecruitVO.mOneCont);
            }
            else
            {
                PopupTipsMgr.Instance.ShowTips(str);
            }
        }
        else
        {
            if (BagDataModel.Instance.GetItemCountById(_RecruitVO.mOneId) >= _RecruitVO.mOneCont)
                GameNetMgr.Instance.mGameServer.ReqDrawCard(_RecruitVO.mRecruitIndex * 2 + 1);
            else
                PopupTipsMgr.Instance.ShowTips(str);
        }
    }


    private void OnCallTen()
    {
        string str = "";
        if (_RecruitVO.mTenId == SpecialItemID.Diamond)
            str = LanguageMgr.GetLanguage(4000055);
        else if (_RecruitVO.mTenId == SpecialItemID.FriendShipPoint)
            str = LanguageMgr.GetLanguage(6001181);
        else if (_RecruitVO.mTenId == SpecialItemID.Vulgar)
            str = LanguageMgr.GetLanguage(6001182);
        else if (_RecruitVO.mTenId == SpecialItemID.High)
            str = LanguageMgr.GetLanguage(6001183);
        if (_RecruitVO.mTenId == 3)
        {
            if (HeroDataModel.Instance.mHeroInfoData.mDiamond >= _RecruitVO.mTenCont)
            {
                GameNetMgr.Instance.mGameServer.ReqDrawCard(_RecruitVO.mRecruitIndex * 2 + 2);
                TDPostDataMgr.Instance.DoCostDiamond(TDCostDiamondType.BuyRecruitTenCount, 1, _RecruitVO.mTenCont);
            }
            else
            {
                PopupTipsMgr.Instance.ShowTips(str);
            }
        }
        else
        {
            if (BagDataModel.Instance.GetItemCountById(_RecruitVO.mTenId) >= _RecruitVO.mTenCont)
                GameNetMgr.Instance.mGameServer.ReqDrawCard(_RecruitVO.mRecruitIndex * 2 + 2);
            else
                PopupTipsMgr.Instance.ShowTips(str);
        }
    }

    private void OnDis()
    {
        Hide();
        StopEffectSound("UI_jiuguan_buy");
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(RecruitEvent.DropOut);
    }
}

public class RecruitRewardItem : UIBaseView
{
    private UIEffectView _uiEffect;
    private CardView _cardView;
    private UIEffectView _uiEffectAnother;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _uiEffect = CreateUIEffect(Find("fx_ui_fanpai"), UILayerSort.WindowSortBeginner + 3);
    }

    public override void Show(params object[] args)
    {
        base.Show(args);
        if (_cardView != null)
            CardViewFactory.Instance.ReturnCardView(_cardView);
        int tableID = int.Parse(args[0].ToString());
        CardConfig cfg = GameConfigMgr.Instance.GetCardConfig(tableID * 100 + 1);
        _cardView = CardViewFactory.Instance.CreateCardView(tableID, CardViewType.ConfigCard);
        ObjectHelper.AddChildToParent(_cardView.mTransform, mTransform, false);
        _cardView.mDisplayObject.SetActive(false);
        _cardView.mRectTransform.localScale = new Vector2(1.5f, 1.5f);
        _cardView.IsSele = cfg.Rarity == 5;
        _uiEffect.PlayEffect(OnEffectEnd, 0.6f);
    }

    private void OnEffectEnd(UIEffectView effect)
    {
        _uiEffect.StopEffect();

        _cardView.mDisplayObject.SetActive(true);
        //_cardView.mRectTransform.DOScale(new Vector2(1.3f, 1.3f), 0.4f).SetEase(Ease.OutBounce);
        DGHelper.DoScale(_cardView.mRectTransform, new Vector2(1.3f, 1.3f), 0.4f, DGEaseType.OutBounce);
    }

    public override void Dispose()
    {
        if (_cardView != null)
            CardViewFactory.Instance.ReturnCardView(_cardView);
        _cardView = null;
        if (_uiEffect != null)
        {
            _uiEffect.Dispose();
            _uiEffect = null;
        }

        base.Dispose();
    }
}