using Framework.UI;
using UnityEngine.UI;
using System;

public class ConfirmTipsView : UIBaseView
{
    private bool _blShowAgain;
    private Text _textContent;
    private Toggle _toggle;
    private Button _btnNo;
    private Button _btnYes;
    private Action<bool,bool> _callBack;

    private bool isTog;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _btnNo = Find<Button>("BtnGrid/ButtonNo");
        _btnYes = Find<Button>("BtnGrid/ButtonYes");
        _textContent = Find<Text>("TextDescription");
        _toggle = Find<Toggle>("Toggle");

        _btnNo.onClick.Add(ClickNo);
        _btnYes.onClick.Add(ClickYes);

        _toggle.onValueChanged.Add((bool blSelect) => { if (blSelect) OnTogChange(); });
    }

    private void OnTogChange()
    {
        isTog = true;
    }

    private void ClickYes()
    {
        OnResult(true);
    }

    public void SetParam(Action<bool, bool> callBack, bool showAgain = false)
    {
        _callBack = callBack;
        _blShowAgain = showAgain;
        _toggle.isOn = false;
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _textContent.text = args[0].ToString();
        _toggle.gameObject.SetActive(_blShowAgain);
    }

    private void ClickNo()
    {
        OnResult(false);
    }

    private void OnResult(bool value)
    {
        if (_callBack != null)
            _callBack.Invoke(value, _toggle.isOn);
        _callBack = null;
        if (value && isTog)
            LocalDataMgr.ArenaCancelBattleAlert = false;
        ConfirmTipsMgr.Instance.HideConfirmTips();
    }
}
