// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class CarnivalConfig
{
	public int Round;
	public string StartTime;
	public string EndTime;

	public static readonly string urlKey = "CarnivalConfig";
	static Dictionary<int,CarnivalConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,CarnivalConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					CarnivalConfig config = new CarnivalConfig();

					int.TryParse(el.GetAttribute ("Round"), out config.Round);

					config.StartTime = el.GetAttribute ("StartTime");

					config.EndTime = el.GetAttribute ("EndTime");

					AllDatas.Add(config.Round, config);
				}
			}
		}
	}

	public static CarnivalConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,CarnivalConfig> Get()
	{
		return AllDatas;
	}
}
