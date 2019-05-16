// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class AccelCostConfig
{
	public int AccelTimes;
	public int Cost;

	public static readonly string urlKey = "AccelCostConfig";
	static Dictionary<int,AccelCostConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,AccelCostConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					AccelCostConfig config = new AccelCostConfig();

					int.TryParse(el.GetAttribute ("AccelTimes"), out config.AccelTimes);

					int.TryParse(el.GetAttribute ("Cost"), out config.Cost);

					AllDatas.Add(config.AccelTimes, config);
				}
			}
		}
	}

	public static AccelCostConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,AccelCostConfig> Get()
	{
		return AllDatas;
	}
}
