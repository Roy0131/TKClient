using UnityEngine.UI;
using UnityEngine;
using Msg.ClientMessage;
using System.Collections.Generic;
using Framework.UI;
using System;

public class FusionLogicView : UIBaseView
{
    private Button _fusionBtn;
    private Text _buttonText;

    private ResCostGroup _costGroup;

    private GameObject _matItemObj;
    private RectTransform _matItemParent;
    private bool _blLevelEnough;
    private Dictionary<int, FusionMatItem> _dictFusionItems;

    private int _fusionId;
    public int mMainCardId { get; set; }

    private UIEffectView _fusionEffect;
    private UIEffectView _fusionEffect02;

    private bool _blFusionModule;

    private bool _isFirst = false;

    private RawImage _roleImage;

    private UIEffectView _effect;
    public FusionLogicView(bool blFusionModule = false)
    {
        _blFusionModule = blFusionModule;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _blLevelEnough = true;
        _roleImage = Find<RawImage>("RoleImage");
        _matItemParent = Find<RectTransform>("MatGroup");
        _matItemObj = Find("MatGroup/MatItem");

        _fusionBtn = Find<Button>("FusionBtn");
        _buttonText = Find<Text>("FusionBtn/Text");

        _costGroup = new ResCostGroup();
        _costGroup.SetDisplayObject(Find("CostRoot/uiCostGroup"));

        if (_blFusionModule)
        {
            _effect = CreateUIEffect(Find("fx_ui_suijihecheng"), UILayerSort.WindowSortBeginner);
            OnClearOldRole();
        }

        _fusionBtn.onClick.Add(OnFusion);
    }

    private void OnClearOldRole()
    {
        if (_blFusionModule)
        {
            RoleRTMgr.Instance.Hide(RoleRTType.RoleInfo);
            if (_roleImage.gameObject.activeSelf)
                _roleImage.gameObject.SetActive(false);
            _effect.PlayEffect();
        }
    }
    private void OnFusion()
    {
        if (!_blLevelEnough)
            return;
        if (!_costGroup.BlEnough)
        {
            LogHelper.Log("材料不足");
            return;
        }

        if (_dictFusionItems.Count > 0)
        {
            foreach (var kv in _dictFusionItems)
            {
                if (!kv.Value.mMatDataVO.BlMatEnough)
                {
                    PopupTipsMgr.Instance.ShowTips(6001131);
                    return;
                }
            }
        }

        IList<int> m1Cards = null;
        if (_dictFusionItems.ContainsKey(1))
            m1Cards = _dictFusionItems[1].mMatDataVO.mlstMatIds;
        IList<int> m2Cards = null;
        if (_dictFusionItems.ContainsKey(2))
            m2Cards = _dictFusionItems[2].mMatDataVO.mlstMatIds;
        IList<int> m3Cards = null;
        if (_dictFusionItems.ContainsKey(3))
            m3Cards = _dictFusionItems[3].mMatDataVO.mlstMatIds;
        GameNetMgr.Instance.mGameServer.ReqRoleFusion(_fusionId, mMainCardId, m1Cards, m2Cards, m3Cards);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<int>(UIEventDefines.FusionMatSelectOK, OnMatSelectOK);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<int>(UIEventDefines.OpenFusionMatSelect, OnOpenMatSelectModule);
        HeroDataModel.Instance.AddEvent<S2CRoleFusionResponse>(HeroEvent.CardFusionBack, OnFusionBack);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(FusionEvent.CreateCommonRole, OnClearOldRole);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<int>(UIEventDefines.FusionMatSelectOK, OnMatSelectOK);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<int>(UIEventDefines.OpenFusionMatSelect, OnOpenMatSelectModule);
        HeroDataModel.Instance.RemoveEvent<S2CRoleFusionResponse>(HeroEvent.CardFusionBack, OnFusionBack);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(FusionEvent.CreateCommonRole, OnClearOldRole);
    }

    private void OnFusionBack(S2CRoleFusionResponse value)
    {
        Action<UIEffectView> OnFusionBack = (view) =>
        {
            LogHelper.LogWarning("[HeroDataModel.DoRoleFusionResponse() => card fusion back, cardId:" + value.RoleId + ", newCardId:" + value.NewCardId + "]");
            FusionConfig cfg = GameConfigMgr.Instance.GetFusionConfig(value.FusionId);
            if (cfg.FusionType == 2)
            {
                if (value.GetItems.Count > 0)
                    GetItemTipMgr.Instance.ShowItemResult(value.GetItems);
                if (value.RoleId > 0)
                    GetItemTipMgr.Instance.ShowRoleResult(value.RoleId);
            }

            for (int i = 1; i < 4; i++)
            {
                if (_dictFusionItems.ContainsKey(i))
                {
                    _dictFusionItems[i].mMatDataVO.ClearAllMat();
                    _dictFusionItems[i].RefreshStatus();
                }
            }

            if (_fusionEffect02 != null)
                _fusionEffect02.StopEffect();
        };

        if (_blFusionModule)
        {
            SoundMgr.Instance.PlayEffectSound("UI_hecheng_Fuse");
            _fusionEffect02.PlayEffect(OnFusionBack, 0.8f);
            CardDataVO vo = HeroDataModel.Instance.GetCardDataByCardId(value.RoleId);
            _effect.StopEffect();
            if (GameConfigMgr.Instance.GetCardConfig(vo.mCardCfgId).Model != null)
            {
                RoleRTMgr.Instance.ShowRoleRTLogic(RoleRTType.RoleInfo, vo.mCardConfig);
                _roleImage.texture = RoleRTMgr.Instance.GetRoleRTImage();
                _roleImage.gameObject.SetActive(true);
            }
        }
        else
            OnFusionBack(null);
    }

    private void OnOpenMatSelectModule(int idx)
    {
        if (!_dictFusionItems.ContainsKey(idx))
        {
            LogHelper.LogError("[FusionLogicView.OnOpenMatSelectModule() => idx:" + idx + " not found!!!]");
            return;
        }
        List<int> lstUsedMatIDs = new List<int>();
        foreach (var kv in _dictFusionItems)
        {
            if (kv.Value.mMatDataVO != null && kv.Value.mMatDataVO.mlstMatIds.Count > 0)
                lstUsedMatIDs.AddRange(kv.Value.mMatDataVO.mlstMatIds);
        }
        GameUIMgr.Instance.OpenModule(ModuleID.RoleSelect, _dictFusionItems[idx].mMatDataVO, lstUsedMatIDs);
    }

    private void OnMatSelectOK(int idx)
    {
        if (!_dictFusionItems.ContainsKey(idx))
            return;
        _dictFusionItems[idx].RefreshStatus();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);


        if (_blFusionModule)
        {
            if (!_isFirst)
            {
                _fusionEffect = CreateUIEffect(Find("fx_ui_hecheng_1"), UILayerSort.WindowSortBeginner);
                _fusionEffect02 = CreateUIEffect(Find("fx_ui_hecheng"), UILayerSort.WindowSortBeginner);
                _fusionEffect.PlayEffect();
                _isFirst = true;
                DelayCall(1.5f, _fusionEffect.StopEffect);
            }
        }

        int id = int.Parse(args[0].ToString());
        //if (_fusionId == id)
        //    return;
        _fusionId = id;
        FusionConfig config = GameConfigMgr.Instance.GetFusionConfig(_fusionId);
        if (string.IsNullOrEmpty(config.ResCondtion))
            _costGroup.Hide();
        else
            _costGroup.Show(config.ResCondtion);
        ClearMatItem();
        _dictFusionItems = new Dictionary<int, FusionMatItem>();
        if (config.Cost1NumCond > 0)
            CreateMatItem(1, config);
        if (config.Cost2NumCond > 0)
            CreateMatItem(2, config);
        if (config.Cost3NumCond > 0)
            CreateMatItem(3, config);
        bool blLevel;
        if (config.FusionType == 2)
        {
            blLevel = true;
        }
        else
        {
            CardDataVO vo = HeroDataModel.Instance.GetCardDataByCardId(mMainCardId);
            blLevel = vo.mCardLevel >= config.MainCardLevelCond;
        }
        if (blLevel != _blLevelEnough)
        {
            _blLevelEnough = blLevel;
            _fusionBtn.interactable = _blLevelEnough;
        }
        if (_blLevelEnough)
        {
            _buttonText.color = Color.white;
            _buttonText.text = LanguageMgr.GetLanguage(5002716);
        }
        else
        {
            _buttonText.color = Color.red;
            _buttonText.text = LanguageMgr.GetLanguage(5002704) + ":" + config.MainCardLevelCond;
        }
    }

    private void CreateMatItem(int idx, FusionConfig config)
    {
        FusionMatDataVO vo = new FusionMatDataVO(idx, config);
        vo.mMainCardId = mMainCardId;
        GameObject itemObj = GameObject.Instantiate(_matItemObj);
        FusionMatItem item = new FusionMatItem();
        item.SetDisplayObject(itemObj);
        item.mRectTransform.SetParent(_matItemParent, false);
        item.Show(vo);
        _dictFusionItems.Add(idx, item);
    }

    private void ClearMatItem()
    {
        if (_dictFusionItems == null)
            return;
        foreach (var kv in _dictFusionItems)
            kv.Value.Dispose();
        _dictFusionItems.Clear();
        _dictFusionItems = null;
    }

    public override void Dispose()
    {
        ClearMatItem();
        if (_costGroup != null)
        {
            _costGroup.Dispose();
            _costGroup = null;
        }

        if (_fusionEffect != null)
        {
            _fusionEffect.Dispose();
            _fusionEffect = null;
        }
        base.Dispose();
    }
    public override void Hide()
    {
        _isFirst = false;
        base.Hide();
    }
}