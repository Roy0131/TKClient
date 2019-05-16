using Framework.UI;
using Msg.ClientMessage;
using NewBieGuide;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HangupBattleView : UIBaseView
{
    private Text _rewardGlodTxt;
    private Text _rewardHeroExpTxt;
    private Text _rewardRoleExpText;

    private Text _levelNameTxt;

    private Button _rdmRewardBtn;
    private Button _fixedRewardBtn;
    private Button _battleBtn;
    private Button _heroBtn;
    private Button _levelinfBtn;
    private Button _randDIsBtn;

    private HangupOutputView _rewardView;

    private HangupRandRewardView _randView;

    private Button _accelerateBtn;

    private Button _btnProgress;
    private Image _btnProgressImage;
    private Image _btnPassedImage;
    private Text _battleText;

    private uint _times = 0;
    private int _hangupTimes = 0;
    private bool isTiming;

    private Transform _goldOri;
    private Transform _goldDes;

    private Transform _soulOri;
    private Transform _soulDes;

    private RawImage _battleImage;

    private GameObject _speciallyObj;
    private UIEffectView _effect;

    protected override void ParseComponent()
    {
        base.ParseComponent();

        _goldOri = Find<Transform>("GoldOri");
        _goldDes = Find<Transform>("GoldDes");
        _soulOri = Find<Transform>("SoulOri");
        _soulDes = Find<Transform>("SoulDes");

        _rewardGlodTxt = Find<Text>("RewardGlodLabel/Text");
        _rewardHeroExpTxt = Find<Text>("RewardHeroExpLabel/Text");
        _rewardRoleExpText = Find<Text>("RewardRoleExpLabel/Text");
        _levelNameTxt = Find<Text>("LevelNameLabel");

        _fixedRewardBtn = Find<Button>("BtnFixedReward");
        _rdmRewardBtn = Find<Button>("BtnRdmReward");
        _heroBtn = Find<Button>("BtnFormation");
        _levelinfBtn = Find<Button>("ButtonLevelInf");
        _randDIsBtn = Find<Button>("HangupRandObect/DisBat");
        _accelerateBtn = Find<Button>("AccelerateBtn");

        _btnProgressImage = Find<Image>("BtnBattle/BattleProgress");
        _btnProgress = Find<Button>("BtnBattle/BattleProgress");
        _btnPassedImage = Find<Image>("BtnBattle/ImagePassed");

        _battleText = Find<Text>("BtnBattle/Text");

        _rdmRewardBtn.onClick.Add(OnGetRdmReward);
        _fixedRewardBtn.onClick.Add(OnGetFixedReward);
        _btnProgress.onClick.Add(OnBattle);
        _accelerateBtn.onClick.Add(OnAccelerate);
        _heroBtn.onClick.Add(OnHeroClick);
        _levelinfBtn.onClick.Add(OnLevelinf);

        _rewardView = new HangupOutputView();
        _rewardView.SetDisplayObject(Find("OutPutObj"));
        _rewardView.SortingOrder = UILayerSort.WindowSortBeginner + 2;

        _randView = new HangupRandRewardView();
        _randView.SetDisplayObject(Find("RandObj"));
        _randView.SortingOrder = UILayerSort.WindowSortBeginner + 2;

        _battleImage = Find<RawImage>("Img_Scene");

        RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.Campain, Find("BtnRdmReward/RedPoint"));
        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.HangupBattleBtn, _btnProgress.transform);
        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.HangupFixedRewardBtn, _fixedRewardBtn.transform);

        ColliderHelper.SetButtonCollider(_levelinfBtn.transform, 80f, 80f);
        //_recordView.SortingOrder = UILayerSort.WindowSortBeginner + 2;
        CreateFixedEffect(_battleText.gameObject, UILayerSort.WindowSortBeginner + 1, SortObjType.Canvas);
        _speciallyObj = Find("fx_ui_finish");
        _effect = CreateUIEffect(_speciallyObj, UILayerSort.WindowSortBeginner);
    }

    private void OnAccelerate()
    {
        ConfirmTipsMgr.Instance.ShowConfirmTips(LanguageMgr.GetLanguage(5002924) + "\n" + LanguageMgr.GetLanguage(5002925, HangupDataModel.Instance.mAccelerateDiamond) + "\n" + LanguageMgr.GetLanguage(5002926, HangupDataModel.Instance.mAccelerateNum), AlertBack);
    }

    private void AlertBack(bool result, bool blShowAgain)
    {
        if (result)
        {
            if (HangupDataModel.Instance.mAccelerateNum > 0)
            {
                if (HeroDataModel.Instance.mHeroInfoData.mDiamond >= HangupDataModel.Instance.mAccelerateDiamond)
                {
                    GameNetMgr.Instance.mGameServer.ReqAccelerate();
                    TDPostDataMgr.Instance.DoCostDiamond(TDCostDiamondType.BuyHangupSpeed,1,HangupDataModel.Instance.mAccelerateDiamond);
                }
                else
                    PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000055));
            }
            else
            {
                PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000107));
            }
        }
        else
        {

        }
    }

    private void OnAccelerateReward(List<ItemInfo> listInfo)
    {
        GetItemTipMgr.Instance.ShowItemResult(listInfo);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        HangupDataModel.Instance.AddEvent(HangupEvent.FixedRewardChange, OnRefreshReward);
        HangupDataModel.Instance.AddEvent<List<ItemInfo>>(HangupEvent.RandomRewardChange, OnShowRandomReward);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<int>(HangupEvent.MapSetHangupStage, OnShowMapReward);
        HangupDataModel.Instance.AddEvent(HangupEvent.CampaignDataChange, OnHangupChange);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(HeroEvent.UpGrade, OnRefreshs);
        HangupDataModel.Instance.AddEvent<List<ItemInfo>>(HangupEvent.AccelerateReward, OnAccelerateReward);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        HangupDataModel.Instance.RemoveEvent(HangupEvent.FixedRewardChange, OnRefreshReward);
        HangupDataModel.Instance.RemoveEvent<List<ItemInfo>>(HangupEvent.RandomRewardChange, OnShowRandomReward);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<int>(HangupEvent.MapSetHangupStage, OnShowMapReward);
        HangupDataModel.Instance.RemoveEvent(HangupEvent.CampaignDataChange, OnHangupChange);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(HeroEvent.UpGrade, OnRefreshs);
        HangupDataModel.Instance.RemoveEvent<List<ItemInfo>>(HangupEvent.AccelerateReward, OnAccelerateReward);
    }

    private void OnRefreshs()
    {
        if (HeroDataModel.Instance.mHeroInfoData.isLevelUp)
        {
            SoundMgr.Instance.PlayEffectSound("UI_level");
            PlayerLevelUpMgr.Instance.ShowLevelUp(HeroDataModel.Instance.mHeroInfoData.mLevel);
            HeroDataModel.Instance.mHeroInfoData.OnIsLevelUp();
            DelayCall(1.5f, () => PlayerLevelUpMgr.Instance.LevelUpHide());
            if (!NewBieGuideMgr.Instance.mBlGuideForce)
                DelayCall(1.5f, OnFunctionMgr);
        }
        else
        {
            HeroDataModel.Instance.mHeroInfoData.OnIsLevelUp();
        }
    }

    private void OnFunctionMgr()
    {
        if (HeroDataModel.Instance.mHeroInfoData.mSystConfig != null)
            FunctionMgr.Instance.ShowFeature(HeroDataModel.Instance.mHeroInfoData.mSystConfig);
    }

    private void OnLevelinf()
    {
        OnShowMapReward(HangupDataModel.Instance.mIntHangupCampaignId);
    }

    private void OnShowMapReward(int id)
    {
        _rewardView.Show(id);
    }

    private void OnShowRandomReward(List<ItemInfo> value)
    {
        if (value.Count > 0)
            _randView.Show(value);
    }

    private void OnRefreshReward()
    {
        _rewardGlodTxt.text = UnitChange.GetUnitNum(HangupDataModel.Instance.GetRewardItemCntById(SpecialItemID.Gold));
        _rewardHeroExpTxt.text = UnitChange.GetUnitNum(HangupDataModel.Instance.GetRewardItemCntById(SpecialItemID.HeroExp));
        _rewardRoleExpText.text = UnitChange.GetUnitNum(HangupDataModel.Instance.GetRewardItemCntById(SpecialItemID.RoleExp));
        int chapterMap = (HangupDataModel.Instance.CurHangupConfig.Difficulty - 1) * 8 + HangupDataModel.Instance.CurHangupConfig.ChapterMap;
        _levelNameTxt.text = LanguageMgr.GetLanguage(5002908) + chapterMap + "-" + HangupDataModel.Instance.CurHangupConfig.ChildMapID.ToString();
    }

    private void OnHangupChange()
    {
        OnRefreshReward();
        if (HangupDataModel.Instance.CheckCampaignCanBattle())
        {
            if (LocalDataMgr.CheckCampFirstBattle(HangupDataModel.Instance.mIntHangupCampaignId))
            {
                isTiming = false;
                _blCanBattle = true;
                _btnProgressImage.gameObject.SetActive(true);
                //_btnProgressImage.fillAmount = 1f;
                _battleText.text = LanguageMgr.GetLanguage(5001506);//战斗
                _effect.StopEffect();
            }
            else
            {
                _effect.PlayEffect();
                _blCanBattle = false;
                isTiming = true;
                _hangupTimes = GameDriver.Instance.mShowDebug ? 1 : 8;
                if (GameDriver.Instance.mShowGuide)
                    _hangupTimes = 8;
                if (_times != 0)
                    TimerHeap.DelTimer(_times);
                int intervals = 1000;
                _times = TimerHeap.AddTimer(0, intervals, OnAddTimes);
            }
        }
        else
        {
            isTiming = false;
            _blCanBattle = false;
            _battleText.text = LanguageMgr.GetLanguage(6001250);//通过
            _effect.StopEffect();
            _btnProgressImage.gameObject.SetActive(false);
            _btnPassedImage.gameObject.SetActive(true);
        }
    }

    private void OnAddTimes()
    {
        if (isTiming)
        {
            if (_hangupTimes > 0)
            {
                _btnProgressImage.gameObject.SetActive(false);
                _battleText.text = "00:00:0" + _hangupTimes;
                if(_hangupTimes == 5)
                    GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.EndCondTrigger, NewBieGuide.EndConditionConst.HangupChapterTipEnd);
            }   
            else
            {
                _btnProgressImage.gameObject.SetActive(true);
                _blCanBattle = true;
                _battleText.text = LanguageMgr.GetLanguage(5001506);//战斗
                LocalDataMgr.SaveCampaignID(HangupDataModel.Instance.mIntHangupCampaignId);
                _effect.StopEffect();
                if (_times != 0)
                    TimerHeap.DelTimer(_times);
                GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.EnterCondTrigger, NewBieGuide.EnterCondConst.HangupProgressOver);
            }     
        }
        else
        {
            if (HangupDataModel.Instance.CheckCampaignCanBattle())
            {
                _btnProgressImage.gameObject.SetActive(true);
                _blCanBattle = true;
                _battleText.text = LanguageMgr.GetLanguage(5001506);//战斗
                LocalDataMgr.SaveCampaignID(HangupDataModel.Instance.mIntHangupCampaignId);
                _effect.StopEffect();
            }
            else
            {
                _btnProgressImage.gameObject.SetActive(false);
                _battleText.text = LanguageMgr.GetLanguage(6001250);//通过
                _effect.StopEffect();
            }
        }
        if (_hangupTimes > 0)
            _hangupTimes -= 1;
    }

    private bool _blCanBattle;
    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        DelayCall(1f, OnRefreshs);
        OnHangupChange();
        _battleImage.texture = RoleRTMgr.Instance.GetRoleRTImage();
    }

    private void OnFinish()
    {
        _effect.StopEffect();
        _btnProgressImage.gameObject.SetActive(true);
        DGHelper.DoKill(_btnPassedImage);
    }

    private void OnGetRdmReward()
    {
        if (RedPointDataModel.Instance.GetRedPointStatus(RedPointEnum.Campain))
            GameNetMgr.Instance.mGameServer.ReqHangupReward(1);
        else
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001148));
    }

    private void OnGetFixedReward()
    {
        GameNetMgr.Instance.mGameServer.ReqHangupReward(0);
        if (HangupDataModel.Instance.GetRewardItemCntById(SpecialItemID.Gold) != 0)
        {
            SoundMgr.Instance.PlayEffectSound("UI_btn_reward1");
            _goldOri.gameObject.SetActive(false);
            _soulOri.gameObject.SetActive(false);
            Action<GameObject> OnLoadJBEffectEnd = (jbObject) =>
            {
                jbObject.transform.SetParent(_goldOri, false);
                _goldOri.gameObject.SetActive(true);
                //jbObject.transform.DOMove(_goldDes.position, 0.6f).SetEase(Ease.Linear);
                DGHelper.DoMove(jbObject.transform, _goldDes.position, 0.6f);
                DelayCall(1f, () => GameObject.Destroy(_goldOri.GetChild(0).gameObject));
            };

            GameResMgr.Instance.LoadEffect("fx_ui_jinbilizi", OnLoadJBEffectEnd);

            Action<GameObject> OnLoadSoulEffectEnd = (soulObject) =>
            {
                soulObject.transform.SetParent(_soulOri, false);
                _soulOri.gameObject.SetActive(true);
                //soulObject.transform.DOMove(_soulDes.position, 0.6f).SetEase(Ease.Linear);
                DGHelper.DoMove(soulObject.transform, _soulDes.position, 0.6f);
                DelayCall(1f, () => GameObject.Destroy(_soulOri.GetChild(0).gameObject));
            };

            GameResMgr.Instance.LoadEffect("fx_ui_zuanshilizi", OnLoadSoulEffectEnd);
        }
       
    }


    private void OnBattle()
    {
        if (!_blCanBattle)
            return;
        LineupSceneMgr.Instance.ShowLineupModule(TeamType.Hangeup, HangupDataModel.Instance.mIntHangupCampaignId);
    }

    private void OnHeroClick()
    {
        GameUIMgr.Instance.OpenModule(ModuleID.RoleBag);
    }

    public override void Dispose()
    {
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.HangupBattleBtn);
        base.Dispose();
    }
}