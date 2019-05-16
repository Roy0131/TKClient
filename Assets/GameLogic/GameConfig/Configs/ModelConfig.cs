// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class ModelConfig
{
	public string Name;
	public int Width;
	public int Height;

	public static readonly string urlKey = "ModelConfig";
	static Dictionary<string,ModelConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<string,ModelConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					ModelConfig config = new ModelConfig();

					config.Name = el.GetAttribute ("Name");

					int.TryParse(el.GetAttribute ("Width"), out config.Width);

					int.TryParse(el.GetAttribute ("Height"), out config.Height);

					AllDatas.Add(config.Name, config);
				}
			}
		}
	}

	public static ModelConfig Get(string key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<string,ModelConfig> Get()
	{
		return AllDatas;
	}
}
