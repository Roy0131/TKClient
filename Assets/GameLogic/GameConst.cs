using System.Collections.Generic;
using UnityEngine;

public class GameConst
{
    public static readonly bool ISDEVELOPER = true;

    public static readonly int BATTLE_MOVE_TIME = 5;

    public static readonly int BATTLE_MOVE_SPEED = 15;

    public static readonly int ENERGY_MAX = 60;

    public static readonly string[] CARD_TYPE_ICONS = new string[3] { "campicon/cardtype1", "campicon/cardtype2", "campicon/cardtype3" };

    public static readonly int AttackerSortLayerAdd = 5;

    public static readonly int EquipForgeLen = 15;

    public static readonly int FriendBossCDTime = 60 * 60 * 8;

    public static readonly int FriendBossStrengthMax = 10;

    public static readonly int ModifyGuildNameCost = 1000;

    public static readonly int GuildCreateCost = 1000;

    public static readonly int GuildDonateIntegralUp = 300;

    public static readonly List<int> AttrListShow = new List<int>() { AttributesType.HP, AttributesType.ATTACK,  AttributesType.DEFENSE, AttributesType.CRIT,
                                                                      AttributesType.CRIT_DAMAGE, AttributesType.CRIT_RESISTANCE,  AttributesType.BLOCK, AttributesType.BLOCK_REDUCE,
                                                                      AttributesType.BLOCK_BREAK, AttributesType.ARMOR_GAIN,  AttributesType.ARMOR_SUNDER, AttributesType.INJURY,
                                                                      AttributesType.INJURY_FREE
                                                                    };

    public static readonly List<SystemLanguage> AllLanguages = new List<SystemLanguage>() { SystemLanguage.Chinese, SystemLanguage.English };
    public static readonly List<string> AllLanguageTitles = new List<string>() { "简体中文", "english" };

    public static readonly int GuildDonateTime = 8 * 60 * 60;

    public static readonly int TalentReset = 10000;

    public static readonly int PlayerNameNum = 200;

    public static readonly int CardBagNum = 500;

    public static readonly int SevenTime = 60 * 60 * 24 * 6;

    public static readonly string FirstReward = "56054,50,2,20000,15051,1";

    public static readonly int MaxFriendship = 300;

    public static readonly int AccelerateLevel = 40;

    public static readonly int PauseTime = 180;

    public static readonly int MailInputText = 120;

    public static readonly int GuildInputText = 50;

    public static readonly int PurifyPointReward = 20;

    public static readonly string Version = "v1.1.6";

    public static readonly string TranslateUrl = "http://coz2gamechat.moyuplay.com:10037/translateJSON";

    public static int GetEquipTypeLanguageId(int equipType)
    {
        int languageID = 0;
        switch (equipType)
        {
            case EquipmentType.Weapon:
                languageID = 210101;
                break;
            case EquipmentType.Clothes:
                languageID = 210102;
                break;
            case EquipmentType.Helmet:
                languageID = 210103;
                break;
            case EquipmentType.Shoes:
                languageID = 210104;
                break;
            case EquipmentType.GemStone:
                languageID = 210105;
                break;
            case EquipmentType.Artifact:
                languageID = 210106;
                break;
        }
        return languageID;
    }

    public static int GetItemTypeLanguageId(int itemType,int equipType)
    {
        int languageID = 0;
        switch (itemType)
        {
            case 2:
                languageID = GetEquipTypeLanguageId(equipType);
                break;
            case 1:
            case 3:
                languageID = 210123;
                break;
            case 4:
                languageID = 210124;
                break;
        }
        return languageID;
    }

    public static int GetFeatureType(int featureType)
    {
        string ServerId = "";
        switch (featureType)
        {
            case FunctionType.Explore:
                ServerId = "SearchTaskEnterLevel";
                break;
            case FunctionType.Arena:
                ServerId = "ArenaEnterLevel";
                break;
            case FunctionType.Tower:
                ServerId = "TowerEnterLevel";
                break;
            case FunctionType.Guild:
                ServerId = "GuildEnterLevel";
                break;
            case FunctionType.FriendBoss:
                ServerId = "FriendBossEnterLevel";
                break;
            case FunctionType.HeroCall:
                ServerId = "LifeTreeEnterLevel";
                break;
            case FunctionType.Expedition:
                ServerId = "ExpeditionEnterLevel";
                break;
            case FunctionType.Artifact:
                ServerId = "ArtifactEnterLevel";
                break;
        }
        return GameConfigMgr.Instance.GetSystemUnlockConfig(ServerId).Level;
    }

    public static int GetLimitedLanguageId(int eventId)
    {
        int languageID = 0;
        switch (eventId)
        {
            case 1:
                languageID = 5007504;
                break;
            case 2:
                languageID = 5007501;
                break;
            case 3:
                languageID = 5007506;
                break;
            case 4:
                languageID = 5007515;
                break;
            case 5:
                languageID = 5007507;
                break;
            case 6:
                languageID = 5007517;
                break;
            case 7:
                languageID = 5007509;
                break;
            case 8:
                languageID = 5007511;
                break;
            case 9:
                languageID = 5007513;
                break;
            case 10:
                languageID = 5007519;
                break;
            case 11:
                languageID = 5007522;
                break;
            case 100:
                languageID = 500011;
                break;
            case 101:
                languageID = 500012;
                break;
            case 102:
                languageID = 5007301;
                break;
            case 103:
                languageID = 5007102;
                break;
        }
        return languageID;
    }

    public static string HeroCallIndex(int index)
    {
        int languageId = 0;
        switch (index)
        {
            case 0:
                languageId = 220001;
                break;
            case 1:
                languageId = 220002;
                break;
            case 2:
                languageId = 220003;
                break;
            case 3:
                languageId = 220004;
                break;
            case 4:
                languageId = 220005;
                break;
            case 5:
                languageId = 220006;
                break;
        }
        return LanguageMgr.GetLanguage(languageId);
    }

    public static string CarnivalName(int index)
    {
        int languageId = 0;
        switch (index)
        {
            case CarnivalConst.Comment:
                languageId = 5007601;
                break;
            case CarnivalConst.Attention:
                languageId = 5007604;
                break;
            case CarnivalConst.Share:
                languageId = 5007607;
                break;
            case CarnivalConst.InviteFriend:
                languageId = 5007610;
                break;
            case CarnivalConst.Exchange:
                languageId = 5007618;
                break;
            case CarnivalConst.InviteAward:
                languageId = 5007614;
                break;
        }
        return LanguageMgr.GetLanguage(languageId);
    }
}

public enum ItemTipsType
{
    NormalTips,
    EquipBagTips,
    RoleEquipTips,
    FragmentTips,
}

public class ActionName
{
    public static readonly string Idle = "idle";
    public static readonly string Death = "death";
    public static readonly string Hit = "hit";
    public static readonly string Attack = "attack";
}

public class GameLayer
{
    public static readonly int SceneLayer = 8;
    public static readonly int NpcLayer = 9;
    //public static readonly int HangupLayer = 10;
    //public static readonly int LineupLayer = 11;
    //public static readonly int RolePreview = 12;
    public static readonly int ModeUILayer = 10;
}

public class RenderLayerOffset
{
    public const int None = 0;

    public const int FighterAttack = 20;
    public const int Bullet = 30;
    public const int TopBullet = 40;
    public const int EffectTop = 50;
    public const int EffectBottom = -50;

    public const int Max = 100;
}

public class EquipmentType
{
    /// <summary>
    /// 非装备
    /// </summary>
    public const int None = 0;
    /// <summary>
    /// 武器
    /// </summary>
    public const int Weapon = 1;
    /// <summary>
    /// 衣服
    /// </summary>
    public const int Clothes = 2;
    /// <summary>
    /// 头盔
    /// </summary>
    public const int Helmet = 3;
    /// <summary>
    /// 鞋子
    /// </summary>
    public const int Shoes = 4;
    /// <summary>
    /// 宝石
    /// </summary>
    public const int GemStone = 5;
    /// <summary>
    /// 神器
    /// </summary>
    public const int Artifact = 6;
}

public enum EquipSlotStatu
{
    None,
    /// <summary>
    /// 未解锁
    /// </summary>
    Locked, 
    /// <summary>
    /// 可解锁
    /// </summary>
    Unlockable,
    /// <summary>
    /// 已解锁,未装备数据
    /// </summary>
    Unlocked,
    /// <summary>
    /// 已有装备
    /// </summary>
    HasEqiup,
}

public enum UIFriendType : ushort
{
    None,
    FriendList,
    RecommemdFriend,
    FriendAskList,
    FriendBoss,
}

public enum UIGuildType : ushort
{
    None, 
    Recommemd,
    Create,
    Search,
}

public enum GuildOfficeType : ushort
{
    Member,//成员
    President,//会长
    Office,//官员
}

public class HelpType
{
    /// <summary>
    /// 战役帮助
    /// </summary>
    public const int HangupHelp = 4100002;

    /// <summary>
    /// 爬塔帮助
    /// </summary>
    public const int TowerHelp = 4100003;

    /// <summary>
    /// 装备升级帮助
    /// </summary>
    public const int EquipmentUpHelp = 4100004;

    /// <summary>
    /// 竞技场帮助
    /// </summary>
    public const int ArenaHelp = 4100005;

    /// <summary>
    /// 抽卡帮助
    /// </summary>
    public const int DrawHelp = 4100006;

    /// <summary>
    /// 抽卡帮助
    /// </summary>
    public const int DrawProHelp = 4100007;
    /// <summary>
    /// 生命古树帮助
    /// </summary>
    public const int DrawLifeHelp = 4100019;

    /// <summary>
    /// 生命古树帮助
    /// </summary>
    public const int DrawLifeProHelp = 4100020;
    /// <summary>
    /// 英雄合成帮助
    /// </summary>
    public const int HeroComHelp = 4100008;

    /// <summary>
    /// 分解帮助
    /// </summary>
    public const int BreakHelp = 4100009;

    /// <summary>
    /// 探索帮助
    /// </summary>
    public const int ExploreHelp = 4100010;

    /// <summary>
    /// 好友助战帮助
    /// </summary>
    public const int FriendHelp = 4100011;

    /// <summary>
    /// 金币副本帮助
    /// </summary>
    public const int GoldCopyHelp = 4100012;

    /// <summary>
    /// 强者副本帮助
    /// </summary>
    public const int StrongCopyHelp = 4100013;

    /// <summary>
    /// 英雄副本帮助
    /// </summary>
    public const int HeroCopyHelp = 4100014;

    /// <summary>
    /// 联盟帮助
    /// </summary>
    public const int GuildHelp = 4100015;

    /// <summary>
    /// 联盟副本帮助
    /// </summary>
    public const int GuildBossHelp = 4100016;

    /// <summary>
    /// 高级抽卡帮助
    /// </summary>
    public const int HighDrawHelp = 4100017;
    
    /// <summary>
    /// 签到帮助
    /// </summary>
    public const int SignInHelp = 5007203;

    /// <summary>
    /// 远征帮助
    /// </summary>
    public const int ExpeditionHelp = 4100022;

    /// <summary>
    /// 神器帮助
    /// </summary>
    public const int ArtifactHelp = 4100023;
}

public class FunctionType
{
    /// <summary>
    /// 探索
    /// </summary>
    public const int Explore = 1;

    /// <summary>
    /// 竞技场
    /// </summary>
    public const int Arena = 2;

    /// <summary>
    /// 爬塔
    /// </summary>
    public const int Tower = 3;

    /// <summary>
    /// 联盟
    /// </summary>
    public const int Guild = 4;

    /// <summary>
    /// 好友Boss
    /// </summary>
    public const int FriendBoss = 5;

    /// <summary>
    /// 英雄召唤
    /// </summary>
    public const int HeroCall = 6;

    /// <summary>
    /// 远征
    /// </summary>
    public const int Expedition = 7;

    /// <summary>
    /// 神器
    /// </summary>
    public const int Artifact = 8;
}

public class GameLoginType
{
    public const int GUEST = 0;
    public const int FACEBOOK = 1;
    public const int INPUTACCOUNT = 2;

    public static string GetChannelValue(int loginChannel)
    {
        switch(loginChannel)
        {
            case GUEST:
                return "guest";
            case FACEBOOK:
                return "facebook";
        }
        return "";
    }

}

public enum GameServerDataSync
{
    /// <summary>
    /// 玩家信息
    /// </summary>
    Base,

    /// <summary>
    /// 背包
    /// </summary>
    Items,

    /// <summary>
    /// 英雄
    /// </summary>
    Roles,

    /// <summary>
    /// 阵容
    /// </summary>
    Teams,

    /// <summary>
    /// 战役
    /// </summary>
    Campaigns,

    /// <summary>
    /// 爬塔
    /// </summary>
    Tower,

    /// <summary>
    /// 竞技场
    /// </summary>
    Arena,

    /// <summary>
    /// 任务
    /// </summary>
    Task,

    /// <summary>
    /// 活动副本
    /// </summary>
    ActiveStage,

    /// <summary>
    /// 联盟
    /// </summary>
    Guild,

    /// <summary>
    /// 好友
    /// </summary>
    Friend,

    /// <summary>
    /// 聊天
    /// </summary>
    Chat,

    /// <summary>
    /// 探索
    /// </summary>
    Explore,

    /// <summary>
    /// 邮件
    /// </summary>
    Mail,

    /// <summary>
    /// 签到
    /// </summary>
    Sign,

    /// <summary>
    /// 七天乐
    /// </summary>
    SevenDays,

    /// <summary>
    /// 天赋
    /// </summary>
    Talent,

    /// <summary>
    /// 点金手
    /// </summary>
    GoldHand,

    /// <summary>
    /// 引导
    /// </summary>
    Guide,
}

public class ScreenType
{
    public static readonly int Chart = 1;//图鉴
    public static readonly int Welfare = 2;//限时活动
    public static readonly int Activity = 3;//福利
}