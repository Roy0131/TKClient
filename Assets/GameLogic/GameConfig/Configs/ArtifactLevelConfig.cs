// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class ArtifactLevelConfig
{
	public int ClientIndex;
	public int ArtifactID;
	public int Rank;
	public int Level;
	public int MaxLevel;
	public int SkillID;
	public string ArtifactAttr;
	public string LevelUpResCost;
	public string RankUpResCost;
	public string DecomposeRes;
	public string Icon;
	public string BattleGIFRes;
	public int BattleGIFScale;
	public string BattleEffectRes;

	public static readonly string urlKey = "ArtifactLevelConfig";
	static Dictionary<int,ArtifactLevelConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,ArtifactLevelConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					ArtifactLevelConfig config = new ArtifactLevelConfig();

					int.TryParse(el.GetAttribute ("ClientIndex"), out config.ClientIndex);

					int.TryParse(el.GetAttribute ("ArtifactID"), out config.ArtifactID);

					int.TryParse(el.GetAttribute ("Rank"), out config.Rank);

					int.TryParse(el.GetAttribute ("Level"), out config.Level);

					int.TryParse(el.GetAttribute ("MaxLevel"), out config.MaxLevel);

					int.TryParse(el.GetAttribute ("SkillID"), out config.SkillID);

					config.ArtifactAttr = el.GetAttribute ("ArtifactAttr");

					config.LevelUpResCost = el.GetAttribute ("LevelUpResCost");

					config.RankUpResCost = el.GetAttribute ("RankUpResCost");

					config.DecomposeRes = el.GetAttribute ("DecomposeRes");

					config.Icon = el.GetAttribute ("Icon");

					config.BattleGIFRes = el.GetAttribute ("BattleGIFRes");

					int.TryParse(el.GetAttribute ("BattleGIFScale"), out config.BattleGIFScale);

					config.BattleEffectRes = el.GetAttribute ("BattleEffectRes");

					AllDatas.Add(config.ClientIndex, config);
				}
			}
		}
	}

	public static ArtifactLevelConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,ArtifactLevelConfig> Get()
	{
		return AllDatas;
	}
}
