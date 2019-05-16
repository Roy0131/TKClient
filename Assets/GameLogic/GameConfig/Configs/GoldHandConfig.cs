// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class GoldHandConfig
{
	public int Level;
	public int GoldReward1;
	public int GemCost1;
	public int GoldReward2;
	public int GemCost2;
	public int GoldReward3;
	public int GemCost3;
	public int RefreshCD;

	public static readonly string urlKey = "GoldHandConfig";
	static Dictionary<int,GoldHandConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,GoldHandConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					GoldHandConfig config = new GoldHandConfig();

					int.TryParse(el.GetAttribute ("Level"), out config.Level);

					int.TryParse(el.GetAttribute ("GoldReward1"), out config.GoldReward1);

					int.TryParse(el.GetAttribute ("GemCost1"), out config.GemCost1);

					int.TryParse(el.GetAttribute ("GoldReward2"), out config.GoldReward2);

					int.TryParse(el.GetAttribute ("GemCost2"), out config.GemCost2);

					int.TryParse(el.GetAttribute ("GoldReward3"), out config.GoldReward3);

					int.TryParse(el.GetAttribute ("GemCost3"), out config.GemCost3);

					int.TryParse(el.GetAttribute ("RefreshCD"), out config.RefreshCD);

					AllDatas.Add(config.Level, config);
				}
			}
		}
	}

	public static GoldHandConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,GoldHandConfig> Get()
	{
		return AllDatas;
	}
}
