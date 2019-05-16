using System.Collections;
using System.Collections.Generic;
using Msg.ClientMessage;
using UnityEngine;

public class HeroCallModel : ModelDataBase<HeroCallModel>
{
    private const string HeroCallKey = "HeroCallKey";
    private const string HeroReplaceKey = "HeroReplaceKey";

    //置换得到的英雄id
    public int newRoleTableId { get; private set; }
    //召唤英雄得到的物品
    public List<int> LstReward { get; private set; }
    /// <summary>
    /// 向服务器发送召唤英雄协议
    /// </summary>
    /// <param name="value"></param>
    public void ReqHeroCall(int value)
    {
        if (CheckNeedRequest(HeroCallKey, 1.0f))
            GameNetMgr.Instance.mGameServer.ReqDrawCard(value);
        else
            DispathEvent(RecruitEvent.DrawCard);
    }
    /// <summary>
    /// 向服务器发送请求置换英雄协议
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="roleId"></param>
    public void ReqRoleDisplace(int groupId,int roleId)
    {
        if (CheckNeedRequest(HeroReplaceKey, 1.0f))
            GameNetMgr.Instance.mGameServer.ReqRoleDisplace(groupId, roleId);
        else
            DispathEvent(HeroCallEvent.HeroReplace);
    }
    private void OnHeroReplacement(S2CRoleDisplaceResponse value)
    {
        newRoleTableId = value.NewRoleTableId;
        DispathEvent(HeroCallEvent.HeroReplace);
    }
    public static void DoHeroReplacement(S2CRoleDisplaceResponse value)
    {
        Instance.OnHeroReplacement(value);
    }
    /// <summary>
    /// 向服务器发送二次确认协议
    /// </summary>
    public void ReqRoleDisplaceConfirm()
    {
        GameNetMgr.Instance.mGameServer.ReqRoleDisplaceComfirm();
    }
    private void OnHeroReplacementConfirm(S2CRoleDisplaceConfirmResponse value)
    {
        DispathEvent(HeroCallEvent.HeroReplaceConfirm);
    }
    public static void DoHeroReplacementConfirm(S2CRoleDisplaceConfirmResponse value)
    {
        Instance.OnHeroReplacementConfirm(value);
    }

}
