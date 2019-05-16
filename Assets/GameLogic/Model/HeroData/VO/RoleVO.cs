using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDetailConst
{
    public const int HeroInfo = 1;
    public const int BagDetail = 2;
    public const int ChatDetail = 3;
}



public class RoleVO
{
    public CardDataVO mCardDataVO { get; private set; }
    public List<CardDataVO> mLstCardDatas { get; private set; }
    public int mCampType { get; private set; }
    public int mCardDetailType { get; private set; }

    public void OnCardVO(CardDataVO vo)
    {
        mCardDataVO = vo;
    }

    public void OnLstCardVO(List<CardDataVO> lstCardDatas)
    {
        mLstCardDatas = lstCardDatas;
    }

    public void OnCampType(int type)
    {
        mCampType = type;
    }

    public void OnDetailType(int type)
    {
        mCardDetailType = type;
    }
}
