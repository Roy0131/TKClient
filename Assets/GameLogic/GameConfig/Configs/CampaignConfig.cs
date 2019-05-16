// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class CampaignConfig
{
	public int ClientID;
	public int CampaignID;
	public int StageID;
	public int UnlockMap;
	public int Difficulty;
	public int ChapterMap;
	public int ChildMapID;
	public int StaticRewardSec;
	public string StaticRewardItem;
	public int RandomDropSec;
	public string RandomDropIDList;
	public int CampainTask;

	public static readonly string urlKey = "CampaignConfig";
	static Dictionary<int,CampaignConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,CampaignConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					CampaignConfig config = new CampaignConfig();

					int.TryParse(el.GetAttribute ("ClientID"), out config.ClientID);

					int.TryParse(el.GetAttribute ("CampaignID"), out config.CampaignID);

					int.TryParse(el.GetAttribute ("StageID"), out config.StageID);

					int.TryParse(el.GetAttribute ("UnlockMap"), out config.UnlockMap);

					int.TryParse(el.GetAttribute ("Difficulty"), out config.Difficulty);

					int.TryParse(el.GetAttribute ("ChapterMap"), out config.ChapterMap);

					int.TryParse(el.GetAttribute ("ChildMapID"), out config.ChildMapID);

					int.TryParse(el.GetAttribute ("StaticRewardSec"), out config.StaticRewardSec);

					config.StaticRewardItem = el.GetAttribute ("StaticRewardItem");

					int.TryParse(el.GetAttribute ("RandomDropSec"), out config.RandomDropSec);

					config.RandomDropIDList = el.GetAttribute ("RandomDropIDList");

					int.TryParse(el.GetAttribute ("CampainTask"), out config.CampainTask);

					AllDatas.Add(config.ClientID, config);
				}
			}
		}
	}

	public static CampaignConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,CampaignConfig> Get()
	{
		return AllDatas;
	}
}
