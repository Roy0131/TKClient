using Msg.ClientMessage;
using System.Collections.Generic;
public class RoundNodeDataVO : DataBaseVO
{
    public int mRoundIndex { get; private set; }
    public List<BuffDataVO> mlstRemoveBuffs { get; private set; }
    public List<FighterDamageDataVO> mlstChangeFighters { get; private set; }
    public List<ActionNodeData> mlstActionNodes { get; private set; }
    public IDictionary<int, int> mMyFighterEnergy { get; private set; }
    public IDictionary<int, int> mEnemyFighterEnergy { get; private set; }
    public int mSelfArtifStartValue { get; private set; }
    public int mTargetArtifStartValue { get; private set; }
    public int mSelfArtifEndValue { get; private set; }
    public int mTargetArtifEndValue { get; private set; }
    public RoundNodeDataVO()
    {
        mRoundIndex = 1;
    }

    protected override void OnInitData<T>(T value)
    {
        IList<BattleReportItem> lstReports = value as IList<BattleReportItem>;

        mlstActionNodes = new List<ActionNodeData>();
        int len = lstReports.Count;
        BattleReportItem reportItem;
        int idx = 0;
        ActionNodeData node;
        while (idx < len)
        {
            reportItem = lstReports[idx];
            node = new ActionNodeData();
            node.InitData(reportItem);
            if (reportItem.HasCombo)
            {
                while (true)
                {
                    idx++;
                    if (idx >= len)
                        break;
                    reportItem = lstReports[idx];
                    if (!lstReports[idx - 1].HasCombo)
                        break;
                    node.AddComboReportItem(reportItem);
                }
            }
            else
            {
                idx++;
            }
            mlstActionNodes.Add(node);
        }
    }

    public void SetRoundNodeData(BattleRoundReports value)
    {
        mRoundIndex = value.RoundNum;
        if (value.Reports != null)
            OnInitData(value.Reports);
        int i = 0;
        if (value.RemoveBuffs != null && value.RemoveBuffs.Count > 0)
        {
            if (mlstRemoveBuffs == null)
                mlstRemoveBuffs = new List<BuffDataVO>();
            BuffDataVO removeData;
            for (i = 0; i < value.RemoveBuffs.Count; i++)
            {
                removeData = new BuffDataVO();
                removeData.InitData(value.RemoveBuffs[i]);
                mlstRemoveBuffs.Add(removeData);
            }
        }

        if (value.ChangedFighters != null && value.ChangedFighters.Count > 0)
        {
            mlstChangeFighters = new List<FighterDamageDataVO>();
            FighterDamageDataVO fighterData;
            for (i = 0; i < value.ChangedFighters.Count; i++)
            {
                fighterData = new FighterDamageDataVO();
                fighterData.InitData(value.ChangedFighters[i]);
                fighterData.mDamage = value.ChangedFighters[i].Damage;
                mlstChangeFighters.Add(fighterData);
            }
        }

        if (mMyFighterEnergy == null)
            mMyFighterEnergy = new Dictionary<int, int>();
        if (mEnemyFighterEnergy == null)
            mEnemyFighterEnergy = new Dictionary<int, int>();
        mEnemyFighterEnergy.Clear();
        mMyFighterEnergy.Clear();
        for (i = 0; i < value.MyMembersEnergy.Count; i++)
        {
            if (value.MyMembersEnergy[i] > 0)
                mMyFighterEnergy.Add(i, value.MyMembersEnergy[i]);

        }
        for (i = 0; i < value.TargetMembersEnergy.Count; i++)
        {
            if (value.TargetMembersEnergy[i] > 0)
                mEnemyFighterEnergy.Add(i, value.TargetMembersEnergy[i]);
        }
        mSelfArtifStartValue = value.MyArtifactStartEnergy;
        mSelfArtifEndValue = value.MyArtifactEndEnergy;
        mTargetArtifStartValue = value.TargetArtifactStartEnergy;
        mTargetArtifEndValue = value.TargetArtifactEndEnergy;
    }

    public override void Dispose()
    {
        base.Dispose();
        if (mlstRemoveBuffs != null)
        {
            RemoveChildDataVO(mlstRemoveBuffs);
            mlstRemoveBuffs = null;
        }
        if (mlstChangeFighters != null)
        {
            RemoveChildDataVO(mlstChangeFighters);
            mlstChangeFighters = null;
        }
        if (mlstActionNodes != null)
        {
            RemoveChildDataVO(mlstActionNodes);
            mlstActionNodes = null;
        }
    }
}