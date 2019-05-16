using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using NewBieGuide;
using Framework.UI;

public class RoleBagView : UILoopBaseView<CardDataVO>
{
    enum SortType
    {
        Rank,
        Level,
    }

    private Text _cardNum;
    private GameObject _cardImg;
    private KindGroup _kindGroup;
    private Toggle _tRole;
    private Toggle _tTujian;
    private Dropdown _dropDown;

    private int _curCampType = 0;
    private int _curToggleType = 0;
    private SortType _sortType = SortType.Rank;

    private List<CardDataVO> _allCardDatas;
    //private List<CardDataVO> _curShowCardDatas;

    //private RLoopScrollRect _loopScrollRect;
    private bool isOpen;

    protected override void ParseComponent()
	{
        base.ParseComponent();
        _cardNum = Find<Text>("StaticObject/ImgNum/CardNum");
        _cardImg = Find("StaticObject/ImgNum");
        _kindGroup = new KindGroup(OnKindChange);
        _kindGroup.SetDisplayObject(Find("uiKindGroup"));

        _tRole = Find<Toggle>("ToggleGroup/ToggleBag");
        _tTujian = Find<Toggle>("ToggleGroup/ToggleTujian");

        InitScrollRect("ScrollView");

        _dropDown = Find<Dropdown>("Dropdown");
        _dropDown.onValueChanged.AddListener(OnSortChange);

        //_loopScrollRect = Find<RLoopScrollRect>("ScrollView");
        //_loopScrollRect.mLoopProvider = this;

        _tRole.onValueChanged.Add((bool value) => { if (value) OnToggleChange(0); });
        _tTujian.onValueChanged.Add((bool value) => { if (value) OnToggleChange(1); });
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        HeroDataModel.Instance.AddEvent(HeroEvent.HeroCardChange, OnCardChange);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        HeroDataModel.Instance.RemoveEvent(HeroEvent.HeroCardChange, OnCardChange);
    }

    private void OnCardChange()
    {
        if (_curToggleType != 0)
            return;
        OnToggleChange(0);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _tTujian.gameObject.SetActive(GameConfigMgr.Instance.GetScreenConfig(ScreenType.Chart).Type == 0);
        _dropDown.options.Clear();
        _dropDown.options.Add(new Dropdown.OptionData(LanguageMgr.GetLanguage(5002705)));
        _dropDown.options.Add(new Dropdown.OptionData(LanguageMgr.GetLanguage(5002704)));
        _dropDown.captionText.text = _dropDown.options[(int)_sortType].text;
        OnToggleChange(_curToggleType);
        if (_lstShowViews.Count > 0)
            NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.RoleItemView, ((CardView)_lstShowViews[0]).GetBtnTransform());
    }

    private void OnToggleChange(int index)
    {
        _curToggleType = index;
        if (index == 0)
        {
            _tRole.interactable = false;
            _tTujian.interactable = true;
            _kindGroup.OnKindReset();
            _kindGroup.SetEmptyKindValue(true);
            _dropDown.gameObject.SetActive(true);
            _allCardDatas = HeroDataModel.Instance.mAllCards;
            _cardImg.SetActive(true);
            _cardNum.text = _allCardDatas.Count + "/" + GameConst.CardBagNum;
            if (!isOpen)
                _curCampType = 0;
            else
                isOpen = false;
            _kindGroup.SetKind(_curCampType);
        }
        else
        {
            _cardImg.SetActive(false);
            _tRole.interactable = true;
            _tTujian.interactable = false;
            _kindGroup.SetEmptyKindValue(false);
            _dropDown.gameObject.SetActive(false);
            _allCardDatas = GameConfigMgr.Instance.mHighestLvCards;
            if (_curCampType == 0 || !isOpen)
                _curCampType = 1;
            else
                isOpen = false;
            _kindGroup.SetKind(_curCampType);
        }
        OnKindChange(_curCampType); 
    }

    private void ShowCardView(List<CardDataVO> values)
    {
        _lstDatas = values;
        _lstDatas.Sort(SortCard);
        _loopScrollRect.ClearCells();
        _loopScrollRect.totalCount = values.Count;
        _loopScrollRect.RefillCells();
    }

    protected override UIBaseView CreateItemView()
    {
        return null;
    }

    public override  UIBaseView CreateNewItemView(int idx)
    {
        UIBaseView view;
        if (_uiViewPools.Count > 0)
        {
            view = _uiViewPools.Dequeue();
            view.Show(_lstDatas[idx]);
        }
        else
        {
            view = CardViewFactory.Instance.CreateCardView(_lstDatas[idx], CardViewType.Common, OnClick);
        }
        if (_curToggleType == 0)
        {
            (view as CardView).LockStatusMask = false;
        }
        else
        {
            CardConfig cfg = GameConfigMgr.Instance.GetCardConfig(_lstDatas[idx].mCardCfgId);
            (view as CardView).LockStatusMask = true;
            if (HeroDataModel.Instance.mlstRoleHandBook.Contains(cfg.ID))
                (view as CardView).LockStatusMask = false;
        }
        _lstShowViews.Add(view);
        return view;
    }

    private void OnClick(CardView view)
    {
        isOpen = true;
        RoleVO roleVO = new RoleVO();
        roleVO.OnCardVO(view.mCardDataVO);
        roleVO.OnLstCardVO(_lstDatas);
        roleVO.OnCampType(_curCampType);
        roleVO.OnDetailType(CardDetailConst.HeroInfo);
        GameUIMgr.Instance.OpenModule(ModuleID.RoleInfo, roleVO);
    }

    private void OnKindChange(int kind)
    {
        _curCampType = kind;
        List<CardDataVO> value;
        if(_curCampType == 0)
        {
            value = _allCardDatas;
        }
        else
        {
            value = new List<CardDataVO>();
            for (int i = 0; i < _allCardDatas.Count; i++)
            {
                if (_allCardDatas[i].mCardConfig.Camp == _curCampType)
                    value.Add(_allCardDatas[i]);
            }
        }
        ShowCardView(value);
    }

    private void OnSortChange(int id)
    {
        _sortType = id == 0 ? SortType.Rank : SortType.Level;
        ShowCardView(_lstDatas);
    }

    private int SortCard(CardDataVO v1, CardDataVO v2)
    {
        if (v1.mCardConfig.Rarity != v2.mCardConfig.Rarity)
        {
            if (_curToggleType == 0)
            {
                if (_sortType == SortType.Level)
                {
                    if (v1.mCardLevel == v2.mCardLevel)
                        return v1.mCardConfig.Rarity > v2.mCardConfig.Rarity ? -1 : 1;
                    else
                        return v1.mCardLevel > v2.mCardLevel ? -1 : 1;
                }
                else
                {
                    if (v1.mCardConfig.Rarity == v2.mCardConfig.Rarity)
                        return v1.mCardLevel > v2.mCardLevel ? -1 : 1;
                    else
                        return v1.mCardConfig.Rarity > v2.mCardConfig.Rarity ? -1 : 1;
                }
            }
            else
            {
                return v1.mCardConfig.Rarity > v2.mCardConfig.Rarity ? -1 : 1;
            }
        }
        else if (v1.mCardConfig.Rarity == v2.mCardConfig.Rarity && v1.mCardLevel != v2.mCardLevel)
        {
            if (_curToggleType == 0)
            {
                if (_sortType == SortType.Level)
                {
                    if (v1.mCardLevel == v2.mCardLevel)
                        return v1.mCardConfig.Rarity > v2.mCardConfig.Rarity ? -1 : 1;
                    else
                        return v1.mCardLevel > v2.mCardLevel ? -1 : 1;
                }
                else
                {
                    if (v1.mCardConfig.Rarity == v2.mCardConfig.Rarity)
                        return v1.mCardLevel > v2.mCardLevel ? -1 : 1;
                    else
                        return v1.mCardConfig.Rarity > v2.mCardConfig.Rarity ? -1 : 1;
                }
            }
            else
            {
                return v1.mCardLevel > v2.mCardLevel ? -1 : 1;
            }
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

    private void OnClose()
    {
        GameUIMgr.Instance.CloseModule(ModuleID.RoleBag);
    }

    public override void Dispose()
    {
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.RoleItemView);
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.RoleBagDisBtn);
        base.Dispose();
    }

    public override void Hide()
    {
        base.Hide();
        if (!isOpen && !_tRole.isOn)
        {
            _tTujian.isOn = false;
            _tRole.isOn = true;
        }
    }
}