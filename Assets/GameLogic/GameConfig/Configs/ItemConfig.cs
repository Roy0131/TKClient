// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class ItemConfig
{
	public int ID;
	public int ItemType;
	public int EquipType;
	public string SellReward;
	public int Quality;
	public int ShowStar;
	public string EquipAttr;
	public string EquipSkill;
	public int ComposeNum;
	public int ComposeType;
	public int ComposeDropID;
	public int NameID;
	public int DescrptionID;
	public string Icon;
	public string UIIcon;
	public int BattlePower;
	public int SuitID;
	public string GetLink;
	public string LeftCornerIcon;

	public static readonly string urlKey = "ItemConfig";
	static Dictionary<int,ItemConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,ItemConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					ItemConfig config = new ItemConfig();

					int.TryParse(el.GetAttribute ("ID"), out config.ID);

					int.TryParse(el.GetAttribute ("ItemType"), out config.ItemType);

					int.TryParse(el.GetAttribute ("EquipType"), out config.EquipType);

					config.SellReward = el.GetAttribute ("SellReward");

					int.TryParse(el.GetAttribute ("Quality"), out config.Quality);

					int.TryParse(el.GetAttribute ("ShowStar"), out config.ShowStar);

					config.EquipAttr = el.GetAttribute ("EquipAttr");

					config.EquipSkill = el.GetAttribute ("EquipSkill");

					int.TryParse(el.GetAttribute ("ComposeNum"), out config.ComposeNum);

					int.TryParse(el.GetAttribute ("ComposeType"), out config.ComposeType);

					int.TryParse(el.GetAttribute ("ComposeDropID"), out config.ComposeDropID);

					int.TryParse(el.GetAttribute ("NameID"), out config.NameID);

					int.TryParse(el.GetAttribute ("DescrptionID"), out config.DescrptionID);

					config.Icon = el.GetAttribute ("Icon");

					config.UIIcon = el.GetAttribute ("UIIcon");

					int.TryParse(el.GetAttribute ("BattlePower"), out config.BattlePower);

					int.TryParse(el.GetAttribute ("SuitID"), out config.SuitID);

					config.GetLink = el.GetAttribute ("GetLink");

					config.LeftCornerIcon = el.GetAttribute ("LeftCornerIcon");

					AllDatas.Add(config.ID, config);
				}
			}
		}
	}

	public static ItemConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,ItemConfig> Get()
	{
		return AllDatas;
	}
}
