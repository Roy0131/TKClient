using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionUnlock
{

    public static bool IsUnlock(int featureType, bool isTips = false)
    {
        if (HeroDataModel.Instance.mHeroInfoData.mLevel >= GameConst.GetFeatureType(featureType))
        {
            return true;
        }
        else
        {
            if (!isTips)
                PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001123, GameConst.GetFeatureType(featureType)));
            return false;
        }
    }
}
