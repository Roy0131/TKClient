using Framework.UI;
using Msg.ClientMessage;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class SettingBookView : UIBaseView
{
    private SettingType _settingType;
    private Text _nameText;
    private Text _nameText1;
    private Text _nameText2;
    private Text _nameText3;
    private InputField _inputField1;
    private InputField _inputField2;
    private InputField _inputField3;
    private Button _disBtn;
    private Button _cancelBtn;
    private Button _deteBtn;
    private RectTransform _wPanle;
    private RectTransform _nPanle;
    private RectTransform _cancelRect;
    private RectTransform _deteRect;

    private Regex re = new Regex(@"[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?");

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _nameText = Find<Text>("Text");
        _nameText1 = Find<Text>("Text1");
        _nameText2 = Find<Text>("Text2");
        _nameText3 = Find<Text>("Text3");
        _inputField1 = Find<InputField>("InputField1");
        _inputField2 = Find<InputField>("InputField2");
        _inputField3 = Find<InputField>("InputField3");
        _disBtn = Find<Button>("Btn_Back");
        _cancelBtn = Find<Button>("Cancel");
        _deteBtn = Find<Button>("Dete");
        _wPanle = Find<RectTransform>("WPanle");
        _nPanle = Find<RectTransform>("NPanle");
        _cancelRect = Find<RectTransform>("Cancel");
        _deteRect = Find<RectTransform>("Dete");

        _disBtn.onClick.Add(OnDisBtn);
        _cancelBtn.onClick.Add(OnDisBtn);
        _deteBtn.onClick.Add(OnDeteBtn);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _settingType = (SettingType)args[0];
        SettingChang();
    }

    private void SettingChang()
    {
        if (_settingType== SettingType.Registered)
        {
            _nameText.text = LanguageMgr.GetLanguage(5002433);
            _nameText1.text = LanguageMgr.GetLanguage(5002409);
            _nameText2.text = LanguageMgr.GetLanguage(5002410);
            _nameText3.text = LanguageMgr.GetLanguage(5002411);
            _wPanle.sizeDelta = new Vector2(660f, 570f);
            _wPanle.anchoredPosition = new Vector2(0f, 0f);
            _nPanle.sizeDelta = new Vector2(600f, 370f);
            _nPanle.anchoredPosition = new Vector2(0f, 0f);
            _cancelRect.anchoredPosition = new Vector2(-150f, -232f);
            _deteRect.anchoredPosition = new Vector2(150f, -232f);
            _inputField1.contentType = InputField.ContentType.Standard;
            _inputField2.contentType = InputField.ContentType.Password;
            _inputField3.contentType = InputField.ContentType.Password;
        }
        else if(_settingType == SettingType.ModifyPassword)
        {
            _nameText.text = LanguageMgr.GetLanguage(5002415);
            _nameText1.text = LanguageMgr.GetLanguage(5002412);
            _nameText2.text = LanguageMgr.GetLanguage(5002413);
            _nameText3.text = LanguageMgr.GetLanguage(5002411);
            _wPanle.sizeDelta = new Vector2(660f, 570f);
            _wPanle.anchoredPosition = new Vector2(0f, 0f);
            _nPanle.sizeDelta = new Vector2(600f, 370f);
            _nPanle.anchoredPosition = new Vector2(0f, 0f);
            _cancelRect.anchoredPosition = new Vector2(-150f, -232f);
            _deteRect.anchoredPosition = new Vector2(150f, -232f);
            _inputField1.contentType = InputField.ContentType.Password;
            _inputField2.contentType = InputField.ContentType.Password;
            _inputField3.contentType = InputField.ContentType.Password;
        }
        else if(_settingType == SettingType.Switch)
        {
            _nameText.text = LanguageMgr.GetLanguage(5002434);
            _nameText1.text = LanguageMgr.GetLanguage(5002409);
            _nameText2.text = LanguageMgr.GetLanguage(5002410);
            _wPanle.sizeDelta = new Vector2(660f, 470f);
            _wPanle.anchoredPosition = new Vector2(0f, 50f);
            _nPanle.sizeDelta = new Vector2(600f, 270f);
            _nPanle.anchoredPosition = new Vector2(0f, 50f);
            _cancelRect.anchoredPosition = new Vector2(-150f, -132f);
            _deteRect.anchoredPosition = new Vector2(150f, -132f);
            _inputField1.contentType = InputField.ContentType.Standard;
            _inputField2.contentType = InputField.ContentType.Password;
        }
        _inputField1.text = "";
        _inputField2.text = "";
        _inputField3.text = "";

        _nameText3.gameObject.SetActive(_settingType == SettingType.Registered || _settingType == SettingType.ModifyPassword);
        _inputField3.gameObject.SetActive(_settingType == SettingType.Registered || _settingType == SettingType.ModifyPassword);
    }

    private void OnDeteBtn()
    {
        if (_settingType == SettingType.Registered)
        {
            if (re.IsMatch(_inputField1.text))
            {
                if (_inputField2.text == _inputField3.text && _inputField2.text.Length >= 6 && _inputField2.text.Length <= 16)
                    LoginHelper.BindNewAccount(LocalDataMgr.PlayerAccount, LocalDataMgr.Password, _inputField1.text, _inputField3.text, OnRegistResult, GameLoginType.INPUTACCOUNT);
                else
                    PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001230));
            }
            else
            {
                PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001228));
            }
        }
        else if (_settingType == SettingType.ModifyPassword)
        {
            if (_inputField1.text.Length >= 6 && _inputField1.text.Length <= 16 && _inputField2.text.Length >= 6 && _inputField2.text.Length <= 16 && _inputField2.text == _inputField3.text)
                LoginHelper.SetPassword(_inputField1.text, _inputField2.text, OnMethod);
            else
                PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001229));
        }
        else if (_settingType == SettingType.Switch)
        {
            if (re.IsMatch(_inputField1.text))
            {
                if (_inputField2.text.Length >= 6 && _inputField2.text.Length <= 16 && _inputField1.text != LocalDataMgr.PlayerAccount)
                {
                    LoginHelper.ReLogin(_inputField1.text, _inputField2.text, GameLoginType.INPUTACCOUNT);
                    OnDisBtn();
                }
                else
                {
                    PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001230));
                }
            }
            else
            {
                PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001228));
            }
        }
    }

    private void OnMethod(int code, S2CSetLoginPasswordResponse value)
    {
        if(code == 0)
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001231));
            LocalDataMgr.Password = value.NewPassword;
        }
        else
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000105));
        }
        OnDisBtn();
    }

    private void OnRegistResult(int code, S2CGuestBindNewAccountResponse value)
    {
        if (code == 0)
        {
            OnDisBtn();
            LoginHelper.ReLogin(value.NewAccount, value.NewPassword, GameLoginType.INPUTACCOUNT);
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001232));
            //GameUIMgr.Instance.CloseModule(ModuleID.Setting);
        }
        else
        {
            NetErrorHelper.DoErrorCode(code);
        }

    }

    private void OnDisBtn()
    {
        Hide();
    }
}
