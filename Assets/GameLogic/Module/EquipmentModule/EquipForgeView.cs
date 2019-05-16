using Framework.UI;
using Msg.ClientMessage;
using NewBieGuide;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipForgeView : UIBaseView
{
    private Button _closeBtn;
    private Button _helpBtn;
    private Button _forgeBtn;

    private ItemView _curItemView;
    private ItemView _tarItemView;

    private RectTransform _curItemRoot;
    private RectTransform _tarItemRoot;
    private GameObject _curItemFlag;
    private GameObject _tarItemFlag;

    private ItemResGroup _resGroup;
    private ResCostGroup _costGroup;

    private SkeletonGraphic _graphic;

    private Image _slider;
    private SubAndAddGroup _subAddGroup;
    private int _itemId;
    private int _itemCount;
    private int _totalCount;
    private Text _sliderText;
    protected override void ParseComponent()
    {
        base.ParseComponent();
        _closeBtn = Find<Button>("Buttons/BtnBack");
        ColliderHelper.SetButtonCollider(_closeBtn.transform);
        _forgeBtn = Find<Button>("Buttons/ForgeBtn");
        _helpBtn = Find<Button>("Buttons/BtnHelp");
        ColliderHelper.SetButtonCollider(_helpBtn.transform);

        _curItemRoot = Find<RectTransform>("CurItemObject");
        _curItemFlag = Find("CurItemObject/unKnownFlag");

        _tarItemRoot = Find<RectTransform>("TarItemObject");
        _tarItemFlag = Find("TarItemObject/unKnownFlag");

        _resGroup = new ItemResGroup();
        _resGroup.SetDisplayObject(Find("uiResGroup"));
        _resGroup.Show(SpecialItemID.Gold);

        _costGroup = new ResCostGroup();
        _costGroup.SetDisplayObject(Find("uiCostGroup"));

        _slider = Find<Image>("Prograssbar/Slider");
        _slider.fillAmount = 0f;

        _sliderText = Find<Text>("Prograssbar/SliderText");
        _sliderText.text = "0/3";

        _subAddGroup = new SubAndAddGroup(OnValueChange);
        _subAddGroup.SetDisplayObject(Find("SubAndAddGroup"));

        _graphic = Find<SkeletonGraphic>("SkeletonGraphic");

        _closeBtn.onClick.Add(OnClose);
        _forgeBtn.onClick.Add(OnForge);
        _helpBtn.onClick.Add(OnHelp);
        Reset();

        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.Forge, _forgeBtn.transform);
        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.EquipmentDisBtn, _closeBtn.transform);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<int>(EquipEvent.EquipForgeSelect, OnForgeEquip);
        BagDataModel.Instance.AddEvent<int, int, IList<ItemInfo>>(BagEvent.ItemUpGradeBack, OnItemUpGrade);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<int>(EquipEvent.EquipForgeSelect, OnForgeEquip);
        BagDataModel.Instance.RemoveEvent<int, int, IList<ItemInfo>>(BagEvent.ItemUpGradeBack, OnItemUpGrade);
    }

    private void OnItemUpGrade(int itemId, int roleId, IList<ItemInfo> listInfo)
    {
        GetItemTipMgr.Instance.ShowItemResult(listInfo);
    }

    private int _costGlodValue;
    private void OnForgeEquip(int itemID)
    {
        Reset();
        //_graphic.AnimationState.SetAnimation(0, "animation", false);
        if (itemID == 0)
            return;
        ItemUpgradeConfig config = GameConfigMgr.Instance.GetItemUpgradeConfig(itemID * 100 + 1);
        if (config == null)
        {
            LogHelper.LogError("[EquipForgeView.OnForgeEquip() => itemCfgId:" + itemID + " config not found!!!]");
            return;
        }
        ItemConfig itemCfg = GameConfigMgr.Instance.GetItemConfig(config.ItemID);
        _curItemView = ItemFactory.Instance.CreateItemView(itemCfg, ItemViewType.EquipRewardItem);
        _curItemView.mRectTransform.SetParent(_curItemRoot, false);

        ItemConfig targetConfig = GameConfigMgr.Instance.GetItemConfig(config.ResultDropID);
        _tarItemView = ItemFactory.Instance.CreateItemView(targetConfig, ItemViewType.EquipRewardItem);
        _tarItemView.mRectTransform.SetParent(_tarItemRoot, false);

        ItemInfo info = BagDataModel.Instance.GetItemById(itemID);
        _totalCount = info == null ? 0 : info.Value;
        string[] value = config.ResCondtion.Split(',');
        _costGlodValue = int.Parse(value[value.Length - 1]);

        int max = _totalCount / 3;
        _subAddGroup.Reset(max, max);
        _itemId = itemID;//info.ItemCfgId;
        OnValueChange();
    }

    private void OnValueChange()
    {
        float flPer = (float)_totalCount / 3f;
        if (flPer > 1.0f)
            flPer = 1.0f;
        _slider.fillAmount = flPer;
        _sliderText.text = _totalCount + "/3";

        string costValue = "1," + _subAddGroup.mCurValue * _costGlodValue;//config.ResCondtion.Replace(info.ItemCfgId + ",3,", "");
        _costGroup.Show(costValue);
    }

    private void Reset()
    {
        if (_curItemView != null)
            ItemFactory.Instance.ReturnItemView(_curItemView);
        if (_tarItemView != null)
            ItemFactory.Instance.ReturnItemView(_tarItemView);
        _subAddGroup.Reset();
        _slider.fillAmount = 0f;
        _itemId = 0;
        _itemCount = 0;
    }

    private void OnForge()
    {
        if (!_costGroup.BlEnough)
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000097));
            return;
        }
        if (_subAddGroup.mCurValue == 0)
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000096));
            return;
        }

        SoundMgr.Instance.PlayEffectSound("UI_wuqizhizao_Forge");
        _graphic.AnimationState.SetAnimation(0, "animation2", false);
        DelayCall(2.0f,()=>_graphic.AnimationState.SetAnimation(0, "animation", true));
        DelayCall(1.5f, OnReward);
    }

    private void OnReward()
    {
        _itemCount = _subAddGroup.mCurValue;
        GameNetMgr.Instance.mGameServer.ReqUpgradeItem(0, _itemId, (int)ItemUpGradeType.Item_UpGrade, _itemCount);
    }

    private void OnClose()
    {
        GameUIMgr.Instance.CloseModule(ModuleID.Equipment);
    }

    private void OnHelp()
    {
        HelpTipsMgr.Instance.ShowTIps(HelpType.EquipmentUpHelp);
    }

    public override void Dispose()
    {
        if (_curItemView != null)
        {
            ItemFactory.Instance.ReturnItemView(_curItemView);
            _curItemView = null;
        }
        if (_tarItemView != null)
        {
            ItemFactory.Instance.ReturnItemView(_tarItemView);
            _tarItemView = null;
        }
        if (_resGroup != null)
        {
            _resGroup.Dispose();
            _resGroup = null;
        }
        if (_costGroup != null)
        {
            _costGroup.Dispose();
            _costGroup = null;
        }
        if (_subAddGroup != null)
        {
            _subAddGroup.Dispose();
            _subAddGroup = null;
        }
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.Forge);
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.EquipmentDisBtn);
        base.Dispose();
    }
}