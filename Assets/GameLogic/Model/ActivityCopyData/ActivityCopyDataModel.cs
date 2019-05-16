using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Msg.ClientMessage;


public class ActivityCopyConst
{
    public const int OneCopy = 0;//所有
    public const int GoldCopy = 1;//金币副本
    public const int ExpCopy = 2;//经验副本
    public const int PieceCopy = 3;//碎片副本
}


public class ActivityCopyDataModel : ModelDataBase<ActivityCopyDataModel>
{
    private const string ActivityCopyData = "activityCopyData";
    private const string FriendData = "friendData";
    public Dictionary<int, ActivityCopyVO> mDictActivityVO { get; private set; }
    public List<CardDataVO> lstFriendCard { get; private set; }
    private int _activityCopyTime = 0;
    public int mMaxChallengeNum { get; private set; }
    public int mChallengeNumPrice { get; private set; }
    public int _curType { get; private set; } = 0;

    public int stageId { get; private set; }

    public void OnType(int type)
    {
        _curType = type;
    }

    public void RefreshStageId(int value)
    {
        stageId = value;
    }

    private CardDataVO _curAssistCardDataVO;

    public CardDataVO CurAssistCardDataVO
    {
        get { return _curAssistCardDataVO; }
        set
        {
            _curAssistCardDataVO = value;
        }
    }

    private int _assistFriendID = 0;
    public int AssistFriendID
    {
        get { return _assistFriendID; }
        set
        {
            _assistFriendID = value;
        }
    }

    public override void Init()
    {
        mDictActivityVO = new Dictionary<int, ActivityCopyVO>();
        lstFriendCard = new List<CardDataVO>();
        base.Init();
    }

    public void ReqActivityCopyData(int ActivityCopyType)
    {
        if (CheckNeedRequest(ActivityCopyData + ActivityCopyType))
            GameNetMgr.Instance.mGameServer.ReqActiveStageData(ActivityCopyType);
        else
            DispathEvent(ActivityCopyEvent.ActivityCopyData, mDictActivityVO[ActivityCopyType]);
    }

    public void ReqFriendData()
    {
        if (CheckNeedRequest(FriendData))
            GameNetMgr.Instance.mGameServer.ReqActiveStageAssistRoleListData();
        else
            DispathEvent(ActivityCopyEvent.FriendData, lstFriendCard);
    }

    private void OnActiveStageData(S2CActiveStageDataResponse value)
    {
        ActivityCopyVO vo;
        for (int i = 0; i < value.StageDatas.Count; i++)
        {
            vo = new ActivityCopyVO();
            vo.InitData(value.StageDatas[i]);
            if (mDictActivityVO.ContainsKey(value.StageDatas[i].StageType))
                mDictActivityVO[value.StageDatas[i].StageType] = vo;
            else
                mDictActivityVO.Add(value.StageDatas[i].StageType, vo);
        }
        _activityCopyTime = (int)Time.realtimeSinceStartup + value.RemainSeconds4Refresh;
        mMaxChallengeNum = value.MaxChallengeNum;
        mChallengeNumPrice = value.ChallengeNumPrice;
        Instance.AddLastReqTime(ActivityCopyData + value.StageDatas[0].StageType);
        DispathEvent(ActivityCopyEvent.ActivityCopyData, mDictActivityVO[value.StageDatas[0].StageType]);
    }

    public int mActivityCopyTime
    {
        get { return _activityCopyTime - (int)Time.realtimeSinceStartup; }
    }

    public ActivityCopyVO OnActivityCopyVOType(int type)
    {
        if (mDictActivityVO.ContainsKey(type))
            return mDictActivityVO[type];
        return null;
    }

    private void OnActiveStageBuyChallengeNumData(S2CActiveStageBuyChallengeNumResponse value)
    {
        if (mDictActivityVO.ContainsKey(value.StageType))
        {
            mDictActivityVO[value.StageType].RefreshRemainChallengeNum(value.RemainBuyNum);
            DispathEvent(ActivityCopyEvent.RemainBuyRefresh);
        }
    }

    private void OnActiveStageAssistRoleListData(S2CActiveStageAssistRoleListResponse value)
    {
        if (lstFriendCard != null)
            lstFriendCard.Clear();
        CardDataVO vo;
        for (int i = 0; i < value.Roles.Count; i++)
        {
            vo = new CardDataVO();
            vo.InitData(value.Roles[i]);
            lstFriendCard.Add(vo);
        }
        Instance.AddLastReqTime(FriendData);
        DispathEvent(ActivityCopyEvent.FriendData, lstFriendCard);
    }


    public static void DoActiveStageData(S2CActiveStageDataResponse value)
    {
        Instance.OnActiveStageData(value);
    }

    public static void DoActiveStageBuyChallengeNumData(S2CActiveStageBuyChallengeNumResponse value)
    {
        Instance.OnActiveStageBuyChallengeNumData(value);
    }

    public static void DoActiveStageAssistRoleListData(S2CActiveStageAssistRoleListResponse value)
    {
        Instance.OnActiveStageAssistRoleListData(value);
    }

    public static void DoActiveNotify(S2CActiveStageRefreshNotify value)
    {

    }

    protected override void DoClearData()
    {
        base.DoClearData();
        if (lstFriendCard != null)
            lstFriendCard.Clear();
        if (mDictActivityVO != null)
            mDictActivityVO.Clear();
    }
}
