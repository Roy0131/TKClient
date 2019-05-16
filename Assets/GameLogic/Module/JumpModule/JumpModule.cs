using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum JumpType
{
    Arena = 1,//竞技场
    Active = 2,//活动副本
    Explore = 3,//探索
    DrawCard = 4,//抽卡
    Battle = 5,//战役
    MysteryShop = 6,//神秘商店
    Friend = 7,//好友
    Gold = 8,//点金
    EquipMentComp = 9,//装备合成
    Tower = 10,//爬塔
    HeroBreak = 11,//英雄分解
    BreakShop = 12,//分解商店
    GuildShop = 13,//联盟商店
    FrendBoss = 14,//好友BOSS  暂未处理
    Guild = 15,//联盟
    ArtifactShop = 16,//远征商店
}


public class JumpModule
{
    public static void JumpType(JumpType type)
    {
        switch (type)
        {
            case global::JumpType.Arena:
                if (FunctionUnlock.IsUnlock(FunctionType.Arena))
                    ArenaDataModel.Instance.ReqArenaData();
                break;
            case global::JumpType.Active:
                GameUIMgr.Instance.OpenModule(ModuleID.ActivityCopy, false);
                break;
            case global::JumpType.Explore:
                if (FunctionUnlock.IsUnlock(FunctionType.Explore))
                    GameUIMgr.Instance.OpenModule(ModuleID.Explore, false);
                break;
            case global::JumpType.DrawCard:
                RecruitDataModel.Instance.ReqDrawCardList();
                break;
            case global::JumpType.Battle:
                HangupDataModel.Instance.ReqCampaignData();
                break;
            case global::JumpType.MysteryShop:
                GameUIMgr.Instance.OpenModule(ModuleID.MysteryShop);
                break;
            case global::JumpType.Friend:
                GameUIMgr.Instance.OpenModule(ModuleID.Friend);
                break;
            case global::JumpType.Gold:
                GameUIMgr.Instance.OpenModule(ModuleID.Gold);
                break;
            case global::JumpType.EquipMentComp:
                GameUIMgr.Instance.OpenModule(ModuleID.Equipment);
                break;
            case global::JumpType.Tower:
                if (FunctionUnlock.IsUnlock(FunctionType.Tower))
                    CTowerDataModel.Instance.ReqTowerData();
                break;
            case global::JumpType.HeroBreak:
                GameUIMgr.Instance.OpenModule(ModuleID.RoleDecompose);
                break;
            case global::JumpType.BreakShop:
                GameUIMgr.Instance.OpenModule(ModuleID.HeroShop, ShopIdConst.HEROSHOP);
                break;
            case global::JumpType.GuildShop:
                if (FunctionUnlock.IsUnlock(FunctionType.Guild))
                {
                    if (HeroDataModel.Instance.mHeroInfoData.mGuildId == 0)
                        GameUIMgr.Instance.OpenModule(ModuleID.Guild);
                    else
                        GameUIMgr.Instance.OpenModule(ModuleID.HeroGuild);
                }
                break;
            case global::JumpType.Guild:
                if (FunctionUnlock.IsUnlock(FunctionType.Guild))
                {
                    if (HeroDataModel.Instance.mHeroInfoData.mGuildId == 0)
                        GameUIMgr.Instance.OpenModule(ModuleID.Guild);
                    else
                        GameUIMgr.Instance.OpenModule(ModuleID.HeroGuild);
                }
                break;
            case global::JumpType.ArtifactShop:
                if (FunctionUnlock.IsUnlock(FunctionType.Expedition))
                {
                    GameUIMgr.Instance.OpenModule(ModuleID.Expedition);
                    GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(ExpeditionEvent.OpenShop);
                }
                break;
        }
    }
}
