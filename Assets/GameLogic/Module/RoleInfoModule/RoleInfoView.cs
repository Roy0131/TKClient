using Framework.UI;
using Msg.ClientMessage;
using NewBieGuide;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleInfoView : UIBaseView
{
    enum Status
    {
        None,
        LevelUp,
        RankUp,
        Max,
    }

    private Text _campText;
    private Text _battlePowerText;
    private Text _levelText;
    private Text _hpText;
    private Text _attackText;
    private Text _defenseText;
    private Image _campIcon;
    private GameObject[] _iconTypes;
    private Button[] _iconBtns;
    private GameObject _inconImg;
    private Text _iconImgText1;
    private Text _iconImgText2;
    private Button _iconImgBtn;

    private CardDataVO _curCardData;

    private RankGroupView _rankGroupView;
    private ResCostGroup _resCostGroup;
    private SkillView _skillView;

    private GameObject _levelUpObject;
    private Button _infoBtn;
    private Text _infoBtnText;
    private Status _status;

    private Text _desText;

    private Button _attrBtn;
    private Button _attrCloseBtn;
    private GameObject _attrObject;
    private GameObject _skillTipsObject;

    private Dictionary<int, Text> _dictAttrNameTxt;
    private Dictionary<int, Text> _dictAttrValueTxt;

    private UIEffectView _uiEffect;
    private UIEffectView _effect02;

    private GameObject _upGradeItemObj;
    private RectTransform _parent;
    private GameObject _panel;

    protected override void ParseComponent()
	{
        base.ParseComponent();
        _campText = Find<Text>("IconObj/TextCamp");
        _battlePowerText = Find<Text>("TextObject/TextBattle");
        _levelText = Find<Text>("TextObject/TextLevel");
        _hpText = Find<Text>("TextObject/HpText");
        _attackText = Find<Text>("TextObject/AttackText");
        _defenseText = Find<Text>("TextObject/DefenseText");
        _inconImg = Find("IconImg");
        _iconImgText1 = Find<Text>("IconImg/IconImg/Text1");
        _iconImgText2 = Find<Text>("IconImg/IconImg/Text2");
        _iconImgBtn = Find<Button>("IconImg/Panel");

        _iconTypes = new GameObject[4];
        _iconBtns = new Button[4];
        for (int i = 0; i < 4; i++)
        {
            int index = i;
            _iconTypes[i] = Find("IconObj/ImageType" + (i + 1));
            _iconBtns[i] = Find<Button>("IconObj/ImageType" + (i + 1));
            _iconBtns[i].onClick.Add(() => { OnIconBtn(index + 1); });
        }

        _campIcon = Find<Image>("ImageCamp");

        _levelUpObject = Find("LevelUpObject");
        _infoBtn = Find<Button>("LevelUpObject/Button");
        _infoBtnText = Find<Text>("LevelUpObject/Button/text");
        _desText = Find<Text>("LevelUpObject/TipsText");

        _skillView = new SkillView();
        _skillView.SetDisplayObject(Find("SkillGroup"));

        _rankGroupView = new RankGroupView();
        _rankGroupView.SetDisplayObject(Find("RankGrid"));

        _resCostGroup = new ResCostGroup();
        _resCostGroup.SetDisplayObject(Find("LevelUpObject/CostRoot/uiCostGroup"));

        _attrBtn = Find<Button>("AttrBtn/BtnHelp");
        _attrCloseBtn = Find<Button>("Info/BackGround");
        _attrObject = Find("Info");
        _skillTipsObject = Find("SkillGroup/SkillTips");

        MouseEventListener.Get(_infoBtn.gameObject).mMouseDown = OnMouseDown;
        MouseEventListener.Get(_infoBtn.gameObject).mMouseUp = OnMouseUp;
        MouseEventListener.Get(_infoBtn.gameObject).mMouseClick = OnClick;

        _attrBtn.onClick.Add(OnShowAttrInfo);
        ColliderHelper.SetButtonCollider(_attrBtn.transform);

        _attrCloseBtn.onClick.Add(OnCloseAttrInfo);

        _dictAttrNameTxt = new Dictionary<int, Text>();
        _dictAttrValueTxt = new Dictionary<int, Text>();
        List<int> attr = GameConst.AttrListShow;
        Text nameTxt, valueTxt;
        GameObject itemObject = Find("Info/AttrGroup/AttrGrid/AttrItem");
        RectTransform root = Find<RectTransform>("Info/AttrGroup/AttrGrid");
        for (int i = 0; i < attr.Count; i++)
        {
            GameObject obj = GameObject.Instantiate(itemObject);
            obj.transform.SetParent(root, false);
            nameTxt = obj.transform.Find("AttrNameText").GetComponent<Text>();
            valueTxt = obj.transform.Find("AttrValueText").GetComponent<Text>();
            _dictAttrNameTxt.Add(attr[i], nameTxt);
            _dictAttrValueTxt.Add(attr[i], valueTxt);
            obj.SetActive(true);
        }

        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.RoleLevelUp, _infoBtn.transform);
        _uiEffect = CreateUIEffect(Find("LevelUpObject/fx_ui_shengji"), UILayerSort.WindowSortBeginner+1);
	    _effect02 = CreateUIEffect(Find("LevelUpObject/fx_ui_juexing"),UILayerSort.WindowSortBeginner+1);

        _parent = Find<RectTransform>("LevelUpObject/UpGradeObj");
        _upGradeItemObj = Find("LevelUpObject/UpGradeObj/UpGrade");
        _iconImgBtn.onClick.Add(OnIconImgBtn);
        _panel = Find("Panel");
        //_uiEffect.StopEffect();
    }

    private void OnIconBtn(int index)
    {
        _inconImg.SetActive(true);
        LabelConfig cfg = GameConfigMgr.Instance.GetLabelConfig(index);
        if (cfg.NameID > 0)
        {
            _iconImgText1.gameObject.SetActive(true);
            _iconImgText1.text = LanguageMgr.GetLanguage(cfg.NameID);
        }
        if (cfg.ExtaDescription > 0)
        {
            _iconImgText2.gameObject.SetActive(true);
            _iconImgText2.text = LanguageMgr.GetLanguage(cfg.ExtaDescription);
        }
    }

    private void OnIconImgBtn()
    {
        _inconImg.SetActive(false);
        _iconImgText1.gameObject.SetActive(false);
        _iconImgText2.gameObject.SetActive(false);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<bool>(UIEventDefines.SkillType, OnSkillType);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<UpGradeItemView>(HeroEvent.CardLevelUp, OnCardUp);
        HeroDataModel.Instance.AddEvent<int>(HeroEvent.Advanced, OnAdvanced);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<int, int, List<ItemInfo>>(HeroEvent.Surmount, OnSurmount);
        HeroDataModel.Instance.AddEvent(HeroEvent.RoleLevelUp, OnRefreshs);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<bool>(UIEventDefines.SkillType, OnSkillType);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<UpGradeItemView>(HeroEvent.CardLevelUp, OnCardUp);
        HeroDataModel.Instance.RemoveEvent<int>(HeroEvent.Advanced, OnAdvanced);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<int, int, List<ItemInfo>>(HeroEvent.Surmount, OnSurmount);
        HeroDataModel.Instance.RemoveEvent(HeroEvent.RoleLevelUp, OnRefreshs);
    }

    private void OnRefreshs()
    {
        _uiEffect.StopEffect();
        SoundMgr.Instance.PlayEffectSound("UI_Hero_up");
        _uiEffect.PlayEffect();
        UpGradeItemView view;
        view = GetRewardItem();
        view.mRectTransform.SetParent(_parent, false);
        view.mRectTransform.SetAsLastSibling();
        view.Show(_curCardData);
        _lstCurShowItems.Add(view);
    }

    private void OnCardUp(UpGradeItemView upGarde)
    {
        ReturnRewardItem(upGarde);
    }

    private void OnSkillType(bool skillType)
    {
        _skillTipsObject.SetActive(!skillType);
    }

    private void OnAdvanced(int value)
    {
        _panel.SetActive(true);
        _effect02.PlayEffect();
        DelayCall(2f, () => _panel.SetActive(false));
        DelayCall(2.3f,()=>_effect02.StopEffect());
    }

    private void OnSurmount(int newCardId, int fusionId, List<ItemInfo> listInfo)
    {
        _panel.SetActive(true);
        _effect02.PlayEffect();
        DelayCall(2f, () => _panel.SetActive(false));
        DelayCall(2.3f, () => _effect02.StopEffect());
    }

    private void OnShowAttrInfo()
    {
        _attrObject.SetActive(true);
        int id;
        AttributeConfig config;
        int value;
        float flValue;
        for (int i = 0; i < GameConst.AttrListShow.Count; i++)
        {
            id = GameConst.AttrListShow[i];
            config = GameConfigMgr.Instance.GetAttrConfig(id);
            _dictAttrNameTxt[id].text = LanguageMgr.GetLanguage(config.NameID);
            value = _curCardData.GetAttriByType(id) + config.UIBaseValue;
            flValue = value;
            if (config.PercentShow == 0)
            {
                config.Divisor = config.Divisor == 0 ? 1 : config.Divisor;
                _dictAttrValueTxt[id].text = (value / config.Divisor).ToString();
            }
            else
            {
                if (config.Divisor != 0)
                    flValue = (float)value / (float)config.Divisor;
                _dictAttrValueTxt[id].text = flValue.ToString("F1") + "%";
            }
        }
    }

    private void OnCloseAttrInfo()
    {
        _attrObject.SetActive(false);
    }


    private uint _key = 0;
    private void OnMouseDown(GameObject go)
    {
        if(_status == Status.LevelUp)
        {
            //Debuger.LogWarning("mouse down...");
            if (_key != 0)
                TimerHeap.DelTimer(_key);
            _key = TimerHeap.AddTimer(200, 200, DoSend);
        }
    }

    private void DoSend()
    {
        if (!_resCostGroup.BlEnough)
        {
            //LogHelper.Log("资源不足！！！");
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001185));
            return;
        }
        GameNetMgr.Instance.mGameServer.ReqRoleLevelup(_curCardData.mCardID);
    }

    #region UpGrade
    private Queue<UpGradeItemView> _lstUpGradeItemPools = new Queue<UpGradeItemView>();
    private List<UpGradeItemView> _lstCurShowItems = new List<UpGradeItemView>();
    public UpGradeItemView GetRewardItem()
    {
        if (_lstUpGradeItemPools.Count > 0)
            return _lstUpGradeItemPools.Dequeue();
        UpGradeItemView view = new UpGradeItemView();
        view.SetDisplayObject(Object.Instantiate(_upGradeItemObj));
        return view;
    }

    private void ReturnRewardItem(UpGradeItemView item)
    {
        item.Hide();
        //ObjectHelper.AddChildToParent(item.mTransform, mTransform);
        _lstUpGradeItemPools.Enqueue(item);
    }

    private void ClearShowRewardItem()
    {
        for (int i = 0; i < _lstCurShowItems.Count; i++)
            ReturnRewardItem(_lstCurShowItems[i]);
        _lstCurShowItems.Clear();
    }

    #endregion

    private void OnMouseUp(GameObject go)
    {
        if (_key != 0)
            TimerHeap.DelTimer(_key);
        _key = 0;
    }

    private void OnClick(GameObject go = null)
    {
        if (_status == Status.LevelUp)
            DoSend();
        else
            GameUIMgr.Instance.OpenModule(ModuleID.RoleRankup, _curCardData);
    }

    private void FillCardInfoData()
    {
        for (int i = 0; i < _iconTypes.Length; i++)
            _iconTypes[i].SetActive(false);
        string[] label = _curCardData.mCardConfig.Label.Split(',');
        for (int i = 0; i < label.Length; i++)
            _iconTypes[int.Parse(label[i]) - 1].SetActive(true);

        _campText.text = LanguageMgr.GetLanguage(220100 + _curCardData.mCardConfig.Type);
        _levelText.text = "Lv:" + _curCardData.mCardLevel + "/" + _curCardData.mCardConfig.MaxLevel;
        _battlePowerText.text = _curCardData.mBattlePower.ToString();
        _hpText.text = _curCardData.GetAttriByType(AttributesType.HP).ToString();
        _attackText.text = _curCardData.GetAttriByType(AttributesType.ATTACK).ToString();
        _defenseText.text = _curCardData.GetAttriByType(AttributesType.DEFENSE).ToString();

        _campIcon.sprite = GameResMgr.Instance.LoadTypeIcon(_curCardData.mCardConfig.Type);
        ObjectHelper.SetSprite(_campIcon,_campIcon.sprite);
        _rankGroupView.Show(_curCardData);

        _skillView.Show(_curCardData.mCardConfig.ShowSkillID, _curCardData.mCardConfig.Rank);

        SkillDataVO.OnSkillType(false);

        if (_curCardData.BlEntityCard)
        {
            if (!_levelUpObject.activeSelf)
                _levelUpObject.SetActive(true);
            if (_curCardData.mCardLevel < _curCardData.mCardConfig.MaxLevel)
                _status = Status.LevelUp;
            else
                _status = _curCardData.mCardRank >= _curCardData.mCardConfig.MaxRank ? Status.Max : Status.RankUp;
        }
        else
        {
            if (_levelUpObject.activeSelf)
                _levelUpObject.SetActive(false);
            _status = Status.None;
        }
        if (_status != Status.None)
        {
            _infoBtn.gameObject.SetActive(true);
            if (_status == Status.LevelUp)
            {
                _infoBtnText.text = LanguageMgr.GetLanguage(5002728);
                _desText.text = "";
                LevelUpConfig config = GameConfigMgr.Instance.GetLevelUpConfig(_curCardData.mCardLevel);
                _resCostGroup.Show(config.CardLevelUpRes);
            }
            else
            {
                _resCostGroup.Hide();
                if (_key != 0)
                    TimerHeap.DelTimer(_key);
                _key = 0;
                if (_status == Status.RankUp)
                {
                    _infoBtnText.text = LanguageMgr.GetLanguage(5002712);
                    _desText.text =LanguageMgr.GetLanguage(5002729);
                }
                else
                {
                    _desText.text = LanguageMgr.GetLanguage(5002730);
                    _infoBtn.gameObject.SetActive(false);
                }
            }
        }
    }

	protected override void Refresh(params object[] args)
	{
        base.Refresh(args);
        _inconImg.SetActive(false);
        _iconImgText1.gameObject.SetActive(false);
        _iconImgText2.gameObject.SetActive(false);
        if (_lstCurShowItems != null)
            ClearShowRewardItem();
        CardDataVO vo = args[0] as CardDataVO;
        if (args.Length > 1)
            _attrBtn.gameObject.SetActive(int.Parse(args[1].ToString()) != CardDetailConst.ChatDetail);
        _curCardData = vo;
        FillCardInfoData();
    }

    public override void Hide()
    {
        base.Hide();
        _uiEffect.StopEffect();
        _status = Status.None;
    }

    public override void Dispose()
	{
        if(_rankGroupView != null)
        {
            _rankGroupView.Dispose();
            _rankGroupView = null;
        }

        if(_skillView != null)
        {
            _skillView.Dispose();
            _skillView = null;
        }

        if (_resCostGroup != null)
        {
            _resCostGroup.Dispose();
            _resCostGroup = null;
        }
        if (_lstCurShowItems != null)
        {
            ClearShowRewardItem();
            _lstCurShowItems = null;
        }
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.RoleLevelUp);
        base.Dispose();
	}
}

public class UpGradeItemView : UIBaseView
{
    private Text _text1;
    private Text _text2;
    private Text _text3;
    private RectTransform _rectText;
    private CardDataVO _cardData;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _text1 = Find<Text>("Text1");
        _text2 = Find<Text>("Text2");
        _text3 = Find<Text>("Text3");
        _rectText = FindOnSelf<RectTransform>();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _cardData = args[0] as CardDataVO;
        _rectText.anchoredPosition = new Vector2(-280f, 0f);
        _text1.text = LanguageMgr.GetLanguage(6001186) + ((_cardData.mCardLevel * _cardData.mCardConfig.GrowthHP - (_cardData.mCardLevel - 1) * _cardData.mCardConfig.GrowthHP) / 100);
        _text2.text = LanguageMgr.GetLanguage(6001187) + ((_cardData.mCardLevel * _cardData.mCardConfig.GrowthAttack - (_cardData.mCardLevel - 1) * _cardData.mCardConfig.GrowthAttack) / 100);
        _text3.text = LanguageMgr.GetLanguage(6001188) + ((_cardData.mCardLevel * _cardData.mCardConfig.GrowthDefence - (_cardData.mCardLevel - 1) * _cardData.mCardConfig.GrowthDefence) / 100);
        //_rectText.DOLocalMoveY(90, 0.6f);
        DGHelper.DoLocalMoveY(_rectText, 90, 0.6f);
        DelayCall(0.6f, OnCall);
    }

    private void OnCall()
    {
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(HeroEvent.CardLevelUp, this);
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}