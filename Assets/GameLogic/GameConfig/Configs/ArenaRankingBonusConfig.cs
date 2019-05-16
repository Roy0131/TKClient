// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class ArenaRankingBonusConfig
{
	public int Index;
	public int RankingMin;
	public int RankingMax;
	public string DayRewardList;
	public string SeasonRewardList;

	public static readonly string urlKey = "ArenaRankingBonusConfig";
	static Dictionary<int,ArenaRankingBonusConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,ArenaRankingBonusConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					ArenaRankingBonusConfig config = new ArenaRankingBonusConfig();

					int.TryParse(el.GetAttribute ("Index"), out config.Index);

					int.TryParse(el.GetAttribute ("RankingMin"), out config.RankingMin);

					int.TryParse(el.GetAttribute ("RankingMax"), out config.RankingMax);

					config.DayRewardList = el.GetAttribute ("DayRewardList");

					config.SeasonRewardList = el.GetAttribute ("SeasonRewardList");

					AllDatas.Add(config.Index, config);
				}
			}
		}
	}

	public static ArenaRankingBonusConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,ArenaRankingBonusConfig> Get()
	{
		return AllDatas;
	}
}
