using UnityEngine;
using System.Collections.Generic;

public class LineupSceneMgr : Singleton<LineupSceneMgr>
{
    private Dictionary<int, LineupFighter> _dictAllFighters;

    public TeamType mLineupTeamType { get; private set; }
    private bool _blRefreshFighter;
    private int _maxRole = 4;
    private BattleType _battleType = BattleType.None;
    private int _battleParam;

    private LineupScene _lineupScene;

    public BattleType mBattleType
    {
        get { return _battleType; }
        set
        {
            if (_battleType == value)
                return;
            _battleType = value;
        }
    }

    public void Init()
    {
        _blRefreshFighter = true;
        _lineupScene = RoleRTMgr.Instance.GetRoleRTLogicByType<LineupScene>(RoleRTType.Lineup);
        HeroDataModel.Instance.AddEvent(HeroEvent.CardFightStatusChange, OnRefreshFighter);
        HeroDataModel.Instance.AddEvent<List<int>>(HeroEvent.HeroRemoveCard, OnRemoveCard);
        GameEventMgr.Instance.mGlobalDispatcher.AddEvent(GameEventMgr.GEventClearAllData, OnClearAllData);
        LineUpDragMgr.Instance.Init();
    }

    private void OnClearAllData()
    {
        _lineupScene = null;
        _blRefreshFighter = true;
        _battleType = BattleType.None;
        DisposeAllFighter();
    }

    private void OnRemoveCard(List<int> value)
    {
        _blRefreshFighter = true;
    }

    private void OnRefreshFighter()
    {
        _blRefreshFighter = true;
    } 

    public bool IsAssetFighter(LineupFighter fighter)
    {
        return fighter == _assFighter;
    }

    #region onbattle logic
    public void DoBattle()
    {
        if (mLineupTeamType == TeamType.Defense)
        {
            GameNetMgr.Instance.mGameServer.ReqSetTeam((int)TeamType.Defense, TeamMembers, LocalDataMgr.GetArtifactSele(TeamType.Defense));
            GameUIMgr.Instance.CloseModule(ModuleID.Lineup);
            return;
        }
        else if (mLineupTeamType == TeamType.FriendBossAssist)
        {
            LineupFighter fighter = GetFighterDataByIndex(4);
            int cardId = 0;
            if (fighter != null)
                cardId = fighter.mCardDataVO.mCardID;
            GameUIMgr.Instance.CloseModule(ModuleID.Lineup);
            if (cardId == FriendDataModel.Instance.mFriendAssistVO.mAssistRoleId)
                return;
            GameNetMgr.Instance.mGameServer.ReqSetAssistRole(cardId);
            return;
        }

        IList<int> members = TeamMembers;
        int roleCount = 0;
        bool blEmptyMembers = true;
        if (members != null)
        {
            for (int i = 0; i < members.Count; i++)
            {
                if (members[i] != 0)
                {
                    blEmptyMembers = false;
                    roleCount++;

                }
            }
        }
        if (blEmptyMembers)
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000126));
            return;
        }
        if (roleCount > _maxRole)
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000125) + _maxRole);
            return;
        }
        int assFriendId = 0, assFighterId = 0, assIndex = 0;
        if (_assFighter != null)
        {
            assFighterId = _assFighter.mCardDataVO.mCardID;
            assIndex = FindAssFighterIndex();
            if (assIndex == -1)
                assIndex = 0;
            assFriendId = ActivityCopyDataModel.Instance.AssistFriendID;
        }
        BattleDataModel.Instance.ReqStartBattle(_battleType, _battleParam, _sweepCount, assFriendId, assFighterId, assIndex, LocalDataMgr.GetArtifactSele(mLineupTeamType));
        if (_sweepCount > 0)
            GameUIMgr.Instance.CloseModule(ModuleID.Lineup);
        else
            TDPostDataMgr.Instance.DoBattle(_battleType, _battleParam);
    }
    #endregion

    #region init line up 
    private void ParamBattleConfig(int battleParam, int otherParam)
    {
        int stageID = 0;
        _maxRole = 4;
        _battleParam = battleParam;
        switch (mLineupTeamType)
        {
            case TeamType.ActiveCopy:
                stageID = otherParam;
                _battleType = BattleType.ActivityCopy;
                break;
            case TeamType.Arena:
                _battleType = BattleType.Pvp;
                break;
            case TeamType.Ctower:
                _battleType = BattleType.CTower;
                TowerConfig tconfig = GameConfigMgr.Instance.GetTowerConfig(_battleParam);
                stageID = tconfig.StageID;
                break;
            case TeamType.ExploreTask:
                _battleType = BattleType.ExploreTask;
                stageID = ExploreDataModel.Instance.mRewardStageId;
                break;
            case TeamType.ExploreStoryTask:
                _battleType = BattleType.ExploreStoryTask;
                stageID = ExploreDataModel.Instance.mRewardStageId;
                break;
            case TeamType.FriendBoss:
                _battleType = BattleType.FriendBoss;
                FriendBossConfig config = GameConfigMgr.Instance.GetFriendBossConfig(otherParam);
                stageID = config.BossStageID;
                break;
            case TeamType.Hangeup:
                _battleType = BattleType.Campaign;
                CampaignConfig cconfig = GameConfigMgr.Instance.GetCampaignByCampaignId(_battleParam);
                stageID = cconfig.StageID;
                break;
            case TeamType.GuildBoss:
                GuildBossConfig gconfig = GameConfigMgr.Instance.GetGuildBossConfig(_battleParam);
                stageID = gconfig.StageID;
                _battleType = BattleType.GuildBoss;
                break;
            case TeamType.FriendBattle:
                _battleType = BattleType.FriendBattle;
                break;
            case TeamType.Defense:
                break;
            case TeamType.FriendBossAssist:
                _maxRole = 1;
                break;
            case TeamType.Expedition:
                _battleType = BattleType.Expedition;
                _maxRole = 5;
                break;
        }
        if (stageID != 0)
        {
            StageConfig config = GameConfigMgr.Instance.GetStageConfig(stageID);
            if (ActivityCopyDataModel.Instance.AssistFriendID != 0)
                _maxRole = config.PlayerCardMax + config.FriendSupportMax;
            else
                _maxRole = config.PlayerCardMax;
        }
    }

    private LineupFighter _assFighter = null;
    private int _sweepCount = 0;
    public void ShowLineupModule(TeamType type, int battleParam = 0, int otherParam = 0, int sweepCount = 0)
    {
        RoleRTMgr.Instance.ShowRoleRTLogic(RoleRTType.Lineup);
        _sweepCount = sweepCount;
        if (_blRefreshFighter || type != mLineupTeamType)
        {
            DisposeAllFighter();
            _dictAllFighters = new Dictionary<int, LineupFighter>();
            Dictionary<int, CardDataVO> dictAttack = HeroDataModel.Instance.GetTeamCardVO(type);
            CardDataVO vo;
            LineupFighter fighter;
            for (int i = 0; i < 9; i++)
            {
                if (dictAttack.ContainsKey(i))
                {
                    vo = dictAttack[i];
                    fighter = CreateFighter(i, vo);
                    _dictAllFighters.Add(i, fighter);
                }
            }
            _blRefreshFighter = false;
        }
        if (type == TeamType.ActiveCopy)
        {
            if (_assFighter != null)
            {
                int key = FindAssFighterIndex();
                if (key != -1)
                    _dictAllFighters.Remove(key);
                _assFighter.Dispose();
                _assFighter = null;
            }
            if (ActivityCopyDataModel.Instance.CurAssistCardDataVO != null)
            {
                int idx = FindEmptyIndex();
                _assFighter = CreateFighter(idx, ActivityCopyDataModel.Instance.CurAssistCardDataVO);
                _dictAllFighters.Add(idx, _assFighter);
            }
        }
        mLineupTeamType = type;
        ParamBattleConfig(battleParam, otherParam);
        GameUIMgr.Instance.OpenModule(ModuleID.Lineup);
    }

    private int FindAssFighterIndex()
    {
        foreach (var kv in _dictAllFighters)
        {
            if (kv.Value == _assFighter)
                return kv.Key;
        }
        return -1;
    }

    private int FindEmptyIndex()
    {
        for (int i = 0; i < 9; i++)
        {
            if (!_dictAllFighters.ContainsKey(i))
                return i;
        }
        return -1;
    }


    private LineupFighter CreateFighter(int posIndex, CardDataVO vo)
    {
        LineupFighter fighter = new LineupFighter();
        fighter.InitData(vo);
        Transform parent = _lineupScene.GetFighterParentByIndex(posIndex);
        fighter.AddToStage(parent);
        return fighter;
    }
    #endregion

    public LineupFighter CreateDragFighter(CardDataVO vo)
    {
        LineupFighter fighter = new LineupFighter();
        fighter.InitData(vo);
        fighter.AddToStage(_lineupScene.NewFighterRoot);
        return fighter;
    }

    public void LineupCreateNewFighter(int posIndex, CardDataVO vo)
    {
        _blRefreshFighter = true;
        LineupFighter fighter = CreateFighter(posIndex, vo);
        _dictAllFighters.Add(posIndex, fighter);
    }

    public void QuickRemoveFighter(CardDataVO vo)
    {
        for (int i = 0; i < 9; i++)
        {
            if (_dictAllFighters.ContainsKey(i))
            {
                if (_dictAllFighters[i].mCardDataVO == vo)
                {
                    RemoveFighterByIndex(i);
                    return;
                }
            }
        }
    }

    public void QuickAddFigther(CardDataVO vo)
    {
        if (mLineupTeamType == TeamType.FriendBossAssist)
        {
            if (_dictAllFighters.ContainsKey(4))
                RemoveFighterByIndex(4);
            LineupCreateNewFighter(4, vo);
        }
        else
        {
            for (int i = 0; i < 9; i++)
            {
                if (_dictAllFighters.ContainsKey(i))
                    continue;
                LineupCreateNewFighter(i, vo);
                return;
            }
        }
    }

    public void HideLineModule()
    {
        RoleRTMgr.Instance.Hide(RoleRTType.Lineup);
        if (mLineupTeamType == TeamType.Ctower ||mLineupTeamType == TeamType.Expedition)
            RoleRTMgr.Instance.ShowRoleRTLogic(RoleRTType.BeforeBattle, RoleRTMgr.Instance.GetLastData(), RoleRTMgr.Instance.GetLastShowHpBar());
    }

    private void DisposeAllFighter()
    {
        if (_dictAllFighters != null)
        {
            Dictionary<int, LineupFighter>.ValueCollection vall = _dictAllFighters.Values;
            foreach (LineupFighter fighter in vall)
                fighter.Dispose();
            _dictAllFighters.Clear();
            _dictAllFighters = null;
        }
        DisposeHangupFighters();
        _assFighter = null;
    }

    public void Dispose()
    {
        HeroDataModel.Instance.RemoveEvent(HeroEvent.CardFightStatusChange, OnRefreshFighter);
        GameEventMgr.Instance.mGlobalDispatcher.RemoveEvent(GameEventMgr.GEventClearAllData, OnClearAllData);
        HeroDataModel.Instance.RemoveEvent<List<int>>(HeroEvent.HeroRemoveCard, OnRemoveCard);
        LineUpDragMgr.Instance.Dispose();
        _lineupScene = null;
        _blRefreshFighter = true;
        DisposeAllFighter();
    }

    public LineupFighter GetFighterDataByIndex(int idx)
    {
        if (_dictAllFighters.ContainsKey(idx))
            return _dictAllFighters[idx];
        return null;
    }

    public int GetBattleFighterCount()
    {
        if (_dictAllFighters == null)
            return 0;
        return _dictAllFighters.Count;
    }

    public void RemoveFighterByIndex(int idx)
    {
        CardDataVO vo;
        if (_dictAllFighters.ContainsKey(idx))
        {
            vo = _dictAllFighters[idx].mCardDataVO;

            if (mLineupTeamType == TeamType.ActiveCopy && vo == ActivityCopyDataModel.Instance.CurAssistCardDataVO)
            {
                _dictAllFighters[idx].mUnitRoot.gameObject.SetActive(true);
                return;
            }
            _dictAllFighters[idx].Dispose();
            _dictAllFighters.Remove(idx);
            _blRefreshFighter = true;
            GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(UIEventDefines.LineupFighterRemove, vo.mCardID);
        }
    }

    public IList<int> TeamMembers
    {
        get
        {
            if (_dictAllFighters == null)
                return null;
            IList<int> result = new List<int>();
            for (int i = 0; i < 9; i++)
            {
                if (_dictAllFighters.ContainsKey(i))
                {
                    if (mLineupTeamType == TeamType.ActiveCopy && _dictAllFighters[i] == _assFighter)
                    {
                        result.Add(0);
                    }
                    else
                    {
                        result.Add(_dictAllFighters[i].mCardDataVO.mCardID);
                    }
                }
                else
                {
                    result.Add(0);
                }
            }
            return result;
        }
    }

    public bool CheckCardInBattle(CardDataVO vo)
    {
        for (int i = 0; i < 9; i++)
        {
            if (_dictAllFighters.ContainsKey(i))
            {
                if (_dictAllFighters[i].mCardDataVO == vo)
                    return true;
            }
        }
        return false;
    }

    public int GetTeamBattlePower()
    {
        int power = 0;
        for (int i = 0; i < 9; i++)
        {
            if (_dictAllFighters.ContainsKey(i))
                power += _dictAllFighters[i].mCardDataVO.mBattlePower;
        }
        return power;
    }

    public void ChangLineFighter(int oldIndex, int newIndex)
    {
        _blRefreshFighter = true;
        LineupFighter oldFighter = GetFighterDataByIndex(oldIndex);
        LineupFighter newIdxFighter = GetFighterDataByIndex(newIndex);

        oldFighter.AddToStage(_lineupScene.GetFighterParentByIndex(newIndex));
        _dictAllFighters[newIndex] = oldFighter;

        _dictAllFighters.Remove(oldIndex);
        if (newIdxFighter != null)
        {
            newIdxFighter.AddToStage(_lineupScene.GetFighterParentByIndex(oldIndex));
            _dictAllFighters.Add(oldIndex, newIdxFighter);
        }
    }

    public void SaveBattleTeam()
    {
        if (_dictAllFighters == null || mLineupTeamType == TeamType.FriendBossAssist)
            return;
        LocalDataMgr.AddBattleTeam(mLineupTeamType, TeamMembers);
    }

    public int OnGetMaxRole()
    {
        return _maxRole;
    }

    public int OnDragCount()
    {
        return _dictAllFighters.Count;
    }

    private List<LineupFighter> _hangupLeftFighters;
    private List<LineupFighter> _hangupRightFighters;

    private void DisposeHangupFighters()
    {
        if (_hangupLeftFighters != null)
        {
            for (int i = 0; i < _hangupLeftFighters.Count; i++)
                _hangupLeftFighters[i].Dispose();
            _hangupLeftFighters.Clear();
            _hangupLeftFighters = null;
        }

        if (_hangupRightFighters != null)
        {
            for (int i = 0; i < _hangupRightFighters.Count; i++)
                _hangupRightFighters[i].Dispose();
            _hangupRightFighters.Clear();
            _hangupRightFighters = null;
        }
    }

    public void ShowBattleStatus()
    {
        if (_lineupScene == null)
            return;
        _lineupScene.SetHangUpStatus(true);
        LineupFighter fighter;
        Transform parent;
        DisposeHangupFighters();
        _hangupLeftFighters = new List<LineupFighter>();
        _hangupRightFighters = new List<LineupFighter>();
        Dictionary<int, CardDataVO> monsters = HangupDataModel.Instance.mHangDataVO.mDictMonsters;
        for (int i = 0; i < 9; i++)
        {
            if (_dictAllFighters.ContainsKey(i))
            {
                fighter = new LineupFighter();
                fighter.InitData(_dictAllFighters[i].mCardDataVO);
                parent = _lineupScene.GetBattlePosByIdxAndSide(i, true);
                fighter.AddToStage(parent);
                _hangupLeftFighters.Add(fighter);
            }

            if (monsters.ContainsKey(i))
            {
                fighter = new LineupFighter();
                fighter.InitData(monsters[i]);
                parent = _lineupScene.GetBattlePosByIdxAndSide(i);
                fighter.AddToStage(parent);
                _hangupRightFighters.Add(fighter);
            }
        }
    }

    public void ShowLineupStatus()
    {
        if (_lineupScene == null)
            return;
        _lineupScene.SetHangUpStatus(false);
    }
}