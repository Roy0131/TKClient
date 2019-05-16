using Msg.ClientMessage;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public class BagItemView : UIBaseView
{
    private Text _callText;
    private Image _callImg;
    private RectTransform _parent;
    private GameObject _callObj;
    public ItemInfo _itemInfo { get; private set; }
    public ItemView _view { get; private set; }

    private bool isClick;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _callText = Find<Text>("Callnum/Text");
        _callImg = Find<Image>("Callnum/Img");
        _parent = Find<RectTransform>("ItemObj");
        _callObj = Find("Callnum");
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _itemInfo = args[0] as ItemInfo;
        isClick = bool.Parse(args[1].ToString());
        OnItemChang();
    }

    private void OnClick(ItemView item)
    {
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(BagEvent.Click, item);
    }

    private void OnItemChang()
    {
        ItemConfig cfg = GameConfigMgr.Instance.GetItemConfig(_itemInfo.Id);
        if (cfg.ItemType == 4)
        {
            _parent.anchoredPosition = new Vector3(0f, 10f, 0f);
            _callObj.SetActive(true);
            _callText.text = BagDataModel.Instance.GetItemCountById(_itemInfo.Id) + "/" + cfg.ComposeNum;
            if (BagDataModel.Instance.GetItemCountById(_itemInfo.Id) >= cfg.ComposeNum)
                _callImg.fillAmount = 1;
            else
                _callImg.fillAmount = (float)BagDataModel.Instance.GetItemCountById(_itemInfo.Id) / (float)cfg.ComposeNum;
        }
        else
        {
            _parent.anchoredPosition = new Vector3(0f, 0f, 0f);
            _callObj.SetActive(false);
        }
        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        if (cfg.ItemType == 2)
            _view = ItemFactory.Instance.CreateItemView(_itemInfo, ItemViewType.EquipItem, OnClick);
        else
            _view = ItemFactory.Instance.CreateItemView(_itemInfo, ItemViewType.ShopItem, OnClick);
        _view.mRectTransform.SetParent(_parent, false);
        if (isClick)
            OnClick(_view);
    }

    public override void Dispose()
    {
        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        _view = null;
        base.Dispose();
    }
}
