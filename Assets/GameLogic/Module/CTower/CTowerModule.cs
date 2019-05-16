using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CTowerModule : ModuleBase {

    public CTowerModule() : base(ModuleID.CTower, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_CTower;
        _soundName = UIModuleSoundName.CTowerSoundName;
        mBlNeedBackMask = true;
    }
    private bool _refreshPos ;
    private Button _closeButton;
    private CTowerView _cTowerView;
    private CTowerRHTView _cTowerRHTView;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _closeButton = Find<Button>("Btn_Back");
        ColliderHelper.SetButtonCollider(_closeButton.transform);
        _cTowerView = new CTowerView();
        _cTowerView.SetDisplayObject(Find("TowerLevel"));
        AddChildren(_cTowerView);

        _cTowerRHTView = new CTowerRHTView();
        _cTowerRHTView.SetDisplayObject(Find("TicHelpRank"));
        AddChildren(_cTowerRHTView);

        _closeButton.onClick.Add(OnClose);
    }

    public override void Dispose()
    {
        if (_cTowerView != null)
        {
            _cTowerView.Dispose();
            _cTowerView = null;
        }
        if (_cTowerRHTView != null)
        {
            _cTowerRHTView.Dispose();
            _cTowerRHTView = null;
        }
        base.Dispose();
    }

    public override void Hide()
    {
        base.Hide();

        StopAllEffectSound();
    }

    protected override void OnClose()
    {
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(CTowerEvent.ClearTowerTimeHeap);
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(CTowerEvent.SetTowerPos);
        base.OnClose();
    }
}


