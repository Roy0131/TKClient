using Framework.UI;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RecruitView : UISoundViewBase//UIBaseView
{
    private Text _diamindText;
    private Text _fillText;
    private Image _diamindImg;
    private Image _filledImg;
    private Button _diamindBtn;
    private Button _DrawBtn;
    private Button _probabHelpBtn;
    private Button _drawHelpBtn;
    private List<RecruitDataVO> _listRecruitVO;
    private List<RecruitItemView> _recruitItemViews;
    private RecruitDrawView _recruitDrawView;

    private GameObject _drawCardObj;

    private int cindCount = 0;
    private int drawType = 0;

    private RectTransform spineObj;
    private GameObject _drawObj;
    private RectTransform _drawObjBg;


    private GameObject _disObj;
    private GameObject _callOneObj;
    private GameObject _callTwoObj;
    private GameObject _drawImg;
    private SkeletonGraphic _skeleSpine;

    private bool isDraw = true;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _diamindText = Find<Text>("OpenObj/HeroExpTotal/TotalHeroExpLabel");
        _fillText = Find<Text>("OpenObj/Img/Text");
        _diamindImg = Find<Image>("OpenObj/HeroExpTotal/Btn_Diamond");
        _filledImg = Find<Image>("OpenObj/Img/ImgFilled");
        _diamindBtn = Find<Button>("OpenObj/HeroExpTotal/BtnDiamondAdd");
        _DrawBtn = Find<Button>("OpenObj/Img/RecrBtn");
        _probabHelpBtn = Find<Button>("OpenObj/ProbabHelp");
        _drawHelpBtn = Find<Button>("OpenObj/DrawHelp");

        _drawCardObj = Find("DrawCardObj");

        spineObj = Find<RectTransform>("SkeletonGraphic");
        _drawObj = Find("DrawCardObj/DrawCard");
        _drawObjBg = Find<RectTransform>("DrawObj");


        _disObj = Find("DrawObj/DisBtn");
        _callOneObj = Find("DrawObj/CallOneBtn");
        _callTwoObj = Find("DrawObj/CallTwoBtn");
        _drawImg = Find("DrawObj/DrawImg");

        _recruitDrawView = new RecruitDrawView();
        _recruitDrawView.SetDisplayObject(Find("DrawObj"));

        _skeleSpine = Find<SkeletonGraphic>("SkeletonGraphic");
        _diamindBtn.onClick.Add(OnDiamind);
        _DrawBtn.onClick.Add(OnDraw);
        _probabHelpBtn.onClick.Add(OnProbabHelp);
        _drawHelpBtn.onClick.Add(OnDrawHelp);

        RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.NormalDraw, Find("DrawCardObj/DrawCard/DiamindObj1/CallBtn/RedPoint"));
        RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.AdvanceDraw, Find("DrawCardObj/DrawCard/DiamindObj2/CallBtn/RedPoint"));


        OnRecruitInit();
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        RecruitDataModel.Instance.AddEvent<int, List<int>,bool>(RecruitEvent.DrawCard, OnDrawCards);
        RecruitDataModel.Instance.AddEvent(RecruitEvent.BagRefresh, OnRefresh);
        RecruitDataModel.Instance.AddEvent(RecruitEvent.HeroRefreshs, OnRefresh);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(RecruitEvent.DropOut, OnDrawDrop);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(RecruitEvent.HonorDraw, OnHonorDraw);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        RecruitDataModel.Instance.RemoveEvent<int, List<int>,bool>(RecruitEvent.DrawCard, OnDrawCards);
        RecruitDataModel.Instance.RemoveEvent(RecruitEvent.BagRefresh, OnRefresh);
        RecruitDataModel.Instance.RemoveEvent(RecruitEvent.HeroRefreshs, OnRefresh);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(RecruitEvent.DropOut, OnDrawDrop);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(RecruitEvent.HonorDraw, OnHonorDraw);
    }

    private void OnHonorDraw()
    {
        _DrawBtn.enabled = false;
        DelayCall(3f, () => _DrawBtn.enabled = true);
    }

    private void OnDrawDrop()
    {
        _drawCardObj.SetActive(true);
        _blNormalWindow = true;
        #region 设置初始位置
        spineObj.anchoredPosition = new Vector2(0f, -497f);
        _drawObjBg.anchoredPosition = new Vector2(0f, -850f);
        _drawObjBg.gameObject.SetActive(false);
        #endregion
        _diamindText.text = UnitChange.GetUnitNum(HeroDataModel.Instance.mHeroInfoData.mDiamond);
        _diamindImg.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(SpecialItemID.Diamond).UIIcon);
        ObjectHelper.SetSprite(_diamindImg,_diamindImg.sprite);
    }

    private bool _blNormalWindow = true;

    private void OnDrawCards(int drawId, List<int> tabId,bool isFreeDraw)
    {
        StopEffectSoundName(UIModuleSoundName.RecruitSoundName);
        drawType = (drawId - 1) / 2;
        Action OnMoveEnd = () =>
        {
            _recruitDrawView.Show(drawId, tabId, isFreeDraw);
        };

        #region 播放图片位移动画
        _drawCardObj.SetActive(false);
        _disObj.SetActive(false);
        _callOneObj.SetActive(false);
        _callTwoObj.SetActive(false);
        _drawImg.SetActive(false);

        _recruitDrawView.ClearShowRewardItem();
        if (_blNormalWindow)
        {
            _blNormalWindow = false;
            _drawObjBg.gameObject.SetActive(true);
            //spineObj.DOLocalMove(new Vector2(0, -1355), 0.3f);
            //_drawObjBg.DOLocalMoveY(-145, 0.4f);
            DGHelper.DoLocalMove(spineObj, new Vector2(0, -1355), 0.3f);
            DGHelper.DoLocalMoveY(_drawObjBg, -145, 0.4f);
            DelayCall(0.3f, OnMoveEnd);
        }
        else
        {
            _drawObjBg.gameObject.SetActive(true);
            OnMoveEnd();
        }
        #endregion

    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        drawType = 0;
        OnRecruitChange();
        OnInitRecruit();
        _skeleSpine.AnimationState.SetAnimation(0, "animation2", false);
        _skeleSpine.AnimationState.AddAnimation(0, "animation", true,3.6f);
    }
    
    private void OnRefresh()
    {
        OnRecruitChange();
        OnInitRecruit();
    }

    private void OnDiamind()
    {
        GameUIMgr.Instance.OpenModule(ModuleID.Recharge);
    }

    private void OnDraw()
    {
        //给服务器发一条抽卡消息
        if (BagDataModel.Instance.GetItemCountById(SpecialItemID.Honor)>= cindCount)
        {
            if (isDraw)
            {
                isDraw = false;
                GameNetMgr.Instance.mGameServer.ReqDrawCard(11);
            }
            DelayCall(1f, () => isDraw = true);
        }
        else
        {
            HelpTipsMgr.Instance.ShowTIps(HelpType.HighDrawHelp);
        }
    }

    private void OnProbabHelp()
    {
        HelpTipsMgr.Instance.ShowTIps(HelpType.DrawProHelp);
    }

    private void OnDrawHelp()
    {
        HelpTipsMgr.Instance.ShowTIps(HelpType.DrawHelp);
    }

    private void OnInitRecruit()
    {
        if (drawType == 2)
        {
            _diamindText.text = UnitChange.GetUnitNum(BagDataModel.Instance.GetItemCountById(SpecialItemID.FriendShipPoint));
            _diamindImg.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(SpecialItemID.FriendShipPoint).UIIcon);
        }
        else
        {
            _diamindText.text = UnitChange.GetUnitNum(HeroDataModel.Instance.mHeroInfoData.mDiamond);
            _diamindImg.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(SpecialItemID.Diamond).UIIcon);
        }
        ObjectHelper.SetSprite(_diamindImg,_diamindImg.sprite);
        ExtractConfig eXConfig = GameConfigMgr.Instance.GetExtractConfig(11);
        string[] Cond1 = eXConfig.ResCondition1.Split(',');
        int cindId=0;
        
        for (int i = 0; i < Cond1.Length; i += 2)
        {
            if (Cond1.Length % 2 != 0)
                continue;
            cindId = int.Parse(Cond1[i]);
            cindCount = int.Parse(Cond1[i + 1]);
        }
        _filledImg.fillAmount = (float)BagDataModel.Instance.GetItemCountById(cindId) / (float)cindCount;
        _fillText.text = (BagDataModel.Instance.GetItemCountById(cindId) + "/" + cindCount);
    }

    private void ClearRecruitItem()
    {
        if (_recruitItemViews != null)
        {
            for (int i = 0; i < _recruitItemViews.Count; i++)
                _recruitItemViews[i].Dispose();
            _recruitItemViews.Clear();
            _recruitItemViews = null;
        }
    }

    private void OnRecruitInit()
    {
        _recruitItemViews = new List<RecruitItemView>();
        for (int i = 0; i < 3; i++)
        {
            GameObject obj = Find("DrawCardObj/DrawCard/DiamindObj" + (i + 1));
            RecruitItemView recruitItemView = new RecruitItemView();
            recruitItemView.SetDisplayObject(obj);
            _recruitItemViews.Add(recruitItemView);
        }
    }

    private void OnRecruitChange()
    {
        _listRecruitVO = RecruitDataModel.Instance.mAllRecruits;
        if (_listRecruitVO == null)
            return;
        for (int i = 0; i < _listRecruitVO.Count; i++)
            _recruitItemViews[i].Show(_listRecruitVO[i]);
    }

    public override void Dispose()
    {
        ClearRecruitItem();
        if (_listRecruitVO != null)
        {
            _listRecruitVO.Clear();
            _listRecruitVO = null;
        }

        base.Dispose();
    }

    protected override void OnShowViewAnimation()
    {
        base.OnShowViewAnimation();
        ObjectHelper.AnimationMoveBack(_drawCardObj.transform,ObjectHelper.direction.up);
    }
}
