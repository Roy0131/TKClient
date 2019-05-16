using Framework.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipFuncView : UIBaseView
{
    private Button _closeBtn;
    private EquipFuncItemView _leftItemView;
    private EquipFuncItemView _rightItemView;

    private ItemResGroup _itemResGroup;
    private ResCostGroup _costGroup;

    private Button _fun1Btn;
    private Button _fun2Btn;
    private Text _fun1BtnText;
    private Text _fun2BtnText;
    private Text _titleText;

    private EquipFunDataVO _vo;
    private GameObject _panel;

    private GameObject _speciallyObj;
    private UIEffectView _effect;
    protected override void ParseComponent()
    {
        base.ParseComponent();
        _leftItemView = new EquipFuncItemView(true);
        _leftItemView.SetDisplayObject(Find("LeftItemObject"));

        _rightItemView = new EquipFuncItemView(false);
        _rightItemView.SetDisplayObject(Find("RightItemObject"));

        _itemResGroup = new ItemResGroup();
        _itemResGroup.SetDisplayObject(Find("uiResGroup"));

        _costGroup = new ResCostGroup();
        _costGroup.SetDisplayObject(Find("uiCostGroup"));
        
        _fun1Btn = Find<Button>("Buttons/Fun1Btn");
        _fun2Btn = Find<Button>("Buttons/Fun2Btn");
        _fun1BtnText = Find<Text>("Buttons/Fun1Btn/Text");
        _fun2BtnText = Find<Text>("Buttons/Fun2Btn/Text");
        _panel = Find("Panel");

        _speciallyObj = Find("fx_ui_fuwen");
        _effect = CreateUIEffect(_speciallyObj, UILayerSort.PopupSortBeginner);

        _titleText = Find<Text>("TextTitle");
        _closeBtn = Find<Button>("ButtonClose");
        ColliderHelper.SetButtonCollider(_closeBtn.transform);
        _closeBtn.onClick.Add(OnClose);
        _fun1Btn.onClick.Add(OnFun1Method);
        _fun2Btn.onClick.Add(OnFun2Method);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        BagDataModel.Instance.AddEvent<int, int, IList<ItemInfo>>(BagEvent.ItemUpGradeBack, OnItemUpGrade);
        HeroDataModel.Instance.AddEvent<List<int>>(HeroEvent.CardDataRefresh, OnCardRefresh);
        BagDataModel.Instance.AddEvent<int>(BagEvent.ItemUpGradeSaveBack, OnSaveBack);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        BagDataModel.Instance.RemoveEvent<int, int, IList<ItemInfo>>(BagEvent.ItemUpGradeBack, OnItemUpGrade);
        HeroDataModel.Instance.RemoveEvent<List<int>>(HeroEvent.CardDataRefresh, OnCardRefresh);
        BagDataModel.Instance.RemoveEvent<int>(BagEvent.ItemUpGradeSaveBack, OnSaveBack);
    }

    private void OnSaveBack(int roleId)
    {
        if (_vo.mRoleId != roleId)
            return;
        if (_fun2Btn.gameObject.activeSelf)
            _fun2Btn.gameObject.SetActive(false);
    }

    private void OnCardRefresh(List<int> roles)
    {
        if (roles.IndexOf(_vo.mRoleId) != -1)
        {
            CardDataVO cardVO = HeroDataModel.Instance.GetCardDataByCardId(_vo.mRoleId);
            _vo.mItemId = cardVO.GetEquipIdByEquipType(_vo.mEquipSlot);
            ShowData();
        }
    }

    private void OnItemUpGrade(int itemId, int roleId, IList<ItemInfo> listInfo)
    {
        if (_vo == null || _vo.mRoleId != roleId)
            return;

        if (_vo.mUpGradeType == ItemUpGradeType.GemStone_Convert)
        {
            _fun2Btn.gameObject.SetActive(true);
            _fun2BtnText.text = "Save";
            _rightItemView.Show(itemId);
        }
        else
        {
            _effect.PlayEffect();
            _panel.SetActive(true);
            DelayCall(0.6f, itemId, listInfo, OnShowTips);
            //if (_vo.mUpGradeType == ItemUpGradeType.GemStone_UpGrade)
            //{
            //    GetItemTipMgr.Instance.ShowItemResult(listInfo);
            //}
            //if (_vo.mUpGradeType == ItemUpGradeType.Artifact_UpGrade && GameConfigMgr.Instance.GetItemConfig(itemId).ShowStar == 5)
            //{
            //    GetItemTipMgr.Instance.ShowItemResult(listInfo);
            //}
            //if (GameConfigMgr.Instance.GetItemConfig(itemId).ShowStar == 5)
            //{
            //    GetItemTipMgr.Instance.ShowItemResult(listInfo);
            //    Hide();
            //}
        }
    }

    private void OnShowTips(int itemId, IList<ItemInfo> listInfo)
    {
        _panel.SetActive(false);
        _effect.StopEffect();
        GetItemTipMgr.Instance.ShowItemResult(listInfo);
        if (GameConfigMgr.Instance.GetItemConfig(itemId).ShowStar == 5)
            GameUIMgr.Instance.CloseModule(ModuleID.EquipFunc);
    }

    private void ShowData()
    {
        _rightItemView.Show(0);
        _leftItemView.Show(_vo.mItemId);
        int id = _vo.mItemId * 100 + (int)_vo.mUpGradeType;
        ItemUpgradeConfig config = GameConfigMgr.Instance.GetItemUpgradeConfig(id);
        if (config == null)
            return;
        if (_vo.mUpGradeType == ItemUpGradeType.Artifact_UpGrade)
            _rightItemView.Show(config.ResultDropID);
        _costGroup.Show(config.ResCondtion);
        string[] cond = config.ResCondtion.Split(',');
        if (cond.Length % 2 != 0)
        {
            LogHelper.LogError("[EquipFuncView.Refresh() => item upgrade rescondtion format error!!]");
            return;
        }
        object[] value = new object[cond.Length / 2];
        int idx = 0;
        for (int i = 0; i < cond.Length; i += 2)
        {
            value[idx] = cond[i];
            idx++;
        }
        _itemResGroup.Show(value);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _fun2Btn.gameObject.SetActive(false); 
        _vo = args[0] as EquipFunDataVO;
        switch (_vo.mUpGradeType)
        {
            case ItemUpGradeType.Artifact_UpGrade:
            case ItemUpGradeType.GemStone_UpGrade:
                _titleText.text = LanguageMgr.GetLanguage(5003013);
                _fun1BtnText.text = LanguageMgr.GetLanguage(5003013);
                break;
            case ItemUpGradeType.GemStone_Convert:
                _titleText.text = LanguageMgr.GetLanguage(5002717);
                _fun1BtnText.text = LanguageMgr.GetLanguage(5002717);
                break;
        }
        ShowData();
    }

    private void OnFun1Method()
    {
        if (!_costGroup.BlEnough)
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001131));
            return;
        }
        GameNetMgr.Instance.mGameServer.ReqUpgradeItem(_vo.mRoleId, _vo.mItemId, (int)_vo.mUpGradeType);
    }

    private void OnFun2Method()
    {
        GameNetMgr.Instance.mGameServer.ReqUpGradeItemSave(_vo.mRoleId);
    }

    private void OnClose()
    {
        GameUIMgr.Instance.CloseModule(ModuleID.EquipFunc);
    }

    public override void Dispose()
    {
        if (_leftItemView != null)
        {
            _leftItemView.Dispose();
            _leftItemView = null;
        }
        if (_rightItemView != null)
        {
            _rightItemView.Dispose();
            _rightItemView = null;
        }
        if (_costGroup != null)
        {
            _costGroup.Dispose();
            _costGroup = null;
        }
        if (_itemResGroup != null)
        {
            _itemResGroup.Dispose();
            _itemResGroup = null;
        }
        base.Dispose();
    }
}