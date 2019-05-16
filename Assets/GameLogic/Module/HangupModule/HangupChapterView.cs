using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Framework.UI;

public class ChapterMapView : UIBaseView
{
    private Image _mapItemImg;
    private List<ChapterChildView> _lstMapItems;

    public ChapterMapView()
    {
        Height = 470f;
        _lstMapItems = new List<ChapterChildView>();
    }

    public Transform GetGuideButtonTransform(int idx)
    {
        return _lstMapItems[idx].GetGuideButtonTransform();
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _mapItemImg = FindOnSelf<Image>();

        ChapterChildView view;
        for (int i = 0; i < 6; i++)
        {
            view = new ChapterChildView();
            view.SetDisplayObject(Find("ChapterItem" + i));
            view.Hide();
            _lstMapItems.Add(view);
        }
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        
        List<CampaignConfig> datas = args[0] as List<CampaignConfig>;
        int i;
        for (i = 0; i < datas.Count; i++)
            _lstMapItems[i].Show(datas[i]);

        for (i = datas.Count; i < _lstMapItems.Count; i++)
            _lstMapItems[i].Hide();
        Height = datas.Count * 72f + 100f;
        Height = Height > 470f ? 470f : Height;
        _mapItemImg.sprite = GameResMgr.Instance.LoadItemIcon("levelicon/panel_map_0" + HangupDataModel.Instance.CurHangupConfig.ChapterMap);
    }

    public override void Dispose()
    {
        if(_lstMapItems != null)
        {
            for (int i = 0; i < _lstMapItems.Count; i++)
                _lstMapItems[i].Dispose();
            _lstMapItems.Clear();
            _lstMapItems = null;
        }
        base.Dispose();
    }

    public void RefreshMapItemState()
    {
        if (_lstMapItems == null)
            return;
        for (int i = 0; i < _lstMapItems.Count; i++)
            _lstMapItems[i].RefreshChatperStatus();
    }

    public float Height
    {
        get;
        set;
    }
}

public class ChapterChildView : UIBaseView
{
    private Text _chapterLabel;
    private GameObject _lockedObject;
    private GameObject _passedObject;
    private GameObject _hangupObject;
    private GameObject _unLockObject;
    private GameObject _routeObject1;
    private GameObject _routeObject2;

    private CampaignConfig _data;
    private CampaignStageStates _status;
    ChapterConfig cfg;
    private Button _itemButton;
    protected override void ParseComponent()
    {
        base.ParseComponent();
        _chapterLabel = Find<Text>("ChapterLabel");
        _lockedObject = Find("LockObject");
        _passedObject = Find("PassedObject");
        _hangupObject = Find("HangupObject");
        _unLockObject = Find("UnLockObject");
        _routeObject1 = Find("RouteObject1");
        _routeObject2 = Find("RouteObject2");


        _itemButton = mDisplayObject.GetComponent<Button>();

        _itemButton.onClick.Add(OnClick);
        mBlShow = true;
    }

    public Transform GetGuideButtonTransform()
    {
        return _itemButton.transform;
    }

    private void OnClick()
    {
        switch (_status)
        {
            case CampaignStageStates.Lock:
                LogHelper.LogWarning("Chapter was Locked!!!");
                break;
            case CampaignStageStates.Hangup:
                LogHelper.LogWarning("chapter was hang up..");
                break;
            case CampaignStageStates.Passed:
                LogHelper.LogWarning("chapter was passed..");
                //HangupDataModel.Instance.ReqHangup(_data.CampaignID);
                GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(HangupEvent.MapSetHangupStage, _data.CampaignID);
                break;
            case CampaignStageStates.Unlock:
                //HangupDataModel.Instance.ReqHangup(_data.CampaignID);
                GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(HangupEvent.MapSetHangupStage, _data.CampaignID);
                break;
        }
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _data = args[0] as CampaignConfig;
        cfg = GameConfigMgr.Instance.GetChapterConfig(_data.ChapterMap);
        _chapterLabel.text = ((_data.Difficulty - 1) * 8 + _data.ChapterMap + "-" + _data.ChildMapID);
        RefreshChatperStatus();
    }

    public override void Hide()
    {
        base.Hide();
        _data = null;
    }

    public void RefreshChatperStatus()
    {
        if (_data == null)
            return;
        _lockedObject.SetActive(false);
        _passedObject.SetActive(false);
        _hangupObject.SetActive(false);
        _unLockObject.SetActive(false);

        _routeObject1.SetActive(_data.CampaignID < HangupDataModel.Instance.mIntUnlockCampaignId && _data.ChildMapID < cfg.CampaignCount);
        _routeObject2.SetActive(_data.CampaignID >= HangupDataModel.Instance.mIntUnlockCampaignId && _data.ChildMapID < cfg.CampaignCount);
        _status = HangupDataModel.Instance.CheckCampaignStatus(_data);
        switch (_status)
        {
            case CampaignStageStates.Hangup:
                _hangupObject.SetActive(true);
                break;
            case CampaignStageStates.Lock:
                _lockedObject.SetActive(true);
                break;
            case CampaignStageStates.Passed:
                _passedObject.SetActive(true);
                break;
            case CampaignStageStates.Unlock:
                _unLockObject.SetActive(true);
                break;
        }
    }

    public override void Dispose()
    {
        _itemButton = null;
        _data = null;
        base.Dispose();
    }
}