// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class ExtractConfig
{
	public int Id;
	public string DropID;
	public string ResCondition1;
	public string ResCondition2;
	public int FreeExtractTime;

	public static readonly string urlKey = "ExtractConfig";
	static Dictionary<int,ExtractConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,ExtractConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					ExtractConfig config = new ExtractConfig();

					int.TryParse(el.GetAttribute ("Id"), out config.Id);

					config.DropID = el.GetAttribute ("DropID");

					config.ResCondition1 = el.GetAttribute ("ResCondition1");

					config.ResCondition2 = el.GetAttribute ("ResCondition2");

					int.TryParse(el.GetAttribute ("FreeExtractTime"), out config.FreeExtractTime);

					AllDatas.Add(config.Id, config);
				}
			}
		}
	}

	public static ExtractConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,ExtractConfig> Get()
	{
		return AllDatas;
	}
}
