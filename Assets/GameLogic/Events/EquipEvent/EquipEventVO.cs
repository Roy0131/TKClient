using System;

public class EquipEventVO
{
    public int mEventType { get; set; }

    public CardDataVO mCardDataVO { get; set; }

    public int mEquipType { get; set; }

    public int mEquipItemId { get; set; }

    public Action<int> mOnWearEquipMethod { get; set; }
}