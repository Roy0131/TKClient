// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class CardConfig
{
	public int ClientID;
	public int ID;
	public int Rank;
	public int MaxLevel;
	public int MaxRank;
	public int Rarity;
	public int Type;
	public int Camp;
	public string Label;
	public string Model;
	public int ShaderSize;
	public int BaseHP;
	public int BaseAttack;
	public int BaseDefence;
	public int GrowthHP;
	public int GrowthAttack;
	public int GrowthDefence;
	public int NormalSkillID;
	public int SuperSkillID;
	public string PassiveSkillID;
	public string DecomposeRes;
	public string Icon;
	public int Name;
	public string ShowSkillID;
	public int BattlePower;
	public int BattlePowerGrowth;
	public int IsHandBook;
	public int BreakShow;
	public string BagFullChangeItem;
	public int ConvertID1;
	public int ConvertID2;
	public string ConvertItem;

	public static readonly string urlKey = "CardConfig";
	static Dictionary<int,CardConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,CardConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					CardConfig config = new CardConfig();

					int.TryParse(el.GetAttribute ("ClientID"), out config.ClientID);

					int.TryParse(el.GetAttribute ("ID"), out config.ID);

					int.TryParse(el.GetAttribute ("Rank"), out config.Rank);

					int.TryParse(el.GetAttribute ("MaxLevel"), out config.MaxLevel);

					int.TryParse(el.GetAttribute ("MaxRank"), out config.MaxRank);

					int.TryParse(el.GetAttribute ("Rarity"), out config.Rarity);

					int.TryParse(el.GetAttribute ("Type"), out config.Type);

					int.TryParse(el.GetAttribute ("Camp"), out config.Camp);

					config.Label = el.GetAttribute ("Label");

					config.Model = el.GetAttribute ("Model");

					int.TryParse(el.GetAttribute ("ShaderSize"), out config.ShaderSize);

					int.TryParse(el.GetAttribute ("BaseHP"), out config.BaseHP);

					int.TryParse(el.GetAttribute ("BaseAttack"), out config.BaseAttack);

					int.TryParse(el.GetAttribute ("BaseDefence"), out config.BaseDefence);

					int.TryParse(el.GetAttribute ("GrowthHP"), out config.GrowthHP);

					int.TryParse(el.GetAttribute ("GrowthAttack"), out config.GrowthAttack);

					int.TryParse(el.GetAttribute ("GrowthDefence"), out config.GrowthDefence);

					int.TryParse(el.GetAttribute ("NormalSkillID"), out config.NormalSkillID);

					int.TryParse(el.GetAttribute ("SuperSkillID"), out config.SuperSkillID);

					config.PassiveSkillID = el.GetAttribute ("PassiveSkillID");

					config.DecomposeRes = el.GetAttribute ("DecomposeRes");

					config.Icon = el.GetAttribute ("Icon");

					int.TryParse(el.GetAttribute ("Name"), out config.Name);

					config.ShowSkillID = el.GetAttribute ("ShowSkillID");

					int.TryParse(el.GetAttribute ("BattlePower"), out config.BattlePower);

					int.TryParse(el.GetAttribute ("BattlePowerGrowth"), out config.BattlePowerGrowth);

					int.TryParse(el.GetAttribute ("IsHandBook"), out config.IsHandBook);

					int.TryParse(el.GetAttribute ("BreakShow"), out config.BreakShow);

					config.BagFullChangeItem = el.GetAttribute ("BagFullChangeItem");

					int.TryParse(el.GetAttribute ("ConvertID1"), out config.ConvertID1);

					int.TryParse(el.GetAttribute ("ConvertID2"), out config.ConvertID2);

					config.ConvertItem = el.GetAttribute ("ConvertItem");

					AllDatas.Add(config.ClientID, config);
				}
			}
		}
	}

	public static CardConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,CardConfig> Get()
	{
		return AllDatas;
	}
}
