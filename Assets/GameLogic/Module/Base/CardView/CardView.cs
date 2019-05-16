using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;
using System;

public class CardView : UIBaseView
{
    protected Action<CardView> _onClickMethod;

    public CardDataVO mCardDataVO { get; private set; }
    protected CardConfig _cardConfig;
    protected Image _cardIcon;
    protected Image _cardTypeImage;
    protected Text _level;
    protected Text _Num;
    protected GameObject _lockObject;
    protected GameObject _selectObject;
    protected Image _cardKindIcon;
    protected Image _cardMask;

    protected GameObject _starObject;
    protected List<GameObject> _lstAllStars;
    protected Button _button;

    private CardViewType _type;

    private CardRarityView _rarityView;

    private UIEffectView _uiEffect;
    private GameObject _lockStatusMask;

    private Vector2 _defSizeDelta;

    //远征界面专用
    private Text _seleName;
    private Image _hpImg;
    private GameObject _expeditionSele;
    private GameObject _expeditionHp;
    protected override void ParseComponent()
    {
        base.ParseComponent();
        _cardIcon = Find<Image>("ImageHeadMask/ImageHead");
        _level = Find<Text>("Textlevel");
        _Num = Find<Text>("CardNum");
        _cardTypeImage = Find<Image>("ImageCamp");
        _starObject = Find("StarObj");
        _lstAllStars = new List<GameObject>();
        _button = Find<Button>("Collider");
        _lockObject = Find("StatusRoot/LockStatus");
        _selectObject = Find("StatusRoot/SelectStatus");
        _cardKindIcon = Find<Image>("ImageBack");
        _cardMask = Find<Image>("ImageHeadMask");
        _lockStatusMask = Find("LockStatusMask");

        //远征界面专用
        _seleName = Find<Text>("ExpeditionSele/Text");
        _hpImg = Find<Image>("HpBar/BloodSlider");
        _expeditionSele = Find("ExpeditionSele");
        _expeditionHp = Find("HpBar");
        _isExpeditionSele = false;

        _rarityView = new CardRarityView();
        _rarityView.SetDisplayObject(Find("Stars/uiStarGroup"));

        _uiEffect = CreateUIEffect(Find("fx_ui_wuxing"), UILayerSort.WindowSortBeginner + 4);

        _button.onClick.Add(OnClick);
        BlSelected = false;
        IsSele = false;
        _islockStatusMask = false;
        _defSizeDelta = mRectTransform.sizeDelta;
    }

    public Transform GetBtnTransform()
    {
        return _button.transform;
    }

    private bool _blSelected;
    public bool BlSelected
    {
        get { return _blSelected; }
        set
        {
            if (_blSelected == value)
                return;
            _blSelected = value;
            _selectObject.SetActive(_blSelected);
        }
    }

    private bool _isSele;
    public bool IsSele
    {
        get { return _isSele; }
        set
        {
            if (_isSele == value)
                return;
            _isSele = value;
            if (_isSele)
                _uiEffect.PlayEffect();
            else
                _uiEffect.StopEffect();
        }
    }

    private bool _islockStatusMask;
    public bool LockStatusMask
    {
        get { return _islockStatusMask; }
        set
        {
            if (_islockStatusMask == value)
                return;
            _islockStatusMask = value;
            _lockStatusMask.SetActive(_islockStatusMask);
        }
    }

    private void OnClick()
    {
        switch(_type)
        {
            case CardViewType.Decompose:
            case CardViewType.FusionMat:
            case CardViewType.HeroCall:
                if (mCardDataVO.mBlLock || mCardDataVO.mBlInBattle || mCardDataVO.mState != 0)
                {
                    if (mCardDataVO.mState == 2)
                        PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000101));
                    else if(mCardDataVO.mBlInBattle)
                        PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000102));
                    else if(mCardDataVO.mBlLock)
                        PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000103));
                    return;
                }
                BlSelected = !BlSelected;
                break;
        }
        if (_onClickMethod != null)
            _onClickMethod.Invoke(this);
    }

    //远征界面专用
    private bool _isExpeditionSele;
    public bool IsExpeditionSele
    {
        get { return _isExpeditionSele; }
        set
        {
            if (_isExpeditionSele == value)
                return;
            _expeditionSele.SetActive(value);
        }
    }
    public void OnExpeditionSeleName(string name)
    {
        _seleName.text = name;
    }
    public void OnExpeditionHp(int amount)
    {
        _expeditionHp.SetActive(true);
        _hpImg.fillAmount = (float)amount / 100;
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        mCardDataVO = args[0] as CardDataVO;
        RefreshView(mCardDataVO.mCardConfig);
        _level.text = mCardDataVO.mCardLevel.ToString();
        _selectObject.SetActive(false);
        _lockObject.SetActive(false);
        switch (_type)
        {
            case CardViewType.FusionMat:
            case CardViewType.Decompose:
            case CardViewType.HeroCall:
                _lockObject.SetActive(mCardDataVO.mBlInBattle || mCardDataVO.mBlLock || mCardDataVO.mState != 0);
                break;
            //case CardViewType.Common:
            //    mRectTransform.localScale = Vector3.one * 1.3f;
            //    break;
        }
        _Num.gameObject.SetActive(mCardDataVO.mCardCount > 1);
        _Num.text = "x" + mCardDataVO.mCardCount;
        int[] camp = { 1, 2, 4, 5, 6, 3 };
        int curCamp = camp[mCardDataVO.mCardConfig.Camp - 1];
        _cardKindIcon.sprite = GameResMgr.Instance.LoadItemIcon("itemicon/icon_kind_0" + curCamp);
        _cardMask.sprite = GameResMgr.Instance.LoadItemIcon("itemicon/panel_kind_0" + curCamp);

        ObjectHelper.SetSprite(_cardKindIcon,_cardKindIcon.sprite);
        ObjectHelper.SetSprite(_cardMask,_cardMask.sprite);
    }

    private void RefreshView(CardConfig cardConfig)
    {
        if (_cardConfig != null && _cardConfig.ClientID == cardConfig.ClientID)
            return;
        _cardConfig = cardConfig;
        _cardTypeImage.sprite = GameResMgr.Instance.LoadItemIcon(GameConst.CARD_TYPE_ICONS[_cardConfig.Type - 1]);
        ObjectHelper.SetSprite(_cardTypeImage,_cardTypeImage.sprite);
        _cardIcon.sprite = GameResMgr.Instance.LoadCardIcon(_cardConfig.Icon);
        ObjectHelper.SetSprite(_cardIcon, _cardIcon.sprite);
        _rarityView.Show(_cardConfig.Rarity);
    }

    public override void Dispose()
    {
        if (_rarityView != null)
            _rarityView.Dispose();
        _rarityView = null;
        _onClickMethod = null;
        _cardConfig = null;
		base.Dispose();
    }

    public void SetParamters(CardViewType type, Action<CardView> onClickMethod)
    {
        _onClickMethod = onClickMethod;
        _button.enabled = onClickMethod != null;
        _type = type;
    }

    public void ReturnCardView()
    {
        _onClickMethod = null;
        _cardConfig = null;
        mCardDataVO = null;
        BlSelected = false;
        IsSele = false;
        _type = CardViewType.None;
        mRectTransform.sizeDelta = _defSizeDelta;
        //远征界面专用
        _expeditionHp.SetActive(false);
        _expeditionSele.SetActive(false);
        //mRectTransform.localScale = Vector3.one;
        Hide();
    }
}