// Auto Generated Code
// Author roy

using System.Collections.Generic;
using System.Xml;

public class SkillConfig
{
	public int ID;
	public int Type;
	public string SkillAttr;
	public int SkillMelee;
	public int SkillEnemy;
	public int RangeType;
	public int SkillTarget;
	public int MaxTarget;
	public int InnerLevel;
	public string ShowParam;
	public int NameID;
	public int DescrptionID;
	public string Icon;
	public int SkillAnimType;
	public string SkillCastEffect;
	public string MoveTargetOffset;
	public int ReverseType;
	public string CastAnim;
	public int CastAnimBeforeMove;
	public string MoveTimeInCast;
	public int CastTime;
	public string CastSound;
	public int CastSoundDelay;
	public int BulletType;
	public string BulletShowTime;
	public int BulletContinueTime;
	public int BulletTargetType;
	public int BulletPenetrate;
	public string BulletAnim;
	public int BulletSpeed;
	public string BulletHitEffect;
	public string BulletHitSound;
	public string DamageSplit;
	public string HitShowTime;
	public string ChaHitEffect;
	public string ChaHitSound;
	public int ShockScreen;

	public static readonly string urlKey = "SkillConfig";
	static Dictionary<int,SkillConfig> AllDatas;

	public static void Parse(XmlNode node)
	{
		AllDatas = new Dictionary<int,SkillConfig>();
		if (node != null)
		{
			XmlNodeList nodeList = node.ChildNodes;
			if (nodeList != null && nodeList.Count > 0)
			{
				foreach (XmlElement el in nodeList)
				{
					SkillConfig config = new SkillConfig();

					int.TryParse(el.GetAttribute ("ID"), out config.ID);

					int.TryParse(el.GetAttribute ("Type"), out config.Type);

					config.SkillAttr = el.GetAttribute ("SkillAttr");

					int.TryParse(el.GetAttribute ("SkillMelee"), out config.SkillMelee);

					int.TryParse(el.GetAttribute ("SkillEnemy"), out config.SkillEnemy);

					int.TryParse(el.GetAttribute ("RangeType"), out config.RangeType);

					int.TryParse(el.GetAttribute ("SkillTarget"), out config.SkillTarget);

					int.TryParse(el.GetAttribute ("MaxTarget"), out config.MaxTarget);

					int.TryParse(el.GetAttribute ("InnerLevel"), out config.InnerLevel);

					config.ShowParam = el.GetAttribute ("ShowParam");

					int.TryParse(el.GetAttribute ("NameID"), out config.NameID);

					int.TryParse(el.GetAttribute ("DescrptionID"), out config.DescrptionID);

					config.Icon = el.GetAttribute ("Icon");

					int.TryParse(el.GetAttribute ("SkillAnimType"), out config.SkillAnimType);

					config.SkillCastEffect = el.GetAttribute ("SkillCastEffect");

					config.MoveTargetOffset = el.GetAttribute ("MoveTargetOffset");

					int.TryParse(el.GetAttribute ("ReverseType"), out config.ReverseType);

					config.CastAnim = el.GetAttribute ("CastAnim");

					int.TryParse(el.GetAttribute ("CastAnimBeforeMove"), out config.CastAnimBeforeMove);

					config.MoveTimeInCast = el.GetAttribute ("MoveTimeInCast");

					int.TryParse(el.GetAttribute ("CastTime"), out config.CastTime);

					config.CastSound = el.GetAttribute ("CastSound");

					int.TryParse(el.GetAttribute ("CastSoundDelay"), out config.CastSoundDelay);

					int.TryParse(el.GetAttribute ("BulletType"), out config.BulletType);

					config.BulletShowTime = el.GetAttribute ("BulletShowTime");

					int.TryParse(el.GetAttribute ("BulletContinueTime"), out config.BulletContinueTime);

					int.TryParse(el.GetAttribute ("BulletTargetType"), out config.BulletTargetType);

					int.TryParse(el.GetAttribute ("BulletPenetrate"), out config.BulletPenetrate);

					config.BulletAnim = el.GetAttribute ("BulletAnim");

					int.TryParse(el.GetAttribute ("BulletSpeed"), out config.BulletSpeed);

					config.BulletHitEffect = el.GetAttribute ("BulletHitEffect");

					config.BulletHitSound = el.GetAttribute ("BulletHitSound");

					config.DamageSplit = el.GetAttribute ("DamageSplit");

					config.HitShowTime = el.GetAttribute ("HitShowTime");

					config.ChaHitEffect = el.GetAttribute ("ChaHitEffect");

					config.ChaHitSound = el.GetAttribute ("ChaHitSound");

					int.TryParse(el.GetAttribute ("ShockScreen"), out config.ShockScreen);

					AllDatas.Add(config.ID, config);
				}
			}
		}
	}

	public static SkillConfig Get(int key)
	{
		if (AllDatas != null && AllDatas.ContainsKey(key))
			return AllDatas[key];
		return null;
	}

	public static Dictionary<int,SkillConfig> Get()
	{
		return AllDatas;
	}
}
