// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class MissionConfig
{
	public int Id;
	public int Title;
	public int DescriptionId;
	public int Type;
	public int EventId;
	public int EventParam;
	public int CompleteNum;
	public int Prev;
	public int Next;
	public string Reward;
	public int Hyperlink;
	public int Sort;

	public static readonly string urlKey = "MissionConfig";
	static Dictionary<int,MissionConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,MissionConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					MissionConfig config = new MissionConfig();

					int.TryParse(el.GetAttribute ("Id"), out config.Id);

					int.TryParse(el.GetAttribute ("Title"), out config.Title);

					int.TryParse(el.GetAttribute ("DescriptionId"), out config.DescriptionId);

					int.TryParse(el.GetAttribute ("Type"), out config.Type);

					int.TryParse(el.GetAttribute ("EventId"), out config.EventId);

					int.TryParse(el.GetAttribute ("EventParam"), out config.EventParam);

					int.TryParse(el.GetAttribute ("CompleteNum"), out config.CompleteNum);

					int.TryParse(el.GetAttribute ("Prev"), out config.Prev);

					int.TryParse(el.GetAttribute ("Next"), out config.Next);

					config.Reward = el.GetAttribute ("Reward");

					int.TryParse(el.GetAttribute ("Hyperlink"), out config.Hyperlink);

					int.TryParse(el.GetAttribute ("Sort"), out config.Sort);

					AllDatas.Add(config.Id, config);
				}
			}
		}
	}

	public static MissionConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,MissionConfig> Get()
	{
		return AllDatas;
	}
}
