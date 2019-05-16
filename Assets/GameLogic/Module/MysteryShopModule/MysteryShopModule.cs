using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class MysteryShopModule : ModuleBase
{
    private Button _disBtn;

    private MysteryShopView _mysteryShopView;
    private bool _isfirst = true;
    private Transform _root;
    public MysteryShopModule()
        : base(ModuleID.MysteryShop, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_MysteryShop;
        _soundName = UIModuleSoundName.MysteryShopSoundName;
        mBlNeedBackMask = true;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _root = Find<Transform>("shop");
        _disBtn = Find<Button>("Btn_Back");
        ColliderHelper.SetButtonCollider(_disBtn.transform);

        _mysteryShopView = new MysteryShopView();
        _mysteryShopView.SetDisplayObject(Find("ShopObj"));
        
        _disBtn.onClick.Add(OnClose);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        ShopDataModel.Instance.AddEvent<int>(ShopEvent.ShopData, OnShopData);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        ShopDataModel.Instance.RemoveEvent<int>(ShopEvent.ShopData, OnShopData);
    }

    private void OnShopData(int shopId)
    {
        if (shopId == ShopIdConst.MYSTERYSHOP)
            _mysteryShopView.Show(shopId);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        ShopDataModel.Instance.ReqShopData(ShopIdConst.MYSTERYSHOP);
    }

    public override void Hide()
    {
        if (_mysteryShopView != null)
            _mysteryShopView.Hide();
        base.Hide();
        StopAllEffectSound();
    }

    public override void Dispose()
    {
        if (_mysteryShopView != null)
        {
            _mysteryShopView.Dispose();
            _mysteryShopView = null;
        }
        base.Dispose();
    }

    protected override void OnShowAnimator()
    {
        base.OnShowAnimator();
        if (_isfirst)
            ObjectHelper.AnimationMoveLiner(_root, ObjectHelper.direction.right);
        _isfirst = false;
    }

    protected override void OnClose()
    {
        base.OnClose();
        _isfirst = true;
    }
}
