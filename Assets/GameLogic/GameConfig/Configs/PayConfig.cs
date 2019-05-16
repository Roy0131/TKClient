// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class PayConfig
{
	public int ID;
	public int Name;
	public int ImgName;
	public int ActivePay;
	public string BundleID;
	public int GemRewardFirst;
	public int GemReward;
	public int MonthCardDay;
	public int MonthCardReward;
	public int RewardShow;
	public int RewardBonusShow;
	public string ItemReward;
	public string RecordGold;

	public static readonly string urlKey = "PayConfig";
	static Dictionary<int,PayConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,PayConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					PayConfig config = new PayConfig();

					int.TryParse(el.GetAttribute ("ID"), out config.ID);

					int.TryParse(el.GetAttribute ("Name"), out config.Name);

					int.TryParse(el.GetAttribute ("ImgName"), out config.ImgName);

					int.TryParse(el.GetAttribute ("ActivePay"), out config.ActivePay);

					config.BundleID = el.GetAttribute ("BundleID");

					int.TryParse(el.GetAttribute ("GemRewardFirst"), out config.GemRewardFirst);

					int.TryParse(el.GetAttribute ("GemReward"), out config.GemReward);

					int.TryParse(el.GetAttribute ("MonthCardDay"), out config.MonthCardDay);

					int.TryParse(el.GetAttribute ("MonthCardReward"), out config.MonthCardReward);

					int.TryParse(el.GetAttribute ("RewardShow"), out config.RewardShow);

					int.TryParse(el.GetAttribute ("RewardBonusShow"), out config.RewardBonusShow);

					config.ItemReward = el.GetAttribute ("ItemReward");

					config.RecordGold = el.GetAttribute ("RecordGold");

					AllDatas.Add(config.ID, config);
				}
			}
		}
	}

	public static PayConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,PayConfig> Get()
	{
		return AllDatas;
	}
}
