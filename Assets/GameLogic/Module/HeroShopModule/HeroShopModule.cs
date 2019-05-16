using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class HeroShopModule : ModuleBase
{
    private Button _disBtn;
    private HeroShopView _heroShopView;
    private Transform _root;
    private int _curShopType;

    public HeroShopModule()
        : base(ModuleID.HeroShop, UILayer.Popup)
    {
        mBlStack = false;
        _modelResName = UIModuleResName.UI_HeroShop;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _disBtn = Find<Button>("Root/Btn_Back");
        ColliderHelper.SetButtonCollider(_disBtn.transform);

        _heroShopView = new HeroShopView();
        _heroShopView.SetDisplayObject(Find("Root/HeroShopObj"));

        _root = Find<Transform>("Root");
        _disBtn.onClick.Add(OnClose);
    }

    protected override void AddEvent()
    {
        base.AddEvent();

        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(HeroEvent.HeroDetail, OnClose);
        ShopDataModel.Instance.AddEvent<int>(ShopEvent.ShopData, OnShopData);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(HeroEvent.HeroDetail, OnClose);
        ShopDataModel.Instance.RemoveEvent<int>(ShopEvent.ShopData, OnShopData);
    }

    private void OnShopData(int shopId)
    {
        if (shopId == _curShopType)
            _heroShopView.Show(_curShopType);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _curShopType = int.Parse(args[0].ToString());
        ShopDataModel.Instance.ReqShopData(_curShopType);
    }

    public override void Hide()
    {
        if (_heroShopView != null)
            _heroShopView.Hide();
        base.Hide();
    }

    public override void Dispose()
    {
        if (_heroShopView != null)
        {
            _heroShopView.Dispose();
            _heroShopView = null;
        }
        base.Dispose();
    }

    protected override void OnShowAnimator()
    {
        base.OnShowAnimator();
        ObjectHelper.PopAnimationBack(_root);
    }
}
