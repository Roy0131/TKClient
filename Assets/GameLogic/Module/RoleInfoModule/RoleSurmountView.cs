using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public class RoleSurmountView : UIBaseView
{
    private Button _disBtn;
    private CardDataVO _cardDataVO;
    private SkillView _skillView;
    private RectTransform _parent;

    private GameObject _lvObject;
    private GameObject _battlePowerObject;
    private GameObject _hpPowerObject;
    private GameObject _attackObject;
    private GameObject _defenseObject;

    private CardView _card1;
    private CardView _card2;
    private int _fusionId;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _disBtn = Find<Button>("DisBtn");
        _lvObject = Find("BjObj/LevelObject");
        _battlePowerObject = Find("BjObj/PowerObject");
        _hpPowerObject = Find("BjObj/HpObject");
        _attackObject = Find("BjObj/AttackObject");
        _defenseObject = Find("BjObj/AromrObject");
        _parent = Find<RectTransform>("BjObj/CardGroup/CardGrid");

        _skillView = new SkillView();
        _skillView.SetDisplayObject(Find("BjObj/SkillGroup"));
        _disBtn.onClick.Add(OnDisBtn);

        ColliderHelper.SetButtonCollider(_disBtn.transform);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _cardDataVO = args[0] as CardDataVO;
        _fusionId = int.Parse(args[1].ToString());
        FusionConfig config = GameConfigMgr.Instance.GetFusionConfig(_fusionId);
        CardDataVO nVO = new CardDataVO(config.MainCardID, _cardDataVO.mCardRank, _cardDataVO.mCardLevel, _cardDataVO.DictEquipment);

        FillAttriValue(_cardDataVO.mCardConfig.MaxLevel, nVO.mCardConfig.MaxLevel, _lvObject.transform);
        FillAttriValue(_cardDataVO.mBattlePower, nVO.mBattlePower, _battlePowerObject.transform);
        FillAttriValue(_cardDataVO.GetAttriByType(AttributesType.HP), nVO.GetAttriByType(AttributesType.HP), _hpPowerObject.transform);
        FillAttriValue(_cardDataVO.GetAttriByType(AttributesType.ATTACK), nVO.GetAttriByType(AttributesType.ATTACK), _attackObject.transform);
        FillAttriValue(_cardDataVO.GetAttriByType(AttributesType.DEFENSE), nVO.GetAttriByType(AttributesType.DEFENSE), _defenseObject.transform);
        //if (_card1 != null)
        //{
        //    CardViewFactory.Instance.ReturnCardView(_card1);
        //    _card1 = null;
        //}
        //if (_card2 != null)
        //{
        //    CardViewFactory.Instance.ReturnCardView(_card2);
        //    _card2 = null;
        //}
        //_card1 = new CardView();
        //_card2 = new CardView();
        _card1 = CardViewFactory.Instance.CreateCardView(nVO, CardViewType.Common);
        _card1.mRectTransform.SetParent(_parent, false);
        _card2 = CardViewFactory.Instance.CreateCardView(_cardDataVO, CardViewType.Common);
        _card2.mRectTransform.SetParent(_parent, false);

        FusionConfig cfg = GameConfigMgr.Instance.GetFusionConfig(nVO.mCardConfig.ID);
        int cardId = cfg.ResultDropID * 100 + nVO.mCardRank;
        CardConfig targetCardConfig = GameConfigMgr.Instance.GetCardConfig(cardId);
        string skillValue = "";
        string[] oldSkill = nVO.mCardConfig.ShowSkillID.Split(',');
        string newSkill = targetCardConfig.ShowSkillID;
        string oldVaule;
        for (int i = 0; i < oldSkill.Length; i += 2)
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
        _skillView.Show(skillValue, nVO.mCardRank);
        SkillDataVO.OnSkillType(true);
    }

    private void FillAttriValue(int curValue, int nextValue, Transform transform)
    {
        transform.Find("oldValue").GetComponent<Text>().text = nextValue.ToString();
        transform.Find("newValue").GetComponent<Text>().text = curValue.ToString();
    }

    private void OnDisBtn()
    {
        SkillDataVO.OnSkillType(false);
        if (_card1 != null)
        {
            CardViewFactory.Instance.ReturnCardView(_card1);
            _card1 = null;
        }
        if (_card2 != null)
        {
            CardViewFactory.Instance.ReturnCardView(_card2);
            _card2 = null;
        }
        Hide();
    }

    public override void Dispose()
    {
        SkillDataVO.OnSkillType(false);
        if (_card1 != null)
        {
            CardViewFactory.Instance.ReturnCardView(_card1);
            _card1 = null;
        }
        if (_card2 != null)
        {
            CardViewFactory.Instance.ReturnCardView(_card2);
            _card2 = null;
        }
        if (_skillView != null)
        {
            _skillView.Dispose();
            _skillView = null;
        }
        base.Dispose();
        
    }
}
