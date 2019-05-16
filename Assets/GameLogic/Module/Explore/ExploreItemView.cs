using Framework.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExploreItemView : UIBaseView
{
    private Image _imgBack;
    private Image _imgBackSquare;
    private Image _imgBackCol;
    private GameObject _imgStar;
    private GameObject _starGrid;
    private Toggle _btnLock;
    private ItemView _view;
    private RectTransform _parent;
    private Text _textTitle;
    private GameObject _consumeObj;
    private Text _consumeText;





    private GameObject _rewardItem;
    private GameObject _gridReward;


    private int _remainTime;
    private Button _btnExplore;
    public ExploreDataVO mExploreDataVO { get; private set; }
    private Text _textTime;
    private Text _textname;

    private uint _timerKey = 0;
    //private SearchTaskConfig _searchTaskCfg;
    private ItemView _itemView;

    private Dictionary<ExploreData, bool> _dictExploreIsOrNot = new Dictionary<ExploreData, bool>();
    private bool _isStory = true;
    private List<ItemInfo> _RewardItemInfo;

    private int _state;
    private Button _btnBroadcast;

    private Dictionary<int, int> _dictRandomRewards;

    private UIEffectView _effect01;

    private GameObject objStart;
    private Text objStartText;
    private GameObject objStartImg;
    private GameObject objOverImg;
    private Text textBtnStart;
    private Text textBtnSpeed;
    private GameObject objGetReward;
    private GameObject objStory;
    private GameObject objGemSpeed;

    private bool isLock = false;

    private bool isStart = false;

    private uint _timer = 0;
    private int _taskTime = 0;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        objStartText = Find<Text>("ImageUnStart/Text");
        _textTime = Find<Text>("ImageStart/Time");
        _textname = Find<Text>("ImageStart/Text");
        _textTitle = Find<Text>("TextTitle");
        textBtnStart = Find<Text>("ButtonStart/TextStartExplore");
        textBtnSpeed = Find<Text>("ButtonStart/TextSpeedExplore");
        _consumeText = Find<Text>("ButtonStart/TextSpeedExplore/Image/Text");
        _imgBack = Find<Image>("ImageBack01");
        _imgBackSquare = Find<Image>("ImageBack");
        _imgBackCol = Find<Image>("ImageBack02");
        _btnLock = Find<Toggle>("ButtonLock");
        _parent = Find<RectTransform>("GridReward");
        _btnExplore = Find<Button>("ButtonStart");
        _btnBroadcast = Find<Button>("ImageBack");
        _imgStar = Find("ImageStar");
        _starGrid = Find("GridStar");
        _gridReward = Find("GridReward");
        objStart = Find("ImageUnStart");
        objStartImg = Find("ImageStart");
        _consumeObj = Find("ButtonStart/TextSpeedExplore/Image");

        _btnLock.onValueChanged.Add(LockChange);
        _btnExplore.onClick.Add(ExploreStart);
        _btnBroadcast.onClick.Add(Broadcast);

        _effect01 = CreateUIEffect(Find("fx_ui_tansuo01"), UILayerSort.WindowSortBeginner);
    }

    private void OnAddTime()
    {
        _taskTime -= 1;
        if (_taskTime > 0)
        {
            _textTime.text = TimeHelper.GetCountTime(_taskTime);
        }
        else if (_taskTime == 0)
        {
            mExploreDataVO.OnState(2);
            SetStateData(mExploreDataVO.mState);
        }
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        ExploreDataModel.Instance.AddEvent<List<int>>(ExploreEvent.ExploreStart, OnExploreStart);
        ExploreDataModel.Instance.AddEvent<List<int>>(ExploreEvent.ExploreSpeedup, OnExploreSpeedup);
        ExploreDataModel.Instance.AddEvent<List<ItemInfo>, int, int>(ExploreEvent.ExploreGetReward, OnReward);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        ExploreDataModel.Instance.RemoveEvent<List<int>>(ExploreEvent.ExploreStart, OnExploreStart);
        ExploreDataModel.Instance.RemoveEvent<List<int>>(ExploreEvent.ExploreSpeedup, OnExploreSpeedup);
        ExploreDataModel.Instance.RemoveEvent<List<ItemInfo>, int, int>(ExploreEvent.ExploreGetReward, OnReward);
    }

    private void OnReward(List<ItemInfo> listInfo, int id, int stageId)
    {
        if (mExploreDataVO.mId == id)
        {
            if (stageId > 0)
                SetStateData(mExploreDataVO.mState);
        }
    }

    private void OnExploreSpeedup(List<int> listId)
    {
        for (int i = 0; i < listId.Count; i++)
        {
            if (listId[i] == mExploreDataVO.mId)
                SetStateData(mExploreDataVO.mState);
        }
    }

    private void OnExploreStart(List<int> listId)
    {
        for (int i = 0; i < listId.Count; i++)
        {
            if (listId[i] == mExploreDataVO.mId)
            {
                isStart = true;
                SetStateData(mExploreDataVO.mState);
            }
        }
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        mExploreDataVO = args[0] as ExploreDataVO;
        _isStory = bool.Parse(args[1].ToString());
        InitData();
        isLock = true;
    }

    private void InitData()
    {
        switch (mExploreDataVO.mExploreTaskCfg.TaskStar)
        {
            case 1:
                _imgBackCol.color = new Color(0.44f, 0.44f, 0.44f, 1.0f);
                break;
            case 2:
                _imgBackCol.color = new Color(0.14f, 0.49f, 0.10f, 1.0f);
                break;
            case 3:
                _imgBackCol.color = new Color(0.23f, 0.25f, 0.56f, 1.0f);
                break;
            case 4:
                _imgBackCol.color = new Color(0.58f, 0.47f, 0.13f, 1.0f);
                break;
            case 5:
                _imgBackCol.color = new Color(0.28f, 0.15f, 0.55f, 1.0f);
                break;
            case 6:
                _imgBackCol.color = new Color(0.58f, 0.20f, 0.13f, 1.0f);
                break;
            case 7:
                _imgBackCol.color = new Color(0.60f, 0.32f, 0.04f, 1.0f);
                break;
        }
        if (mExploreDataVO.mExploreTaskCfg.TaskStar <= 3)
            _imgBack.sprite = GameResMgr.Instance.LoadItemIcon("levelicon/panel_chatu_01");
        else
            _imgBack.sprite = GameResMgr.Instance.LoadItemIcon("levelicon/panel_chatu_0" + (mExploreDataVO.mExploreTaskCfg.TaskStar - 2));
        _imgBackSquare.sprite = GameResMgr.Instance.LoadItemIcon("levelicon/icon_tansuodengjik_0" + mExploreDataVO.mExploreTaskCfg.TaskStar);
        if (_starGrid.transform.childCount != 0)
        {
            for (int i = 0; i < _starGrid.transform.childCount; i++)
                GameObject.Destroy(_starGrid.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < mExploreDataVO.mExploreTaskCfg.TaskStar; i++)
        {
            GameObject obj = GameObject.Instantiate(_imgStar);
            obj.SetActive(true);
            obj.transform.SetParent(_starGrid.transform, false);
        }
        if (!_isStory)
        {
            string _titleRole = LanguageMgr.GetLanguage(mExploreDataVO.mRoleId4Title);
            string _titleName = LanguageMgr.GetLanguage(mExploreDataVO.mNameId4Title);
            string.Format(_titleName, _titleRole);
            _textTitle.text = string.Format(_titleName, _titleRole);
        }
        else
        {
            _textTitle.text = mExploreDataVO.mTaskName;
        }
        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        List<ItemInfo> listInfo = new List<ItemInfo>();
        ItemInfo info;
        if (_isStory)
        {
            if (mExploreDataVO.mConstReward != null && mExploreDataVO.mConstReward.Count % 2 == 0)
            {
                for (int i = 0; i < mExploreDataVO.mConstReward.Count; i += 2)
                {
                    info = new ItemInfo();
                    info.Id = mExploreDataVO.mConstReward[i];
                    info.Value = mExploreDataVO.mConstReward[i + 1];
                    listInfo.Add(info);
                }
            }
        }
        else
        {
            if (mExploreDataVO.mRandomRewards != null && mExploreDataVO.mRandomRewards.Count % 2 == 0)
            {
                for (int i = 0; i < mExploreDataVO.mRandomRewards.Count; i += 2)
                {
                    info = new ItemInfo();
                    info.Id = mExploreDataVO.mRandomRewards[i];
                    info.Value = mExploreDataVO.mRandomRewards[i + 1];
                    listInfo.Add(info);
                }
            }
        }
        for (int i = 0; i < listInfo.Count; i++)
        {
            if (GameConfigMgr.Instance.GetItemConfig(listInfo[i].Id).ItemType == 2)
                _view = ItemFactory.Instance.CreateItemView(listInfo[i], ItemViewType.EquipHeroItem);
            else
                _view = ItemFactory.Instance.CreateItemView(listInfo[i], ItemViewType.HeroItem);
            _view.mRectTransform.SetParent(_parent, false);
        }
        SetStateData(mExploreDataVO.mState);
    }

    private void SetStateData(int state)
    {
        if (mExploreDataVO.mExploreTaskCfg.Type == 1 && mExploreDataVO.mState == 0)
            _btnLock.isOn = mExploreDataVO.mIsLock;
        else
            _btnLock.isOn = true;
        objStart.SetActive(state == 0);
        objStartImg.SetActive(state != 0);
        textBtnStart.gameObject.SetActive(state != 1);
        textBtnSpeed.gameObject.SetActive(state == 1);
        _textTime.gameObject.SetActive(state == 1);
        _textname.gameObject.SetActive(state != 1);
        switch (state)
        {
            case 0:
                _effect01.StopEffect();
                objStartText.text = TimeHelper.GetCountTime(mExploreDataVO.mRemainSeconds);
                textBtnStart.text = LanguageMgr.GetLanguage(5002202);
                _btnExplore.interactable = true;
                break;
            case 1:
                _effect01.StopEffect();
                _taskTime = mExploreDataVO.RemainSeconds;
                if (_timer != 0)
                    TimerHeap.DelTimer(_timer);
                int interval = 1000;
                _timer = TimerHeap.AddTimer(0, interval, OnAddTime);
                textBtnSpeed.text = LanguageMgr.GetLanguage(5002203);
                _consumeObj.SetActive(mExploreDataVO.mExploreTaskCfg.AccelCost > 0);
                _consumeText.text = mExploreDataVO.mExploreTaskCfg.AccelCost.ToString();
                if (mExploreDataVO.mExploreTaskCfg.Type == 1)
                    _btnExplore.interactable = true;
                else
                    _btnExplore.interactable = false;
                break;
            case 2:
                _effect01.PlayEffect();
                _textname.text = LanguageMgr.GetLanguage(5002935);
                textBtnStart.text = LanguageMgr.GetLanguage(5002204);
                _btnExplore.interactable = true;
                break;
            case 3:
                _effect01.PlayEffect();
                _textname.text = LanguageMgr.GetLanguage(5002935);
                textBtnStart.text = LanguageMgr.GetLanguage(5002904);
                _btnExplore.interactable = true;
                break;
        }
    }

    private void Broadcast()
    {
        if (mExploreDataVO.mState == 1)
            GameUIMgr.Instance.OpenModule(ModuleID.ExploreBroadcast, mExploreDataVO, _isStory, _remainTime);
    }

    /// <summary>
    /// 点击探索按钮
    /// </summary>
    private void ExploreStart()
    {
        switch (mExploreDataVO.mState)
        {
            case 0:
                GameUIMgr.Instance.OpenModule(ModuleID.ExploreGroup, mExploreDataVO, _isStory);
                break;
            case 1:
                if (!_isStory)
                    ConfirmTipsMgr.Instance.ShowConfirmTips(LanguageMgr.GetLanguage(6001272, mExploreDataVO.mExploreTaskCfg.AccelCost), AlertBack);
                break;
            case 2:
                GameNetMgr.Instance.mGameServer.ReqExploreGetReward(mExploreDataVO.mId, _isStory);
                break;
            case 3:
                ExploreDataModel.Instance.RefreshStageId(mExploreDataVO.mRewardStageId);

                if (!_isStory)
                    LineupSceneMgr.Instance.ShowLineupModule(TeamType.ExploreTask, mExploreDataVO.mId);
                else
                    LineupSceneMgr.Instance.ShowLineupModule(TeamType.ExploreStoryTask, mExploreDataVO.mId);
                break;
        }
    }
    private void AlertBack(bool result, bool blShowAgain)
    {
        if (result)
        {
            if (HeroDataModel.Instance.mHeroInfoData.mDiamond >= GameConfigMgr.Instance.GetSearchTaskConfig(mExploreDataVO.mTaskId).AccelCost)
            {
                GameNetMgr.Instance.mGameServer.ReqExploreSpeedup(mExploreDataVO.mId, _isStory);
                TDPostDataMgr.Instance.DoCostDiamond(TDCostDiamondType.BuyExploreSpeed,1, GameConfigMgr.Instance.GetSearchTaskConfig(mExploreDataVO.mTaskId).AccelCost);
            }
            else
                PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000055));
        }
        else
        {

        }
    }
    /// <summary>
    /// 改变lock的状态
    /// </summary>
    private void LockChange(bool isOn)
    {
        if (isLock)
        {
            if (mExploreDataVO.mExploreTaskCfg.Type == 1)
            {
                if (mExploreDataVO.mState == 0)
                {
                    GameNetMgr.Instance.mGameServer.ReqExploreLock(mExploreDataVO.mId, isOn);
                }
                else
                {
                    _btnLock.isOn = true;
                    if (isStart)
                        isStart = false;
                    else
                        PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000111));
                }
            }
            else
            {
                _btnLock.isOn = true;
                if (isStart)
                    isStart = false;
                else
                    PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000132));
            }
        }
    }

    public override void Hide()
    {
        isLock = false;
        base.Hide();
    }

    public override void Dispose()
    {
        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        _view = null;
        if (_timer != 0)
            TimerHeap.DelTimer(_timer);
        base.Dispose();
    }
}
