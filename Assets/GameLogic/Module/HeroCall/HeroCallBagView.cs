using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public class HeroCallBagView : UIBaseView
{
    //private readonly ExploreDataVO _exploreDataVo;
    private int _numCard;
    private CardView _cardView;
    private GameObject _objGrid;
    private Button _btnClose;

    private Dictionary<CardDataVO, bool> _dictCardIsChoose;
    private List<CardDataVO> _lstVo;

    private int _cardId;

    //private int _id;
    private bool _createCamp;
    private bool _createType;
    private int _camp;
    private int _type;
    private Dis _dis;
    private readonly List<int> _lstSelnum = new List<int>();
    private List<CardView> _view;

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _lstVo = args[0] as List<CardDataVO>;
        _cardId = int.Parse(args[1].ToString());
        _dis = (Dis)args[2];
        OnCreateCard(_lstVo);
        OnSetSelState();
        OnCloseItem();

    }
    protected override void ParseComponent()
    {
        base.ParseComponent();
        _objGrid = Find("HeroCardGrid/ScrollView/Content");
        _btnClose = Find<Button>("ImageBackBtn");
        _btnClose.onClick.Add(Hide);
        for (int i = 0; i < 6; i++)
        {
            int j = i;
            Find("GridCamp/Image" + j).GetComponent<Image>().sprite = GameResMgr.Instance.LoadCampIcon(j + 1);
            Find<Button>("GridCamp/Image" + j).onClick.Add(() => { OnCamp(j + 1); });
        }

        for (int i = 0; i < 3; i++)
        {
            int j = i;
            Find("GridCamp/Image" + (j + 6)).GetComponent<Image>().sprite = GameResMgr.Instance.LoadTypeIcon(j + 1);
            Find<Button>("GridCamp/Image" + (j + 6)).onClick.Add(() => { OnType(j + 1); });
        }

       
    }
    /// <summary>
    /// 初始打开设置阵营和类型的选择关闭
    /// </summary>
    private void OnSetSelState()
    {
        for (int i = 0; i < 9; i++)
        {
            int j = i;
            Find("GridCamp/Image" + j).transform.Find("Selection").gameObject.SetActive(false);
        }
    }

    private void OnCloseItem()
    {
        if (_dis == Dis.disCamp)
        {
            for (int i = 0; i < 6; i++)
            {
                int j = i;
                Find("GridCamp/Image" + j).SetActive(true);
            }
            for (int i = 0; i < 3; i++)
            {
                int j = i;
                Find("GridCamp/Image" + (j + 6)).SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                int j = i;
                Find("GridCamp/Image" + j).SetActive(false);
            }
            for (int i = 0; i < 3; i++)
            {
                int j = i;
                Find("GridCamp/Image" + (j + 6)).SetActive(true);
            }
        }
    }
    //创建卡牌
    private void OnCreateCard(List<CardDataVO> lstVo)
    {
        //if (_objGrid.transform.childCount != 0)
        //    for (int i = 0; i < _objGrid.transform.childCount; i++)
        //        Object.Destroy(_objGrid.transform.GetChild(i).gameObject);
        if (_view != null)
        {
            for (int i = 0; i < _view.Count; i++)
            {
                CardViewFactory.Instance.ReturnCardView(_view[i]);
            }
        }
        _view = new List<CardView>();
        if (lstVo != null)
        {
            if (lstVo.Count != 0)
            {
                for (int i = 0; i < lstVo.Count; i++)
                {
                    CardView cardView = CardViewFactory.Instance.CreateCardView(lstVo[i], CardViewType.HeroCall, OnClick);
                    cardView.mRectTransform.SetParent(_objGrid.transform, false);
                    _view.Add(cardView);
                    if (lstVo[i].mCardID == _cardId)
                        cardView.BlSelected = true;
                }
            }
        }
    }

    private void OnClick(CardView item)
    {
        //if (item.BlSelected) return;
        item.BlSelected = true;
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(HeroCallEvent.HeroSelect, item);
        Hide();
    }


    private void OnCamp(int camp)
    {
        SetPicValue();
        Find("GridCamp/Image" + (camp - 1)).transform.Find("Selection").gameObject.SetActive(true);

        if (!_createCamp)
        {
            OnOrderCamp(camp);
            _createCamp = true;
            _camp = camp;
        }
        else
        {
            if (_camp - camp == 0)
            {
                OnCreateCard(_lstVo);
                _createCamp = false;
                SetPicValue();
            }
            else
            {
                OnOrderCamp(camp);
                _createCamp = true;
            }

            _camp = camp;
        }
    }

    private void OnType(int type)
    {
        SetPicValue();
        Find("GridCamp/Image" + (type + 5)).transform.Find("Selection").gameObject.SetActive(true);
        if (!_createType)
        {
            OnOrderType(type);
            _createType = true;
            _type = type;
        }
        else
        {
            if (_type - type == 0)
            {
                OnCreateCard(_lstVo);
                _createType = false;
                SetPicValue();
            }
            else
            {
                OnOrderType(type);
                _createType = true;
            }

            _type = type;
        }
    }

    //排序阵营
    private void OnOrderCamp(int camp)
    {
        List<CardDataVO> value = new List<CardDataVO>();

        for (int i = 0; i < _lstVo.Count; i++)
        {
            if (_lstVo[i].mCardConfig.Camp == camp)
                value.Add(_lstVo[i]);
        }
        OnCreateCard(value);
    }

    //排序类型
    private void OnOrderType(int type)
    {
        List<CardDataVO> value = new List<CardDataVO>();

        for (int i = 0; i < _lstVo.Count; i++)
        {
            if (_lstVo[i].mCardConfig.Type == type)
                value.Add(_lstVo[i]);
        }
        OnCreateCard(value);
        _createType = true;
    }

    private void SetPicValue()
    {
        for (int i = 0; i < 9; i++) Find("GridCamp/Image" + i).transform.Find("Selection").gameObject.SetActive(false);
    }

    public override void Hide()
    {

        base.Hide();
    }
}