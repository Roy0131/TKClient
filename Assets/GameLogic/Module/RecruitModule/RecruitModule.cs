using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NewBieGuide;
using Framework.UI;

public class RecruitModule : ModuleBase
{
    private Button _disBtn;
    private GameObject _speciallyObj;
    private UIEffectView _effect;
    private RecruitView _recruitView;

    public RecruitModule()
        : base(ModuleID.Recruit, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_Recruit;
        _soundName = UIModuleSoundName.RecruitSoundName;
        mBlNeedBackMask = true;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();

        _disBtn = Find<Button>("Btn_Back");
        ColliderHelper.SetButtonCollider(_disBtn.transform);
        _speciallyObj = Find("RecruitObj/OpenObj/Img/fx_ui_huangguangtiao");
        _recruitView = new RecruitView();
        _recruitView.SetDisplayObject(Find("RecruitObj"));
        AddChildren(_recruitView);

        _disBtn.onClick.Add(OnClose);
        _effect = CreateUIEffect(_speciallyObj, UILayerSort.WindowSortBeginner);

        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.RecruitBackBtn, _disBtn.transform);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        RecruitDataModel.Instance.AddEvent<int, List<int>, bool>(RecruitEvent.DrawCard, OnDrawCards);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(RecruitEvent.DropOut, OnDrawDrop);
        BagDataModel.Instance.AddEvent<List<int>>(BagEvent.BagItemRefresh, OnBagItemRefresh);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        RecruitDataModel.Instance.RemoveEvent<int, List<int>, bool>(RecruitEvent.DrawCard, OnDrawCards);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(RecruitEvent.DropOut, OnDrawDrop);
        BagDataModel.Instance.RemoveEvent<List<int>>(BagEvent.BagItemRefresh, OnBagItemRefresh);
    }

    private void OnBagItemRefresh(List<int> listid)
    {
        if (listid.Contains(SpecialItemID.Honor))
        {
            if (BagDataModel.Instance.GetItemCountById(SpecialItemID.Honor) >= 1000)
                _effect.PlayEffect();
            else
                _effect.StopEffect();
        }
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        if (BagDataModel.Instance.GetItemCountById(SpecialItemID.Honor) >= 1000)
            _effect.PlayEffect();
        else
            _effect.StopEffect();
        
        DelayCall(0.6f, OnAnimationEnd);
    }

    private void OnAnimationEnd()
    {
        GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.EndCondTrigger, EndConditionConst.RecriutModuleOpen);
    }

    private void OnDrawDrop()
    {
        _disBtn.gameObject.SetActive(true);
    }

    private void OnDrawCards(int drawId, List<int> tabId, bool isFreeDraw)
    {
        _disBtn.gameObject.SetActive(false);
    }

    public override void Dispose()
    {
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.RecruitBackBtn);
        if (_recruitView != null)
        {
            _recruitView.Dispose();
            _recruitView = null;
        }
        if (_effect != null)
        {
            _effect.Dispose();
            _effect = null;
        }
        base.Dispose();
    }

    public override void Hide()
    {
        base.Hide();
        StopAllEffectSound();
    }
}
