using Msg.ClientMessage;
using System.Collections.Generic;

public class ArtifactDataModel : ModelDataBase<ArtifactDataModel>
{
    private const string ArtifactData = "artifactData";
    public List<ArtifactDataVO> mListArtifactVO { get; private set; }
    public Dictionary<int, List<ItemInfo>> mAllArtifactAtt { get; private set; }

    public ArtifactDataVO OnArtifactVO(int id)
    {
        return mListArtifactVO[id - 1];
    }

    private int OnArtifactVOSort(ArtifactDataVO V1, ArtifactDataVO V2)
    {
        return V1.mSortIndex < V2.mSortIndex ? -1 : 1;
    }

    public void ReqArtifactData()
    {
        if (CheckNeedRequest(ArtifactData, 300))
            GameNetMgr.Instance.mGameServer.ReqArtifactData();
        else
            DispathEvent(ArtifactEvent.ArtifactData);
    }

    private void OnArtifactData(S2CArtifactDataResponse value)
    {
        if (mListArtifactVO != null)
            mListArtifactVO.Clear();
        mListArtifactVO = new List<ArtifactDataVO>();
        if (mAllArtifactAtt != null)
            mAllArtifactAtt.Clear();
        mAllArtifactAtt = new Dictionary<int, List<ItemInfo>>();
        ArtifactDataVO vo;
        ArtifactData data;
        foreach (ArtifactUnlockConfig cfg in ArtifactUnlockConfig.Get().Values)
        {
            bool isUnlock = false;
            vo = new ArtifactDataVO();
            data = new ArtifactData();
            data.Id = cfg.ArtifactID;
            data.Level = 0;
            data.Rank = 1;
            if (value.ArtifactList != null && value.ArtifactList.Count > 0)
            {
                for (int i = 0; i < value.ArtifactList.Count; i++)
                {
                    if (value.ArtifactList[i].Id == cfg.ArtifactID)
                    {
                        isUnlock = true;
                        data = value.ArtifactList[i];
                    }
                }
            }
            vo.InitData(data);
            mListArtifactVO.Add(vo);
            if (isUnlock)
                mAllArtifactAtt.Add(cfg.ArtifactID, vo.mCurArtifactAtt);
        }
        mListArtifactVO.Sort(OnArtifactVOSort);
        AddLastReqTime(ArtifactData);
        DispathEvent(ArtifactEvent.ArtifactData);
    }

    private void OnArtifactUnlock(S2CArtifactUnlockResponse value)
    {
        for (int i = 0; i < mListArtifactVO.Count; i++)
        {
            if (mListArtifactVO[i].mArtifactData.Id== value.Id)
            {
                mListArtifactVO[i].OnArtifactReset();
                if (mAllArtifactAtt.ContainsKey(value.Id))
                    mAllArtifactAtt[value.Id] = mListArtifactVO[i].mCurArtifactAtt;
                else
                    mAllArtifactAtt.Add(value.Id, mListArtifactVO[i].mCurArtifactAtt);
                DispathEvent(ArtifactEvent.ArtifactUnlock, value.Id);
            }
        }
    }

    private void OnArtifactUp(S2CArtifactLevelUpResponse value)
    {
        for (int i = 0; i < mListArtifactVO.Count; i++)
        {
            if (mListArtifactVO[i].mArtifactData.Id == value.Id)
            {
                mListArtifactVO[i].OnArtifactLevel(value.Level);
                if (mAllArtifactAtt.ContainsKey(value.Id))
                    mAllArtifactAtt[value.Id] = mListArtifactVO[i].mCurArtifactAtt;
                else
                    mAllArtifactAtt.Add(value.Id, mListArtifactVO[i].mCurArtifactAtt);
                DispathEvent(ArtifactEvent.ArtifactRefresh, mListArtifactVO[i]);
            }
        }
    }

    private void OnArtifactRankUp(S2CArtifactRankUpResponse value)
    {
        for (int i = 0; i < mListArtifactVO.Count; i++)
        {
            if (mListArtifactVO[i].mArtifactData.Id == value.Id)
            {
                mListArtifactVO[i].OnArtifactRank(value.Rank);
                if (mAllArtifactAtt.ContainsKey(value.Id))
                    mAllArtifactAtt[value.Id] = mListArtifactVO[i].mCurArtifactAtt;
                else
                    mAllArtifactAtt.Add(value.Id, mListArtifactVO[i].mCurArtifactAtt);
                DispathEvent(ArtifactEvent.ArtifactRefresh, mListArtifactVO[i]);
            }
        }
    }

    private void OnArtifactReset(S2CArtifactResetResponse value)
    {
        for (int i = 0; i < mListArtifactVO.Count; i++)
        {
            if (mListArtifactVO[i].mArtifactData.Id == value.Id)
            {
                DispathEvent(ArtifactEvent.ArtifactReset, value.Id);
                mListArtifactVO[i].OnArtifactReset();
                if (mAllArtifactAtt.ContainsKey(value.Id))
                    mAllArtifactAtt[value.Id] = mListArtifactVO[i].mCurArtifactAtt;
                else
                    mAllArtifactAtt.Add(value.Id, mListArtifactVO[i].mCurArtifactAtt);
                DispathEvent(ArtifactEvent.ArtifactRefresh, mListArtifactVO[i]);
            }
        }
    }

    public static void DoArtifactData(S2CArtifactDataResponse value)
    {
        Instance.OnArtifactData(value);
    }

    public static void DoArtifactUnlock(S2CArtifactUnlockResponse value)
    {
        Instance.OnArtifactUnlock(value);
    }

    public static void DoArtifactUp(S2CArtifactLevelUpResponse value)
    {
        Instance.OnArtifactUp(value);
    }

    public static void DoArtifactRankUp(S2CArtifactRankUpResponse value)
    {
        Instance.OnArtifactRankUp(value);
    }

    public static void DoArtifactReset(S2CArtifactResetResponse value)
    {
        Instance.OnArtifactReset(value);
    }

    public static void DoArtifactNotify(S2CArtifactDataUpdateNotify value)
    {

    }

    protected override void DoClearData()
    {
        base.DoClearData();
        if (mListArtifactVO != null)
            mListArtifactVO.Clear();
        if (mAllArtifactAtt != null)
            mAllArtifactAtt.Clear();
    }
}
