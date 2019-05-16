public class NetErrorCode
{
    public const int None = 0;
    public const int Internal = -1;
    public const int PlayerAccOrPsdError = -2;
    public const int PlayerNotExist = -3;
    public const int PlayerAlreadyLogin = -4;
    public const int PlayerOtherPlaceLogin = -5;
    public const int PlayerTokenError = -6;
    public const int PlayerSelectSvrNotFound = -7;
    public const int PlayerAlreadSelectSvr = -8;
    public const int PlayerGoldNotEnough = -11;//金币不足
    public const int PlayerDiamondNotEnough = -12;//钻石不足

    public const int NeedReconnectGameSvr = -17;
    public const int PlayerIDInvalid = -20;
    public const int PlayerIDNotFound = -21;

    public const int AccountAlreadyRegister = -100;
    public const int AccountInvalid = -101;
    public const int AccountPasswordInvalid = -102;
    public const int AccountNotRegisted = -103;
    public const int AccountNotGuest = -104;
    public const int AccountBindAlreadyExists = -105;

    public const int SetAttackMembersFailed = -10000;
    public const int SetDefenseMembersFailed = -10001;
    public const int PlayerIsDefensing = -10002;

    public const int ItemNumNotEnough = -11002;//物品数量不足
    public const int ItemFusionFailed = -11006;//不能合成物品
    public const int ItemCountNotEnough = -11007;//合成碎片数量不足
    public const int ItemUpResNotEnough = -11009;//物品升级资源不足
    public const int ItemOneUpNotEnough = -11013;//装备一键合成材料不足

    public const int RoleNotFound = -12000;//角色不存在
    public const int RoleLevelDataNotFound = -12002;//角色升级数据不存在
    public const int RoleLevelIsMax = -12003;//角色等级已经最大
    public const int RoleRankIsMax = -12004;//角色品阶已经最大
    public const int RoleRankUpDataNotFound = -12005;//角色品阶升级数据不存在
    public const int RoleInTeamCantDecompose = -12006;//角色在阵容中，不能分解
    public const int RoleFusionFailed = -12007;//角色合成失败
    public const int FusionNeedRoleNotFound = -12008;//合成角色需要的角色不存在
    public const int FusionMainRoleNotFound = -12011;//角色合成主角色不存在
    public const int FusionRoleMaterialNotEnough = -12012;//角色合成材料数量不足
    public const int FusionNeedResourceNotEnough = -12014;//角色合成需要的资源不足

    public const int AlreadyFightCampign = -13001;//已打过该战役
    public const int CantFightTheCampaign = -13004;//不能打该战役
    public const int DiamondNotEnoughForAccel = -13007;//钻石不足，无法加速战斗

    public const int MailNotFound = -14001;//邮件不存在

    public const int TalentLevelIsMax = -15001;//天赋等级已满
    public const int TalentUpNotEnoughResource = -15002;//天赋升级没有足够资源

    public const int TowerAlreadyFighted = -16000;//已经打过塔层
    public const int TowerCantFight = -16005;//不能打该层塔

    public const int GoldHandRefreshIsCoolingdown = -17001;//点金手刷新CF中

    public const int ArenaIsReseting = -19002;//竞技场正在重置
    public const int ArenaSeasonIsReseting = -19003;//竞技场赛季正在重置
    public const int ArenaTicketsNotEnough = -19004;//竞技场门票不足

    public const int ActiveStagePurchaseNumOut = -20100;//活动副本购买挑战次数用完
    public const int ActiveStageChallengeNumMax = -20101;//活动副本挑战次数最大
    //public const int ActiveStageLevelNotEnough = -20102;//活动副本玩家等级不够

    public const int FriendPlayerNoInAskList = -21002;//玩家不在申请列表中
    public const int FriendNotFound = -21003;//没有该好友
    public const int FriendBossIsFighting = -21006;//好友BOSS正在被挑战
    public const int FriendBossIsFinished = -21007;//好友BOSS已经结束
    public const int FriendBossNotFound = -21008;//好友BOSS不存在

    public const int TaskNotFound = -22000;//任务不存在
    public const int TaskNotComplete = -22001;//任务未完成
    public const int TaskNotReward = -22002;//任务未领奖
    public const int TaskAlreadyRewarded = -22003;//任务已领奖
    public const int TaskPrevNotComplete = -22004;//前置任务未完成

    public const int ExploreAlreadyStarted = -23003;//探索任务已经开始
    public const int ExploreCantUnlockIfStarted = -23008;//探索任务已经开始不能解锁
    public const int ExploreIsIncomplete = -12009;//探索任务未完成
    public const int ExploreNoFightBossState = -23010;//探索任务的状态不能击打BOSS
    public const int ExploreStateNotStarted = -23011;//探索任务不是进行状态

    public const int GuildAlreadyCreatedOrJoined = -24000;//玩家已经创建或加入联盟
    public const int GuildIsAlreadyMember = -24010;//已经是联盟成员
    public const int GuildAlreadyAskedDonate = -24019;//联盟已经请求过捐赠
    public const int GuildNotAskJoin = -24023;//玩家未申请加入联盟
    public const int GuildStageIsFighting = -25003;//联盟副本有玩家正在挑战
    public const int GUildStageCantFighting = -25004;//联盟副本不能挑战

    public const int AlreadyAward = -27001;//已领取过奖励
    public const int MustAwardInSequence = -27002;//必须按签到顺序领奖
    public const int AllAwarded = -27003;//签到的奖励都已领完

    public const int DaysFinished = -28000;//活动已结束
    public const int DatsAwarded = -28001;//已领奖

    public const int ChargeOrdeExist = -29019; //apple支付订单已存在
}

public class NetErrorHelper
{
    private static int _reLoginCount = 0;
    public static void DoErrorCode(int errorCode)
    {
        LoadingMgr.Instance.HideRechargeMask();
        switch (errorCode)
        {
            case NetErrorCode.NeedReconnectGameSvr:
                _reLoginCount = 0;
                GameNetMgr.Instance.mGameServer.ReqReconnect();
                break;
            case NetErrorCode.PlayerIDInvalid:
            case NetErrorCode.PlayerIDNotFound:
                _reLoginCount++;
                if (_reLoginCount >= 3)
                {
                    return;
                }
                LoginHelper.ReLogin(LocalDataMgr.PlayerAccount, LocalDataMgr.Password, LocalDataMgr.LoginChannel);
                break;
            case NetErrorCode.PlayerGoldNotEnough:
            case NetErrorCode.PlayerDiamondNotEnough:
            case NetErrorCode.DiamondNotEnoughForAccel:
                GameNetMgr.Instance.mGameServer.ReqDataSync(GameServerDataSync.Base);
                break;
            case NetErrorCode.ItemNumNotEnough:
            case NetErrorCode.ItemFusionFailed:
            case NetErrorCode.ItemCountNotEnough:
            case NetErrorCode.ItemUpResNotEnough:
            case NetErrorCode.ItemOneUpNotEnough:
            case NetErrorCode.ArenaTicketsNotEnough:
                GameNetMgr.Instance.mGameServer.ReqDataSync(GameServerDataSync.Items);
                break;
            case NetErrorCode.RoleNotFound:
            case NetErrorCode.RoleLevelDataNotFound:
            case NetErrorCode.RoleLevelIsMax:
            case NetErrorCode.RoleRankIsMax:
            case NetErrorCode.RoleRankUpDataNotFound:
            case NetErrorCode.RoleInTeamCantDecompose:
            case NetErrorCode.RoleFusionFailed:
            case NetErrorCode.FusionNeedRoleNotFound:
            case NetErrorCode.FusionMainRoleNotFound:
            case NetErrorCode.FusionRoleMaterialNotEnough:
            case NetErrorCode.FusionNeedResourceNotEnough:
                GameNetMgr.Instance.mGameServer.ReqDataSync(GameServerDataSync.Roles);
                break;
            case NetErrorCode.AlreadyFightCampign:
            case NetErrorCode.CantFightTheCampaign:
                GameNetMgr.Instance.mGameServer.ReqDataSync(GameServerDataSync.Campaigns);
                break;
            case NetErrorCode.MailNotFound:
                GameNetMgr.Instance.mGameServer.ReqDataSync(GameServerDataSync.Mail);
                break;
            case NetErrorCode.TalentLevelIsMax:
            case NetErrorCode.TalentUpNotEnoughResource:
                GameNetMgr.Instance.mGameServer.ReqDataSync(GameServerDataSync.Talent);
                break;
            case NetErrorCode.TowerAlreadyFighted:
            case NetErrorCode.TowerCantFight:
                GameNetMgr.Instance.mGameServer.ReqDataSync(GameServerDataSync.Tower);
                break;
            case NetErrorCode.GoldHandRefreshIsCoolingdown:
                GameNetMgr.Instance.mGameServer.ReqDataSync(GameServerDataSync.GoldHand);
                break;
            case NetErrorCode.ArenaIsReseting:
            case NetErrorCode.ArenaSeasonIsReseting:
                GameNetMgr.Instance.mGameServer.ReqDataSync(GameServerDataSync.Arena);
                break;
            case NetErrorCode.ActiveStagePurchaseNumOut:
            case NetErrorCode.ActiveStageChallengeNumMax:
                GameNetMgr.Instance.mGameServer.ReqDataSync(GameServerDataSync.ActiveStage);
                break;
            //case NetErrorCode.ActiveStageLevelNotEnough:
            //    GameNetMgr.Instance.mGameServer.ReqDataSync(GameServerDataSync.Base, GameServerDataSync.ActiveStage);
            //    break;
            case NetErrorCode.FriendPlayerNoInAskList:
            case NetErrorCode.FriendNotFound:
            case NetErrorCode.FriendBossIsFighting:
            case NetErrorCode.FriendBossIsFinished:
            case NetErrorCode.FriendBossNotFound:
                GameNetMgr.Instance.mGameServer.ReqDataSync(GameServerDataSync.Friend);
                break;
            case NetErrorCode.TaskNotFound:
            case NetErrorCode.TaskNotComplete:
            case NetErrorCode.TaskNotReward:
            case NetErrorCode.TaskAlreadyRewarded:
            case NetErrorCode.TaskPrevNotComplete:
                GameNetMgr.Instance.mGameServer.ReqDataSync(GameServerDataSync.Task);
                break;
            case NetErrorCode.ExploreAlreadyStarted:
            case NetErrorCode.ExploreCantUnlockIfStarted:
            case NetErrorCode.ExploreIsIncomplete:
            case NetErrorCode.ExploreNoFightBossState:
            case NetErrorCode.ExploreStateNotStarted:
                GameNetMgr.Instance.mGameServer.ReqDataSync(GameServerDataSync.Explore);
                break;
            case NetErrorCode.GuildAlreadyCreatedOrJoined:
            case NetErrorCode.GuildIsAlreadyMember:
            case NetErrorCode.GuildAlreadyAskedDonate:
            case NetErrorCode.GuildNotAskJoin:
            case NetErrorCode.GuildStageIsFighting:
            case NetErrorCode.GUildStageCantFighting:
                GameNetMgr.Instance.mGameServer.ReqDataSync(GameServerDataSync.Guild);
                break;
            case NetErrorCode.AlreadyAward:
            case NetErrorCode.MustAwardInSequence:
            case NetErrorCode.AllAwarded:
                GameNetMgr.Instance.mGameServer.ReqDataSync(GameServerDataSync.Sign);
                break;
            case NetErrorCode.DaysFinished:
            case NetErrorCode.DatsAwarded:
                GameNetMgr.Instance.mGameServer.ReqDataSync(GameServerDataSync.SevenDays);
                break;
            case NetErrorCode.ChargeOrdeExist:
                LocalDataMgr.RemoveAllOrderData();
                break;
            default:
                _reLoginCount = 0;
                PopupTipsMgr.Instance.ShowTips(errorCode);
                break;
        }
    }
}