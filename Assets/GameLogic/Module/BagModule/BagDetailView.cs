using Msg.ClientMessage;
using NewBieGuide;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public class BagDetailView : UIBaseView
{
    private Text _itemName;
    private Text _itemType;
    private Text _itemDes;
    private Button _itemDetailBtn;
    private Button _itemCallBtn;
    private Button _itemSellBtn;
    private Button _gainBtn;
    private Button _wholeBtn;
    private GameObject _suitObj;
    private GameObject _desObj;
    private GameObject _bagroad;
    private GameObject _JumpObj;
    private RectTransform _desTrsf;
    private RectTransform _bagroadParent;
    private RectTransform _parent;

    //private ItemInfo _itemInfo;
    private ItemConfig _cfg;

    private BagSellView _bagSell;
    private BagCallView _bagCall;
    private List<BagRoadView> _listBagRoadView;

    private List<Text> _lstSuitText;
    private List<Text> _lstDesText;

    private ItemView _itemView;
    private ItemView _curItemView;
    private ItemView _view;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _itemName = Find<Text>("BackObj/Name");
        _itemType = Find<Text>("BackObj/Type");
        _itemDes = Find<Text>("BackObj/Des");
        _itemDetailBtn = Find<Button>("Buttons/ButtonDetail");
        _itemCallBtn = Find<Button>("Buttons/ButtonCall");
        _itemSellBtn = Find<Button>("Buttons/ButtonSell");
        _gainBtn = Find<Button>("Buttons/GainBtn");
        _wholeBtn = Find<Button>("Road_Panel/WholeBtn");
        _desTrsf = Find<RectTransform>("BackObj/DesObj");
        _bagroadParent = Find<RectTransform>("Road_Panel/Panel_Scroll/KnapsackPanel");
        _parent = Find<RectTransform>("BackObj/BagItem");
        _suitObj = Find("BackObj/SuitObj");
        _desObj = Find("BackObj/DesObj/TextDescribe");
        _bagroad = Find("Road_Panel");
        _JumpObj = Find("Road_Panel/Panel_Scroll/KnapsackPanel/JumpObj");

        _bagSell = new BagSellView();
        _bagSell.SetDisplayObject(Find("SellObj"));

        _bagCall = new BagCallView();
        _bagCall.SetDisplayObject(Find("CallObj"));

        _lstSuitText = new List<Text>();
        for (int i = 0; i < 4; i++)
            _lstSuitText.Add(Find<Text>(("BackObj/SuitObj/SuitRoot/SuitItem" + i)));

        _itemDetailBtn.onClick.Add(OnDetail);
        _itemCallBtn.onClick.Add(OnCall);
        _itemSellBtn.onClick.Add(OnSell);
        _gainBtn.onClick.Add(OnGain);
        _wholeBtn.onClick.Add(OnHideBtn);

        ColliderHelper.SetButtonCollider(_gainBtn.transform, 80, 80);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        BagDataModel.Instance.AddEvent<List<ItemInfo>>(BagEvent.ItemFusionRefresh, OnShowFusionResult);
        BagDataModel.Instance.AddEvent<int, int>(BagEvent.ItemSellResult, OnShowSellResult);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(BagEvent.BagJump, OnJump);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        BagDataModel.Instance.RemoveEvent<List<ItemInfo>>(BagEvent.ItemFusionRefresh, OnShowFusionResult);
        BagDataModel.Instance.RemoveEvent<int, int>(BagEvent.ItemSellResult, OnShowSellResult);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(BagEvent.BagJump, OnJump);
    }

    private void OnShowFusionResult(List<ItemInfo> value)
    {
        GetItemTipMgr.Instance.ShowRoles(value);
    }

    private void OnShowSellResult(int itemId, int itemCount)
    {
        ItemConfig curItemConfig = GameConfigMgr.Instance.GetItemConfig(itemId);
        List<ItemInfo> _listItemInfo = new List<ItemInfo>();
        ItemInfo _itemInfo;
        string[] univalent = curItemConfig.SellReward.Split(',');
        if (univalent.Length % 2 != 0)
            return;
        for (int j = 0; j < univalent.Length; j += 2)
        {
            _itemInfo = new ItemInfo();
            _itemInfo.Id = int.Parse(univalent[j]);
            _itemInfo.Value = int.Parse(univalent[j + 1]) * itemCount;
            _listItemInfo.Add(_itemInfo);
        }
        GetItemTipMgr.Instance.ShowItemResult(_listItemInfo);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _curItemView = args[0] as ItemView;
        if (_itemView != null)
            _itemView.BlSelected = false;
        _itemView = _curItemView;
        _curItemView.BlSelected = true;
        OnDetailChang();
        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.FragmentCall, _itemCallBtn.transform);
    }

    private void OnClick(ItemView view)
    {

    }

    protected Color _suitYColor = new Color(165f / 255f, 230f / 255f, 52f / 255f, 1f);
    protected Color _suitNColor = new Color(221f / 255f, 223f / 255f, 222f / 255f, 1f);
    protected Color _suitSColor = new Color(105f / 255f, 73f / 255f, 56f / 255f, 1f);

    private void OnDetailChang()
    {
        _cfg = _curItemView.mItemDataVO.mItemConfig;
        if (_cfg == null)
            return;
        _itemName.text = LanguageMgr.GetLanguage(_cfg.NameID);
        _itemType.text = LanguageMgr.GetLanguage(GameConst.GetItemTypeLanguageId(_cfg.ItemType, _cfg.EquipType));

        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        if (_curItemView.mItemDataVO.mItemConfig.ItemType == 2)
            _view = ItemFactory.Instance.CreateItemView(_curItemView.mItemDataVO.mItemConfig, ItemViewType.EquipItem, OnClick);
        else

            _view = ItemFactory.Instance.CreateItemView(_curItemView.mItemDataVO.mItemConfig, ItemViewType.ShopItem, OnClick);
        _view.mRectTransform.SetParent(_parent, false);

        ClearAttrText();
        _lstDesText = new List<Text>();
        _suitObj.SetActive(_cfg.SuitID > 0);
        if (_cfg.SuitID > 0)
        {
            int len = 0;
            SuitConfig suitConfig = GameConfigMgr.Instance.GetSuitConfig(_cfg.SuitID);
            Text t = _lstSuitText[0];
            t.text = LanguageMgr.GetLanguage(suitConfig.NameID) + "(4)";
            ParseSuit(_lstSuitText[1], suitConfig.AttrSuit2, len >= 2 ? _suitYColor : _suitNColor);
            ParseSuit(_lstSuitText[2], suitConfig.AttrSuit3, len >= 3 ? _suitYColor : _suitNColor);
            ParseSuit(_lstSuitText[3], suitConfig.AttrSuit4, len >= 4 ? _suitYColor : _suitNColor);
        }
        if (_cfg.ItemType == 2)
        {
            _itemDes.gameObject.SetActive(false);
            if (!string.IsNullOrEmpty(_cfg.EquipAttr))
            {
                string[] attrs = _cfg.EquipAttr.Split(',');
                if (attrs.Length % 2 != 0)
                    return;
                string attrStr;
                for (int i = 0; i < attrs.Length; i += 2)
                {
                    attrStr = attrs[i] + "," + attrs[i + 1];
                    ParseSuit(CreateText(), attrStr, _suitSColor);
                }
            }
            if (!string.IsNullOrEmpty(_cfg.EquipSkill))
            {
                Text t = CreateText();
                int skillId = int.Parse(_cfg.EquipSkill);
                SkillConfig skillCfg = GameConfigMgr.Instance.GetSkillConfig(skillId);
                t.text = LanguageMgr.GetLanguage(skillCfg.DescrptionID, skillCfg.ShowParam.Split(','));
            }
        }
        else
        {
            _itemDes.gameObject.SetActive(true);
            _itemDes.text = LanguageMgr.GetLanguage(_cfg.DescrptionID) + "\n<color=#FD6854>" + LanguageMgr.GetLanguage(5001107) + "</color>";
        }
        _itemSellBtn.gameObject.SetActive(_cfg.ItemType == 2);
        if (_cfg.ItemType == 4)
        {
            _itemCallBtn.gameObject.SetActive(_curItemView.mItemDataVO.mCount >= _cfg.ComposeNum);
            _itemDetailBtn.gameObject.SetActive(_cfg.ComposeDropID > 1000 && _cfg.ComposeDropID < 10000);
            if (_curItemView.mItemDataVO.mCount >= _cfg.ComposeNum)
            {
                _itemDetailBtn.transform.localPosition = new Vector3(285f, -265f, 0f);
                if (_cfg.ComposeDropID > 1000 && _cfg.ComposeDropID < 10000)
                    _itemCallBtn.transform.localPosition = new Vector3(440f, -265f, 0);
                else
                    _itemCallBtn.transform.localPosition = new Vector3(360f, -265f, 0);
            }
            else
            {
                _itemDetailBtn.transform.localPosition = new Vector3(360f, -265f, 0f);
            }
        }
        else
        {
            _itemCallBtn.gameObject.SetActive(false);
            _itemDetailBtn.gameObject.SetActive(false);
        }
    }

    private void ClearAttrText()
    {
        if (_lstDesText == null)
            return;
        if (_lstDesText.Count > 0)
        {
            for (int i = 0; i < _lstDesText.Count; i++)
                GameObject.Destroy(_lstDesText[i].gameObject);
            _lstDesText.Clear();
            _lstDesText = null;
        }
    }

    private Text CreateText()
    {
        GameObject attrObject = GameObject.Instantiate(_desObj);
        attrObject.transform.SetParent(_desTrsf, false);
        attrObject.SetActive(true);
        Text t = attrObject.GetComponent<Text>();
        _lstDesText.Add(t);
        return t;
    }

    private void ParseSuit(Text t, string suitValue, Color color)
    {
        string[] suits = suitValue.Split(',');
        int attrId = int.Parse(suits[0]);
        float value = float.Parse(suits[1]);
        AttributeConfig config = GameConfigMgr.Instance.GetAttrConfig(attrId);
        t.color = color;
        if (config.PercentShow == 0)
        {
            t.text = LanguageMgr.GetLanguage(config.NameID) + "  +" + (value / config.Divisor).ToString();
        }
        else
        {
            if (config.Divisor != 0)
                value = (float)value / (float)config.Divisor;
            t.text = LanguageMgr.GetLanguage(config.NameID) + "  +" + value.ToString("F1") + "%";
        }
    }

    private void OnDetail()
    {
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(BagEvent.Detail);
        CardDataVO vo;
        int comId = _cfg.ComposeDropID;
        CardConfig cardCfg = GameConfigMgr.Instance.GetCardConfig(comId * 100 + 1);
        CardConfig cardCfgs = GameConfigMgr.Instance.GetCardConfig(cardCfg.ID * 100 + cardCfg.MaxRank);
        vo = new CardDataVO(cardCfgs.ID, cardCfgs.Rank, cardCfgs.MaxLevel);
        RoleVO roleVO = new RoleVO();
        roleVO.OnCardVO(vo);
        GameUIMgr.Instance.OpenModule(ModuleID.RoleInfo, roleVO);
    }

    private void OnCall()
    {
        if (GameConst.CardBagNum > HeroDataModel.Instance.mAllCards.Count)
            _bagCall.Show(_curItemView);
        else
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001112));
    }

    private void OnSell()
    {
        _bagSell.Show(_curItemView);
    }

    private void OnHideBtn()
    {
        _bagroad.SetActive(false);
    }

    private void OnGain()
    {
        ClearRecruitItem();
        _listBagRoadView = new List<BagRoadView>();
        if (_cfg == null || _cfg.GetLink == "")
            return;
        string[] attrs = _cfg.GetLink.Split(',');
        for (int i = 0; i < attrs.Length; i++)
        {
            GameObject obj = GameObject.Instantiate(_JumpObj);
            obj.transform.SetParent(_bagroadParent, false);
            BagRoadView roadView = new BagRoadView();
            roadView.SetDisplayObject(obj);
            roadView.Show(int.Parse(attrs[i]));
            _listBagRoadView.Add(roadView);
        }
        _bagroad.SetActive(true);
    }

    private void OnJump()
    {
        _bagroad.SetActive(false);
    }

    private void ClearRecruitItem()
    {
        if (_listBagRoadView != null)
        {
            for (int i = 0; i < _listBagRoadView.Count; i++)
                _listBagRoadView[i].Dispose();
            _listBagRoadView.Clear();
            _listBagRoadView = null;
        }
    }

    public override void Hide()
    {
        _curItemView.BlSelected = false;
        base.Hide();
    }

    public override void Dispose()
    {
        base.Dispose();
        ClearAttrText();
        ClearRecruitItem();
        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        _view = null;
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.FragmentCall);
    }
}
