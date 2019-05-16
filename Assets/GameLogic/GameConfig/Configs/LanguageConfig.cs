// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class LanguageConfig
{
	public int ID { get; set; }
	public string Chinese { get; set; }
	public string English { get; set; }
	public string TraditionalChinese { get; set; }

	public static readonly string urlKey = "LanguageConfig";
	static Dictionary<int,LanguageConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,LanguageConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					LanguageConfig config = new LanguageConfig();

					config.ID = int.Parse(el.GetAttribute ("ID"));

					config.Chinese = el.GetAttribute ("Chinese");

					config.English = el.GetAttribute ("English");

					config.TraditionalChinese = el.GetAttribute ("TraditionalChinese");

					AllDatas.Add(config.ID, config);
				}
			}
		}
	}

	public static LanguageConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,LanguageConfig> Get()
	{
		return AllDatas;
	}
}
