// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class GuildBossConfig
{
	public int BossIndex;
	public int StageID;
	public string BattleReward;
	public string RankReward1Cond;
	public string RankReward1;
	public string RankReward2Cond;
	public string RankReward2;
	public string RankReward3Cond;
	public string RankReward3;
	public string RankReward4Cond;
	public string RankReward4;
	public string RankReward5Cond;
	public string RankReward5;
	public string Image;

	public static readonly string urlKey = "GuildBossConfig";
	static Dictionary<int,GuildBossConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,GuildBossConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					GuildBossConfig config = new GuildBossConfig();

					int.TryParse(el.GetAttribute ("BossIndex"), out config.BossIndex);

					int.TryParse(el.GetAttribute ("StageID"), out config.StageID);

					config.BattleReward = el.GetAttribute ("BattleReward");

					config.RankReward1Cond = el.GetAttribute ("RankReward1Cond");

					config.RankReward1 = el.GetAttribute ("RankReward1");

					config.RankReward2Cond = el.GetAttribute ("RankReward2Cond");

					config.RankReward2 = el.GetAttribute ("RankReward2");

					config.RankReward3Cond = el.GetAttribute ("RankReward3Cond");

					config.RankReward3 = el.GetAttribute ("RankReward3");

					config.RankReward4Cond = el.GetAttribute ("RankReward4Cond");

					config.RankReward4 = el.GetAttribute ("RankReward4");

					config.RankReward5Cond = el.GetAttribute ("RankReward5Cond");

					config.RankReward5 = el.GetAttribute ("RankReward5");

					config.Image = el.GetAttribute ("Image");

					AllDatas.Add(config.BossIndex, config);
				}
			}
		}
	}

	public static GuildBossConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,GuildBossConfig> Get()
	{
		return AllDatas;
	}
}
