// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class TalentConfig
{
	public int TalentID;
	public int TalentBaseID;
	public int MaxLevel;
	public int Level;
	public int CanLearn;
	public string UpgradeCost;
	public int PreSkillCond;
	public int PreSkillLevCond;
	public int PageLabel;
	public int TeamSpeedBonus;
	public string TalentEffectCond;
	public string TalentAttr;
	public string ShowParam;
	public int NameID;
	public int DescrptionID;
	public string Icon;
	public string PanelIcon;

	public static readonly string urlKey = "TalentConfig";
	static Dictionary<int,TalentConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,TalentConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					TalentConfig config = new TalentConfig();

					int.TryParse(el.GetAttribute ("TalentID"), out config.TalentID);

					int.TryParse(el.GetAttribute ("TalentBaseID"), out config.TalentBaseID);

					int.TryParse(el.GetAttribute ("MaxLevel"), out config.MaxLevel);

					int.TryParse(el.GetAttribute ("Level"), out config.Level);

					int.TryParse(el.GetAttribute ("CanLearn"), out config.CanLearn);

					config.UpgradeCost = el.GetAttribute ("UpgradeCost");

					int.TryParse(el.GetAttribute ("PreSkillCond"), out config.PreSkillCond);

					int.TryParse(el.GetAttribute ("PreSkillLevCond"), out config.PreSkillLevCond);

					int.TryParse(el.GetAttribute ("PageLabel"), out config.PageLabel);

					int.TryParse(el.GetAttribute ("TeamSpeedBonus"), out config.TeamSpeedBonus);

					config.TalentEffectCond = el.GetAttribute ("TalentEffectCond");

					config.TalentAttr = el.GetAttribute ("TalentAttr");

					config.ShowParam = el.GetAttribute ("ShowParam");

					int.TryParse(el.GetAttribute ("NameID"), out config.NameID);

					int.TryParse(el.GetAttribute ("DescrptionID"), out config.DescrptionID);

					config.Icon = el.GetAttribute ("Icon");

					config.PanelIcon = el.GetAttribute ("PanelIcon");

					AllDatas.Add(config.TalentID, config);
				}
			}
		}
	}

	public static TalentConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,TalentConfig> Get()
	{
		return AllDatas;
	}
}
