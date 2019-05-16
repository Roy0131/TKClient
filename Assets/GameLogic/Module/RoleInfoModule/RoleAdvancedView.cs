using Framework.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleAdvancedView : UIBaseView
{
    private Text _skillName;
    private Button _disBtn;
    private CardDataVO _cardDataVO;
    private SkillView _skillView;

    private GameObject _lvObject;
    private GameObject _battlePowerObject;
    private GameObject _hpPowerObject;
    private GameObject _attackObject;
    private GameObject _defenseObject;
    private GameObject _skillGroupObject;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _skillName = Find<Text>("BjObj/SkillGroup/Text");
        _disBtn = Find<Button>("DisBtn");

        _lvObject = Find("BjObj/LevelObject");
        _battlePowerObject = Find("BjObj/PowerObject");
        _hpPowerObject = Find("BjObj/HpObject");
        _attackObject = Find("BjObj/AttackObject");
        _defenseObject = Find("BjObj/AromrObject");
        _skillGroupObject = Find("BjObj/SkillGroup");

        _skillView = new SkillView();
        _skillView.SetDisplayObject(Find("BjObj/SkillGroup"));

        _disBtn.onClick.Add(OnDisBtn);
        ColliderHelper.SetButtonCollider(_disBtn.transform);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _cardDataVO = args[0] as CardDataVO;
        CardDataVO nVO = new CardDataVO(_cardDataVO.mCardTableId, _cardDataVO.mCardRank - 1, _cardDataVO.mCardLevel, _cardDataVO.DictEquipment);

        FillAttriValue(_cardDataVO.mCardConfig.MaxLevel, nVO.mCardConfig.MaxLevel, _lvObject.transform);
        FillAttriValue(_cardDataVO.mBattlePower, nVO.mBattlePower, _battlePowerObject.transform);
        FillAttriValue(_cardDataVO.GetAttriByType(AttributesType.HP), nVO.GetAttriByType(AttributesType.HP), _hpPowerObject.transform);
        FillAttriValue(_cardDataVO.GetAttriByType(AttributesType.ATTACK), nVO.GetAttriByType(AttributesType.ATTACK), _attackObject.transform);
        FillAttriValue(_cardDataVO.GetAttriByType(AttributesType.DEFENSE), nVO.GetAttriByType(AttributesType.DEFENSE), _defenseObject.transform);

        Dictionary<int, int> dictSkill = new Dictionary<int, int>();
        string[] showSkill = _cardDataVO.mCardConfig.ShowSkillID.Split(',');
        if (showSkill.Length % 2 != 0)
            return;
        for (int i = 0; i < showSkill.Length; i += 2)
            dictSkill.Add(int.Parse(showSkill[i]), int.Parse(showSkill[i + 1]));
        if (dictSkill.ContainsKey(_cardDataVO.mCardRank))
        {
            _skillGroupObject.gameObject.SetActive(true);
            SkillConfig cfg = GameConfigMgr.Instance.GetSkillConfig(dictSkill[_cardDataVO.mCardRank]);
            string skillValue = _cardDataVO.mCardRank + "," + dictSkill[_cardDataVO.mCardRank];
            _skillView.Show(skillValue, _cardDataVO.mCardRank);
            SkillDataVO.OnSkillType(true);
            _skillName.text = LanguageMgr.GetLanguage(cfg.NameID);
        }
        else
        {
            _skillGroupObject.gameObject.SetActive(false);
        }
    }

    private void FillAttriValue(int curValue, int nextValue, Transform transform)
    {
        transform.Find("oldValue").GetComponent<Text>().text = nextValue.ToString();
        transform.Find("newValue").GetComponent<Text>().text = curValue.ToString();
    }

    private void OnDisBtn()
    {
        Hide();
        SkillDataVO.OnSkillType(false);
    }

    public override void Dispose()
    {
        if (_skillView != null)
        {
            _skillView.Dispose();
            _skillView = null;
        }
        SkillDataVO.OnSkillType(false);
        base.Dispose();
    }
}
