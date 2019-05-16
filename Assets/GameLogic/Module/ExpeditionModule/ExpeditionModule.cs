using Framework.UI;
using Msg.ClientMessage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpeditionModule : ModuleBase
{
    private Text _expeditionTime;
    private Button _closeBtn;
    private Button _help;
    private Button _expeditionShop;
    private Text _purifyPointNum;
    private Image _purifyPointImg;
    private Button _purifyPointBtn;
    private GameObject _speciallyObj;
    private UIEffectView _effect;

    private ExpeditionView _expeditionView;
    private List<CheckpointItemView> _listItemView;
    private List<ExpeditionConfig> _listConfig;

    private uint _timer = 0;
    private int _time = 0;

    public ExpeditionModule()
        : base(ModuleID.Expedition, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_Expedition;
        mBlNeedBackMask = true;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _expeditionTime = Find<Text>("Root/ExpeditionTime/Text");
        _purifyPointNum = Find<Text>("Root/Img/Text");
        _purifyPointImg = Find<Image>("Root/Img/ImgFilled");
        _purifyPointBtn = Find<Button>("Root/Img/RecrBtn");
        _closeBtn = Find<Button>("Root/BtnBack");
        ColliderHelper.SetButtonCollider(_closeBtn.transform, 120, 120);
        _help = Find<Button>("Root/BtnHelp");
        ColliderHelper.SetButtonCollider(_help.transform);
        _expeditionShop = Find<Button>("Root/BtnShop");

        _speciallyObj = Find("Root/Img/fx_ui_huangguangtiao");
        _effect = CreateUIEffect(_speciallyObj, UILayerSort.WindowSortBeginner);

        _listConfig = new List<ExpeditionConfig>();
        _listItemView = new List<CheckpointItemView>();
        foreach (ExpeditionConfig cfg in ExpeditionConfig.Get().Values)
        {
            CheckpointItemView checkpointItemView = new CheckpointItemView();
            checkpointItemView.SetDisplayObject(Find("CheckpointItem/Checkpoint" + cfg.ID));
            _listConfig.Add(cfg);
            _listItemView.Add(checkpointItemView);
        }

        _expeditionView = new ExpeditionView();
        _expeditionView.SetDisplayObject(Find("Lineup"));
        _expeditionView.mTransform.gameObject.SetActive(false);

        _purifyPointBtn.onClick.Add(OnPurifyPointReward);
        _closeBtn.onClick.Add(OnClose);
        _help.onClick.Add(OnHelp);
        _expeditionShop.onClick.Add(OnExpeditionShop);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        ExpeditionDataModel.Instance.AddEvent(ExpeditionEvent.ExpeditionData, OnExpeditionData);
        ExpeditionDataModel.Instance.AddEvent(ExpeditionEvent.ExpeditionPurifyPoint, OnPurifyPoint);
        ExpeditionDataModel.Instance.AddEvent<List<int>>(ExpeditionEvent.ExpeditionReward, OnExpeditionReward);
        ExpeditionDataModel.Instance.AddEvent<ExoeditionDataVO>(ExpeditionEvent.ExpeditionStageData, OnExpeditionStageData);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(ExpeditionEvent.OpenShop, OnExpeditionShop);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        ExpeditionDataModel.Instance.RemoveEvent(ExpeditionEvent.ExpeditionData, OnExpeditionData);
        ExpeditionDataModel.Instance.RemoveEvent(ExpeditionEvent.ExpeditionPurifyPoint, OnPurifyPoint);
        ExpeditionDataModel.Instance.RemoveEvent<List<int>>(ExpeditionEvent.ExpeditionReward, OnExpeditionReward);
        ExpeditionDataModel.Instance.RemoveEvent<ExoeditionDataVO>(ExpeditionEvent.ExpeditionStageData, OnExpeditionStageData);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(ExpeditionEvent.OpenShop, OnExpeditionShop);
    }

    private void OnExpeditionStageData(ExoeditionDataVO vo)
    {
        _expeditionView.Show(vo);
    }

    private void OnExpeditionReward(List<int> listId)
    {
        List<ItemInfo> _listItemInfo = new List<ItemInfo>();
        if (listId.Count > 0 && listId.Count % 2 == 0)
        {
            ItemInfo _itemInfo;
            for (int i = 0; i < listId.Count; i += 2)
            {
                _itemInfo = new ItemInfo();
                _itemInfo.Id = listId[i];
                _itemInfo.Value = listId[i + 1];
                _listItemInfo.Add(_itemInfo);
            }
            GetItemTipMgr.Instance.ShowItemResult(_listItemInfo);
        }
    }

    private void OnExpeditionData()
    {
        _time = ExpeditionDataModel.Instance.ExpeditionTime;
        if (_timer != 0)
            TimerHeap.DelTimer(_timer);
        int interval = 1000;
        _timer = TimerHeap.AddTimer(0, interval, OnAddTime);
        OnPurifyPoint();
        for (int i = 0; i < _listItemView.Count; i++)
        {
            if (i > ExpeditionDataModel.Instance.mCurStage)
                _listItemView[i].Hide();
            else
                _listItemView[i].Show(_listConfig[i]);
        }
    }

    private void OnAddTime()
    {
        _time -= 1;
        if (_time > 0)
            _expeditionTime.text = LanguageMgr.GetLanguage(5003402, TimeHelper.GetCountTime(_time));
        else if (_time == 0)
            GameNetMgr.Instance.mGameServer.ReqExpeditionData();
    }

    private void OnPurifyPoint()
    {
        _purifyPointNum.text = ExpeditionDataModel.Instance.mPurifyPoint + "/" + GameConst.PurifyPointReward;
        _purifyPointImg.fillAmount = (float)ExpeditionDataModel.Instance.mPurifyPoint / (float)GameConst.PurifyPointReward;
        if (ExpeditionDataModel.Instance.mPurifyPoint < GameConst.PurifyPointReward)
            _effect.StopEffect();
        else
            _effect.PlayEffect();
    }

    private void OnPurifyPointReward()
    {
        if (ExpeditionDataModel.Instance.mPurifyPoint < GameConst.PurifyPointReward)
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(5003403));
        else
            GameNetMgr.Instance.mGameServer.ReqExpeditionReward();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        ExpeditionDataModel.Instance.ReqExpeditionData();
    }

    private void OnHelp()
    {
        HelpTipsMgr.Instance.ShowTIps(HelpType.ExpeditionHelp);
    }

    private void OnExpeditionShop()
    {
        GameUIMgr.Instance.OpenModule(ModuleID.HeroShop, ShopIdConst.EXPEDITIONSHOP);
    }

    public override void Hide()
    {
        if (_expeditionView != null)
            _expeditionView.Hide();
        base.Hide();
    }

    public override void Dispose()
    {
        if (_timer != 0)
            TimerHeap.DelTimer(_timer);
        if (_expeditionView!=null)
        {
            _expeditionView.Dispose();
            _expeditionView = null;
        }
        base.Dispose();
    }
}
