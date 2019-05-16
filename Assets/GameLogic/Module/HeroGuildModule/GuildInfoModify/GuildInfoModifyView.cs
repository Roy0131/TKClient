using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public class GuildInfoModifyView : UIBaseView
{
    private Button _closeBtn;
    private Button _dismissBtn;
    private Button _iconBtn;
    private Button _nameBtn;
    private Button _noticeBtn;

    private InputField _noticeInput;
    private Text _nameText;

    private Text _dismissText;
    private Text _dismissBtnText;
    private Image _guildIcon;
    private GuildDataVO vo;
    private ValidateInput _validateInput;

    private GuildIconSelectView _iconSelectView;

    #region modify guild name view logic
    private GameObject _modifyNameRoot;
    private InputField _modifyNameInput;
    private Button _modifyBtn;
    private Button _modifyCloseBtn;
    #endregion
    protected override void ParseComponent()
    {
        base.ParseComponent();
        _closeBtn = Find<Button>("CloseBtn");
        _dismissBtn = Find<Button>("DismissBtn");
        _iconBtn = Find<Button>("IconBtn");
        _nameBtn = Find<Button>("InfoBtn");
        _noticeBtn = Find<Button>("NoticeBtn");

        _noticeInput = Find<InputField>("NoticeInput");
        _nameText = Find<Text>("Img/GuildName");
        _dismissText = Find<Text>("DismissText");
        _dismissBtnText = Find<Text>("DismissBtn/Text");

        _validateInput = new ValidateInput(GameConst.GuildInputText);
        _noticeInput.onValidateInput = _validateInput.OnValidateInput;

        _guildIcon = Find<Image>("GuildIcon");

        _iconSelectView = new GuildIconSelectView(OnSelectIcon);
        _iconSelectView.SetDisplayObject(Find("LogoView"));

        _closeBtn.onClick.Add(Hide);
        _dismissBtn.onClick.Add(OnDismissGuild);
        _iconBtn.onClick.Add(OnShowIcon);
        _nameBtn.onClick.Add(OnShowModifyName);

        #region modify guild name logic
        _modifyNameRoot = Find("ModifyNameView");
        _modifyCloseBtn = Find<Button>("ModifyNameView/ImageBack");
        _modifyNameInput = Find<InputField>("ModifyNameView/NameInput");
        _modifyBtn = Find<Button>("ModifyNameView/SureBtn");

        _modifyBtn.onClick.Add(OnModifyName);
        _modifyCloseBtn.onClick.Add(OnHideModifyView);
        #endregion

        _noticeInput.onValueChanged.Add(delegate { OnInputNotice(); });
        _noticeBtn.onClick.Add(OnModifyNotice);
        ColliderHelper.SetButtonCollider(_closeBtn.transform);
    }

    private void OnModifyNotice()
    {
        if (string.IsNullOrWhiteSpace(_noticeInput.text))
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001150));
            return;
        }
        GameNetMgr.Instance.mGameServer.ReqGuildNoticeModify(_noticeInput.text);
        _noticeBtn.gameObject.SetActive(false);
    }

    private void OnInputNotice()
    {
        if (_noticeInput.text != vo.mNoticeContent)
            _noticeBtn.gameObject.SetActive(true);
        else
            _noticeBtn.gameObject.SetActive(false);
        //if (string.IsNullOrWhiteSpace(_noticeInput.text))
        //{
        //    if (_noticeBtn.gameObject.activeSelf)
        //        _noticeBtn.gameObject.SetActive(false);
        //}
        //else
        //{
        //    if (!_noticeBtn.gameObject.activeSelf)
        //        _noticeBtn.gameObject.SetActive(true);
        //}
    }

    #region modify guild name logic
    private void OnModifyName()
    {
        if (string.IsNullOrWhiteSpace(_modifyNameInput.text))
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001151));
            return;
        }
        if (HeroDataModel.Instance.mHeroInfoData.mDiamond < GameConst.ModifyGuildNameCost)
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000055));
            return;
        }
        TDPostDataMgr.Instance.DoCostDiamond(TDCostDiamondType.BuyGuildNameCount, 1, GameConst.ModifyGuildNameCost);
        GameNetMgr.Instance.mGameServer.ReqGuildInfoModify(_modifyNameInput.text, GuildDataModel.Instance.mGuildDataVO.mLogo);
        OnHideModifyView();
    }

    private void OnHideModifyView()
    {
        _modifyNameRoot.SetActive(false);
    }
    
    private void OnShowModifyName()
    {
        _modifyNameRoot.SetActive(true);
        _modifyNameInput.text = "";
    }
    #endregion

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        vo = GuildDataModel.Instance.mGuildDataVO;
        _nameText.text = vo.mGuildName;
        _guildIcon.sprite = GameResMgr.Instance.LoadGuildIcon(vo.mLogoIcon);
        ObjectHelper.SetSprite(_guildIcon,_guildIcon.sprite);
        _noticeInput.text = vo.mNoticeContent;
        _dismissBtn.gameObject.SetActive(vo.mOfficeType == GuildOfficeType.President);
        RefreshDismissStatu();
    }

    private void RefreshDismissStatu()
    {
        ClearDismissCD();
        GuildDataVO vo = GuildDataModel.Instance.mGuildDataVO;
        if (vo.DisMissRemainTime > 0)
        {
            _dismissBtnText.text = LanguageMgr.GetLanguage(6001152);
            _remainTime = vo.DisMissRemainTime;
            _key = TimerHeap.AddTimer(0, 1000, OnCDTime);
        }
        else
        {
            _dismissText.text = "";
            _dismissBtnText.text = LanguageMgr.GetLanguage(6001153);
            _remainTime = 0;
        }
    }

    private uint _key;
    private int _remainTime;
    private void ClearDismissCD()
    {
        if (_key != 0)
            TimerHeap.DelTimer(_key);
        _key = 0;
    }

    private void OnCDTime()
    {
        if (_remainTime < 0)
        {
            ClearDismissCD();
            return;
        }
        _dismissText.text = TimeHelper.GetCountTime(_remainTime);
        _remainTime--;
    }

    private void OnDismissGuild()
    {
        if (_remainTime > 0)
            GameNetMgr.Instance.mGameServer.ReqCancelDismissGuild();
        else
            GameNetMgr.Instance.mGameServer.ReqDismissGuild();
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GuildDataModel.Instance.AddEvent(GuildEvent.GuildDismissTimeRefresh, RefreshDismissStatu);
        GuildDataModel.Instance.AddEvent(GuildEvent.GuildNoticeRefresh, OnRefreshNotice);
        GuildDataModel.Instance.AddEvent(GuildEvent.GuildNameRefresh, OnRefreshName);
        GuildDataModel.Instance.AddEvent(GuildEvent.GuildIconRefresh, OnRefreshIcon);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GuildDataModel.Instance.RemoveEvent(GuildEvent.GuildDismissTimeRefresh, RefreshDismissStatu);
        GuildDataModel.Instance.RemoveEvent(GuildEvent.GuildNoticeRefresh, OnRefreshNotice);
        GuildDataModel.Instance.RemoveEvent(GuildEvent.GuildNameRefresh, OnRefreshName);
        GuildDataModel.Instance.RemoveEvent(GuildEvent.GuildIconRefresh, OnRefreshIcon);
    }

    private void OnRefreshNotice()
    {
        _noticeInput.text = GuildDataModel.Instance.mGuildDataVO.mNoticeContent;
    }

    private void OnRefreshName()
    {
        _nameText.text = GuildDataModel.Instance.mGuildDataVO.mGuildName;
    }

    private void OnRefreshIcon()
    {
        _guildIcon.sprite = GameResMgr.Instance.LoadGuildIcon(GuildDataModel.Instance.mGuildDataVO.mLogoIcon);
        ObjectHelper.SetSprite(_guildIcon,_guildIcon.sprite);
    }

    private void OnShowIcon()
    {
        _iconSelectView.Show();
    }

    private void OnSelectIcon(GuildMarkConfig config)
    {
        _iconSelectView.Hide();
        GuildDataVO vo = GuildDataModel.Instance.mGuildDataVO;
        if (vo.mLogo == config.ID)
            return;
        GameNetMgr.Instance.mGameServer.ReqGuildInfoModify(vo.mGuildName, config.ID);
    }

    public override void Hide()
    {
        ClearDismissCD();
        base.Hide();
    }

    public override void Dispose()
    {
        ClearDismissCD();
        if (_iconSelectView != null)
        {
            _iconSelectView.Dispose();
            _iconSelectView = null;
        }
        base.Dispose();
    }
}