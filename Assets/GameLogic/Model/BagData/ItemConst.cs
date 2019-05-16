public class ItemType
{
    /// <summary>
    /// 货币
    /// </summary>
    public static readonly int Currency = 1;

    /// <summary>
    /// 装备
    /// </summary>
    public static readonly int Equip = 2;

    /// <summary>
    /// 普通消耗品
    /// </summary>
    public static readonly int Consumables = 3;

    /// <summary>
    /// 碎片
    /// </summary>
    public static readonly int Fragment = 4;

    /// <summary>
    /// 头像
    /// </summary>
    public static readonly int Avatar = 5;

    public static bool IsReallyCurrency(int id)
    {
        return id == SpecialItemID.Gold || id == SpecialItemID.Diamond || id == SpecialItemID.RoleExp;
    }
}

public class SpecialItemID
{
    public static readonly int Gold = 1;

    public static readonly int HeroExp = 2;

    public static readonly int Diamond = 3;

    public static readonly int AttackHeroExp = 4;

    public static readonly int DefenseHeroExp = 5;

    public static readonly int SkillHeroExp = 6;

    public static readonly int RoleExp = 7;

    public static readonly int CTowerTicket = 8;

    public static readonly int FriendShipPoint = 9;

    public static readonly int HeroCoins = 10;

    public static readonly int FriendStrength = 12;

    public static readonly int Vulgar = 15;

    public static readonly int High = 16;

    public static readonly int Honor = 17;

    public static readonly int Arena_Ticket = 18;

    public static readonly int Talent = 19;

    public static readonly int GuildCoin = 20;

    public static readonly int VIPExp = 21;

    public static readonly int FateKey = 22;

    public static readonly int EnergyStone = 23;

    public static readonly int ExpeditionGold = 24;

    public static readonly int VibratingGold = 25;

    public static readonly int Ullr = 26;
}