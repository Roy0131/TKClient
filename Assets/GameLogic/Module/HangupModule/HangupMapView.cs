using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Framework.UI;

public class HangupMapView : UIBaseView
{
    private Queue<ChapterMapView> _chapterMapPools = new Queue<ChapterMapView>();
    private GameObject _mapItemObject;

    private RectTransform _mapItemContainer;
    private int _curChapterID = 0;
    private int _curDifficulty = 0;

    private List<ChapterMapView> _lstChatperMaps;
    private VerticalLayoutGroup _vGroup;

    private Button _mapBtn;
    private GameObject _redObj;
    private GameObject _speciallyObj;
    private UIEffectView _effect;



    protected override void ParseComponent()
    {
        base.ParseComponent();
        _mapItemObject = Find("ScrollView/Content/MapItemContainer/MapItemObject");
        _mapItemContainer = Find<RectTransform>("ScrollView/Content/MapItemContainer");
        _vGroup = Find<VerticalLayoutGroup>("ScrollView/Content");

        _mapBtn = Find<Button>("MapButton");
        _mapBtn.onClick.Add(OnShowMap);
        _redObj = Find("MapButton/RedPoint");
        _speciallyObj = Find("MapButton/fx_ui_daditu");
        _effect = CreateUIEffect(_speciallyObj, UILayerSort.WindowSortBeginner);

        _lstChatperMaps = new List<ChapterMapView>();
    }

    private void OnShowMap()
    {
        GameUIMgr.Instance.OpenModule(ModuleID.CampaignMap);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        HangupDataModel.Instance.AddEvent(HangupEvent.CampaignDataChange, OnRefreshMap);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        HangupDataModel.Instance.RemoveEvent(HangupEvent.CampaignDataChange, OnRefreshMap);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        OnRefreshMap();
    }

    private void OnRefreshMap()
    {
        if (HangupDataModel.Instance.CurUnlockConfig != null)
        {
            if (HangupDataModel.Instance.mIntUnlockCampaignId > 1 && HangupDataModel.Instance.CurUnlockConfig.ChildMapID == 1 &&
                HangupDataModel.Instance.mIntHangupCampaignId != HangupDataModel.Instance.mIntUnlockCampaignId)
            {
                _redObj.SetActive(true);
                _effect.PlayEffect();
            }
            else
            {
                _redObj.SetActive(false);
                _effect.StopEffect();
            }
        }
        else
        {
            _redObj.SetActive(false);
            _effect.StopEffect();
        }

        List<CampaignConfig> lstCampaignDatas = HangupDataModel.Instance.mlstCurCampaign;
        int i = 0, index;
        float totalHeight = 0f, per = 0f;
        if (_curChapterID == HangupDataModel.Instance.CurHangupConfig.ChapterMap && _curDifficulty == HangupDataModel.Instance.CurHangupConfig.Difficulty)
        {
            totalHeight = 0f;
            for (i = 0; i < _lstChatperMaps.Count; i++)
            {
                _lstChatperMaps[i].RefreshMapItemState();
                totalHeight += _lstChatperMaps[i].Height;
            }
            index = lstCampaignDatas.IndexOf(HangupDataModel.Instance.CurHangupConfig);
            per = ((float)index / (float)lstCampaignDatas.Count) * (totalHeight - 100f);
            _mapItemContainer.sizeDelta = new Vector2(300f, totalHeight);
            (_vGroup.transform as RectTransform).anchoredPosition = new Vector2(0, -per + 40f);
            return;
        }
        NewBieGuide.NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieGuide.NewBieMaskID.HangupChapterBtn);
        NewBieGuide.NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieGuide.NewBieMaskID.HangupChapterBtn2);
        _curChapterID = HangupDataModel.Instance.CurHangupConfig.ChapterMap;
        _curDifficulty = HangupDataModel.Instance.CurHangupConfig.Difficulty;
        ClearShowChapterMap();

        float tmp = (float)lstCampaignDatas.Count / 6f;
        int page = Mathf.CeilToInt(tmp);
        List<CampaignConfig> datas;
        int len = 6;
        float lastHeight = 470f;
        _vGroup.enabled = false;
        for (i = 0; i < page; i++)
        {
            if (len > lstCampaignDatas.Count - i * 6)
                len = lstCampaignDatas.Count - i * 6;
            datas = new List<CampaignConfig>();
            datas.AddRange(lstCampaignDatas.GetRange(i * 6, len));
            ChapterMapView view = GetChapterMapView();
            view.mRectTransform.SetParent(_mapItemContainer, false);
            view.mRectTransform.SetAsFirstSibling();
            view.mRectTransform.anchoredPosition = new Vector2(0f, i * lastHeight);
            view.Show(datas);
            lastHeight = view.Height;
            totalHeight += view.Height;
            _lstChatperMaps.Add(view);
            if(i == 0)
            {
                NewBieGuide.NewBieGuideMgr.Instance.RegistMaskTransform(NewBieGuide.NewBieMaskID.HangupChapterBtn, view.GetGuideButtonTransform(1));
                NewBieGuide.NewBieGuideMgr.Instance.RegistMaskTransform(NewBieGuide.NewBieMaskID.HangupChapterBtn2, view.GetGuideButtonTransform(2));
            }
        }
        index = lstCampaignDatas.IndexOf(HangupDataModel.Instance.CurHangupConfig);
        per = ((float)index / (float)lstCampaignDatas.Count) * (totalHeight - 100f);
        _mapItemContainer.sizeDelta = new Vector2(300f, totalHeight);
        (_vGroup.transform as RectTransform).anchoredPosition = new Vector2(0, -per + 40f);
        _vGroup.enabled = true;
    }

    private ChapterMapView GetChapterMapView()
    {
        if (_chapterMapPools.Count > 0)
            return _chapterMapPools.Dequeue();
        GameObject itemObj = GameObject.Instantiate(_mapItemObject);
        ChapterMapView view = new ChapterMapView();
        view.SetDisplayObject(itemObj);
        return view;
    }

    private void ReturnChapterMapView(ChapterMapView  view)
    {
        view.Hide();
        ObjectHelper.AddChildToParent(view.mTransform, mTransform);
        _chapterMapPools.Enqueue(view);
    }

    private void ClearShowChapterMap()
    {
        if (_lstChatperMaps == null)
            return;
        for (int i = 0; i < _lstChatperMaps.Count; i++)
            ReturnChapterMapView(_lstChatperMaps[i]);
        _lstChatperMaps.Clear();
    }

    public override void Dispose()
    {
        ClearShowChapterMap();
        if(_chapterMapPools != null)
        {
            while (_chapterMapPools.Count > 0)
                _chapterMapPools.Dequeue().Dispose();
            _chapterMapPools.Clear();
            _chapterMapPools = null;
        }
        _lstChatperMaps = null;
        base.Dispose();
    }
}