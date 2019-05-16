using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;

public class WelfareDataModel : ModelDataBase<WelfareDataModel>
{
    private const string SignData = "signData";
    private const string SevenData = "sevenData";
    private const string LimitedData = "limitedData";
    private SignDataVO _signDataVO;
    private List<SevenDataVO> _listSevenVO;

    private List<LimitedDataVO> _listLimitedVO;

    private int _sevenTime;

    public override void Init()
    {
        _signDataVO = new SignDataVO();
        _listLimitedVO = new List<LimitedDataVO>();
        _sevenTime = 0;
        base.Init();
    }

    public void ReqSignData()
    {
        if (CheckNeedRequest(SignData))
            GameNetMgr.Instance.mGameServer.ReqSignData();
        else
            Instance.DispathEvent(WelfareEvent.SignData, _signDataVO);
    }

    public void ReqSevenData()
    {
        if (CheckNeedRequest(SevenData))
            GameNetMgr.Instance.mGameServer.ReqSevenData();
        else
            Instance.DispathEvent(WelfareEvent.SevenData, _listSevenVO);
    }

    public void ReqLimitedData()
    {
        if (CheckNeedRequest(LimitedData))
            GameNetMgr.Instance.mGameServer.ReqActivityData();
        else
            Instance.DispathEvent(WelfareEvent.LimitedData, _listLimitedVO);
    }

    private void OnSignData(S2CSignDataResponse value)
    {
        _signDataVO.InitData(value);
        AddLastReqTime(SignData);
        SignState();
        Instance.DispathEvent(WelfareEvent.SignData, _signDataVO);
    }

    private void OnSignAward(S2CSignAwardResponse value)
    {
        List<ItemInfo> listInfo = new List<ItemInfo>();
        _signDataVO.OnAward(value.Index);
        listInfo.AddRange(value.Rewards);
        SignState();
        Instance.DispathEvent(WelfareEvent.SignAward, listInfo);
    }

    private void OnSevenData(S2CSevenDaysDataResponse value)
    {
        if (value.AwardStates.Count == 0)
        {
            GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(WelfareEvent.SevenTime);
            return;
        }
        if (_listSevenVO != null)
            _listSevenVO.Clear();
        _listSevenVO = new List<SevenDataVO>();
        _sevenTime = (int)Time.realtimeSinceStartup + value.RemainSeconds;
        SevenDataVO vo;
        for (int i = 1; i <= SevendayConfig.Get().Count; i++)
        {
            vo = new SevenDataVO();
            SevendayConfig cfg = GameConfigMgr.Instance.GetSevendayConfig(i);
            ItemInfo itemInfo;
            string[] rewards = cfg.Reward.Split(',');
            if (rewards.Length % 2 != 0)
                return;
            for (int j = 0; j < rewards.Length; j += 2)
            {
                itemInfo = new ItemInfo();
                itemInfo.Id = int.Parse(rewards[j]);
                itemInfo.Value = int.Parse(rewards[j + 1]);
                vo.OnSevenChang(i, itemInfo);
            }
            vo.InitData(value);
            _listSevenVO.Add(vo);
        }
        for (int k = 0; k < value.AwardStates.Count; k++)
            _listSevenVO[k].OnSevenStat(value.AwardStates[k]);
        AddLastReqTime(SevenData);
        SevenState();
        Instance.DispathEvent(WelfareEvent.SevenData, _listSevenVO);
    }

    public int mSevenTime
    {
        get { return _sevenTime - (int)Time.realtimeSinceStartup; }
    }

    private void SignState()
    {
        if (_signDataVO.mMinIndex == _signDataVO.mMaxIndex)
            RedPointDataModel.Instance.SetRedPointDataState(RedPointEnum.Sign, false);
    }

    private void SevenState()
    {
        if (_listSevenVO[_listSevenVO[0].mCurHeaven - 1].mStatus == 1)
            RedPointDataModel.Instance.SetRedPointDataState(RedPointEnum.Seven, false);
    }

    private void OnSevenAward(S2CSevenDaysAwardResponse value)
    {
        List<ItemInfo> listInfo = new List<ItemInfo>();
        listInfo.AddRange(value.Rewards);
        _listSevenVO[value.Days - 1].OnSevenAward();
        SevenState();
        Instance.DispathEvent(WelfareEvent.SevenAward, listInfo);
    }

    private void OnActivityData(S2CActivityDataResponse value)
    {
        if (_listLimitedVO != null || _listLimitedVO.Count > 0)
            _listLimitedVO.Clear();
        _listLimitedVO = new List<LimitedDataVO>();
        LimitedDataVO vo;
        for (int i = 0; i < value.Data.Count; i++)
        {
            vo = new LimitedDataVO();
            vo.InitData(value.Data[i]);
            _listLimitedVO.Add(vo);
        }
        _listLimitedVO.Sort((x, y) => x.mActivityId.CompareTo(y.mActivityId));
        Instance.DispathEvent(WelfareEvent.LimitedData, _listLimitedVO);
    }

    private void OnDataNotify(S2CActivityDataNotify value)
    {
        for (int i = 0; i < _listLimitedVO.Count; i++)
        {
            if (_listLimitedVO[i].mActivityId == value.Id)
            {
                for (int j = 0; j < _listLimitedVO[i].mListLimitedItemDataVO.Count; j++)
                {
                    if (_listLimitedVO[i].mListLimitedItemDataVO[j].mSubActiveID == value.SubId)
                        _listLimitedVO[i].mListLimitedItemDataVO[j].OnValue(value.Value);
                }
            }
        }
        Instance.DispathEvent(WelfareEvent.ActivityDataNotify, value.SubId);
    }

    public static void DoSignData(S2CSignDataResponse value)
    {
        Instance.OnSignData(value);
    }

    public static void DoSignAward(S2CSignAwardResponse value)
    {
        Instance.OnSignAward(value);
    }

    public static void DoSevenData(S2CSevenDaysDataResponse value)
    {
        Instance.OnSevenData(value);
    }

    public static void DoSevenAward(S2CSevenDaysAwardResponse value)
    {
        Instance.OnSevenAward(value);
    }

    public static void DoActivityData(S2CActivityDataResponse value)
    {
        Instance.OnActivityData(value);
    }

    public static void DoDataNotify(S2CActivityDataNotify value)
    {
        Instance.OnDataNotify(value);
    }

    protected override void DoClearData()
    {
        base.DoClearData();
        if (_listSevenVO != null)
            _listSevenVO.Clear();
    }
}
