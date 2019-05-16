using UnityEngine;
using UnityEngine.UI;
using Framework.UI;
using System;

public class MemberView : UILoopBaseView<GuildMemberVO>
{
    private Button _askBtn;
    private Button _recruitBtn;
    private Button _exitBtn;
    private Button _closeBtn;

    private GameObject _itemObject;
    
    protected override void ParseComponent()
    {
        base.ParseComponent();
        InitScrollRect("ScrollView");

        _itemObject = Find("ScrollView/Content/UserItem");
        _askBtn = Find<Button>("Buttons/AskBtn");
        _recruitBtn = Find<Button>("Buttons/RecruitBtn");
        _exitBtn = Find<Button>("Buttons/ExitBtn");
        _closeBtn = Find<Button>("Buttons/CloseBtn");

        _askBtn.onClick.Add(OnShowAskView);
        _recruitBtn.onClick.Add(OnShowRecruitView);
        _exitBtn.onClick.Add(OnExitGuild);
        _closeBtn.onClick.Add(OnClose);

        ColliderHelper.SetButtonCollider(_closeBtn.transform);
    }

    private void OnShowAskView()
    {
        GameEventMgr.Instance.mGlobalDispatcher.DispathEvent(GuildEvent.ShowAskJoinMemberView);
    }

    private void OnShowRecruitView()
    {
        GameEventMgr.Instance.mGlobalDispatcher.DispathEvent(GuildEvent.ShowRecruitView);
    }

    private void OnExitGuild()
    {
        if (GuildDataModel.Instance.mGuildDataVO.mOfficeType == GuildOfficeType.President)
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000033));
            return;
        }
        Action<bool, bool> OnAlertBack = (b1, b2) =>
        {
            if (b1)
            {
                GameNetMgr.Instance.mGameServer.ExitGuild();
            }
        };
        ConfirmTipsMgr.Instance.ShowConfirmTips(LanguageMgr.GetLanguage(6001161), OnAlertBack);
    }

    private void OnClose()
    {
        GameUIMgr.Instance.CloseModule(ModuleID.MemberMgr);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GuildDataModel.Instance.AddEvent(GuildEvent.GuildMemberDataRefresh, OnShowMemberView);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GuildDataModel.Instance.RemoveEvent(GuildEvent.GuildMemberDataRefresh, OnShowMemberView);
    }

    private void OnShowMemberView()
    {
        _lstDatas = GuildDataModel.Instance.mlstMemberDatas;
        _loopScrollRect.ClearCells();
        if (_lstDatas.Count == 0)
            return;
        _loopScrollRect.totalCount = _lstDatas.Count;
        _loopScrollRect.RefillCells();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _askBtn.gameObject.SetActive(GuildDataModel.Instance.mGuildDataVO.BlMgr);
        _recruitBtn.gameObject.SetActive(GuildDataModel.Instance.mGuildDataVO.mOfficeType == GuildOfficeType.President);
        GuildDataModel.Instance.ReqGuildMemberData();
    }

    protected override UIBaseView CreateItemView()
    {
        MemberBaseView view = new MemberBaseView();
        view.SetDisplayObject(GameObject.Instantiate(_itemObject));
        return view;
    }
}