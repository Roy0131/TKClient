// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class EffectConfig
{
	public string Name;
	public int Layer;
	public int Duration;
	public int SelfMutexType;
	public int EffectSpecialType;

	public static readonly string urlKey = "EffectConfig";
	static Dictionary<string,EffectConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<string,EffectConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					EffectConfig config = new EffectConfig();

					config.Name = el.GetAttribute ("Name");

					int.TryParse(el.GetAttribute ("Layer"), out config.Layer);

					int.TryParse(el.GetAttribute ("Duration"), out config.Duration);

					int.TryParse(el.GetAttribute ("SelfMutexType"), out config.SelfMutexType);

					int.TryParse(el.GetAttribute ("EffectSpecialType"), out config.EffectSpecialType);

					AllDatas.Add(config.Name, config);
				}
			}
		}
	}

	public static EffectConfig Get(string key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<string,EffectConfig> Get()
	{
		return AllDatas;
	}
}
