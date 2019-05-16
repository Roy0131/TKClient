// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class MainActiveConfig
{
	public int MainActiveID;
	public int ActiveType;
	public int EventID;
	public int Title;
	public int Description;
	public string StartTime;
	public string EndTime;
	public string SubActiveList;
	public string FrontImg;
	public string BackImg;

	public static readonly string urlKey = "MainActiveConfig";
	static Dictionary<int,MainActiveConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,MainActiveConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					MainActiveConfig config = new MainActiveConfig();

					int.TryParse(el.GetAttribute ("MainActiveID"), out config.MainActiveID);

					int.TryParse(el.GetAttribute ("ActiveType"), out config.ActiveType);

					int.TryParse(el.GetAttribute ("EventID"), out config.EventID);

					int.TryParse(el.GetAttribute ("Title"), out config.Title);

					int.TryParse(el.GetAttribute ("Description"), out config.Description);

					config.StartTime = el.GetAttribute ("StartTime");

					config.EndTime = el.GetAttribute ("EndTime");

					config.SubActiveList = el.GetAttribute ("SubActiveList");

					config.FrontImg = el.GetAttribute ("FrontImg");

					config.BackImg = el.GetAttribute ("BackImg");

					AllDatas.Add(config.MainActiveID, config);
				}
			}
		}
	}

	public static MainActiveConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,MainActiveConfig> Get()
	{
		return AllDatas;
	}
}
