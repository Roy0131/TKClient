using IHLogic;
using Msg.ClientMessage;
using System.Collections.Generic;

public class RechargeDataModel : ModelDataBase<RechargeDataModel>
{
    private const string RechargeData = "rechargeData";
    public List<MonthCardData> mListMonthCard { get; private set; }
    public int mFirstChargeState { get; private set; }

    public List<string> mLstStrId { get; private set; }

    public override void Init()
    {
        mListMonthCard = new List<MonthCardData>();
        mLstStrId = new List<string>();
        base.Init();
    }

    public void ReqRechargeData()
    {
        if (CheckNeedRequest(RechargeData))
            GameNetMgr.Instance.mGameServer.ReqRechargeData();
        else
            Instance.DispathEvent(RechargeEvent.RechargeData, mListMonthCard);
    }

    private void OnRechargeData(S2CChargeDataResponse value)
    {
        if (mListMonthCard != null)
            mListMonthCard.Clear();
        mListMonthCard.AddRange(value.Datas);
        mFirstChargeState = value.FirstChargeState;
        mLstStrId.AddRange(value.ChargedBundleIds);
        AddLastReqTime(RechargeData);
        Instance.DispathEvent(RechargeEvent.RechargeData, mListMonthCard);
    }

    private void OnRecharge(S2CChargeResponse value)
    {
        NativeLogicInterface.Instance.RechargeBack(value.ClientIndex);
        Instance.DispathEvent(RechargeEvent.Recharge, value.IsFirst,value.BundleId);

    }

    private void OnFirstReward(S2CChargeFirstAwardResponse value)
    {
        mFirstChargeState = 2;
        List<ItemInfo> listInfo = new List<ItemInfo>();
        listInfo.AddRange(value.Rewards);
        Instance.DispathEvent(RechargeEvent.FirstRecharge, listInfo);
    }

    private void OnFirstRewardNot(S2CChargeFirstRewardNotify value)
    {
        mFirstChargeState = 1;
        Instance.DispathEvent(RechargeEvent.FirstRechargeNot);
    }

    public static void DoRechargeData(S2CChargeDataResponse value)
    {
        Instance.OnRechargeData(value);
    }

    public static void DoRecharge(S2CChargeResponse value)
    {
        Instance.OnRecharge(value);
    }

    public static void DoFirstReward(S2CChargeFirstAwardResponse value)
    {
        Instance.OnFirstReward(value);
    }

    public static void DoFirstRewardNot(S2CChargeFirstRewardNotify value)
    {
        Instance.OnFirstRewardNot(value);
    }

    protected override void DoClearData()
    {
        base.DoClearData();
        if (mListMonthCard != null)
            mListMonthCard.Clear();
    }
}
