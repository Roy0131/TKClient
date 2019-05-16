// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class StageConfig
{
	public int StageID;
	public int MaxWaves;
	public string MonsterList;
	public string BackGroundMap;
	public string BackGroundSound;
	public int MaxRound;
	public int TimeUpWin;
	public int PlayerCardMax;
	public int FriendSupportMax;
	public string NpcSupportList;
	public string RewardList;
	public int DescrptionID;

	public static readonly string urlKey = "StageConfig";
	static Dictionary<int,StageConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,StageConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					StageConfig config = new StageConfig();

					int.TryParse(el.GetAttribute ("StageID"), out config.StageID);

					int.TryParse(el.GetAttribute ("MaxWaves"), out config.MaxWaves);

					config.MonsterList = el.GetAttribute ("MonsterList");

					config.BackGroundMap = el.GetAttribute ("BackGroundMap");

					config.BackGroundSound = el.GetAttribute ("BackGroundSound");

					int.TryParse(el.GetAttribute ("MaxRound"), out config.MaxRound);

					int.TryParse(el.GetAttribute ("TimeUpWin"), out config.TimeUpWin);

					int.TryParse(el.GetAttribute ("PlayerCardMax"), out config.PlayerCardMax);

					int.TryParse(el.GetAttribute ("FriendSupportMax"), out config.FriendSupportMax);

					config.NpcSupportList = el.GetAttribute ("NpcSupportList");

					config.RewardList = el.GetAttribute ("RewardList");

					int.TryParse(el.GetAttribute ("DescrptionID"), out config.DescrptionID);

					AllDatas.Add(config.StageID, config);
				}
			}
		}
	}

	public static StageConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,StageConfig> Get()
	{
		return AllDatas;
	}
}
