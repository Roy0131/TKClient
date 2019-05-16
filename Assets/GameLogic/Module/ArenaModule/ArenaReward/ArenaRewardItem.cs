using Framework.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaRewardItem : UIBaseView
{
    private LayoutElement _layoutEle;
    private Image _recordImg;
    private GameObject _dailyObject;
    private GameObject _seasonObject;
    private GameObject _normalObject;

    private Text _normalRankText;
    private RectTransform _normalRewardGrid;

    private RectTransform _seasonRewardGrid;
    private Text _seasonSelfRankText;
    private Text _seasonRankText;
    private Text _seasonRewardDesText;

    private RectTransform _dailyRewardGrid;
    private Text _dailySelfTopRankTipsText;
    private Text _dailySelfTopRankText;
    private Text _dailySelfCurRankText;
    private Text _dailyEndTimeText;
    private Text _dailyRewardDesText;

    private RectTransform _rewardRoot;
    private int _index;
    private List<ItemView> _lstRewardView;
    private ArenaRewardType _type;
    private Image _normalImg;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _layoutEle = FindOnSelf<LayoutElement>();
        _dailyObject = Find("DailySelfItem");
        _dailyRewardGrid = Find<RectTransform>("DailySelfItem/RewardGrid");
        _dailySelfTopRankTipsText = Find<Text>("DailySelfItem/RewardRankTipsText");
        _dailySelfTopRankText = Find<Text>("DailySelfItem/RewardRankText");
        _dailySelfCurRankText = Find<Text>("DailySelfItem/SelfRankText");
        _dailyEndTimeText = Find<Text>("DailySelfItem/TimeText");
        _dailyRewardDesText = Find<Text>("DailySelfItem/DailyRewardDes");

        _recordImg = Find<Image>("SeasonSelfItem/ImageRecord");
        _seasonObject = Find("SeasonSelfItem");
        _seasonRewardGrid = Find<RectTransform>("SeasonSelfItem/RewardGrid");
        _seasonSelfRankText = Find<Text>("SeasonSelfItem/SelfRankText");
        _seasonRankText = Find<Text>("SeasonSelfItem/RankText");
        _seasonRewardDesText = Find<Text>("SeasonSelfItem/SeasonRewardDes");

        _normalObject = Find("NormalItem");
        _normalRankText = Find<Text>("NormalItem/RankText");
        _normalRewardGrid = Find<RectTransform>("NormalItem/RewardGrid");
        _normalImg = Find<Image>("NormalItem/ImageRecord");

        _lstRewardView = new List<ItemView>();
    }

    private uint _cdKey = 0;
    private void ClearCDTime()
    {
        if (_cdKey == 0)
            return;
        TimerHeap.DelTimer(_cdKey);
        _cdKey = 0;
    }

    private void OnDailyTimeCD()
    {
        if (_dailyRemainTime <= 0)
        {
            ClearCDTime();
            return;
        }
        _dailyRemainTime--;
        _dailyEndTimeText.text = TimeHelper.GetCountTime(_dailyRemainTime);
    }

    private int _dailyRemainTime;
    private void StartDailyCDTime()
    {
        _dailyRemainTime = ArenaDataModel.Instance.mArenaDataVO.DayRemainTime;
        if (_dailyRemainTime > 0)
            _cdKey = TimerHeap.AddTimer(0, 1000, OnDailyTimeCD);
        else
            _dailyEndTimeText.text = "00:00:00";
    }
    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        ClearCDTime();
        ArenaRewardVO vo = args[0] as ArenaRewardVO;
        if (_index == 0)
        {
            if (_type == ArenaRewardType.DailyReward)
            {
                StartDailyCDTime();
                _dailySelfTopRankTipsText.text = LanguageMgr.GetLanguage(500121, ":");
                _dailySelfTopRankText.text = ArenaDataModel.Instance.mArenaDataVO.mTopRank.ToString();
                _dailySelfCurRankText.text = LanguageMgr.GetLanguage(500122, vo.RewardTitle);
                _dailyRewardDesText.text = LanguageMgr.GetLanguage(500123);
            }
            else
            {
                string rewardTitle = vo.RewardTitle;
                if (_type == ArenaRewardType.SeasonReward)
                {
                    ArenaRewardVO selfVO = ArenaDataModel.Instance.GetSelfDailyRewardVO(ArenaRewardType.SeasonReward);
                    rewardTitle = selfVO == null ? "Unknow" : selfVO.RewardTitle;
                    _seasonSelfRankText.text = LanguageMgr.GetLanguage(500122, rewardTitle);
                    _seasonRewardDesText.text = LanguageMgr.GetLanguage(500124);
                    _recordImg.sprite = GameResMgr.Instance.LoadItemIcon("itemicon/icon_mingci_0" + (_index + 1));
                }
                else
                {
                    ArenaDivisionConfig config = GameConfigMgr.Instance.GetArenaDivisionConfig(ArenaDataModel.Instance.mArenaDataVO.mGrade);
                    if (config != null)
                        _seasonSelfRankText.text = LanguageMgr.GetLanguage(500125, LanguageMgr.GetLanguage(config.Name));
                    else
                        _seasonSelfRankText.text = LanguageMgr.GetLanguage(500125, rewardTitle);
                    _seasonRewardDesText.text = LanguageMgr.GetLanguage(500126);
                    _recordImg.sprite = GameResMgr.Instance.LoadItemIcon(vo.config.Icon);
                    ObjectHelper.SetSprite(_recordImg,_recordImg.sprite);
                }
                _seasonRankText.text = vo.RewardTitle;
            }
        }
        else
        {
            if (_type == ArenaRewardType.RankReward)
            {
                _normalRankText.gameObject.SetActive(false);
                _normalImg.gameObject.SetActive(true);
                _normalImg.sprite = GameResMgr.Instance.LoadItemIcon(vo.config.Icon);
                ObjectHelper.SetSprite(_normalImg,_normalImg.sprite);
            }
            else
            {
                switch (vo.RewardTitle)
                {
                    case "1":
                        _normalRankText.gameObject.SetActive(false);
                        _normalImg.gameObject.SetActive(true);
                        break;
                    case "2":
                        _normalRankText.gameObject.SetActive(false);
                        _normalImg.gameObject.SetActive(true);
                        break;
                    case "3":
                        _normalRankText.gameObject.SetActive(false);
                        _normalImg.gameObject.SetActive(true);
                        break;
                    default:
                        _normalRankText.gameObject.SetActive(true);
                        _normalImg.gameObject.SetActive(false);
                        _normalRankText.text = vo.RewardTitle;
                        break;
                }
                if (_type == ArenaRewardType.DailyReward)
                {
                    _normalImg.sprite = GameResMgr.Instance.LoadItemIcon("itemicon/icon_mingci_0" + _index);
                    ObjectHelper.SetSprite(_normalImg, _normalImg.sprite);
                }
                else
                {
                    _normalImg.sprite = GameResMgr.Instance.LoadItemIcon("itemicon/icon_mingci_0" + (_index + 1));
                    ObjectHelper.SetSprite(_normalImg,_normalImg.sprite);
                }
            }
        }
        ClearRewardView();
        ItemView view;
        for (int i = 0; i < vo.mlstReward.Count; i++)
        {
            view = ItemFactory.Instance.CreateItemView(vo.mlstReward[i], ItemViewType.BagItem);
            view.mRectTransform.SetParent(_rewardRoot, false);
            _lstRewardView.Add(view);
        }
    }

    private void ClearRewardView()
    {
        if (_lstRewardView == null || _lstRewardView.Count == 0)
            return;
        for (int i = 0; i < _lstRewardView.Count; i++)
            ItemFactory.Instance.ReturnItemView(_lstRewardView[i]);
        _lstRewardView.Clear();
    }

    public void SetRewardType(ArenaRewardType type, int index)
    {
        _index = index;
        _type = type;
        float height = 100f;
        _dailyObject.SetActive(false);
        _seasonObject.SetActive(false);
        _normalObject.SetActive(false);
        if (index > 0)
        {
            _normalObject.SetActive(true);
            _rewardRoot = _normalRewardGrid;
        }
        else
        {
            if (type == ArenaRewardType.DailyReward)
            {
                _dailyObject.SetActive(true);
                _rewardRoot = _dailyRewardGrid;
                height = 256f;
            }
            else
            {
                _seasonObject.SetActive(true);
                _rewardRoot = _seasonRewardGrid;
                height = 172f;
            }
        }
        mRectTransform.sizeDelta = new Vector2(810f, height);
        _layoutEle.preferredHeight = height;
    }

    public override void Dispose()
    {
        ClearCDTime();
        ClearRewardView();
        _lstRewardView = null;
        base.Dispose();
    }
}