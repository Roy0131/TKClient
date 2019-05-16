using Msg.ClientMessage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarnivalDataModel : ModelDataBase<CarnivalDataModel>
{
    private const string CarnivalData = "carnivalData";
    private Dictionary<int, List<CarnivalDataVO>> _allCarnivalDataVO;
    public int mRound = 0;
    private int _CarnivalTime;
    public string mInviteCode { get; private set; }

    public override void Init()
    {
        _CarnivalTime = 0;
        base.Init();
    }

    public void ReqCarnivalData()
    {
        if (CheckNeedRequest(CarnivalData))
            GameNetMgr.Instance.mGameServer.ReqCarnivalData();
        else
        {
            CarnivalEventVO vo = new CarnivalEventVO();
            vo.mCarnivalVO = _allCarnivalDataVO;
            Instance.DispathEvent(CarnivalEvent.CarnivalData, vo);
        }
    }

    //数据
    private void OnCarnivalData(S2CCarnivalDataResponse value)
    {
        if (_allCarnivalDataVO != null)
            _allCarnivalDataVO.Clear();
        _allCarnivalDataVO = new Dictionary<int, List<CarnivalDataVO>>();
        mRound = value.Round;
        _CarnivalTime = value.RemainSeconds + (int)Time.realtimeSinceStartup;
        mInviteCode = value.InviteCode;
        for (int i = 0; i < value.TaskList.Count; i++)
        {
            CarnivalSubConfig cfg = GameConfigMgr.Instance.GetCarnivalSubConfig(value.TaskList[i].Id);
            CarnivalDataVO vo = new CarnivalDataVO();
            vo.InitData(value.TaskList[i]);
            vo.OnCarnivalSubConfig(cfg);
            if (_allCarnivalDataVO.ContainsKey(cfg.ActiveType))
            {
                _allCarnivalDataVO[cfg.ActiveType].Add(vo);
            }
            else
            {
                List<CarnivalDataVO> listVO = new List<CarnivalDataVO>();
                listVO.Add(vo);
                _allCarnivalDataVO.Add(cfg.ActiveType, listVO);
            }
        }
        foreach (var item in _allCarnivalDataVO)
            item.Value.Sort((x, y) => x.mId.CompareTo(y.mId));
        AddLastReqTime(CarnivalData);
        CarnivalEventVO tVO = new CarnivalEventVO();
        tVO.mCarnivalVO = _allCarnivalDataVO;
        Instance.DispathEvent(CarnivalEvent.CarnivalData, tVO);
    }

    public int mCarnivalTime
    {
        get { return _CarnivalTime - (int)Time.realtimeSinceStartup; }
    }

    //任务进度
    private void OnCarnivalTaskSet(S2CCarnivalTaskSetResponse value)
    {
        foreach (List<CarnivalDataVO> item in _allCarnivalDataVO.Values)
        {
            for (int i = 0; i < item.Count; i++)
            {
                if (item[i].mId == value.TaskId)
                    item[i].OnValue();
            }
        }
        Instance.DispathEvent(CarnivalEvent.CarnivalTaskSet, value.TaskId);
    }

    //兑换
    private void OnCarnivalItemExchange(S2CCarnivalItemExchangeResponse value)
    {
        foreach (List<CarnivalDataVO> item in _allCarnivalDataVO.Values)
        {
            for (int i = 0; i < item.Count; i++)
            {
                if (item[i].mId == value.TaskId)
                    item[i].OnValue();
            }
        }
        Instance.DispathEvent(CarnivalEvent.CarnivalItemExchange, value.TaskId);
    }

    //请求分享
    private void OnCarnivalShare(S2CCarnivalShareResponse value)
    {
        Instance.DispathEvent(CarnivalEvent.CarnivalShare, value.InviteCode);
    }

    //被邀请
    private void OnCarnivalBeInvited(S2CCarnivalBeInvitedResponse value)
    {
        Instance.DispathEvent(CarnivalEvent.CarnivalBeInvited);
    }

    //任务进度通知
    private void OnCarnivalNotify(S2CCarnivalTaskDataNotify value)
    {
        foreach (List<CarnivalDataVO> item in _allCarnivalDataVO.Values)
        {
            for (int i = 0; i < item.Count; i++)
            {
                if (item[i].mId == value.Data.Id)
                    item[i].InitData(value.Data);
            }
        }
        Instance.DispathEvent(CarnivalEvent.CarnivalNotify, value.Data.Id);
    }

    public List<CarnivalDataVO> OnCarnivalDataVOValue(int type)
    {
        return _allCarnivalDataVO[type];
    }

    public void OnCarnivaleFinished(int type)
    {
        switch (type)
        {
            case CarnivalConst.Comment:
                PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000141));
                GameNetMgr.Instance.mGameServer.ReqCarnivalTaskSet(_allCarnivalDataVO[CarnivalConst.Comment][0].mId);
                break;
            case CarnivalConst.Attention:
                PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000142));
                GameNetMgr.Instance.mGameServer.ReqCarnivalTaskSet(_allCarnivalDataVO[CarnivalConst.Attention][0].mId);
                break;
            case CarnivalConst.Share:
                PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000117));
                GameNetMgr.Instance.mGameServer.ReqCarnivalShare();
                break;
        }
    }

    public static void DoCarnivalData(S2CCarnivalDataResponse value)
    {
        Instance.OnCarnivalData(value);
    }

    public static void DoCarnivalTaskSet(S2CCarnivalTaskSetResponse value)
    {
        Instance.OnCarnivalTaskSet(value);
    }

    public static void DoCarnivalItemExchange(S2CCarnivalItemExchangeResponse value)
    {
        Instance.OnCarnivalItemExchange(value);
    }

    public static void DoCarnivalShare(S2CCarnivalShareResponse value)
    {
        Instance.OnCarnivalShare(value);
    }

    public static void DoCarnivalBeInvited(S2CCarnivalBeInvitedResponse value)
    {
        Instance.OnCarnivalBeInvited(value);
    }

    public static void DoCarnivalNotify(S2CCarnivalTaskDataNotify value)
    {
        Instance.OnCarnivalNotify(value);
    }
}
