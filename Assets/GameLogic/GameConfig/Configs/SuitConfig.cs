// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class SuitConfig
{
	public int SuitID;
	public string AttrSuit2;
	public string AttrSuit3;
	public string AttrSuit4;
	public int BpowerSuit2;
	public int BpowerSuit3;
	public int BpowerSuit4;
	public int NameID;

	public static readonly string urlKey = "SuitConfig";
	static Dictionary<int,SuitConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,SuitConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					SuitConfig config = new SuitConfig();

					int.TryParse(el.GetAttribute ("SuitID"), out config.SuitID);

					config.AttrSuit2 = el.GetAttribute ("AttrSuit2");

					config.AttrSuit3 = el.GetAttribute ("AttrSuit3");

					config.AttrSuit4 = el.GetAttribute ("AttrSuit4");

					int.TryParse(el.GetAttribute ("BpowerSuit2"), out config.BpowerSuit2);

					int.TryParse(el.GetAttribute ("BpowerSuit3"), out config.BpowerSuit3);

					int.TryParse(el.GetAttribute ("BpowerSuit4"), out config.BpowerSuit4);

					int.TryParse(el.GetAttribute ("NameID"), out config.NameID);

					AllDatas.Add(config.SuitID, config);
				}
			}
		}
	}

	public static SuitConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,SuitConfig> Get()
	{
		return AllDatas;
	}
}
