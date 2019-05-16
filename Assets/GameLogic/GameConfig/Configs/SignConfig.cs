// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class SignConfig
{
	public int TotalIndex;
	public int Group;
	public int GroupIndex;
	public string Reward;

	public static readonly string urlKey = "SignConfig";
	static Dictionary<int,SignConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,SignConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					SignConfig config = new SignConfig();

					int.TryParse(el.GetAttribute ("TotalIndex"), out config.TotalIndex);

					int.TryParse(el.GetAttribute ("Group"), out config.Group);

					int.TryParse(el.GetAttribute ("GroupIndex"), out config.GroupIndex);

					config.Reward = el.GetAttribute ("Reward");

					AllDatas.Add(config.TotalIndex, config);
				}
			}
		}
	}

	public static SignConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,SignConfig> Get()
	{
		return AllDatas;
	}
}
