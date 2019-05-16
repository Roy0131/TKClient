// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class FusionConfig
{
	public int FormulaID;
	public int FusionType;
	public int ResultDropID;
	public string Icon;
	public string LeftCornerIcon;
	public int StarShow;
	public int MainCardID;
	public int MainCardLevelCond;
	public string ResCondtion;
	public int Cost1IDCond;
	public int Cost1CampCond;
	public int Cost1TypeCond;
	public int Cost1StarCond;
	public int Cost1NumCond;
	public string Cost1Icon;
	public int Cost2IDCond;
	public int Cost2CampCond;
	public int Cost2TypeCond;
	public int Cost2StarCond;
	public int Cost2NumCond;
	public string Cost2Icon;
	public int Cost3IDCond;
	public int Cost3CampCond;
	public int Cost3TypeCond;
	public int Cost3StarCond;
	public int Cost3NumCond;
	public string Cost3Icon;

	public static readonly string urlKey = "FusionConfig";
	static Dictionary<int,FusionConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,FusionConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					FusionConfig config = new FusionConfig();

					int.TryParse(el.GetAttribute ("FormulaID"), out config.FormulaID);

					int.TryParse(el.GetAttribute ("FusionType"), out config.FusionType);

					int.TryParse(el.GetAttribute ("ResultDropID"), out config.ResultDropID);

					config.Icon = el.GetAttribute ("Icon");

					config.LeftCornerIcon = el.GetAttribute ("LeftCornerIcon");

					int.TryParse(el.GetAttribute ("StarShow"), out config.StarShow);

					int.TryParse(el.GetAttribute ("MainCardID"), out config.MainCardID);

					int.TryParse(el.GetAttribute ("MainCardLevelCond"), out config.MainCardLevelCond);

					config.ResCondtion = el.GetAttribute ("ResCondtion");

					int.TryParse(el.GetAttribute ("Cost1IDCond"), out config.Cost1IDCond);

					int.TryParse(el.GetAttribute ("Cost1CampCond"), out config.Cost1CampCond);

					int.TryParse(el.GetAttribute ("Cost1TypeCond"), out config.Cost1TypeCond);

					int.TryParse(el.GetAttribute ("Cost1StarCond"), out config.Cost1StarCond);

					int.TryParse(el.GetAttribute ("Cost1NumCond"), out config.Cost1NumCond);

					config.Cost1Icon = el.GetAttribute ("Cost1Icon");

					int.TryParse(el.GetAttribute ("Cost2IDCond"), out config.Cost2IDCond);

					int.TryParse(el.GetAttribute ("Cost2CampCond"), out config.Cost2CampCond);

					int.TryParse(el.GetAttribute ("Cost2TypeCond"), out config.Cost2TypeCond);

					int.TryParse(el.GetAttribute ("Cost2StarCond"), out config.Cost2StarCond);

					int.TryParse(el.GetAttribute ("Cost2NumCond"), out config.Cost2NumCond);

					config.Cost2Icon = el.GetAttribute ("Cost2Icon");

					int.TryParse(el.GetAttribute ("Cost3IDCond"), out config.Cost3IDCond);

					int.TryParse(el.GetAttribute ("Cost3CampCond"), out config.Cost3CampCond);

					int.TryParse(el.GetAttribute ("Cost3TypeCond"), out config.Cost3TypeCond);

					int.TryParse(el.GetAttribute ("Cost3StarCond"), out config.Cost3StarCond);

					int.TryParse(el.GetAttribute ("Cost3NumCond"), out config.Cost3NumCond);

					config.Cost3Icon = el.GetAttribute ("Cost3Icon");

					AllDatas.Add(config.FormulaID, config);
				}
			}
		}
	}

	public static FusionConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,FusionConfig> Get()
	{
		return AllDatas;
	}
}
