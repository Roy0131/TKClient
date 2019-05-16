// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class GuildLevelUpConfig
{
	public int Level;
	public int Exp;
	public int Members;

	public static readonly string urlKey = "GuildLevelUpConfig";
	static Dictionary<int,GuildLevelUpConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,GuildLevelUpConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					GuildLevelUpConfig config = new GuildLevelUpConfig();

					int.TryParse(el.GetAttribute ("Level"), out config.Level);

					int.TryParse(el.GetAttribute ("Exp"), out config.Exp);

					int.TryParse(el.GetAttribute ("Members"), out config.Members);

					AllDatas.Add(config.Level, config);
				}
			}
		}
	}

	public static GuildLevelUpConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,GuildLevelUpConfig> Get()
	{
		return AllDatas;
	}
}
