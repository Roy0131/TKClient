using Framework.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuildBossHurtItemView : UIBaseView
{
    GuildBossHurtVO hurtVO;
    private Text _rank;
    private Text _name;
    private Text _hurt;
    private Image _head;
    private Image _fill;
    private List<GameObject> _listRankObj;
    private RectTransform _parent;
    private int _bossId;


    protected override void ParseComponent()
    {
        base.ParseComponent();
        _rank = Find<Text>("Rank/Text");
        _name = Find<Text>("Name");
        _hurt = Find<Text>("FillImg/Text");
        _head = Find<Image>("Head");
        _fill = Find<Image>("FillImg/Fill");
        _parent = Find<RectTransform>("ItemObj");
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
        _bossId = int.Parse(args[0].ToString());
        hurtVO = args[1] as GuildBossHurtVO;
        DiposeChildren();
        _childrenViews = new List<UIBaseView>();
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
                if (hurtVO.mDamage.Rank == (i+1))
                    _listRankObj[i].SetActive(true);
                else
                    _listRankObj[i].SetActive(false);
            }
            _rank.text = "";
        }

        _name.text = hurtVO.mDamage.MemberName;
        _hurt.text = hurtVO.mDamage.Damage.ToString();
        if (hurtVO.mDamage.Head > 0)
        {
            _head.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(hurtVO.mDamage.Head).Icon);
            ObjectHelper.SetSprite(_head,_head.sprite);
        }
        else
            _head.sprite = null;
        _fill.fillAmount = (float)hurtVO.mDamage.Damage / (float)GuildBossDataModel.Instance.HurtBossId(_bossId);
        ItemInfo iteminfo = new ItemInfo();
        ItemView view = new ItemView();
        iteminfo = GuildBossDataModel.Instance.GetRankReward(hurtVO.mDamage.Rank);
        view = ItemFactory.Instance.CreateItemView(iteminfo, ItemViewType.HeroItem, null);
        view.mRectTransform.SetParent(_parent, false);
        AddChildren(view);
    }
}
