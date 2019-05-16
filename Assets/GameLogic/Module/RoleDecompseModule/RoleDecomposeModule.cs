using UnityEngine;
using Framework.UI;
using System;

public class RoleDecomposeModule : ModuleBase
{
    private DecRoleListView _decRoleListView;
    private DecRewardView _rewardView;
    private Transform _root;

    public RoleDecomposeModule()
        : base(ModuleID.RoleDecompose, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_RoleDecompose;
        _soundName = UIModuleSoundName.RoleDecomposeSoundName;
        mBlNeedBackMask = true;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _root = Find<Transform>("RightSide");
        _decRoleListView = new DecRoleListView();
        _decRoleListView.SetDisplayObject(Find("LeftSide"));
        AddChildren(_decRoleListView);

        RoleListView roleListView = new RoleListView();
        roleListView.SetDisplayObject(_root.gameObject);
        AddChildren(roleListView);

        _rewardView = new DecRewardView();
        _rewardView.SetDisplayObject(Find("RewardView"));
        _rewardView.SortingOrder = UILayerSort.WindowSortBeginner + 2;
    }

    public override void Show(params object[] args)
    {
        base.Show(args);
        DecomposeDataModel.Instance.PrePareData();
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        DecomposeDataModel.Instance.AddEvent<bool>(DecomposeEvent.ShowDecomposeReward, OnShowReward);
        HeroDataModel.Instance.AddEvent(HeroEvent.HeroCardChange, OnHeroCardChange);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        DecomposeDataModel.Instance.RemoveEvent<bool>(DecomposeEvent.ShowDecomposeReward, OnShowReward);
        HeroDataModel.Instance.RemoveEvent(HeroEvent.HeroCardChange, OnHeroCardChange);
    }

    private void OnHeroCardChange()
    {
        DecomposeDataModel.Instance.ResetSelectRole();
        _decRoleListView.Show();
    }

    private void OnShowReward(bool blDecomposeBack)
    {
        Action OnShowReward = () =>
        {
            _rewardView.Show();
        };

        if (blDecomposeBack)
        {
            SoundMgr.Instance.PlayEffectSound("UI_fenjie_disassemble");
            _decRoleListView.RoleDecomposeBack();
            DelayCall(0.35f, OnShowReward);
        }
        else
        {
            OnShowReward();
        }
    }

    public override void Dispose()
    {
        if (_decRoleListView != null)
        {
            _decRoleListView.Dispose();
            _decRoleListView = null;
        }
        if (_rewardView != null)
        {
            _rewardView.Dispose();
            _rewardView = null;
        }
        base.Dispose();
    }

    public override void Hide()
    {
        base.Hide();
        StopAllEffectSound();
    }

    protected override void OnShowAnimator()
    {
        base.OnShowAnimator();
        ObjectHelper.AnimationMoveBack(_root,ObjectHelper.direction.left);
    }
}