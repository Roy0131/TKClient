using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using Framework.UI;

public class EquipmentModule : ModuleBase
{
    private Transform _root;
    public EquipmentModule()
        : base(ModuleID.Equipment, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_Equipment;
        mBlNeedBackMask = true;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        EquipForgeView forgeView = new EquipForgeView();
        forgeView.SetDisplayObject(Find("LeftSide"));
        AddChildren(forgeView);
        _root = Find<Transform>("RightSide");
        EquipListView equipView = new EquipListView();
        equipView.SetDisplayObject(_root.gameObject);
        AddChildren(equipView);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        BagDataModel.Instance.AddEvent<IList<ItemInfo>>(BagEvent.OneKeyUpGrade, OnOneKeyUpGrade);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        BagDataModel.Instance.RemoveEvent<IList<ItemInfo>>(BagEvent.OneKeyUpGrade, OnOneKeyUpGrade);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        SoundMgr.Instance.PlayEffectSound("UI_wuqizhizao_open");
    }

    private void OnOneKeyUpGrade(IList<ItemInfo> listInfo)
    {
        GetItemTipMgr.Instance.ShowItemResult(listInfo);
    }
    protected override void OnShowAnimator()
    {
        base.OnShowAnimator();
        ObjectHelper.AnimationMoveBack(_root,ObjectHelper.direction.left);
        DelayCall(0.8f, () => GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.EnterCondTrigger, NewBieGuide.EnterCondConst.EquipmentOpen));
    }

    public override void Hide()
    {
        base.Hide();
        StopAllEffectSound();
    }
}