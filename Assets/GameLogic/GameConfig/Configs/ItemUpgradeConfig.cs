// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class ItemUpgradeConfig
{
	public int UpgradeID;
	public int ItemID;
	public int UpgradeType;
	public int ResultDropID;
	public string ResCondtion;

	public static readonly string urlKey = "ItemUpgradeConfig";
	static Dictionary<int,ItemUpgradeConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,ItemUpgradeConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					ItemUpgradeConfig config = new ItemUpgradeConfig();

					int.TryParse(el.GetAttribute ("UpgradeID"), out config.UpgradeID);

					int.TryParse(el.GetAttribute ("ItemID"), out config.ItemID);

					int.TryParse(el.GetAttribute ("UpgradeType"), out config.UpgradeType);

					int.TryParse(el.GetAttribute ("ResultDropID"), out config.ResultDropID);

					config.ResCondtion = el.GetAttribute ("ResCondtion");

					AllDatas.Add(config.UpgradeID, config);
				}
			}
		}
	}

	public static ItemUpgradeConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,ItemUpgradeConfig> Get()
	{
		return AllDatas;
	}
}
