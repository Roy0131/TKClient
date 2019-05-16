// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class SearchTaskConfig
{
	public int Id;
	public int Type;
	public int TaskStar;
	public int CardStarCond;
	public int CardNum;
	public int SearchTime;
	public int AccelCost;
	public string ConstReward;
	public string TaskNameList;
	public string TaskRoleList;

	public static readonly string urlKey = "SearchTaskConfig";
	static Dictionary<int,SearchTaskConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,SearchTaskConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					SearchTaskConfig config = new SearchTaskConfig();

					int.TryParse(el.GetAttribute ("Id"), out config.Id);

					int.TryParse(el.GetAttribute ("Type"), out config.Type);

					int.TryParse(el.GetAttribute ("TaskStar"), out config.TaskStar);

					int.TryParse(el.GetAttribute ("CardStarCond"), out config.CardStarCond);

					int.TryParse(el.GetAttribute ("CardNum"), out config.CardNum);

					int.TryParse(el.GetAttribute ("SearchTime"), out config.SearchTime);

					int.TryParse(el.GetAttribute ("AccelCost"), out config.AccelCost);

					config.ConstReward = el.GetAttribute ("ConstReward");

					config.TaskNameList = el.GetAttribute ("TaskNameList");

					config.TaskRoleList = el.GetAttribute ("TaskRoleList");

					AllDatas.Add(config.Id, config);
				}
			}
		}
	}

	public static SearchTaskConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,SearchTaskConfig> Get()
	{
		return AllDatas;
	}
}
