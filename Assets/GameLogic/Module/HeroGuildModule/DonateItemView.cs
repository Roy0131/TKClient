using Framework.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DonateItemView : UIBaseView
{
    private Text _itemNum;
    private Image _fillImg;
    private RectTransform _itemParent;
    private GameObject _selected;
    public int itemId { get; private set; }
    private bool _blSelected;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _itemNum = Find<Text>("Callnum/Text");
        _fillImg = Find<Image>("Callnum/Img");
        _itemParent = Find<RectTransform>("Obj");
        _selected = Find("Selected");
    }

    protected override void AddEvent()
    {
        base.AddEvent();
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        itemId = int.Parse(args[0].ToString());
        OnDonateItem();
    }

    private void OnClik(ItemView view)
    {
        GameEventMgr.Instance.mGlobalDispatcher.DispathEvent(GuildEvent.ReqDonate, this);
    }

    private void OnDonateItem()
    {
        DiposeChildren();
        _childrenViews = new List<UIBaseView>();
        ItemInfo itemInfo = new ItemInfo();
        itemInfo.Id = itemId;
        itemInfo.Value = 1;
        ItemView view= ItemFactory.Instance.CreateItemView(itemInfo, ItemViewType.ShopItem,OnClik);
        view.mRectTransform.SetParent(_itemParent, false);
        AddChildren(view);
        _itemNum.text = BagDataModel.Instance.GetItemCountById(itemId) + "/" + GameConfigMgr.Instance.GetItemConfig(itemId).ComposeNum;
        _fillImg.fillAmount = (float)BagDataModel.Instance.GetItemCountById(itemId) / (float)GameConfigMgr.Instance.GetItemConfig(itemId).ComposeNum;
    }

    public override void Hide()
    {
        base.Hide();
        _blSelected = false;
        _selected.SetActive(false);
    }

    public bool BlSelected
    {
        get { return _blSelected; }
        set
        {
            if (_blSelected == value)
                return;
            _blSelected = value;
            _selected.SetActive(_blSelected == true);
        }
    }
}
