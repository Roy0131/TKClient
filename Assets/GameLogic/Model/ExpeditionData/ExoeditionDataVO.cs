using Msg.ClientMessage;
using System.Collections.Generic;

public class ExoeditionDataVO : DataBaseVO
{
    public List<ExpeditionEnemyRole> mListEnemyRole { get; private set; }
    public string mPlayerName { get; private set; }
    public int mPlayerId { get; private set; }
    public int mPlayerLevel { get; private set; }
    public int mPlayerVipLevel { get; private set; }
    public int mPlayerHead { get; private set; }
    public int mPlayerPower { get; private set; }
    public int mGoldCome { get; private set; }
    public int mExpeditionGoldCome { get; private set; }
    public List<ItemInfo> mListInfo { get; private set; }

    protected override void OnInitData<T>(T value)
    {
        S2CExpeditionLevelDataResponse req = value as S2CExpeditionLevelDataResponse;
        mPlayerName = req.PlayerName;
        mPlayerId = req.PlayerId;
        mPlayerLevel = req.PlayerLevel;
        mPlayerVipLevel = req.PlayerVipLevel;
        mPlayerHead = req.PlayerHead;
        mPlayerPower = req.PlayerPower;
        mGoldCome = req.GoldIncome;
        mExpeditionGoldCome = req.ExpeditionGoldIncome;

        if (mListInfo != null)
            mListInfo.Clear();
        mListInfo = new List<ItemInfo>();
        ItemInfo info1 = new ItemInfo();
        info1.Id = SpecialItemID.Gold;
        info1.Value = req.GoldIncome;
        mListInfo.Add(info1);
        ItemInfo info2 = new ItemInfo();
        info2.Id = SpecialItemID.ExpeditionGold;
        info2.Value = req.ExpeditionGoldIncome;
        mListInfo.Add(info2);

        if (mListEnemyRole != null)
            mListEnemyRole.Clear();
        mListEnemyRole = new List<ExpeditionEnemyRole>();
        mListEnemyRole.AddRange(req.RoleList);
    }

    public void OnEnemyRoles(IList<ExpeditionEnemyRole> listRole)
    {
        if (mListEnemyRole != null)
            mListEnemyRole.Clear();
        mListEnemyRole = new List<ExpeditionEnemyRole>();
        mListEnemyRole.AddRange(listRole);
    }

    public ExpeditionEnemyRole GetRoleVOByPos(int pos)
    {
        for (int i = 0; i < mListEnemyRole.Count; i++)
        {
            if (mListEnemyRole[i].Position == pos)
                return mListEnemyRole[i];
        }
        return null;
    }
}