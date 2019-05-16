// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class SearchTaskDialogConfig
{
	public int TaskNameIndex;
	public string DialogTextList;

	public static readonly string urlKey = "SearchTaskDialogConfig";
	static Dictionary<int,SearchTaskDialogConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,SearchTaskDialogConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					SearchTaskDialogConfig config = new SearchTaskDialogConfig();

					int.TryParse(el.GetAttribute ("TaskNameIndex"), out config.TaskNameIndex);

					config.DialogTextList = el.GetAttribute ("DialogTextList");

					AllDatas.Add(config.TaskNameIndex, config);
				}
			}
		}
	}

	public static SearchTaskDialogConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,SearchTaskDialogConfig> Get()
	{
		return AllDatas;
	}
}
