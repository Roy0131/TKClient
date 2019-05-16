using Msg.ClientMessage;
using System.Collections.Generic;

public class HeroInfoDataVO : DataBaseVO
{
    public int mLevel { get; private set; }
    public int mExp { get; private set; }
    public int mGold { get; private set; }
    public int mDiamond { get; private set; }
    public int mIcon { get; private set; }
    public int mVipLevel { get; private set; }
    public string mHeroName { get; private set; }
    public int mGuildId { get; private set; }
    public string mGuildName { get; private set; }
    public int mGuildLevel { get; private set; }
    public int mExpMax { get; private set; }
    public int mGuildLogo { get; private set; }
    public SystemUnlockConfig mSystConfig { get; private set; }
    public bool isLevelUp { get; private set; }


    protected override void OnInitData<T>(T value)
    {
        S2CPlayerInfoResponse playerInfo = value as S2CPlayerInfoResponse;
        UpdateLevel(playerInfo.Level);
        //mExp = playerInfo.Exp;
        UpdateExp(playerInfo.Exp);
        UpdateGold(playerInfo.Gold);
        //mGold = playerInfo.Gold;
        mDiamond = playerInfo.Diamond;
        mIcon = playerInfo.Icon;
        mHeroName = playerInfo.Name;
        mGuildId = playerInfo.GuildId;
        mGuildName = playerInfo.GuildName;
        mGuildLevel = playerInfo.GuildLevel;
        mGuildLogo = playerInfo.GuildLogo;
        mVipLevel = playerInfo.VipLevel;
        LevelUpConfig cfg = GameConfigMgr.Instance.GetLevelUpConfig(mLevel);
        mExpMax = cfg.PlayerLevelUpExp;
    }

    public void UpdateGuildId(int guildID)
    {
        mGuildId = guildID;
    }

    public void UpdateLevel(int level)
    {
        if (mLevel > 0)
        {
            if (mSystConfig == null)
            {
                foreach (SystemUnlockConfig syst in SystemUnlockConfig.Get().Values)
                {
                    if (level > mLevel && syst.Level > mLevel && syst.Level <= level)
                    {
                        mSystConfig = new SystemUnlockConfig();
                        mSystConfig = syst;
                    }
                }
            }
            if (level > mLevel)
                isLevelUp = true;
        }
        mLevel = level;
        TDPostDataMgr.Instance.DoUpdateLevel(level);
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(HeroEvent.UpGrade);
    }

    public void OnIsLevelUp()
    {
        isLevelUp = false;
    }


    public void OnConfigClear()
    {
        mSystConfig = null;
    }

    public void UpdateExp(int exp)
    {
        mExp = exp;
    }

    public void UpdateGold(int gold)
    {
        mGold = gold;
        HeroDataModel.Instance.DispathEvent(HeroEvent.HeroGoldChange);
    }

    public void UpdateDiamond(int diamond)
    {
        mDiamond = diamond;
    }

    public void UpdateVipLevel(int vipLevel)
    {
        mVipLevel = vipLevel;
    }

    public void UpdateHeroName(string name)
    {
        mHeroName = name;
    }

    public void UpdateHeroHead(int headId)
    {
        mIcon = headId;
    }

    public int GetCurrencyValue(int type)
    {
        if (type == SpecialItemID.Diamond)
            return mDiamond;
        else if (type == SpecialItemID.Gold)
            return mGold;
        else if (type == SpecialItemID.RoleExp)
            return mExp;
        //else if (type == SpecialItemID.Talent)
        //    return;
        //else if (type == SpecialItemID.GuildCoin)
        //    return;
        return 0;
    }
}