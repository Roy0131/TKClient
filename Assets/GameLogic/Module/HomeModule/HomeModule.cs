using Framework.UI;
using NewBieGuide;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Msg.ClientMessage;

public class HomeModule : ModuleBase
{
    private Text _label;
    private Button _welfareBtn;
    private Button _btnDailyCheck;
    private Button _btnActivityCopy;
    private Button _btnWelfare;
    private Button _btnSetting;
    private Button _btnCarnival;

    private Button _btnBack;
    private RectTransform _trans;
    private RectTransform _verLayout;

    private Button _btnHeroDeta;
    private Button _btnArtifact;
    private Button _btnTalent;
    private Button _btnTask;
    private Button _btnUnion;
    private Button _btnBag;
    private Button _btnHero;
    private Button _btnDiamondStore;

    private Button _btnEmail;
    private Button _btnFriend;
    private Button _btnChat;
    private Button _btnRank;

    private Button _btnGold;
    private Button _btnDiamond;

    private Button _firstReCharge;

    private Text _heroLevel;
    private Text _totalGoldLabel;
    private Text _totalDiamondLabel;
    private Image _imgTotalGold;
    private Image _imgTotalDiamond;
    private Image _playerAvatar;

    private Transform _root01;
    private Transform _root02;
    private Transform _root03;
    private Transform _root04;
    private Transform _root05;
    private Transform _root06;
    private Transform _root07;
    private Transform _root08;
    private Transform _root09;

    private GameObject _welfareObj;

    private static GameObject _activityStageRedPoint;
    private static GameObject _welfareStageRedPoint;

    public HomeModule()
        : base(ModuleID.Home, UILayer.Module)
    {
        _modelResName = UIModuleResName.UI_Home;
    }

    private void BindFuncRedPoint()
    {
        RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.BagFragment, _btnBag.transform.Find("RedPoint").gameObject);
        RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.Task, _btnTask.transform.Find("RedPoint").gameObject);
        RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.Sign, _btnDailyCheck.transform.Find("RedPoint").gameObject);
        RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.Welfare, _btnWelfare.transform.Find("RedPoint").gameObject);
        RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.Mail, _btnEmail.transform.Find("RedPoint").gameObject);
        RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.GoldHand, _btnGold.transform.Find("RedPoint").gameObject);
        RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.Guild, _btnUnion.transform.Find("RedPoint").gameObject);
        RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.Friend, _btnFriend.transform.Find("RedPoint").gameObject);
        RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.Chat, _btnChat.transform.Find("RedPoint").gameObject);
        RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.FirstCharge, _firstReCharge.transform.Find("RedPoint").gameObject);
        //RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.ActivityStage, _btnActivityCopy.transform.Find("RedPoint").gameObject);
    }

    public void UnBindFuncRedPoint()
    {
        //RedPointTipsMgr.Instance.RedPointUnBindObject(RedPointEnum.BagFragment, _btnBag.transform.Find("RedPoint").gameObject);
    }

    private void RegisteGuideMaskObject()
    {
        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.BagModule, _btnBag.transform);
        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.RoleBagModule, _btnHero.transform);
        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.BoonModule, _btnDailyCheck.transform);
    }

    private void UnRegisteGuideMaskObject()
    {
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.BagModule);
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.RoleBagModule);
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.BoonModule);
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();

        _blGameInitFinished = false;
        _root01 = Find<Transform>("ButtonMainGrid");
        _root02 = Find<Transform>("Diamond");
        _root03 = Find<Transform>("SettingGrid");
        _root04 = Find<Transform>("OtherGrid");
        _root05 = Find<Transform>("RoleIcon");
        _root06 = Find<Transform>("TotalGold");
        _root07 = Find<Transform>("TotalDiamond");
        _root08 = Find<Transform>("ImageBack");
        _root09 = Find<Transform>("ButtonRank");

        _heroLevel = Find<Text>("RoleIcon/ImageBack/Level");
        _totalGoldLabel = Find<Text>("TotalGold/TotalGoldLabel");
        _totalDiamondLabel = Find<Text>("TotalDiamond/TotalDiamondLabel");
        _imgTotalGold = Find<Image>("TotalGold/BtnTotalGold");
        _imgTotalDiamond = Find<Image>("TotalDiamond/BtnTotalDiamond");
        _playerAvatar = Find<Image>("RoleIcon/ImageBack/heroDeta");

        _btnBack = Find<Button>("ImageBack/Button");
        _trans = Find<RectTransform>("ImageBack/Button/Image");
        _verLayout = Find<RectTransform>("SettingGrid");

        _label = Find<Text>("Label");

        _btnHeroDeta = Find<Button>("RoleIcon/ImageBack/BtnImage");
        _welfareBtn = Find<Button>("Welfare/WelfareButton");
        _btnDailyCheck = Find<Button>("SettingGrid/ButtonDailyCheck");
        _btnActivityCopy = Find<Button>("SettingGrid/ButtonActivityCopy");
        _btnWelfare = Find<Button>("SettingGrid/ButtonWelfare");
        _btnSetting = Find<Button>("SettingGrid/ButtonSetting");
        _btnCarnival = Find<Button>("Carnival");

        _btnArtifact = Find<Button>("ButtonMainGrid/ButtonArtifact");
        _btnTalent = Find<Button>("ButtonMainGrid/ButtonTalent");
        _btnTask = Find<Button>("ButtonMainGrid/ButtonTask");
        _btnUnion = Find<Button>("ButtonMainGrid/ButtonUnion");
        _btnBag = Find<Button>("ButtonMainGrid/ButtonBag");
        _btnHero = Find<Button>("ButtonMainGrid/ButtonHero");

        _btnDiamondStore = Find<Button>("Diamond/ButtonDiamondStore");

        _btnEmail = Find<Button>("OtherGrid/ButtonEmail");
        _btnFriend = Find<Button>("OtherGrid/ButtonFriend");
        _btnChat = Find<Button>("OtherGrid/ButtonChat");
        _btnRank = Find<Button>("ButtonRank");

        _btnGold = Find<Button>("TotalGold/BtnBuyGold");
        _btnDiamond = Find<Button>("TotalDiamond/BtnBuyDiamond");

        _firstReCharge = Find<Button>("FirstRecharge");
        ColliderHelper.SetButtonCollider(_btnGold.transform);
        ColliderHelper.SetButtonCollider(_btnDiamond.transform);

        _welfareStageRedPoint = Find("Welfare/RedPoint");
        RefreshWelfareRedPoint(true);
        _activityStageRedPoint = Find("SettingGrid/ButtonActivityCopy/RedPoint");

        if (HeroDataModel.Instance.mHeroInfoData.mLevel > 19)
            RefreshARedPoint(true);
        else
            RefreshARedPoint(false);

        _btnHeroDeta.onClick.Add(() => { OnOpenMoude(_btnHeroDeta.name); });
        _welfareBtn.onClick.Add(() => { OnOpenMoude(_welfareBtn.name); });
        _btnDailyCheck.onClick.Add(() => { OnOpenMoude(_btnDailyCheck.name); });
        _btnActivityCopy.onClick.Add(() => { OnOpenMoude(_btnActivityCopy.name); });
        _btnWelfare.onClick.Add(() => { OnOpenMoude(_btnWelfare.name); });
        _btnSetting.onClick.Add(() => { OnOpenMoude(_btnSetting.name); });
        _btnCarnival.onClick.Add(() => { OnOpenMoude(_btnCarnival.name); });
        _btnBack.onClick.Add(AniSetting);

        _btnArtifact.onClick.Add(() => { OnOpenMoude(_btnArtifact.name); });
        _btnTalent.onClick.Add(() => { OnOpenMoude(_btnTalent.name); });
        _btnTask.onClick.Add(() => { OnOpenMoude(_btnTask.name); });
        _btnUnion.onClick.Add(() => { OnOpenMoude(_btnUnion.name); });
        _btnBag.onClick.Add(() => { OnOpenMoude(_btnBag.name); });
        _btnHero.onClick.Add(() => { OnOpenMoude(_btnHero.name); });

        _btnDiamondStore.onClick.Add(() => { OnOpenMoude(_btnDiamondStore.name); });

        _btnEmail.onClick.Add(() => { OnOpenMoude(_btnEmail.name); });
        _btnFriend.onClick.Add(() => { OnOpenMoude(_btnFriend.name); });
        _btnChat.onClick.Add(() => { OnOpenMoude(_btnChat.name); });
        _btnRank.onClick.Add(() => { OnOpenMoude(_btnRank.name); });

        _btnGold.onClick.Add(() => { OnOpenMoude(_btnGold.name); });
        _btnDiamond.onClick.Add(() => { OnOpenMoude(_btnDiamond.name); });

        _firstReCharge.onClick.Add(() => { OnOpenMoude(_firstReCharge.name); });

        CreateFixedEffect(Find("Diamond/fx_ui_zuanshi01"), UILayerSort.ModuleSortBeginner);

        BindFuncRedPoint();
        RegisteGuideMaskObject();

        _welfareObj = Find("Welfare");
        _welfareObj.SetActive(GameConfigMgr.Instance.GetScreenConfig(ScreenType.Welfare).Type == 0);
        _btnWelfare.gameObject.SetActive(GameConfigMgr.Instance.GetScreenConfig(ScreenType.Activity).Type == 0);

        NameObject();
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(HeroEvent.UpGrade, NameObject);
    }

    private void NameObject()
    {
        if (HeroDataModel.Instance.mHeroInfoData != null)
            Find("ButtonMainGrid/ButtonArtifact/Text").gameObject.SetActive(HeroDataModel.Instance.mHeroInfoData.mLevel >= GameConst.GetFeatureType(FunctionType.Artifact));
    }

    private void AniSetting()
    {
        if (_verLayout.transform.localScale.y == 1.0f)
        {
            //_verLayout.DOScale(new Vector3(1, 0, 1), 0.15f).SetEase(Ease.Linear);
            DGHelper.DoScale(_verLayout, new Vector3(1, 0, 1), 0.15f);
            _trans.localScale = Vector2.one;
        }
        else
        {
            //_verLayout.DOScale(Vector3.one, 0.15f).SetEase(Ease.Linear);
            DGHelper.DoScale(_verLayout, Vector3.one, 0.15f);
            _trans.localScale = new Vector2(1f, -1f);
        }
    }

    private bool _blGameInitFinished = false;

    private void OnOpenMoude(string name)
    {
        if (!_blGameInitFinished)
            return;
        switch (name)
        {
            case "BtnImage":
                LogHelper.Log("打开heroDeta");
                GameUIMgr.Instance.OpenModule(ModuleID.Player);
                break;
            case "ButtonActivityCopy":
                LogHelper.Log("打开ButtonActivityCopy");
                GameUIMgr.Instance.OpenModule(ModuleID.ActivityCopy, false);
                break;
            case "ButtonSetting":
                LogHelper.Log("打开ButtonSetting");
                GameUIMgr.Instance.OpenModule(ModuleID.Setting, true);
                break;
            case "ButtonTask":
                GameUIMgr.Instance.OpenModule(ModuleID.Task);
                //TaskDataModel.Instance.ReqTaskData(TaskTypeConst.DAILYTask);
                LogHelper.Log("打开ButtonTask");
                break;
            case "ButtonUnion":
                if (FunctionUnlock.IsUnlock(FunctionType.Guild))
                {
                    if (HeroDataModel.Instance.mHeroInfoData.mGuildId == 0)
                        GameUIMgr.Instance.OpenModule(ModuleID.Guild);
                    else
                        GameUIMgr.Instance.OpenModule(ModuleID.HeroGuild);
                }
                LogHelper.Log("打开ButtonUnion");
                break;
            case "ButtonBag":
                GameUIMgr.Instance.OpenModule(ModuleID.Bag);
                break;
            case "ButtonHero":
                LogHelper.Log("打开ButtonHero");
                GameUIMgr.Instance.OpenModule(ModuleID.RoleBag);
                break;
            case "ButtonEmail":
                GameUIMgr.Instance.OpenModule(ModuleID.Mail);
                break;
            case "ButtonFriend":
                GameUIMgr.Instance.OpenModule(ModuleID.Friend);
                break;
            case "ButtonChat":
                LogHelper.Log("打开ButtonChat");
                GameUIMgr.Instance.OpenModule(ModuleID.Chat);
                break;
            case "ButtonRank":
                GameUIMgr.Instance.OpenModule(ModuleID.Rank);
                LogHelper.Log("打开ButtonRank");
                break;
            case "BtnBuyGold":
                GameUIMgr.Instance.OpenModule(ModuleID.Gold);
                break;
            case "ButtonDailyCheck":
                GameUIMgr.Instance.OpenModule(ModuleID.Attendance);
                break;
            case "ButtonWelfare":
                GameUIMgr.Instance.OpenModule(ModuleID.Welfare, WelfareConst.Activity);
                break;
            case "WelfareButton":
                GameUIMgr.Instance.OpenModule(ModuleID.Welfare, WelfareConst.Welfare);
                break;
            case "BtnBuyDiamond":
            case "ButtonDiamondStore":
                GameUIMgr.Instance.OpenModule(ModuleID.Recharge);
                break;
            case "ButtonArtifact":
                if (FunctionUnlock.IsUnlock(FunctionType.Artifact))
                    GameUIMgr.Instance.OpenModule(ModuleID.Artifact);
                break;
            case "ButtonTalent":
                GameUIMgr.Instance.OpenModule(ModuleID.Talent, TalentTypeConst.Basis);
                //PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001155));
                break;
            case "Carnival":
                GameUIMgr.Instance.OpenModule(ModuleID.Carnival);
                break;
            case "FirstRecharge":
                GameUIMgr.Instance.OpenModule(ModuleID.Welfare, WelfareConst.Activity, WelfareType.FirstCharge);
                break;
        }
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        HeroDataModel.Instance.AddEvent(HeroEvent.HeroInfoChange, OnRefreshs);
        HeroDataModel.Instance.AddEvent(HeroEvent.AmendHead, OnHead);
        RechargeDataModel.Instance.AddEvent<List<MonthCardData>>(RechargeEvent.RechargeData, OnRechargeChange);
        RechargeDataModel.Instance.AddEvent<List<ItemInfo>>(RechargeEvent.FirstRecharge, OnHideFirstRechargeBtn);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        HeroDataModel.Instance.RemoveEvent(HeroEvent.HeroInfoChange, OnRefreshs);
        HeroDataModel.Instance.RemoveEvent(HeroEvent.AmendHead, OnHead);
        RechargeDataModel.Instance.RemoveEvent<List<MonthCardData>>(RechargeEvent.RechargeData, OnRechargeChange);
        RechargeDataModel.Instance.RemoveEvent<List<ItemInfo>>(RechargeEvent.FirstRecharge, OnHideFirstRechargeBtn);
    }

    private void OnHideFirstRechargeBtn(List<ItemInfo> value)
    {
        _firstReCharge.gameObject.SetActive(false);
    }

    private void OnRechargeChange(List<MonthCardData> value)
    {
        _firstReCharge.gameObject.SetActive(RechargeDataModel.Instance.mFirstChargeState != 2);
    }
    
    private void OnHead()
    {
        if (HeroDataModel.Instance.mHeroInfoData.mIcon > 0)
            _playerAvatar.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(HeroDataModel.Instance.mHeroInfoData.mIcon).Icon);
        ObjectHelper.SetSprite(_playerAvatar, _playerAvatar.sprite);
    }

    private void OnRefreshs()
    {
        _heroLevel.text = HeroDataModel.Instance.mHeroInfoData.mLevel.ToString();
        _totalGoldLabel.text = UnitChange.GetUnitNum(HeroDataModel.Instance.mHeroInfoData.mGold);
        _totalDiamondLabel.text = UnitChange.GetUnitNum(HeroDataModel.Instance.mHeroInfoData.mDiamond);
        //OnRechargeChange();
        _firstReCharge.gameObject.SetActive(RechargeDataModel.Instance.mFirstChargeState != 2);
        if (HeroDataModel.Instance.mHeroInfoData.mIcon > 0)
            _playerAvatar.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(HeroDataModel.Instance.mHeroInfoData.mIcon).Icon);
    }
    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        OnRefreshs();
    }

    public void PlayAnimation()
    {
        ObjectHelper.AnimationMoveLiner(_root01, ObjectHelper.direction.up);
        ObjectHelper.AnimationMoveLiner(_root02, ObjectHelper.direction.up);
        ObjectHelper.AnimationMoveLiner(_root03, ObjectHelper.direction.left);
        ObjectHelper.AnimationMoveLiner(_root04, ObjectHelper.direction.right);
        ObjectHelper.AnimationMoveLiner(_root05, ObjectHelper.direction.down);
        ObjectHelper.AnimationMoveLiner(_root06, ObjectHelper.direction.down);
        ObjectHelper.AnimationMoveLiner(_root07, ObjectHelper.direction.down);
        ObjectHelper.AnimationMoveLiner(_root08, ObjectHelper.direction.left);
        ObjectHelper.AnimationMoveLiner(_root09, ObjectHelper.direction.right);
    }


    protected override void OnShowAnimator()
    {
        base.OnShowAnimator();
        PlayAnimation();

        if (_enterKey != 0)
        {
            TimerHeap.DelTimer(_enterKey);
            _enterKey = 0;
        }
        _enterKey = TimerHeap.AddTimer(1000, 0, OnEnterHome);
    }

    private uint _enterKey = 0;
    private void OnEnterHome()
    {
        TimerHeap.DelTimer(_enterKey);
        _enterKey = 0;
        _blGameInitFinished = true;
        NewBieGuide.NewBieGuideMgr.Instance.OnEnterGame();
    }

    public void RefreshWelfareRedPoint(bool value)
    {
        _welfareStageRedPoint.SetActive(value);
    }

    public void RefreshARedPoint(bool value)
    {
        _activityStageRedPoint.SetActive(value);
    }

    public override void Dispose()
    {
        base.Dispose();
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(HeroEvent.UpGrade, NameObject);
    }
}