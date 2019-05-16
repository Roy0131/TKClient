using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarnivalTwoView : UIBaseView
{
    private int _activeType;
    private List<CarnivalDataVO> _listVO;
    private Text _titleText;
    private Text _detail;
    private Text _con;
    private Text _copyText;
    private Text _butText;
    private Text _time;
    private Button _copyBtn;
    private Button _but;
    private RectTransform _rectDetail;
    private GameObject _shareObj;
    private GameObject _img1;
    private GameObject _img2;
    private List<UIBaseView> _listUiItemView;
    private RectTransform _parent;
    private GameObject _rechargeItemObj;
    private GameObject _exchangeItemObj;


    protected override void ParseComponent()
    {
        base.ParseComponent();
        _titleText = Find<Text>("TitleText");
        _detail = Find<Text>("Detail");
        _con = Find<Text>("Share/Con");
        _copyText = Find<Text>("Share/CopyBtn/Text");
        _butText = Find<Text>("Buy/Text");
        _time = Find<Text>("Time");
        _copyBtn = Find<Button>("Share/CopyBtn");
        _but = Find<Button>("Buy");
        _rectDetail = Find<RectTransform>("Detail");
        _parent = Find<RectTransform>("Scroll/Group");
        _rechargeItemObj = Find("RechargeItemObj");
        _exchangeItemObj = Find("ExchangeItemObj");
        _shareObj = Find("Share");
        _img1 = Find("Img/Img1");
        _img2 = Find("Img/Img2");

        _copyBtn.onClick.Add(OnCopyBtn);
        _but.onClick.Add(OnBtn);
        _activeType = 0;
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        CarnivalDataModel.Instance.AddEvent<int>(CarnivalEvent.CarnivalNotify, OnCarnivalNotify);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        CarnivalDataModel.Instance.RemoveEvent<int>(CarnivalEvent.CarnivalNotify, OnCarnivalNotify);
    }

    private void OnCarnivalNotify(int id)
    {
        OnShare();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _activeType = int.Parse(args[0].ToString());
        _listVO = args[1] as List<CarnivalDataVO>;
        CarnivalConfig cfg = GameConfigMgr.Instance.GetCarnivalConfig(CarnivalDataModel.Instance.mRound);
        _time.text = cfg.StartTime + " -- " + cfg.EndTime;
        _shareObj.SetActive(_activeType == CarnivalConst.InviteFriend);
        _img1.SetActive(_activeType == CarnivalConst.Exchange);
        _img2.SetActive(_activeType == CarnivalConst.InviteFriend);
        OnShare();
        if (_activeType == CarnivalConst.InviteFriend)
        {
            _titleText.text = LanguageMgr.GetLanguage(5007611);
            _detail.text = LanguageMgr.GetLanguage(5007612);
            _rectDetail.anchoredPosition = new Vector2(186f, 110f);
            _con.text = LanguageMgr.GetLanguage(5007613) + CarnivalDataModel.Instance.mInviteCode;
            _copyText.text = LanguageMgr.GetLanguage(4000139);
        }
        else if (_activeType == CarnivalConst.Exchange)
        {
            _titleText.text = LanguageMgr.GetLanguage(5007619);
            _detail.text = LanguageMgr.GetLanguage(5007620);
            _rectDetail.anchoredPosition = new Vector2(186f, 90f);
        }
        OnBaseClear();
        _listUiItemView = new List<UIBaseView>();
        for (int i = 0; i < _listVO.Count; i++)
        {
            UIBaseView uiBaseView;
            GameObject obj;
            if (_activeType == CarnivalConst.InviteFriend)
            {
                obj = GameObject.Instantiate(_rechargeItemObj);
                uiBaseView = new CarnivalTwoRechargeView();
            }
            else
            {
                obj = GameObject.Instantiate(_exchangeItemObj);
                uiBaseView = new CarnivalTwoExchangeView();
            }
            obj.transform.SetParent(_parent, false);
            uiBaseView.SetDisplayObject(obj);
            uiBaseView.Show(_activeType, _listVO[i]);
            _listUiItemView.Add(uiBaseView);
        }
    }

    private void OnShare()
    {
        if (CarnivalDataModel.Instance.OnCarnivalDataVOValue(CarnivalConst.Share)[0].mValue >= CarnivalDataModel.Instance.OnCarnivalDataVOValue(CarnivalConst.Share)[0].mEventCount)
        {
            _but.interactable = false;
            _butText.text = LanguageMgr.GetLanguage(5001207);
        }
        else
        {
            _but.interactable = true;
            _butText.text = LanguageMgr.GetLanguage(5007622);
        }
    }

    private void OnBaseClear()
    {
        if (_listUiItemView != null)
        {
            for (int i = 0; i < _listUiItemView.Count; i++)
                _listUiItemView[i].Dispose();
            _listUiItemView.Clear();
            _listUiItemView = null;
        }
    }

   private void OnBtn()
    {
        //GameNetMgr.Instance.mGameServer.ReqCarnivalShare();
        GameNative.Instance.DoFBShare();
    }

    private void OnCopyBtn()
    {
        //复制邀请码
        GameNative.Instance.DoCopy(CarnivalDataModel.Instance.mInviteCode);
        PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000140));
    }

    public override void Hide()
    {
        _parent.anchoredPosition = new Vector2(0f, 0f);
        base.Hide();
    }

    public override void Dispose()
    {
        OnBaseClear();
        base.Dispose();
    }
}
