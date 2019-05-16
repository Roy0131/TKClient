using Framework.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendBossView : UIBaseView
{
    private Button _closeBtn;
    private Button _battleBtn;
    private Button _sweepBtn;

    private Text _strengthText;
    private Text _strCDTimeText;

    private CardView _bossCardItem;
    private RectTransform _bossCardRoot;

    private Text _hpText;
    private Image _hpSlider;
    private int _playerID;
    private int _bossConfigID;

    private FriendBossSweepView _sweepView;
    protected override void ParseComponent()
    {
        base.ParseComponent();

        _bossCardRoot = Find<RectTransform>("BossRoot");
        _strengthText = Find<Text>("Strength/Count");
        _strCDTimeText = Find<Text>("StaminaTimeText");
        _hpText = Find<Text>("BossHpBar/HpText");
        _hpSlider = Find<Image>("BossHpBar/Slider");

        _closeBtn = Find<Button>("CloseBtn");
        _battleBtn = Find<Button>("BattleBtn");
        _sweepBtn = Find<Button>("SweepBtn");

        _sweepView = new FriendBossSweepView(OnStartSweep);
        _sweepView.SetDisplayObject(Find("FriendBossSweep"));

        _closeBtn.onClick.Add(Hide);
        _battleBtn.onClick.Add(OnBattle);
        _sweepBtn.onClick.Add(OnSweep);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        BattleDataModel.Instance.AddEvent(BattleEvent.BattleSweepEnd, OnSweepEnd);
        BagDataModel.Instance.AddEvent<List<int>>(BagEvent.BagItemRefresh, OnBagRefresh);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        BattleDataModel.Instance.RemoveEvent(BattleEvent.BattleSweepEnd, OnSweepEnd);
        BagDataModel.Instance.RemoveEvent<List<int>>(BagEvent.BagItemRefresh, OnBagRefresh);
    }

    private void OnBagRefresh(List<int> listItemId)
    {
        if (listItemId.Contains(SpecialItemID.FriendStrength))
            RefreshStrength();
    }

    private void OnSweepEnd()
    {
        int lastHp = BattleDataModel.Instance.mBattleExtParam;
        FriendBossDataVO bossDataVO;
        if (_playerID == HeroDataModel.Instance.mHeroPlayerId)
        {
            FriendDataModel.Instance.RefreshHeroBossHp(lastHp);//.mFriendAssistVO.mBossDataVO.RefreshBossHp(lastHp);
            bossDataVO = FriendDataModel.Instance.mFriendAssistVO.mBossDataVO;
        }
        else
        {
            FriendDataModel.Instance.RefreshFriendBossHp(_playerID, lastHp);
            FriendDataVO vo = FriendDataModel.Instance.GetFriendById(_playerID);
            bossDataVO = vo.mFriendBossVO;
        }

        if (lastHp == 0)
        {
            Hide();
        }
        else
        {
            _hpText.text = bossDataVO.mBossHpPercent + "%";
            _hpSlider.fillAmount = bossDataVO.mBossHpPercent / 100f;
        }
        RefreshStrength();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _sweepView.Hide();
        _playerID = int.Parse(args[0].ToString());
        FriendBossDataVO bossDataVO;
        if (_playerID == HeroDataModel.Instance.mHeroPlayerId)
        {
            bossDataVO = FriendDataModel.Instance.mFriendAssistVO.mBossDataVO;
        }
        else
        {
            FriendDataVO vo = FriendDataModel.Instance.GetFriendById(_playerID);
            bossDataVO = vo.mFriendBossVO;
        }
        _bossConfigID = bossDataVO.mBossConfigID;
        if (_bossCardItem == null)
        {
            _bossCardItem = CardViewFactory.Instance.CreateCardView(bossDataVO.mBossCardVO, CardViewType.Common);
            _bossCardItem.mRectTransform.SetParent(_bossCardRoot, false);
        }
        else
        {
            _bossCardItem.Show(bossDataVO.mBossCardVO);
        }
        _hpText.text = bossDataVO.mBossHpPercent + "%";
        _hpSlider.fillAmount = bossDataVO.mBossHpPercent / 100f;
        RefreshStrength();
    }

    private void RefreshStrength()
    {
        int curStrCount = BagDataModel.Instance.GetItemCountById(SpecialItemID.FriendStrength);
        _strengthText.text = curStrCount + "/" + GameConst.FriendBossStrengthMax;

        ClearCDTime();
        FriendAssistDataVO vo = FriendDataModel.Instance.mFriendAssistVO;
        _cdTime = vo.NextStrengthAddTime;
        if (_cdTime == 0)
        {
            _cdTime = curStrCount >= GameConst.FriendBossStrengthMax ? 0 : vo.mStrengthCostTime;
        }
        else
        {
            _strCDTimeText.text = "";
        }
        if (_cdTime > 0)
            _strCDTimeKey = TimerHeap.AddTimer(0, 1000, OnCDTime);
    }

    private void OnCDTime()
    {
        _strCDTimeText.text = LanguageMgr.GetLanguage(5001508) + TimeHelper.GetCountTime(_cdTime);
        if (_cdTime == 0)
        {
            RefreshStrength();
            return;
        }
        _cdTime--;
    }

    private uint _strCDTimeKey = 0;
    private int _cdTime;
    private void ClearCDTime()
    {
        if (_strCDTimeKey != 0)
            TimerHeap.DelTimer(_strCDTimeKey);
        _strCDTimeKey = 0;
    }

    private void OnBattle()
    {
        int curStrCount = BagDataModel.Instance.GetItemCountById(SpecialItemID.FriendStrength);
        if (curStrCount == 0)
        {
            LogHelper.LogWarning("体力不足");
            return;
        }

        FriendDataModel.Instance.RefreshStageId(GameConfigMgr.Instance.GetFriendBossConfig(_bossConfigID).BossStageID);
        LineupSceneMgr.Instance.ShowLineupModule(TeamType.FriendBoss, _playerID, _bossConfigID);
    }

    private void OnSweep()
    {
        _sweepView.Show();
    }

    private void OnStartSweep(int sweepCount)
    {
        if (sweepCount == 0)
            return;
        _sweepView.Hide();
        LineupSceneMgr.Instance.ShowLineupModule(TeamType.FriendBoss, _playerID, _bossConfigID, sweepCount);
        //BattleDataModel.Instance.ReqStartBattle(BattleType.FriendBoss, _friendId, sweepCount);
    }

    public override void Hide()
    {
        ClearCDTime();
        base.Hide();
    }

    public override void Dispose()
    {
        ClearCDTime();
        base.Dispose();
    }
}