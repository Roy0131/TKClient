using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Framework.UI;

public class EquipFuncItemView : UIBaseView
{
    private bool _blLeft;
    private GameObject _unknowFlag;

    private GameObject _itemInfoObject;
    private RectTransform _itemViewRoot;
    private RectTransform _attrRoot;
    private GameObject _attrObject;
    private List<Text> _lstAttr;
    private List<Text> _lstDesText;
    private Text _nameText;
    private ItemView _itemView;

    private int _itemId;
    public EquipFuncItemView(bool blLeft)
    {
        _blLeft = blLeft;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _unknowFlag = Find("UnknowObject");

        _itemInfoObject = Find("ItemInfoObject");
        _itemViewRoot = Find<RectTransform>("ItemInfoObject/ItemRoot");
        _attrRoot = Find<RectTransform>("ItemInfoObject/Scroll/AttrRoot");
        _attrObject = Find("ItemInfoObject/Scroll/AttrRoot/AttrText");
        _nameText = Find<Text>("ItemInfoObject/NameText");

        _unknowFlag.SetActive(!_blLeft);
        _itemInfoObject.SetActive(_blLeft);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        int itemID = int.Parse(args[0].ToString());
        if (itemID == 0)
        {
            if (!_unknowFlag.activeSelf)
                _unknowFlag.SetActive(true);
            if (_itemInfoObject.activeSelf)
                _itemInfoObject.SetActive(false);
            return;
        }
        //if (_itemId == itemID)
        //    return;
        if (_unknowFlag.activeSelf)
            _unknowFlag.SetActive(false);
        if (!_itemInfoObject.activeSelf)
            _itemInfoObject.SetActive(true);
        ClearAttr();
        _itemId = itemID;
        ItemConfig config = GameConfigMgr.Instance.GetItemConfig(itemID);
        if (_itemView != null)
            ItemFactory.Instance.ReturnItemView(_itemView);
        _itemView = ItemFactory.Instance.CreateItemView(config, ItemViewType.EquipHeroItem);
        _itemView.mRectTransform.SetParent(_itemViewRoot, false);
        _nameText.text = LanguageMgr.GetLanguage(config.NameID);
        if (string.IsNullOrEmpty(config.EquipAttr))
            return;
        string[] equipAttr = config.EquipAttr.Split(',');
        if (equipAttr.Length % 2 != 0)
        {
            LogHelper.LogError("EquipFuncItemView.Refresh() => config euquipattr format error!!");
            return;
        }
        ClearAttrText();
        _lstAttr = new List<Text>();
        _lstDesText = new List<Text>();
        GameObject attrObject;
        Text attrText;
        AttributeConfig attrConfig;
        for (int i = 0; i < equipAttr.Length; i += 2)
        {
            int attrId = int.Parse(equipAttr[i]);
            float value = float.Parse(equipAttr[i + 1]);
            attrObject = GameObject.Instantiate(_attrObject);
            attrText = attrObject.transform.GetComponent<Text>();
            attrObject.SetActive(true);
            attrObject.transform.SetParent(_attrRoot, false);
            attrConfig = GameConfigMgr.Instance.GetAttrConfig(attrId);
            if (attrConfig.PercentShow == 0)
            {
                attrText.text = LanguageMgr.GetLanguage(attrConfig.NameID) + "  +" + (value / attrConfig.Divisor).ToString();
            }
            else
            {
                if (attrConfig.Divisor != 0)
                    value = (float)value / (float)attrConfig.Divisor;
                attrText.text = LanguageMgr.GetLanguage(attrConfig.NameID) + "  +" + value.ToString("F1") + "%";
            }
            _lstAttr.Add(attrText);
        }
        if (!string.IsNullOrEmpty(config.EquipSkill))
        {
            Text t = CreateText();
            int skillId = int.Parse(config.EquipSkill.ToString());
            SkillConfig skillCfg = GameConfigMgr.Instance.GetSkillConfig(skillId);
            t.text = LanguageMgr.GetLanguage(skillCfg.DescrptionID, skillCfg.ShowParam.Split(','));
        }
    }

    private Text CreateText()
    {
        GameObject attrObject = GameObject.Instantiate(_attrObject);
        attrObject.transform.SetParent(_attrRoot, false);
        attrObject.SetActive(true);
        Text t = attrObject.GetComponent<Text>();
        _lstDesText.Add(t);
        return t;
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

    private void ClearAttr()
    {
        if (_lstAttr == null)
            return;
        for (int i = 0; i < _lstAttr.Count; i++)
            GameObject.Destroy(_lstAttr[i].gameObject);
        _lstAttr.Clear();
        _lstAttr = null;
    }

    public override void Dispose()
    {
        ClearAttrText();
        ClearAttr();
        base.Dispose();
    }
}