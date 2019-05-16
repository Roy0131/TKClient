// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class GuildDonateConfig
{
	public int ItemID;
	public int RequestNum;
	public string DonateRewardItem;
	public int LimitScore;

	public static readonly string urlKey = "GuildDonateConfig";
	static Dictionary<int,GuildDonateConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,GuildDonateConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					GuildDonateConfig config = new GuildDonateConfig();

					int.TryParse(el.GetAttribute ("ItemID"), out config.ItemID);

					int.TryParse(el.GetAttribute ("RequestNum"), out config.RequestNum);

					config.DonateRewardItem = el.GetAttribute ("DonateRewardItem");

					int.TryParse(el.GetAttribute ("LimitScore"), out config.LimitScore);

					AllDatas.Add(config.ItemID, config);
				}
			}
		}
	}

	public static GuildDonateConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,GuildDonateConfig> Get()
	{
		return AllDatas;
	}
}
