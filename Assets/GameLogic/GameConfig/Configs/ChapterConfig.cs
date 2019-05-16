// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class ChapterConfig
{
	public int ChapterID;
	public int CampaignCount;
	public int Icon;

	public static readonly string urlKey = "ChapterConfig";
	static Dictionary<int,ChapterConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,ChapterConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					ChapterConfig config = new ChapterConfig();

					int.TryParse(el.GetAttribute ("ChapterID"), out config.ChapterID);

					int.TryParse(el.GetAttribute ("CampaignCount"), out config.CampaignCount);

					int.TryParse(el.GetAttribute ("Icon"), out config.Icon);

					AllDatas.Add(config.ChapterID, config);
				}
			}
		}
	}

	public static ChapterConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,ChapterConfig> Get()
	{
		return AllDatas;
	}
}
