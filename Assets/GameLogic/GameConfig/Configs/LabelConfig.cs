// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class LabelConfig
{
	public int Label;
	public string Icon;
	public int NameID;
	public int ExtaDescription;

	public static readonly string urlKey = "LabelConfig";
	static Dictionary<int,LabelConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,LabelConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					LabelConfig config = new LabelConfig();

					int.TryParse(el.GetAttribute ("Label"), out config.Label);

					config.Icon = el.GetAttribute ("Icon");

					int.TryParse(el.GetAttribute ("NameID"), out config.NameID);

					int.TryParse(el.GetAttribute ("ExtaDescription"), out config.ExtaDescription);

					AllDatas.Add(config.Label, config);
				}
			}
		}
	}

	public static LabelConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,LabelConfig> Get()
	{
		return AllDatas;
	}
}
