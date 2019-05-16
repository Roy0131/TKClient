using Framework.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuildBossKillItemView : UIBaseView
{
    private ItemInfo _info;
    private int _min;
    private int _max;
    private Text _rank;
    private RectTransform _parent;
    private List<GameObject> _listRankObj;


    protected override void ParseComponent()
    {
        base.ParseComponent();
        _rank = Find<Text>("Rank/Text");
        _parent = Find<RectTransform>("Obj");
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
        _info = args[0] as ItemInfo;
        _min = int.Parse(args[1].ToString());
        _max = int.Parse(args[2].ToString());
        OnKillItem();
    }

    private void OnKillItem()
    {
        DiposeChildren();
        _childrenViews = new List<UIBaseView>();
        ItemView view;
        view = ItemFactory.Instance.CreateItemView(_info, ItemViewType.RewardItem, null);
        view.mRectTransform.SetParent(_parent, false);
        AddChildren(view);
        if (_min == _max)
        {
            for (int i = 0; i < _listRankObj.Count; i++)
            {
                if (_min == (i + 1))
                    _listRankObj[i].SetActive(true);
                else
                    _listRankObj[i].SetActive(false);
            }
            _rank.text = "";
        }
        else
        {
            _rank.text = _min + "-" + _max;
            for (int i = 0; i < _listRankObj.Count; i++)
                _listRankObj[i].SetActive(false);
        }
    }
}
