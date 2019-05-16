using Framework.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Msg.ClientMessage;

public class LineupCardView : UILoopBaseView<CardDataVO>
{
    private List<CardDataVO> allCards;
    private KindGroup _kindGroup;
    private GridLayoutGroup _layoutGroup;
    private int _drageItemIndex;
    private Text _tips;

    protected override void ParseComponent()
    {
        base.ParseComponent();

        _layoutGroup = Find<GridLayoutGroup>("ScrollView/Content");
        InitScrollRect("ScrollView");

        _kindGroup = new KindGroup(OnChangeCamp);
        _kindGroup.SetDisplayObject(Find("KindGroup"));

        _tips = Find<Text>("Tpis");

        _lstCardViews = new List<CardView>();
    }

    #region show card logic
    protected override UIBaseView CreateItemView()
    {
        return null;
    }

    public override UIBaseView CreateNewItemView(int idx)
    {
        CardView view = CardViewFactory.Instance.CreateCardView(_lstCardDatas[idx], CardViewType.Lineup);
        DragHelper dragHelper = view.mDisplayObject.AddComponent<DragHelper>();
        dragHelper.mDragMethod = OnDragMethod;
        dragHelper.mRoleId = _lstCardDatas[idx].mCardID;
        view.BlSelected = LineupSceneMgr.Instance.CheckCardInBattle(_lstCardDatas[idx]);
        //远征专用
        if (LineupSceneMgr.Instance.mLineupTeamType == TeamType.Expedition)
        {
            view.OnExpeditionHp(100);
            List<ExpeditionSelfRole> listExpeditionSelfRole = new List<ExpeditionSelfRole>();
            listExpeditionSelfRole = ExpeditionDataModel.Instance.mListExpeditionSelfRole;
            for (int i = 0; i < listExpeditionSelfRole.Count; i++)
            {
                if (_lstCardDatas[idx].mCardID == listExpeditionSelfRole[i].Id)
                {
                    if (listExpeditionSelfRole[i].HpPercent > 0)
                    {
                        view.OnExpeditionHp(listExpeditionSelfRole[i].HpPercent);
                        if (listExpeditionSelfRole[i].Weak > 0)
                        {
                            view.IsExpeditionSele = true;
                            dragHelper.enabled = false;
                            view.OnExpeditionSeleName(LanguageMgr.GetLanguage(5003406));
                        }
                        else
                        {
                            view.IsExpeditionSele = false;
                        }
                    }
                    else
                    {
                        view.OnExpeditionHp(0);
                        dragHelper.enabled = false;
                        view.IsExpeditionSele = true;
                        view.OnExpeditionSeleName(LanguageMgr.GetLanguage(5003407));
                    }
                }
            }
        }
        _lstCardViews.Add(view);
        return view;
    }

    public override void RetItemView(UIBaseView view)
    {
        _lstCardViews.Remove(view as CardView);
        DragHelper drag = view.mDisplayObject.GetComponent<DragHelper>();
        if (drag != null)
            GameObject.Destroy(drag);
         
        CardViewFactory.Instance.ReturnCardView(view as CardView);
    }

    private List<CardDataVO> _lstCardDatas;
    private List<CardView> _lstCardViews;
    private void RefreshCard(List<CardDataVO> lstShowCards)
    {
        DiposeChildren();
        _lstCardDatas = lstShowCards;
        _tips.gameObject.SetActive(_lstCardDatas.Count == 0 || _lstCardDatas == null);
        _lstCardDatas.Sort(SortCard);

        _loopScrollRect.totalCount = _lstCardDatas.Count;
        _loopScrollRect.RefillCells();
        InputController.Instance.AddInputEvent(InputEventType.MouseUp, OnMouseUp);
        if(_lstCardViews.Count >= 2)
        {
            for (int i = 0; i < 2; i++)
                NewBieGuide.NewBieGuideMgr.Instance.RegisteLineupCardTransform(i, _lstCardViews[i].mTransform);
        }
        else if(_lstCardViews.Count >= 1)
        {
            NewBieGuide.NewBieGuideMgr.Instance.RegisteLineupCardTransform(0, _lstCardViews[0].mTransform);
        }
    }

    protected override void DiposeChildren()
    {
        NewBieGuide.NewBieGuideMgr.Instance.UnRegisteLineupCardTransform(0);
        NewBieGuide.NewBieGuideMgr.Instance.UnRegisteLineupCardTransform(1);
        _loopScrollRect.ClearCells();
        _lstCardDatas = null;
        InputController.Instance.RemoveInputEvent(InputEventType.MouseUp, OnMouseUp);
    }
    #endregion

    private void OnLineUpChange()
    {
        int i;
        for (i = 0; i < _lstCardViews.Count; i++)
        {
            (_lstCardViews[i] as CardView).BlSelected = false;
        }
        LineupFighter fighter;
        CardView view;
        for (i = 0; i < 9; i++)
        {
            fighter = LineupSceneMgr.Instance.GetFighterDataByIndex(i);
            if (fighter == null)
                continue;
            for (int j = 0; j < _lstCardViews.Count; j++)
            {
                view = _lstCardViews[j] as CardView;
                if (view.mCardDataVO == fighter.mCardDataVO)
                    view.BlSelected = true;
            }
        }
    }

    #region Drag operate
    protected override void AddEvent()
    {
        base.AddEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(UIEventDefines.LineupChangeIndex, OnLineUpChange);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(UIEventDefines.LineupChangeIndex, OnLineUpChange);
    }

    private void OnMouseUp(Vector2 pos)
    {
        if (_curHelper == null)
            return;
        if (_key != 0)
        {
            TimerHeap.DelTimer(_key);
            _key = 0;
        }
        if (_blDragging)
        {
            _curDragCardView.mRectTransform.SetParent(_loopScrollRect.content, false);
            _curDragCardView.mRectTransform.SetSiblingIndex(_drageItemIndex);
            _loopScrollRect.enabled = true;
            _layoutGroup.enabled = true;
            LineUpDragMgr.Instance.EndDrag();
        }
        LineUpDragMgr.Instance.HideDragFighter();
        _blDragging = false;
        _curHelper = null;
        _curDragCardView = null;
    }

    private uint _key = 0;
    private bool _blDragging = false;
    private DragHelper _curHelper;
    private float _flDownTime = 0f;
    private CardView _curDragCardView;

    private CardView GetCardViewById(int id)
    {
        for (int i = 0; i < _lstCardViews.Count; i++)
        {
            if (_lstCardViews[i].mCardDataVO.mCardID == id)
                return _lstCardViews[i];
        }
        return null;
    }

    private void OnDragMethod(DragEventType type, PointerEventData evtData, DragHelper helper)
    {
        if (evtData.button != PointerEventData.InputButton.Left)
            return;
        _curDragCardView = GetCardViewById(helper.mRoleId);
        if (_curDragCardView == null)
            return;
        Action OnDragItem = () =>
        {
            _layoutGroup.enabled = false;
            _loopScrollRect.enabled = false;
            _blDragging = true;
            _drageItemIndex = _curDragCardView.mRectTransform.GetSiblingIndex();
            LogHelper.LogWarning("_drageItemIndex:" + _drageItemIndex);
            LineUpDragMgr.Instance.mDragCardView = _curDragCardView;
            LineUpDragMgr.Instance.StartDrag(evtData, helper, DragStatus.DragCard);
            TimerHeap.DelTimer(_key);
            _key = 0;
        };

        switch(type)
        {
            case DragEventType.MouseDown:
                if (!NewBieGuide.NewBieGuideMgr.Instance.mBlGuideForce)
                {
                    if (Time.realtimeSinceStartup - _flDownTime < 0.5f)
                    {
                        //double click
                        if (LineupSceneMgr.Instance.OnDragCount() < LineupSceneMgr.Instance.OnGetMaxRole() && !_curDragCardView.BlSelected)
                        {
                            LineupSceneMgr.Instance.QuickAddFigther(_curDragCardView.mCardDataVO);
                            GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(UIEventDefines.LineupChangeIndex);
                        }
                        else
                        {
                            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001256) + LineupSceneMgr.Instance.OnGetMaxRole());
                        }
                        RemoveDragTimer();
                        return;
                    }
                    _flDownTime = Time.realtimeSinceStartup;
                }
                if (_curDragCardView.BlSelected)
                    return;
                if (_key > 0)
                    return;
				_curHelper = helper;
                _blDragging = false;
                _key = TimerHeap.AddTimer(100, 0, OnDragItem);
                break;
            case DragEventType.MouseUp:
                if (!NewBieGuide.NewBieGuideMgr.Instance.mBlGuideForce)
                {
                    if (Time.realtimeSinceStartup - _flDownTime < 0.2f)
                    {
                        if (_curDragCardView.BlSelected)
                        {
                            LineupSceneMgr.Instance.QuickRemoveFighter(_curDragCardView.mCardDataVO);
                            GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(UIEventDefines.LineupChangeIndex);
                            RemoveDragTimer();
                            return;
                        }
                    }
                }
                break;
            case DragEventType.BeginDrag:
                if (_curDragCardView.BlSelected)
                {
                    _loopScrollRect.OnBeginDrag(evtData);
                    return;
                }
                if (!_blDragging)
                {
                    RemoveDragTimer();
                    _blDragging = false;
                    _loopScrollRect.OnBeginDrag(evtData);
                }
                break;
            case DragEventType.Dragging:
                if (_curDragCardView.BlSelected)
                {
                    _loopScrollRect.OnDrag(evtData);
                    return;
                }
                if (!_blDragging)
                {
                    RemoveDragTimer();
                    _loopScrollRect.OnDrag(evtData);
                }
                break;
            case DragEventType.EndDrag:
                RemoveDragTimer();
                if (!_blDragging)
                    _loopScrollRect.OnEndDrag(evtData);
                break;
        }
    }

    private void RemoveDragTimer()
    {
        if (_key > 0)
        {
            TimerHeap.DelTimer(_key);
            _key = 0;
        }
        _flDownTime = 0f;
    }
    #endregion

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        OnChangeCamp(_kindGroup.KindID);
        _tips.text = LanguageMgr.GetLanguage(4000133);
    }

    private void OnChangeCamp(int kindValue)
    {
        if (LineupSceneMgr.Instance.mLineupTeamType == TeamType.Expedition)
        {
            _layoutGroup.spacing = new Vector2(25, 30);
            _layoutGroup.padding.bottom = 25;
        }
        else
        {
            _layoutGroup.spacing = new Vector2(25, 17);
            _layoutGroup.padding.bottom = 0;
        }

        List<CardDataVO> values = new List<CardDataVO>();
        List<CardDataVO> allCards = HeroDataModel.Instance.OnAllCard();
        if (kindValue == 0)
        {
            values = allCards;
        }
        else
        {
            for (int i = 0; i < allCards.Count; i++)
            {
                if (allCards[i].mCardConfig.Camp == kindValue)
                    values.Add(allCards[i]);
            }
        }

        RefreshCard(values);
    }

    public int SortCard(CardDataVO v1, CardDataVO v2)
    {
        if (v1.mCardLevel != v2.mCardLevel)
        {
            return v1.mCardLevel > v2.mCardLevel ? -1 : 1;
        }
        else if (v1.mCardLevel == v2.mCardLevel && v1.mCardConfig.Rarity != v2.mCardConfig.Rarity)
        {
            return v1.mCardConfig.Rarity > v2.mCardConfig.Rarity ? -1 : 1;
        }
        else if (v1.mCardConfig.Rarity == v2.mCardConfig.Rarity && v1.mCardLevel == v2.mCardLevel && v1.mCardConfig.Type != v2.mCardConfig.Type)
        {
            return v1.mCardConfig.Type > v2.mCardConfig.Type ? 1 : -1;
        }
        else if (v1.mCardConfig.Rarity == v2.mCardConfig.Rarity && v1.mCardLevel == v2.mCardLevel && v1.mCardConfig.Type == v2.mCardConfig.Type
            && v1.mCardConfig.Camp != v2.mCardConfig.Camp)
        {
            int[] campSort = { 1, 3, 6, 4, 2, 5 };
            int curCamp1 = campSort[v1.mCardConfig.Camp - 1];
            int curCamp2 = campSort[v2.mCardConfig.Camp - 1];
            return curCamp1 > curCamp2 ? 1 : -1;
        }
        else if (v1.mCardConfig.Rarity == v2.mCardConfig.Rarity && v1.mCardLevel == v2.mCardLevel && v1.mCardConfig.Type == v2.mCardConfig.Type
            && v1.mCardConfig.Camp == v2.mCardConfig.Camp && v1.mCardConfig.ID != v2.mCardConfig.ID)
        {
            return v1.mCardConfig.ID > v2.mCardConfig.ID ? -1 : 1;
        }
        else if (v1.mCardConfig.Rarity == v2.mCardConfig.Rarity && v1.mCardLevel == v2.mCardLevel && v1.mCardConfig.Type == v2.mCardConfig.Type
            && v1.mCardConfig.Camp == v2.mCardConfig.Camp && v1.mCardConfig.ID == v2.mCardConfig.ID && v1.mBattlePower != v2.mBattlePower)
        {
            return v1.mBattlePower > v2.mBattlePower ? -1 : 1;
        }
        else
        {
            return 0;
        }
    }
}
