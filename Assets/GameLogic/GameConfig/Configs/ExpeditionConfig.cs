// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class ExpeditionConfig
{
	public int ID;
	public string BackGroundMap;
	public int StageType;
	public int PlayerCardMax;
	public int PurifyPoint;

	public static readonly string urlKey = "ExpeditionConfig";
	static Dictionary<int,ExpeditionConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,ExpeditionConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					ExpeditionConfig config = new ExpeditionConfig();

					int.TryParse(el.GetAttribute ("ID"), out config.ID);

					config.BackGroundMap = el.GetAttribute ("BackGroundMap");

					int.TryParse(el.GetAttribute ("StageType"), out config.StageType);

					int.TryParse(el.GetAttribute ("PlayerCardMax"), out config.PlayerCardMax);

					int.TryParse(el.GetAttribute ("PurifyPoint"), out config.PurifyPoint);

					AllDatas.Add(config.ID, config);
				}
			}
		}
	}

	public static ExpeditionConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,ExpeditionConfig> Get()
	{
		return AllDatas;
	}
}
