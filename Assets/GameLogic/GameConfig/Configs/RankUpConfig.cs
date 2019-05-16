// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class RankUpConfig
{
	public int Rank;
	public string Type1RankUpRes;
	public string Type2RankUpRes;
	public string Type3RankUpRes;
	public string Type1DecomposeRes;
	public string Type2DecomposeRes;
	public string Type3DecomposeRes;

	public static readonly string urlKey = "RankUpConfig";
	static Dictionary<int,RankUpConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,RankUpConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					RankUpConfig config = new RankUpConfig();

					int.TryParse(el.GetAttribute ("Rank"), out config.Rank);

					config.Type1RankUpRes = el.GetAttribute ("Type1RankUpRes");

					config.Type2RankUpRes = el.GetAttribute ("Type2RankUpRes");

					config.Type3RankUpRes = el.GetAttribute ("Type3RankUpRes");

					config.Type1DecomposeRes = el.GetAttribute ("Type1DecomposeRes");

					config.Type2DecomposeRes = el.GetAttribute ("Type2DecomposeRes");

					config.Type3DecomposeRes = el.GetAttribute ("Type3DecomposeRes");

					AllDatas.Add(config.Rank, config);
				}
			}
		}
	}

	public static RankUpConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,RankUpConfig> Get()
	{
		return AllDatas;
	}
}
