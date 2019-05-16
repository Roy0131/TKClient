// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class ArtifactUnlockConfig
{
	public int ArtifactID;
	public int UnLockLevel;
	public int UnLockVIPLevel;
	public string UnLockResCost;
	public int MaxRank;
	public int Name;
	public int Sort;
	public int ShowLevel;
	public string BackGroundRGB;
	public string BackGroundImg;
	public string SelectFx;

	public static readonly string urlKey = "ArtifactUnlockConfig";
	static Dictionary<int,ArtifactUnlockConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,ArtifactUnlockConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					ArtifactUnlockConfig config = new ArtifactUnlockConfig();

					int.TryParse(el.GetAttribute ("ArtifactID"), out config.ArtifactID);

					int.TryParse(el.GetAttribute ("UnLockLevel"), out config.UnLockLevel);

					int.TryParse(el.GetAttribute ("UnLockVIPLevel"), out config.UnLockVIPLevel);

					config.UnLockResCost = el.GetAttribute ("UnLockResCost");

					int.TryParse(el.GetAttribute ("MaxRank"), out config.MaxRank);

					int.TryParse(el.GetAttribute ("Name"), out config.Name);

					int.TryParse(el.GetAttribute ("Sort"), out config.Sort);

					int.TryParse(el.GetAttribute ("ShowLevel"), out config.ShowLevel);

					config.BackGroundRGB = el.GetAttribute ("BackGroundRGB");

					config.BackGroundImg = el.GetAttribute ("BackGroundImg");

					config.SelectFx = el.GetAttribute ("SelectFx");

					AllDatas.Add(config.ArtifactID, config);
				}
			}
		}
	}

	public static ArtifactUnlockConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,ArtifactUnlockConfig> Get()
	{
		return AllDatas;
	}
}
