using UnityEngine.UI;
using Framework.UI;

public class MemberRecruitView : UIBaseView
{
    private InputField _inputField;
    private Button _sendBtn;
    private Button _closeBtn;
    private ValidateInput _validateInput;
    private Text _textTitle;
    protected override void ParseComponent()
    {
        base.ParseComponent();
        _inputField = Find<InputField>("InputField");
        _sendBtn = Find<Button>("SendBtn");
        _closeBtn = Find<Button>("CloseBtn");
        _textTitle = Find<Text>("StaticRoot/Text");

        _validateInput = new ValidateInput(GameConst.GuildInputText);
        _inputField.onValidateInput = _validateInput.OnValidateInput;

        _sendBtn.onClick.Add(OnSend);
        _closeBtn.onClick.Add(Hide);

        ColliderHelper.SetButtonCollider(_closeBtn.transform);
    }
    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _textTitle.text = LanguageMgr.GetLanguage(5003177);
    }
    private void OnSend()
    {
        string value = _inputField.text;
        if (string.IsNullOrWhiteSpace(value))
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001159));
            return;
        }
        if (value.Length > 50)
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001160));
            return;
        }
        GameNetMgr.Instance.mGameServer.ReqGuildRecruit(value);
    }

    public override void Show(params object[] args)
    {
        base.Show(args);
        _inputField.text = "";
    }


}