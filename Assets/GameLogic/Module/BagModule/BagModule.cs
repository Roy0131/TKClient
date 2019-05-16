using Framework.UI;
using NewBieGuide;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BagModule : ModuleBase
{
    private Text _goldText;
    private Text _dmadText;
    private Image _goldImg;
    private Image _dmadImg;
    private Button _goldBtn;
    private Button _dmadBtn;
    private Button _disBtn;
    private Toggle[] _toggles;
    private GameObject _buttons;
    private GameObject _backObj;
    private Transform _root;

    private GridLayoutGroup _gridLayoutGroup;
    private BagItemViewMgr _bagItemViewMgr;
    private BagDetailView _bagDetailView;
    private int _curItemType;

    private bool isDetail;

    public BagModule()
        : base(ModuleID.Bag, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_Bag;
        mBlNeedBackMask = true;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _goldText = Find<Text>("TotalGold/GoldNum");
        _dmadText = Find<Text>("TotalDmad/DmadNum");
        _goldImg = Find<Image>("TotalGold/GoldImg");
        _dmadImg = Find<Image>("TotalDmad/DmadImg");
        _goldBtn = Find<Button>("TotalGold/GoldBtn");
        _dmadBtn = Find<Button>("TotalDmad/DmadBtn");
        _disBtn = Find<Button>("Btn_Back");
        _buttons = Find("Root/BagItemDetailObj/Buttons");
        _backObj = Find("Root/BagItemDetailObj/BackObj");
        _root = Find<Transform>("Root");

        _gridLayoutGroup = Find<GridLayoutGroup>("Root/BagItemObj/Panel_Scroll/KnapsackPanel");

        _toggles = new Toggle[4];
        for (int i = 0; i < 4; i++)
            _toggles[i] = Find<Toggle>("Root/ToggleGroup/Tog" + (i + 1));
        foreach (Toggle tog in _toggles)
            tog.onValueChanged.Add((bool blSelect) => { if (blSelect) OnItemTypeChange(tog); });

        _bagItemViewMgr = new BagItemViewMgr();
        _bagItemViewMgr.SetDisplayObject(Find("Root/BagItemObj"));

        _bagDetailView = new BagDetailView();
        _bagDetailView.SetDisplayObject(Find("Root/BagItemDetailObj"));

        _disBtn.onClick.Add(OnClose);
        _goldBtn.onClick.Add(OnGold);
        _dmadBtn.onClick.Add(OnDmad);

        RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.BagFragment, Find("Root/ToggleGroup/Tog3/Red"));
        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.FragmentToggle, _toggles[2].transform);
        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.BagModuleBackBtn, _disBtn.transform);

        ColliderHelper.SetButtonCollider(_disBtn.transform, 120, 120);
        ColliderHelper.SetButtonCollider(_goldBtn.transform);
        ColliderHelper.SetButtonCollider(_dmadBtn.transform);
    }

    private void OnItemTypeChange(Toggle tog)
    {
        switch (tog.name)
        {
            case "Tog1":
                _curItemType = ItemTypeConst.EQUIPMENT;
                break;
            case "Tog2":
                _curItemType = ItemTypeConst.PROPERTY;
                break;
            case "Tog3":
                _curItemType = ItemTypeConst.DEBRIS;
                break;
            case "Tog4":
                _curItemType = ItemTypeConst.ARTIFACT;
                break;
        }
        if (_curItemType == ItemTypeConst.DEBRIS)
            _gridLayoutGroup.cellSize = new Vector2(100f, 120f);
        else
            _gridLayoutGroup.cellSize = new Vector2(100f, 100f);
        _bagItemViewMgr.Show(_curItemType);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        HeroDataModel.Instance.AddEvent(HeroEvent.HeroInfoChange, OnRefreshTotal);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<ItemView>(BagEvent.Click, OnItemClick);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<bool>(BagEvent.BagNull, OnBagNull);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(BagEvent.Detail, OnDetail);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        HeroDataModel.Instance.RemoveEvent(HeroEvent.HeroInfoChange, OnRefreshTotal);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<ItemView>(BagEvent.Click, OnItemClick);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<bool>(BagEvent.BagNull, OnBagNull);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(BagEvent.Detail, OnDetail);
    }

    private void OnDetail()
    {
        isDetail = true;
    }

    private void OnBagNull(bool isNull)
    {
        if (isNull)
        {
            _buttons.SetActive(false);
            _backObj.SetActive(false);
        }
        else
        {
            _buttons.SetActive(true);
            _backObj.SetActive(true);
        }
    }

    private void OnItemClick(ItemView view)
    {
        _bagDetailView.Show(view);
    }

    private void OnRefreshTotal()
    {
        _goldText.text = UnitChange.GetUnitNum(HeroDataModel.Instance.mHeroInfoData.mGold);
        _dmadText.text = UnitChange.GetUnitNum(HeroDataModel.Instance.mHeroInfoData.mDiamond);
        _goldImg.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(SpecialItemID.Gold).UIIcon);
        _dmadImg.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(SpecialItemID.Diamond).UIIcon);

        ObjectHelper.SetSprite(_goldImg,_goldImg.sprite);
        ObjectHelper.SetSprite(_dmadImg,_dmadImg.sprite);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        if (isDetail)
        {
            OnItemTypeChange(_toggles[_curItemType-1]);
            isDetail = false;
        }
        else
        {
            OnItemTypeChange(_toggles[0]);
        }
        OnRefreshTotal();
    }

    private void OnGold()
    {
        GameUIMgr.Instance.OpenModule(ModuleID.Gold);
    }

    private void OnDmad()
    {
        GameUIMgr.Instance.OpenModule(ModuleID.Recharge);
    }

    public override void Hide()
    {
        if (!isDetail)
            _toggles[0].isOn = true;
        base.Hide();
    }

    public override void Dispose()
    {
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.FragmentToggle);
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.BagModuleBackBtn);
        base.Dispose();
    }

    protected override void OnShowAnimator()
    {
        base.OnShowAnimator();
        ObjectHelper.PopAnimationLiner(_root);
        Action OnAnimatorEnd = () =>
        {
            GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.EndCondTrigger, NewBieGuide.EndConditionConst.BagModuleOpen);
        };

        DelayCall(0.5f, OnAnimatorEnd);
    }
}
