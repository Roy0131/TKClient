using Framework.UI;
using UnityEngine;

public class SevenItemView : UIBaseView
{
    private GameObject _sele;
    private RectTransform _parent;
    private GameObject _speciallyObj;
    private UIEffectView _effect;
    private ItemView _view;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _parent = Find<RectTransform>("Pos");
        _sele = Find("Sele");
        _speciallyObj = Find("fx_ui_qiandao");
        _effect = CreateUIEffect(_speciallyObj, UILayerSort.WindowSortBeginner);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        SevenDataVO _sevenVO = args[0] as SevenDataVO;
        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        if (GameConfigMgr.Instance.GetItemConfig(_sevenVO.mInfo.Id).ItemType == 2)
            _view = ItemFactory.Instance.CreateItemView(_sevenVO.mInfo, ItemViewType.EquipItem);
        else
            _view = ItemFactory.Instance.CreateItemView(_sevenVO.mInfo, ItemViewType.BagItem);
        _view.mRectTransform.SetParent(_parent, false);
        if (_sevenVO.mStatus == 0 && _sevenVO.mHeaven == _sevenVO.mCurHeaven)
            _effect.PlayEffect();
        else
            _effect.StopEffect();
        _sele.SetActive(_sevenVO.mStatus == 1);
        if (_sevenVO.mStatus == 1)
            _view.SetGray();
        else
            _view.SetNormal();
    }

    public override void Dispose()
    {
        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        _view = null;
        base.Dispose();
    }
}
