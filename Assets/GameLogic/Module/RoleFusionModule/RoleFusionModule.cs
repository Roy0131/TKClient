using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Framework.UI;

public class RoleFusionModule : ModuleBase
{
    private FusionLogicView _logicView;
    private List<RoleFusionItem> _lstRoleFusionItems;
    private Button _backBtn;
    private Button _helpBtn;
    private Transform _root01;
    private Transform _root02;
    public RoleFusionModule()
        : base(ModuleID.RoleFusion, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_RoleFusion;
        _soundName = UIModuleSoundName.RoleFusionSoundName;
        mBlNeedBackMask = true;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _logicView = new FusionLogicView(true);
        _logicView.SetDisplayObject(Find("FusionRoot"));

        GameObject itemObj = Find("ImageBack/ScrollView/Content/RoleItem");
        RectTransform parent = Find<RectTransform>("ImageBack/ScrollView/Content");
        _root01 = Find<Transform>("ImageBack");
        _root02 = Find<Transform>("StaticTextRoot");
     
        RoleFusionItem item;
        GameObject obj;
        _lstRoleFusionItems = new List<RoleFusionItem>();
        for (int i = 1; i <= 9; i++)
        {
            obj = GameObject.Instantiate(itemObj);
            obj.transform.SetParent(parent, false);
            item = new RoleFusionItem();
            item.mOnClickMethod = OnSelectFusionItem;
            item.SetDisplayObject(obj);
            item.Show(i);
            _lstRoleFusionItems.Add(item);
            RedPointTipsMgr.Instance.ChildNodeBindObject(item.mFusionId, RedPointEnum.RoleFusion, item.mRedPointObject);
        }

        _backBtn = Find<Button>("Buttons/Btn_Back");
        ColliderHelper.SetButtonCollider(_backBtn.transform);
        _helpBtn = Find<Button>("Buttons/BtnHelp");
        ColliderHelper.SetButtonCollider(_helpBtn.transform);
        _backBtn.onClick.Add(OnClose);
        _helpBtn.onClick.Add(OnHelp);

        ColliderHelper.SetButtonCollider(_backBtn.transform);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        OnSelectFusionItem(1);
    }

    private void OnSelectFusionItem(int fusionId)
    {
        for (int i = 0; i < _lstRoleFusionItems.Count; i++)
            _lstRoleFusionItems[i].BlSelected = _lstRoleFusionItems[i].mFusionId == fusionId;
        _logicView.Show(fusionId);
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(FusionEvent.CreateCommonRole);
    }

    public override void Hide()
    {
        base.Hide();
        if (_logicView != null)
            _logicView.Hide();

        StopAllEffectSound();
    }

    private void OnHelp()
    {
        HelpTipsMgr.Instance.ShowTIps(HelpType.HeroComHelp);
    }

    public override void Dispose()
    {
        if (_lstRoleFusionItems != null)
        {
            for (int i = 0; i < _lstRoleFusionItems.Count; i++)
            {
                RedPointTipsMgr.Instance.ChildNodeUnBindObject(_lstRoleFusionItems[i].mFusionId, RedPointEnum.RoleFusion, _lstRoleFusionItems[i].mRedPointObject);
                _lstRoleFusionItems[i].Dispose();
            }
            _lstRoleFusionItems.Clear();
            _lstRoleFusionItems = null;
        }
        if (_logicView != null)
        {
            _logicView.Dispose();
            _logicView = null;
        }
        base.Dispose();
    }
    protected override void OnShowAnimator()
    {
        base.OnShowAnimator();


        ObjectHelper.AnimationMoveBack(_root01,ObjectHelper.direction.left);


        ObjectHelper.AnimationMoveLiner(_root02,ObjectHelper.direction.down);
    }
}