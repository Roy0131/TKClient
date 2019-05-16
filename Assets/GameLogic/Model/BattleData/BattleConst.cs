public enum BattleUnitType : int
{
    //HpBar,
    BattleUI,
    Fighter,
    Bullet,
    HangupFighter,
    HangupBullet,
    Effection,
    AnimatorFighter,
    LineupFighter,
}

//0，无
//1，近身攻击（移动到攻击目标）
//2，远程发射
//3，移动后发射子弹（移动到当前行的对面固定位置）
public enum SkillAnimtionType : int
{
    None = 0,//none attack logic to run
    MTTargetAttack = 1, //move to target and start attack 
    StandAttack = 2, //stand and start attack
    MTCenterAttack = 3, //move to spcial seat and start attack
}

public enum BattleRoundActionType : byte
{
    EnterRound,
    BattleRound,
}

public enum AttackNodeStatus : int
{
    None,
    MoveToAttack,
    Attacking,
    //AttackDamage,
    MoveReset,
    WaitingEndTime,
}

//0:没有子弹
//1:飞行子弹
//2:目标位置特效（类似地上魔法阵）
//3:连线
//4:自身特效
public enum BulletType : int
{
    None = 0,
    Linear,
    FiexedEffect,
    LineEffect,
    SelfEffect,
    Bomb,
    Shadow,
}

//1：单体，
//2：横排，
//3：竖排，
//4：多体，
//5：十字，
//6：大十字，
//7：全体，

public class SkillRangeType
{
    public const int Single = 1;
    public const int Row = 2;
    public const int Col = 3;
    public const int Multiple = 4;
    public const int CrossOne = 5;
    public const int CrossTwo = 6;
    public const int All = 7;
}

//1:我方，2：敌方
public class SkillTargetType
{
    public const int Friendly = 1;
    public const int Enemy = 2;
}