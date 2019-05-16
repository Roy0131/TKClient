using UnityEngine.UI;
using Framework.UI;

public class HeroGuildView : UIBaseView
{
    private Image _guildIcon;
    private Text _guildNameText;
    private Text _guildIdText;
    private Text _memberText;
    private Text _noticeText;
    private Text _expPerText;
    private Image _expSlider;
    private Text _guildLvText;
    private Text _donate;

    private Button _mailBtn;
    private Button _logBtn;
    private Button _signBtn;
    private Button _setBtn;
    private Button _mgrBtn;
    private Button _askDonateBtn;
    private Button _dominionBtn;
    private Button _help;

    private Text _signText;
    private Text _donateText;

    private int _taskTime;
    private uint _timer = 0;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _guildIcon = Find<Image>("GuildIcon");
        _guildNameText = Find<Text>("GuildName");
        _guildIdText = Find<Text>("GuildIdText");
        _memberText = Find<Text>("MemberText");
        _noticeText = Find<Text>("NoticeText");
        _expPerText = Find<Text>("ExpSlider/ExpText");
        _expSlider = Find<Image>("ExpSlider");
        _guildLvText = Find<Text>("LevelText");
        _donate = Find<Text>("Donate");

        _mailBtn = Find<Button>("Buttons/MailBtn");
        _logBtn = Find<Button>("Buttons/LogBtn");
        _signBtn = Find<Button>("Buttons/SignBtn");
        _setBtn = Find<Button>("Buttons/SetBtn");
        _mgrBtn = Find<Button>("Buttons/MgrBtn");
        _askDonateBtn = Find<Button>("Buttons/DonateBtn");
        _dominionBtn = Find<Button>("Buttons/DominionBtn");
        _help = Find<Button>("Buttons/BtnHelp");

        _signText = Find<Text>("Buttons/SignBtn/Text");
        _donateText = Find<Text>("Buttons/DonateBtn/Text");

        _mailBtn.onClick.Add(OnShowMail);
        _logBtn.onClick.Add(OnShowLog);
        _signBtn.onClick.Add(OnSign);
        _setBtn.onClick.Add(OnShowSetting);
        _mgrBtn.onClick.Add(OnGuildMgr);
        _askDonateBtn.onClick.Add(OnShowDonate);
        _dominionBtn.onClick.Add(OnShowDominion);
        _help.onClick.Add(OnHelp);

        

        RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.Guild, Find("Buttons/SignBtn/RedPoint"));

        ColliderHelper.SetButtonCollider(_setBtn.transform);
        ColliderHelper.SetButtonCollider(_help.transform, 120, 120);
    }

    private int _signLastTime;
    private uint _signKey = 0;

    private void StartSignTime()
    {
        ClearCDTime();
        _signLastTime = GuildDataModel.Instance.mGuildDataVO.SignRemainTime;
        _signKey = TimerHeap.AddTimer(0, 1000, OnShowCDTime);
    }

    private void StartDonateTime()
    {
        ClearDonateTime();
        _taskTime = GuildDataModel.Instance.mGuildDataVO.AskDonateRemainTime;
        _timer = TimerHeap.AddTimer(0, 1000, OnAddTime);
    }

    private void ClearCDTime()
    {
        if (_signKey != 0)
            TimerHeap.DelTimer(_signKey);
        _signKey = 0;
    }

    private void ClearDonateTime()
    {
        if (_timer != 0)
            TimerHeap.DelTimer(_timer);
        _timer = 0;
    }

    private void OnShowCDTime()
    {
        if (_signLastTime < 0)
        {
            ClearCDTime();
            _signText.text = "";
            ObjectHelper.SetEnableStatus(_signBtn, true);
            return;
        }
        _signText.text = TimeHelper.GetCountTime(_signLastTime);
        _signLastTime--;
    }

    private void OnAddTime()
    {
        if (_taskTime <= 0)
        {
            ClearDonateTime();
            _donateText.text = LanguageMgr.GetLanguage(6001156);
            ObjectHelper.SetEnableStatus(_askDonateBtn, true);
            return;
        }
        _donateText.text = TimeHelper.GetCountTime(_taskTime);
        _taskTime--;
    }

    private void OnRefreshGuildData()
    {
        _donate.text = LanguageMgr.GetLanguage(5003151);
        GuildDataVO vo = GuildDataModel.Instance.mGuildDataVO;
        _guildIcon.sprite = GameResMgr.Instance.LoadGuildIcon(vo.mLogoIcon);
        ObjectHelper.SetSprite(_guildIcon,_guildIcon.sprite);
        _guildNameText.text = vo.mGuildName;
        _guildIdText.text =  vo.mGuildId.ToString();
        _noticeText.text = LanguageMgr.GetLanguage(5002416) + ":\n　　" + vo.mNoticeContent;
        _guildLvText.text = vo.mLevel.ToString();
        float flPer = ((float)vo.mExp / (float)vo.mLevelUpConfig.Exp);
        _expSlider.fillAmount = flPer;
        _expPerText.text = (int)(flPer * 100) + "%";

        if (vo.mOfficeType == GuildOfficeType.President || vo.mOfficeType == GuildOfficeType.Office)
        {
            _setBtn.gameObject.SetActive(true);
            //_mgrBtn.gameObject.SetActive(true);
            _mailBtn.interactable = true;
            //_mailBtn.gameObject.SetActive(true);
        }
        else
        {
            _setBtn.gameObject.SetActive(false);
            //_mgrBtn.gameObject.SetActive(false);
            _mailBtn.interactable = false;
            //_mailBtn.gameObject.SetActive(false);
        }
        RefreshSignStatus();
        OnRefreshDonate();
        OnRefreshMember();
    }

    private void OnRefreshMember()
    {
        GuildDataVO vo = GuildDataModel.Instance.mGuildDataVO;
        if (vo.mCurMembers >= vo.mMaxMembers)
            _memberText.text = vo.mMaxMembers + "/" + vo.mMaxMembers;
        else
            _memberText.text = vo.mCurMembers + "/" + vo.mMaxMembers;
    }

    private void RefreshSignStatus()
    {
        GuildDataVO vo = GuildDataModel.Instance.mGuildDataVO;
        if (vo.SignRemainTime > 0)
        {
            StartSignTime();
            ObjectHelper.SetEnableStatus(_signBtn, false);
        }
        else
        {
            _signText.text = "";
            ObjectHelper.SetEnableStatus(_signBtn, true);
        }
        float flPer = ((float)vo.mExp / (float)vo.mLevelUpConfig.Exp);
        _expSlider.fillAmount = flPer;
        _expPerText.text = (int)(flPer * 100) + "%";
    }

    private void OnRefreshDonate()
    {
        GuildDataVO vo = GuildDataModel.Instance.mGuildDataVO;
        if (vo.AskDonateRemainTime > 0)
        {
            StartDonateTime();
            ObjectHelper.SetEnableStatus(_askDonateBtn, false);
        }
        else
        {
            _donateText.text = LanguageMgr.GetLanguage(6001156);
            ObjectHelper.SetEnableStatus(_askDonateBtn, true);
        }
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GuildDataModel.Instance.AddEvent(GuildEvent.GuildDataRefresh, OnRefreshGuildData);
        GuildDataModel.Instance.AddEvent(GuildEvent.GuildSignUpdate, RefreshSignStatus);
        GuildDataModel.Instance.AddEvent(GuildEvent.GuildLevelUp, OnRefreshGuildData);
        GuildDataModel.Instance.AddEvent(GuildEvent.ReqDonateResult, OnRefreshDonate);
        GuildDataModel.Instance.AddEvent(GuildEvent.GuildNoticeRefresh, OnRefreshNotice);
        GuildDataModel.Instance.AddEvent(GuildEvent.GuildNameRefresh, OnRefreshName);
        GuildDataModel.Instance.AddEvent(GuildEvent.GuildIconRefresh, OnRefreshIcon);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<BattleType>(BattleEvent.FightJump, OnShowGuildMap);
        GuildDataModel.Instance.AddEvent(GuildEvent.GuildMemberChange, OnRefreshMember);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GuildDataModel.Instance.RemoveEvent(GuildEvent.GuildDataRefresh, OnRefreshGuildData);
        GuildDataModel.Instance.RemoveEvent(GuildEvent.GuildSignUpdate, RefreshSignStatus);
        GuildDataModel.Instance.RemoveEvent(GuildEvent.GuildLevelUp, OnRefreshGuildData);
        GuildDataModel.Instance.RemoveEvent(GuildEvent.ReqDonateResult, OnRefreshDonate);
        GuildDataModel.Instance.RemoveEvent(GuildEvent.GuildNoticeRefresh, OnRefreshNotice);
        GuildDataModel.Instance.RemoveEvent(GuildEvent.GuildNameRefresh, OnRefreshName);
        GuildDataModel.Instance.RemoveEvent(GuildEvent.GuildIconRefresh, OnRefreshIcon);
        GuildDataModel.Instance.RemoveEvent(GuildEvent.GuildMemberChange, OnRefreshMember);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<BattleType>(BattleEvent.FightJump, OnShowGuildMap);
    }

    private void OnRefreshNotice()
    {
        _noticeText.text = LanguageMgr.GetLanguage(6001221) + "　　" + GuildDataModel.Instance.mGuildDataVO.mNoticeContent;
    }

    private void OnRefreshName()
    {
        _guildNameText.text = GuildDataModel.Instance.mGuildDataVO.mGuildName;
    }

    private void OnRefreshIcon()
    {
        _guildIcon.sprite = GameResMgr.Instance.LoadGuildIcon(GuildDataModel.Instance.mGuildDataVO.mLogoIcon);
        ObjectHelper.SetSprite(_guildIcon,_guildIcon.sprite);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        GuildDataModel.Instance.ReqGuildData();
    }

    public override void Hide()
    {
        base.Hide();
        ClearCDTime();
        ClearDonateTime();
    }

    public override void Dispose()
    {
        ClearCDTime();
        ClearDonateTime();
        base.Dispose();
    }

    private void OnShowMail()
    {
        MailSendMgr.Instance.ShowMailSend(0, LanguageMgr.GetLanguage(6001157), MailTypeConst.GUILD);
    }

    private void OnShowLog()
    {
        GameEventMgr.Instance.mGlobalDispatcher.DispathEvent(GuildEvent.ShowGuildLogView);
    }

    private void OnSign()
    {
        GameNetMgr.Instance.mGameServer.ReqGuildSign();
    }

    private void OnShowSetting()
    {
        GameEventMgr.Instance.mGlobalDispatcher.DispathEvent(GuildEvent.ShowModifyInfoView);
    }

    private void OnGuildMgr()
    {
        GameUIMgr.Instance.OpenModule(ModuleID.MemberMgr);
    }

    private void OnShowDonate()
    {
        GameEventMgr.Instance.mGlobalDispatcher.DispathEvent(GuildEvent.ShowDonateView);
    }

    private void OnShowDominion()
    {
        GameEventMgr.Instance.mGlobalDispatcher.DispathEvent(GuildEvent.ShowGuildMapView);
    }

    private void OnShowGuildMap(BattleType type)
    {
        if (type == BattleType.GuildBoss)
            GameEventMgr.Instance.mGlobalDispatcher.DispathEvent(GuildEvent.ShowGuildMap);
    }

    private void OnHelp()
    {
        HelpTipsMgr.Instance.ShowTIps(HelpType.GuildHelp);
    }
}
