using Framework.UI;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RoleFusionItem : UIBaseView
{
    private Image _icon;
    private Image _subscript;
    private Text _countText;
    private Button _button;
    private GameObject _seleObj;
    private bool _blSelected;
    private CardRarityView _rarityView;
    public GameObject mRedPointObject { get; private set; }

    public Action<int> mOnClickMethod { get; set; }

    public int mFusionId { get; private set; }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _icon = Find<Image>("Icon");
        _subscript = Find<Image>("Subscript");
        _button = Find<Button>("Button");
        _countText = Find<Text>("Count");
        _seleObj = Find("Sele");
        mRedPointObject = Find("RedDot");

        _rarityView = new CardRarityView();
        _rarityView.SetDisplayObject(Find("StarGroup"));

        _button.onClick.Add(OnSelect);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        mFusionId = int.Parse(args[0].ToString());

        FusionConfig config = GameConfigMgr.Instance.GetFusionConfig(mFusionId);
        _rarityView.Show(config.StarShow);
        _icon.sprite = GameResMgr.Instance.LoadCardIcon(config.Icon);
        ObjectHelper.SetSprite(_icon,_icon.sprite);
        _subscript.sprite = GameResMgr.Instance.LoadItemIcon(config.LeftCornerIcon);
        ObjectHelper.SetSprite(_subscript,_subscript.sprite);
    }

    private void OnSelect()
    {
        if (mOnClickMethod != null)
            mOnClickMethod.Invoke(mFusionId);
    }

    public override void Hide()
    {
        base.Hide();
        _blSelected = false;
        _seleObj.SetActive(false);
    }

    public bool BlSelected
    {
        get { return _blSelected; }
        set
        {
            if (_blSelected == value)
                return;
            _blSelected = value;
            _seleObj.SetActive(_blSelected);
        }
    }

    public override void Dispose()
    {
        mOnClickMethod = null;
        if (_rarityView != null)
            _rarityView.Dispose();
        _rarityView = null;
        base.Dispose();
    }

  
}