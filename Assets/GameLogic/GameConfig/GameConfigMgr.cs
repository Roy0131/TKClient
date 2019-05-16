using System;
using System.Xml;
using System.Reflection;
using System.Collections.Generic;

public class GameConfigMgr : Singleton<GameConfigMgr>
{
    private Dictionary<string, Type> _dictAllCfgTypes = new Dictionary<string, Type>();

    public void Init()
    {
        RegConfig<CardConfig>();
        RegConfig<SkillConfig>();
        RegConfig<StatusConfig>();
        RegConfig<ModelConfig>();
        RegConfig<StageConfig>();
        RegConfig<ItemConfig>();
        RegConfig<CampaignConfig>();
        RegConfig<ChapterConfig>();
        RegConfig<DropConfig>();
        RegConfig<EffectConfig>();
        RegConfig<TowerConfig>();
        RegConfig<LanguageConfig>();
        RegConfig<LevelUpConfig>();
        RegConfig<RankUpConfig>();
        RegConfig<FusionConfig>();
        RegConfig<ExtractConfig>();
        RegConfig<GoldHandConfig>();
        RegConfig<AttributeConfig>();
        RegConfig<SuitConfig>();
        RegConfig<ItemUpgradeConfig>();
        RegConfig<ShopConfig>();
        RegConfig<ShopItemConfig>();
        RegConfig<ArenaDivisionConfig>();
        RegConfig<ArenaRankingBonusConfig>();
        RegConfig<ActiveStageConfig>();
        RegConfig<MissionConfig>();
        RegConfig<SearchTaskConfig>();
        RegConfig<SearchTaskDialogConfig>();
        RegConfig<FriendBossConfig>();
        RegConfig<GuildMarkConfig>();
        RegConfig<GuildLevelUpConfig>();
        RegConfig<GuildDonateConfig>();
        RegConfig<GuildBossConfig>();
        RegConfig<TalentConfig>();
        RegConfig<LinkListConfig>();
        RegConfig<SignConfig>();
        RegConfig<SevendayConfig>();
        RegConfig<PayConfig>();
        RegConfig<VipConfig>();
        RegConfig<SystemUnlockConfig>();
        RegConfig<MailConfig>();
        RegConfig<MainActiveConfig>();
        RegConfig<SubActiveConfig>();
        RegConfig<ExpeditionConfig>();
        RegConfig<ScreenConfig>();
        RegConfig<LabelConfig>();
        RegConfig<ArtifactUnlockConfig>();
        RegConfig<ArtifactLevelConfig>();
        RegConfig<CarnivalSubConfig>();
        RegConfig<CarnivalConfig>();

        ParseConfig();
        ParseSpceilConfig();
        OnStrPayConfig();
    }

    private Dictionary<int, List<int>> _dictDropConfigs = new Dictionary<int, List<int>>();
    public Dictionary<int, CardConfig> mDictLowestLvCards = new Dictionary<int, CardConfig>();
    public List<CardDataVO> mHighestLvCards = new List<CardDataVO>();

    public Dictionary<string, PayConfig> mStrPayConfig = new Dictionary<string, PayConfig>();

    public Dictionary<int, List<ActiveStageConfig>> _dictActive = new Dictionary<int, List<ActiveStageConfig>>();
    private void ParseSpceilConfig()
    {

        Dictionary<int, ActiveStageConfig>.ValueCollection activeStage = ActiveStageConfig.Get().Values;
        List<ActiveStageConfig> lstCfgs;
        foreach (ActiveStageConfig config in activeStage)
        {
            if (_dictActive.ContainsKey(config.Type))
            {
                lstCfgs = _dictActive[config.Type];
            }
            else
            {
                lstCfgs = new List<ActiveStageConfig>();
                _dictActive.Add(config.Type, lstCfgs);
            }
            lstCfgs.Add(config);
        }

        Dictionary<int, DropConfig>.ValueCollection vallColl = DropConfig.Get().Values;
        List<int> lstTmp;
        foreach (DropConfig cfg in vallColl)
        {
            if (cfg.DropItemID == 0)
                continue;
            if (_dictDropConfigs.ContainsKey(cfg.DropGroupID))
            {
                lstTmp = _dictDropConfigs[cfg.DropGroupID];
            }
            else
            {
                lstTmp = new List<int>();
                _dictDropConfigs.Add(cfg.DropGroupID, lstTmp);
            }
            if (lstTmp.IndexOf(cfg.DropItemID) == -1)
                lstTmp.Add(cfg.DropItemID);
        }

        Dictionary<int, CardConfig>.ValueCollection cardValueColl = CardConfig.Get().Values;
        lstTmp = new List<int>();
        CardConfig cardCfg;
        int cardCfgItemId;
        CardDataVO vo;
        foreach (CardConfig cfg in cardValueColl)
        {
            if (lstTmp.IndexOf(cfg.ID) != -1)
                continue;
            if (cfg.ID > 10000)
                continue;
            lstTmp.Add(cfg.ID);
            cardCfgItemId = cfg.ID * 100 + 1;
            cardCfg = GetCardConfig(cardCfgItemId);
            mDictLowestLvCards.Add(cardCfgItemId, cardCfg);

            cardCfg = GetCardConfig(cfg.ID * 100 + cardCfg.MaxRank);
            if (cardCfg.IsHandBook != 1)
                continue;
            vo = new CardDataVO(cardCfg.ID, cardCfg.Rank, cardCfg.MaxLevel);
            mHighestLvCards.Add(vo);
        }
        mHighestLvCards.Sort(SortCardConfig);
    }

    private int SortCardConfig(CardDataVO v1, CardDataVO v2)
    {
        return v1.mCardTableId > v2.mCardTableId ? -1 : 1;
    }

    private void ParseConfig()
    {
        foreach (KeyValuePair<string, Type> kv in _dictAllCfgTypes)
        {
            try
            {
                string xmlName = kv.Key.ToString();
                XmlDocument doc = GameResMgr.Instance.LoadXml(xmlName);
                if (doc == null)
                {
                    LogHelper.LogError("[Config name:" + xmlName + " can't find!!!]");
                    continue;
                }
                XmlNode node = doc.SelectSingleNode("Config");
                object instance = Activator.CreateInstance(kv.Value);
                MethodInfo mi = kv.Value.GetMethod("Parse");
                mi.Invoke(instance, new object[] { node });
            }
            catch (Exception ex)
            {
                LogHelper.LogError("[parse configs error, ex:" + ex.Message + "]");
            }
        }
    }

    private void OnStrPayConfig()
    {
        Dictionary<int, PayConfig>.ValueCollection allPay = PayConfig.Get().Values;
        foreach (PayConfig cfg in allPay)
            mStrPayConfig.Add(cfg.BundleID, cfg);
    }

    private void RegConfig<T>()
    {
        Type t = typeof(T);
        _dictAllCfgTypes.Add(t.GetField("urlKey").GetValue(null) as string, t);
    }

    public CardConfig GetCardConfig(int id)
    {
        return CardConfig.Get(id);
    }

    public StatusConfig GetStatusConfig(int id)
    {
        return StatusConfig.Get(id);
    }

    public SkillConfig GetSkillConfig(int id)
    {
        return SkillConfig.Get(id);
    }

    public ModelConfig GetModelConfig(string modelName)
    {
        return ModelConfig.Get(modelName);
    }

    public StageConfig GetStageConfig(int stageId)
    {
        return StageConfig.Get(stageId);
    }

    public CampaignConfig GetCampaignByClientId(int clientId)
    {
        return CampaignConfig.Get(clientId);
    }

    private Dictionary<int, CampaignConfig> _dictCampaigns; //= new Dictionary<int, CampaignConfig>();  //key is CampaignID
    public CampaignConfig GetCampaignByCampaignId(int campaignId)
    {
        if (_dictCampaigns == null)
        {
            _dictCampaigns = new Dictionary<int, CampaignConfig>();
            Dictionary<int, CampaignConfig> allCampaigns = CampaignConfig.Get();
            if (allCampaigns == null)
            {
                LogHelper.LogError("[GameConfigMgr.GetCampaignByCampaignId() => campaign config was invalid!!!]");
                return null;
            }
            Dictionary<int, CampaignConfig>.ValueCollection valColl = allCampaigns.Values;
            foreach (CampaignConfig cfg in valColl)
                _dictCampaigns.Add(cfg.CampaignID, cfg);
        }
        if (_dictCampaigns.ContainsKey(campaignId))
            return _dictCampaigns[campaignId];
        return null;
    }

    public ChapterConfig GetChapterConfig(int id)
    {
        return ChapterConfig.Get(id);
    }

    public List<int> GetDropConfig(int groupID)
    {
        if (_dictDropConfigs.ContainsKey(groupID))
            return _dictDropConfigs[groupID];
        return null;//DropConfig.Get(id);
    }

    public ItemConfig GetItemConfig(int itemID)
    {
        return ItemConfig.Get(itemID);
    }

    public EffectConfig GetEffectConfig(string effectName)
    {
        return EffectConfig.Get(effectName);
    }

    public TowerConfig GetTowerConfig(int towerId)
    {
        return TowerConfig.Get(towerId);
    }

    public LevelUpConfig GetLevelUpConfig(int level)
    {
        return LevelUpConfig.Get(level);
    }

    public RankUpConfig GetRankUpConfig(int rank)
    {
        return RankUpConfig.Get(rank);
    }

    public FusionConfig GetFusionConfig(int id)
    {
        return FusionConfig.Get(id);
    }

    public ExtractConfig GetExtractConfig(int id)
    {
        return ExtractConfig.Get(id);
    }

    public GoldHandConfig GetGoldHandConfig(int id)
    {
        return GoldHandConfig.Get(id);
    }

    public AttributeConfig GetAttrConfig(int id)
    {
        return AttributeConfig.Get(id);
    }

    public SuitConfig GetSuitConfig(int id)
    {
        return SuitConfig.Get(id);
    }

    public ItemUpgradeConfig GetItemUpgradeConfig(int id)
    {
        return ItemUpgradeConfig.Get(id);
    }

    public ShopConfig GetShopConfig(int id)
    {
        return ShopConfig.Get(id);
    }

    public ShopItemConfig GetShopItemConfig(int id)
    {
        return ShopItemConfig.Get(id);
    }

    public ArenaDivisionConfig GetArenaDivisionConfig(int grade)
    {
        return ArenaDivisionConfig.Get(grade);
    }

    public ActiveStageConfig GetActiveStageConfig(int id)
    {
        return ActiveStageConfig.Get(id);
    }

    public MissionConfig GetMissionConfig(int id)
    {
        return MissionConfig.Get(id);
    }

    public SearchTaskConfig GetSearchTaskConfig(int id)
    {
        return SearchTaskConfig.Get(id);
    }
    public SearchTaskDialogConfig GetSearchTaskDialogConfig(int id)
    {
        return SearchTaskDialogConfig.Get(id);
    }
    public FriendBossConfig GetFriendBossConfig(int id)
    {
        return FriendBossConfig.Get(id);
    }

    public GuildMarkConfig GetGuildMarkConfig(int id)
    {
        return GuildMarkConfig.Get(id);
    }

    public GuildLevelUpConfig GetGuildLevelUpConfig(int id)
    {
        return GuildLevelUpConfig.Get(id);
    }

    public GuildDonateConfig GetGuildDonateConfig(int id)
    {
        return GuildDonateConfig.Get(id);
    }

    public GuildBossConfig GetGuildBossConfig(int id)
    {
        return GuildBossConfig.Get(id);
    }

    public TalentConfig GetTalentConfig(int id)
    {
        return TalentConfig.Get(id);
    }

    public LinkListConfig GetLinkListConfig(int id)
    {
        return LinkListConfig.Get(id);
    }

    public SignConfig GetSignConfig(int id)
    {
        return SignConfig.Get(id);
    }

    public SevendayConfig GetSevendayConfig(int id)
    {
        return SevendayConfig.Get(id);
    }

    public PayConfig GetPayConfig(int id)
    {
        return PayConfig.Get(id);
    }

    public PayConfig GetStrPayConfig(string bundleId)
    {
        if (mStrPayConfig.ContainsKey(bundleId))
            return mStrPayConfig[bundleId];
        return null;
    }

    public VipConfig GetVipConfig(int id)
    {
        return VipConfig.Get(id);
    }

    public SystemUnlockConfig GetSystemUnlockConfig(string str)
    {
        return SystemUnlockConfig.Get(str);
    }

    public MailConfig GetMailConfig(int id)
    {
        return MailConfig.Get(id);
    }

    public MainActiveConfig GetMainActiveConfig(int id)
    {
        return MainActiveConfig.Get(id);
    }

    public SubActiveConfig GetSubActiveConfig(int id)
    {
        return SubActiveConfig.Get(id);
    }

    public ExpeditionConfig GetExpeditionConfig(int id)
    {
        return ExpeditionConfig.Get(id);
    }

    public ScreenConfig GetScreenConfig(int id)
    {
        return ScreenConfig.Get(id);
    }

    public LabelConfig GetLabelConfig(int id)
    {
        return LabelConfig.Get(id);
    }

    public ArtifactUnlockConfig GetArtifactUnlockConfig(int id)
    {
        return ArtifactUnlockConfig.Get(id);
    }

    public ArtifactLevelConfig GetArtifactLevelConfig(int id)
    {
        return ArtifactLevelConfig.Get(id);
    }

    public CarnivalSubConfig GetCarnivalSubConfig(int id)
    {
        return CarnivalSubConfig.Get(id);
    }

    public CarnivalConfig GetCarnivalConfig(int id)
    {
        return CarnivalConfig.Get(id);
    }
}