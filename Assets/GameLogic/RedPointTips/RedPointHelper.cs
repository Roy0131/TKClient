
public enum RedPointEnum //: int
{
    None,
    Task = 1, // 成就任务(有奖励)             按位与运算大于0 :  0x01 成就任务          0x02 每日任务
    Welfare = 2, // 福利(有奖励)				按位与运算大于0 :  0x01 首充              0x02 七天乐
    Campain = 3, // 战役(有随机战利品)			按位与运算大于0 :  0x01 有战利品
    Draw = 4, // 抽卡(免费抽卡)				按位与运算大于0 :  0x01 普通招募          0x02 高级招募
    Explore = 5, // 探索(有奖励)				按位与运算大于0 :  0x01 有奖励
    Chat = 6, // 聊天(有新聊天消息)            按位与运算大于0 :  0x01 世界频道          0x02 公会频道         0x04 招募频道
    Mail = 7, // 邮件(有未读邮件)              按位与运算大于0 :  0x01 有未读邮件
    Friend = 8, // 好友                      按位与运算大于0 :  0x01 好友助战可以搜索   0x02 有新好友申请
    GoldHand = 9,// 点金手(免费点金)          按位与运算大于0 :  0x01 有免费点金
    Guild = 10, // 公会(签到)                按位与运算大于0 :  0x01 可签到
    Sign = 11, //签到
    EquipFusion = 30, //装备合成
    EquipFusionType1 = 3001,
    EquipFusionType2 = 3002,
    EquipFusionType3 = 3003,
    EquipFusionType4 = 3004,

    RoleFusion = 31, //角色合成

    BagFragment = 40,//背包碎片


    Achieve_Task = 101, //成就任务
    DAILY_TASK = 102,  //每日任务

    FirstCharge = 201,//首充
    Seven = 202, //七天乐

    NormalDraw = 401, //普通招募
    AdvanceDraw = 402,//高级招募

    WorldChat = 601, //世界频道
    GuildChat = 602, //公会频道
    RecruitChat = 603, //招募频道

    FriendAssit = 801, //好友助战
    FriendApply = 802, //好友申请
}

public class RedPointConst
{
    public const int Bit_1 = 0x01;
    public const int Bit_2 = 0x02;
    public const int Bit_4 = 0x04;
}

public class RedPointHelper
{
    public static RedPointEnum GetRedPointEnum(int value)
    {
        //if (CheckRedPointEnum(value))
        return (RedPointEnum)value;
        //return RedPointEnum.None;
    }

    //public static bool CheckRedPointEnum(int value)
    //{
    //    return System.Enum.IsDefined(typeof(RedPointEnum), value);
    //}
}