using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Framework.UI;

public class ItemResGroup : UIBaseView
{
    private GameObject _item;
    private List<int> _lstItems;

    private Dictionary<int, Text> _dictCurItems;
    private Dictionary<int, Text> _dictConsItems;
    private List<GameObject> _lstResObject;
	protected override void ParseComponent()
	{
        base.ParseComponent();
        _item = Find("Item");
        _lstItems = new List<int>();
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
        if (_dictCurItems != null && _dictCurItems.Count > 0)
        {
            foreach (var kv in _dictCurItems)
                kv.Value.text = UnitChange.GetUnitNum(HeroDataModel.Instance.mHeroInfoData.GetCurrencyValue(kv.Key));
        }
    }

	private void OnItemChange(List<int> items)
    {
        if(_dictConsItems != null && _dictConsItems.Count > 0)
        {
            if(items == null)
            {
                int value;
                foreach (var kv in _dictConsItems)
                {
                    value = BagDataModel.Instance.GetItemCountById(kv.Key);
                    kv.Value.text = UnitChange.GetUnitNum(value);
                }
            }
            else
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (_dictConsItems.ContainsKey(items[i]))
                        _dictConsItems[items[i]].text = UnitChange.GetUnitNum(BagDataModel.Instance.GetItemCountById(items[i]));
                }
            }
        }
    }

	protected override void Refresh(params object[] args)
	{
        base.Refresh(args);
        bool blChange = false;
        int id, i;
        if (args.Length == _lstItems.Count)
        {
            for (i = 0; i < args.Length; i++)
            {
                id = int.Parse(args[i].ToString());
                if (!_lstItems.Contains(id))
                {
                    blChange = true;
                    break;
                }
            }
        }
        else
        {
            blChange = true;
        }
        if (blChange)
        {
            _lstItems.Clear();
            _dictCurItems = new Dictionary<int, Text>();
            _dictConsItems = new Dictionary<int, Text>();
            GameObject itemObj;
            ItemConfig config;

            ClearItemObject();
            Text _text;
            _lstResObject = new List<GameObject>();
            _dictConsItems = new Dictionary<int, Text>();
            _dictCurItems = new Dictionary<int, Text>();
            for (i = 0; i < args.Length; i++)
            {
                id = int.Parse(args[i].ToString());
                config = GameConfigMgr.Instance.GetItemConfig(id);
                itemObj = GameObject.Instantiate(_item);
                itemObj.transform.SetParent(mRectTransform, false);
                itemObj.transform.Find("icon").GetComponent<Image>().sprite = GameResMgr.Instance.LoadItemIcon(config.UIIcon);
                ObjectHelper.SetSprite(itemObj.transform.Find("icon").GetComponent<Image>(), itemObj.transform.Find("icon").GetComponent<Image>().sprite);
                _text = itemObj.transform.Find("countText").GetComponent<Text>();
                itemObj.SetActive(true);
                _lstResObject.Add(itemObj);
                if (config.ItemType == ItemType.Currency && ItemType.IsReallyCurrency(config.ID))
                {
                    _dictCurItems.Add(id, _text);
                    _text.text = UnitChange.GetUnitNum(HeroDataModel.Instance.mHeroInfoData.GetCurrencyValue(id));
                }    
                else 
                {
                    _dictConsItems.Add(id, _text);
                    _text.text = UnitChange.GetUnitNum(BagDataModel.Instance.GetItemCountById(id));
                }   
                _lstItems.Add(id);
            }   
        }
        else
        {
            OnHeroInfoChange();
            OnItemChange(null);
        }
    }

    private void ClearItemObject()
    {
        if (_lstResObject == null)
            return;
        for (int i = 0; i < _lstResObject.Count; i++)
            GameObject.Destroy(_lstResObject[i]);
        _lstResObject.Clear();
        _lstResObject = null;

        if (_dictConsItems != null)
        {
            _dictConsItems.Clear();
            _dictConsItems = null;
        }

        if (_dictCurItems != null)
        {
            _dictCurItems.Clear();
            _dictCurItems = null;
        }
    }

	public override void Dispose()
	{
        ClearItemObject();
        if(_lstItems != null)
        {
            _lstItems.Clear();
            _lstItems = null;
        }
        base.Dispose();
	}
}