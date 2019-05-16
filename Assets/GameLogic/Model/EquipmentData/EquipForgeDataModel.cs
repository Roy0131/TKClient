using System.Collections.Generic;

public class EquipForgeDataModel : ModelDataBase<EquipForgeDataModel>
{
    private Dictionary<int, List<EquipForgeDataVO>> _dictEquipForges;
    protected override void OnInit()
    {
        base.OnInit();
        _dictEquipForges = new Dictionary<int, List<EquipForgeDataVO>>();

        int id;
        List<EquipForgeDataVO> lst;
        int type = 1;
        EquipForgeDataVO vo;
        RedPointEnum parentID;
        for (int i = 10; i < 14; i++)
        {
            lst = new List<EquipForgeDataVO>();
            parentID = RedPointHelper.GetRedPointEnum((int)RedPointEnum.EquipFusion * 100 + type);
            for (int j = 1; j < 16; j++)
            {
                id = i * 1000 + j;
                id = id * 100 + 1;
                vo = new EquipForgeDataVO();
                vo.InitData(GameConfigMgr.Instance.GetItemUpgradeConfig(id));
                lst.Add(vo);
                RedPointTipsMgr.Instance.DynamicCreateChildNode(parentID, vo.mForgeItemID, vo.BlCanEquipForge);
            }
            _dictEquipForges.Add(type, lst);
            type++;
        }
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        BagDataModel.Instance.AddEvent<List<int>>(BagEvent.BagItemRefresh, OnBagItemChange);
        BagDataModel.Instance.AddEvent(BagEvent.BagAllItemRefresh, OnInitStates);
        HeroDataModel.Instance.AddEvent(HeroEvent.HeroGoldChange, OnInitStates);
    }

    private void OnBagItemChange(List<int> value)
    {
        ItemConfig cfg;        
        for (int i = 0; i < value.Count; i++)
        {
            cfg = GameConfigMgr.Instance.GetItemConfig(value[i]);
            if (cfg.EquipType > 0 && cfg.EquipType < 5)
                CheckEquipForgeState(cfg.EquipType);
        }
    }

    private void OnInitStates()
    {
        for (int i = 1; i <= 4; i++)
            CheckEquipForgeState(i);
    }

    private void CheckEquipForgeState(int equipType)
    {
        if (!_dictEquipForges.ContainsKey(equipType))
            return;
        bool value = false;
        List<EquipForgeDataVO> lstForgeDatas = _dictEquipForges[equipType];
        RedPointEnum parentID = RedPointHelper.GetRedPointEnum((int)RedPointEnum.EquipFusion * 100 + equipType);
        for (int i = 0; i < lstForgeDatas.Count; i++)
        {
            value |= lstForgeDatas[i].BlCanEquipForge;
            RedPointDataModel.Instance.SetDynamicChildNodeState(parentID, lstForgeDatas[i].mForgeItemID, lstForgeDatas[i].BlCanEquipForge);
        }
        //RedPointEnum redPointID = RedPointHelper.GetRedPointEnum((int)RedPointEnum.EquipFusion * 100 + equipType);
        //RedPointDataModel.Instance.SetRedPointDataState(redPointID, value);
    }

    public List<EquipForgeDataVO> GetEquipForeData(int equipType)
    {
        if (_dictEquipForges.ContainsKey(equipType))
            return _dictEquipForges[equipType];
        return null;
    }

    protected override void DoClearData()
    {
        base.DoClearData();
        if (_dictEquipForges != null)
            _dictEquipForges.Clear();
        OnInit();
    }
}

public class EquipForgeDataVO : DataBaseVO
{
    public int mForgeMaterialID { get; private set; }
    public int mForgeItemID { get; private set; }
    public ItemUpgradeConfig mItemUpgradeConfig { get; private set; }
    private int _goldCount = 0;
    protected override void OnInitData<T>(T value)
    {
        mItemUpgradeConfig = value as ItemUpgradeConfig;
        mForgeItemID = mItemUpgradeConfig.ResultDropID;
        mForgeMaterialID = mForgeItemID - 1;
        string[] resConds = mItemUpgradeConfig.ResCondtion.Split(',');
        _goldCount = int.Parse(resConds[resConds.Length - 1]);
    }

    public bool BlCanEquipForge
    {
        get
        {
            int count = BagDataModel.Instance.GetItemCountById(mForgeMaterialID);
            if (count < 3)
                return false;
            if (HeroDataModel.Instance.mHeroInfoData != null)
            {
                if (HeroDataModel.Instance.mHeroInfoData.mGold < _goldCount)
                    return false;
            }
            return true;
        }
    }
}