using Framework.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

public enum SettingType
{
    Registered,//注册
    ModifyPassword,//修改密码
    Switch,//切换账号
}

public class SettingView : UIBaseView
{
    private Text _name;
    private Text _userName;
    private Text _passWord;
    private Image _head;
    private Image _musicImg1;
    private Image _musicImg2;
    private Image _soundImg1;
    private Image _soundImg2;
    private Button _musicBtn1;
    private Button _musicBtn2;
    private Button _soundBtn1;
    private Button _soundBtn2;
    private Button _languageBtn;
    private Button _facebook1Btn;
    private Button _registerBtn;
    private Button _switchBtn;
    private Button _facebook2Btn;
    private Button _privacyBtn;
    private Text _textServerTime;

    private uint _timerKey = 0;
    private int _remainSeconds = 60;
    private int _timeNow;
    private int _timeNowS;
    private int _timeNowM;
    private int _timeNowH;
    private string _timeS;
    private string _timeM;
    private string _timeH;

    private bool isOpenMusic;
    private bool isOpenSound;
    SettingBookView _settingBookView;
    SwitchLanguageView _switchLanguageView;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _name = Find<Text>("SiteHome/Registered/Name");
        _userName = Find<Text>("SiteHome/Registered/UserName");
        _passWord = Find<Text>("SiteHome/Registered/ModifyPassWord/Text");
        _head = Find<Image>("SiteHome/Registered/Head");
        _musicImg1 = Find<Image>("SiteHome/PushPanle/SystemObj/Music/Img1");
        _musicImg2 = Find<Image>("SiteHome/PushPanle/SystemObj/Music/Img2");
        _soundImg1 = Find<Image>("SiteHome/PushPanle/SystemObj/Sound/Img1");
        _soundImg2 = Find<Image>("SiteHome/PushPanle/SystemObj/Sound/Img2");
        _musicBtn1 = Find<Button>("SiteHome/PushPanle/SystemObj/Music/Img1");
        _musicBtn2 = Find<Button>("SiteHome/PushPanle/SystemObj/Music/Img2");
        _soundBtn1 = Find<Button>("SiteHome/PushPanle/SystemObj/Sound/Img1");
        _soundBtn2 = Find<Button>("SiteHome/PushPanle/SystemObj/Sound/Img2");
        _languageBtn = Find<Button>("SiteHome/PushPanle/SystemObj/Language/Image");
        _facebook1Btn = Find<Button>("SiteHome/PushPanle/SystemObj/Facebook/Image");
        _registerBtn = Find<Button>("SiteHome/Registered/ModifyPassWord");
        _switchBtn = Find<Button>("SiteHome/Registered/Switch");
        _facebook2Btn = Find<Button>("SiteHome/Button");

        _privacyBtn = Find<Button>("SiteHome/PrivacyBtn");

        _textServerTime = Find<Text>("SiteHome/ServerTime");

        _settingBookView = new SettingBookView();
        _settingBookView.SetDisplayObject(Find("SettingBook"));

        _switchLanguageView = new SwitchLanguageView();
        _switchLanguageView.SetDisplayObject(Find("Language"));


        _musicBtn1.onClick.Add(OnMusicBtn);
        _musicBtn2.onClick.Add(OnMusicBtn);
        _soundBtn1.onClick.Add(OnSoundBtn);
        _soundBtn2.onClick.Add(OnSoundBtn);
        _languageBtn.onClick.Add(OnLanguageBtn);
        _facebook1Btn.onClick.Add(OnFaceBook1Btn);
        _registerBtn.onClick.Add(OnRegisterBtn);
        _switchBtn.onClick.Add(OnSwitchBtn);
        _facebook2Btn.onClick.Add(OnFaceBook2Btn);
        _privacyBtn.onClick.Add(OnShowPrivacy);
        _privacyBtn.gameObject.SetActive(!GameDriver.Instance.mBlRunHot);
    }

    private void OnShowPrivacy()
    {
        Application.OpenURL("https://lionstudios.cc/privacy/");
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(SettingEvent.SwitchLanguage, OnRefreshLanguage);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(SettingEvent.SwitchLanguage, OnRefreshLanguage);
    }

    private void OnRefreshLanguage()
    {
        if (LocalDataMgr.LoginChannel == GameLoginType.GUEST)
        {
            if (LoginHelper.mBoundAccount != "")
            {
                _passWord.text = LanguageMgr.GetLanguage(5002415);
                _userName.text = LoginHelper.mBoundAccount;
            }
            else
            {
                _passWord.text = LanguageMgr.GetLanguage(5002406);
                _userName.text = LanguageMgr.GetLanguage(6001233);
            }
        }
        else
        {
            _passWord.text = LanguageMgr.GetLanguage(5002415);
            _userName.text = ServerDataModel.Instance.mAcc;
        }
        _switchLanguageView.Hide();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        if (HeroDataModel.Instance.mHeroInfoData == null)
            return;
        OnMusicSoundIcon();
        _name.text = HeroDataModel.Instance.mHeroInfoData.mHeroName;
        OnRefreshLanguage();
        _head.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(HeroDataModel.Instance.mHeroInfoData.mIcon).Icon);
        ObjectHelper.SetSprite(_head,_head.sprite);
        _registerBtn.gameObject.SetActive(LocalDataMgr.LoginChannel != GameLoginType.FACEBOOK);

        _timeNow = HeroDataModel.Instance.mSystemTime;
        _timeNowS = Convert.ToInt32(TimeHelper.GetTimeS(_timeNow).Split(':')[2]);
        _timeNowM = Convert.ToInt32(TimeHelper.GetTimeS(_timeNow).Split(':')[1]);
        _timeNowH = Convert.ToInt32(TimeHelper.GetTimeS(_timeNow).Split(' ')[1].Split(':')[0]);

        DIsTime();
    }
  
    private void OnMusicBtn()
    {
        LocalDataMgr.IsMusic = !LocalDataMgr.IsMusic;
        OnMusicSoundIcon();
        SoundMgr.Instance.OpenOrCloseSound(LocalDataMgr.IsMusic, false);
    }

    private void OnSoundBtn()
    {
        LocalDataMgr.IsSound = !LocalDataMgr.IsSound;
        OnMusicSoundIcon();
        SoundMgr.Instance.OpenOrCloseSound(LocalDataMgr.IsSound, true);
    }

    private void OnMusicSoundIcon()
    {
        _musicImg1.gameObject.SetActive(LocalDataMgr.IsMusic);
        _musicImg2.gameObject.SetActive(!LocalDataMgr.IsMusic);
        _soundImg1.gameObject.SetActive(LocalDataMgr.IsSound);
        _soundImg2.gameObject.SetActive(!LocalDataMgr.IsSound);
    }

    private void OnLanguageBtn()
    {
        _switchLanguageView.Show();
    }

    private void OnFaceBook1Btn()
    {
        if(LocalDataMgr.LoginChannel == GameLoginType.FACEBOOK)
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000135));
            return;
        }
        GameLoginMgr.Instance.BindFBAccount();
    }

    private void OnRegisterBtn()
    {
        if (LocalDataMgr.LoginChannel == GameLoginType.GUEST)
        {
            if (LoginHelper.mBoundAccount != "")
                _settingBookView.Show(SettingType.ModifyPassword);
            else
                _settingBookView.Show(SettingType.Registered);
        }
        else
            _settingBookView.Show(SettingType.ModifyPassword);
    }

    private void OnSwitchBtn()
    {
        _settingBookView.Show(SettingType.Switch);
    }

    private void OnFaceBook2Btn()
    {
        Application.OpenURL("https://www.facebook.com/Idle-Battles-1635110626787952/?modal=admin_todo_tour");
    }
    /// <summary>
    /// 倒计时显示
    /// </summary>
    private void DIsTime()
    {
        _timerKey = TimerHeap.AddTimer(0, 1000, OnTimeCD);
    }
    private void OnTimeCD()
    {
        if (_timeNowS > 58)
        {
            _timeNowS = 0;
            if (_timeNowM > 58)
            {
                _timeNowM = 0;
                if (_timeNowH > 22)
                    _timeNowH = 0;
                else
                    _timeNowH++;
            }
            else
            {
                _timeNowM++;
            }
        }
        else
        {
            _timeNowS++;
        }

        if (_timeNowS < 10)
            _timeS = "0" + _timeNowS;
        else
            _timeS = _timeNowS.ToString();

        if (_timeNowM < 10)
            _timeM = "0" + _timeNowM;
        else
            _timeM = _timeNowM.ToString();

        if (_timeNowH < 10)
            _timeH = "0" + _timeNowH;
        else
            _timeH = _timeNowH.ToString();

        _textServerTime.text = string.Format(LanguageMgr.GetLanguage(5002422), " " + _timeH + ":" + _timeM + ":" + _timeS);
       
    }
    private void ClearRemainTimer()
    {
        if (_timerKey != 0)
            TimerHeap.DelTimer(_timerKey);
        _timerKey = 0;
    }
    public override void Hide()
    {
        ClearRemainTimer();
        base.Hide();
    }
    public override void Dispose()
    {
        ClearRemainTimer();
        base.Dispose();
    }
}
