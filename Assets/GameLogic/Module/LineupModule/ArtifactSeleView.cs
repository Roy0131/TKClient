using Framework.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactSeleView : UIBaseView
{
    private Text _title;
    private Button _disBtn;
    private List<ArtifactSeleItemView> _listArtifactSeleItemView;
    private GameObject _selectItem;
    private RectTransform _Parent;

    private RectTransform _scrollRect;



    protected override void ParseComponent()
    {
        base.ParseComponent();
        _title = Find<Text>("Img/TextTitle");
        _disBtn = Find<Button>("Img/ButtonClose");
        _Parent = Find<RectTransform>("Img/ScrollView/Content");
        _selectItem = Find("Img/ScrollView/Content/Item");

        _scrollRect = Find<RectTransform>("Img/ScrollView");

        _disBtn.onClick.Add(Hide);
        ColliderHelper.SetButtonCollider(_disBtn.transform, 120, 120);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        ArtifactDataModel.Instance.AddEvent(ArtifactEvent.ArtifactData, OnArtifactData);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<ArtifactSeleItemView>(ArtifactEvent.ArtifactSelect, OnArtifactSelect);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        ArtifactDataModel.Instance.RemoveEvent(ArtifactEvent.ArtifactData, OnArtifactData);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<ArtifactSeleItemView>(ArtifactEvent.ArtifactSelect, OnArtifactSelect);
    }

    private void OnArtifactSelect(ArtifactSeleItemView view)
    {
        for (int i = 0; i < _listArtifactSeleItemView.Count; i++)
        {
            if (_listArtifactSeleItemView[i] == view)
                _listArtifactSeleItemView[i].BlSelected = true;
            else
                _listArtifactSeleItemView[i].BlSelected = false;
        }
    }

    private void OnArtifactData()
    {
        OnSelectitemClear();
        _listArtifactSeleItemView = new List<ArtifactSeleItemView>();
        List<ArtifactDataVO> listArtifactVO = new List<ArtifactDataVO>();
        for (int i = 0; i < ArtifactDataModel.Instance.mListArtifactVO.Count; i++)
            listArtifactVO.Add(ArtifactDataModel.Instance.mListArtifactVO[i]);
        listArtifactVO.Sort(OnArtifactVO);
        for (int i = 0; i < listArtifactVO.Count; i++)
        {
            GameObject obj = GameObject.Instantiate(_selectItem);
            obj.transform.SetParent(_Parent, false);
            ArtifactSeleItemView seleItemView = new ArtifactSeleItemView();
            seleItemView.mScrollRect = _scrollRect;
            seleItemView.SetDisplayObject(obj);
            seleItemView.Show(listArtifactVO[i]);
            if (listArtifactVO[i].mArtifactData.Id == LocalDataMgr.GetArtifactSele(LineupSceneMgr.Instance.mLineupTeamType))
                seleItemView.BlSelected = true;
            else
                seleItemView.BlSelected = false;
            _listArtifactSeleItemView.Add(seleItemView);
        }
    }

    private int OnArtifactVO(ArtifactDataVO v1, ArtifactDataVO v2)
    {
        return v1.mArtifactData.Level > v2.mArtifactData.Level ? -1 : 1;
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _title.text = LanguageMgr.GetLanguage(400001);
        ArtifactDataModel.Instance.ReqArtifactData();
    }

    private void OnSelectitemClear()
    {
        if (_listArtifactSeleItemView != null)
        {
            for (int i = 0; i < _listArtifactSeleItemView.Count; i++)
                _listArtifactSeleItemView[i].Dispose();
            _listArtifactSeleItemView.Clear();
            _listArtifactSeleItemView = null;
        }
    }

    public override void Hide()
    {
        _Parent.anchoredPosition = new Vector2(0f, 0f);
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(ArtifactEvent.ArtifactEffect);
        base.Hide();
    }

    public override void Dispose()
    {
        OnSelectitemClear();
        base.Dispose();
    }
}
