using Framework.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleRankupView : UIBaseView
{
    private Button _closeBtn;
    private Button _rankupBtn;
    private ItemResGroup _itemResGroup;
    private ResCostGroup _costGroup;
    
    private GameObject _lvObject;
    private GameObject _battlePowerObject;
    private GameObject _hpPowerObject;
    private GameObject _attackObject;
    private GameObject _defenseObject;
    private GameObject _skillGroupObject;
    private GameObject _skillTipsObject;

    private CardDataVO _vo;
    private SkillView _skillView;

    protected override void ParseComponent()
	{
        base.ParseComponent();

        _skillView = new SkillView();
        _skillView.SetDisplayObject(Find("SkillGroup"));

        _closeBtn = Find<Button>("ButtonClose");
        _rankupBtn = Find<Button>("ButtonRankUp");

        _itemResGroup = new ItemResGroup();
        _itemResGroup.SetDisplayObject(Find("ResGroup"));

        _costGroup = new ResCostGroup();
        _costGroup.SetDisplayObject(Find("CostGroup"));

        _lvObject = Find("LevelObject");
        _battlePowerObject = Find("PowerObject");
        _hpPowerObject = Find("HpObject");
        _attackObject = Find("AttackObject");
        _defenseObject = Find("AromrObject");
        _skillGroupObject = Find("SkillGroup");
        _skillTipsObject = Find("SkillGroup/SkillTips");

        _closeBtn.onClick.Add(OnClose);
        _rankupBtn.onClick.Add(OnRankUp);

        ColliderHelper.SetButtonCollider(_closeBtn.transform);
    }

    protected override void Refresh(params object[] args)
	{
        base.Refresh(args);
        if (args.Length > 0)
            _vo = args[0] as CardDataVO;


        CardDataVO nVO = new CardDataVO(_vo.mCardTableId, _vo.mCardRank + 1, _vo.mCardLevel, _vo.DictEquipment);

        FillAttriValue(_vo.mCardConfig.MaxLevel, nVO.mCardConfig.MaxLevel, _lvObject.transform);
        FillAttriValue(_vo.mBattlePower, nVO.mBattlePower, _battlePowerObject.transform);
        FillAttriValue(_vo.GetAttriByType(AttributesType.HP), nVO.GetAttriByType(AttributesType.HP), _hpPowerObject.transform);
        FillAttriValue(_vo.GetAttriByType(AttributesType.ATTACK), nVO.GetAttriByType(AttributesType.ATTACK), _attackObject.transform);
        FillAttriValue(_vo.GetAttriByType(AttributesType.DEFENSE), nVO.GetAttriByType(AttributesType.DEFENSE), _defenseObject.transform);
        RankUpConfig config = GameConfigMgr.Instance.GetRankUpConfig(_vo.mCardRank);
        string costValue = "";
        if (_vo.mCardConfig.Type == 1)
            costValue = config.Type1RankUpRes;
        else if (_vo.mCardConfig.Type == 2)
            costValue = config.Type2RankUpRes;
        else
            costValue = config.Type3RankUpRes;
        string[] tmpCost = costValue.Split(',');
        object[] tmp = new object[tmpCost.Length / 2];
        int index = 0;
        for (int i = 0; i < tmpCost.Length; i+= 2)
        {
            tmp[index] = tmpCost[i];
            index++;
        }

        _costGroup.Show(costValue);
        _itemResGroup.Show(tmp);


        Dictionary<int, int> dictSkill = new Dictionary<int, int>();
        string[] showSkill = _vo.mCardConfig.ShowSkillID.Split(',');
        if (showSkill.Length % 2 != 0)
            return;
        for (int i = 0; i < showSkill.Length; i += 2)
            dictSkill.Add(int.Parse(showSkill[i]), int.Parse(showSkill[i + 1]));
        if (dictSkill.ContainsKey(nVO.mCardRank))
        {
            _skillGroupObject.gameObject.SetActive(true);
            SkillConfig cfg = GameConfigMgr.Instance.GetSkillConfig(dictSkill[nVO.mCardRank]);
            string skillValue = nVO.mCardRank + "," + dictSkill[nVO.mCardRank];
            _skillView.Show(skillValue, nVO.mCardRank);
            SkillDataVO.OnSkillType(true);
        }
        else
        {
            _skillGroupObject.gameObject.SetActive(false);
        }
    }

    private void FillAttriValue(int curValue, int nextValue, Transform transform)
    {
        transform.Find("oldValue").GetComponent<Text>().text = curValue.ToString();
        transform.Find("newValue").GetComponent<Text>().text = nextValue.ToString();
    }

	private void OnClose()
    {
        GameUIMgr.Instance.CloseModule(ModuleID.RoleRankup);
        _vo = null;
        SkillDataVO.OnSkillType(false);
    }

    private void OnRankUp()
    {
        if (!_costGroup.BlEnough)
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001185));
            return;
        }    
        GameNetMgr.Instance.mGameServer.ReqRoleRankup(_vo.mCardID);
        OnClose();
        SkillDataVO.OnSkillType(false);
    }
	
}