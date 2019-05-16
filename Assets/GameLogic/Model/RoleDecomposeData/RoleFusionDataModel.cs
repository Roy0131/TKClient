using System.Collections.Generic;

public class RoleFusionDataModel : ModelDataBase<RoleFusionDataModel>
{
    public List<RoleFusionDataVO> mlstFusionRoles { get; private set; }
    protected override void OnInit()
    {
        base.OnInit();
        mlstFusionRoles = new List<RoleFusionDataVO>();
        RoleFusionDataVO vo;
        for (int i = 1; i <= 9; i++)
        {
            vo = new RoleFusionDataVO();
            vo.InitData(GameConfigMgr.Instance.GetFusionConfig(i));
            mlstFusionRoles.Add(vo);
            RedPointTipsMgr.Instance.DynamicCreateChildNode(RedPointEnum.RoleFusion, vo.mFusionConfig.FormulaID, false);
        }
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        HeroDataModel.Instance.AddEvent(HeroEvent.HeroCardChange, OnRoleCardChange);
    }

    protected void OnRoleCardChange()
    {
        for (int i = 0; i < mlstFusionRoles.Count; i++)
            RedPointTipsMgr.Instance.UpdateDynamicChildState(RedPointEnum.RoleFusion, mlstFusionRoles[i].mFusionConfig.FormulaID, mlstFusionRoles[i].BlCanFusion);
    }

    protected override void DoClearData()
    {
        base.DoClearData();
    }
}

public class RoleFusionDataVO : DataBaseVO
{
    public FusionConfig mFusionConfig { get; private set; }
    public List<FusionMatDataVO> mlstFusionMatDatas;

    protected override void OnInitData<T>(T value)
    {
        mFusionConfig = value as FusionConfig;
        mlstFusionMatDatas = new List<FusionMatDataVO>();
        FusionMatDataVO vo;
        if (mFusionConfig.Cost1NumCond > 0)
        {
            vo = new FusionMatDataVO(1, mFusionConfig);
            mlstFusionMatDatas.Add(vo);
        }
        if (mFusionConfig.Cost2NumCond > 0)
        {
            vo = new FusionMatDataVO(2, mFusionConfig);
            mlstFusionMatDatas.Add(vo);
        }
        if (mFusionConfig.Cost3NumCond > 0)
        {
            vo = new FusionMatDataVO(3, mFusionConfig);
            mlstFusionMatDatas.Add(vo);
        }
    }

    private int _cardNum = 0;
    private FusionMatDataVO _vo;
    public bool BlCanFusion
    {
        get
        {
            List<CardDataVO> allCard = HeroDataModel.Instance.mAllCards;
            
            for (int i = 0; i < mlstFusionMatDatas.Count; i++)
            {
                _cardNum = 0;
                if (mlstFusionMatDatas[i].mMatNum > allCard.Count)
                    return false;
                _vo = mlstFusionMatDatas[i];
                for (int j = 0; j < allCard.Count; j++)
                {
                    if (!EqualsTableId(allCard[j]))
                        continue;
                    if (!EqualsCamp(allCard[j]))
                        continue;
                    if (!EqualsType(allCard[j]))
                        continue;
                    if (!EqualsStar(allCard[j]))
                        continue;
                    _cardNum++;
                }
                if (_cardNum < mlstFusionMatDatas[i].mMatNum)
                    return false;
            }
            return true;
        }
    }

    private bool EqualsTableId(CardDataVO vo)
    {
        return _vo.mCardTableId == 0 ? true : vo.mCardTableId == _vo.mCardTableId;
    }

    private bool EqualsCamp(CardDataVO vo)
    {
        return _vo.mCampCond == 0 ? true : vo.mCardConfig.Camp == _vo.mCampCond;
    }

    private bool EqualsType(CardDataVO vo)
    {
        return _vo.mTypeCond == 0 ? true : vo.mCardConfig.Type == _vo.mTypeCond;
    }

    private bool EqualsStar(CardDataVO vo)
    {
        return _vo.mStarCond == 0 ? true : vo.mCardConfig.Rarity == _vo.mStarCond;
    }
}