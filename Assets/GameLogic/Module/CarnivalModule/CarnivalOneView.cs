using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using IHLogic;
using UnityEngine;
using UnityEngine.UI;

public class CarnivalOneView : UIBaseView
{
    private int _activeType;
    private List<CarnivalDataVO> _listVO;
    private Text _text1;
    private Text _text2;
    private Text _text3;
    private Text _butText;
    private Text _time;
    private Text _title;
    private Button _but;
    private GameObject _bjObj1;
    private GameObject _bjObj2;
    private GameObject _bjObj3;
    private GameObject _bjObj4;
    private GameObject _invitationSystem;
    private RectTransform _rect;
    private InputField _invitation;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _text1 = Find<Text>("Img/Text1");
        _text2 = Find<Text>("Img/Text2");
        _text3 = Find<Text>("Img/Text3");
        _butText = Find<Text>("Buy/Text");
        _time = Find<Text>("Time");
        _title = Find<Text>("InvitationSystem/Con");
        _but = Find<Button>("Buy");
        _invitation = Find<InputField>("InvitationSystem/InputField");
        _bjObj1 = Find("Img/Img1");
        _bjObj2 = Find("Img/Img2");
        _bjObj3 = Find("Img/Img3");
        _bjObj4 = Find("Img/Img4");
        _invitationSystem = Find("InvitationSystem");
        _rect = Find<RectTransform>("Reward");

        _but.onClick.Add(OnBut);
        _activeType = 0;
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        CarnivalDataModel.Instance.AddEvent(CarnivalEvent.CarnivalBeInvited, OnCarnivalBeInvited);
        CarnivalDataModel.Instance.AddEvent<int>(CarnivalEvent.CarnivalTaskSet, OnCarnivalTaskSet);
        CarnivalDataModel.Instance.AddEvent<int>(CarnivalEvent.CarnivalNotify, OnCarnivalNotify);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        CarnivalDataModel.Instance.RemoveEvent(CarnivalEvent.CarnivalBeInvited, OnCarnivalBeInvited);
        CarnivalDataModel.Instance.RemoveEvent<int>(CarnivalEvent.CarnivalTaskSet, OnCarnivalTaskSet);
        CarnivalDataModel.Instance.RemoveEvent<int>(CarnivalEvent.CarnivalNotify, OnCarnivalNotify);
    }

    private void OnCarnivalNotify(int id)
    {
        if (_listVO[0].mId == id)
            OnInit();
    }

    private void OnCarnivalTaskSet(int id)
    {
        if (_listVO[0].mId == id)
            OnInit();
    }

    private void OnCarnivalBeInvited()
    {
        OnInit();
        GetItemTipMgr.Instance.ShowItemResult(_listVO[0].mRewardInfo);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _activeType = int.Parse(args[0].ToString());
        _listVO = args[1] as List<CarnivalDataVO>;
        _invitation.text = "";
        OnInit();
        _bjObj1.SetActive(_activeType == CarnivalConst.InviteAward);
        _bjObj2.SetActive(_activeType == CarnivalConst.Attention);
        _bjObj3.SetActive(_activeType == CarnivalConst.Comment);
        _bjObj4.SetActive(_activeType == CarnivalConst.Share);
        _invitationSystem.SetActive(_activeType == CarnivalConst.InviteAward);
        CarnivalConfig cfg = GameConfigMgr.Instance.GetCarnivalConfig(CarnivalDataModel.Instance.mRound);
        _time.text = cfg.StartTime + " -- " + cfg.EndTime;
        if (_activeType== CarnivalConst.Comment)
        {
            _text1.text = LanguageMgr.GetLanguage(5007601);
            _text2.text = LanguageMgr.GetLanguage(5007602);
            _text3.text = LanguageMgr.GetLanguage(5007603);
            _rect.anchoredPosition = new Vector2(300f, -100f);
        }
        else if(_activeType == CarnivalConst.Attention)
        {
            _text1.text = LanguageMgr.GetLanguage(5007604);
            _text2.text = LanguageMgr.GetLanguage(5007605);
            _text3.text = LanguageMgr.GetLanguage(5007606);
            _rect.anchoredPosition = new Vector2(300f, -100f);
        }
        else if(_activeType == CarnivalConst.Share)
        {
            _text1.text = LanguageMgr.GetLanguage(5007607);
            _text2.text = LanguageMgr.GetLanguage(5007608);
            _text3.text = LanguageMgr.GetLanguage(5007609);
            _rect.anchoredPosition = new Vector2(300f, -100f);
        }
        else
        {
            _text1.text = LanguageMgr.GetLanguage(5007615);
            _text2.text = LanguageMgr.GetLanguage(5007616);
            _text3.text = LanguageMgr.GetLanguage(5007617);
            _title.text = LanguageMgr.GetLanguage(5007615) + ":";
            _rect.anchoredPosition = new Vector2(300f, -50f);
        }
        DiposeChildren();
        _childrenViews = new List<UIBaseView>();
        ItemView view;
        for (int i = 0; i < _listVO[0].mRewardInfo.Count; i++)
        {
            if (GameConfigMgr.Instance.GetItemConfig(_listVO[0].mRewardInfo[i].Id).ItemType == 2)
                view = ItemFactory.Instance.CreateItemView(_listVO[0].mRewardInfo[i], ItemViewType.EquipItem);
            else
                view = ItemFactory.Instance.CreateItemView(_listVO[0].mRewardInfo[i], ItemViewType.BagItem);
            view.mRectTransform.SetParent(_rect, false);
            AddChildren(view);
        }
    }

    private void OnInit()
    {
        if (_listVO[0].mValue >= _listVO[0].mEventCount)
        {
            _but.interactable = false;
            if (_activeType == CarnivalConst.InviteAward)
                _butText.text = LanguageMgr.GetLanguage(5001204);
            else
                _butText.text = LanguageMgr.GetLanguage(5001207);
        }
        else
        {
            _but.interactable = true;
            if (_activeType == CarnivalConst.InviteAward)
                _butText.text = LanguageMgr.GetLanguage(5001203);
            else if(_activeType == CarnivalConst.Share)
                _butText.text = LanguageMgr.GetLanguage(5007622);
            else
                _butText.text = LanguageMgr.GetLanguage(5001202);
        }
    }
    
    private void OnBut()
    {
        if (_activeType == CarnivalConst.Comment)
        {
            //GameNetMgr.Instance.mGameServer.ReqCarnivalTaskSet(_listVO[0].mId);
            LogicMain.mCarnivalType = _activeType;
            if (Application.platform == RuntimePlatform.Android)
            {
                Application.OpenURL("https://play.google.com/store/apps/details?id=com.bettergame.ib");
            }else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Application.OpenURL("https://itunes.apple.com/us/app/idle-battles-heroes-vs-zombies/id1460784402?l=zh&ls=1&mt=8");
            }
        }
        else if (_activeType == CarnivalConst.Attention)
        {
            //GameNetMgr.Instance.mGameServer.ReqCarnivalTaskSet(_listVO[0].mId);
            LogicMain.mCarnivalType = _activeType;
            Application.OpenURL("https://www.facebook.com/Idle-Battles-1635110626787952/?modal=admin_todo_tour");
        }
        else if(_activeType == CarnivalConst.Share)
        {
            //分享
            //GameNetMgr.Instance.mGameServer.ReqCarnivalShare();
            //PopupTipsMgr.Instance.ShowTips("Call Facebook Share");
            GameNative.Instance.DoFBShare();
        }
        else
        {
            if (_invitation.text != "")
            {
                if (_invitation.text == CarnivalDataModel.Instance.mInviteCode)
                    PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000136));
                else
                    GameNetMgr.Instance.mGameServer.ReqCarnivalBeInvited(_invitation.text);
                _invitation.text = "";
            }
            else
            {
                PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000137));
            }
        }
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
