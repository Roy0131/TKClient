using System;
using System.Collections.Generic;
using Msg.ClientMessage;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public class HeroReplaceView : UIBaseView
{
    private HeroCallBagView _heroCallBagView;
    private List<CardDataVO> _lstDatas;
    private CardDataVO _cardVo;
    private GameObject _objBag;
    private Button _btnLeft;
    private Button _btnRight;
    private RawImage _roleImage;
    private GameObject _objReplace;
    private GameObject _objSaveCancel;

    private int _cardId = 0;
    private int _groupId;
    private GameObject _LeftRole;
    private GameObject _RightRole;

    private Button _btnReplace;
    private Button _btnCancel;
    private Button _btnSave;

    private Dis dis;

    private ReplaceRoleView _replaceRoleLeftView;
    private ReplaceRoleView _replaceRoleRightView;
    private CardView _cardView;

    private Dictionary<int, string> _dict;
    private int _numCost;
    private List<int> _lstCard = new List<int>();

    private bool _blSave;

    private Image _icon;
    private Text _heroesNum;

    private UIEffectView _effectOri;
    private UIEffectView _effectL01;
    private UIEffectView _effectL02;
    private UIEffectView _effectL03;
    private UIEffectView _effectR01;
    private UIEffectView _effectR02;
    private UIEffectView _effectR03;

    private UIEffectView _effectOriz;
    private UIEffectView _effectL01z;
    private UIEffectView _effectL02z;
    private UIEffectView _effectL03z;
    private UIEffectView _effectR01z;
    private UIEffectView _effectR02z;
    private UIEffectView _effectR03z;

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _roleImage.gameObject.SetActive(false);
        dis = (Dis)args[0];
        _cardVo = new CardDataVO();
        HeroSort(_lstCard);
        _blSave = true;
        if (dis == Dis.disCamp)
        {
            _effectOri.PlayEffect();
            _effectL01.PlayEffect();
        }
        else
        {
            _effectOriz.PlayEffect();
            _effectL01z.PlayEffect();
        }
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _objBag = Find("HeroBag/uiHerobag");
        _btnLeft = Find<Button>("BtnLeft");
        _btnRight = Find<Button>("BtnRight");
        _icon = Find<Image>("replace/ButtonRepacement/Img");
        _heroesNum = Find<Text>("replace/ButtonRepacement/Img/Text");
        _roleImage = Find<RawImage>("RoleLR/Role");
        _btnReplace = Find<Button>("replace/ButtonRepacement");
        _btnCancel = Find<Button>("SaveCancel/ButtonCancel");
        _btnSave = Find<Button>("SaveCancel/ButtonSave");
        
        _LeftRole = Find("RoleLR/LeftRole");
        _RightRole = Find("RoleLR/RightRole");

        _objReplace = Find("replace");
        _objSaveCancel = Find("SaveCancel");

        _replaceRoleLeftView = new ReplaceRoleView();
        _replaceRoleLeftView.SetDisplayObject(_LeftRole);

        _replaceRoleRightView = new ReplaceRoleView();
        _replaceRoleRightView.SetDisplayObject(_RightRole);

        _heroCallBagView = new HeroCallBagView();
        _heroCallBagView.SetDisplayObject(_objBag);

        _btnLeft.onClick.Add(OpenHeroBag);

        _btnReplace.onClick.Add(OnReplace);
        _btnCancel.onClick.Add(OnCancel);
        _btnSave.onClick.Add(OnSave);

        _objReplace.SetActive(false);
        _objSaveCancel.SetActive(false);

        _effectOri = CreateUIEffect(Find("Fx/fx_ui_zhihuan_light"), UILayerSort.WindowSortBeginner + 2);
        _effectL01 = CreateUIEffect(Find("Fx/fx_ui_zhihuan_zuo01"), UILayerSort.WindowSortBeginner + 2);
        _effectL02 = CreateUIEffect(Find("Fx/fx_ui_zhihuan_zuo02"), UILayerSort.WindowSortBeginner + 3);
        _effectL03 = CreateUIEffect(Find("Fx/fx_ui_zhihuan_zuo03"), UILayerSort.WindowSortBeginner + 3);
        _effectR01 = CreateUIEffect(Find("Fx/fx_ui_zhihuan_you01"), UILayerSort.WindowSortBeginner + 3);
        _effectR02 = CreateUIEffect(Find("Fx/fx_ui_zhihuan_you02"), UILayerSort.WindowSortBeginner + 3);
        _effectR03 = CreateUIEffect(Find("Fx/fx_ui_zhihuan_you03"), UILayerSort.WindowSortBeginner + 3);

        _effectOriz = CreateUIEffect(Find("Fx/fx_ui_zhihuan_lightzi"), UILayerSort.WindowSortBeginner + 2);
        _effectL01z = CreateUIEffect(Find("Fx/fx_ui_zhihuan_zuo01zi"), UILayerSort.WindowSortBeginner + 2);
        _effectL02z = CreateUIEffect(Find("Fx/fx_ui_zhihuan_zuo02zi"), UILayerSort.WindowSortBeginner + 3);
        _effectL03z = CreateUIEffect(Find("Fx/fx_ui_zhihuan_zuo03zi"), UILayerSort.WindowSortBeginner + 3);
        _effectR01z = CreateUIEffect(Find("Fx/fx_ui_zhihuan_you01zi"), UILayerSort.WindowSortBeginner + 3);
        _effectR02z = CreateUIEffect(Find("Fx/fx_ui_zhihuan_you02zi"), UILayerSort.WindowSortBeginner + 3);
        _effectR03z = CreateUIEffect(Find("Fx/fx_ui_zhihuan_you03zi"), UILayerSort.WindowSortBeginner + 3);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<CardView>(HeroCallEvent.HeroSelect, OnSetSelRoleData);
        HeroCallModel.Instance.AddEvent(HeroCallEvent.HeroReplace, OnSetReplaceData);
        HeroCallModel.Instance.AddEvent(HeroCallEvent.HeroReplaceConfirm, OnConfirm);
        HeroDataModel.Instance.AddEvent<List<int>>(HeroEvent.CardDataRefresh, HeroSort);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<CardView>(HeroCallEvent.HeroSelect, OnSetSelRoleData);
        HeroCallModel.Instance.RemoveEvent(HeroCallEvent.HeroReplace, OnSetReplaceData);
        HeroCallModel.Instance.RemoveEvent(HeroCallEvent.HeroReplaceConfirm, OnConfirm);
        HeroDataModel.Instance.RemoveEvent<List<int>>(HeroEvent.CardDataRefresh, HeroSort);
    }

    /// <summary>
    ///     点击按钮显示英雄背包
    /// </summary>
    private void OpenHeroBag()
    {
        LogHelper.Log("[HeroReplaceView.OpenHeroBag()]");
        if (_blSave)
            _heroCallBagView.Show(_lstDatas, _cardId, dis);
    }

    /// <summary>
    /// 点击选择的要置换的英雄显示出来
    /// </summary>
    /// <param name="item"></param>
    private void OnSetSelRoleData(CardView value)
    {
        _objReplace.SetActive(true);
        _objSaveCancel.SetActive(false);
        ItemInfo itemInfo = new ItemInfo();
        string[] itemList = value.mCardDataVO.mCardConfig.ConvertItem.Split(',');
        if (itemList.Length % 2 != 0)
            return;
        for (int i = 0; i < itemList.Length; i += 2)
        {
            itemInfo.Id = int.Parse(itemList[i]);
            itemInfo.Value = int.Parse(itemList[i + 1]);
        }
        _icon.sprite= GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(itemInfo.Id).UIIcon);
        _heroesNum.text = itemInfo.Value.ToString();

        _cardView = new CardView();
        _cardView = value;

        if (dis == Dis.disCamp)
        {
            _effectL01.StopEffect();
            _effectL02.PlayEffect();
            _effectR01.PlayEffect();
            _effectR02.PlayEffect();
            _groupId = GameConfigMgr.Instance.GetCardConfig(value.mCardDataVO.mCardCfgId).ConvertID2;
        }
        else
        {
            _effectL01z.StopEffect();
            _effectL02z.PlayEffect();
            _effectR01z.PlayEffect();
            _effectR02z.PlayEffect();
            _groupId = GameConfigMgr.Instance.GetCardConfig(value.mCardDataVO.mCardCfgId).ConvertID1;
        }
        _numCost = Convert.ToInt32(GameConfigMgr.Instance.GetCardConfig(value.mCardDataVO.mCardCfgId).ConvertItem.Split(',')[1]);
        _cardId = value.mCardDataVO.mCardID;
        _cardVo = new CardDataVO();
        _cardVo = value.mCardDataVO;

        _dict = new Dictionary<int, string>();
        _dict.Add(1, _cardView.mCardDataVO.mCardConfig.Model);
        RoleRTMgr.Instance.ShowRoleRTLogic(RoleRTType.HeroCall, _dict);
        if (!_roleImage.gameObject.activeSelf)
            _roleImage.gameObject.SetActive(true);
        _roleImage.texture = RoleRTMgr.Instance.GetRoleRTImage();

        _replaceRoleLeftView.Show(_cardVo);
    }

    /// <summary>
    /// 点击置换服务器返回的角色
    /// </summary>
    private void OnSetReplaceData()
    {
        _objSaveCancel.SetActive(true);
        _objReplace.SetActive(false);

        if (dis == Dis.disCamp)
        {
            _effectL02.StopEffect();
            _effectR01.StopEffect();
            _effectR02.StopEffect();
            _effectR03.PlayEffect();
        }
        else
        {
            _effectL02z.StopEffect();
            _effectR01z.StopEffect();
            _effectR02z.StopEffect();
            _effectR03z.PlayEffect();
        }

        _cardVo = new CardDataVO(HeroCallModel.Instance.newRoleTableId,_cardVo.mCardRank,_cardVo.mCardLevel,_cardVo.DictEquipment);
        _dict.Add(2, _cardVo.mCardConfig.Model);
        RoleRTMgr.Instance.ShowRoleRTLogic(RoleRTType.HeroCall, _dict);
        if (!_roleImage.gameObject.activeSelf)
            _roleImage.gameObject.SetActive(true);
        _roleImage.texture = RoleRTMgr.Instance.GetRoleRTImage();

        _replaceRoleRightView.Show(_cardVo);
        _blSave = false;
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(HeroCallEvent.heroreplacesuccess);
    }

    /// <summary>
    /// 点击置换按钮
    /// </summary>
    private void OnReplace()
    {
        if (BagDataModel.Instance.GetItemCountById(SpecialItemID.EnergyStone) >= _numCost)
            HeroCallModel.Instance.ReqRoleDisplace(_groupId, _cardId);
        else
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000127));
    }

    /// <summary>
    /// 点击取消按钮
    /// </summary>
    private void OnCancel()
    {
        _objSaveCancel.SetActive(false);
        _objReplace.SetActive(true);

        if (dis == Dis.disCamp)
        {
            _effectR03.StopEffect();
            _effectR01.PlayEffect();
            _effectR02.PlayEffect();
        }
        else
        {
            _effectR03z.StopEffect();
            _effectR01z.PlayEffect();
            _effectR02z.PlayEffect();
        }

        _replaceRoleRightView.Hide();

        if (_dict.ContainsKey(2))
            _dict.Remove(2);
        RoleRTMgr.Instance.ShowRoleRTLogic(RoleRTType.HeroCall, _dict);
        if (!_roleImage.gameObject.activeSelf)
            _roleImage.gameObject.SetActive(true);
        _roleImage.texture = RoleRTMgr.Instance.GetRoleRTImage();
        _blSave = true;
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(HeroCallEvent.herocancel);
        LogHelper.Log("不保存置换的英雄");
    }

    /// <summary>
    /// 点击保存按钮
    /// </summary>
    private void OnSave()
    {
        _effectR03.StopEffect();
        HeroCallModel.Instance.ReqRoleDisplaceConfirm();
    }

    /// <summary>
    /// 确认保存英雄
    /// </summary>
    private void OnConfirm()
    {
        _blSave = true;
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(HeroCallEvent.herosavesuccess);
        LogHelper.Log("保存英雄成功");
        List<ItemInfo> itemInfos = new List<ItemInfo>();
        ItemInfo itemInfo = new ItemInfo();
        itemInfo.Id = _cardVo.mCardTableId;
        itemInfo.Value = _cardVo.mCardCount;
        itemInfos.Add(itemInfo);
        GetItemTipMgr.Instance.ShowRoles(itemInfos, _cardVo.mCardLevel);
        
        _objSaveCancel.SetActive(false);
        _objReplace.SetActive(false);

        if (dis == Dis.disCamp)
            _effectL01.PlayEffect();
        else
            _effectL01z.PlayEffect();

        _replaceRoleLeftView.Hide();
        _replaceRoleRightView.Hide();
        if (_dict != null)
            _dict = null;
        if (_cardVo != null)
            _cardVo = null;
        _cardId = 0;
        RoleRTMgr.Instance.Hide(RoleRTType.HeroCall);
    }

    public override void Hide()
    {
        base.Hide();
        _objSaveCancel.SetActive(false);
        _objReplace.SetActive(false);
        _replaceRoleLeftView.Hide();
        _replaceRoleRightView.Hide();
        if (_cardVo != null)
            _cardVo = null;
        _cardId = 0;

        _effectOri.StopEffect();
        _effectL01.StopEffect();
        _effectL02.StopEffect();
        _effectL03.StopEffect();
        _effectR01.StopEffect();
        _effectR02.StopEffect();
        _effectR03.StopEffect();
        _effectOriz.StopEffect();
        _effectL01z.StopEffect();
        _effectL02z.StopEffect();
        _effectL03z.StopEffect();
        _effectR01z.StopEffect();
        _effectR02z.StopEffect();
        _effectR03z.StopEffect();

        RoleRTMgr.Instance.Hide(RoleRTType.HeroCall);
    }

    /// <summary>
    /// 英雄排序
    /// </summary>
    private void HeroSort(List<int> value)
    {
        _lstDatas = new List<CardDataVO>();
        for (int i = 0; i < HeroDataModel.Instance.mAllCards.Count; i++)
        {
            if (HeroDataModel.Instance.mAllCards[i].mCardConfig.Rarity >= 4 && HeroDataModel.Instance.mAllCards[i].mCardConfig.Rarity <= 5)
                _lstDatas.Add(HeroDataModel.Instance.mAllCards[i]);
        }
        //排序
        _lstDatas.Sort(SortCard);
    }

    private int SortCard(CardDataVO v1, CardDataVO v2)
    {
        if (v1.mCardConfig.Rarity != v2.mCardConfig.Rarity)
        {
            return v1.mCardConfig.Rarity > v2.mCardConfig.Rarity ? -1 : 1;
        }
        else if (v1.mCardConfig.Rarity == v2.mCardConfig.Rarity && v1.mCardLevel != v2.mCardLevel)
        {
            return v1.mCardLevel > v2.mCardLevel ? -1 : 1;
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
public class ReplaceRoleView : UIBaseView
{
    private CardDataVO _vo;
    private CardRarityView _rarityView;
    private Image _imgCamp;
    private Text _textName;

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _vo = args[0] as CardDataVO;
        if (_vo == null) return;

        _rarityView.Show(_vo.mCardConfig.Rarity);

        _textName.text = LanguageMgr.GetLanguage(_vo.mCardConfig.Name);
        _imgCamp.sprite = GameResMgr.Instance.LoadCampIcon(_vo.mCardConfig.Camp);
        ObjectHelper.SetSprite(_imgCamp, _imgCamp.sprite);
    }
    protected override void ParseComponent()
    {
        base.ParseComponent();
        _imgCamp = Find<Image>("Grid/ImageCamp");
        _textName = Find<Text>("Grid/Name");

        _rarityView = new CardRarityView();
        _rarityView.SetDisplayObject(Find("cardStars/uiStarGroup"));
    }

}

