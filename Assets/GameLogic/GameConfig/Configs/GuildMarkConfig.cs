// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class GuildMarkConfig
{
	public int ID;
	public string Icon;

	public static readonly string urlKey = "GuildMarkConfig";
	static Dictionary<int,GuildMarkConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,GuildMarkConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					GuildMarkConfig config = new GuildMarkConfig();

					int.TryParse(el.GetAttribute ("ID"), out config.ID);

					config.Icon = el.GetAttribute ("Icon");

					AllDatas.Add(config.ID, config);
				}
			}
		}
	}

	public static GuildMarkConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,GuildMarkConfig> Get()
	{
		return AllDatas;
	}
}
