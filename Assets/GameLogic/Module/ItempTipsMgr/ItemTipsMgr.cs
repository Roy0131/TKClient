using UnityEngine;
public class ItemTipsMgr : Singleton<ItemTipsMgr>
{
    private ItemTipsView _equipView;

    private void InitView()
    {
        if (_equipView == null)
        {
            _equipView = new ItemTipsView();
            GameObject obj = GameResMgr.Instance.LoadUIObjectSync(SingletonResName.UIEquipTips);
            _equipView.SetDisplayObject(obj);
        }
        GameUIMgr.Instance.AddObjectToTopRoot(_equipView.mRectTransform);
    }

    public void ShowRoleEquipTips(CardDataVO vo, int equipType)
    {
        InitView();
        _equipView.ShowTips(vo, equipType);
    }

    public void ShowItemTips(ItemConfig config, ItemTipsType type = ItemTipsType.NormalTips)
    {
        InitView();
        if (config.ItemType == 4 && config.ComposeDropID > 0 && config.ComposeDropID < 10000)
            type = ItemTipsType.FragmentTips;
        _equipView.ShowTips(config, type);
    }

    public void HideEquipView()
    {
        if (_equipView == null)
            return;
        _equipView.Hide();
    }
}