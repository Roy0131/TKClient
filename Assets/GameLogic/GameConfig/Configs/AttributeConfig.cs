// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class AttributeConfig
{
	public int ID;
	public int NameID;
	public int PercentShow;
	public int Divisor;
	public int UIBaseValue;

	public static readonly string urlKey = "AttributeConfig";
	static Dictionary<int,AttributeConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,AttributeConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					AttributeConfig config = new AttributeConfig();

					int.TryParse(el.GetAttribute ("ID"), out config.ID);

					int.TryParse(el.GetAttribute ("NameID"), out config.NameID);

					int.TryParse(el.GetAttribute ("PercentShow"), out config.PercentShow);

					int.TryParse(el.GetAttribute ("Divisor"), out config.Divisor);

					int.TryParse(el.GetAttribute ("UIBaseValue"), out config.UIBaseValue);

					AllDatas.Add(config.ID, config);
				}
			}
		}
	}

	public static AttributeConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,AttributeConfig> Get()
	{
		return AllDatas;
	}
}
