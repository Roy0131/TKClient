using Msg.ClientMessage;
using System.Collections.Generic;

public class FriendDataModel : ModelDataBase<FriendDataModel>
{
    private const string FriendKey = "friendKeys";
    private const string RecommemdKey = "recommemdKeys";
    private const string FriendAskPlayerListKey = "askFriendKeys";
    private const string FriendAssistDataKey = "friendAssistDataKeys";

    public List<FriendDataVO> mlstAllFriends = new List<FriendDataVO>();
    private Dictionary<int, FriendDataVO> _dictAllFriends = new Dictionary<int, FriendDataVO>();

    public List<FriendDataVO> mlstRecommendFriends = new List<FriendDataVO>();
    private Dictionary<int, FriendDataVO> _dictRecommemdFriends = new Dictionary<int, FriendDataVO>();

    public List<FriendDataVO> mlstFriendAskPlayers = new List<FriendDataVO>();
    private Dictionary<int, FriendDataVO> _dictAskFriends = new Dictionary<int, FriendDataVO>();

    public FriendAssistDataVO mFriendAssistVO;

    public int stageId { get; private set; }
    private bool isReqData;

    public void RefreshStageId(int value)
    {
        stageId = value;
    }

    #region 好友数据：添加、删除、好友总列表
    public void ReqFriendList()
    {
        isReqData = true;
        if (CheckNeedRequest(FriendKey))
        {
            mlstAllFriends.Clear();
            _dictAllFriends.Clear();
            GameNetMgr.Instance.mGameServer.ReqFriendList();
        }
        else
        {
            Instance.DispathEvent(FriendEvent.FriendListRefresh);
        }
    }

    private void AddFriends(IList<FriendInfo> value)
    {
        if (value.Count > 0)
        {
            FriendDataVO vo;
            for (int i = 0; i < value.Count; i++)
            {
                if (_dictAllFriends.ContainsKey(value[i].Id))
                {
                    LogHelper.Log("[FriendDataModel.AddFriends() => add new friend repeated!!!, player id:" + value[i].Id + "]");
                    continue;
                }
                vo = new FriendDataVO();
                vo.InitData(value[i]);
                mlstAllFriends.Add(vo);
                _dictAllFriends.Add(vo.mPlayerId, vo);
                //Debuger.LogWarning("添加好友，好友id:" + vo.mPlayerId);
            }
            mlstAllFriends.Sort(SortFriendList);
        }
        if (isReqData)
        {
            AddLastReqTime(FriendKey);
            isReqData = false;
        }
        DispathEvent(FriendEvent.FriendListRefresh);
    }

    private void RemoveFriends(IList<int> value)
    {
        for (int i = 0; i < value.Count; i++)
        {
            if (!_dictAllFriends.ContainsKey(value[i]))
            {
                LogHelper.LogWarning("[FriendDataModel.RemoveFriends() => remove friend, but player id:" + value[i] + " not found!!!]");
                continue;
            }
            mlstAllFriends.Remove(_dictAllFriends[value[i]]);
            _dictAllFriends.Remove(value[i]);
            //Debuger.LogWarning("删除好友，好友id:" + value[i]);
        }
        List<int> listId = new List<int>();
        listId.AddRange(value);
        DispathEvent(FriendEvent.RemoveFriend, listId);
    }

    private void RefreshFriendShipStatus(S2CFriendGivePointsResponse value)
    {
        if (value.FriendIds == null || value.FriendIds.Count <= 0)
            return;
        FriendDataVO vo;
        List<int> lstPlayers = new List<int>();
        for (int i = 0; i < value.FriendIds.Count; i++)
        {
            if (!_dictAllFriends.ContainsKey(value.FriendIds[i]))
            {
                LogHelper.LogWarning("[FriendDataModel.RefreshFriendShipStatus() => refresh friend givepionts statu, but player id:" + value.FriendIds[i] + " not found!!!]");
                continue;
            }
            vo = _dictAllFriends[value.FriendIds[i]];
            vo.RefreshGivePointStatus(value.IsGivePoints[i]);
            lstPlayers.Add(vo.mPlayerId);
        }
        DispathEvent(FriendEvent.FriendRefreshGivePoints, lstPlayers);
    }

    private void RefreshFriendStrength(S2CFriendGetPointsResponse value)
    {
        if (value.FriendIds == null || value.FriendIds.Count == 0)
            return;
        FriendDataVO vo;
        List<int> lstPlayers = new List<int>();
        for (int i = 0; i < value.FriendIds.Count; i++)
        {
            if (!_dictAllFriends.ContainsKey(value.FriendIds[i]))
            {
                LogHelper.LogWarning("[FriendDataModel.RefreshFriendStrength() => refresh friend getpoints statu, but player id:" + value.FriendIds[i] + " not found!!!]");
                continue;
            }
            vo = _dictAllFriends[value.FriendIds[i]];
            vo.RefreshGetPointStatus(value.GetPoints[i]);
            lstPlayers.Add(vo.mPlayerId);
            Instance.mFriendAssistVO.OnGiveGetPoint(value.GetPoints[i]);
        }
        DispathEvent(FriendEvent.FriendRefreshGetPoints, lstPlayers);
    }

    public void RefreshFriendBossHp(int friendID, int hp)
    {
        if (!_dictAllFriends.ContainsKey(friendID))
        {
            LogHelper.LogWarning("[FriendDataModel.RefreshFriendBossHp() => refresh friend boss hp, but friend id:" + friendID + " not found!!!]");
            return;
        }
        _dictAllFriends[friendID].RefreshBossHp(hp);
        DispathEvent(FriendEvent.FriendRefreshBossHp, friendID);
    }

    public FriendDataVO GetFriendById(int id)
    {
        if (_dictAllFriends.ContainsKey(id))
            return _dictAllFriends[id];
        return null;
    }

    //好友列表
    public static void DoFriendListResult(S2CFriendListResponse value)
    {
        //Debuger.LogError("好友数据返回，长度：" + value.Friends.Count);
        Instance.AddFriends(value.Friends);
    }

    //好友列表增加通知
    public static void DoFriendListAddResult(S2CFriendListAddNotify value)
    {
        if (value.FriendsAdd == null)
            return;
        //Debuger.LogError("好友列表增加，数量:" + value.FriendsAdd.Count);
        Instance.AddFriends(value.FriendsAdd);
    }

    public static void DoFriendListRemoveResult(S2CFriendRemoveNotify value)
    {
        //Debuger.LogError("删除单个好友数据:" + value.FriendId);
        Instance.RemoveFriends(new List<int>() { value.FriendId });
    }

    //删除好友
    public static void DoRemoveFriendResult(S2CFriendRemoveResponse value)
    {
        //Debuger.LogError("删除好友, 数量:" + value.PlayerIds.Count);
        Instance.RemoveFriends(value.PlayerIds);        
    }

    //赠送友情点
    public static void DoFriendGivePointsResult(S2CFriendGivePointsResponse value)
    {
        Instance.RefreshFriendShipStatus(value);
    }

    //领取友情点
    public static void DoFriendGetPointsResult(S2CFriendGetPointsResponse value)
    {
        Instance.RefreshFriendStrength(value);
    }

    private static int SortFriendList(FriendDataVO v1, FriendDataVO v2)
    {
        if (v1.mBossId > 0 && v2.mBossId == 0)
            return -1;
        else if (v1.mBossId == 0 && v2.mBossId > 0)
            return 1;
        if (v1.mOfflineTime != v2.mOfflineTime)
            return v1.mOfflineTime > v2.mOfflineTime ? 1 : -1;
        if (v1.mPlayerLevel != v1.mPlayerLevel)
            return v1.mPlayerLevel > v2.mPlayerLevel ? -1 : 1;
        return v1.mPlayerId > v2.mPlayerId ? 1 : -1;
    }
    #endregion

    #region 推荐好友相关操作：推荐列表、申请添加
    public void ReqRecommemdFriend()
    {
        if (CheckNeedRequest(RecommemdKey))
        {
            mlstRecommendFriends.Clear();
            _dictRecommemdFriends.Clear();
            GameNetMgr.Instance.mGameServer.ReqRecommendFriend();
        }
        else
        {
            Instance.DispathEvent(FriendEvent.RecommemdFriendRefresh);
        }
    }

    //推荐好友列表
    public static void DoRecommendFriendResult(S2CFriendRecommendResponse value)
    {
        if (value.Players != null)
        {
            FriendDataVO vo;
            for (int i = 0; i < value.Players.Count; i++)
            {
                vo = new FriendDataVO();
                vo.InitData(value.Players[i]);
                Instance.mlstRecommendFriends.Add(vo);
                Instance._dictRecommemdFriends.Add(vo.mPlayerId, vo);
            }
            Instance.AddLastReqTime(RecommemdKey);
        }
        Instance.DispathEvent(FriendEvent.RecommemdFriendRefresh);
    }

    private void OnAskFriendResult(IList<int> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            FriendDataVO vo;
            if (_dictRecommemdFriends.ContainsKey(players[i]))
            {
                vo = _dictRecommemdFriends[players[i]];
                _dictRecommemdFriends.Remove(players[i]);
                mlstRecommendFriends.Remove(vo);
            }
        }
        Instance.DispathEvent(FriendEvent.RecommemdFriendRefresh);
    }

    //申请好友
    public static void DoFriendAskResult(S2CFriendAskResponse value)
    {
        Instance.OnAskFriendResult(value.PlayerIds);
        List<int> listId = new List<int>();
        listId.AddRange(value.PlayerIds);
        Instance.DispathEvent(FriendEvent.AddFriend, listId);
    }
    #endregion

    #region 好友申请列表相关接口: 好友申请列表、同意、拒绝
    public void ReqFriendAskPlayerList()
    {
        if (CheckNeedRequest(FriendAskPlayerListKey))
        {
            mlstFriendAskPlayers.Clear();
            _dictAskFriends.Clear();
            GameNetMgr.Instance.mGameServer.ReqFriendAskPlayerList();
        }
        else
        {
            Instance.DispathEvent(FriendEvent.FriendAskPlayerListRefresh);
        }
    }

    private void AddAskFriends(IList<PlayerInfo> value)
    {
        if (value != null && value.Count > 0)
        {
            FriendDataVO vo;
            for (int i = 0; i < value.Count; i++)
            {
                vo = new FriendDataVO();
                vo.InitData(value[i]);
                if (_dictAskFriends.ContainsKey(vo.mPlayerId))
                {
                    LogHelper.LogWarning("[FriendDataModel.AddAskFriends() => ask friend add repeated!!!, friend id:" + vo.mPlayerId + "]");
                    continue;
                }
                mlstFriendAskPlayers.Add(vo);
                _dictAskFriends.Add(vo.mPlayerId, vo);
            }
        }
        AddLastReqTime(FriendAskPlayerListKey);
        DispathEvent(FriendEvent.FriendAskPlayerListRefresh);
    }

    private void RemoveAskFriends(IList<int> value)
    {
        for (int i = 0; i < value.Count; i++)
        {
            if (!_dictAskFriends.ContainsKey(value[i]))
            {
                LogHelper.LogWarning("[FriendDataModel.RemoveAskFriends() => remove friend ask request, but player id:" + value[i] + " not found!!!]");
                continue;
            }
            mlstFriendAskPlayers.Remove(_dictAskFriends[value[i]]);
            _dictAskFriends.Remove(value[i]);
        }
        DispathEvent(FriendEvent.FriendAskPlayerListRefresh);
    }

    //好友申请列表
    public static void DoFriendAskListReulst(S2CFriendAskPlayerListResponse value)
    {
        Instance.AddAskFriends(value.Players);
    }

    //好友申请列表增加通知
    public static void DoAskFriendListApplyResult(S2CFriendAskPlayerListAddNotify value)
    {
        Instance.AddAskFriends(value.PlayersAdd);
    }

    //同意添加好友
    public static void DoFriendAgreeResult(S2CFriendAgreeResponse value)
    {
        if (value.Friends == null || value.Friends.Count == 0)
            return;
        IList<int> tmpLst = new List<int>();
        for (int i = 0; i < value.Friends.Count; i++)
            tmpLst.Add(value.Friends[i].Id);
        Instance.RemoveAskFriends(tmpLst);
        Instance.AddFriends(value.Friends);
    }

    //拒绝添加好友
    public static void DoFriendRefuseResult(S2CFriendRefuseResponse value)
    {
        Instance.RemoveAskFriends(value.PlayerIds);
    }
    #endregion

    #region 助战、好友BOSS相关接口
    public void ReqFriendAssistData()
    {
        if (CheckNeedRequest(FriendAssistDataKey))
            GameNetMgr.Instance.mGameServer.ReqFriendAssistData();
        else
            DispathEvent(FriendEvent.FriendAssistDataRefresh);
    }

    public static void DoFriendDataResult(S2CFriendDataResponse value)
    {
        Instance.AddLastReqTime(FriendAssistDataKey);
        if (Instance.mFriendAssistVO == null)
            Instance.mFriendAssistVO = new FriendAssistDataVO();
        Instance.mFriendAssistVO.InitData(value);
        Instance.DispathEvent(FriendEvent.FriendAssistDataRefresh);
    }

    public static void DoSetAssistRoleResult(S2CFriendSetAssistRoleResponse value)
    {
        Instance.mFriendAssistVO.RefreshAssistCardId(value.RoleId);
        Instance.DispathEvent(FriendEvent.FriendAssistCardRefresh);
    }

    public static void DoFriendAssistPointsResult(S2CFriendGetAssistPointsResponse value)
    {
        Instance.mFriendAssistVO.RefreshAssistPoint(value.GetPoints);
        Instance.mFriendAssistVO.OnGetPoint(value.GetPoints);
        Instance.DispathEvent(FriendEvent.FriendAssistPointRefresh, value.GetPoints);
    }

    public static void DoSearchFriendBossResult(S2CFriendSearchBossResponse value)
    {
        Instance.mFriendAssistVO.RefreshBossData(value.FriendBossTableId);
        Instance.DispathEvent(FriendEvent.FriendAssistBossRefresh);
        if (value.Items != null && value.Items.Count > 0)
            GetItemTipMgr.Instance.ShowItemResult(value.Items);
        RedPointDataModel.Instance.SetRedPointDataState(RedPointEnum.FriendAssit, false);
    }

    public void RefreshHeroBossHp(int hpValue)
    {
        if (mFriendAssistVO == null)
            return;
        mFriendAssistVO.RefreshBossHp(hpValue);
        Instance.DispathEvent(FriendEvent.FriendAssistBossRefresh);
    }

    #endregion

    public bool BlFriendListGetOrSendPoint
    {
        get
        {
            for (int i = 0; i < mlstAllFriends.Count; i++)
            {
                if (mlstAllFriends[i].BlGetOrSendPoint)
                    return true;
            }
            return false;
        }
    }

    protected override void DoClearData()
    {
        base.DoClearData();
        if (mlstAllFriends != null)
            mlstAllFriends.Clear();
        if (_dictAllFriends != null)
            _dictAllFriends.Clear();
        if (mlstRecommendFriends != null)
            mlstRecommendFriends.Clear();
        if (_dictRecommemdFriends != null)
            _dictRecommemdFriends.Clear();
        if (mlstFriendAskPlayers != null)
            mlstFriendAskPlayers.Clear();
        if (_dictAskFriends != null)
            _dictAskFriends.Clear();
    }
}