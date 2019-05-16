// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class LinkListConfig
{
	public int ID;
	public int Text;

	public static readonly string urlKey = "LinkListConfig";
	static Dictionary<int,LinkListConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,LinkListConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					LinkListConfig config = new LinkListConfig();

					int.TryParse(el.GetAttribute ("ID"), out config.ID);

					int.TryParse(el.GetAttribute ("Text"), out config.Text);

					AllDatas.Add(config.ID, config);
				}
			}
		}
	}

	public static LinkListConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,LinkListConfig> Get()
	{
		return AllDatas;
	}
}
