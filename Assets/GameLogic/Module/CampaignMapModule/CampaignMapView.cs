using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;
using System;

public class CampaignMapView : UIBaseView
{
    enum CampaignChapterStates
    {
        None,
        Lock, 
        Passed,
        Battling,
        CanUnLock,
    }

#region campaign map item logic
    class CampaignMapItem : UIBaseView
    {
        private Action<int> _method;
        private int _chapterID;
        private Button _mapBtn;
        private SkeletonGraphic _mapAnimator;
        private SkeletonGraphic _lockAnimator;
        private SkeletonGraphic _battleAnimator;
        private CampaignChapterStates _chapterStates = CampaignChapterStates.None;
        private GrayComponent _gray;
        private GrayComponent _lockGray;


        public CampaignMapItem(Action<int> method, int chatperID)
        {
            _method = method;
            _chapterID = chatperID;
        }

        protected override void ParseComponent()
        {
            base.ParseComponent();
            _mapBtn = Find<Button>("map");
            _mapAnimator = Find<SkeletonGraphic>("map");
            _lockAnimator = Find<SkeletonGraphic>("jiesuo");
            _battleAnimator = Find<SkeletonGraphic>("duizhan");

            _gray = new GrayComponent(_mapAnimator);
            _lockGray = new GrayComponent(_lockAnimator);

            _mapBtn.onClick.Add(OnClick);
        }

        public void SetChapterState(CampaignChapterStates states)
        {
            if (_chapterStates == states)
                return;
            _chapterStates = states;
            _mapBtn.interactable = true;
            _battleAnimator.gameObject.SetActive(false);
            _lockAnimator.gameObject.SetActive(false);
            //_gray.SetNormal();
            _lockGray.SetNormal();
            switch (_chapterStates)
            {
                case CampaignChapterStates.Lock:
                    _mapBtn.interactable = false;
                    _mapAnimator.AnimationState.TimeScale = 0f;
                    //_gray.SetGray();
                    _lockAnimator.gameObject.SetActive(true);
                    _lockAnimator.AnimationState.TimeScale = 0f;
                    _lockGray.SetGray();
                    break;
                case CampaignChapterStates.Battling:
                    _mapAnimator.AnimationState.TimeScale = 1f;
                    _battleAnimator.gameObject.SetActive(true);
                    break;
                case CampaignChapterStates.CanUnLock:
                    _mapAnimator.AnimationState.TimeScale = 0f;
                    _lockAnimator.gameObject.SetActive(true);
                    if(_lockAnimator.AnimationState != null)
                    {
                        _lockAnimator.AnimationState.ClearTrack(0);
                        _lockAnimator.AnimationState.SetAnimation(0, "animation", false);
                        DelayCall(Time.deltaTime, () => { _lockAnimator.AnimationState.TimeScale = 0f; });
                    }

                    //_gray.SetGray();
                    break;
                case CampaignChapterStates.Passed:
                    _mapAnimator.AnimationState.TimeScale = 1f;
                    break;
            }
        }

        private void OnClick()
        {
            if (_chapterStates == CampaignChapterStates.Lock)
                return;
            Action OnUnlockEnd = () =>
            {
                _lockAnimator.gameObject.SetActive(false);
                SetChapterState(CampaignChapterStates.Battling);
                if (_method != null)
                    _method.Invoke(_chapterID);
                _mapBtn.enabled = true;
            };
            if(_chapterStates == CampaignChapterStates.CanUnLock)
            {
                _lockAnimator.AnimationState.TimeScale = 1f;
                _lockAnimator.AnimationState.SetAnimation(0, "animation", false);
                DelayCall(1.3f, OnUnlockEnd);
                _mapBtn.enabled = false;
            }
            else
            {
                if (_method != null)
                    _method.Invoke(_chapterID);
            }
        }

        public override void Dispose()
        {
            _method = null;
            _mapAnimator = null;
            _lockAnimator = null;
            _battleAnimator = null;
            if (_gray != null)
            {
                _gray.Dispose();
                _gray = null;
            }
            base.Dispose();
        }
    }
#endregion

    private Toggle _befaToggle;
    private Toggle _alphaToggle;
    private Toggle _omegaToggle;

    private List<CampaignMapItem> _lstMapItems;

    private ScrollRect _scrollRect;
    private Button _closeBtn;
    private int _curDiffulty;
    private UIEffectView _effect01;
    private UIEffectView _effect02;
    private UIEffectView _effect03;

    private GameObject _befaBGObject;
    private GameObject _alphaBGObject;
    private GameObject _omegaBGObject;

    private Text _textNormal;
    
    protected override void ParseComponent()
    {
        base.ParseComponent();

        _befaToggle = Find<Toggle>("ToggleGroup/befa");
        _alphaToggle = Find<Toggle>("ToggleGroup/alpha");
        _omegaToggle = Find<Toggle>("ToggleGroup/omega");
        _textNormal = Find<Text>("ToggleGroup/alpha/Label");

        _scrollRect = Find<ScrollRect>("ScrollView");
        //_imgBack = Find<Image>("ScrollView/Content/MapItemObject/Back01");

        _befaBGObject = Find("ScrollView/Content/MapItemObject/befaBGObject");
        _alphaBGObject = Find("ScrollView/Content/MapItemObject/alphaBGObject");
        _omegaBGObject = Find("ScrollView/Content/MapItemObject/omegaBGObject");

        _closeBtn = Find<Button>("BtnBack");

        ColliderHelper.SetButtonCollider(_closeBtn.transform);

        _effect01 = CreateUIEffect(Find("ToggleGroup/befa/fx_ui_zhanyitubiao"), UILayerSort.WindowSortBeginner);
        _effect02 = CreateUIEffect(Find("ToggleGroup/alpha/fx_ui_zhanyitubiao"), UILayerSort.WindowSortBeginner);
        _effect03 = CreateUIEffect(Find("ToggleGroup/omega/fx_ui_zhanyitubiao"), UILayerSort.WindowSortBeginner);



        _lstMapItems = new List<CampaignMapItem>();
        CampaignMapItem mapItem;
        GameObject obj;
        for (int i = 1; i <= 8; i++)
        {
            obj = Find("ScrollView/Content/MapItemObject/map" + i);

            mapItem = new CampaignMapItem(OnChapterClicked, i);
            mapItem.SetDisplayObject(obj);
            _lstMapItems.Add(mapItem);
        }
        _curDiffulty = HangupDataModel.Instance.CurHangupConfig.Difficulty;
        //_effect01.PlayEffect();
        _closeBtn.onClick.Add(delegate { GameUIMgr.Instance.CloseModule(ModuleID.CampaignMap); });
        if (_curDiffulty == 1)
        {
            _effect01.PlayEffect();
            _befaToggle.isOn = true;
        }
        else if(_curDiffulty==2)
        {
            _effect02.PlayEffect();
            _alphaToggle.isOn = true;
        }
        else
        {
            _effect03.PlayEffect();
            _omegaToggle.isOn = true;
        }


        _befaToggle.onValueChanged.Add((bool value) => { if (value) OnDiffiultChange(1); });
        _alphaToggle.onValueChanged.Add((bool value) => { if (value) OnDiffiultChange(2); });
        _omegaToggle.onValueChanged.Add((bool value) => { if (value) OnDiffiultChange(3); });
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _textNormal.text = LanguageMgr.GetLanguage(5002944);
        if (HangupDataModel.Instance.CurHangupConfig.ChapterMap < 3)
            _scrollRect.content.anchoredPosition = new Vector2(667f, 0f);
        else if (HangupDataModel.Instance.CurHangupConfig.ChapterMap < 4)
            _scrollRect.content.anchoredPosition = new Vector2(250f, 0f);
        else if (HangupDataModel.Instance.CurHangupConfig.ChapterMap < 5)
            _scrollRect.content.anchoredPosition = new Vector2(-100f, 0f);
        else if (HangupDataModel.Instance.CurHangupConfig.ChapterMap < 6)
            _scrollRect.content.anchoredPosition = new Vector2(-300f, 0f);
        else
            _scrollRect.content.anchoredPosition = new Vector2(-667f, 0f);
        if (_chapterMapLen == 0)
        {
            Dictionary<int, ChapterConfig> dict = ChapterConfig.Get();
            foreach (var kv in dict)
                _chapterMapLen += kv.Value.CampaignCount;
        }
        RefreshChapterStatus();
    }

    private int _chapterMapLen = 0;
    private bool ContainValue(int min, int max, int value)
    {
        return value >= min && value <= max;
    }
    private void RefreshChapterStatus()
    {
        int unlockId = HangupDataModel.Instance.mIntUnlockCampaignId;
        int hangupId = HangupDataModel.Instance.mIntHangupCampaignId;
        ChapterConfig chapterCfg;
        CampaignChapterStates states;
        int startId = (_curDiffulty - 1) * _chapterMapLen;
        int curMapStartId, curMapEndId;
        for (int i = 0; i < _lstMapItems.Count; i++)
        {
            chapterCfg = GameConfigMgr.Instance.GetChapterConfig(i + 1);
            curMapStartId = startId + 1;
            curMapEndId = startId + chapterCfg.CampaignCount;
            startId = curMapEndId;
            if(unlockId < curMapStartId)
            {
                states = CampaignChapterStates.Lock;
            }
            else
            {
                if(unlockId > curMapEndId)
                {
                    if (ContainValue(curMapStartId, curMapEndId, hangupId))
                        states = CampaignChapterStates.Battling;
                    else
                        states = CampaignChapterStates.Passed;
                }
                else
                {
                    if (ContainValue(curMapStartId, curMapEndId, hangupId))
                        states = CampaignChapterStates.Battling;
                    else
                        states = unlockId > curMapStartId ? CampaignChapterStates.Passed : CampaignChapterStates.CanUnLock;
                }
            }
            _lstMapItems[i].SetChapterState(states);
        }
    }

    private void OnChapterClicked(int chapterId)
    {
        //Debuger.Log("chapterId:" + chapterId);
        int id = _curDiffulty * 10000 + chapterId * 100 + 1;
        CampaignConfig cfg = GameConfigMgr.Instance.GetCampaignByClientId(id);
        if (cfg == null)
        {
            LogHelper.LogWarning("campaign client id:" + id + " was not found!!!");
            return;
        }
        GameUIMgr.Instance.CloseModule(ModuleID.CampaignMap);
        int hangupID = HangupDataModel.Instance.mIntHangupCampaignId;
        CampaignConfig hangupCfg = GameConfigMgr.Instance.GetCampaignByCampaignId(hangupID);
        if (hangupCfg.Difficulty == cfg.Difficulty && hangupCfg.ChapterMap == cfg.ChapterMap)
        {
            LogHelper.Log("hangup campaign chapter no change!!!");
            return;
        }
        else
        {
            HangupDataModel.Instance.ReqHangup(cfg.CampaignID);
        }
    }

    private void OnDiffiultChange(int difficulty)
    {
        _befaBGObject.SetActive(false);
        _alphaBGObject.SetActive(false);
        _omegaBGObject.SetActive(false);
        switch (difficulty)
        {
            case 1:
                _effect01.PlayEffect();
                _effect02.StopEffect();
                _effect03.StopEffect();
                _befaBGObject.SetActive(true);
                break;
            case 2:
                _effect01.StopEffect();
                _effect02.PlayEffect();
                _effect03.StopEffect();
                _alphaBGObject.SetActive(true);
                break;
            case 3:
                _effect01.StopEffect();
                _effect02.StopEffect();
                _effect03.PlayEffect();
                _omegaBGObject.SetActive(true);
                break;
        }
        _curDiffulty = difficulty;
        if (difficulty== HangupDataModel.Instance.CurHangupConfig.Difficulty)
        {
            if (HangupDataModel.Instance.CurHangupConfig.ChapterMap < 3)
                _scrollRect.content.anchoredPosition = new Vector2(667f, 0f);
            else if (HangupDataModel.Instance.CurHangupConfig.ChapterMap < 4)
                _scrollRect.content.anchoredPosition = new Vector2(250f, 0f);
            else if (HangupDataModel.Instance.CurHangupConfig.ChapterMap < 5)
                _scrollRect.content.anchoredPosition = new Vector2(-100f, 0f);
            else if (HangupDataModel.Instance.CurHangupConfig.ChapterMap < 6)
                _scrollRect.content.anchoredPosition = new Vector2(-300f, 0f);
            else
                _scrollRect.content.anchoredPosition = new Vector2(-667f, 0f);
        }
        else
        {
            _scrollRect.horizontalNormalizedPosition = 0f;
        }
        RefreshChapterStatus();
    }

    public override void Dispose()
    {
        if (_lstMapItems != null)
        {
            for (int i = 0; i < _lstMapItems.Count; i++)
                _lstMapItems[i].Dispose();
            _lstMapItems.Clear();
            _lstMapItems = null;
        }
        _scrollRect = null;
        base.Dispose();
    }
}