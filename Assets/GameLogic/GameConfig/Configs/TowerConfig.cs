// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class TowerConfig
{
	public int TowerID;
	public int StageID;
	public int UnlockTower;

	public static readonly string urlKey = "TowerConfig";
	static Dictionary<int,TowerConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,TowerConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					TowerConfig config = new TowerConfig();

					int.TryParse(el.GetAttribute ("TowerID"), out config.TowerID);

					int.TryParse(el.GetAttribute ("StageID"), out config.StageID);

					int.TryParse(el.GetAttribute ("UnlockTower"), out config.UnlockTower);

					AllDatas.Add(config.TowerID, config);
				}
			}
		}
	}

	public static TowerConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,TowerConfig> Get()
	{
		return AllDatas;
	}
}
