using System;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;
using LitJson;

public class ChatItemView : UIBaseView
{
    private ChatItemDataVO _vo;
    private Text _textName;
    private Text _textTime;
    private Text _textContent;
    private Button _nameBtn;
    private Button _roleBtn;
    private GameObject _objBackImg1;
    private GameObject _objBackImg2;
    private Text _textContentLong;
    private Button _joinGuildBtn;
    private GameObject _joinBack;
    private GameObject _levelObj;
    //private GameObject _joinGuild;
    private Text _textLevel;

    private Image _imgRole;

    private Button _btnOtherInfo;
    public bool mBlHeroChat { get; set; }

    private CardDataVO _cardVO;
    private Text _textContentRe;

    private bool _isName = false;
    private bool _isTranslateRequest = true;
    private string _enText = "";
    private string _cnText = "";

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _textName = Find<Text>("uiCardItem/TextName/TextDes");
        _textTime = Find<Text>("uiCardItem/TextTime");
        _textContent = Find<Text>("ImageBack1/TextDes");
        _nameBtn = Find<Button>("uiCardItem/TextName/Btn");
        _roleBtn = Find<Button>("ImageBack1/TextDes");
        _textContentLong = Find<Text>("ImageBack2/TextDes");
        _objBackImg1 = Find("ImageBack1");
        _objBackImg2 = Find("ImageBack2");
        _joinGuildBtn = Find<Button>("ImageBackJoin/Button");
        _joinBack = Find("ImageBackJoin");
        //_joinGuild = Find("JoinGuild");
        _btnOtherInfo = Find<Button>("uiCardItem/Collider");

        _imgRole = Find<Image>("uiCardItem/ImageHeadMask/ImageHead");
        _textLevel = Find<Text>("uiCardItem/ImageLevelBack/TextLevel");
        _textContentRe = Find<Text>("ImageBackJoin/TextContent");
        _levelObj = Find("uiCardItem/ImageLevelBack");

        _joinGuildBtn.onClick.Add(OnJoinGuild);
        _btnOtherInfo.onClick.Add(OnDisInfo);
        _roleBtn.onClick.Add(OnRole);
        _nameBtn.onClick.Add(OnNameBtn);
    }

    private void OnNameBtn()
    {
        if(_isTranslateRequest)
        {
            TranslateRequest req1 = new TranslateRequest((string obj) => {
                JsonData allMonsters = JsonMapper.ToObject(obj.ToString());
                _enText = allMonsters["text"].ToString();
            });
            req1.StartSend(_vo.mContent, "EN");

            TranslateRequest req2 = new TranslateRequest((string obj) => {
                JsonData allMonsters = JsonMapper.ToObject(obj.ToString());
                _cnText = allMonsters["text"].ToString();
                OnTextContent();
                _isTranslateRequest = false;
            });
            req2.StartSend(_vo.mContent, "CN");
        }
        else
        {
            OnTextContent();
        }
    }

    private void OnTextContent()
    {
        if (_isName)
            _textContent.text = _enText;
        else
            _textContent.text = _cnText;
        _isName = !_isName;
    }

    private void OnDisInfo()
    {
        if (_vo.mPlayerId == HeroDataModel.Instance.mHeroPlayerId)
            return;
        PlayerVO vo = new PlayerVO(_vo.mPlayerId, PlayerInfoType.FriendOther);
        //vo.mGuildOfficeType = _vo.mOfficeType;
        PlayerInfoDataModel.Instance.ShowPlayerInfo(vo);
    }
    private void OnJoinGuild()
    {
        if (HeroDataModel.Instance.mHeroInfoData.mGuildId > 0)
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001121));
            return;
        }
        GameUIMgr.Instance.OpenModule(ModuleID.Guild, UIGuildType.Search);
        GameNetMgr.Instance.mGameServer.ReqSearchGuild(_vo.mExtraValue.ToString());
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        if (LocalDataMgr.CurLanguage == SystemLanguage.English)
            _isName = true;
        else
            _isName = false;
        _vo = args[0] as ChatItemDataVO;
        _levelObj.SetActive(_vo.mChatChannel != ChatChannelConst.Recruit);
        mBlHeroChat = _vo.mPlayerId == HeroDataModel.Instance.mHeroPlayerId;
        InitData();
    }

    private void InitData()
    {
        //Debuger.Log("创建别人的对话数据");
        InitTimeData();

        if (_vo.mPlayerHead > 0)
        {
            _imgRole.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(_vo.mPlayerHead).Icon);
            ObjectHelper.SetSprite(_imgRole,_imgRole.sprite);
        }
        else
            _imgRole.sprite = null;
        _textLevel.text ="Lv"+ _vo.mPlayerLevel;
        if (_vo.mChatChannel == ChatChannelConst.Recruit)
        {
            _nameBtn.gameObject.SetActive(false);
            _objBackImg1.SetActive(false);
            _objBackImg2.SetActive(false);
            _joinBack.SetActive(true);
            //_joinGuildBtn.gameObject.SetActive(true);
            if (mBlHeroChat)
            {
                _textName.text = "[Lv:" + HeroDataModel.Instance.mHeroInfoData.mGuildLevel + "." + HeroDataModel.Instance.mHeroInfoData.mGuildName + "]";
            }
            else
            {
                _textName.text = "[Lv:" + _vo.mPlayerLevel + "." + _vo.mPlayerName + "]";
            }

            string content = _vo.mContent;
            content = content.Length > 30 ? content.Insert(30, "\n") : content;
            _textContentRe.text = content;//_vo.mContent;
        }
        else
        {
            _joinBack.SetActive(false);
            //_joinGuildBtn.gameObject.SetActive(false);
            _textName.text = _vo.mPlayerName;
            string[] content = _vo.mContent.Split('*');
            if (content.Length % 7 == 0)
            {
                _nameBtn.gameObject.SetActive(false);
                _cardVO = new CardDataVO(int.Parse(content[0]), int.Parse(content[1]), int.Parse(content[2]));
                _cardVO.OnBattlePower(int.Parse(content[3]));
                _cardVO.OnDictAttris(AttributesType.HP, int.Parse(content[4]));
                _cardVO.OnDictAttris(AttributesType.ATTACK, int.Parse(content[5]));
                _cardVO.OnDictAttris(AttributesType.DEFENSE, int.Parse(content[6]));
                _textContent.text = "[<color=#FF0000>" + LanguageMgr.GetLanguage(GameConfigMgr.Instance.GetCardConfig(int.Parse(content[0]) * 100 + 1).Name) + "</color>]";
            }
            else
            {
                _nameBtn.gameObject.SetActive(_vo.mPlayerId != HeroDataModel.Instance.mHeroPlayerId);
                _textContent.text = _vo.mContent;
            }
            SetBack(System.Text.Encoding.UTF8.GetBytes(_vo.mContent).Length);
        }
    }

    private void OnRole()
    {
        if (_cardVO != null)
        {
            RoleVO roleVO = new RoleVO();
            roleVO.OnCardVO(_cardVO);
            roleVO.OnDetailType(CardDetailConst.ChatDetail);
            GameUIMgr.Instance.OpenModule(ModuleID.RoleInfo, roleVO);
        }
    }

    private void InitTimeData()
    {
        string[] _timeData = TimeHelper.GetTime(Convert.ToInt32(_vo.mSendTime)).Split();
        string _md = _timeData[0].Split('-')[1] + "-" + _timeData[0].Split('-')[2];//月日
        string _hm = _timeData[1];//时分

        _textTime.text = "[" + _hm + " " + _md + "]";
    }

    private void SetBack(float width)
    {
        if (width > 45)
        {
            _objBackImg1.SetActive(false);
            _objBackImg2.SetActive(true);
            _textContentLong.text = _vo.mContent;
        }
        else
        {
            _objBackImg1.SetActive(true);
            _objBackImg2.SetActive(false);
        }
    }

    public override void Hide()
    {
        _cardVO = null;
        base.Hide();
    }

    public override void Dispose()
    {
        _cardVO = null;
        base.Dispose();
    }
}
