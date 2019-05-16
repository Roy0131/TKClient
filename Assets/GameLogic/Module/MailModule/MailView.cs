using Framework.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailView : UILoopBaseView<MailDataVO>
{
    #region 定义
    private Text _mailName;
    private Text _backText;
    private Text _contText;
    private Text _sourceText;
    private Button _recrive;
    private Button _drawBtn;
    private Button _deleteBtn;
    private Button _replyBtn;
    private GameObject _pointObj;
    private GameObject _awardObj;
    private GameObject _SendImgObj;
    private GameObject _MailObj;
    private Toggle _tog1;
    private Toggle _tog2;
    private RectTransform _awardParent;
    private RectTransform _parent;
    private RectTransform _scroll;

    private MailDataVO _curMailVO;
    private int _CurMailType;
    private int _mailId;
    #endregion

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _mailName = Find<Text>("Text");
        _backText = Find<Text>("Back/Text");
        _contText = Find<Text>("Back/ContentText");
        _sourceText = Find<Text>("Back/SourceText");
        _parent = Find<RectTransform>("Panel_Scroll/KnapsackPanel");
        _MailObj = Find("MailPanl");
        _tog1 = Find<Toggle>("ToggleGroup/Tog1");
        _tog2 = Find<Toggle>("ToggleGroup/Tog2");
        _recrive = Find<Button>("Recrive");
        _drawBtn = Find<Button>("DrawBtn");
        _deleteBtn = Find<Button>("DeleteBtn");
        _replyBtn = Find<Button>("ReplyBtn");
        _awardParent = Find<RectTransform>("AwardObj/Panel_Scroll/KnapsackPanel");
        _scroll = Find<RectTransform>("Panel_Scroll");

        _pointObj = Find("PointPanl");
        _awardObj = Find("AwardObj");
        _SendImgObj = Find("AwardObj/SendImg");

        InitScrollRect("Panel_Scroll");

        _tog1.onValueChanged.Add((bool value) => { if (value) OnMailTypeChange(MailTypeConst.SYSTEM); });
        _tog2.onValueChanged.Add((bool value) => { if (value) OnMailTypeChange(MailTypeConst.PLAYERS); });

        _deleteBtn.onClick.Add(OnDeleteMail);
        _replyBtn.onClick.Add(OnReply);
        _recrive.onClick.Add(OnAttachedItem);
        _drawBtn.onClick.Add(OnDraw);

        _CurMailType = MailTypeConst.SYSTEM;
    }
    //页签切换
    private void OnMailTypeChange(int index)
    {
        _replyBtn.gameObject.SetActive(index != MailTypeConst.SYSTEM);
        if (index == MailTypeConst.SYSTEM)
        {
            _mailName.text = LanguageMgr.GetLanguage(5002802);
            _scroll.anchoredPosition = new Vector2(-306, -55f);
            _scroll.sizeDelta = new Vector2(292.8f, 430);
        }
        else
        {
            _mailName.text = LanguageMgr.GetLanguage(5002803);
            _scroll.anchoredPosition = new Vector2(-306, -25.5f);
            _scroll.sizeDelta = new Vector2(292.8f, 489);
        }
        _CurMailType = index;
        OnMailChange();
        if (_lstDatas.Count > 0)
            OnShowMailDetail(_lstShowViews[0] as MailItemView);
    }
    //回复邮件
    private void OnReply()
    {
        MailSendMgr.Instance.ShowMailSend(_curMailVO.mMailBasicData.SenderId, _curMailVO.mMailBasicData.SenderName, MailTypeConst.PLAYERS);
    }
    //删除邮件数据
    private void OnDeleteMail()
    {
        if (_curMailVO == null)
        {
            return;
        }
        List<int> listId = new List<int>();
        listId.Add(_curMailVO.mMailBasicData.Id);
        GameNetMgr.Instance.mGameServer.ReqMailDelete(listId);
    }
    //一键删除 未启用
    private void OnOneDalete()
    {
        List<int> listId = new List<int>();
        if (_lstDatas.Count > 0)
        {
            for (int i = 0; i < _lstDatas.Count; i++)
            {
                if (_lstDatas[i].mMailBasicData.HasAttached == true && _lstDatas[i].mMailBasicData.IsGetAttached == true ||
                    _lstDatas[i].mMailBasicData.HasAttached == false && _lstDatas[i].mMailBasicData.IsRead == true)
                {
                    listId.Add(_lstDatas[i].mMailBasicData.Id);
                }
            }
        }
        if (listId.Count == 0)
            return;
        GameNetMgr.Instance.mGameServer.ReqMailDelete(listId);
    }
    //删除邮件视图
    private void OnDelete(List<int> listMailID)
    {
        OnMailChange();
        if (_lstDatas.Count > 0)
            OnShowMailDetail(_lstShowViews[0] as MailItemView);
    }
    //领取结果
    private void OnAttachedItem(MailEventVO vo)//List<int> listID, List<ItemInfo> listInfo)
    {
        OnMailChange();
        if (_lstDatas.Count > 0)
            OnShowMailDetail(_lstShowViews[0] as MailItemView);
        for (int i = 0; i < vo.mlstItems.Count; i++)
            RewardTipsMgr.Instance.ShowTips(vo.mlstItems[i]);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<MailItemView>(MailEvent.ShowMailDetail, OnShowMailDetail);
        MailDataModel.Instance.AddEvent<List<int>>(MailEvent.DeleteMail, OnDelete);
        MailDataModel.Instance.AddEvent(MailEvent.MailDetailBack, ShowCurMailDetail);
        MailDataModel.Instance.AddEvent<MailEventVO>(MailEvent.AttachedItem, OnAttachedItem);
        MailDataModel.Instance.AddEvent(MailEvent.MailNew, OnNewMail);
        MailDataModel.Instance.AddEvent(MailEvent.Refresh, OnRefresh);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<MailItemView>(MailEvent.ShowMailDetail, OnShowMailDetail);
        MailDataModel.Instance.RemoveEvent<List<int>>(MailEvent.DeleteMail, OnDelete);
        MailDataModel.Instance.RemoveEvent(MailEvent.MailDetailBack, ShowCurMailDetail);
        MailDataModel.Instance.RemoveEvent<MailEventVO>(MailEvent.AttachedItem, OnAttachedItem);
        MailDataModel.Instance.RemoveEvent(MailEvent.MailNew, OnNewMail);
        MailDataModel.Instance.RemoveEvent(MailEvent.Refresh, OnRefresh);
    }

    private void OnNewMail()
    {
        OnMailChange();
        OnShowMailDetail(_lstShowViews[0] as MailItemView);
    }

    private void OnRefresh()
    {
        OnMailTypeChange(MailTypeConst.SYSTEM);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        MailDataModel.Instance.ReqMailList();
    }
    //领取
    private void OnDraw()
    {
        List<int> listId = new List<int>();
        listId.Add(_curMailVO.mMailBasicData.Id);
        GameNetMgr.Instance.mGameServer.ReqMailAttachedItems(listId);
    }
    //一键领取
    private void OnAttachedItem()
    {
        List<int> listId = new List<int>();
        for (int i = 0; i < _lstDatas.Count; i++)
        {
            if (_lstDatas[i].mMailBasicData.HasAttached == true && _lstDatas[i].mMailBasicData.IsGetAttached == false)
                listId.Add(_lstDatas[i].mMailBasicData.Id);
        }
        if (listId.Count == 0)
            return;
        GameNetMgr.Instance.mGameServer.ReqMailAttachedItems(listId);
    }
    //邮件内容
    private void OnShowMailDetail(MailItemView itemView)
    {
        _mailId = itemView.mMailDataVO.mMailBasicData.Id;
        if (itemView.mMailDataVO.mMailBasicData.SenderId == HeroDataModel.Instance.mHeroPlayerId)
        {
            _replyBtn.gameObject.SetActive(false);
            _deleteBtn.gameObject.transform.localPosition = new Vector3(162f, -249f, 0f);
        }
        else
        {
            if (_CurMailType != MailTypeConst.SYSTEM)
            {
                _replyBtn.gameObject.SetActive(true);
                _deleteBtn.gameObject.transform.localPosition = new Vector3(22f, -249, 0f);
            }
            else
            {
                _replyBtn.gameObject.SetActive(false);
                _deleteBtn.gameObject.transform.localPosition = new Vector3(162f, -249f, 0f);
            }
        }
        for (int i = 0; i < _lstShowViews.Count; i++)
            (_lstShowViews[i] as MailItemView).BlSelected = (_lstShowViews[i] as MailItemView) == itemView;
        _curMailVO = itemView.mMailDataVO;
        _awardObj.SetActive(itemView.mMailDataVO.mMailBasicData.HasAttached == true);
        _drawBtn.gameObject.SetActive(itemView.mMailDataVO.mMailBasicData.HasAttached == true && itemView.mMailDataVO.mMailBasicData.IsGetAttached == false);
        _deleteBtn.gameObject.SetActive(itemView.mMailDataVO.mMailBasicData.HasAttached == false || itemView.mMailDataVO.mMailBasicData.HasAttached == true && itemView.mMailDataVO.mMailBasicData.IsGetAttached == true);
        if (itemView.mMailDataVO.mMailDetailData == null)
            GameNetMgr.Instance.mGameServer.ReqMailDetail(itemView.mMailDataVO.mMailBasicData.Id);
        else
            ShowCurMailDetail();
    }
    //邮件附件
    private void ShowCurMailDetail()
    {
        DiposeChildren();
        _childrenViews = new List<UIBaseView>();
        _SendImgObj.SetActive(_curMailVO.mMailBasicData.IsGetAttached == true);
        if (_curMailVO.mMailBasicData.Type == MailTypeConst.SYSTEM && _curMailVO.mMailBasicData.Subtype > 0)
        {
            MailConfig cfg = GameConfigMgr.Instance.GetMailConfig(_curMailVO.mMailBasicData.Subtype);
            _backText.text = LanguageMgr.GetLanguage(cfg.MailTitleID);
            if (_curMailVO.mMailBasicData.Value > 0)
            {
                if (_curMailVO.mMailBasicData.Subtype == 1104)
                {
                    ArenaDivisionConfig config = GameConfigMgr.Instance.GetArenaDivisionConfig(_curMailVO.mMailBasicData.Value);
                    _contText.text = LanguageMgr.GetLanguage(cfg.MailContentID, TimeHelper.GetTime(_curMailVO.mMailBasicData.SendTime, "yyyy-MM-dd"), LanguageMgr.GetLanguage(config.Name));
                }
                else
                {
                    _contText.text = LanguageMgr.GetLanguage(cfg.MailContentID, TimeHelper.GetTime(_curMailVO.mMailBasicData.SendTime, "yyyy-MM-dd"), _curMailVO.mMailBasicData.Value);
                }
            }
            else
            {

                _contText.text = LanguageMgr.GetLanguage(cfg.MailContentID, TimeHelper.GetTime(_curMailVO.mMailBasicData.SendTime, "yyyy-MM-dd"));
            }
            _sourceText.text = LanguageMgr.GetLanguage(5002802);
        }
        else
        {
            _backText.text = _curMailVO.mMailBasicData.Title;
            _contText.text = _curMailVO.mMailDetailData.Content;
            _sourceText.text = _curMailVO.mMailBasicData.SenderName;
        }
        if (_curMailVO.mMailDetailData.AttachedItems != null)
        {
            IList<ItemInfo> iteminfo = _curMailVO.mMailDetailData.AttachedItems;
            ItemView view;
            for (int i = 0; i < iteminfo.Count; i++)
            {
                view = ItemFactory.Instance.CreateItemView(iteminfo[i], ItemViewType.BagItem, null);
                view.mRectTransform.SetParent(_awardParent, false);
                view.SetNormal();
                if (_curMailVO.mMailBasicData.IsGetAttached == true)
                    view.SetGrayClip();
                AddChildren(view);
            }
        }
    }

    private int SortMail(MailDataVO V0, MailDataVO V1)
    {
        if (V0.mMailBasicData.IsRead == V1.mMailBasicData.IsRead && V0.mMailBasicData.HasAttached == V1.mMailBasicData.HasAttached
            && V0.mMailBasicData.IsGetAttached == V1.mMailBasicData.IsGetAttached)
            return V0.mMailBasicData.SendTime > V1.mMailBasicData.SendTime ? -1 : 1;
        else if(V0.mMailBasicData.IsRead == V1.mMailBasicData.IsRead && V0.mMailBasicData.HasAttached == V1.mMailBasicData.HasAttached)
            return V0.mMailBasicData.IsGetAttached == false ? -1 : 1;
        else if(V0.mMailBasicData.IsRead == V1.mMailBasicData.IsRead)
            return V0.mMailBasicData.HasAttached == true ? -1 : 1;
        else if (V0.mMailBasicData.IsRead != V1.mMailBasicData.IsRead)
            return V0.mMailBasicData.IsRead == false ? -1 : 1;
        return 0;
    }
    //邮件数据
    private void OnMailChange()
    {
        _lstDatas = MailDataModel.Instance.GetMailDataVOType(_CurMailType);
        _lstDatas.Sort(SortMail);
        _pointObj.SetActive(_lstDatas.Count == 0);
        _loopScrollRect.ClearCells();
        if (_lstDatas.Count == 0)
            return;
        _mailId = _lstDatas[0].mMailBasicData.Id;
        _loopScrollRect.totalCount = _lstDatas.Count;
        _loopScrollRect.RefillCells();
    }

    protected override UIBaseView CreateItemView()
    {
        MailItemView item = new MailItemView();
        item.SetDisplayObject(GameObject.Instantiate(_MailObj));
        return item;
    }

    protected override void SetItemData(UIBaseView view, int idx)
    {
        base.SetItemData(view, idx);
        (view as MailItemView).BlSelected = _lstDatas[idx].mMailBasicData.Id == _mailId;
    }

    public override void Hide()
    {
        _tog1.isOn = true;
        if (RewardTipsMgr.Instance.mCurShowTips != null)
            RewardTipsMgr.Instance.ReturnView(RewardTipsMgr.Instance.mCurShowTips);
        base.Hide();
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
