using Msg.ClientMessage;
using System.Collections.Generic;
using System.Linq;
using System;

public class GuildDataModel : ModelDataBase<GuildDataModel>
{
    private const string GuildDataKey = "guildDataKey";
    private const string RecommemdKey = "recommemdKey";
    private const string SearchGuildKey = "searchGuildKey";
    private const string DonateListKey = "donateListKey";
    private const string MemberDataKey = "memberDataKey";
    private const string AskJoinDataKey = "askJoinDataKey";
    private const string GuildLogsKey = "GuildLogsKey";

    public GuildDataVO mGuildDataVO { get; private set; }
    public List<GuildDataVO> mlstRecommemdGuilds { get; private set; }
    public List<GuildDataVO> mlstSearchGuilds { get; private set; }
    public List<GuildDonateVO> mlstDonateDatas { get; private set; }
    public List<GuildMemberVO> mlstMemberDatas { get; private set; }
    public List<GuildMemberVO> mlstAskJoinDatas { get; private set; }
    private Dictionary<int, GuildMemberVO> _dictAskJoinDatas;
    public Dictionary<string, List<GuildLogsVO>> mdictGuildLogDatas { get; private set; }
    private List<GuildLogsVO> mlstGuildLogs;

    protected override void OnInit()
    {
        base.OnInit();
        mGuildDataVO = new GuildDataVO();
        mlstRecommemdGuilds = new List<GuildDataVO>();
        mlstSearchGuilds = new List<GuildDataVO>();
        mlstDonateDatas = new List<GuildDonateVO>();
        mlstMemberDatas = new List<GuildMemberVO>();
        mlstAskJoinDatas = new List<GuildMemberVO>();
        _dictAskJoinDatas = new Dictionary<int, GuildMemberVO>();
        //mlstGuildLogDatas = new List<GuildLogsVO>();
    }

    #region guild msg data
    public void ReqGuildData()
    {
        if (CheckNeedRequest(GuildDataKey))
            GameNetMgr.Instance.mGameServer.ReqGuildData();
        else
            DispathEvent(GuildEvent.GuildDataRefresh);
    }

    private void InitGuildData(GuildInfo value)
    {
        Instance.AddLastReqTime(GuildDataKey);
        mGuildDataVO.InitData(value);
        DispathEvent(GuildEvent.GuildDataRefresh);
    }

    public static void DoGuildDataResult(S2CGuildDataResponse value)
    {
        Instance.InitGuildData(value.Info);
    }

    public static void DoGuildCreateResult(S2CGuildCreateResponse value)
    {
        Instance.InitGuildData(value.Info);
    }

    public static void DoGuildSignResult(S2CGuildSignInResponse value)
    {
        Instance.mGuildDataVO.UpDateSignData(value);
        if (value.RewardItems.Count > 0)
        {
            IList<ItemInfo> lstRewards = new List<ItemInfo>();
            ItemInfo info;
            for (int i = 0; i < value.RewardItems.Count; i += 2)
            {
                info = new ItemInfo();
                info.Id = value.RewardItems[i];
                info.Value = value.RewardItems[i + 1];
                lstRewards.Add(info);
            }
            GetItemTipMgr.Instance.ShowItemResult(lstRewards);
        }
        RedPointDataModel.Instance.SetRedPointDataState(RedPointEnum.Guild, false);
        Instance.DispathEvent(GuildEvent.GuildSignUpdate);
    }

    public static void DoDismissGuildResult(S2CGuildDismissResponse value)
    {
        Instance.mGuildDataVO.UpdateDismissGuild(value.RealDismissRemainSeconds);
        Instance.DispathEvent(GuildEvent.GuildDismissTimeRefresh);
    }

    public static void DoDeleteGuildResult(S2CGuildDeleteNotify value)
    {
        HeroDataModel.Instance.mHeroInfoData.UpdateGuildId(0);
    }

    public static void DoCancelDismissGuildResult(S2CGuildCancelDismissResponse value)
    {
        Instance.mGuildDataVO.UpdateDismissGuild(0);
        Instance.DispathEvent(GuildEvent.GuildDismissTimeRefresh);
    }

    public static void DoGuildInfoModifyResult(S2CGuildInfoModifyResponse value)
    {
        Instance.mGuildDataVO.UpdateGuildNameAndIcon(value.NewGuildName, value.NewGuildLogo);
    }

    public static void DoGuildNoticeModifyResult(S2CGuildAnouncementResponse value)
    {
        Instance.mGuildDataVO.UpdateGuildNotice(value.Content);
    }
    #endregion

    #region recommemd guild 、search guild msg data
    public void ReqRecommemdGuild()
    {
        GameNetMgr.Instance.mGameServer.ReqRecommemdGuild();
    }

    public static void DoRecommemdGuildResult(S2CGuildRecommendResponse value)
    {
        Instance.mlstRecommemdGuilds.Clear();
        if (value.InfoList != null && value.InfoList.Count > 0)
        {
            GuildDataVO vo;
            for (int i = 0; i < value.InfoList.Count; i++)
            {
                vo = new GuildDataVO();
                vo.InitData(value.InfoList[i]);
                Instance.mlstRecommemdGuilds.Add(vo);
            }
            Instance.AddLastReqTime(RecommemdKey);
        }
        Instance.DispathEvent(GuildEvent.RecommemdGuildRefresh);
    }

    public static void DoSearchGuildResult(S2CGuildSearchResponse value)
    {
        Instance.mlstSearchGuilds.Clear();
        if (value.InfoList != null && value.InfoList.Count > 0)
        {
            GuildDataVO vo;
            for (int i = 0; i < value.InfoList.Count; i++)
            {
                vo = new GuildDataVO();
                vo.InitData(value.InfoList[i]);
                Instance.mlstSearchGuilds.Add(vo);
            }
            Instance.AddLastReqTime(SearchGuildKey);
        }
        Instance.DispathEvent(GuildEvent.SerachGuildRefresh);
    }

    public static void DoGuildAskJoinResult(S2CGuildAskJoinResponse value)
    {
        Instance.DispathEvent(GuildEvent.GuildAskJoinBack, value.GuildId);
    }

    public static void DoGuildAgreeJoinResult(S2CGuildAgreeJoinNotify value)
    {
        HeroDataModel.Instance.mHeroInfoData.UpdateGuildId(value.GuildId);
        Instance.DispathEvent(GuildEvent.GuildAgreeJoinNotify);
    }
    #endregion

    #region guild donate msg data
    public void ReqGuildDonateData()
    {
        if (CheckNeedRequest(DonateListKey))
            GameNetMgr.Instance.mGameServer.ReqDonateList();
        else
            DispathEvent(GuildEvent.GuildDonateListRefresh);
    }

    public static void DoDonateListResult(S2CGuildDonateListResponse value)
    {
        if (value.InfoList != null || value.InfoList.Count > 0)
        {
            Instance.AddLastReqTime(DonateListKey);
            Instance.mlstDonateDatas.Clear();
            GuildDonateVO vo;
            for (int i = 0; i < value.InfoList.Count; i++)
            {
                vo = new GuildDonateVO();
                vo.InitData(value.InfoList[i]);
                Instance.mlstDonateDatas.Add(vo);
            }
        }
        Instance.DispathEvent(GuildEvent.GuildDonateListRefresh);
    }

    public static void DoGuildDonateResult(S2CGuildDonateResponse value)
    {
        if (value.DonateOver)
        {
            for (int i = Instance.mlstDonateDatas.Count - 1; i >= 0; i--)
            {
                if (Instance.mlstDonateDatas[i].mPlayerID == value.PlayerId)
                    Instance.mlstDonateDatas.Remove(Instance.mlstDonateDatas[i]);
            }
        }
        else
        {
            for (int i = 0; i < Instance.mlstDonateDatas.Count; i++)
            {
                if (Instance.mlstDonateDatas[i].mPlayerID == value.PlayerId)
                    Instance.mlstDonateDatas[i].OnDonate(value.ItemNum);
            }
        }
        if (value.DonateNum >= GameConst.GuildDonateIntegralUp)
        {
            for (int i = 0; i < Instance.mlstDonateDatas.Count; i++)
                Instance.mlstDonateDatas[i].OnIsDonate(true);
        }
        else
        {
            for (int i = 0; i < Instance.mlstDonateDatas.Count; i++)
                Instance.mlstDonateDatas[i].OnIsDonate(false);
        }
        Instance.mGuildDataVO.DonateNum(value.DonateNum);
        Instance.DispathEvent(GuildEvent.GuildDonateReward, value.ItemId, value.ItemNum);
        Instance.DispathEvent(GuildEvent.GuildDonateRefresh, value.DonateOver);
    }

    public static void DoGuildDonateItemNot(S2CGuildDonateItemNotify value)
    {
        if (value.DonateOver)
        {
            for (int i = Instance.mlstDonateDatas.Count - 1; i >= 0; i--)
            {
                if (Instance.mlstDonateDatas[i].mPlayerID == HeroDataModel.Instance.mHeroPlayerId)
                    Instance.mlstDonateDatas.Remove(Instance.mlstDonateDatas[i]);
            }
        }
        else
        {
            for (int i = 0; i < Instance.mlstDonateDatas.Count; i++)
            {
                if (Instance.mlstDonateDatas[i].mPlayerID == HeroDataModel.Instance.mHeroPlayerId)
                    Instance.mlstDonateDatas[i].OnDonate(value.ItemNum);
            }
        }
        Instance.DispathEvent(GuildEvent.GuildDonateRefresh, value.DonateOver);
    }

    public static void DoReqDonateResult(S2CGuildAskDonateResponse value)
    {
        GameNetMgr.Instance.mGameServer.ReqDonateList();
        Instance.mGuildDataVO.UpDateAskDinateData();
        Instance.DispathEvent(GuildEvent.ReqDonateResult);
    }
    #endregion

    #region guild manager msg data
    public void ReqGuildMemberData()
    {
        if (CheckNeedRequest(MemberDataKey))
            GameNetMgr.Instance.mGameServer.ReqGuildMembers();
        else
            DispathEvent(GuildEvent.GuildMemberDataRefresh);
    }

    public static void DoQuitGuildResult(S2CGuildQuitResponse value)
    {
        Instance.ResetRequestTime();
        HeroDataModel.Instance.mHeroInfoData.UpdateGuildId(0);
        GameUIMgr.Instance.CloseModule(ModuleID.HeroGuild);
        GameUIMgr.Instance.CloseModule(ModuleID.MemberMgr);
        PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001206));
    }

    public static void DoMembersResult(S2CGuildMemebersResponse value)
    {
        Instance.mlstMemberDatas.Clear();
        if (value.Members != null && value.Members.Count > 0)
        {
            Instance.AddLastReqTime(MemberDataKey);
            GuildMemberVO vo;
            for (int i = 0; i < value.Members.Count; i++)
            {
                vo = new GuildMemberVO();
                vo.InitData(value.Members[i]);
                Instance.mlstMemberDatas.Add(vo);
            }
            Instance.mlstMemberDatas.Sort(SortMembers);
        }
        Instance.DispathEvent(GuildEvent.GuildMemberDataRefresh);
    }

    private static int SortMembers(GuildMemberVO v1, GuildMemberVO v2)
    {
        if (v1.mOfficeType == v2.mOfficeType)
        {
            if (v1.mLastOnlineTime == 0 && v2.mLastOnlineTime == 0)
            {
                if (v1.mPlayerLevel == v2.mPlayerLevel)
                    return -1;
                else
                    return v1.mPlayerLevel > v2.mPlayerLevel ? -1 : 1;
            }
            else
            {
                if (v1.mPlayerLevel == v2.mPlayerLevel)
                    return v1.mLastOnlineTime < v2.mLastOnlineTime ? -1 : 1;
                else
                    return v1.mPlayerLevel > v2.mPlayerLevel ? -1 : 1;
            }
        }
        else if (v1.mOfficeType == GuildOfficeType.President)
            return -1;
        else if (v1.mOfficeType == GuildOfficeType.Office)
            return v2.mOfficeType == GuildOfficeType.President ? 1 : -1;
        else
            return 1;
    }

    public void ReqAskJoinMemberData()
    {
        if (CheckNeedRequest(AskJoinDataKey))
            GameNetMgr.Instance.mGameServer.ReqGuildAskListMembers();
        else
            DispathEvent(GuildEvent.AskJoinMemberDataRefresh);
    }

    public static void DoAskListResult(S2CGuildAskListResponse value)
    {
        Instance.mlstAskJoinDatas.Clear();
        Instance._dictAskJoinDatas.Clear();
        if (value.AskList != null || value.AskList.Count > 0)
        {
            GuildMemberVO vo;
            for (int i = 0; i < value.AskList.Count; i++)
            {
                vo = new GuildMemberVO();
                vo.InitData(value.AskList[i]);
                Instance.mlstAskJoinDatas.Add(vo);
                Instance._dictAskJoinDatas.Add(vo.mPlayerId, vo);
            }
            Instance.AddLastReqTime(AskJoinDataKey);
        }
        Instance.DispathEvent(GuildEvent.AskJoinMemberDataRefresh);
    }

    public static void DoAgreeJoinResult(S2CGuildAgreeJoinResponse value)
    {
        GuildMemberVO vo;
        for(int i = 0; i < value.Player2Res.Count; i++)
        {
            if (value.Player2Res[i].Value >= 0)
            {
                vo = Instance._dictAskJoinDatas[value.Player2Res[i].Id];//[kv.Key];
                Instance.mlstAskJoinDatas.Remove(vo);
            }
            else
            {
                if (value.Player2Res.Count < 2)
                {
                    PopupTipsMgr.Instance.ShowTips(value.Player2Res[i].Value);
                }
            }
            
        }
        Instance.DispathEvent(GuildEvent.AskJoinMemberDataRefresh);
        if (!value.IsRefuse)
        {
            Instance.mGuildDataVO.AddNewMemberCount(value.Player2Res.Count);
            GameNetMgr.Instance.mGameServer.ReqGuildMembers();
            Instance.DispathEvent(GuildEvent.GuildMemberChange);
        }
    }

    public static void DoRecruitResult(S2CGuildRecruitResponse value)
    {
        Instance.DispathEvent(GuildEvent.GuildRecruitSendBack);
    }

    public GuildMemberVO FindGuildMemberVO(int playerID)
    {
        if (mlstMemberDatas.Count == 0)
            return null;
        for (int i = 0; i < mlstMemberDatas.Count; i++)
        {
            if (mlstMemberDatas[i].mPlayerId == playerID)
                return mlstMemberDatas[i];
        }
        return null;
    }

    //公会任命或罢免官员, setType: 1 任命  2 罢免
    public void ReqSetOfficer(int playerID, int setType)
    {
        GameNetMgr.Instance.mGameServer.ReqSetOfficer(playerID, setType);
    }

    private void OnGuildMemberSetOfficer(int playerId, int officer)
    {
        GuildMemberVO vo = FindGuildMemberVO(playerId);
        if (vo == null)
            return;
        vo.UpdateOfficer(officer);
        mlstMemberDatas.Sort(SortMembers);
        DispathEvent(GuildEvent.GuildMemberDataRefresh);
    }

    public static void DoSetOfficerResult(S2CGuildSetOfficerResponse value)
    {
        Instance.OnGuildMemberSetOfficer(value.PlayerIds[0], value.Position);
        if (value.SetType == 1)
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001207));
        else
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001208));
    }

    public static void DoSetOfficerNotify(S2CGuildSetOfficerNotify value)
    {
        Instance.OnGuildMemberSetOfficer(value.MemberId, value.NewPosition);
    }

    //公会驱逐会员
    public void ReqKickMember(int playerId)
    {
        GameNetMgr.Instance.mGameServer.ReqKickMember(playerId);
    }

    private void OnKickMember(int playerID)
    {
        GuildMemberVO vo = FindGuildMemberVO(playerID);
        if (vo == null)
            return;
        mlstMemberDatas.Remove(vo);
        DispathEvent(GuildEvent.GuildMemberDataRefresh);
        Instance.mGuildDataVO.AddNewMemberCount(-1);
        Instance.DispathEvent(GuildEvent.GuildMemberChange);
    }

    public static void DoKickMemberResult(S2CGuildKickMemberResponse value)
    {
        Instance.OnKickMember(value.PlayerIds[0]);
        PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001209));
    }

    public static void DoKickMemberNotify(S2CGuildKickMemberNotify value)
    {
        Instance.OnKickMember(value.MemberId);
    }

    //转让会长
    public void ReqChangePresident(int playerID)
    {
        GameNetMgr.Instance.mGameServer.ReqChangePresident(playerID);
    }

    private void OnChangePresident(int playerID)
    {
        GuildMemberVO vo = FindGuildMemberVO(playerID);
        if (vo == null)
            return;
        vo.UpdateOfficer((int)GuildOfficeType.President);

        if (mGuildDataVO.mOfficeType == GuildOfficeType.President && HeroDataModel.Instance.mHeroPlayerId != playerID)
        {
            vo = FindGuildMemberVO(HeroDataModel.Instance.mHeroPlayerId);
            vo.UpdateOfficer(0);
        }

        mlstMemberDatas.Sort(SortMembers);
        DispathEvent(GuildEvent.GuildMemberDataRefresh);

        mGuildDataVO.UpdatePresidentId(playerID);
        DispathEvent(GuildEvent.GuildDataRefresh);
    }

    public static void DoChangePresidentResult(S2CGuildChangePresidentResponse value)
    {
        Instance.OnChangePresident(value.NewPresidentId);
        PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001210));
    }

    public static void DoChangePresidentNotify(S2CGuildChangePresidentNotify value)
    {
        Instance.OnChangePresident(value.NewPresidentId);
    }
    #endregion
    #region log
    public void ReqGuildLogs()
    {

        if (CheckNeedRequest(GuildLogsKey))
            GameNetMgr.Instance.mGameServer.ReqGuildLogs();
        else
            DispathEvent(GuildEvent.GuildLogsRefresh);
    }
    private void InitGuildLogsData(S2CGuildLogsResponse value)
    {
        LogHelper.Log("服务器返回log数据");
        Instance.AddLastReqTime(GuildLogsKey);
        //Instance.mlstGuildLogDatas.Clear();
        GuildLogsVO _vo;
        mlstGuildLogs = new List<GuildLogsVO>();
        for (int i = 0; i < value.Logs.Count; i++)
        {
            _vo = new GuildLogsVO();
            _vo.InitData(value.Logs[i]);
            mlstGuildLogs.Add(_vo);
        }

        mlstGuildLogs.Sort((x, y) =>
            {
                if (x.mIdDay.CompareTo(y.mIdDay) != 0)
                    return x.mIdDay.CompareTo(y.mIdDay);
                if (x.mIdTime.CompareTo(y.mIdTime) != 0)
                    return x.mIdTime.CompareTo(y.mIdTime);
                return 1;
            }

        );
           

        mdictGuildLogDatas = new Dictionary<string, List<GuildLogsVO>>();
        LogHelper.Log(mlstGuildLogs.Count + "服务器返回log数据");
        List<GuildLogsVO> _logVo = new List<GuildLogsVO>();

        for (int i = 0; i < mlstGuildLogs.Count; i++)
        {
            LogHelper.Log(mlstGuildLogs[i].mTimeTitle);
            if (mdictGuildLogDatas.ContainsKey(mlstGuildLogs[i].mTimeTitle))
            {
                mdictGuildLogDatas[mlstGuildLogs[i].mTimeTitle].Add(mlstGuildLogs[i]);
            }
            else
            {
                _logVo = new List<GuildLogsVO>();
                _logVo.Add(mlstGuildLogs[i]);
                mdictGuildLogDatas.Add(mlstGuildLogs[i].mTimeTitle, _logVo);
            }
            #region
            //_lstTitle.Add(mlstGuildLogs[i].mTimeTitle);
            //Debuger.Log(mlstGuildLogs[i].mTimeTitle+"---"+mlstGuildLogs[i].mTimeFirst+"---"+mlstGuildLogs[i].mTextBehavior);
            ////mdictGuildLogDatas.Add(mlstGuildLogs[i].mTimeTitle, mlstGuildLogs);
            //if (!mdictGuildLogDatas.ContainsKey(mlstGuildLogs[i].mTimeTitle))
            //{
            //    Debuger.Log("butong");
            //    //mdictGuildLogDatas.Add(mlstGuildLogs[i].mTimeTitle, mlstGuildLogs);
            //    //mdictGuildLogDatas[mlstGuildLogs[i].mTimeTitle].Add(mlstGuildLogs[i]);
            //}
            //else
            //{
            //    //mdictGuildLogDatas[mlstGuildLogs[i].mTimeTitle].Add(mlstGuildLogs[i]);
            //    Debuger.Log("xiangtong");
            //}
            #endregion
        }

        //foreach (var ky in mdictGuildLogDatas)
        //{
        //    Debuger.Log(ky.Key + "------------"+ky.Value.Count);
        //}
        DispathEvent(GuildEvent.GuildLogsRefresh);
    }
    public static void DoGuildLogsData(S2CGuildLogsResponse value)
    {
        Instance.InitGuildLogsData(value);
    }
    #endregion

    protected override void DoClearData()
    {
        base.DoClearData();
        if (mlstRecommemdGuilds != null)
            mlstRecommemdGuilds.Clear();
        if (mlstSearchGuilds != null)
            mlstSearchGuilds.Clear();
        if (mlstDonateDatas != null)
            mlstDonateDatas.Clear();
        if (mlstMemberDatas != null)
            mlstMemberDatas.Clear();
        if (mlstAskJoinDatas != null)
            mlstAskJoinDatas.Clear();
        if (_dictAskJoinDatas != null)
            _dictAskJoinDatas.Clear();
        if (mdictGuildLogDatas != null)
            mdictGuildLogDatas.Clear();
        if (mlstGuildLogs != null)
            mlstGuildLogs.Clear();
    }
}