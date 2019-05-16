// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class ActiveStageConfig
{
	public int ID;
	public int Type;
	public int PlayerLevelCond;
	public int StageID;
	public int PlayerLevelSuggestion;

	public static readonly string urlKey = "ActiveStageConfig";
	static Dictionary<int,ActiveStageConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,ActiveStageConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					ActiveStageConfig config = new ActiveStageConfig();

					int.TryParse(el.GetAttribute ("ID"), out config.ID);

					int.TryParse(el.GetAttribute ("Type"), out config.Type);

					int.TryParse(el.GetAttribute ("PlayerLevelCond"), out config.PlayerLevelCond);

					int.TryParse(el.GetAttribute ("StageID"), out config.StageID);

					int.TryParse(el.GetAttribute ("PlayerLevelSuggestion"), out config.PlayerLevelSuggestion);

					AllDatas.Add(config.ID, config);
				}
			}
		}
	}

	public static ActiveStageConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,ActiveStageConfig> Get()
	{
		return AllDatas;
	}
}
