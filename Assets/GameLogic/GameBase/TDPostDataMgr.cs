using System.Collections.Generic;

public class TDCostDiamondType
{
    public static string BuyArenaCount = "buyArenaCount";
    public static string BuyTowerTicket = "BuyTowerTicket";
    public static string BuyExploreRefresh = "BuyExploreRefresh";
    public static string BuyActivityStage = "BuyActivityStage";
    public static string BuyExploreSpeed = "BuyExploreSpeed";
    public static string BuyHangupSpeed = "BuyHangupSpeed";
    public static string BuyPlayerModifyName = "BuyPlayerModifyName";
    public static string BuyMysteryShopCount = "buyMysteryShopCount";
    public static string BuyMysteryRefreshCount = "buyMysteryRefreshCount";
    public static string BuyTalentRefreshCount = "buyTalentRefreshCount";
    public static string BuyGuildBossRefreshCount = "buyGuildBossRefreshCount";
    public static string BuyGuildNameCount = "buyGuildNameCount";
    public static string BuyGoldCount = "buyGoldCount";
    public static string BuyRecruitOneCount = "buyRecruitOneCount";
    public static string BuyRecruitTenCount = "buyRecruitTenCount";
}

public class TDPostDataMgr : Singleton<TDPostDataMgr>
{

    public void DoSetAccount(string acc, int svrID)
    {
#if VRelease
        TDGAAccount account = TDGAAccount.SetAccount(acc);
        //account.SetGameServer("server_" + svrID);
#endif
    }

    public void DoUpdateLevel(int level)
    {
#if VRelease
        TDGAAccount acc = TDGAAccount.SetAccount(LocalDataMgr.PlayerAccount);
        acc.SetLevel(level);
#endif
    }

    public void DoCostDiamond(string type, int count, int diamondValue)
    {
#if VRelease
        TDGAItem.OnPurchase(type, count, diamondValue);
#endif
    }

    public void DoBattle(BattleType type, int param)
    {
#if VRelease
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("Param", param);
        TalkingDataGA.OnEvent(("BattleType_" + (int)type), dict);
#endif
    }

    public void DoNewBieStart(int guideIdx, int stepIdx)
    {
#if VRelease
        //Debuger.LogWarning("guideIdx:" + guideIdx + ", stepidx:" + stepIdx);
        TalkingDataGA.OnEvent("Guide" + guideIdx + "_" + stepIdx, null);
#endif
    }
}
