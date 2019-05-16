using Framework.UI;
using NewBieGuide;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DecRoleListView : UIBaseView
{
    private Button _rewardBtn;
    private Button _decBtn;
    private Button _allBtn;
    private Button _backBtn;
    private Button _helpBtn;
    private Button _shopBtn;

    private ItemResGroup _resGroup;

    private Dictionary<int, RectTransform> _dictCardParent;
    private Dictionary<int, CardView> _dictCardView;
    private Dictionary<int, int> _dictCardPos;
    private Dictionary<int, UIEffectView> _dictCardEffects;

    private GameObject _itemEffectObj;
    private GameObject _sidePanle;

    protected override void ParseComponent()
    {
        base.ParseComponent();

        _rewardBtn = Find<Button>("Buttons/PreviewBtn");
        _decBtn = Find<Button>("Buttons/DecomposeBtn");
        _allBtn = Find<Button>("Buttons/BtnAll");
        _backBtn = Find<Button>("Buttons/BtnBack");
        _helpBtn = Find<Button>("Buttons/BtnHelp");
        _shopBtn = Find<Button>("Buttons/BtnShop");
        ColliderHelper.SetButtonCollider(_backBtn.transform);
        ColliderHelper.SetButtonCollider(_helpBtn.transform);
        _itemEffectObj = Find("fx_ui_fenjie01");
        _sidePanle = Find("SidePanle");

        _rewardBtn.onClick.Add(OnShowReward);
        _decBtn.onClick.Add(OnDecompose);
        _allBtn.onClick.Add(OnFillAllRole);
        _helpBtn.onClick.Add(OnShowHelp);
        _backBtn.onClick.Add(OnBack);
        _shopBtn.onClick.Add(OnShop);

        
        _resGroup = new ItemResGroup();
        _resGroup.SetDisplayObject(Find("uiResGroup"));
        _resGroup.Show(SpecialItemID.HeroExp, SpecialItemID.AttackHeroExp, SpecialItemID.DefenseHeroExp, SpecialItemID.SkillHeroExp);

        _dictCardParent = new Dictionary<int, RectTransform>();
        _dictCardView = new Dictionary<int, CardView>();
        _dictCardPos = new Dictionary<int, int>();
        _dictCardEffects = new Dictionary<int, UIEffectView>();

        for (int i = 0; i < 12; i++)
        {
            _dictCardParent.Add(i, Find<RectTransform>("ScrollView/Content/bg" + i));
            _dictCardPos.Add(i, 0);
        }

        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.Disassemble, _decBtn.transform);
        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.DeComposeDisBtn, _backBtn.transform);
        CreateFixedEffect(Find("fx_ui_fenjie02"), UILayerSort.WindowSortBeginner);
        CreateFixedEffect(Find("Buttons/BtnShop/fx_ui_zuanshi02"), UILayerSort.WindowSortBeginner);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        ClearAllCardView();
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        DecomposeDataModel.Instance.AddEvent<int>(DecomposeEvent.AddRoleToDecompose, OnAddRole);
        DecomposeDataModel.Instance.AddEvent<int>(DecomposeEvent.RemoveRoleToDecompose, OnRemoveRole);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(HeroEvent.HeroShop, OnShop);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        DecomposeDataModel.Instance.RemoveEvent<int>(DecomposeEvent.AddRoleToDecompose, OnAddRole);
        DecomposeDataModel.Instance.RemoveEvent<int>(DecomposeEvent.RemoveRoleToDecompose, OnRemoveRole);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(HeroEvent.HeroShop, OnShop);
    }

    private void OnAddRole(int cardId)
    {
        int pos = -1;
        foreach(var kv in _dictCardPos)
        {
            if (kv.Value == 0)
            {
                pos = kv.Key;
                break;
            }
        }
        if (pos == -1)
        {
            LogHelper.LogWarning("DecRoleListView.OnAddRole() => 找不到空位!!!");
            return;
        }
        _dictCardPos[pos] = cardId;
        CardDataVO vo = HeroDataModel.Instance.GetCardDataByCardId(cardId);
        CardView view = CardViewFactory.Instance.CreateCardView(vo, CardViewType.Common, OnClick);
        RectTransform root = _dictCardParent[pos];
        view.mRectTransform.SetParent(root, false);
        _dictCardView.Add(vo.mCardID, view);

        UIEffectView effect = CreateUIEffect(_itemEffectObj, UILayerSort.WindowSortBeginner + 1, true);
        ObjectHelper.AddChildToParent(effect.mTransform, root, false);
        _dictCardEffects.Add(vo.mCardID, effect);
    }
    
    private void OnRemoveRole(int cardId)
    {
        int pos = -1;
        foreach(var kv in _dictCardPos)
        {
            if (kv.Value == cardId)
            {
                pos = kv.Key;
                break;
            }
        }
        if (pos == -1)
            return;
        
        _dictCardPos[pos] = 0;
        CardViewFactory.Instance.ReturnCardView(_dictCardView[cardId]);
        _dictCardView.Remove(cardId);

        ReturnUIEffect(_dictCardEffects[cardId]);
        _dictCardEffects.Remove(cardId);
    }

    private void OnClick(CardView view)
    {
        DecomposeDataModel.Instance.RemoveVOToDecompose(view.mCardDataVO);
    }

    public void RoleDecomposeBack()
    {
        ClearAllCardView(true);
    }

    private void OnShowReward()
    {
        DecomposeDataModel.Instance.PareseDecomposeReward();
    }
    

    private void OnDecompose()
    {
        bool blShowAlert = false;
        CardDataVO vo;
        foreach (var kv in _dictCardPos)
        {
            if (kv.Value <= 0)
                continue;
            vo = HeroDataModel.Instance.GetCardDataByCardId(kv.Value);
            if (vo == null)
                continue;
            if (vo.mCardConfig.Rarity >= 4)
            {
                blShowAlert = true;
                break;
            }
        }

        Action<bool, bool> OnAlertBack = (b1, b2) =>
        {
            if(b1)
                DecomposeDataModel.Instance.ReqDecomposeRole();
        };

        if (blShowAlert)
            ConfirmTipsMgr.Instance.ShowConfirmTips(LanguageMgr.GetLanguage(5001409), OnAlertBack);
        else
            OnAlertBack(true,true);//DecomposeDataModel.Instance.ReqDecomposeRole();
    }

    private void OnFillAllRole()
    {
        DecomposeDataModel.Instance.FillAllToDecompose();
    }

    private void OnShowHelp()
    {
        HelpTipsMgr.Instance.ShowTIps(HelpType.BreakHelp);
    }

    private void OnBack()
    {
        GameUIMgr.Instance.CloseModule(ModuleID.RoleDecompose);
    }

    private void OnShop()
    {
        GameUIMgr.Instance.OpenModule(ModuleID.HeroShop, ShopIdConst.HEROSHOP);
    }

    private void ClearAllCardView(bool blPlayEffect = false)
    {
        Action<UIEffectView> onPlayEnd = (effect) =>
        {
            _sidePanle.SetActive(false);
            ReturnUIEffect(effect);
        };

        if(_dictCardView.Count > 0)
        {
            foreach(var kv in _dictCardView)
                CardViewFactory.Instance.ReturnCardView(kv.Value);
            _dictCardView.Clear();

            foreach (var kv in _dictCardEffects)
            {
                if (blPlayEffect)
                {
                    _sidePanle.SetActive(true);
                    kv.Value.PlayEffect(onPlayEnd, 1f);
                }
                else
                {
                    ReturnUIEffect(kv.Value);
                }
            }
            _dictCardEffects.Clear();

            for (int i = 0; i < 12; i++)
                _dictCardPos[i] = 0;         
        }
    }

    public override void Dispose()
    {
        ClearAllCardView();
        _dictCardPos = null;
        _dictCardView = null;
        if(_dictCardParent != null)
        {
            _dictCardParent.Clear();
            _dictCardParent = null;
        }
        if (_resGroup != null)
        {
            _resGroup.Dispose();
            _resGroup = null;
        }
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.Disassemble);
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.DeComposeDisBtn);
        base.Dispose();
    }
}