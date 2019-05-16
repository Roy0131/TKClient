using UnityEngine;
using UnityEngine.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using Framework.UI;
using System;

#region player info ui manager
public class PlayerInfoMgr : Singleton<PlayerInfoMgr>
{

    #region init player info view
    private PlayerInfoView _infoView;
    private void InitView()
    {
        if (_infoView == null)
        {

        }

    }
    #endregion
    public void ShowPlayerInfo(PlayerVO vo)
    {
        //InitView();
        if (_infoView == null)
        {
            Action<GameObject> OnLoaded = (uiObject) =>
            {
                _infoView = new PlayerInfoView();
                _infoView.SetDisplayObject(uiObject);
                GameUIMgr.Instance.AddObjectToTopRoot(_infoView.mRectTransform);
                _infoView.Show(vo);
            };

            GameResMgr.Instance.LoadUIObjectAsync(SingletonResName.UIPlayerInfo, OnLoaded);
        }
        else
        {
            _infoView.Show(vo);
        }
    }

    public void CloseView()
    {
        _infoView.Hide();
    }
}
#endregion

public class PlayerInfoView : UIBaseView
{
    #region player info function logic view
    class PFunctionLogic : UIBaseView
    {
        private int _playerID;
        private PlayerInfoType _type;
        private GuildOfficeType _playerOfficeType;
        private string _name;
        private Button _f1Btn;
        private Text _f1Text;
        private Button _f2Btn;
        private Text _f2Text;
        private Button _f3Btn;
        private Text _f3Text;
        private Button _f4Btn;
        private Text _f4Text;

        protected override void ParseComponent()
        {
            base.ParseComponent();
            _f1Btn = Find<Button>("Fun1Btn");
            _f1Text = Find<Text>("Fun1Btn/Text");

            _f2Btn = Find<Button>("Fun2Btn");
            _f2Text = Find<Text>("Fun2Btn/Text");

            _f3Btn = Find<Button>("Fun3Btn");
            _f3Text = Find<Text>("Fun3Btn/Text");

            _f4Btn = Find<Button>("Fun4Btn");
            _f4Text = Find<Text>("Fun4Btn/Text");

            _f1Btn.onClick.Add(OnFun1Click);
            _f2Btn.onClick.Add(OnFun2Click);
            _f3Btn.onClick.Add(OnFun3Click);
            _f4Btn.onClick.Add(OnFun4Click);
        }

        protected override void AddEvent()
        {
            base.AddEvent();
            FriendDataModel.Instance.AddEvent<List<int>>(FriendEvent.AddFriend, OnAddSuccess);
        }

        protected override void RemoveEvent()
        {
            base.RemoveEvent();
            FriendDataModel.Instance.RemoveEvent<List<int>>(FriendEvent.AddFriend, OnAddSuccess);
        }

        private void OnAddSuccess(List<int> listId)
        {
            if (listId.Count > 0)
                PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000104));
        }

        private void OnFun1Click()
        {
            if (_type == PlayerInfoType.GuildMember)
            {
                GuildDataVO guildVO = GuildDataModel.Instance.mGuildDataVO;
                if (guildVO.mOfficeType == GuildOfficeType.President)
                {
                    if (_playerOfficeType == GuildOfficeType.Member)
                    {
                        //任命官员
                        GuildDataModel.Instance.ReqSetOfficer(_playerID, 1);
                    }
                    else
                    {
                        //罢免官员
                        GuildDataModel.Instance.ReqSetOfficer(_playerID, 2);
                    }
                }
                else
                    MailSendMgr.Instance.ShowMailSend(_playerID, _name);
            }
            else
            {
                if (_type == PlayerInfoType.FriendPlayer)
                    GameNetMgr.Instance.mGameServer.ReqRemoveFriend(_playerID);
                else
                    GameNetMgr.Instance.mGameServer.ReqAskFriend(_playerID);
            }
            OnClose();
        }

        private void OnFun2Click()
        {
            if (_type != PlayerInfoType.GuildMember)
                MailSendMgr.Instance.ShowMailSend(_playerID, _name);
            else
                GuildDataModel.Instance.ReqChangePresident(_playerID);
            OnClose();
        }

        private void OnFun3Click()
        {
            if (_type == PlayerInfoType.GuildMember)
            {
                //驱逐成员
                if (_playerOfficeType == GuildOfficeType.Office)
                {
                    PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001169));
                    return;
                }
                GuildDataModel.Instance.ReqKickMember(_playerID);
            }
            OnClose();
        }

        private void OnFun4Click()
        {
            if (_type == PlayerInfoType.GuildMember)
                MailSendMgr.Instance.ShowMailSend(_playerID, _name);
            OnClose();
        }

        protected override void Refresh(params object[] args)
        {
            base.Refresh(args);
            PlayerVO vo = args[0] as PlayerVO;
            _type = vo.mType;
            _playerID = vo.mPlayerId;
            _name = vo.mPlayerName;
            _playerOfficeType = vo.mGuildOfficeType;
            _f1Btn.gameObject.SetActive(false);
            _f2Btn.gameObject.SetActive(false);
            _f3Btn.gameObject.SetActive(false);
            _f4Btn.gameObject.SetActive(false);
            switch (_type)
            {
                case PlayerInfoType.GuildMember:

                    GuildDataVO guildVO = GuildDataModel.Instance.mGuildDataVO;
                    if (guildVO.mOfficeType == GuildOfficeType.President)
                    {
                        _f1Btn.gameObject.SetActive(true);
                        _f2Btn.gameObject.SetActive(true);
                        _f3Btn.gameObject.SetActive(true);
                        _f4Btn.gameObject.SetActive(true);
                        _f1Text.text = _playerOfficeType == GuildOfficeType.Office ? LanguageMgr.GetLanguage(6001170) : LanguageMgr.GetLanguage(5003124);
                        _f2Text.text = LanguageMgr.GetLanguage(5003125);
                        _f3Text.text = LanguageMgr.GetLanguage(6001171);
                        _f4Text.text = LanguageMgr.GetLanguage(6001172);
                    }
                    else
                    {
                        _f1Btn.gameObject.SetActive(true);
                        _f1Text.text = LanguageMgr.GetLanguage(6001172);
                    }
                    break;
                case PlayerInfoType.FriendOther:
                case PlayerInfoType.FriendPlayer:
                    _f1Btn.gameObject.SetActive(true);
                    _f2Btn.gameObject.SetActive(true);
                    if (_type == PlayerInfoType.FriendPlayer)
                        _f1Text.text = LanguageMgr.GetLanguage(5001503);
                    else
                        _f1Text.text = LanguageMgr.GetLanguage(6001173);
                    _f2Text.text = LanguageMgr.GetLanguage(6001172);
                    break;
            }
        }

        private void OnClose()
        {
            PlayerInfoMgr.Instance.CloseView();
        }
    }
    #endregion

    #region normal logic
    private Text _nameText;
    private Text _playerIdText;
    private Text _guildText;
    private Text _levelText;
    private Text _powerText;
    private Image _playerIcon;

    private Button _closeBtn;
    private List<RectTransform> _lstCardRoot;
    private List<CardView> _lstDefenseCards;
    
    private RectTransform _backGroundImage;
    private Text _delBtnText;    
    private PFunctionLogic _funcLogic;
    private Transform _root;

    protected override void ParseComponent()
    {
        base.ParseComponent();

        _nameText = Find<Text>("Root/PlayerRoot/NameText");
        _playerIdText = Find<Text>("Root/PlayerRoot/IDText");
        _guildText = Find<Text>("Root/PlayerRoot/GuildText");
        _levelText = Find<Text>("Root/PlayerRoot/LevelText");
        _powerText = Find<Text>("Root/PlayerRoot/PowerText");

        _playerIcon = Find<Image>("Root/PlayerRoot/PlayerIcon");

        _lstCardRoot = new List<RectTransform>();
        for (int i = 0; i < 9; i++)
            _lstCardRoot.Add(Find<RectTransform>("Root/CardRoot/Card" + i));
        
        _backGroundImage = Find<RectTransform>("Root/StaticObject/BackGround");
        _delBtnText = Find<Text>("Root/FunButtons/DeleteBtn/Text");

        _closeBtn = Find<Button>("Root/BtnClose");
        _closeBtn.onClick.Add(OnClose);
        
        _funcLogic = new PFunctionLogic();
        _funcLogic.SetDisplayObject(Find("Root/FunButtons"));

        _root = Find<Transform>("Root");
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        PlayerVO vo = args[0] as PlayerVO;
        _nameText.text = vo.mPlayerName;
        _levelText.text = vo.mPlayerLevel.ToString();
        _guildText.text = vo.mGuildName;
        _powerText.text = vo.mBattlePower.ToString();
        _playerIdText.text = vo.mPlayerId.ToString();
        if (vo.mPlayerIcon > 0)
        {
            _playerIcon.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(vo.mPlayerIcon).Icon);
            ObjectHelper.SetSprite(_playerIcon,_playerIcon.sprite);
        }
        else
            _playerIcon.sprite = null;

            ClearDefenseCards();
        _lstDefenseCards = new List<CardView>();

        PlayerTeamRole roleData;
        CardView cardView;
        CardDataVO cardvo;
        for (int i = 0; i < 9; i++)
        {
            roleData = vo.GetPlayerTeamRole(i);
            if (roleData == null)
                continue;
            cardvo = new CardDataVO(roleData.TableId, roleData.Rank, roleData.Level);
            cardView = CardViewFactory.Instance.CreateCardView(cardvo, CardViewType.Common);
            GameUIMgr.Instance.ChildAddToParent(cardView.mRectTransform, _lstCardRoot[i]);
            _lstDefenseCards.Add(cardView);
        }
        if (vo.mType == PlayerInfoType.Normal)
        {
            _backGroundImage.sizeDelta = new Vector2(810f, 450f);
            _funcLogic.Hide();
        }
        else
        {
            _backGroundImage.sizeDelta = new Vector2(810f, 540f);
            _funcLogic.Show(vo);
        }
    }

    private void ClearDefenseCards()
    {
        if (_lstDefenseCards == null)
            return;
        for (int i = 0; i < _lstDefenseCards.Count; i++)
            CardViewFactory.Instance.ReturnCardView(_lstDefenseCards[i]);
        _lstDefenseCards.Clear();
        _lstDefenseCards = null;
    }

    public override void Dispose()
    {
        ClearDefenseCards();
        if (_lstCardRoot != null)
        {
            _lstCardRoot.Clear();
            _lstCardRoot = null;
        }
        if (_funcLogic != null)
        {
            _funcLogic.Dispose();
            _funcLogic = null;
        }
        base.Dispose();
    }

    private void OnClose()
    {
        PlayerInfoMgr.Instance.CloseView();
    }
    protected override void OnShowViewAnimation()
    {
        base.OnShowViewAnimation();
        ObjectHelper.PopAnimationLiner(_root);
    }
    #endregion
}