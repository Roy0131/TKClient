// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class VipConfig
{
	public int VipLevel;
	public int Money;
	public int AccelTimes;
	public int ActiveStageBuyTimes;
	public int GoldFingerBonus;
	public int HonorPointBonus;
	public string MonthCardItemBonus;
	public int QuickBattle;
	public int SearchTaskCount;
	public string VIPItemRewardShow;

	public static readonly string urlKey = "VipConfig";
	static Dictionary<int,VipConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,VipConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					VipConfig config = new VipConfig();

					int.TryParse(el.GetAttribute ("VipLevel"), out config.VipLevel);

					int.TryParse(el.GetAttribute ("Money"), out config.Money);

					int.TryParse(el.GetAttribute ("AccelTimes"), out config.AccelTimes);

					int.TryParse(el.GetAttribute ("ActiveStageBuyTimes"), out config.ActiveStageBuyTimes);

					int.TryParse(el.GetAttribute ("GoldFingerBonus"), out config.GoldFingerBonus);

					int.TryParse(el.GetAttribute ("HonorPointBonus"), out config.HonorPointBonus);

					config.MonthCardItemBonus = el.GetAttribute ("MonthCardItemBonus");

					int.TryParse(el.GetAttribute ("QuickBattle"), out config.QuickBattle);

					int.TryParse(el.GetAttribute ("SearchTaskCount"), out config.SearchTaskCount);

					config.VIPItemRewardShow = el.GetAttribute ("VIPItemRewardShow");

					AllDatas.Add(config.VipLevel, config);
				}
			}
		}
	}

	public static VipConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,VipConfig> Get()
	{
		return AllDatas;
	}
}
