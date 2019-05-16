// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class CarnivalSubConfig
{
	public int ID;
	public string Reward;
	public int ActiveType;
	public int DescriptionId;
	public int Param1;
	public int Param2;
	public int Param3;
	public int Param4;
	public int EventCount;

	public static readonly string urlKey = "CarnivalSubConfig";
	static Dictionary<int,CarnivalSubConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,CarnivalSubConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					CarnivalSubConfig config = new CarnivalSubConfig();

					int.TryParse(el.GetAttribute ("ID"), out config.ID);

					config.Reward = el.GetAttribute ("Reward");

					int.TryParse(el.GetAttribute ("ActiveType"), out config.ActiveType);

					int.TryParse(el.GetAttribute ("DescriptionId"), out config.DescriptionId);

					int.TryParse(el.GetAttribute ("Param1"), out config.Param1);

					int.TryParse(el.GetAttribute ("Param2"), out config.Param2);

					int.TryParse(el.GetAttribute ("Param3"), out config.Param3);

					int.TryParse(el.GetAttribute ("Param4"), out config.Param4);

					int.TryParse(el.GetAttribute ("EventCount"), out config.EventCount);

					AllDatas.Add(config.ID, config);
				}
			}
		}
	}

	public static CarnivalSubConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,CarnivalSubConfig> Get()
	{
		return AllDatas;
	}
}
