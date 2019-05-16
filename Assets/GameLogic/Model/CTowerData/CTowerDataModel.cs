using System.Collections.Generic;
using Msg.ClientMessage;
using UnityEngine;

public class CTowerDataModel : ModelDataBase<CTowerDataModel>
{
    private const string TowerDataKey = "TowerDataKey";
    private const string TowerRecordsInfoKey = "TowerRecordsInfoKey";
    private const string TowerRecordDataKey = "TowerRecordDataKey";
    private const string TowerRankingKey = "TowerRankingKey";

    private bool isMineRequest;

    private int _timeNeed;

    public int remainSeconds { get; private set; }
    public int towerKeys { get; private set; }
    public int currTowerID { get; private set; }

    public List<TowerFightRecord> lstFIghtRecordDataVo { get; private set; } //爬塔某层录像的具体数据
    public List<TowerRankInfo> mListTowerRankInfo { get; private set; }

    /// <summary>
    ///     向服务器请求爬塔当前层，钥匙数，上次获得钥匙的时间节点数据
    /// </summary>
    /// <param name="forceReq"></param>
    public void ReqTowerData(bool forceReq = false)
    {
        if (CheckNeedRequest(TowerDataKey, 2.0f))
        {
            isMineRequest = true;
            GameNetMgr.Instance.mGameServer.ReqTowerData();
        }
        else
        {
            GameUIMgr.Instance.OpenModule(ModuleID.CTower);
            DispathEvent(CTowerEvent.RefreshTowerData);
        }
    }

    /// <summary>
    ///     服务器返回爬塔数据
    /// </summary>
    /// <param name="value"></param>
    private void OnCTowerData(S2CTowerDataResponse value)
    {
        Instance.AddLastReqTime(TowerDataKey);

        //Debuger.Log("服务器返回爬塔界面数据");
        towerKeys = value.TowerKeys;
        currTowerID = value.CurrTowerId;
        remainSeconds = value.RemainSeconds + (int)Time.realtimeSinceStartup;

        //Debuger.Log(towerKeys + "剩余门票数量");
        //Debuger.Log(remainSeconds + "剩余时间秒数");
        //Debuger.Log(currTowerID + "爬塔层数");

        //Debuger.Log(isMineRequest + "是否是我的请求");
        if (isMineRequest)
        {
            GameUIMgr.Instance.OpenModule(ModuleID.CTower);
            DispathEvent(CTowerEvent.RefreshTowerData);
        }

        isMineRequest = false;
    }

    public int mRemainSeconds
    {
        get { return remainSeconds - (int)Time.realtimeSinceStartup; }
    }

    public static void DoCTowerData(S2CTowerDataResponse value)
    {
        Instance.OnCTowerData(value);
    }

    /// <summary>
    ///     向服务器请求爬塔某层录像的数据
    /// </summary>
    public void ReqTowerRecordInfoData()
    {
        if (CheckNeedRequest(TowerRecordsInfoKey))
            GameNetMgr.Instance.mGameServer.ReqTowerRecordInfoData(currTowerID + 1);
        else
            DispathEvent(CTowerEvent.RefreshTowerRecordsInfoData);
        LogHelper.Log("向服务器请求爬塔某层录像的数据");
    }

    /// <summary>
    ///     处理该层录像的数据
    /// </summary>
    /// <param name="value"></param>
    private void OnTowerRecordInfoData(S2CTowerRecordsInfoResponse value)
    {
        Instance.AddLastReqTime(TowerRecordsInfoKey);

        LogHelper.Log("服务器返回爬塔某层的录像数据");
        lstFIghtRecordDataVo = new List<TowerFightRecord>();
        lstFIghtRecordDataVo.AddRange(value.Records);
        DispathEvent(CTowerEvent.RefreshTowerRecordsInfoData);
    }

    public static void DoTowerRecordInfoData(S2CTowerRecordsInfoResponse value)
    {
        Instance.OnTowerRecordInfoData(value);
    }

    /// <summary>
    ///     向服务器请求爬塔某个录像具体数据
    /// </summary>
    public void ReqTowerRecordData(int towerFightID)
    {
        if (CheckNeedRequest(TowerRecordDataKey))
            GameNetMgr.Instance.mGameServer.ReqTowerRecordData(towerFightID);
        else
            DispathEvent(CTowerEvent.RefreshTowerRecordData);
    }

    /// <summary>
    ///     某单个录像的具体数据
    /// </summary>
    /// <param name="value"></param>
    public static void DoTowerRecordData(S2CTowerRecordDataResponse value)
    {
        Instance.AddLastReqTime(TowerRecordDataKey);

        S2CBattleResultResponse recordData = S2CBattleResultResponse.Parser.ParseFrom(value.RecordData);
        BattleDataModel.DoBattleResult(recordData);
    }

    /// <summary>
    ///     向服务器请求爬塔排行榜数据
    /// </summary>
    /// <param name="forceReq"></param>
    public void ReqTowerRankingData(bool forceReq = false)
    {
        LogHelper.Log("向服务器请求排行榜数据");
        if (CheckNeedRequest(TowerRankingKey))
            GameNetMgr.Instance.mGameServer.ReqTowerRankingData();
        else
            DispathEvent(CTowerEvent.RefreshTowerRankingListData);
    }

    /// <summary>
    ///     爬塔排行榜
    /// </summary>
    /// <param name="value"></param>
    private void OnTowerRankingData(S2CTowerRankingListResponse value)
    {
        Instance.AddLastReqTime(TowerRankingKey);
        if (mListTowerRankInfo != null)
            mListTowerRankInfo.Clear();
        mListTowerRankInfo = new List<TowerRankInfo>();
        mListTowerRankInfo.AddRange(value.Ranks);
        DispathEvent(CTowerEvent.RefreshTowerRankingListData);
    }

    public static void DoTowerRankingData(S2CTowerRankingListResponse value)
    {
        Instance.OnTowerRankingData(value);
    }

    protected override void DoClearData()
    {
        base.DoClearData();
        if (lstFIghtRecordDataVo != null)
            lstFIghtRecordDataVo.Clear();
        if (mListTowerRankInfo != null)
            mListTowerRankInfo.Clear();
    }
}