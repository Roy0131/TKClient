// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class StatusConfig
{
	public int BuffID;
	public string Effect;
	public int Type;
	public string Icon;
	public int ClientSpecialEffect;
	public string ShowEffect;
	public int BuffTextID;

	public static readonly string urlKey = "StatusConfig";
	static Dictionary<int,StatusConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,StatusConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					StatusConfig config = new StatusConfig();

					int.TryParse(el.GetAttribute ("BuffID"), out config.BuffID);

					config.Effect = el.GetAttribute ("Effect");

					int.TryParse(el.GetAttribute ("Type"), out config.Type);

					config.Icon = el.GetAttribute ("Icon");

					int.TryParse(el.GetAttribute ("ClientSpecialEffect"), out config.ClientSpecialEffect);

					config.ShowEffect = el.GetAttribute ("ShowEffect");

					int.TryParse(el.GetAttribute ("BuffTextID"), out config.BuffTextID);

					AllDatas.Add(config.BuffID, config);
				}
			}
		}
	}

	public static StatusConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,StatusConfig> Get()
	{
		return AllDatas;
	}
}
