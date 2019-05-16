public class UILayerSort
{
    public const int ModuleSortBeginner = 11;
    public const int WindowSortBeginner = 21;
    public const int PopupSortBeginner = 31;
    public const int TopSortBeginner = 41;
}

public enum UILayer
{
    Module,
    Window,
    Popup,
}

public enum ModuleID
{
    Console,
    Home,
    Battle,
    Hangup,
    Lineup,
    CampaignMap,
    Bag,
    CTower,
    Mail,
    RoleBag,
    RoleInfo,
    RoleRankup,
    BeforeBattle,
    TowerVideo,
    TowerRank,
    Recruit,
    RoleSelect,

    RoleFusion,
    RoleDecompose,
    Equipment,

    ActivityCopy,
    ActivityFriend,

    Explore,
    ExploreGroup,
    ExploreBroadcast,
    ExploreHeroBag,
    Gold,
    EquipFunc,
    MysteryShop,
    HeroShop,
    Arena,
    Task,
    Friend,
    Rank,

    Setting,
    Chat,

    Talent,

    #region module
    Guild,
    HeroGuild,
    MemberMgr,
    GuildBoss,
    #endregion

    Player,
    Recharge,

    HeroCall,

    Attendance,
    Welfare,
    Expedition,

    Artifact,
    Carnival,
}

public static class SingletonResName
{
    public const string UICardItem = "uiCardItem";
    public const string UILoading = "uiLoading";
    public const string UINewBieGuide = "uiNewbieGuide";
    public const string UIItem = "uiBagItem";
    public const string UIConfirm = "uiConfirmTips";
    public const string UIGetItem = "uiGetItem";
    public const string UIEquipTips = "uiEquipTips";
    public const string UIRewardTips = "uiRewardTips";
    public const string UITips = "uiTips";

    public const string UIHpBar = "uiHpBar";
    public const string UIBlood = "uiBlood";
    public const string UIBuffTips = "uiBuffTips";
    public const string UIRecconetTips = "uiDxcl";

    public const string UIHelpTips = "uiHelpWindow";
    public const string UIFunction = "uiFunction";
    public const string UILevelUp = "uiPlayerLevelUp";
    public const string UIMailSend = "uiMailSend";
    public const string UIPlayerInfo = "uiPlayerInfo";
    public const string UILantern = "uiLantern";
}

public class UIModuleResName
{
    public static readonly string UI_Console = "uiConsole";
    public static readonly string UI_Home = "uiHome";
    public static readonly string UI_Battle = "uiBattle";
    public static readonly string UI_Hangup = "uiHangup";
    public static readonly string UI_Lineup = "uiLineup";//排兵布阵ui界面
    public static readonly string UI_CampaignMap = "uiMapSelect";
    public static readonly string UI_Bag = "uiBag";
    public static readonly string UI_CTower = "uiCTower";
    public static readonly string UI_BeforeBattle = "uiBeforeBattle";
    public static readonly string UI_TowerVideo = "uiVideoWindow";
    public static readonly string UI_TowerRank = "uiTowerRank";
    public static readonly string UI_Mail = "uiMail";

    public static readonly string UI_RoleBag = "uiRoleBag";
    public static readonly string UI_RoleInfo = "uiHeroinfo";

    public static readonly string UI_RoleRankup = "uiRankup";

    public static readonly string UI_Recruit = "uiRecruit";

    public static readonly string UI_RoleSelect = "uiRoleSelect";

    public static readonly string UI_RoleFusion = "uiRoleFusion";
    public static readonly string UI_RoleDecompose = "uiRoleDecompose";
    public static readonly string UI_Equipment = "uiEquipment";

    public static readonly string UI_Activity = "uiActivityCopy";
    public static readonly string UI_ActivityFriend = "uiActivityFriend";

    public static readonly string UI_Explore = "uiExplore";
    public static readonly string UI_ExploreGroup = "uiExploreGroup";
    public static readonly string UI_ExploreBroadcast = "uiExploreBroadcast";
    public static readonly string UI_ExploreHeroBag = "uiExploreHerobag";

    public static readonly string UI_Gold = "uiGold";

    public static readonly string UI_EquipFunc = "uiEquipFunction";

    public static readonly string UI_MysteryShop = "uiMysteryShop";
    public static readonly string UI_HeroShop = "uiHeroShop";

    public static readonly string UI_Arena = "uiArena";

    public static readonly string UI_Task = "uiTask";

    public static readonly string UI_Friend = "uiFriend";

    public static readonly string UI_Rank = "uiRank";

    public static readonly string UI_Setting = "uiSetting";

    public static readonly string UI_Chat = "uiChat";

    public static readonly string UI_Talent = "uiTalent";

    #region guild
    public static readonly string UI_Guild = "uiGuild";
    public static readonly string UI_HeroGuild = "uiHeroGuild";
    public static readonly string UI_MemberMgr = "uiGuildMgr";
    public static readonly string UI_GuildBoss = "uiGuildBoss";
    #endregion

    public static readonly string UI_Player = "uiPlayer";

    public static readonly string UI_Welfare = "uiWelfare";

    public static readonly string UI_Rechatge = "uiRecharge";

    public static readonly string UI_HeroCall = "uiHeroCall";


    public static readonly string UI_Attendance = "uiAttendance";

    public static readonly string UI_Expedition = "uiExpedition";

    public static readonly string UI_Artifact = "uiArtifact";

    public static readonly string UI_Carnival = "uiCarnival";
}

public class UIModuleSoundName
{
    public static readonly string MysteryShopSoundName = "UI_shangdian_open";

    public static readonly string AranaSoundName = "UI_jingjichang_open";

    public static readonly string CTowerSoundName = "UI_dianti_open";

    public static readonly string ExplpreSoundName = "UI_tansuo_open";

    public static readonly string GoldSoundName = "UI_dianjin_open";

    public static readonly string HanguoSoundName = "UI_zhanyi_open";

    public static readonly string RecruitSoundName = "UI_jiuguan_open";

    public static readonly string RoleDecomposeSoundName = "UI_fenjie_open";

    public static readonly string RoleFusionSoundName = "UI_hecheng_Fuse";

    public static readonly string HeroCallSoundName = "UI_HeroCall_open";
}