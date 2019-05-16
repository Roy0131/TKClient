// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class ShopItemConfig
{
	public int GoodID;
	public int ShopID;
	public string ItemList;
	public string BuyCost;
	public int StockNum;

	public static readonly string urlKey = "ShopItemConfig";
	static Dictionary<int,ShopItemConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,ShopItemConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					ShopItemConfig config = new ShopItemConfig();

					int.TryParse(el.GetAttribute ("GoodID"), out config.GoodID);

					int.TryParse(el.GetAttribute ("ShopID"), out config.ShopID);

					config.ItemList = el.GetAttribute ("ItemList");

					config.BuyCost = el.GetAttribute ("BuyCost");

					int.TryParse(el.GetAttribute ("StockNum"), out config.StockNum);

					AllDatas.Add(config.GoodID, config);
				}
			}
		}
	}

	public static ShopItemConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,ShopItemConfig> Get()
	{
		return AllDatas;
	}
}
