using Msg.ClientMessage;
using System.Collections.Generic;

public enum CampaignStageStates
{
    Lock, //锁定
    Passed, //已通关
    Hangup, //正在挂机
    Unlock,//可以手动PVE
}

public class HangupDataModel : ModelDataBase<HangupDataModel>
{
    private const string _campaignDataKey = "campaignDataKey";

    private IList<int> _lstPassedCampaigns;
    public int mIntUnlockCampaignId { get; private set; }
    public int mIntHangupCampaignId { get; private set; }
    public List<CampaignConfig> mlstCurCampaign { get; private set; } //挂机界面上，左边的头卡列表

    private Dictionary<int, int> _dictRewards = new Dictionary<int, int>(); //当前挂机所拥有的奖励（累积奖励）

    private CampaignConfig _curHangupConfig; //当前正在挂机的关卡配置数据 
    private CampaignConfig _curUnlockConfig; //当前可以打的关上配置
    private int _curChapterMapId; //当前挂机所在ChapterID
    private int _curDifficulty; //当前挂机所在Difficulty
    public List<int> mlstRandomRewards { get; private set; } = new List<int>();
    private HangDataVO _hangDataVO = new HangDataVO();

    private bool _blShowModule = false;

    public int mAccelerateNum { get; private set; }
    public int mAccelerateDiamond { get; private set; }
    public int mAccelerateTime { get; private set; }


    public void ReqCampaignData()
    {
        _blShowModule = true;
        if (CheckNeedRequest(_campaignDataKey, 30))
            GameNetMgr.Instance.mGameServer.ReqCampaignData();
        else
            OnCampaignResult(); 
    }

    private void OnAccelerate(S2CCampaignAccelerateIncomeResponse value)
    {
        mAccelerateNum = value.RemainNum;
        mAccelerateDiamond = value.NextCostDiamond;
        List<ItemInfo> listInfo = new List<ItemInfo>();
        listInfo.AddRange(value.Incomes);
        DispathEvent(HangupEvent.AccelerateReward, listInfo);
    }

    private void OnCampaignData(S2CCampaignDataResponse value)
    {
        AddLastReqTime(_campaignDataKey);
        _lstPassedCampaigns = value.PassedCampaignIds;
        mIntUnlockCampaignId = value.UnlockCampaignId == 0 ? 1 : value.UnlockCampaignId;
        mIntHangupCampaignId = value.HangupCampaignId;
        mAccelerateNum = value.RemainAccelerateNum;
        mAccelerateDiamond = value.AccelerateCostDiamond;
        mAccelerateTime = value.AccelerateRefreshRemainSeconds;
        if (value.StaticIncomes != null)
        {
            int key;
            int itemValue;
            for (int i = 0; i < value.StaticIncomes.Count; i++)
            {
                key = value.StaticIncomes[i].Id;
                itemValue = value.StaticIncomes[i].Value;
                if (_dictRewards.ContainsKey(key))
                    _dictRewards[key] = itemValue;
                else
                    _dictRewards.Add(key, itemValue);
            }
        }
        _curUnlockConfig = GameConfigMgr.Instance.GetCampaignByCampaignId(mIntUnlockCampaignId);
        if (mIntHangupCampaignId == 0)
        {
            ReqHangup(1);
        }
        else
        {
            LogHelper.LogWarning("UnlockCampaignId:" + mIntUnlockCampaignId + ", HangupCampaignId:" + mIntHangupCampaignId);
            OnCampaignResult();
        }
    }

    public HangDataVO mHangDataVO
    {
        get { return _hangDataVO; }
    }

    private void OnCampaignResult()
    {
        OnStartRewardTimer();
        _hangDataVO.Reset();
        _hangDataVO.StageID = _curHangupConfig.StageID;
        List<CardDataVO> value = HeroDataModel.Instance.GetRdmHangupRoles();
        if(value != null)
        {
            for (int i = 0; i < value.Count; i++)
                _hangDataVO.AddRole(value[i].mCardConfig);
        }
        if (_blShowModule)
            GameUIMgr.Instance.OpenModule(ModuleID.Hangup);
        else
            HangUpMgr.Instance.ShowHangup(_hangDataVO);
        _blShowModule = false;
        DispathEvent(HangupEvent.CampaignDataChange);
    }


    public void ReqHangup(int campaignId)
    {
        if (campaignId == mIntHangupCampaignId)
            return;
        GameNetMgr.Instance.mGameServer.ReqSetHangup(campaignId);
    }

    private void OnHangupBack(S2CBattleSetHangupCampaignResponse value)
    {
        mIntHangupCampaignId = value.CampaignId;
        OnCampaignResult();
    }

    private uint _rewardTimer = 0;
    private Dictionary<int, int> _dictStaticRewards = new Dictionary<int, int>(); //当前挂机地图固定时间增加的奖励数据
    private void OnStartRewardTimer()
    {
        if (_curHangupConfig != null && _curHangupConfig.CampaignID == mIntHangupCampaignId)
            return;
        if (_rewardTimer != 0)
            TimerHeap.DelTimer(_rewardTimer);
        _curHangupConfig = GameConfigMgr.Instance.GetCampaignByCampaignId(mIntHangupCampaignId);
        if (_curHangupConfig == null)
        {
            LogHelper.LogError("hangup campaign id was invalid, id:" + mIntHangupCampaignId);
            return;
        }
        if (_curChapterMapId != _curHangupConfig.ChapterMap || _curDifficulty != _curHangupConfig.Difficulty)
        {
            _curChapterMapId = _curHangupConfig.ChapterMap;
            _curDifficulty = _curHangupConfig.Difficulty;
            if (mlstCurCampaign == null)
                mlstCurCampaign = new List<CampaignConfig>();
            mlstCurCampaign.Clear();
            ChapterConfig chapterCfg = GameConfigMgr.Instance.GetChapterConfig(_curHangupConfig.ChapterMap);
            int chapterLen = chapterCfg.CampaignCount;
            int clientID;
            CampaignConfig cfg;
            for (int i = 0; i < chapterLen; i++)
            {
                clientID = _curDifficulty * 10000 + _curChapterMapId * 100 + i + 1;
                cfg = GameConfigMgr.Instance.GetCampaignByClientId(clientID);
                mlstCurCampaign.Add(cfg);
            }
        }

        string[] rewards = _curHangupConfig.StaticRewardItem.Split(',');
        
        if (rewards.Length % 2 != 0)
        {
            LogHelper.LogError("HangupDataModel.OnStartRewardTimer() => hangup campaign id:" + mIntHangupCampaignId + " staticreward format was invalid!!");
            return;
        }
        _dictStaticRewards.Clear();
        int itemCfgId;
        int itemCount;
        for (int i = 0; i < rewards.Length; i += 2)
        {
            itemCfgId = int.Parse(rewards[i]);
            itemCount = int.Parse(rewards[i + 1]);
            _dictStaticRewards.Add(itemCfgId, itemCount);//new List<int>() { itemCfgId, itemCount });
        }
        
        string[] randdrop = _curHangupConfig.RandomDropIDList.Split(',');
        if (randdrop.Length % 2 != 0)
        {
            return;
        }
        mlstRandomRewards.Clear();
        int dropId;
        for (int i = 0; i < randdrop.Length; i += 2)
        {
            dropId = int.Parse(randdrop[i]);

            mlstRandomRewards.AddRange(GameConfigMgr.Instance.GetDropConfig(dropId));
            mlstRandomRewards.Sort((x, y) => x.CompareTo(y));
           // mlstRandomRewards.Add(dropId);
        }
        
        int interval = _curHangupConfig.StaticRewardSec *1000;
        _rewardTimer = TimerHeap.AddTimer((uint)interval, interval, OnAddReward);
    }

    private void OnAddReward()
    {
        Dictionary<int, int>.KeyCollection keyColl = _dictStaticRewards.Keys;
        foreach (int key in keyColl)
        {
            if (_dictRewards.ContainsKey(key))
                _dictRewards[key] = _dictRewards[key] + _dictStaticRewards[key];
            else
                _dictRewards.Add(key, _dictStaticRewards[key]);
        }
        DispathEvent(HangupEvent.FixedRewardChange);
    }

    private void OnHangupReward(S2CCampaignHangupIncomeResponse value)
    {
        List<ItemInfo> info;
        if (value.IncomeType == 0)
        {
            _dictRewards.Clear();
            DispathEvent(HangupEvent.FixedRewardChange);
        }
        else if (value.IncomeType == 1)
        {
            info = new List<ItemInfo>();
            info.AddRange(value.Rewards);
            info.Sort((x, y) => x.Id.CompareTo(y.Id));
            RedPointDataModel.Instance.SetRedPointDataState(RedPointEnum.Campain, false);
            DispathEvent(HangupEvent.RandomRewardChange, info);
        }
        else
            BattleDataModel.Instance.SetBattleReward(value);
    }

    public int GetRewardItemCntById(int itemCfgId)
    {
        if (_dictRewards.ContainsKey(itemCfgId))
            return _dictRewards[itemCfgId];
        return 0;
    }

    public string GetStaticRewardByItemId(int id)
    {
        if (_dictStaticRewards.ContainsKey(id))
            return "+"+_dictStaticRewards[id] + "/" + _curHangupConfig.StaticRewardSec + "s";
        return "0/s";
    }

    public CampaignConfig CurHangupConfig
    {
        get { return _curHangupConfig; }
    }

    public CampaignConfig CurUnlockConfig
    {
        get { return _curUnlockConfig; }
    }

    public bool CheckCampaignCanBattle()
    {
        return _lstPassedCampaigns.IndexOf(mIntHangupCampaignId) == -1;
    }

    public CampaignStageStates CheckCampaignStatus(CampaignConfig config)
    {
        if (_curHangupConfig.CampaignID == config.CampaignID)
            return CampaignStageStates.Hangup;
        if (_curUnlockConfig == null)
            return CampaignStageStates.Passed;
        if (_curUnlockConfig.CampaignID == config.CampaignID)
            return CampaignStageStates.Unlock;
        return _curUnlockConfig.CampaignID > config.CampaignID ? CampaignStageStates.Passed : CampaignStageStates.Lock;
    }

    #region Do Server notifiy
    public static void DoCampaignData(S2CCampaignDataResponse value)
    {
        Instance.OnCampaignData(value);
    }

    public static void DoAccelerate(S2CCampaignAccelerateIncomeResponse value)
    {
        Instance.OnAccelerate(value);
    }

    public static void DoHangupReward(S2CCampaignHangupIncomeResponse value)
    {
        Instance.OnHangupReward(value);
    }

    public static void DoHangupBack(S2CBattleSetHangupCampaignResponse value)
    {
        Instance.OnHangupBack(value);
    }
    #endregion

    protected override void DoClearData()
    {
        base.DoClearData();
        _curHangupConfig = null;
        _curUnlockConfig = null;
        mIntUnlockCampaignId = 0;
        mIntHangupCampaignId = 0;
        _curChapterMapId = 0;
        _curDifficulty = 0;
        if (_lstPassedCampaigns != null)
        {
            _lstPassedCampaigns.Clear();
            _lstPassedCampaigns = null;
        }
        if (mlstCurCampaign != null)
            mlstCurCampaign.Clear();
        if (_dictRewards != null)
            _dictRewards.Clear();
        if (mlstRandomRewards != null)
            mlstRandomRewards.Clear();
        if (_dictStaticRewards != null)
            _dictStaticRewards.Clear();
    }
}