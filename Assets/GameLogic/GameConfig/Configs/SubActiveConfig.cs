// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class SubActiveConfig
{
	public int SubActiveID;
	public string BundleID;
	public int DescriptionId;
	public int Param1;
	public int Param2;
	public int Param3;
	public int Param4;
	public int EventCount;
	public string Reward;

	public static readonly string urlKey = "SubActiveConfig";
	static Dictionary<int,SubActiveConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,SubActiveConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					SubActiveConfig config = new SubActiveConfig();

					int.TryParse(el.GetAttribute ("SubActiveID"), out config.SubActiveID);

					config.BundleID = el.GetAttribute ("BundleID");

					int.TryParse(el.GetAttribute ("DescriptionId"), out config.DescriptionId);

					int.TryParse(el.GetAttribute ("Param1"), out config.Param1);

					int.TryParse(el.GetAttribute ("Param2"), out config.Param2);

					int.TryParse(el.GetAttribute ("Param3"), out config.Param3);

					int.TryParse(el.GetAttribute ("Param4"), out config.Param4);

					int.TryParse(el.GetAttribute ("EventCount"), out config.EventCount);

					config.Reward = el.GetAttribute ("Reward");

					AllDatas.Add(config.SubActiveID, config);
				}
			}
		}
	}

	public static SubActiveConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,SubActiveConfig> Get()
	{
		return AllDatas;
	}
}
