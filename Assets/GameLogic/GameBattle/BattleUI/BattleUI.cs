using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class HpBarView : UIBaseView
{
    private Image _typeIcon;
    private Image _bloodSlider;
    private Image _energySlider;
    private Image _bloodSliderBack;
    private GameObject _shieldObject;
    private Image _shieldSlider;
    private UIEffectView _effect;

    private FighterDataVO _fighterData;
    private Text _debugLabel;
    private Transform _buffGrid;
    private Image _buffItem;
    private float _flHeight;
    public HpBarView(FighterDataVO fighterData)
    {
        _fighterData = fighterData;
        _flHeight = _fighterData.mflModelHeight;
    }

    public HpBarView()
    {

    }

    public void ShowUIHpbar(int hpPercent, int level, int cardType)
    {
        float flHpPer = hpPercent / 100f;
        flHpPer = flHpPer > 1.01f ? 1.0f : flHpPer;
        if (flHpPer > 0f)
            flHpPer = 0.05f + 0.95f * flHpPer;
        _bloodSlider.fillAmount = flHpPer;
        _bloodSliderBack.fillAmount = flHpPer;
        if (_energySlider.gameObject.activeInHierarchy)
            _energySlider.gameObject.SetActive(false);
        if (_shieldObject.activeInHierarchy)
            _shieldObject.SetActive(false);
        _typeIcon.sprite = GameResMgr.Instance.LoadItemIcon(GameConst.CARD_TYPE_ICONS[cardType - 1]);
        ObjectHelper.SetSprite(_typeIcon, _typeIcon.sprite);
        _debugLabel.text = level.ToString();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _debugLabel.text = _fighterData.mLevel.ToString();
        _typeIcon.sprite = GameResMgr.Instance.LoadItemIcon(GameConst.CARD_TYPE_ICONS[_fighterData.mCardType - 1]);
        ObjectHelper.SetSprite(_typeIcon, _typeIcon.sprite);
        RefreshHpAndEnergy(_fighterData.mCurHp, _fighterData.mEnergy, _fighterData.mCurShield, false);
    }

    public void RefreshHpAndEnergy(int curHp, int curEnergy, int shield, bool blTween = true)
    {
        float flHpPer = (float)curHp / (float)_fighterData.mMaxHp;
        flHpPer = flHpPer > 1.01f ? 1.0f : flHpPer;
        if (flHpPer > 0f)
            flHpPer = 0.05f + 0.95f * flHpPer;
        _bloodSlider.fillAmount = flHpPer;
        if (blTween)
            DGHelper.DoImageFillAmount(_bloodSliderBack, flHpPer, 0.5f);
        else
            _bloodSliderBack.fillAmount = flHpPer;

        float flEnergyPer = (float)curEnergy / GameConst.ENERGY_MAX;
        //flEnergyPer = flEnergyPer > 1.01f ? 1.0f : flEnergyPer;

        if (flEnergyPer >= 1f)
        {
            flEnergyPer = 1.0f;
            _effect.PlayEffect();
        }
        else
        {

            _effect.StopEffect();
        }
        if (flEnergyPer > 0f)
            flEnergyPer = 0.05f + 0.95f * flEnergyPer;
        if (blTween)
            DGHelper.DoImageFillAmount(_energySlider, flEnergyPer, 0.1f);

        else
            _energySlider.fillAmount = flEnergyPer;

        if (shield > 0)
        {
            if (!_shieldObject.activeInHierarchy)
                _shieldObject.SetActive(true);
            float flShieldPer = (float)shield / (float)_fighterData.mShieldMax;
            if (flShieldPer > 0.01f)
                flShieldPer = 0.05f + 0.95f * flShieldPer;
            _shieldSlider.fillAmount = flShieldPer;
        }
        else
        {
            if (_shieldObject.activeInHierarchy)
                _shieldObject.SetActive(false);
        }
    }

    public void UpdatePosition(Vector3 pos)
    {
        pos = new Vector3(pos.x, pos.y + _flHeight, pos.z); //  + 0.1f      
        mRectTransform.anchoredPosition = GameUIMgr.Instance.WorldToUIPoint(pos);
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();

        _buffGrid = Find<Transform>("BuffGrid");
        _buffItem = Find<Image>("BuffItem");

        _typeIcon = Find<Image>("Icon");
        _bloodSlider = Find<Image>("HpBar/bloodSlider");
        _energySlider = Find<Image>("HpBar/energySlider");
        _bloodSliderBack = Find<Image>("HpBar/bloodSliderBack");
        _shieldObject = Find("HpBar/ShieldObject");
        _shieldSlider = Find<Image>("HpBar/ShieldObject/ShieldSlider");
        _effect = CreateUIEffect(Find("HpBar/energySlider/fx_skill_nvqizhi"), 1);

        _shieldObject.SetActive(false);
        _debugLabel = Find<Text>("Text");
    }

    public void OnCreateBuff(StatusConfig cfg, int value)
    {
        if (_buffGrid.Find(cfg.Icon) != null)
        {
            _buffGrid.Find(cfg.Icon).transform.Find("Text").GetComponent<Text>().text = value.ToString();
        }
        else
        {
            _buffItem.sprite = GameResMgr.Instance.LoadBuffIcon(cfg.Icon);
            GameObject buffItem = GameObject.Instantiate(_buffItem.gameObject);
            buffItem.name = cfg.Icon;
            buffItem.transform.SetParent(_buffGrid, false);
            if (_buffGrid.childCount <= 4)
            {
                buffItem.SetActive(true);
            }
            else
            {
                buffItem.SetActive(false);
            }
        }
    }

    public void OnRemoveBuff(StatusConfig cfg, int value)
    {
        if (value > 1)
        {
            _buffGrid.Find(cfg.Icon).transform.Find("Text").GetComponent<Text>().text = value.ToString();
        }
        else
        {
            GameObject.Destroy(_buffGrid.Find(cfg.Icon).gameObject);
        }
    }

    protected override void DisposeGameObject()
    {
        if (mDisplayObject != null)
            GameObject.Destroy(mDisplayObject);
        _fighterData = null;
        _typeIcon = null;
        _bloodSlider = null;
        _energySlider = null;
        _bloodSliderBack = null;
    }
}

public class BloodView : UIBaseView
{
    private Text _redText;
    private Text _yellowText;
    private Text _greenText;
    private Text _whiteText;

    public bool mBlMoveEnd { get; private set; }
    private Fighter _fighter;
    public int mCurDamage { get; private set; }
    public int mCurHp { get; private set; }
    public int mCurEnergy { get; private set; }
    public int mCurShield { get; private set; }
    public string mSkillHitEffect { get; private set; }
    public string mSkillHitSound { get; private set; }
    public bool mBlInjury { get; private set; }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _redText = Find<Text>("TextRed");
        _yellowText = Find<Text>("TextYellow");
        _greenText = Find<Text>("TextGreen");
        _whiteText = Find<Text>("TextWhite");
    }

    private Text _curText;

    private int _side;
    private int _seat;
    private int _total;
    private int _index;

    public void InitData(Fighter fighter, FighterDamageDataVO vo)
    {
        _side = fighter.mData.mSide;
        _seat = fighter.mData.mSeatIndex;
        _index = vo.Index;
        _total = vo.Total;

        mBlInjury = vo.mBlInjury;
        mCurDamage = vo.CurDamage;

        _fighter = fighter;
        float value = vo.CurDamage;
        mSkillHitEffect = vo.mSkillHitEffect;
        mSkillHitSound = vo.mSkillHitSound;
        _greenText.gameObject.SetActive(false);
        _redText.gameObject.SetActive(false);
        _whiteText.gameObject.SetActive(false);
        _yellowText.gameObject.SetActive(false);

        if (vo.mAntiType == 1)
            _curText = _yellowText;
        else if (vo.mAntiType == 0)
            _curText = _redText;
        else
            _curText = _whiteText;
        if (vo.CurDamage < 0)
            _curText = _greenText;
        if (vo.mBlAbsorb)
            _curText.text = LanguageMgr.GetLanguage(220203);//"吸收";
        else if (vo.mBlCritical)
            _curText.text = LanguageMgr.GetLanguage(220201) + " " + (-vo.CurDamage);// "暴击 "
        else if (vo.mBlBlock)
            _curText.text = LanguageMgr.GetLanguage(220202) + "\n" + (-vo.CurDamage);//格挡
        else
            _curText.text = "" + (-vo.CurDamage);

        mRectTransform.localScale = Vector3.one * 0.5f;
        mBlMoveEnd = false;
        mCurEnergy = vo.mEnergy;
        mCurShield = vo.mShield;
        vo.DoNextIndex();
        if (vo.mHasMultiDamage)
        {
            if ((fighter.mData.mCurHp - mCurDamage) < 0)
                mCurHp = 1;//vo.mCurHp;
            else
                mCurHp = fighter.mData.mCurHp - mCurDamage;
        }
        else
        {
            if (!vo.BlDamageEnd)
            {
                if ((fighter.mData.mCurHp - mCurDamage) < 0)
                    mCurHp = 1;//vo.mCurHp;
                else
                    mCurHp = fighter.mData.mCurHp - mCurDamage;
            }
            else
            {
                mCurHp = vo.mCurHp;
            }
        }
        //if (!vo.BlDamageEnd && vo.mHasMultiDamage)
        //{
        //    if ((fighter.mData.mCurHp - mCurDamage) < 0)
        //        mCurHp = 1;//vo.mCurHp;
        //    else
        //        mCurHp = fighter.mData.mCurHp - mCurDamage;
        //}
        //else
        //{
        //    mCurHp = vo.mCurHp;
        //}
        _curText.gameObject.SetActive(false);
    }

    private bool blHeroFighter;
    public void Play(Vector3 pos)
    {
        Show();
        _curText.gameObject.SetActive(true);

        DGHelper.DoKill(mRectTransform);
        DGHelper.DoKill(_curText);

        blHeroFighter = BattleDataModel.Instance.IsHeroFighter(_fighter.mData.mSide);
        float x = blHeroFighter ? -10f : 10f;
        Vector2 targetAchPos = new Vector2(pos.x + x, pos.y + 50f);

        DGHelper.DoAnchorPos(mRectTransform, targetAchPos, 0.3f, DGEaseType.InBack, OnScaleEnd);
        DGHelper.DoScale(mRectTransform, 1.3f, 0.2f);
        mRectTransform.anchoredPosition = pos;
    }

    private void OnScaleEnd()
    {
        float x = blHeroFighter ? -30f : 30f;
        Vector2 targetAchPos = new Vector2(mRectTransform.anchoredPosition.x + x, mRectTransform.anchoredPosition.y - 20f);

        DGHelper.DoAnchorPos(mRectTransform, targetAchPos, 0.3f);
        DGHelper.DoTextFade(_curText, 0f, 0.4f, 0, DoMoveEnd);
    }

    private void DoMoveEnd()
    {
        mBlMoveEnd = true;
        _fighter = null;
        DGHelper.DoKill(mRectTransform);
        DGHelper.DoKill(_curText);
    }

    public override void Hide()
    {
        base.Hide();
        DGHelper.DoKill(mRectTransform);
        DGHelper.DoKill(_curText);
    }

    protected override void DisposeGameObject()
    {
        DGHelper.DoKill(mRectTransform);
        DGHelper.DoKill(_curText);
        if (mDisplayObject != null)
            GameObject.Destroy(mDisplayObject);
        _fighter = null;
    }
}