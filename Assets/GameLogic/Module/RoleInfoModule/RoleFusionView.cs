using Framework.UI;
using UnityEngine.UI;

public class RoleFusionView : UIBaseView
{
    private SkillView _skillView;
    private CardRarityView _curStarView;
    private CardRarityView _tarStarView;

    private Text _levelUpMax;

    private FusionLogicView _logicView;
    private int _cardId;

    private Text _textPp;

	protected override void ParseComponent()
	{
        base.ParseComponent();

        _skillView = new SkillView();
        _skillView.SetDisplayObject(Find("SkillGroup"));

        _curStarView = new CardRarityView();
        _curStarView.SetDisplayObject(Find("Stars/curStarGroup/uiStarGroup"));

        _tarStarView = new CardRarityView();
        _tarStarView.SetDisplayObject(Find("Stars/tarStarGroup/uiStarGroup"));

        _logicView = new FusionLogicView();
        _logicView.SetDisplayObject(Find("FusionRoot"));

        _levelUpMax = Find<Text>("StaticTextRoot/LevelUpMax");
        _textPp = Find<Text>("StaticTextRoot/TextPp");
    }

    public override void Hide()
    {
        base.Hide();
        if (_logicView != null)
            _logicView.Hide();
    }
  
    protected override void Refresh(params object[] args)
	{
        base.Refresh(args);

        CardDataVO vo = args[0] as CardDataVO;
        //if (vo.mCardID == _cardId)
        //    return;
        _cardId = vo.mCardID;
        _logicView.mMainCardId = _cardId;
        _logicView.Show(vo.mCardConfig.ID);
        
        FusionConfig cfg = GameConfigMgr.Instance.GetFusionConfig(vo.mCardConfig.ID);
        int cardId = cfg.ResultDropID * 100 + vo.mCardRank;
        CardConfig targetCardConfig = GameConfigMgr.Instance.GetCardConfig(cardId);
        if (targetCardConfig == null)
            return;
        _levelUpMax.text = LanguageMgr.GetLanguage(5002732) + targetCardConfig.MaxLevel;
        _textPp.text = LanguageMgr.GetLanguage(5002733);
        _curStarView.Show(vo.mCardConfig.Rarity);
        _tarStarView.Show(targetCardConfig.Rarity);

        string skillValue = "";
        string[] oldSkill = vo.mCardConfig.ShowSkillID.Split(',');
        string newSkill = targetCardConfig.ShowSkillID;
        string oldVaule;
        for (int i = 0; i < oldSkill.Length; i+=2)
        {
            oldVaule = oldSkill[i] + "," + oldSkill[i + 1];
            if (newSkill.Contains(oldVaule))
                newSkill = newSkill.Replace(oldVaule, "");
            else
                skillValue = oldVaule;
        }
        newSkill = newSkill.Replace(",", "");
        string rank = newSkill.Substring(0, 1);
        string id = newSkill.Substring(1, newSkill.Length - 1);
        skillValue = skillValue + "," + rank + "," + id;
        _skillView.Show(skillValue, vo.mCardRank);

    }

	public override void Dispose()
	{
        if(_skillView != null)
        {
            _skillView.Dispose();
            _skillView = null;
        }
        if(_curStarView != null)
        {
            _curStarView.Dispose();
            _curStarView = null;
        }
        if(_tarStarView != null)
        {
            _tarStarView.Dispose();
            _tarStarView = null;
        }
        if(_logicView != null)
        {
            _logicView.Dispose();
            _logicView = null;
        }
        base.Dispose();
	}
}