using Framework.UI;
using UnityEngine.UI;

public class MailSendView : UIBaseView
{
    private Text _idText;
    private InputField _contField;
    private Button _disBtn;
    private Button _replyBtn;

    private ValidateInput _validateInput;

    private int _receiVerId = 0;
    private string _sendName = "";
    private string _mailContent = "";
    private string _mailTitle = "";
    private int _mailType;


    protected override void ParseComponent()
    {
        base.ParseComponent();

        _idText = Find<Text>("ReplyObj/IdText");
        _contField = Find<InputField>("ReplyObj/ContField");
        _disBtn = Find<Button>("ReplyObj/DisBtn");
        _replyBtn = Find<Button>("ReplyObj/ReplyBtn");
        _disBtn.onClick.Add(OnDis);
        _replyBtn.onClick.Add(OnSend);

        _validateInput = new ValidateInput(GameConst.MailInputText);
        _contField.onValidateInput = _validateInput.OnValidateInput;

        ColliderHelper.SetButtonCollider(_disBtn.transform, 120, 120);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        MailDataModel.Instance.AddEvent<int>(MailEvent.SendMail, OnSendMail);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        MailDataModel.Instance.RemoveEvent<int>(MailEvent.SendMail, OnSendMail);
    }

    private void OnSendMail(int mailId)
    {
        PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001166));
        Hide();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _receiVerId = int.Parse(args[0].ToString());
        _sendName = args[1] as string;
        _mailType = int.Parse(args[2].ToString());
        _contField.text = "";
        _idText.text = _sendName;
        if (_mailType == MailTypeConst.PLAYERS)
            _mailTitle = LanguageMgr.GetLanguage(5002803);
        if (_mailType == MailTypeConst.GUILD)
            _mailTitle = LanguageMgr.GetLanguage(6001167);
    }

    private void OnAssMail()
    {
        if (_contField.text != "" && _contField.text != null)
            _mailContent = _contField.text;
    }

    private void OnDis()
    {
        Hide();
    }

    private void OnSend()
    {
        OnAssMail();
        if (_mailContent != "" && _mailContent != null)
            GameNetMgr.Instance.mGameServer.ReqMailSend(_receiVerId, _mailType, _mailTitle, _mailContent);
    }
}
