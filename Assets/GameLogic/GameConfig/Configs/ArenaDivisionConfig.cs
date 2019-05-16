// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class ArenaDivisionConfig
{
	public int Division;
	public int DivisionScoreMin;
	public int DivisionScoreMax;
	public int WinScore;
	public int WinningStreakScoreBonus;
	public int LoseScore;
	public int NewSeasonScore;
	public string RewardList;
	public int Name;
	public string Icon;

	public static readonly string urlKey = "ArenaDivisionConfig";
	static Dictionary<int,ArenaDivisionConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,ArenaDivisionConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					ArenaDivisionConfig config = new ArenaDivisionConfig();

					int.TryParse(el.GetAttribute ("Division"), out config.Division);

					int.TryParse(el.GetAttribute ("DivisionScoreMin"), out config.DivisionScoreMin);

					int.TryParse(el.GetAttribute ("DivisionScoreMax"), out config.DivisionScoreMax);

					int.TryParse(el.GetAttribute ("WinScore"), out config.WinScore);

					int.TryParse(el.GetAttribute ("WinningStreakScoreBonus"), out config.WinningStreakScoreBonus);

					int.TryParse(el.GetAttribute ("LoseScore"), out config.LoseScore);

					int.TryParse(el.GetAttribute ("NewSeasonScore"), out config.NewSeasonScore);

					config.RewardList = el.GetAttribute ("RewardList");

					int.TryParse(el.GetAttribute ("Name"), out config.Name);

					config.Icon = el.GetAttribute ("Icon");

					AllDatas.Add(config.Division, config);
				}
			}
		}
	}

	public static ArenaDivisionConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,ArenaDivisionConfig> Get()
	{
		return AllDatas;
	}
}
