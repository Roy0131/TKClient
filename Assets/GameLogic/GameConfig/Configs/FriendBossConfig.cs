// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class FriendBossConfig
{
	public int ID;
	public int LevelMin;
	public int LevelMax;
	public int BossStageID;
	public int ChallengeDropID;
	public string RewardLastHit;
	public string RewardOwner;
	public int BossIDShow;
	public int BossRankShow;
	public int BossLevelShow;

	public static readonly string urlKey = "FriendBossConfig";
	static Dictionary<int,FriendBossConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,FriendBossConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					FriendBossConfig config = new FriendBossConfig();

					int.TryParse(el.GetAttribute ("ID"), out config.ID);

					int.TryParse(el.GetAttribute ("LevelMin"), out config.LevelMin);

					int.TryParse(el.GetAttribute ("LevelMax"), out config.LevelMax);

					int.TryParse(el.GetAttribute ("BossStageID"), out config.BossStageID);

					int.TryParse(el.GetAttribute ("ChallengeDropID"), out config.ChallengeDropID);

					config.RewardLastHit = el.GetAttribute ("RewardLastHit");

					config.RewardOwner = el.GetAttribute ("RewardOwner");

					int.TryParse(el.GetAttribute ("BossIDShow"), out config.BossIDShow);

					int.TryParse(el.GetAttribute ("BossRankShow"), out config.BossRankShow);

					int.TryParse(el.GetAttribute ("BossLevelShow"), out config.BossLevelShow);

					AllDatas.Add(config.ID, config);
				}
			}
		}
	}

	public static FriendBossConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,FriendBossConfig> Get()
	{
		return AllDatas;
	}
}
