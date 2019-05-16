using UnityEngine.UI;
using UnityEngine;
using Msg.ClientMessage;
using System.Collections.Generic;

public class CTowerVideoModule : ModuleBase
{
    private Text _noFightRecord;
    private Button _btnClose;
    private Button _btnBlackClose;
    private GameObject _itemParent;

    private List<TowerFightRecord> _lstFightRecordDataVo = new List<TowerFightRecord>();
    private List<CTowerVideoView> _cTowerVideoViews;
    private IList<TowerFightRecord> lstFightRecords;

    public CTowerVideoModule() : base(ModuleID.TowerVideo, UILayer.Popup)
    {
        _modelResName = UIModuleResName.UI_TowerVideo;
    }
    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        CTowerDataModel.Instance.ReqTowerRecordInfoData();
    }
    protected override void ParseComponent()
    {
        base.ParseComponent();
        _noFightRecord = Find<Text>("Text");
        _btnClose = Find<Button>("ButtonClose");
        ColliderHelper.SetButtonCollider(_btnClose.transform);
        _btnBlackClose = Find<Button>("BlackBack");

        _itemParent = Find("VideoGrid");

        _btnBlackClose.onClick.Add(OnClose);
        _btnClose.onClick.Add(OnClose);

        _noFightRecord.text = LanguageMgr.GetLanguage(5001911);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        CTowerDataModel.Instance.AddEvent(CTowerEvent.RefreshTowerRecordsInfoData, RefreshData);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        CTowerDataModel.Instance.RemoveEvent(CTowerEvent.RefreshTowerRecordsInfoData, RefreshData);
    }
    private void RefreshData()
    {
        OnTowerVideoViews();
        _cTowerVideoViews = new List<CTowerVideoView>();
        _lstFightRecordDataVo = CTowerDataModel.Instance.lstFIghtRecordDataVo;
        _noFightRecord.gameObject.SetActive(_lstFightRecordDataVo.Count == 0);
        if (_lstFightRecordDataVo.Count == 0)
            return;
        for (int i = 0; i < _lstFightRecordDataVo.Count; i++)
        {
            GameObject item = GameObject.Instantiate(Find("VideoItem"));
            item.transform.SetParent(_itemParent.transform, false);

            CTowerVideoView _ctowerVideoView = new CTowerVideoView();
            _ctowerVideoView.SetDisplayObject(item);
            _ctowerVideoView.Show(_lstFightRecordDataVo[i]);
            _cTowerVideoViews.Add(_ctowerVideoView);
        }
    }

    private void OnTowerVideoViews()
    {
        if (_cTowerVideoViews != null)
        {
            for (int i = 0; i < _cTowerVideoViews.Count; i++)
                _cTowerVideoViews[i].Dispose();
            _cTowerVideoViews.Clear();
            _cTowerVideoViews = null;
        }
    }

    public override void Dispose()
    {
        OnTowerVideoViews();
        base.Dispose();
    }
}
