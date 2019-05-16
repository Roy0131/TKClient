// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class DropConfig
{
	public int ClientIndex;
	public int DropGroupID;
	public int DropItemID;

	public static readonly string urlKey = "DropConfig";
	static Dictionary<int,DropConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,DropConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					DropConfig config = new DropConfig();

					int.TryParse(el.GetAttribute ("ClientIndex"), out config.ClientIndex);

					int.TryParse(el.GetAttribute ("DropGroupID"), out config.DropGroupID);

					int.TryParse(el.GetAttribute ("DropItemID"), out config.DropItemID);

					AllDatas.Add(config.ClientIndex, config);
				}
			}
		}
	}

	public static DropConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,DropConfig> Get()
	{
		return AllDatas;
	}
}
