using Framework.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleSelectView : UILoopBaseView<CardDataVO>
{
    private Text _tips;
    private FusionMatDataVO _vo;
    private RectTransform _cardRoot;
    private Button _backBtn;
    private Button _okBtn;
    private Button _resetBtn;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _tips = Find<Text>("Tips");
        _cardRoot = Find<RectTransform>("ScrollView/Content");
        _backBtn = Find<Button>("Btn_Back");
        _okBtn = Find<Button>("BtnGrid/OkBtn");
        _resetBtn = Find<Button>("BtnGrid/ResetBtn");

        InitScrollRect("ScrollView");

        _backBtn.onClick.Add(OnClose);
        _okBtn.onClick.Add(OnClose);
        _resetBtn.onClick.Add(OnReset);

        ColliderHelper.SetButtonCollider(_backBtn.transform);
    }

    private void OnClose()
    {
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(UIEventDefines.FusionMatSelectOK, _vo.mIndex);
    }

    private bool EqualsTableId(CardDataVO vo)
    {
        return _vo.mCardTableId == 0 ? true : vo.mCardTableId == _vo.mCardTableId;
    }

    private bool EqualsCamp(CardDataVO vo)
    {
        return _vo.mCampCond == 0 ? true : vo.mCardConfig.Camp == _vo.mCampCond;
    }

    private bool EqualsType(CardDataVO vo)
    {
        return _vo.mTypeCond == 0 ? true : vo.mCardConfig.Type == _vo.mTypeCond;
    }

    private bool EqualsStar(CardDataVO vo)
    {
        return _vo.mStarCond == 0 ? true : vo.mCardConfig.Rarity == _vo.mStarCond;
    }

    protected void OnReset()
    {
        if (_vo == null)
            return;
        for (int i = 0; i < _lstShowViews.Count; i++)
        {
            _vo.RemoveCardData(((CardView)_lstShowViews[i]).mCardDataVO.mCardID);
            ((CardView)_lstShowViews[i]).BlSelected = false;
        }
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _vo = args[0] as FusionMatDataVO;
        List<int> lstUsedCardIDS = args[1] as List<int>;
        List<CardDataVO> lstCards = HeroDataModel.Instance.mAllCards;
        List<CardDataVO> result = new List<CardDataVO>();
        int i = 0;
        for (i = 0; i < lstCards.Count; i++)
        {
            if (lstCards[i].mCardID == _vo.mMainCardId)
                continue;
            if (lstUsedCardIDS.Contains(lstCards[i].mCardID) && !_vo.BlContainId(lstCards[i].mCardID))
                continue;
            if (!EqualsTableId(lstCards[i]))
                continue;
            if (!EqualsCamp(lstCards[i]))
                continue;
            if (!EqualsType(lstCards[i]))
                continue;
            if (!EqualsStar(lstCards[i]))
                continue;
            result.Add(lstCards[i]);
        }
        _lstDatas = result;
        _loopScrollRect.ClearCells();
        if (_lstDatas.Count == 0)
        {
            _tips.gameObject.SetActive(true);
            return;
        }
        result.Sort(SortCard);
        _tips.gameObject.SetActive(false);

        _loopScrollRect.totalCount = _lstDatas.Count;
        _loopScrollRect.RefillCells();
    }

    protected override UIBaseView CreateItemView()
    {
        return null;
    }

    public override UIBaseView CreateNewItemView(int idx)
    {
        UIBaseView view;
        if (_uiViewPools.Count > 0)
        {
            view = _uiViewPools.Dequeue();
            view.Show(_lstDatas[idx]);
        }
        else
        {
            view = CardViewFactory.Instance.CreateCardView(_lstDatas[idx], CardViewType.FusionMat, OnClick);
        }
        ((CardView)view).BlSelected = _vo.BlContainId(_lstDatas[idx].mCardID);
        _lstShowViews.Add(view);
        return view;
    }

    public override void RetItemView(UIBaseView view)
    {
        ((CardView)view).BlSelected = false;
        base.RetItemView(view);
    }

    private int SortCard(CardDataVO v1, CardDataVO v2)
    {
        if (_vo.mCampCond > 0)
        {
            if (v1.mCardConfig.Type != v2.mCardConfig.Type)
                return v1.mCardConfig.Type < v2.mCardConfig.Type ? -1 : 1;
            else
                return v1.mCardConfig.ID < v2.mCardConfig.ID ? -1 : 1;
        }
        else
        {
            if (v1.mCardConfig.Camp != v2.mCardConfig.Camp)
                return v1.mCardConfig.Camp < v2.mCardConfig.Camp ? -1 : 1;
            else
                return v1.mCardConfig.ID < v2.mCardConfig.ID ? -1 : 1;
        }
    }

    private void OnClick(CardView view)
    {
        if (view.mCardDataVO.mBlLock)
            return;
        if (view.mCardDataVO.mBlInBattle)
            return;
        if (view.BlSelected)
        {
            if (_vo.BlMatEnough)
            {
                view.BlSelected = false;
                PopupTipsMgr.Instance.ShowTips("You've chosen enough characters");
                return;
            }
            _vo.AddCardData(view.mCardDataVO.mCardID);
        }
        else
        {
            _vo.RemoveCardData(view.mCardDataVO.mCardID);
        }
    }
}
