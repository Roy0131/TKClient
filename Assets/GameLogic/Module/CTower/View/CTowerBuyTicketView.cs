using Framework.UI;
using UnityEngine.UI;

public class CTowerBuyTicketView : UIBaseView {

    private Button _btnClose;
    private Button _btnAdd;
    private Button _btnSub;
    private Text _inputFieldText;
    private Text _ticketNum;
    private Button _btnBuy;

    private int _num;
    private int DiamondNum;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _btnClose = Find<Button>("BlackBack");
        _btnBuy = Find<Button>("Content/ButtonBuy");
        _btnAdd = Find<Button>("Content/BuyTicket/ButtonAdd");
        _btnSub = Find<Button>("Content/BuyTicket/ButtonSub");
        _inputFieldText = Find<Text>("Content/BuyTicket/InputField/TextNum");
        _ticketNum = Find<Text>("Content/ImageBack02/TextNum");

        _btnClose.onClick.Add(OnClose);
        _btnBuy.onClick.Add(BuyTicket);
        _btnAdd.onClick.Add(OnAdd);
        _btnSub.onClick.Add(OnSub);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        ShopDataModel.Instance.AddEvent<int>(ShopEvent.ShopData, OnShowBuyTicket);
        ShopDataModel.Instance.AddEvent<int>(ShopEvent.ShopBuy, OnShopBuy);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        ShopDataModel.Instance.RemoveEvent<int>(ShopEvent.ShopData, OnShowBuyTicket);
        ShopDataModel.Instance.RemoveEvent<int>(ShopEvent.ShopBuy, OnShopBuy);
    }

    private void OnShopBuy(int id)
    {
        OnClose();
    }

    private void OnShowBuyTicket(int shopId)
    {
        if (shopId != ShopIdConst.TOWERSHOP)
            return;
        _num = 1;
        InputFieldTicket();
        DisDiamondNum();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        ShopDataModel.Instance.ReqShopData(ShopIdConst.TOWERSHOP);
    }
    //门票显示数量
    private void InputFieldTicket()
    {
        _inputFieldText.text = _num.ToString();
    }

    //门票增加
    private void OnAdd()
    {
        _num++;
        InputFieldTicket();
        DisDiamondNum();
    }
    //门票减少
    private void OnSub()
    {
        _num = _num > 0 ?--_num  : 0;
        InputFieldTicket();
        DisDiamondNum();
    }
    //钻石数量显示
    private void DisDiamondNum()
    {
        DiamondNum = HeroDataModel.Instance.mHeroInfoData.mDiamond;
        _ticketNum.text = DiamondNum+"/"+_num*5;
        
    }
    
    //购买门票
    private void BuyTicket()
    {
        if (ShopDataModel.Instance.GetShopDataByShopId(ShopIdConst.TOWERSHOP).mListItemVO.Count == 0)
        {
            LogHelper.Log("没有该类商品的数据");
            return;
        }
        else
        {
            int id = ShopDataModel.Instance.GetShopDataByShopId(ShopIdConst.TOWERSHOP).mListItemVO[0].mId;
            if (DiamondNum < _num * 5)
            {
                PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000055));
                return;
            }
            GameNetMgr.Instance.mGameServer.ReqShopBuy(ShopIdConst.TOWERSHOP, id, _num);
            TDPostDataMgr.Instance.DoCostDiamond(TDCostDiamondType.BuyTowerTicket, _num,5);
        }
    }

    private void OnClose()
    {
        Hide();
    }
}
