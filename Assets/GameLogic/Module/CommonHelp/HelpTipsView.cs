using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class HelpTipsView : UIBaseView {

    private Button _btnClose;
    private Text _helpText;
    private Transform _root;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _btnClose = Find<Button>("Root/ButtonClose");
        _helpText = Find<Text>("Root/ScrollView/Text");
        _root = Find<Transform>("Root");
        _btnClose.onClick.Add(HelpTipsMgr.Instance.HideTips);

        ColliderHelper.SetButtonCollider(_btnClose.transform);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        DisHelp();
    }

    private void DisHelp()
    {
        _helpText.text = LanguageMgr.GetLanguage(HelpTipsMgr.Instance.descrptionID);
    }
    protected override void OnShowViewAnimation()
    {
        base.OnShowViewAnimation();
        ObjectHelper.PopAnimationLiner(_root);
    }
}
