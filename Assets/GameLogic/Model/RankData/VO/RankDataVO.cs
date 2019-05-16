using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;

public class RankDataVO : DataBaseVO
{
    public List<RankItemInfo> mListRankItemInfo { get; private set; }
    public int mSelfHigRank { get; private set; }
    public int mSelfRank { get; private set; }
    public string mData { get; private set; }
    public string mSelfData { get; private set; }
    public Dictionary<int,string> mDictData { get; private set; }


    protected override void OnInitData<T>(T value)
    {
        if (mListRankItemInfo == null)
            mListRankItemInfo = new List<RankItemInfo>();
        mListRankItemInfo.Clear();
        S2CRankListResponse req = value as S2CRankListResponse;
        mListRankItemInfo.AddRange(req.RankItems);
        mListRankItemInfo.Sort((x, y) => x.Rank.CompareTo(y.Rank));
        mSelfRank = req.SelfRank;
        if (req.RankListType == RankTypeConst.Points)
        {
            mDictData = new Dictionary<int, string>();
            if (req.SelfValue>0)
            {
                CampaignConfig cfg = GameConfigMgr.Instance.GetCampaignByCampaignId(req.SelfValue);
                mSelfData = ((cfg.Difficulty - 1) * 8 + cfg.ChapterMap + "-" + cfg.ChildMapID);
            }
            else
            {
                mSelfData = LanguageMgr.GetLanguage(6001217);
            }
            mData = LanguageMgr.GetLanguage(6001218);
            for (int i = 0; i < mListRankItemInfo.Count; i++)
            {
                CampaignConfig cfgs = GameConfigMgr.Instance.GetCampaignByCampaignId(mListRankItemInfo[i].PlayerPassedCampaignId);
                mDictData.Add(mListRankItemInfo[i].PlayerId, (cfgs.Difficulty - 1) * 8 + cfgs.ChapterMap + "-" + cfgs.ChildMapID);
            }
        }
        if (req.RankListType == RankTypeConst.ComBat)
        {
            mDictData = new Dictionary<int, string>();
            mData = "";
            mSelfData = req.SelfValue.ToString();
            for (int i = 0; i < mListRankItemInfo.Count; i++)
            {
                mDictData.Add(mListRankItemInfo[i].PlayerId, mListRankItemInfo[i].PlayerRolesPower.ToString());
            }
        }
        if (req.RankListType == RankTypeConst.Guild)
        {
            mDictData = new Dictionary<int, string>();
            mData = LanguageMgr.GetLanguage(6001220);
            mSelfData = "45645";
            for (int i = 0; i < mListRankItemInfo.Count; i++)
            {
                mDictData.Add(mListRankItemInfo[i].PlayerId, mListRankItemInfo[i].PlayerPower.ToString());
            }
        }
        if (req.RankListType == RankTypeConst.Artifact)
        {
            mDictData = new Dictionary<int, string>();
            mData = "";
            mSelfData = "54614";
            for (int i = 0; i < mListRankItemInfo.Count; i++)
            {
                mDictData.Add(mListRankItemInfo[i].PlayerId, mListRankItemInfo[i].PlayerPower.ToString());
            }
        }
        if (req.RankListType == RankTypeConst.Arena)
        {
            mDictData = new Dictionary<int, string>();
            mSelfHigRank = req.SelfHistoryTopRank;
            mData = LanguageMgr.GetLanguage(6001220);
            mSelfData = req.SelfValue.ToString();
            for (int i = 0; i < mListRankItemInfo.Count; i++)
            {
                mDictData.Add(mListRankItemInfo[i].PlayerId, mListRankItemInfo[i].PlayerArenaScore.ToString());
            }
        }
    }
}
