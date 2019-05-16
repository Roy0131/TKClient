using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public class ActivityCopyView : UIBaseView
{
    private ActivityCopyVO _activityCopyVO;
    private Text _textTitle;
    private Text _refresh;
    private Text _residue;
    private Text _num;
    private Text _timeText;
    private Button _addBtn;
    private Button _disBtn;
    private RectTransform _rectScrollCont;
    private GameObject _rankItemObj;

    private ActivityCopyBuyNumView _activityCopyBuyNumView;

    private uint _time = 0;
    private int _refreshTime = 0;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _textTitle = Find<Text>("Root/TextTitle");
        _refresh = Find<Text>("Root/Refresh");
        _residue = Find<Text>("Root/TextResidue");
        _num = Find<Text>("Root/Num");
        _timeText = Find<Text>("Root/Refresh/TextCountDown");
        _addBtn = Find<Button>("Root/ButtonAdd");
        _disBtn = Find<Button>("Root/ButtonClose");
        ColliderHelper.SetButtonCollider(_disBtn.transform);
        _rectScrollCont = Find<RectTransform>("Root/Scroll/Cont");
        _rankItemObj = Find("Root/RankItem");

        _activityCopyBuyNumView = new ActivityCopyBuyNumView();
        _activityCopyBuyNumView.SetDisplayObject(Find("ActivityCopyBuyNum"));

        _refresh.text = LanguageMgr.GetLanguage(5001322);
        _residue.text = LanguageMgr.GetLanguage(5001907);

        _addBtn.onClick.Add(OnAddBuy);
        _disBtn.onClick.Add(Hide);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _activityCopyVO = args[0] as ActivityCopyVO;
        _refreshTime = ActivityCopyDataModel.Instance.mActivityCopyTime;
        if (_time != 0)
            TimerHeap.DelTimer(_time);
        int interval = 1000;
        _time = TimerHeap.AddTimer(0, interval, OnAddTime);
        _textTitle.text = _activityCopyVO.mTitle;
        _num.text = _activityCopyVO.mRemainChallengeNum + "/" + ActivityCopyDataModel.Instance.mMaxChallengeNum;

        DiposeChildren();
        _childrenViews = new List<UIBaseView>();
        for (int i = 0; i < _activityCopyVO.mListActiveCfg.Count; i++)
        {
            GameObject itemview = GameObject.Instantiate(_rankItemObj);
            itemview.transform.SetParent(_rectScrollCont, false);
            ActivityCopyItemView activityCopyItemView = new ActivityCopyItemView();
            activityCopyItemView.SetDisplayObject(itemview);
            activityCopyItemView.Show(_activityCopyVO.mListActiveCfg[i], i, _activityCopyVO.mRemainChallengeNum);
            AddChildren(activityCopyItemView);
        }
    }

    private void OnAddTime()
    {
        if (_refreshTime > 0)
        {
            _refresh.gameObject.SetActive(true);
            _refreshTime--;
            _timeText.text = TimeHelper.GetCountTime(_refreshTime);
        }
        else
        {
            _refresh.gameObject.SetActive(false);
        }
    }

    private void OnAddBuy()
    {
        if (HeroDataModel.Instance.mHeroInfoData.mVipLevel == 0)
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000108));
        }
        else
        {
            if (_activityCopyVO.mRemainBuyChallengeNum == 0)
                PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000109));
            else
                _activityCopyBuyNumView.Show(_activityCopyVO.mRemainBuyChallengeNum);
        }
    }

    public override void Hide()
    {
        _rectScrollCont.anchoredPosition = Vector2.zero;
        if (_activityCopyBuyNumView != null)
            _activityCopyBuyNumView.Hide();
        base.Hide();
    }

    public override void Dispose()
    {
        if (_activityCopyBuyNumView != null)
        {
            _activityCopyBuyNumView.Dispose();
            _activityCopyBuyNumView = null;
        }
        base.Dispose();
    }
}
