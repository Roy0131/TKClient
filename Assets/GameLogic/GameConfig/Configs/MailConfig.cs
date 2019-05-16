// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class MailConfig
{
	public int MailID;
	public int MailType;
	public int MailTitleID;
	public int MailContentID;

	public static readonly string urlKey = "MailConfig";
	static Dictionary<int,MailConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,MailConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					MailConfig config = new MailConfig();

					int.TryParse(el.GetAttribute ("MailID"), out config.MailID);

					int.TryParse(el.GetAttribute ("MailType"), out config.MailType);

					int.TryParse(el.GetAttribute ("MailTitleID"), out config.MailTitleID);

					int.TryParse(el.GetAttribute ("MailContentID"), out config.MailContentID);

					AllDatas.Add(config.MailID, config);
				}
			}
		}
	}

	public static MailConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,MailConfig> Get()
	{
		return AllDatas;
	}
}
