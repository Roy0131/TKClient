using Framework.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuildBossCopyItemView : UIBaseView
{
    GuildBossHurtVO hurtVO;
    private Text _rank;
    private Text _grade;
    private Text _name;
    private Text _hurt;
    private Image _head;
    private List<GameObject> _listRankObj;


    protected override void ParseComponent()
    {
        base.ParseComponent();
        _rank = Find<Text>("Rank/Text");
        _grade = Find<Text>("Grade/Text");
        _name = Find<Text>("Name");
        _hurt = Find<Text>("Hurt");
        _head = Find<Image>("Head");
        _listRankObj = new List<GameObject>();
        for (int i = 0; i < 3; i++)
        {
            GameObject obj = Find("Rank/Rank" + (i + 1));
            _listRankObj.Add(obj);
        }
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        hurtVO = args[0] as GuildBossHurtVO;
        if (hurtVO.mDamage.Rank > 3)
        {
            _rank.text = hurtVO.mDamage.Rank.ToString();
            for (int i = 0; i < _listRankObj.Count; i++)
                _listRankObj[i].SetActive(false);
        }
        else
        {
            for (int i = 0; i < _listRankObj.Count; i++)
            {
                if (hurtVO.mDamage.Rank == (i + 1))
                    _listRankObj[i].SetActive(true);
                else
                    _listRankObj[i].SetActive(false);
            }
            _rank.text = "";
        }
        _grade.text = hurtVO.mDamage.Level.ToString();
        _name.text = hurtVO.mDamage.MemberName;
        _hurt.text = UnitChange.GetUnitNum(hurtVO.mDamage.Damage);
        if (hurtVO.mDamage.Head > 0)
        {
            _head.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(hurtVO.mDamage.Head).Icon);
            ObjectHelper.SetSprite(_head,_head.sprite);
        }
        else
            _head.sprite = null;
    }
}
