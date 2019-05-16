using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Framework.UI;

public class ResCostGroup : UIBaseView
{
    private GameObject _itemObject;
    private Dictionary<int, Text> _dictCurCost; //货币
    private Dictionary<int, Text> _dictConsResCost; //消耗品
    private List<GameObject> _lstItemObjects;
    private Dictionary<int, int> _dictResValue;

    private string _costValue;
    private bool _blCurEnough; //货币资源是否足够
    private bool _blConsResEnough; //消耗品是否足够

    public bool BlEnough
    {
        get { return _blConsResEnough && _blCurEnough; }
    }

	protected override void ParseComponent()
	{
        base.ParseComponent();
        _itemObject = Find("item");
        _blConsResEnough = true;
        _blCurEnough = true;
	}

    protected override void AddEvent()
    {
        base.AddEvent();
        BagDataModel.Instance.AddEvent<List<int>>(BagEvent.BagItemRefresh, OnItemChange);
        HeroDataModel.Instance.AddEvent(HeroEvent.HeroInfoChange, OnHeroInfoChange);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        BagDataModel.Instance.RemoveEvent<List<int>>(BagEvent.BagItemRefresh, OnItemChange);
        HeroDataModel.Instance.RemoveEvent(HeroEvent.HeroInfoChange, OnHeroInfoChange);
    }

    private void OnHeroInfoChange()
    {
        if (_dictCurCost != null && _dictCurCost.Count > 0)
        {
            int value;
            _blCurEnough = true;
            bool blTmp;
            foreach (var kv in _dictCurCost)
            {
                value = HeroDataModel.Instance.mHeroInfoData.GetCurrencyValue(kv.Key);
                blTmp = value >= _dictResValue[kv.Key];
                if (_blCurEnough)
                    _blCurEnough = blTmp;
                kv.Value.color = blTmp ? Color.white : Color.red;
                kv.Value.text = UnitChange.GetUnitNum(_dictResValue[kv.Key]);
            }
        }
    }

    private void OnItemChange(List<int> items)
    {
        if (_dictConsResCost != null && _dictConsResCost.Count > 0)
        {
            int value;
            _blConsResEnough = true;
            bool blTmp;
            foreach (var kv in _dictConsResCost)
            {
                value = BagDataModel.Instance.GetItemCountById(kv.Key);
                blTmp = value >= _dictResValue[kv.Key];
                if (_blConsResEnough)
                    _blConsResEnough = blTmp;
                kv.Value.color = blTmp ? Color.white : Color.red;
                kv.Value.text = UnitChange.GetUnitNum(_dictResValue[kv.Key]);
            }
        }
    }

    protected override void Refresh(params object[] args)
	{
        base.Refresh(args);
        string costValue = args[0].ToString();
        if (!string.Equals(costValue, _costValue))
        {
            _costValue = costValue;
            ClearResCostItems();
            _dictConsResCost = new Dictionary<int, Text>();
            _dictCurCost = new Dictionary<int, Text>();
            _lstItemObjects = new List<GameObject>();
            _dictResValue = new Dictionary<int, int>();
            string[] res = costValue.Split(',');
            if (res.Length % 2 != 0)
                return;
            GameObject itemObject;
            int id;
            ItemConfig itemConfig;
            Text itemText;
            int value;
            for (int i = 0; i < res.Length; i+=2)
            {
                itemObject = GameObject.Instantiate(_itemObject);
                _lstItemObjects.Add(itemObject);
                itemObject.transform.SetParent(mRectTransform, false);
                itemObject.SetActive(true);

                id = int.Parse(res[i]);
                value = int.Parse(res[i + 1]);
                itemConfig = GameConfigMgr.Instance.GetItemConfig(id);

                itemObject.transform.Find("icon").GetComponent<Image>().sprite = GameResMgr.Instance.LoadItemIcon(itemConfig.UIIcon);
                ObjectHelper.SetSprite(itemObject.transform.Find("icon").GetComponent<Image>(), itemObject.transform.Find("icon").GetComponent<Image>().sprite);
                itemText = itemObject.transform.Find("count").GetComponent<Text>();

                if (itemConfig.ItemType == ItemType.Currency)
                    _dictCurCost.Add(id, itemText);
                else
                    _dictConsResCost.Add(id, itemText);
                _dictResValue.Add(id, value);
            }
        }
        _blCurEnough = true;
        _blConsResEnough = true;
        OnHeroInfoChange();
        OnItemChange(null);
	}

    private void ClearResCostItems()
    {
        if (_lstItemObjects == null)
            return;
        for (int i = 0; i < _lstItemObjects.Count; i++)
            GameObject.Destroy(_lstItemObjects[i]);
        _lstItemObjects.Clear();
        _lstItemObjects = null;

        _dictConsResCost.Clear();
        _dictConsResCost = null;

        _dictCurCost.Clear();
        _dictCurCost = null;

        _dictResValue.Clear();
        _dictResValue = null;
    }

    public override void Dispose()
    {
        ClearResCostItems();
        _itemObject = null;
        base.Dispose();
    }
}