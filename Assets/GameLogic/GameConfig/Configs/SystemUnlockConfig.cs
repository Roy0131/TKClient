// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class SystemUnlockConfig
{
	public string ServerID;
	public int Level;
	public int LinkID;
	public string ClientGuildShowIcon;
	public int NameID;

	public static readonly string urlKey = "SystemUnlockConfig";
	static Dictionary<string,SystemUnlockConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<string,SystemUnlockConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					SystemUnlockConfig config = new SystemUnlockConfig();

					config.ServerID = el.GetAttribute ("ServerID");

					int.TryParse(el.GetAttribute ("Level"), out config.Level);

					int.TryParse(el.GetAttribute ("LinkID"), out config.LinkID);

					config.ClientGuildShowIcon = el.GetAttribute ("ClientGuildShowIcon");

					int.TryParse(el.GetAttribute ("NameID"), out config.NameID);

					AllDatas.Add(config.ServerID, config);
				}
			}
		}
	}

	public static SystemUnlockConfig Get(string key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<string,SystemUnlockConfig> Get()
	{
		return AllDatas;
	}
}
