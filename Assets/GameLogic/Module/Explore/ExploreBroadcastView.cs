using Framework.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class ExploreBroadcastView : UIBaseView
{
    private ExploreDataVO _exploreDataVo;
    private Button _btnTaskClear;
    private Transform _gridReward;
    private bool _isStory;
    private Text _textDis;
    private GameObject _text;
    private GameObject _textGrid;
    private uint _timerKey;
    private int _remainTime;
    private SearchTaskDialogConfig _searchTaskDialogCfg;
    private SearchTaskConfig _searchTaskCfg;
    private List<string> _lstStr;
    private int _num;
    private int _textTime;
    private int _timeUse;
    private RawImage _rawImage;

    private readonly HangDataVO _hangDataVO = new HangDataVO();

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _btnTaskClear = Find<Button>("ImageBack04/Button");
        _gridReward = Find<Transform>("GridReward");
        _textDis = Find<Text>("ImageBack04/TextItem");
        _text = Find("ImageBack04/TextItem");
        _textGrid = Find("ImageBack04/ScrollView/Content");
        _rawImage = Find<RawImage>("ImageBack/RawImage");
        _btnTaskClear.onClick.Add(OnClose);

    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _exploreDataVo = args[0] as ExploreDataVO;
        _isStory = (bool)args[1];
        _remainTime = (int)args[2];
        _num = 0;
        InitData();
    }

    private void InitData()
    {
        DiposeChildren();

        if (_isStory)
            _btnTaskClear.interactable = false;
        else
            _btnTaskClear.interactable = true;
        _searchTaskDialogCfg = GameConfigMgr.Instance.GetSearchTaskDialogConfig(_exploreDataVo.mNameId4Title == 0 ? Convert.ToInt32(GameConfigMgr.Instance.GetSearchTaskConfig(_exploreDataVo.mTaskId).TaskNameList) : _exploreDataVo.mNameId4Title);

        _searchTaskCfg = GameConfigMgr.Instance.GetSearchTaskConfig(_exploreDataVo.mTaskId);
        _timeUse = _searchTaskCfg.SearchTime - _remainTime;

        _hangDataVO.Reset();
        List<CardDataVO> value = HeroDataModel.Instance.mAllCards;
        if (value != null)
            for (int i = 0; i < value.Count; i++)
                if (_exploreDataVo.mRoleIds != null)
                    for (int j = 0; j < _exploreDataVo.mRoleIds.Count; j++)
                        if (value[i].mCardID == _exploreDataVo.mRoleIds[j])
                            _hangDataVO.AddRole(value[i].mCardConfig);
        string[] taskRole = _searchTaskCfg.TaskRoleList.Split(',');
        List<int> lstBossId = new List<int>();

        if (_searchTaskCfg.TaskRoleList.Split(',').Length == 1)
        {
            _hangDataVO.AddBossCard(GameConfigMgr.Instance.GetCardConfig(Convert.ToInt32(_searchTaskCfg.TaskRoleList) * 100 + 1));
        }
        else
        {
            for (int i = 0; i < taskRole.Length; i++)
                if (Convert.ToInt32(taskRole[i].Split('|')[0]) == _exploreDataVo.mRoleId4Title)
                    for (int j = 0; j < taskRole[i].Split('|').Length; j++)
                        lstBossId.Add(Convert.ToInt32(taskRole[i].Split('|')[j]));
            lstBossId.Remove(lstBossId[0]);
            for (int i = 0; i < lstBossId.Count; i++) _hangDataVO.AddBossCard(GameConfigMgr.Instance.GetCardConfig(lstBossId[i] * 100 + 1));
        }
        HangUpMgr.Instance.ShowHangup(_hangDataVO);
        _rawImage.texture = RoleRTMgr.Instance.GetRoleRTImage();
        _rawImage.gameObject.SetActive(true);
        #region
        //奖励创建显示
        //Dictionary<int, int> dictRandomRewards = new Dictionary<int, int>();
        //if (_exploreDataVo.mRandomRewards != null)
        //{
        //    for (int i = 0; i < _exploreDataVo.mRandomRewards.Count / 2; i++) dictRandomRewards.Add(_exploreDataVo.mRandomRewards[i], _exploreDataVo.mRandomRewards[i + 1]);
        //    foreach (KeyValuePair<int, int> kv in dictRandomRewards)
        //    {
        //        ItemInfo info = new ItemInfo
        //        {
        //            Id = kv.Key,
        //            Value = kv.Value
        //        };

        //        ItemView itemView = ItemFactory.Instance.CreateItemView(info, ItemViewType.RewardItem);

        //        itemView.mRectTransform.SetParent(_gridReward.transform, false);
        //        AddChildren(itemView);
        //    }
        //}
        //else
        //{
        //string constReward =GameConfigMgr.Instance.GetSearchTaskConfig(_exploreDataVo.mTaskId).ConstReward;
        //for (int i = 0; i < constReward.Split(',').Length / 2; i++) dictRandomRewards.Add(Convert.ToInt32(constReward.Split(',')[i]) ,Convert.ToInt32(constReward.Split(',')[i + 1]));
        //foreach (KeyValuePair<int, int> kv in dictRandomRewards)
        //{
        //    ItemInfo info = new ItemInfo
        //    {
        //        Id = kv.Key,
        //        Value = kv.Value
        //    };

        //    ItemView itemView = ItemFactory.Instance.CreateItemView(info, ItemViewType.RewardItem);
        //    itemView.mRectTransform.SetParent(_gridReward.transform, false);
        //    AddChildren(itemView);
        //}
        #endregion


        List<CardDataVO> lstAllCard = new List<CardDataVO>();
        lstAllCard.AddRange(HeroDataModel.Instance.mAllCards);
        if (_exploreDataVo.mRoleIds.Count != 0)
        {
            for (int i = 0; i < _exploreDataVo.mRoleIds.Count; i++)
            {
                if (HeroDataModel.Instance.GetCardDataByCardId(_exploreDataVo.mRoleIds[i]) != null)
                {
                    CardView cardView = CardViewFactory.Instance.CreateCardView(HeroDataModel.Instance.GetCardDataByCardId(_exploreDataVo.mRoleIds[i]), CardViewType.Common);
                    cardView.mRectTransform.SetParent(_gridReward, false);
                    AddChildren(cardView);
                }
            }
        }
        //}

        //内容的显示
        if (_textGrid.transform.childCount != 0)
            for (int i = 0; i < _textGrid.transform.childCount; i++)
                Object.Destroy(_textGrid.transform.GetChild(i).gameObject);
        string textId = "";
        textId = _searchTaskDialogCfg.DialogTextList;
        _lstStr = new List<string>();
        if (textId != null)
            for (int i = 0; i < textId.Split(',').Length; i++)
                _lstStr.Add(textId.Split(',')[i]);
        GameObject obj;
        if (_timeUse < 5)
        {
            _num = 0;
            _textTime -= 5;
        }
        else if (_timeUse >= 5 && _timeUse < 10)
        {
            for (int i = 0; i < 2; i++)
            {
                _textDis.text = TimeHelper.GetCountTime(i*5) + LanguageMgr.GetLanguage(Convert.ToInt32(_lstStr[i]));
                obj = Object.Instantiate(_text);
                obj.transform.SetParent(_textGrid.transform, false);
                obj.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                if (_timeUse % 5 == 0)
                    _textTime = _timeUse - 15;
                else
                    _textTime = _timeUse / 5 * 5-10;
                _num = (_timeUse - (3 - i) * 5) % (5 * _lstStr.Count) / 5;
                _textDis.text = TimeHelper.GetCountTime(_textTime) + LanguageMgr.GetLanguage(Convert.ToInt32(_lstStr[_num]));
                _textTime +=5;
                obj = Object.Instantiate(_text);
                obj.transform.SetParent(_textGrid.transform, false);
                obj.SetActive(true);
            }
        }

        //添加每五秒刷新一条消息
        _timerKey = TimerHeap.AddTimer(0, 5000, OnTimeCD);
    }

    /// <summary>
    ///     倒计时显示
    /// </summary>
    private void OnTimeCD()
    {
        if (_remainTime < 0)
        {
            ClearRemainTimer();
            ExploreDataModel.Instance.ReqExploreData(_isStory);
            GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(ExploreEvent.ExploreTaskClear);
            return;
        }

        if (_num >= _lstStr.Count - 1)
            _num = 0;
        else
            _num++;
        _textTime += 5;
        _textDis.text = TimeHelper.GetCountTime(_textTime) + "  " + LanguageMgr.GetLanguage(Convert.ToInt32(_lstStr[_num]));
        GameObject obj = Object.Instantiate(_text);
        obj.transform.SetParent(_textGrid.transform, false);
        obj.SetActive(true);
    }

    private void ClearRemainTimer()
    {
        if (_timerKey != 0)
            TimerHeap.DelTimer(_timerKey);
        _timerKey = 0;
    }

    public override void Hide()
    {
        ClearRemainTimer();
        base.Hide();
    }

    public override void Dispose()
    {
        ClearRemainTimer();
        base.Dispose();
    }

    private void OnClose()
    {
        GameNetMgr.Instance.mGameServer.ReqExploreTaskRemove(_exploreDataVo.mId, _isStory);
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(ExploreEvent.ExploreTaskClear);
    }
}