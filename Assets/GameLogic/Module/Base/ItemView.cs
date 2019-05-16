using Framework.UI;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ItemView : UIBaseView
{
    private ItemViewType _itemViewType;
    public Image _backImg;
    public Image _itemIcon;
    public Image _itemKind;
    private Image _subscript;
    private GameObject _selectObject;
    private Button _button;
    private Action<ItemView> _OnClickMethod;
    public Text _countText;
    private Text _debugText;
    private GameObject _frameObj;
    private GameObject _tatterObj;
    private bool _blSelected;
    private bool _redSele;
    private RectTransform _iconRect;
    private CardRarityView _rarityView;
    private GameObject _starObj;
    private CardRarityView _rarityViewEquip;
    private GameObject _starEquipObj;

    private ImageGray _gray;
    private ImageGray _grays;

    public ItemDataVO mItemDataVO { get; private set; }
    public GameObject mRedPointObject { get; private set; }
    public ItemView()
    {

    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _backImg = Find<Image>("ImageBack");
        _itemIcon = Find<Image>("ImageBack/ImageIcon");
        _itemKind = Find<Image>("ImageBack/ItemKind");
        _subscript = Find<Image>("ImageBack/Subscript");
        _selectObject = Find("ImageBack/ImageChosen");
        _button = Find<Button>("ImageBack");
        _countText = Find<Text>("ImageBack/CountText");
        _debugText = Find<Text>("ImageBack/DebugText");
        _iconRect = Find<RectTransform>("ImageBack/ImageIcon");
        mRedPointObject = Find("ImageBack/RedDot");
        _frameObj = Find("ImageBack/Frame");
        _tatterObj = Find("ImageBack/Tatter");
        _starObj = Find("ImageBack/StarGroup");
        _starEquipObj = Find("ImageBack/StarGroupEquip");

        _gray = Find<ImageGray>("ImageBack/ImageIcon");
        _grays = Find<ImageGray>("ImageBack");

        _rarityView = new CardRarityView();
        _rarityView.SetDisplayObject(Find("ImageBack/StarGroup"));

        _rarityViewEquip = new CardRarityView();
        _rarityViewEquip.SetDisplayObject(Find("ImageBack/StarGroupEquip"));

        _button.onClick.Add(OnClick);
    }

    public void SetGray()
    {
        _gray.SetGray();
        _grays.SetGray();
    }

    public void SetGrayClip()
    {
        _gray.SetGrayClip();
        _grays.SetGrayClip();
    }

    public void SetNormal()
    {
        _gray.SetNormal();
        _grays.SetNormal();
    }

    public Transform GetBtnTransform()
    {
        return _button.transform;
    }

    public void RefreshCount(int value)
    {
        mItemDataVO.RefreshCount(value);
        _countText.text = value.ToString();
        _debugText.text = "ID:" + mItemDataVO.mItemConfig.ID;
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        mItemDataVO = args[0] as ItemDataVO;
        _itemIcon.sprite = GameResMgr.Instance.LoadItemIcon(mItemDataVO.mItemConfig.Icon);
        ObjectHelper.SetSprite(_itemIcon,_itemIcon.sprite);
        _backImg.sprite = GameResMgr.Instance.LoadItemIcon("itemicon/panel_kind_0" + mItemDataVO.mItemConfig.Quality);
        _itemKind.sprite = GameResMgr.Instance.LoadItemIcon("itemicon/icon_kind_0" + mItemDataVO.mItemConfig.Quality);
        ObjectHelper.SetSprite(_backImg, _backImg.sprite);
        ObjectHelper.SetSprite(_itemKind, _itemKind.sprite);
        _countText.text = UnitChange.GetUnitNum(mItemDataVO.mCount);
        _countText.gameObject.SetActive(mItemDataVO.mCount > 1 && _itemViewType != ItemViewType.ShopItem || mItemDataVO.mCount > 1 && mItemDataVO.mItemConfig.ItemType != 4);
        _debugText.text = "ID:" + mItemDataVO.mItemConfig.ID;
        if (mItemDataVO.mItemConfig.ItemType == 4)
        {
            _tatterObj.SetActive(true);
            _iconRect.sizeDelta = new Vector2(100, 100);
            if (mItemDataVO.mItemConfig.LeftCornerIcon != "")
            {
                _subscript.sprite = GameResMgr.Instance.LoadItemIcon(mItemDataVO.mItemConfig.LeftCornerIcon);
                _subscript.gameObject.SetActive(true);
            }
            else
            {
                _subscript.gameObject.SetActive(false);
            }
        }
        else
        {
            _iconRect.sizeDelta = new Vector2(65, 65);
            _tatterObj.SetActive(false);
            _subscript.gameObject.SetActive(false);
        }
        if (_itemViewType == ItemViewType.EquipItem || _itemViewType == ItemViewType.EquipRewardItem || _itemViewType == ItemViewType.EquipHeroItem)
            _rarityViewEquip.Show(mItemDataVO.mItemConfig.ShowStar);
        else
            _rarityView.Show(mItemDataVO.mItemConfig.ShowStar);
        _starObj.SetActive(mItemDataVO.mItemConfig.ShowStar > 0 && _itemViewType != ItemViewType.EquipItem && _itemViewType != ItemViewType.EquipRewardItem && _itemViewType != ItemViewType.EquipHeroItem);
        _starEquipObj.SetActive(mItemDataVO.mItemConfig.ShowStar > 0 && _itemViewType == ItemViewType.EquipItem || _itemViewType == ItemViewType.EquipRewardItem || _itemViewType == ItemViewType.EquipHeroItem);
    }

    protected virtual void OnClick()
    {
        if (_OnClickMethod == null)
        {
            ItemTipsMgr.Instance.ShowItemTips(mItemDataVO.mItemConfig);
            return;
        }
        _OnClickMethod.Invoke(this);
    }

    public virtual void SetParamters(ItemViewType type, Action<ItemView> OnClickMethod)
    {
        _OnClickMethod = OnClickMethod;
        _itemViewType = type;
        mRectTransform.sizeDelta = Vector2.one * 100f;
        switch (type)
        {
            case ItemViewType.BagItem:
                mRectTransform.localScale = Vector3.one * 1f;
                break;
            case ItemViewType.EquipItem:
                mRectTransform.localScale = Vector3.one * 1f;
                break;
            case ItemViewType.RewardItem:
                mRectTransform.localScale = Vector3.one * 0.8f;
                break;
            case ItemViewType.EquipRewardItem:
                mRectTransform.localScale = Vector3.one * 0.8f;
                break;
            case ItemViewType.HeroItem:
                mRectTransform.localScale = Vector3.one * 0.6f;
                break;
            case ItemViewType.EquipHeroItem:
                mRectTransform.localScale = Vector3.one * 0.6f;
                break;
        }
    }

    public bool BlSelected
    {
        get { return _blSelected; }
        set
        {
            if (_blSelected == value)
                return;
            _blSelected = value;
            _frameObj.SetActive(_blSelected);
        }
    }

    public bool RedSele
    {
        get { return _redSele; }
        set
        {
            if (_redSele == value)
                return;
            _redSele = value;
            mRedPointObject.SetActive(_redSele);
        }
    }

    public override void Hide()
    {
        _OnClickMethod = null;
        _itemViewType = ItemViewType.None;
        mRectTransform.localScale = Vector3.one;
        if (mItemDataVO != null)
        {
            mItemDataVO.Dispose();
            mItemDataVO = null;
        }
        BlSelected = false;
        RedSele = false;
        mRedPointObject.SetActive(false);
        SetNormal();
        base.Hide();
    }

    public override void Dispose()
    {
        _OnClickMethod = null;
        _button = null;
        if (mItemDataVO != null)
        {
            mItemDataVO.Dispose();
            mItemDataVO = null;
        }
        if (_rarityView != null)
            _rarityView.Dispose();
        _rarityView = null;
        BlSelected = false;
        RedSele = false;
        base.Dispose();
    }
}