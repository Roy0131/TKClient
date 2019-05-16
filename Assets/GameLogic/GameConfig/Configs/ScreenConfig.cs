// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class ScreenConfig
{
	public int ID;
	public int Type;

	public static readonly string urlKey = "ScreenConfig";
	static Dictionary<int,ScreenConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,ScreenConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					ScreenConfig config = new ScreenConfig();

					int.TryParse(el.GetAttribute ("ID"), out config.ID);

					int.TryParse(el.GetAttribute ("Type"), out config.Type);

					AllDatas.Add(config.ID, config);
				}
			}
		}
	}

	public static ScreenConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,ScreenConfig> Get()
	{
		return AllDatas;
	}
}
