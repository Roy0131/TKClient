using Framework.UI;
using NewBieGuide;
using System;
using UnityEngine;
using UnityEngine.UI;

public class RoleBagModule : ModuleBase
{
    private Button _closeBtn;
    protected RoleBagView _roleBagView;
    private Transform _root;

    public RoleBagModule()
        : base(ModuleID.RoleBag, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_RoleBag;
        mBlNeedBackMask = true;
    }

	protected override void ParseComponent()
	{
        base.ParseComponent();
        _closeBtn = Find<Button>("Btn_Back");
        _root = Find<Transform>("Root");

        _roleBagView = new RoleBagView();
        _roleBagView.SetDisplayObject(Find("Root"));
        AddChildren(_roleBagView);

        _closeBtn.onClick.Add(OnClose);
        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.RoleBagDisBtn, _closeBtn.transform);
        ColliderHelper.SetButtonCollider(_closeBtn.transform);
    }

    protected override void OnShowAnimator()
    {
        base.OnShowAnimator();
        ObjectHelper.PopAnimationLiner(_root);
        Action OnAnimatorEnd = () =>
        {
            GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.EndCondTrigger, NewBieGuide.EndConditionConst.RoleBagModuleOpen);
        };

        DelayCall(0.5f, OnAnimatorEnd);
    }
}