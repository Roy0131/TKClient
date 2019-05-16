using UnityEngine;
using UnityEngine.UI;
using Framework.UI;
using System;

public class HangupModule : ModuleBase
{
    private Button _disBtn;

    private HangupBattleView _battleView;
    private HangupMapView _mapView;
    private ItemResGroup _itemResGroup;
    private Button _helpBtn;
    private Transform _resRoot;

    public HangupModule()
        :base(ModuleID.Hangup, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_Hangup;
        _soundName = UIModuleSoundName.HanguoSoundName;
        mBlNeedBackMask = true;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();

        _disBtn = Find<Button>("Btn_Back");
        _resRoot = Find<Transform>("ResRoot/uiResGroup");
        _itemResGroup = new ItemResGroup();
        _itemResGroup.SetDisplayObject(_resRoot.gameObject);
        _itemResGroup.Show(SpecialItemID.Gold, SpecialItemID.HeroExp);
        _helpBtn = Find<Button>("BtnHelp");
        _battleView = new HangupBattleView();
        _battleView.SetDisplayObject(Find("HangupBattleObject"));
        AddChildren(_battleView);
        
        _mapView = new HangupMapView();
        _mapView.SetDisplayObject(Find("HangupMapObject"));
        AddChildren(_mapView);
        _helpBtn.onClick.Add(OnShowHelp);
        _disBtn.onClick.Add(OnClose);
        NewBieGuide.NewBieGuideMgr.Instance.RegistMaskTransform(NewBieGuide.NewBieMaskID.HangupBackBtn, _disBtn.transform);

        ColliderHelper.SetButtonCollider(_disBtn.transform);
        ColliderHelper.SetButtonCollider(_helpBtn.transform);
    }

    public override void Dispose()
    {
        NewBieGuide.NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieGuide.NewBieMaskID.HangupBackBtn);
        base.Dispose();
    }

    public override void Hide()
    {
        base.Hide();
        HangUpMgr.Instance.HideHangup();
        StopAllEffectSound();
    }

    public override void Show(params object[] args)
    {
        base.Show(args);
        HangUpMgr.Instance.ShowHangup(HangupDataModel.Instance.mHangDataVO);
        _itemResGroup.Show(SpecialItemID.Gold, SpecialItemID.HeroExp);
    }
    private void OnShowHelp()
    {
        HelpTipsMgr.Instance.ShowTIps(HelpType.HangupHelp);
    }

    protected override void OnShowAnimator()
    {
        base.OnShowAnimator();
        ObjectHelper.AnimationMoveBounce(_mapView.mTransform, ObjectHelper.direction.right);

        ObjectHelper.AnimationMoveBounce(_battleView.mTransform, ObjectHelper.direction.left);

        ObjectHelper.AnimationMoveLiner(_resRoot,ObjectHelper.direction.down);

        Action OnAnimatorEnd = () =>
        {
            GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.EndCondTrigger, NewBieGuide.EndConditionConst.HangupModuleOpen);
        };

        DelayCall(0.7f, OnAnimatorEnd);
    }
}