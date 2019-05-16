public class EquipFunDataVO
{
    public int mRoleId { get; set; }
    public int mItemId { get; set; }
    public int mEquipSlot { get; set; }
    public ItemUpGradeType mUpGradeType { get; set; } //2：宝石升级， 3：宝石置换， 4：神器升级
}

public enum ItemUpGradeType
{
    Item_UpGrade = 1,   
    GemStone_UpGrade = 2,
    GemStone_Convert = 3,
    Artifact_UpGrade = 4,
}