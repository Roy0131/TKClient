using UnityEngine.UI;
using Framework.UI;

public class ItemTipsView : UIBaseView
{
    private Button _closeBtn;
    private TipsViewBase _tipsViewBase;
    protected override void ParseComponent()
    {
        base.ParseComponent();

        _tipsViewBase = new TipsViewBase();
        _tipsViewBase.SetDisplayObject(Find("RoleEquipTips"));

        _closeBtn = Find<Button>("Collider");
        _closeBtn.onClick.Add(HideEquipTips);
    }

    public void ShowTips(CardDataVO vo, int equipType)
    {
        _tipsViewBase.ShowRoleEquipTips(vo, equipType, ItemTipsType.RoleEquipTips);
        Show();
    }

    public void ShowTips(ItemConfig config, ItemTipsType type)
    {
        _tipsViewBase.ShowEquipBagTips(config, type);
        Show();
    }

    private void HideEquipTips()
    {
        ItemTipsMgr.Instance.HideEquipView();
    }

    public override void Dispose()
    {
        if (_tipsViewBase != null)
        {
            _tipsViewBase.Dispose();
            _tipsViewBase = null;
        }
        base.Dispose();
    }
}