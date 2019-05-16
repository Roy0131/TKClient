using NewBieGuide;
using System.Collections.Generic;
using UnityEngine.UI;
using Framework.UI;

public class RoleListView : UILoopBaseView<CardDataVO>
{
    private List<RarityItem> _lstRarityItems;
    private Text _roleCountText;
    private Text _tips;

    protected override void ParseComponent()
    {
        base.ParseComponent();

        RarityItem item;
        _lstRarityItems = new List<RarityItem>();
        for (int i = 1; i <= 5; i++)
        {
            item = new RarityItem(OnRarityItemClick);
            item.SetDisplayObject(Find("RarityGrid/Rarity" + i));
            _lstRarityItems.Add(item);
        }
        _roleCountText = Find<Text>("CountText");
        _tips = Find<Text>("Tips");
        InitScrollRect("RoleView");
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);

        for (int i = 0; i < _lstRarityItems.Count; i++)
            _lstRarityItems[i].Reset();//.SetSelectedStatus(DecomposeDataModel.Instance.CheckRaritySelected(_lstRarityItems[i].mRarity));
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        DecomposeDataModel.Instance.AddEvent(DecomposeEvent.RefreshRoleList, ShowCardView);
        DecomposeDataModel.Instance.AddEvent<int>(DecomposeEvent.RemoveRoleToDecompose, OnRemoveRole);
        DecomposeDataModel.Instance.AddEvent<int>(DecomposeEvent.AddRoleToDecompose, OnAddRole);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        DecomposeDataModel.Instance.RemoveEvent(DecomposeEvent.RefreshRoleList, ShowCardView);
        DecomposeDataModel.Instance.RemoveEvent<int>(DecomposeEvent.RemoveRoleToDecompose, OnRemoveRole);
        DecomposeDataModel.Instance.RemoveEvent<int>(DecomposeEvent.AddRoleToDecompose, OnAddRole);
    }

    private void OnRarityItemClick(RarityItem item)
    {
        if (item.mBlSelected)
            DecomposeDataModel.Instance.AddRarityCond(item.mRarity);
        else
            DecomposeDataModel.Instance.RemoveRarityCond(item.mRarity);
    }

    private void OnRemoveRole(int cardId)
    {
        for (int i = 0; i < _lstShowViews.Count; i++)
        {
            if (((CardView)_lstShowViews[i]).mCardDataVO.mCardID == cardId)
            {
                ((CardView)_lstShowViews[i]).BlSelected = false;
                ShowProgress();
            }
        }
    }

    private void OnAddRole(int cardId)
    {
        for (int i = 0; i < _lstShowViews.Count; i++)
        {
            if (((CardView)_lstShowViews[i]).mCardDataVO.mCardID == cardId)
            {
                ((CardView)_lstShowViews[i]).BlSelected = true;
                ShowProgress();
            }
        }
    }

    private void ShowProgress()
    {
        _roleCountText.text = LanguageMgr.GetLanguage(5002903) + DecomposeDataModel.Instance.mRoleTotalCount + "/" + HeroDataModel.Instance.mAllCards.Count;
    }

    private void ShowCardView()
    {
        ShowProgress();
        _loopScrollRect.ClearCells();
        _lstDatas = DecomposeDataModel.Instance.mlstAllCardDataVO;
        if (_lstDatas.Count == 0)
        {
            _tips.gameObject.SetActive(true);
            _tips.text = LanguageMgr.GetLanguage(5001408);
            return;
        }
        _tips.gameObject.SetActive(false);
        _loopScrollRect.totalCount = _lstDatas.Count;
        _loopScrollRect.RefillCells();
        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.DeComposeCardView, ((CardView)_lstShowViews[0]).GetBtnTransform());
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
            view = CardViewFactory.Instance.CreateCardView(_lstDatas[idx], CardViewType.Decompose, OnSelectCardView);
        }
        (view as CardView).BlSelected = DecomposeDataModel.Instance.BlCardContain(_lstDatas[idx].mCardID);//false;
        _lstShowViews.Add(view);
        return view;
    }

    private void OnSelectCardView(CardView view)
    {
        if (DecomposeDataModel.Instance.BlFullCard && view.BlSelected)
        {
            view.BlSelected = false;
            return;
        }
        if (view.BlSelected)
            DecomposeDataModel.Instance.AddVOToDecompose(view.mCardDataVO);
        else
            DecomposeDataModel.Instance.RemoveVOToDecompose(view.mCardDataVO);
    }

    public override void Hide()
    {
        base.Hide();
    }

    public override void Dispose()
    {
        if (_loopScrollRect != null)
        {
            _loopScrollRect.ClearCells();
            _loopScrollRect = null;
        }
        if (_lstRarityItems != null)
        {
            for (int i = 0; i < _lstRarityItems.Count; i++)
                _lstRarityItems[i].Dispose();
            _lstRarityItems.Clear();
            _lstRarityItems = null;
        }
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.DeComposeCardView);
        base.Dispose();
    }
}