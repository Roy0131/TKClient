
namespace NewBieGuide
{
    public enum GuideType : ushort
    {
        ModuleGuide,
        NameUIGuide, //名字
        AbilityGuide, //能力
        GoalGuide, //目标先把
        DialogGuide, //站位说明
    }

    public enum DialogStyle : ushort
    {
        None,
        LeftTop,
        LeftBottom,
        RightTop,
        RightBottom,
    }

    public enum GuideMaskType
    {
        None = 0,
        Global,
        Rect,
        Circle,
    }

    public class NewBieMaskID
    {
        #region GetItem
        public const int GetItemSureBtn = 10;
        #endregion

        #region recruit module 
        public const int RecruitBuild = 20;
        public const int RecruitNormal = 21;
        public const int RecruitAdvanceBtn = 22;
        public const int RecruitResultOkBtn = 23;
        public const int RecruitBackBtn = 24;
        #endregion

        #region campaign module
        public const int HangupBuild = 30;
        public const int HangupBattleBtn = 31;
        public const int LineUpBattleBtn = 32;
        public const int BattleInfoBtn = 33;
        public const int BattleExitBtn = 34;
        public const int HangupBackBtn = 35;
        public const int HangupChapterBtn = 36;
        public const int HangupSetCampaignBtn = 37;
        public const int HangupFixedRewardBtn = 38;
        public const int HangupChapterBtn2 = 39;
        public const int RestraintBtn = 301;
        #endregion

        #region bag module
        public const int BagModule = 40;
        public const int FragmentToggle = 41;
        public const int FragmentItemView = 42;
        public const int FragmentCall = 43;
        public const int FragmentCallOk = 44;
        public const int BagModuleBackBtn = 45;
        #endregion

        #region DeCompose module
        public const int DeComposeModule = 50;
        public const int DeComposeCardView = 51;
        public const int Disassemble = 52;
        public const int DisassembleOk = 53;
        public const int DeComposeDisBtn = 54;
        #endregion

        #region RoleBag module
        public const int RoleBagModule = 60;
        public const int RoleItemView = 61;
        public const int RoleLevelUp = 62;
        public const int HeroEquipment = 63;
        public const int HeroEquipSlot = 64;
        public const int EquipItemView = 65;
        public const int EquipUse = 66;
        public const int HeroInfoDisBtn = 67;
        public const int RoleBagDisBtn = 68;
        #endregion

        #region Boon module
        public const int BoonModule = 80;
        public const int BoonCheck = 81;
        public const int BoonDisBtn = 82;
        #endregion

        #region Equipment module
        public const int EquipmentModule = 90;
        public const int EquipItem = 91;
        public const int Forge = 92;
        public const int EquipmentDisBtn = 93;
        #endregion

        #region arena module
        public const int ArenaDefenseBtn = 100;
        public const int ArenaMatchBtn = 101;
        public const int ArenaMatchBattleBtn = 102;
        public const int ArenaDisBtn = 103;
        #endregion
    }

    public class EnterCondConst
    {
        public const int None = 0;

        public const int EnterBattleScene = 1;
        public const int BattleRewardShow = 2;

        public const int DrawShow = 21;

        public const int LineUpShow = 31;
        public const int BattleBackHome = 33;

        public const int HangupProgressOver = 70;

        public const int EquipmentOpen = 90;
    }

    public class EndConditionConst
    {
        public const int BattleRequestBack = 1; //战斗请求返回
        public const int BattleEnd = 2;

        public const int RecriutModuleOpen = 20; //抽卡界面打开
        public const int RecriutResult = 21;//抽卡结果返回，动画结束

        public const int HangupModuleOpen = 30; //挂机界面打开
        public const int HangupChapterTipEnd = 32; //切换战斗地图后，倒计时提示

        public const int BagModuleOpen = 40;//打开背包

        public const int DeComposeReward = 50;

        public const int RoleBagModuleOpen = 60;//打开角色背包

        public const int BoonOpen = 80;
        public const int EquipForgeAnimationEnd = 90; //

        public const int ArenaModuleOpen = 100; //竞技场界面打开
        public const int ArenaMatchResult = 101; //竞技场匹配玩家成功
        public const int ArenaSetDefenseResult = 102; //竞技场设置阵营成功
        public const int ArenaCardRewardEnd = 103;//竞技场翻牌结束

    }

    public class GuideJumpCondConst
    {
        public const int RecruitNormalJump = 21;
        public const int RecruitAdvanceBtn = 22;

        public const int BattleBtn1 = 31;
        public const int BattleBtn2 = 32;

        public const int RoleLevel1 = 61;
        public const int RoleLevel2 = 62;
        public const int RoleLevel3 = 63;
        public const int RoleLevel4 = 64;

        public const int HangupChapter2 = 71;
        public const int HangupChapter3 = 72;

        public const int EquipCount = 90;

        public const int ArenaDefense = 100;
        public const int ArenaJump = 101;
    }

    public class GuideSpecialID
    {
        public const int GuideLineUp = 31;
        public const int GuideLineUp2 = 32;
        public const int GuideSpecial3 = 33;
    }
}