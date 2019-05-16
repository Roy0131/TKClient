using Framework.UI;
using Msg.ClientMessage;
using UnityEngine;

public class SignItemView : UIBaseView
{
    private RectTransform _parent;
    private GameObject _sele;
    private GameObject _speciallyObj;
    private UIEffectView _effect;
    private ItemView _view;


    protected override void ParseComponent()
    {
        base.ParseComponent();
        _parent = Find<RectTransform>("Bj");
        _sele = Find("Sele");

        _speciallyObj = Find("Bj/fx_ui_qiandao");
        _effect = CreateUIEffect(_speciallyObj, UILayerSort.WindowSortBeginner);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        int minIndex = int.Parse(args[1].ToString());
        int maxIndex = int.Parse(args[2].ToString());
        SignConfig cfg = args[0] as SignConfig;
        ItemInfo itemInfo;
        string[] rewards = cfg.Reward.Split(',');
        if (rewards.Length % 2 != 0)
            return;
        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        for (int i = 0; i < rewards.Length; i += 2)
        {
            itemInfo = new ItemInfo();
            itemInfo.Id = int.Parse(rewards[i]);
            itemInfo.Value = int.Parse(rewards[i + 1]);

            if (GameConfigMgr.Instance.GetItemConfig(itemInfo.Id).ItemType == 2)
                _view = ItemFactory.Instance.CreateItemView(itemInfo, ItemViewType.EquipItem);
            else
                _view = ItemFactory.Instance.CreateItemView(itemInfo, ItemViewType.BagItem);
            _view.mRectTransform.SetParent(_parent, false);
            if (cfg.TotalIndex <= minIndex)
                _view.SetGrayClip();
            else
                _view.SetNormal();
        }
        _sele.SetActive(cfg.TotalIndex <= minIndex);
        if (cfg.TotalIndex > minIndex && cfg.TotalIndex <= maxIndex)
            _effect.PlayEffect();
        else
            _effect.StopEffect();
    }

    public override void Dispose()
    {
        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        _view = null;
        base.Dispose();
    }
}
