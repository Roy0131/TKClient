// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class ShopConfig
{
	public int ID;
	public int ShopType;
	public int ShopMaxSlot;
	public string AutoRefreshTime;
	public int FreeRefreshTime;
	public string RefreshRes;

	public static readonly string urlKey = "ShopConfig";
	static Dictionary<int,ShopConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,ShopConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					ShopConfig config = new ShopConfig();

					int.TryParse(el.GetAttribute ("ID"), out config.ID);

					int.TryParse(el.GetAttribute ("ShopType"), out config.ShopType);

					int.TryParse(el.GetAttribute ("ShopMaxSlot"), out config.ShopMaxSlot);

					config.AutoRefreshTime = el.GetAttribute ("AutoRefreshTime");

					int.TryParse(el.GetAttribute ("FreeRefreshTime"), out config.FreeRefreshTime);

					config.RefreshRes = el.GetAttribute ("RefreshRes");

					AllDatas.Add(config.ID, config);
				}
			}
		}
	}

	public static ShopConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,ShopConfig> Get()
	{
		return AllDatas;
	}
}
