using System.Collections.Generic;

public class FusionMatDataVO
{
    public int mIndex { get; private set; }

    public int mType { get; private set; }

    public int mMatNum { get; private set; }

    public int mCampCond { get; private set; }

    public int mTypeCond { get; private set; }

    public int mStarCond { get; private set; }

    public int mCardTableId { get; private set; }

    public string mDefIcon { get; private set; }

    public IList<int> mlstMatIds { get; private set; }

    public int mMainCardId { get; set; }

    public string mIcon { get; private set; }

    public string mSubscript { get; private set; }

    public int mStarShow { get; private set; }

    public FusionMatDataVO(int index, FusionConfig config)
    {
        mlstMatIds = new List<int>();
        mType = config.FusionType;
        mIndex = index;
        mIcon = config.Icon;
        mSubscript = config.LeftCornerIcon;
        mStarShow = config.StarShow - 1;
        switch (mIndex)
        {
            case 1:
                mCardTableId = config.Cost1IDCond;
                mMatNum = config.Cost1NumCond;
                mCampCond = config.Cost1CampCond;
                mTypeCond = config.Cost1TypeCond;
                mStarCond = config.Cost1StarCond;
                mDefIcon = config.Cost1Icon;
                break;
            case 2:
                mCardTableId = config.Cost2IDCond;
                mMatNum = config.Cost2NumCond;
                mCampCond = config.Cost2CampCond;
                mTypeCond = config.Cost2TypeCond;
                mStarCond = config.Cost2StarCond;
                mDefIcon = config.Cost2Icon;
                break;
            case 3:
                mCardTableId = config.Cost3IDCond;
                mMatNum = config.Cost3NumCond;
                mCampCond = config.Cost3CampCond;
                mTypeCond = config.Cost3TypeCond;
                mStarCond = config.Cost3StarCond;
                mDefIcon = config.Cost3Icon;
                break;
        }
    }

    public void AddCardData(int cardId)
    {
        mlstMatIds.Add(cardId);
    }

    public void RemoveCardData(int cardId)
    {
        if (mlstMatIds.Contains(cardId))
            mlstMatIds.Remove(cardId);
    }

    public bool BlMatEnough
    {
        get { return mlstMatIds.Count >= mMatNum; }
    }

    public bool BlContainId(int id)
    {
        return mlstMatIds.Contains(id);
    }

    public void ClearAllMat()
    {
        mlstMatIds.Clear();
    }
}