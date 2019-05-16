using Msg.ClientMessage;
using System.Collections.Generic;


public class RankTypeConst
{
    public const int Arena = 1;//竞技场排行榜
    public const int Points = 2;//关卡排行榜
    public const int ComBat = 3;//战力排行榜
    public const int Guild = 4;//公会排行榜
    public const int Artifact = 5;//神器排行榜
}

public class RankDataModel : ModelDataBase<RankDataModel>
{
    private const string RankKey = "rankKey";

    private Dictionary<int, RankDataVO> _dictAllRankData = new Dictionary<int, RankDataVO>();


    public void ReqRankData(int rankId)
    {
        if (CheckNeedRequest(RankKey + rankId, 30))
            GameNetMgr.Instance.mGameServer.ReqRankData(rankId, true);
        else
            Instance.DispathEvent(RankEvent.RankRefresh, rankId);
    }

    private void OnRank(S2CRankListResponse value)
    {
        RankDataVO VO =new RankDataVO();
        VO.InitData(value);
        if (!_dictAllRankData.ContainsKey(value.RankListType))
            _dictAllRankData.Add(value.RankListType, VO);
        else
            _dictAllRankData[value.RankListType] = VO;
        AddLastReqTime(RankKey + value.RankListType);
        DispathEvent(RankEvent.RankRefresh, value.RankListType);
    }

    public RankDataVO GetRankType(int rankType)
    {
        if (_dictAllRankData.ContainsKey(rankType))
            return _dictAllRankData[rankType];
        return null;
    }

    public static void DoRank(S2CRankListResponse value)
    {
        Instance.OnRank(value);
    }

    protected override void DoClearData()
    {
        base.DoClearData();
        if (_dictAllRankData != null)
            _dictAllRankData.Clear();
    }
}
