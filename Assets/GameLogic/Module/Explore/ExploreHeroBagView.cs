using Framework.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ExploreHeroBagView : UIBaseView
{
    private ExploreDataVO _exploreDataVo;
    private GameObject _objGrid;
    private RectTransform _rect;
    private Button _btnClose;

    private List<CardDataVO> _lstVo = new List<CardDataVO>();
    private List<CardView> _listView;
    private List<CardDataVO> _lstSel;


    private List<int> _lstSelnum;

    private int _id;
    private bool _createCamp;
    private bool _createType = false;
    private int _camp;
    private int _type;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _objGrid = Find("HeroCardGrid/ScrollView/Content");
        _rect = Find<RectTransform>("HeroCardGrid/ScrollView/Content");
        _btnClose = Find<Button>("ImageBackBtn");
        _btnClose.onClick.Add(OnClose);
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

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        if (_lstSel != null)
            _lstSel.Clear();
        _lstSel = new List<CardDataVO>();
        if (_lstSelnum != null)
            _lstSelnum.Clear();
        _lstSelnum = new List<int>();

        _lstVo = args[0] as List<CardDataVO>;
        _exploreDataVo = args[1] as ExploreDataVO;
        _id = int.Parse(args[2].ToString());
        foreach (var kv in args[3] as Dictionary<CardDataVO, int>)
        {
            _lstSel.Add(kv.Key);
            _lstSelnum.Add(kv.Value);
        }
        OnCreateCard(_lstVo);
    }

    //创建卡牌
    private void OnCreateCard(List<CardDataVO> lstVo)
    {
        _rect.anchoredPosition = new Vector2(0f, 64f);
        OnClear();
        _listView = new List<CardView>();
        if (lstVo.Count == 0) return;
        {
            for (int i = 0; i < lstVo.Count; i++)
            {
                CardView cardView = CardViewFactory.Instance.CreateCardView(lstVo[i], CardViewType.Common, OnClick);
                cardView.mRectTransform.SetParent(_objGrid.transform, false);
                //服务器返回的已经选择的角色
                if (ExploreDataModel.Instance.mLstSelRoleID != null && ExploreDataModel.Instance.mLstSelRoleID.Count > 0)
                {
                    for (int j = 0; j < ExploreDataModel.Instance.mLstSelRoleID.Count; j++)
                    {
                        if (cardView.mCardDataVO.mCardID == ExploreDataModel.Instance.mLstSelRoleID[j])
                            cardView.BlSelected = true;
                    }
                }
                if (_lstSel != null && _lstSel.Count > 0)
                {
                    for (int j = 0; j < _lstSel.Count; j++)
                        if (cardView.mCardDataVO.mCardID == _lstSel[j].mCardID)
                            cardView.BlSelected = true;
                }
                _listView.Add(cardView);
            }
        }
    }

    private void OnClear()
    {
        if (_listView!=null)
        {
            for (int i = 0; i < _listView.Count; i++)
                _listView[i].Dispose();
            _listView.Clear();
            _listView = null;
        }
    }

    private void OnClick(CardView item)
    {
        if (item.BlSelected) return;
        for (int i = 0; i < _lstSelnum.Count; i++) LogHelper.Log(_lstSelnum[i] + "已选择的id");
        if (_lstSel.Count < GameConfigMgr.Instance.GetSearchTaskConfig(_exploreDataVo.mTaskId).CardNum)
        {
            if (_lstSel.Contains(item.mCardDataVO)) return;
            _lstSel.Add(item.mCardDataVO);
            item.BlSelected = true;
            //Debuger.Log(_lstCardSelectVo.Count + "添加的数量");
            ExploreEventVO vo = new ExploreEventVO();
            vo.mCardDataVO = item.mCardDataVO;
            vo.mHeroCardID = _id;
            GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(ExploreEvent.ExploreHeroCard, vo); // item.mCardDataVO, _id);
            if (_id == GameConfigMgr.Instance.GetSearchTaskConfig(_exploreDataVo.mTaskId).CardNum - 1) _id = -1;
            _id++;
            for (int i = 0; i < _lstSelnum.Count; i++)
                if (_id == _lstSelnum[i])
                {
                    if (_lstSelnum[i] + 1 <= GameConfigMgr.Instance.GetSearchTaskConfig(_exploreDataVo.mTaskId).CardNum - 1)
                        _id = _lstSelnum[i] + 1;
                    else
                        _id = 0;
                }
        }
        else
        {
            LogHelper.Log("任务所需英雄数量已满");
        }
    }

    private void OnClose()
    {
        SetPicValue();
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(ExploreEvent.ExploreCloseBag);
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

    public override void Dispose()
    {
        OnClear();
        base.Dispose();
    }
}