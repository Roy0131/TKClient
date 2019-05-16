using UnityEngine;
using UnityEngine.UI;

public class CTowerBeforeModule : ModuleBase {

    public CTowerBeforeModule() : base(ModuleID.BeforeBattle, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_BeforeBattle;
    }

    private Button _btnClose;
    private GameObject _beforeBattleWindow;
    private GameObject _beforeRightWindow;
    private CTowerBeforeLeftView BeforeLeftWindow;
    private CTowerBeforeRightView BeforeRightWindow;
    protected override void ParseComponent()
    {
        base.ParseComponent();
        _btnClose = Find<Button>("Btn_Back");
        ColliderHelper.SetButtonCollider(_btnClose.transform);
        _beforeBattleWindow = Find("LeftSide");
        _beforeRightWindow = Find("RightSide");

        BeforeLeftWindow = new CTowerBeforeLeftView();
        BeforeLeftWindow.SetDisplayObject(_beforeBattleWindow);
        AddChildren(BeforeLeftWindow);

        BeforeRightWindow = new CTowerBeforeRightView();
        BeforeRightWindow.SetDisplayObject(_beforeRightWindow);
        AddChildren(BeforeRightWindow);


        _btnClose.onClick.Add(OnClose);
    }

    public override void Dispose()
    {
        base.Dispose();
        if (BeforeLeftWindow != null)
        {
            BeforeLeftWindow.Dispose();
            BeforeLeftWindow = null;
        }
        if (BeforeRightWindow != null)
        {
            BeforeRightWindow.Dispose();
            BeforeRightWindow = null;
        }
    }
}
