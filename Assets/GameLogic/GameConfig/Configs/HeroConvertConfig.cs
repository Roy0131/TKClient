// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class HeroConvertConfig
{
	public int ClientIndex;
	public int ConvertGroupID;
	public int HeroID;

	public static readonly string urlKey = "HeroConvertConfig";
	static Dictionary<int,HeroConvertConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,HeroConvertConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					HeroConvertConfig config = new HeroConvertConfig();

					int.TryParse(el.GetAttribute ("ClientIndex"), out config.ClientIndex);

					int.TryParse(el.GetAttribute ("ConvertGroupID"), out config.ConvertGroupID);

					int.TryParse(el.GetAttribute ("HeroID"), out config.HeroID);

					AllDatas.Add(config.ClientIndex, config);
				}
			}
		}
	}

	public static HeroConvertConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,HeroConvertConfig> Get()
	{
		return AllDatas;
	}
}
