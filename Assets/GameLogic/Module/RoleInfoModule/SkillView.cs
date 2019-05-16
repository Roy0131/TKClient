using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class SkillView : UIBaseView
{
    private RoleSkillGroup _skillGroup;
    private GameObject _skillTips;
    private Image _skillTipsIcon;
    private Text _skillName;
    private Text _skillDes;
    private Text _unlockCond;
    private Text _skillRank;
    private GameObject _skillImg;
    private int _skillId;

	protected override void ParseComponent()
	{
        base.ParseComponent();

        _skillGroup = new RoleSkillGroup();
        _skillGroup.SetDisplayObject(Find("SkillGrid"));
        AddChildren(_skillGroup);

        _skillTips = Find("SkillTips");
        _skillName = Find<Text>("SkillTips/skillName");
        _skillDes = Find<Text>("SkillTips/skillDes");
        _unlockCond = Find<Text>("SkillTips/unLockCond");
        _skillTipsIcon = Find<Image>("SkillTips/ImageIcon");
        _skillImg = Find("SkillTips/ImageIcon/Img");
        _skillRank = Find<Text>("SkillTips/ImageIcon/Img/Text");
        _skillTips.SetActive(false);
	}

	protected override void Refresh(params object[] args)
	{
		_skillGroup.Show(args);
        base.Refresh(args);
	}

	protected override void AddEvent()
	{
        base.AddEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<int, int,bool>(UIEventDefines.ShowSkillTips, OnShowSkillTips);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(UIEventDefines.HideSkillTips, OnHideSkillTips);
	}

	protected override void RemoveEvent()
	{
        base.RemoveEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<int, int,bool>(UIEventDefines.ShowSkillTips, OnShowSkillTips);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(UIEventDefines.HideSkillTips, OnHideSkillTips);
	}

	private void OnShowSkillTips(int skillId, int rankCond,bool blUnlock)
    {
        _skillTips.SetActive(true);
        if (_skillId == skillId)
            return;
        _skillId = skillId;
        SkillConfig config = GameConfigMgr.Instance.GetSkillConfig(skillId);
        _skillTipsIcon.sprite = GameResMgr.Instance.LoadSkillIcon(config.Icon);
        string[] args = config.ShowParam.Split(',');
        _skillDes.text = LanguageMgr.GetLanguage(config.DescrptionID, args);
        _skillName.text = LanguageMgr.GetLanguage(config.NameID);
        if (!blUnlock)
        {
            _unlockCond.gameObject.SetActive(true);
            _unlockCond.text = LanguageMgr.GetLanguage(5002710, rankCond - 1);
        }
        else
        {
            _unlockCond.gameObject.SetActive(false);
        }
        _skillRank.text = config.InnerLevel.ToString();
        _skillImg.SetActive(config.InnerLevel > 1);
    }

    private void OnHideSkillTips()
    {
        _skillTips.SetActive(false);
    }
}

public class SkillDataVO
{
    public static bool mSkillType { get; private set; }

    public static void OnSkillType(bool type)
    {
        mSkillType = type;
    }
}