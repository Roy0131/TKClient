using Framework.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuildBossView : UIBaseView
{
    private Text _time;
    private List<Toggle> _listTog;
    private Button _leftBtn;
    private Button _rightBtn;
    private Button _resetBtn;
    private Button _help;
    private GameObject _leftObj;
    private GameObject _rightObj;
    private GameObject _resetTextObj;
    private GameObject _resetTimeObj;
    private GuildCurBossHurtVO _curBossHurtVO;
    private List<GuildBossItemView> _guildossItemView;

    GuildBossCopyView _guildBossCopyView;
    GuildBossHurtView _guildBossHurtView;
    private int _bookmarkNum;
    private int _bookmark;
    private int bossNum;

    private uint _timer = 0;
    private int _resetTime = 0;

    private bool isCopyBoss = false;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _time = Find<Text>("ResetBtn/Time");
        _leftBtn = Find<Button>("LeftBtn");
        _rightBtn = Find<Button>("RightBtn");
        _resetBtn = Find<Button>("ResetBtn");
        _help = Find<Button>("HelpBtn");
        _leftObj = Find("LeftBtn");
        _rightObj = Find("RightBtn");
        _resetTextObj = Find("ResetBtn/Text");
        _resetTimeObj = Find("ResetBtn/Time");

        _guildossItemView = new List<GuildBossItemView>();
        for (int i = 0; i < 3; i++)
        {
            GameObject obj = Find("ScrollView/Content/Item" + (i + 1));
            GuildBossItemView bossItemView = new GuildBossItemView();
            bossItemView.SetDisplayObject(obj);
            _guildossItemView.Add(bossItemView);
        }

        _guildBossCopyView = new GuildBossCopyView();
        _guildBossCopyView.SetDisplayObject(Find("CopyDetails"));

        _guildBossHurtView = new GuildBossHurtView();
        _guildBossHurtView.SetDisplayObject(Find("HurtRanking"));

        _leftBtn.onClick.Add(OnLeft);
        _rightBtn.onClick.Add(OnRight);
        _resetBtn.onClick.Add(OnReset);
        _help.onClick.Add(OnHelp);
    }

    private void OnMarkNum()
    {
        bossNum = 0;
        Dictionary<int, GuildBossConfig> AllDatas = GuildBossConfig.Get();
        foreach (GuildBossConfig cfg in AllDatas.Values)
            bossNum += 1;
        if (bossNum % 3 == 0)
            _bookmarkNum = bossNum / 3;
        else
            _bookmarkNum = (bossNum / 3) + 1;
        _listTog = new List<Toggle>();
        for (int i = 0; i < _bookmarkNum; i++)
        {
            GameObject obj = Find("TogGroup/Toggle" + i);
            obj.SetActive(true);
            _listTog.Add(Find<Toggle>("TogGroup/Toggle" + i));
        }
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GuildBossDataModel.Instance.AddEvent<GuildCurBossHurtVO>(GuildEvent.ReqBossResult, OnBossResult);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(GuildEvent.ReqCurBoss, OnCurBoss);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<int>(GuildEvent.ShowHurt, OnShowHurt);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GuildBossDataModel.Instance.RemoveEvent<GuildCurBossHurtVO>(GuildEvent.ReqBossResult, OnBossResult);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(GuildEvent.ReqCurBoss, OnCurBoss);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<int>(GuildEvent.ShowHurt, OnShowHurt);
    }

    private void OnResetTime()
    {
        _resetTime = _curBossHurtVO.ResetTime;
        _resetTimeObj.SetActive(_resetTime > 0);
        _resetTextObj.SetActive(_resetTime <= 0);
        if (_resetTime <= 0 && GuildDataModel.Instance.mGuildDataVO.mOfficeType == GuildOfficeType.President)
        {
            _resetBtn.interactable = true;
            return;
        }
        else
        {
            _resetBtn.interactable = false;
        }
        if (_timer != 0)
            TimerHeap.DelTimer(_timer);
        int interval = 1000;
        _timer = TimerHeap.AddTimer(0, interval, OnAddTime);
    }

    private void OnAddTime()
    {
        if (_resetTime > 0)
        {
            _resetTime -= 1;
            _time.text = TimeHelper.GetCountTime(_resetTime);
        }
    }

    private void OnShowHurt(int bossId)
    {
        _guildBossHurtView.Show(bossId);
    }

    private void OnCurBoss()
    {
        _guildBossCopyView.Show(_curBossHurtVO);
    }

    private void OnBossResult(GuildCurBossHurtVO curBossVO)
    {
        if (isCopyBoss && curBossVO.mCurBossId > 0)
            _guildBossCopyView.Show(curBossVO);
        _curBossHurtVO = curBossVO;
        OnResetTime();
        OnMarkNum();
        if (curBossVO.mCurBossId % 3 == 0)
            _bookmark = curBossVO.mCurBossId / 3 - 1;
        else
            _bookmark = curBossVO.mCurBossId / 3;
        OnBookmark(_bookmark);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        isCopyBoss = bool.Parse(args[0].ToString());
        GuildBossDataModel.Instance.ReqBossData();
    }

    private void OnBossChange()
    {
        for (int i = 0; i < _guildossItemView.Count; i++)
        {
            if (_bookmark == _bookmarkNum - 1 && i >= bossNum % 3 && bossNum % 3 != 0)
                _guildossItemView[i].Hide();
            else
                _guildossItemView[i].Show(_bookmark * 3 + i, _curBossHurtVO.mCurBossId);
        }
    }

    private void OnBookmark(int book)
    {
        _leftObj.SetActive(book > 0);
        _rightObj.SetActive(book < (_bookmarkNum - 1));
        for (int i = 0; i < _bookmarkNum; i++)
        {
            if (i == book)
                _listTog[i].isOn = true;
            else
                _listTog[i].isOn = false;
        }

        OnBossChange();
    }

    private void OnLeft()
    {
        _bookmark -= 1;
        OnBookmark(_bookmark);
    }

    private void OnRight()
    {
        _bookmark += 1;
        OnBookmark(_bookmark);
    }

    private void OnReset()
    {
        ConfirmTipsMgr.Instance.ShowConfirmTips(LanguageMgr.GetLanguage(5003147), BossRefresh);
    }

    private void BossRefresh(bool result, bool blShowAgain)
    {
        if (result)
            GameNetMgr.Instance.mGameServer.ReqStageReset();
    }

    private void OnHelp()
    {
        HelpTipsMgr.Instance.ShowTIps(HelpType.GuildBossHelp);
    }

    public override void Dispose()
    {
        _guildBossCopyView.Dispose();
        _guildBossCopyView = null;
        _guildBossHurtView.Dispose();
        _guildBossHurtView = null;
        base.Dispose();
    }

    public override void Hide()
    {
        _guildBossCopyView.Hide();
        _guildBossHurtView.Hide();
        base.Hide();
    }
}
