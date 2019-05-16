using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class LanguageItemView : UIBaseView
{
    private Text _languageName;
    private Button _img1Btn;
    private Button _img2Btn;
    private GameObject _img2;

    private string _languageTitle;
    private SystemLanguage _systemLanguage;


    public LanguageItemView(SystemLanguage systemLanguage)
    {
        _systemLanguage = systemLanguage;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _languageName = Find<Text>("LanguageName");
        _img1Btn = Find<Button>("Img1");
        _img2 = Find("Img2");

        _img1Btn.onClick.Add(OnSwitch);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _languageName.text = args[0].ToString();
        //_img1Btn.gameObject.SetActive(_systemLanguage != LocalDataMgr.CurLanguage);
        //_img2.SetActive(_systemLanguage == LocalDataMgr.CurLanguage);
        if (LocalDataMgr.CurLanguage == SystemLanguage.English)
        {
            _img1Btn.gameObject.SetActive(_systemLanguage != LocalDataMgr.CurLanguage);
            _img2.SetActive(_systemLanguage == LocalDataMgr.CurLanguage);
        }
        else
        {
            _img1Btn.gameObject.SetActive(_systemLanguage == SystemLanguage.English);
            _img2.SetActive(_systemLanguage != SystemLanguage.English);
        }
    }

    private void OnSwitch()
    {
        LocationMgr.Instance.SwitchLanguage(_systemLanguage);
        //LocalDataMgr.CurLanguage = _systemLanguage;
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(SettingEvent.SwitchLanguage);
    }
}
