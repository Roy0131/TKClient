using NewBieGuide;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public class TipsViewBase : UIBaseView
{
    protected Text _equipNameText;
    protected Text _equipTypeText;
    protected Text _powerText;
    protected RectTransform _backGround;

    protected ItemView _itemView;
    protected RectTransform _itemViewRoot;

    protected Button _fun1Btn;
    protected Button _fun2Btn;
    protected Text _fun1BtnText;
    protected Text _fun2BtnText;
    protected RectTransform _btnsRect;

    protected ItemTipsType _type;
    protected int _equipType;

    protected float _height = 250f;
    protected float _textHeight = 32f;
    protected float _startY = -170;

    protected RectTransform _suitRoot;
    protected List<Text> _lstSuitText;
    protected Image _suitBg;

    protected RectTransform _attriRoot;
    protected GameObject _attriObject;
    protected List<Text> _lstAttrText;

    protected Text _descripteText;

    protected int composeDropID = 0;


    public TipsViewBase()
    {

    }

    protected override void ParseComponent()
    {
        base.ParseComponent();

        _backGround = Find<RectTransform>("BackGround");
        _equipNameText = Find<Text>("EquipNameText");
        _equipTypeText = Find<Text>("Image/EquipType");
        _powerText = Find<Text>("BattlePowerText");

        _itemViewRoot = Find<RectTransform>("ItemContainer");
        _attriRoot = Find<RectTransform>("AttriObject");
        _attriObject = Find("AttriObject/AttrItem");

        _suitRoot = Find<RectTransform>("SuitObject");
        _suitBg = Find<Image>("SuitObject/SuitBG");

        _lstSuitText = new List<Text>();
        for (int i = 0; i < 4; i++)
            _lstSuitText.Add(Find<Text>("SuitObject/SuitRoot/SuitItem" + i));

        _btnsRect = Find<RectTransform>("Buttons");
        _fun1Btn = Find<Button>("Buttons/Btn1");
        _fun2Btn = Find<Button>("Buttons/Btn2");
        _fun1BtnText = Find<Text>("Buttons/Btn1/Text");
        _fun2BtnText = Find<Text>("Buttons/Btn2/Text");
        _descripteText = Find<Text>("DescriptText");
        _fun1Btn.onClick.Add(OnFun1Click);
        _fun2Btn.onClick.Add(OnFun2Click);

        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.EquipUse, _fun1Btn.transform);
    }

    private void FillBaseData(ItemConfig config, int suitCount = 0)
    {
        ClearAttrText();

        _lstAttrText = new List<Text>();
        _equipNameText.text = LanguageMgr.GetLanguage(config.NameID);
        _equipTypeText.text = LanguageMgr.GetLanguage(GameConst.GetItemTypeLanguageId(config.ItemType, config.EquipType));
        if (config.DescrptionID > 0)
            _descripteText.text = LanguageMgr.GetLanguage(config.DescrptionID);
        else
            _descripteText.text = "";
        if (_itemView != null)
            ItemFactory.Instance.ReturnItemView(_itemView);
        if (config.ItemType == 2)
            _itemView = ItemFactory.Instance.CreateItemView(config, ItemViewType.EquipItem, OnClick);
        else
            _itemView = ItemFactory.Instance.CreateItemView(config, ItemViewType.BagItem, OnClick);
        _itemView.mRectTransform.SetParent(_itemViewRoot, false);

        _height = 250f;
        if(!string.IsNullOrEmpty(config.EquipAttr))
        {
            string[] attrs = config.EquipAttr.Split(',');
            if (attrs.Length % 2 != 0)
            {
                LogHelper.LogError("[TipsViewBase.FillBaseData() => item id:" + config.ID + " EquipAttr format error!!!]");
                return;
            }
            string attrStr;
            for (int i = 0; i < attrs.Length; i += 2)
            {
                attrStr = attrs[i] + "," + attrs[i + 1];
                ParseSuit(CreateText(), attrStr, _defColor);
                _height += _textHeight;
            }
        }
        if (!string.IsNullOrEmpty(config.EquipSkill))
        {
            Text t = CreateText();
            int skillId = int.Parse(config.EquipSkill);
            SkillConfig skillCfg = GameConfigMgr.Instance.GetSkillConfig(skillId);
            t.text = LanguageMgr.GetLanguage(skillCfg.DescrptionID, skillCfg.ShowParam.Split(','));
            _height += _textHeight;
        }
    }

    private void OnClick(ItemView view)
    {

    }

    private Color _defColor = new Color(134f / 255f, 86f / 255f, 66f / 255f, 1f);
    private Text CreateText()
    {
        GameObject attrObject = GameObject.Instantiate(_attriObject);
        attrObject.transform.SetParent(_attriRoot, false);
        attrObject.SetActive(true);
        Text t = attrObject.GetComponent<Text>();
        _lstAttrText.Add(t);
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

    protected Color _suitYColor = new Color(165f / 255f, 230f / 255f, 52f / 255f, 1f);
    protected Color _suitNColor = new Color(221f / 255f, 223f / 255f, 222f / 255f, 1f);
    private void FillSuitData(int suitID, int suitCount = 0)
    {
        _suitRoot.gameObject.SetActive(suitID > 0);
        if (suitID > 0)
        {

            int len = suitCount == 0 ? 0 : suitCount;

            SuitConfig suitConfig = GameConfigMgr.Instance.GetSuitConfig(suitID);
            Text t = _lstSuitText[0];
            t.text = LanguageMgr.GetLanguage(suitConfig.NameID) + "(" + len + "/4)";
            ParseSuit(_lstSuitText[1], suitConfig.AttrSuit2, len >= 2 ? _suitYColor : _suitNColor);
            ParseSuit(_lstSuitText[2], suitConfig.AttrSuit3, len >= 3 ? _suitYColor : _suitNColor);
            ParseSuit(_lstSuitText[3], suitConfig.AttrSuit4, len >= 4 ? _suitYColor : _suitNColor);
            _suitRoot.anchoredPosition = new Vector2(0f, -(_height - 95f));
            _height += 150;
        }
        _backGround.sizeDelta = new Vector2(552f, _height);
        _btnsRect.anchoredPosition = new Vector2(-273f, -_height + 80f);
        mRectTransform.gameObject.SetActive(false);
        mRectTransform.gameObject.SetActive(true);
        //mRectTransform.anchoredPosition = new Vector2(0, (Screen.height - _height) / 2f);
    }

    public void Layout(ItemTipsType type)
    {
        _type = type;
        if (!_fun1Btn.gameObject.activeSelf)
            _fun1Btn.gameObject.SetActive(true);
        switch (_type)
        {
            case ItemTipsType.RoleEquipTips:
                if (!_fun2Btn.gameObject.activeSelf)
                    _fun2Btn.gameObject.SetActive(true);
                break;
            case ItemTipsType.EquipBagTips:
                _fun1BtnText.text = LanguageMgr.GetLanguage(210122);
                if (_fun2Btn.gameObject.activeSelf)
                    _fun2Btn.gameObject.SetActive(false);
                break;
            case ItemTipsType.NormalTips:
                if (_fun1Btn.gameObject.activeSelf)
                    _fun1Btn.gameObject.SetActive(false);
                if (_fun2Btn.gameObject.activeSelf)
                    _fun2Btn.gameObject.SetActive(false);
                break;
            case ItemTipsType.FragmentTips:
                _fun1BtnText.text = LanguageMgr.GetLanguage(5001103);
                if (_fun2Btn.gameObject.activeSelf)
                    _fun2Btn.gameObject.SetActive(false);
                break;
        }
    }

    public void ShowRoleEquipTips(CardDataVO vo, int equipType, ItemTipsType tipsType)
    {
        Layout(tipsType);
        int equipId = vo.GetEquipIdByEquipType(equipType);
        ItemConfig config = GameConfigMgr.Instance.GetItemConfig(equipId);
        FillBaseData(config);
        FillSuitData(config.SuitID, vo.GetSuitCount(config.SuitID));
        _equipType = equipType;
        if (_equipType == EquipmentType.Artifact)
        {
            _fun1BtnText.text = LanguageMgr.GetLanguage(5003010);
            _fun2BtnText.text = LanguageMgr.GetLanguage(5002706);
            if (config.ShowStar == 5)
                _fun2Btn.gameObject.SetActive(false);
            else
                _fun2Btn.gameObject.SetActive(true);
        }
        else if (_equipType == EquipmentType.GemStone)
        {
            _fun1BtnText.text = LanguageMgr.GetLanguage(5002717);
            _fun2BtnText.text = LanguageMgr.GetLanguage(5002706);
            if (config.ShowStar == 5 && config.Quality == 6)
                _fun2Btn.gameObject.SetActive(false);
            else
                _fun2Btn.gameObject.SetActive(true);
        }
        else
        {
            _fun1BtnText.text = LanguageMgr.GetLanguage(5003010);
            _fun2BtnText.text = LanguageMgr.GetLanguage(6001242);
            _fun2Btn.gameObject.SetActive(true);
        }
    }

    public void ShowEquipBagTips(ItemConfig config, ItemTipsType tipsType)
    {
        composeDropID = config.ComposeDropID;
        Layout(tipsType);
        FillBaseData(config);
        FillSuitData(config.SuitID);

        if (config.ItemType == 4)
        {
            _backGround.sizeDelta = new Vector2(552f, 300);
            _btnsRect.anchoredPosition = new Vector2(-273f, -220);
        }
    }

    protected void OnFun1Click()
    {
        HideTips();
        if (_type == ItemTipsType.RoleEquipTips)
            GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(EquipEvent.RoleEquipTipsFun1Called, _equipType);
        else if (_type == ItemTipsType.EquipBagTips)
            GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(EquipEvent.EquipBagTipsFun1Called);
        else if (_type == ItemTipsType.FragmentTips && composeDropID > 0)
        {
            GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(HeroEvent.HeroDetail);
            CardDataVO vo;
            CardConfig cardCfg = GameConfigMgr.Instance.GetCardConfig(composeDropID * 100 + 1);
            CardConfig cardCfgs = GameConfigMgr.Instance.GetCardConfig(cardCfg.ID * 100 + cardCfg.MaxRank);
            vo = new CardDataVO(cardCfgs.ID, cardCfgs.Rank, cardCfgs.MaxLevel);
            RoleVO roleVO = new RoleVO();
            roleVO.OnCardVO(vo);
            GameUIMgr.Instance.OpenModule(ModuleID.RoleInfo, roleVO);
        }
    }

    protected void OnFun2Click()
    {
        HideTips();
        if (_type == ItemTipsType.RoleEquipTips)
            GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(EquipEvent.RoleEquipTipsFun2Called, _equipType);
    }

    private void HideTips()
    {
        ItemTipsMgr.Instance.HideEquipView();
    }


    private void ClearAttrText()
    {
        if (_lstAttrText == null)
            return;
        if (_lstAttrText.Count > 0)
        {
            for (int i = 0; i < _lstAttrText.Count; i++)
                GameObject.Destroy(_lstAttrText[i].gameObject);
            _lstAttrText.Clear();
            _lstAttrText = null;
        }
    }

    public override void Dispose()
    {
        ClearAttrText();
        if (_lstSuitText != null)
        {
            _lstSuitText.Clear();
            _lstSuitText = null;
        }
        if (_itemView != null)
        {
            ItemFactory.Instance.ReturnItemView(_itemView);
            _itemView = null;
        }
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.EquipUse);
        base.Dispose();
    }
}