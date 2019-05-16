// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class SevendayConfig
{
	public int TotalIndex;
	public string Reward;

	public static readonly string urlKey = "SevendayConfig";
	static Dictionary<int,SevendayConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,SevendayConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					SevendayConfig config = new SevendayConfig();

					int.TryParse(el.GetAttribute ("TotalIndex"), out config.TotalIndex);

					config.Reward = el.GetAttribute ("Reward");

					AllDatas.Add(config.TotalIndex, config);
				}
			}
		}
	}

	public static SevendayConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,SevendayConfig> Get()
	{
		return AllDatas;
	}
}
