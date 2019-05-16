using Framework.UI;
using Msg.ClientMessage;
using NewBieGuide;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetItemView : UIBaseView
{
    public enum ResultType
    {
        role,
        items,
        roleAndItems,
    }
    
    private List<UIBaseView> _lstItemViews;

    private RectTransform _bigRootRect;
    private RectTransform _smallRootRect;
    private GridLayoutGroup _gridLayoutGroup;
    private Text _bigTitle;
    private Text _smallTitle;
    private Button _bigBtn;
    private Button _smallBtn;
    private Button _panelBtn;

    private GameObject _bigObject;
    private GameObject _smallObject;
    private ResultType _type;

    private GameObject _speciallyObj;
    private UIEffectView _effect;
    private Transform _root;
    protected override void ParseComponent()
    {
        base.ParseComponent();
        _root = Find<Transform>("GetItem/Root");
        //_bigTitle = Find<Text>("GetItemMul/Title");
        _smallTitle = Find<Text>("GetItem/Root/Title");
        //_bigRootRect = Find<RectTransform>("GetItemMul/ScrollView/Content");
        _smallRootRect = Find<RectTransform>("GetItem/Root/ScrollView/Content");
        _gridLayoutGroup = Find<GridLayoutGroup>("GetItem/Root/ScrollView/Content");
        //_bigBtn = Find<Button>("GetItemMul/ButtonOk");
        _smallBtn = Find<Button>("GetItem/Root/ButtonOk");
        _panelBtn = Find<Button>("GetItem/BlackBack");

        //_bigObject = Find("GetItemMul");
        _smallObject = Find("GetItem");

        _speciallyObj = Find("fx_ui_dianjin");
        _effect = CreateUIEffect(_speciallyObj, UILayerSort.TopSortBeginner);
        
        //_bigBtn.onClick.Add(OnClose);
        _smallBtn.onClick.Add(OnClose);
        _panelBtn.onClick.Add(OnClose);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        DelayCall(0.4f, () => NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.GetItemSureBtn, _smallBtn.transform));
        DelayCall(0.4f, () => GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.EndCondTrigger, NewBieGuide.EndConditionConst.EquipForgeAnimationEnd));
    }

    private RectTransform _curRootRect;
    private Text _curText;
    
    private void InitComponent(int count)
    {
        _curText = _smallTitle;
        _curRootRect = _smallRootRect;
        if (count > 4)
        {
            _curRootRect.pivot = new Vector2(0.5f, 1f);
            _gridLayoutGroup.childAlignment = TextAnchor.UpperLeft;
        }
        else
        {
            _curRootRect.pivot = new Vector2(0.5f, 0.5f);
            _gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
        }
        //_bigObject.SetActive(count > 1);
        //_smallObject.SetActive(count <= 1);
        //if (count > 1)
        //{
        //    _curText = _bigTitle;
        //    _curRootRect = _bigRootRect;
        //}
        //else
        //{
        //    _curText = _smallTitle;
        //    _curRootRect = _smallRootRect;
        //}
    }

    private void OnClose()
    {
        GetItemTipMgr.Instance.CloseItemView(this);
        _effect.StopEffect();
    }

    private IList<ItemInfo> _items;
    public void ShowRoleAndItems(int roleId, IList<ItemInfo> items)
    {
        _type = ResultType.roleAndItems;
        _items = items;
        CreateRoleView(roleId);
    }

    public void ShowRoleResult(int roleId)
    {
        _type = ResultType.role;
        InitComponent(1);
        CreateRoleView(roleId);
    }

    private void CreateRoleView(int roleId)
    {
        ClearView();
        CardDataVO vo = HeroDataModel.Instance.GetCardDataByCardId(roleId);
        if (vo == null)
        {
            LogHelper.LogWarning("[GetItemView.ShowRoleResult() => roleId:" + roleId + " data not found!!!]");
            return;
        }
        _lstItemViews = new List<UIBaseView>();
        CardView view = CardViewFactory.Instance.CreateCardView(vo, CardViewType.Common);
        view.mRectTransform.SetParent(_curRootRect, false);
        _lstItemViews.Add(view);
        _curText.text = LanguageMgr.GetLanguage(6001253);
    }

    private void ClearView()
    {
        if (_lstItemViews == null)
            return;
        for (int i = 0; i < _lstItemViews.Count; i++)
        {
            if (_lstItemViews[i].GetType() == typeof(CardView))
                CardViewFactory.Instance.ReturnCardView(_lstItemViews[i] as CardView);
            else
                ItemFactory.Instance.ReturnItemView(_lstItemViews[i] as ItemView);
        }
        _lstItemViews.Clear();
        _lstItemViews = null;
    }

    public void ShowItemResult(IList<ItemInfo> value)
    {
        _type = ResultType.items;
        CreateItemViews(value);
    }

    private void CreateItemViews(IList<ItemInfo> value)
    {
        List<ItemInfo> listInfo = new List<ItemInfo>();
        listInfo.AddRange(value);
        ClearView();
        InitComponent(listInfo.Count);
        _lstItemViews = new List<UIBaseView>();
        ItemView view;
        listInfo.Sort(OnSort);
        for (int i = 0; i < listInfo.Count; i++)
        {
            if (GameConfigMgr.Instance.GetItemConfig(listInfo[i].Id).ItemType == 2)
                view = ItemFactory.Instance.CreateItemView(listInfo[i], ItemViewType.EquipItem);
            else
                view = ItemFactory.Instance.CreateItemView(listInfo[i], ItemViewType.BagItem);
            view.mRectTransform.SetParent(_curRootRect, false);
            _lstItemViews.Add(view);
        }
        _curText.text = LanguageMgr.GetLanguage(6001252);
        _effect.PlayEffect();
    }

    public void ShowResult(List<ItemInfo> value,int level = 1)
    {
        _type = ResultType.role;
        CreateViews(value, level);
    }

    private void CreateViews(List<ItemInfo> value,int level)
    {
        ClearView();
        InitComponent(value.Count);
        _lstItemViews = new List<UIBaseView>();
        UIBaseView view;
        CardDataVO vo;
        value.Sort(OnSort);
        for (int i = 0; i < value.Count; i++)
        {
            vo = new CardDataVO(value[i].Id);
            vo.mCardCount = value[i].Value;
            vo.OnLevel(level);
            view = CardViewFactory.Instance.CreateCardView(vo, CardViewType.ConfigCard);
            view.mRectTransform.SetParent(_curRootRect, false);
            _lstItemViews.Add(view);
        }
        _curText.text = LanguageMgr.GetLanguage(6001252);
        _effect.PlayEffect();
    }

    private int OnSort(ItemInfo v0, ItemInfo v1)
    {
        if (v0.Id != v1.Id)
            return v0.Id < v1.Id ? -1 : 1;
        return 0;
    }

    public override void Hide()
    {
        _items = null;
        ClearView();
        _effect.StopEffect();
        base.Hide();
    }

    public override void Dispose()
    {
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.GetItemSureBtn);
        base.Dispose();
    }
    protected override void OnShowViewAnimation()
    {
        base.OnShowViewAnimation();
        ObjectHelper.PopAnimationBack(_root);
    }
}