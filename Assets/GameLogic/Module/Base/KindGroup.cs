using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Framework.UI;
using System;

public class KindGroup : UIBaseView
{
    private List<Button> _lstKindBtns;
    private List<GameObject> _lstSelectFlags;
    private Action<int> _onKindChange;
    private int _curKind = 0;
    private bool _blCanKindEmpty = true;

    public KindGroup(Action<int> onKindChange)
    {
        _onKindChange = onKindChange;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _lstKindBtns = new List<Button>();
        _lstSelectFlags = new List<GameObject>();

        Button btn;
        for (int i = 1; i <= 6; i++)
        {
            btn = Find<Button>("Kind" + i);
            int tmp = i;
            btn.onClick.Add(delegate { OnKindChange(tmp); });
            _lstKindBtns.Add(btn);
            _lstSelectFlags.Add(btn.transform.Find("Selected").gameObject);
        }
    }

    public int KindID
    {
        get { return _curKind; }
    }

    public void OnKindReset()
    {
        if (_curKind == 0)
        {
            return;
        }
        _lstSelectFlags[_curKind - 1].SetActive(false);
        _curKind = 0;
    }

    public void SetKind(int kind)
    {
        if (_curKind == kind)
            return;
        if (_curKind != 0)
            _lstSelectFlags[_curKind - 1].SetActive(false);
        _curKind = kind;
        _lstSelectFlags[_curKind - 1].SetActive(true);
    }

    public void SetEmptyKindValue( bool value)
    {
        _blCanKindEmpty = value;
    }

    private void OnKindChange(int kindValue)
    {
        if (_curKind != 0)
            _lstSelectFlags[_curKind - 1].SetActive(false);
        if (_curKind != 0 && _curKind == kindValue)
        {
            if (!_blCanKindEmpty)
            {
                _lstSelectFlags[_curKind - 1].SetActive(true);
                return;
            }
            _curKind = 0;
        }
        else
        {
            _curKind = kindValue;
            _lstSelectFlags[_curKind - 1].SetActive(true);
        }
        if (_onKindChange != null)
            _onKindChange.Invoke(_curKind);
    }

    public override void Dispose()
    {
        if (_lstKindBtns != null)
        {
            _lstKindBtns.Clear();
            _lstKindBtns = null;

            _lstSelectFlags.Clear();
            _lstSelectFlags = null;
        }
        _onKindChange = null;
        base.Dispose();
    }
}

public class CardRarityView : UIBaseView
{
    private List<GameObject> _lstStars;
    private int _curRarity = -1;
    private List<GameObject> _lstUIPrefabs;

	protected override void ParseComponent()
	{
        base.ParseComponent();
        _lstStars = new List<GameObject>();
        _lstUIPrefabs = new List<GameObject>();
        _lstUIPrefabs.Add(Find("star"));
        _lstUIPrefabs.Add(Find("star1"));
        _lstUIPrefabs.Add(Find("star2"));
	}

	protected override void Refresh(params object[] args)
	{
        base.Refresh(args);
        int rarity = int.Parse(args[0].ToString());
        if (rarity == _curRarity)
            return;
        _curRarity = rarity;
        int idx = (_curRarity - 1) / 5;
        GameObject cloneStar = _lstUIPrefabs[idx];
        int len = (_curRarity - 1) % 5 + 1;
        ClearStar();
        GameObject starObject;
        for (int i = 0; i < len; i++)
        {
            starObject = GameObject.Instantiate(cloneStar);
            starObject.SetActive(true);
            starObject.transform.SetParent(mRectTransform, false);
            _lstStars.Add(starObject);
        }
	}

    private void ClearStar()
    {
        if(_lstStars.Count > 0)
        {
            for (int i = 0; i < _lstStars.Count; i++)
                GameObject.Destroy(_lstStars[i]);
            _lstStars.Clear();
        }
    }

	public override void Dispose()
	{
        ClearStar();
        if(_lstUIPrefabs != null)
        {
            _lstUIPrefabs.Clear();
            _lstUIPrefabs = null;
        }
        _lstStars = null;
        base.Dispose();
	}
}

public class RankGroupView : UIBaseView
{
    private GameObject _itemObj;

    private List<GameObject> _lstTotalObj;
    private List<GameObject> _lstCurObj;

	protected override void ParseComponent()
	{
        base.ParseComponent();
        _itemObj = Find("item");

        _lstTotalObj = new List<GameObject>();
        _lstCurObj = new List<GameObject>();
        GameObject itemObj;
        for (int i = 0; i < 8; i++)
        {
            itemObj = GameObject.Instantiate(_itemObj);
            itemObj.transform.SetParent(mRectTransform, false);
            _lstTotalObj.Add(itemObj);
            _lstCurObj.Add(itemObj.transform.Find("icon").gameObject);
        }
	}

	protected override void Refresh(params object[] args)
	{
        base.Refresh(args);
        CardDataVO vO = args[0] as CardDataVO;
        int rank = vO.mCardRank - 1;
        int maxRank = vO.mCardConfig.MaxRank - 1;
      //  Debuger.LogWarning("Rank:" + rank + ", maxRank:" + maxRank);

        for (int i = 0; i < _lstTotalObj.Count; i++)
        {
            _lstCurObj[i].SetActive(i < rank);
            _lstTotalObj[i].SetActive(i < maxRank);
        }
	}
}