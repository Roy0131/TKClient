using Msg.ClientMessage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpeditionDataModel : ModelDataBase<ExpeditionDataModel>
{
    private const string ExpeditionData = "expeditionData";
    private const string ExpeditionStageData = "expeditionStageData";
    private int mTime = 0;
    public int ExpeditionTime
    {
        get { return mTime - (int)Time.realtimeSinceStartup; }
    }
    public int mCurCfgId { get; private set; } = 0;
    public int mCurStage { get; private set; } = 0;
    public int mPurifyPoint { get; private set; } = 0;
    public List<ExpeditionSelfRole> mListExpeditionSelfRole { get; private set; }
    public ExoeditionDataVO mExoeditionDataVO { get; private set; }

    public void OnCurCfgId(int id)
    {
        mCurCfgId = id;
    }

    public void ReqExpeditionData()
    {
        if (CheckNeedRequest(ExpeditionData))
            GameNetMgr.Instance.mGameServer.ReqExpeditionData();
        else
            DispathEvent(ExpeditionEvent.ExpeditionData);
    }

    public void ReqExpeditionStageData()
    {
        if (CheckNeedRequest(ExpeditionStageData + mCurStage))
            GameNetMgr.Instance.mGameServer.ReqExpeditionStageData();
        else
            DispathEvent(ExpeditionEvent.ExpeditionStageData, mExoeditionDataVO);
    }

    private void OnExpeditionData(S2CExpeditionDataResponse value)
    {
        if (mListExpeditionSelfRole != null)
            mListExpeditionSelfRole.Clear();
        mListExpeditionSelfRole = new List<ExpeditionSelfRole>();
        mCurStage = value.CurrLevel;
        mPurifyPoint = value.PurifyPoints;
        mTime = value.RemainRefreshSeconds + (int)Time.realtimeSinceStartup;
        mListExpeditionSelfRole.AddRange(value.Roles);
        AddLastReqTime(ExpeditionData);
        DispathEvent(ExpeditionEvent.ExpeditionData);
    }

    private void OnExpeditionStageData(S2CExpeditionLevelDataResponse value)
    {
        if (mExoeditionDataVO != null)
        {
            mExoeditionDataVO.Dispose();
            mExoeditionDataVO = null;
        }
        mExoeditionDataVO = new ExoeditionDataVO();
        mExoeditionDataVO.InitData(value);
        AddLastReqTime(ExpeditionStageData);
        DispathEvent(ExpeditionEvent.ExpeditionStageData, mExoeditionDataVO);
    }

    private void OnExpeditionCurrStageData(S2CExpeditionCurrLevelSync value)
    {
        if (mListExpeditionSelfRole != null)
            mListExpeditionSelfRole.Clear();
        mListExpeditionSelfRole = new List<ExpeditionSelfRole>();
        mListExpeditionSelfRole.AddRange(value.SelfRoles);
        AddLastReqTime(ExpeditionData + mCurStage);
        if (mCurStage == value.CurrLevel)
        {
            mExoeditionDataVO.OnEnemyRoles(value.EnemyRoles);
            AddLastReqTime(ExpeditionStageData);
        }
        else
        {
            mCurStage = value.CurrLevel;
        }
    }

    private void OnExpeditionReward(S2CExpeditionPurifyRewardResponse value)
    {
        List<int> listId = new List<int>();
        listId.AddRange(value.Rewards);
        DispathEvent(ExpeditionEvent.ExpeditionReward, listId);
    }

    private void OnExpeditionPurifyPoint(S2CExpeditionPurifyPointsSync value)
    {
        mPurifyPoint = value.PurifyPoints;
        DispathEvent(ExpeditionEvent.ExpeditionPurifyPoint);
    }


    public static void DoExpeditionData(S2CExpeditionDataResponse value)
    {
        Instance.OnExpeditionData(value);
    }

    public static void DoExpeditionStageData(S2CExpeditionLevelDataResponse value)
    {
        Instance.OnExpeditionStageData(value);
    }

    public static void DoExpeditionCurrStageData(S2CExpeditionCurrLevelSync value)
    {
        Instance.OnExpeditionCurrStageData(value);
    }

    public static void DoExpeditionReward(S2CExpeditionPurifyRewardResponse value)
    {
        Instance.OnExpeditionReward(value);
    }

    public static void DoExpeditionPurifyPoint(S2CExpeditionPurifyPointsSync value)
    {
        Instance.OnExpeditionPurifyPoint(value);
    }
}
