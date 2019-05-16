using Msg.ClientMessage;
using System;
using System.Collections.Generic;
using IHLogic;

public class HeroDataModel : ModelDataBase<HeroDataModel>
{
    public string mHeroAccount { get; private set; }
    public int mHeroPlayerId { get; private set; }
    public HeroInfoDataVO mHeroInfoData { get; private set; }
    public List<CardDataVO> mAllCards { get; private set; }

    private Dictionary<int, CardDataVO> _dictAllCards;
    public List<int> mlstRoleHandBook;


    private uint _timerKey = 0;
    private int _remainSeconds = 60;
    private int _timeNowS;
    private string _timeS;

    public int mSystemTime { get; private set; }


    public List<CardDataVO> OnAllCard()
    {
        List<CardDataVO> allCard = new List<CardDataVO>();
        if (LineupSceneMgr.Instance.mLineupTeamType == TeamType.Expedition)
        {
            for (int i = 0; i < mAllCards.Count; i++)
            {
                if (mAllCards[i].mCardLevel >= 45)
                    allCard.Add(mAllCards[i]);
            }
            return allCard;
        }
        return mAllCards;
    }

    protected override void OnInit()
    {
        base.OnInit();
        mHeroAccount = null;
        mHeroPlayerId = 0;
        mAllCards = new List<CardDataVO>();
        _dictAllCards = new Dictionary<int, CardDataVO>();
        mHeroInfoData = new HeroInfoDataVO();
        mlstRoleHandBook = new List<int>();
        _blEnter = false;
        _blNotify = false;
    }

    private bool _blEnter;
    private bool _blNotify;

    private void OnEnterGame(S2CEnterGameResponse value)
    {
        mHeroAccount = value.Acc;
        mHeroPlayerId = value.PlayerId;
        LogHelper.Log("[Hero enter game, local player id:" + LocalDataMgr.PlayerId  + ", server player id:" + value.PlayerId + "]");
        LocalDataMgr.PlayerId = mHeroPlayerId;
        GameLoginMgr.Instance.EnterGameSuccess();
        NativeLogicInterface.Instance.ReqPayPrice();
        NetReconnectMgr.Instance.HideReconnect();
        GameNetMgr.Instance.mGameServer.ReqFriendList();
        _blEnter = true;
        CheckClientEnterFinished();
        LogHelper.Log("login response --> client enter gameserver back!, hero player id:" + mHeroPlayerId);
    }

    private void OnSetTeamResponse(S2CSetTeamResponse value)
    {
        if ((TeamType)value.TeamType == TeamType.Defense)
        {
            LocalDataMgr.AddBattleTeam(TeamType.Defense, value.TeamMembers);
            GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.EndCondTrigger, NewBieGuide.EndConditionConst.ArenaSetDefenseResult);
        }    
    }

    private void OnTeamsChange(S2CTeamsResponse value)
    {
        LocalDataMgr.InitBattleTeam(value.Teams);
    }

    public Dictionary<int, CardDataVO> GetTeamCardVO(TeamType type)
    {
        Dictionary<int, CardDataVO> result = new Dictionary<int, CardDataVO>();
        CardDataVO vo;
        if (type == TeamType.FriendBossAssist)
        {
            if (LocalDataMgr.FriendAssistCardId != 0)
            {
                vo = GetCardDataByCardId(LocalDataMgr.FriendAssistCardId);
                if (vo != null)
                    result.Add(4, vo);
            }
        }
        else
        {
            IList<int> tmpLst = LocalDataMgr.GetBattleTeamCards(type);
            List<ExpeditionSelfRole> listExpeditionSelfRole = new List<ExpeditionSelfRole>();
            listExpeditionSelfRole = ExpeditionDataModel.Instance.mListExpeditionSelfRole;
            if (tmpLst != null)
            {
                for (int i = 0; i < tmpLst.Count; i++)
                {
                    bool isContinue = false;
                    vo = GetCardDataByCardId(tmpLst[i]);
                    if (vo == null)
                        continue;
                    if (type == TeamType.Expedition)
                    {
                        for (int j = 0; j < listExpeditionSelfRole.Count; j++)
                        {
                            if (vo.mCardID == listExpeditionSelfRole[j].Id)
                            {
                                if (listExpeditionSelfRole[j].HpPercent <= 0 || listExpeditionSelfRole[j].Weak > 0)
                                    isContinue = true;
                            }
                        }
                        if (isContinue)
                            continue;
                    }
                    result.Add(i, vo);
                }
            }
        }
        return result;
    }
    
    private void OnRoleChange(S2CRolesChangeNotify value)
    {
        int i;
        CardDataVO vo;
        int sCardId;
        CardDataVO tmpVO;
        if (value.Adds != null && value.Adds.Count > 0)
        {
            List<int> lstAdds = new List<int>();
            for (i = 0; i < value.Adds.Count; i++)
            {
                vo = new CardDataVO();
                vo.InitData(value.Adds[i]);
                if (_dictAllCards.ContainsKey(vo.mCardID))
                {
                    tmpVO = _dictAllCards[vo.mCardID];
                    if (mAllCards.Contains(tmpVO))
                        mAllCards.Remove(tmpVO);
                    _dictAllCards[vo.mCardID] = vo;
                }
                else
                {
                    _dictAllCards.Add(vo.mCardID, vo);
       
                }
                mAllCards.Add(vo);
                if (mlstRoleHandBook != null && !mlstRoleHandBook.Contains(vo.mCardTableId))
                    mlstRoleHandBook.Add(vo.mCardTableId);
                lstAdds.Add(vo.mCardID);
            }
            LogHelper.LogWarning("[HeroDataModel.OnRoleChange() => 增加角色!!!!]");
            DispathEvent(HeroEvent.HeroAddCard, lstAdds);
        }
        if (value.Updates != null && value.Updates.Count > 0)
        {
            List<int> lstUpdates = new List<int>();
            for (i = 0; i < value.Updates.Count; i++)
            {
                sCardId = value.Updates[i].Id;
                if (_dictAllCards.ContainsKey(sCardId))
                {
                    vo = _dictAllCards[sCardId];
                }
                else
                {
                    LogHelper.LogWarning("server notify client update card, but scardId:" + sCardId + " not found, client create a new card!!");
                    vo = new CardDataVO();
                    _dictAllCards.Add(sCardId, vo);
                }
                vo.RefreshData(value.Updates[i]);
                lstUpdates.Add(vo.mCardID);
            }
            DispathEvent(HeroEvent.CardDataRefresh, lstUpdates);
            LogHelper.LogWarning("[HeroDataModel.OnRoleChange() => 角色更新!!!!]");
        }
        if (value.Removes != null && value.Removes.Count > 0)
        {
            List<int> lstRemoves = new List<int>();
            for (i = 0; i < value.Removes.Count; i++)
            {
                sCardId = value.Removes[i];
                if (!_dictAllCards.ContainsKey(sCardId))
                {
                    LogHelper.LogWarning("server notify client remove card, but scardId:" + sCardId + " not found!");
                    continue;
                }
                tmpVO = _dictAllCards[sCardId];
                if (mAllCards.Contains(tmpVO))
                    mAllCards.Remove(tmpVO);
                _dictAllCards.Remove(sCardId);
                lstRemoves.Add(sCardId);
            }
            LocalDataMgr.RemoveTeamCard(TeamType.Expedition, lstRemoves);
            LogHelper.LogWarning("[HeroDataModel.OnRoleChange() => 角色删除!!!!]");
            DispathEvent(HeroEvent.HeroRemoveCard, lstRemoves);
        }
        LogHelper.Log("login response --> all roles pushed by changenotify message, card len:" + _dictAllCards.Count);
        DispathEvent(HeroEvent.HeroCardChange);
    }

    private void OnRoleResponse(S2CRolesResponse value)
    {
        if (value == null || value.Roles == null)
            return;
        CardDataVO vo;
        for (int i = 0; i < value.Roles.Count; i++)
        {
            vo = new CardDataVO();
            vo.InitData(value.Roles[i]);
            _dictAllCards.Add(vo.mCardID, vo);
            mAllCards.Add(vo);
        }
        DispathEvent(HeroEvent.HeroCardChange);
        LogHelper.Log("login response --> all roles pushed by roleresponese message, card len:" + _dictAllCards.Count);
    }

    private void OnEnterGameFinished(S2CEnterGameCompleteNotify value)
    {
        LogHelper.Log("login response --> server notify login finished!");
        _blNotify = true;
        if (!_blEnter)
        {
            LogHelper.LogError("[HeroDataModel.OnEnterGameFinished() => S2CEnterGameResponse client not recived, login failed]");
            TimerHeap.AddTimer(400, 0, () => { LoginHelper.ReLogin(LocalDataMgr.PlayerAccount, LocalDataMgr.Password, LocalDataMgr.LoginChannel); });
        }
        else
        {
            CheckClientEnterFinished();
        }
    }

    private void CheckClientEnterFinished()
    {
        if (_blNotify && _blEnter)
        {
            GameNetMgr.Instance.mGameServer.ReqRoleHandBookData();
            RedPointDataModel.Instance.ReqRedStates();
            ChatModel.Instance.Init();
            GameStageMgr.Instance.ChangeStage(StageType.Home);
            GameLoginMgr.Instance.EnterGameSuccess();
            LocalDataMgr.ParseOrderCache();
            CheckCacheOrder();
        }
    }

    public void CheckCacheOrder()
    {
        //LogHelper.LogWarning("[HeroDataModel.CheckClientEnterFinished() => cache order count:" + LocalDataMgr.mDictOrderDatas.Count + "]");
        if (LocalDataMgr.mDictOrderDatas.Count > 0)
            IHLogic.NativeLogicInterface.Instance.ReSendOrder();
    }

    public List<CardDataVO> GetRdmHangupRoles()
    {
        List<CardDataVO> result = new List<CardDataVO>();
        Dictionary<int, CardDataVO> dictAttack = GetTeamCardVO(TeamType.Hangeup);
        List<int> attackers = new List<int>(dictAttack.Keys);
        int len = attackers.Count >= 3 ? 3 : attackers.Count;
        while (result.Count < len)
        {
            int idx = UnityEngine.Random.Range(0, attackers.Count);
            if (dictAttack.ContainsKey(attackers[idx]))
            {
                CardDataVO vo = dictAttack[attackers[idx]];
                result.Add(vo);
            }
            attackers.RemoveAt(idx);

        }
        return result;
    }

    public CardDataVO GetCardDataByCardId(int cardId)
    {
        if (_dictAllCards.ContainsKey(cardId))
            return _dictAllCards[cardId];
        return null;
    }

    public int GetBattlePowerByTeamType(TeamType type)
    {
        Dictionary<int, CardDataVO> cards = GetTeamCardVO(type);
        if (cards == null)
            return 0;
        int result = 0;
        foreach (var kv in cards)
            result += kv.Value.mBattlePower;
        return result;
    }

    private void OnPlayerInfo(S2CPlayerInfoResponse value)
    {
        
        if (mHeroInfoData == null)
            mHeroInfoData = new HeroInfoDataVO();
        mHeroInfoData.InitData(value);
        mSystemTime = value.SysTime;
        DispathEvent(HeroEvent.HeroInfoChange);

    }
    /// <summary>
    /// 倒计时显示
    /// </summary>
    private void DIsTime()
    {
        _timerKey = TimerHeap.AddTimer(0, 1000, OnTimeCD);
    }
    private void OnTimeCD()
    {
        mSystemTime++;
    }
    private void ClearRemainTimer()
    {
        if (_timerKey != 0)
            TimerHeap.DelTimer(_timerKey);
        _timerKey = 0;
    }
    private void OnPlayerName(S2CPlayerChangeNameResponse value)
    {
        mHeroInfoData.UpdateHeroName(value.NewName);
        DispathEvent(HeroEvent.AmendName);
    }

    private void OnPlayerHead(S2CPlayerChangeHeadResponse value)
    {
        mHeroInfoData.UpdateHeroHead(value.NewHead);
        DispathEvent(HeroEvent.AmendHead);
    }

    public void OnSystemValue(int value)
    {
        ClearRemainTimer();
        DIsTime();
    }

    #region receive data proccess method (static method)

    public static void UpdateSystemTime(S2CHeartbeat value)
    {
        Instance.mSystemTime = value.SysTime;
        Instance.OnSystemValue(value.SysTime);
    }

    public static void DoPlayerInfo(S2CPlayerInfoResponse value)
    {
        Instance.OnPlayerInfo(value);
    }

    public static void DoPlayerName(S2CPlayerChangeNameResponse value)
    {
        Instance.OnPlayerName(value);
    }

    public static void DoPlayerHead(S2CPlayerChangeHeadResponse value)
    {
        Instance.OnPlayerHead(value);
    }

    public static void DoEnterGame(S2CEnterGameResponse value)
    {
        Instance.OnEnterGame(value);
    }

    public static void DoTeamChange(S2CTeamsResponse value)
    {
        Instance.OnTeamsChange(value);
    }

    public static void DoEnterGameComplete(S2CEnterGameCompleteNotify value)
    {
        Instance.OnEnterGameFinished(value);
    }

    public static void DoHeroRoleChange(S2CRolesChangeNotify value)
    {
        Instance.OnRoleChange(value);
    }

    public static void DoHeroRoleResponse(S2CRolesResponse value)
    {
        Instance.OnRoleResponse(value);
    }

    public static void DoRoleHandBookResponse(S2CRoleHandbookResponse value)
    {
        if (Instance.mlstRoleHandBook == null)
            Instance.mlstRoleHandBook = new List<int>();
        if (value.Roles != null)
            Instance.mlstRoleHandBook.AddRange(value.Roles);
    }

    public static void DoRoleLevelupResponse(S2CRoleLevelUpResponse value)
    {
        //CardDataVO vo = Instance.GetCardDataByCardId(value.RoleId);
        //if (vo == null)
        //{
        //    Debuger.LogError("[HeroDataModel.DoRoleLevelupResponse() => card level up, card id:" + value.RoleId + " can't found!!!");
        //    return;
        //}
        //vo.RoleLevelup(value.Level);
        Instance.DispathEvent(HeroEvent.RoleLevelUp);

    }

    public static void DoRoleRankupResponse(S2CRoleRankUpResponse value)
    {
        //CardDataVO vo = Instance.GetCardDataByCardId(value.RoleId);
        //if(vo == null)
        //{
        //    Debuger.LogError("[HeroDataModel.DoRoleRankupResponse() => card rank up, card id:" + value.RoleId + " can't found!!!");
        //    return;
        //}
        //vo.RoleRankup(value.Rank);
        //Instance.DispathEvent(HeroEvent.CardRankUp);
        Instance.DispathEvent(HeroEvent.Advanced, value.RoleId);
    }

    public static void DoRoleFusionResponse(S2CRoleFusionResponse value)
    {
        Instance.DispathEvent(HeroEvent.CardFusionBack, value);
    }

    public static void DoRoleLockResponse(S2CRoleLockResponse value)
    {
        //CardDataVO vo = Instance.GetCardDataByCardId(value.RoleId);
        //if (vo == null)
        //{
        //    Debuger.LogError("[HeroDataModel.DoRoleRankupResponse() => card lock, card id:" + value.RoleId + " can't found!!!");
        //    return;
        //}
        //vo.mBlLock = value.IsLock;
        //Instance.DispathEvent(HeroEvent.CardLockStatusChange, value.RoleId);
    }

    public static void DoOpenLeftSlotResponse(S2CRoleLeftSlotOpenResponse value)
    {

    }

    public static void DoWearEquipOneKeyResponse(S2CRoleOneKeyEquipResponse value)
    {

    }

    public static void DoTakeoffEquipOneKeyResponse(S2CRoleOneKeyUnequipResponse value)
    {

    }

    public static void DoWearEquipmentResponse(S2CItemEquipResponse value)
    {

    }

    public static void DoTakeoffEquipmentResponse(S2CItemUnequipResponse value)
    {

    }

    public static void DoRoleAttriResponse(S2CRoleAttrsResponse value)
    {

    }

    public static void DoSetTeamResponse(S2CSetTeamResponse value)
    {
        Instance.OnSetTeamResponse(value);
    }
    #endregion

    protected override void DoClearData()
    {
        base.DoClearData();
        if (mAllCards != null)
            mAllCards.Clear();
        if (_dictAllCards != null)
            _dictAllCards.Clear();
        if (mlstRoleHandBook != null)
            mlstRoleHandBook.Clear();
        if (mHeroInfoData != null)
            mHeroInfoData = null;
        _blEnter = false;
        _blNotify = false;
    }
}