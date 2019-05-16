using Msg.ClientMessage;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public class BagSellView : UIBaseView
{
    private Text _name;
    private Text _price;
    private Image _sellImg;
    private InputField _inputField;
    private Button _cutBtn;
    private Button _addBtn;
    private Button _sellBtn;
    private Button _wholeBtn;
    private ItemView _itemView;
    private ItemConfig _cfg;
    private RectTransform _parent;
    private ItemView _view;
    private ItemConfig _curItemConfig;

    int Sellnum = 1;//出售数量
    int Number;//当前物品数量
    int Money;
    int SellID;

    int itemCfgId = 0;
    int itemCfgNum = 0;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _name = Find<Text>("SellPanel/Name");
        _price = Find<Text>("ImageGold/Text");
        _sellImg = Find<Image>("ImageGold/Image");
        _inputField = Find<InputField>("SellPanel/InputField");
        _cutBtn = Find<Button>("SellPanel/CutBtn");
        _addBtn = Find<Button>("SellPanel/AddBtn");
        _sellBtn = Find<Button>("SellPanel/SellBtn");
        _wholeBtn = Find<Button>("WholeBtn");
        _parent = Find<RectTransform>("SellPanel/BagItem");

        _cutBtn.onClick.Add(OnCutnum);
        _addBtn.onClick.Add(OnAddnum);
        _sellBtn.onClick.Add(OnSellSend);
        _wholeBtn.onClick.Add(OnHideBtn);

        _inputField.onValueChanged.Add(delegate { OnDele(); });
    }

    public void OnSell()
    {
        _curItemConfig = GameConfigMgr.Instance.GetItemConfig(_cfg.ID);

        string[] univalent = _curItemConfig.SellReward.Split(',');
        if (univalent.Length % 2 != 0)
            return;
        for (int i = 0; i < univalent.Length; i += 2)
        {
            itemCfgId = int.Parse(univalent[i]);
            itemCfgNum = int.Parse(univalent[i + 1]);
        }

        SellID = _cfg.ID;
        Number = _itemView.mItemDataVO.mCount;
        Money = itemCfgNum;
        _name.text = LanguageMgr.GetLanguage(_cfg.NameID);
        _sellImg.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(itemCfgId).Icon);
        ObjectHelper.SetSprite(_sellImg,_sellImg.sprite);
        ObjectHelper.SetSprite(_sellImg,_sellImg.sprite);
        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        _view = ItemFactory.Instance.CreateItemView(_itemView.mItemDataVO.mItemConfig, ItemViewType.EquipItem, OnClick);
        _view.mRectTransform.SetParent(_parent, false);
    }

    private void OnClick(ItemView view)
    {

    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _itemView = args[0] as ItemView;
        _cfg = _itemView.mItemDataVO.mItemConfig;
        OnSell();
        OnPlader(Number);
        OnPrice();
    }

    private void OnDele()
    {
        if (_inputField.text != "")
        {
            if (Number >= int.Parse(_inputField.text))
            {
                if (int.Parse(_inputField.text) >= 1)
                {
                    Sellnum = int.Parse(_inputField.text);
                }
                else
                {
                    Sellnum = 1;
                    OnPlader(Sellnum);
                }
            }
            else
            {
                _inputField.text = Number.ToString();
            }
        }
        OnPrice();
    }

    private void OnPrice()
    {
        _price.text = (Sellnum * Money).ToString();
    }

    private void OnSellSend()
    {
        //向服务器发一条消息
        GameNetMgr.Instance.mGameServer.ReqItemSell(SellID, Sellnum);
        Hide();
    }

    private void OnPlader(int num)
    {
        _inputField.text = num.ToString();
    }

    private void OnCutnum()
    {
        if (Sellnum <= 1)
            return;
        Sellnum -= 1;
        OnPlader(Sellnum);
        OnPrice();
    }

    private void OnAddnum()
    {
        if (Sellnum>=Number)
            return;
        Sellnum+=1;
        OnPlader(Sellnum);
        OnPrice();
    }

    private void OnHideBtn()
    {
        Hide();
    }

    public override void Dispose()
    {
        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        _view = null;
        base.Dispose();
    }
}
