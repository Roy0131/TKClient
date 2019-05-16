// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class DifficultyConfig
{
	public int DifficultyID;
	public int ChapterCount;
	public int Icon;

	public static readonly string urlKey = "DifficultyConfig";
	static Dictionary<int,DifficultyConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,DifficultyConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					DifficultyConfig config = new DifficultyConfig();

					int.TryParse(el.GetAttribute ("DifficultyID"), out config.DifficultyID);

					int.TryParse(el.GetAttribute ("ChapterCount"), out config.ChapterCount);

					int.TryParse(el.GetAttribute ("Icon"), out config.Icon);

					AllDatas.Add(config.DifficultyID, config);
				}
			}
		}
	}

	public static DifficultyConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,DifficultyConfig> Get()
	{
		return AllDatas;
	}
}
