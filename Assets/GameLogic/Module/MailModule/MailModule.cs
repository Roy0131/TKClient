using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class MailModule : ModuleBase
{
    private Button _disBtn;

    private MailView _mailView;
    private Transform _root;

    public MailModule()
        : base(ModuleID.Mail, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_Mail;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();

        _disBtn = Find<Button>("Root/Btn_Back");

        _mailView = new MailView();
        _mailView.SetDisplayObject(Find("Root/MailObj"));
        AddChildren(_mailView);

        _disBtn.onClick.Add(OnClose);
        _root = Find<Transform>("Root");

        ColliderHelper.SetButtonCollider(_disBtn.transform, 120, 120);
    }

    protected override void OnShowAnimator()
    {
        base.OnShowAnimator();
        ObjectHelper.PopAnimationLiner(_root);
    }

}
