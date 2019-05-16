using Framework.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuildBossCopyView : UILoopBaseView<GuildBossHurtVO>
{
    private GuildCurBossHurtVO _curBossHurtVO;
    private Text _hurtNum;
    private Text _hp;
    private Text _time;
    private Text _amdNum;
    private Image _fill;
    private Image _amd;
    private Image _bossImg;
    private Button _disBtn;
    private Button _reward;
    private Button _res;
    private Button _fight;
    private Button _rewardDis;
    private GameObject _rewardObj;
    private GameObject _timeObj;
    private GameObject _conObj;
    private GameObject _comObj;
    private GameObject _rankItem;
    private GameObject _rewardOj;
    private RectTransform _parent;
    private RectTransform _killParent;
    private List<GuildBossKillItemView> listKillItemView;
    private uint _timer = 0;
    private int _bossTime = 0;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _hurtNum = Find<Text>("Copy/Num");
        _hp = Find<Text>("Copy/FillImg/Text");
        _time = Find<Text>("Copy/Time");
        _amdNum = Find<Text>("Copy/Res/Num");
        _fill = Find<Image>("Copy/FillImg/Fill");
        _amd = Find<Image>("Copy/Res/Img");
        _bossImg = Find<Image>("Copy/BossImg/Image");
        _disBtn = Find<Button>("Copy/CloseBtn");
        _reward = Find<Button>("Copy/Reward");
        _res = Find<Button>("Copy/Res");
        _fight = Find<Button>("Copy/Fight");
        _rewardDis = Find<Button>("Reward/KillReward/CloseBtn");
        _rewardObj = Find("Reward");
        _timeObj = Find("Copy/Time");
        _conObj = Find("Copy/Res");
        _comObj = Find("Copy/Fight");
        _rankItem = Find("Copy/Panel_Scroll/KnapsackPanel/RankItem");
        _rewardOj = Find("Reward/KillReward/Panel_Scroll/KnapsackPanel/Kill");
        _parent = Find<RectTransform>("Reward/KillReward/Panel_Scroll/KnapsackPanel/Combat/Obj");
        _killParent = Find<RectTransform>("Reward/KillReward/Panel_Scroll/KnapsackPanel");
        InitScrollRect("Copy/Panel_Scroll");

        _disBtn.onClick.Add(OnDis);
        _reward.onClick.Add(OnReward);
        _res.onClick.Add(OnRes);
        _fight.onClick.Add(OnFight);
        _rewardDis.onClick.Add(OnRewardDis);

        ColliderHelper.SetButtonCollider(_disBtn.transform);
        ColliderHelper.SetButtonCollider(_reward.transform, 80, 80);
        ColliderHelper.SetButtonCollider(_rewardDis.transform);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GuildBossDataModel.Instance.AddEvent<int>(GuildEvent.ReqBossDmg, OnBossDmg);
        GuildBossDataModel.Instance.AddEvent(GuildEvent.ResPawn, OnInit);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GuildBossDataModel.Instance.RemoveEvent<int>(GuildEvent.ReqBossDmg, OnBossDmg);
        GuildBossDataModel.Instance.RemoveEvent(GuildEvent.ResPawn, OnInit);
    }

    private void OnCopyResult(GuildCurBossHurtVO curHurtVO)
    {
        _curBossHurtVO = curHurtVO;
        GameNetMgr.Instance.mGameServer.ReqStageDamage(curHurtVO.mCurBossId);
        OnInit();
    }

    private void OnBossDmg(int bossId)
    {   
        _lstDatas = GuildBossDataModel.Instance.HurtVO(bossId);
        _loopScrollRect.ClearCells();
        if (_lstDatas == null)
            return;
        _loopScrollRect.totalCount = _lstDatas.Count;
        _loopScrollRect.RefillCells();
    }

    protected override UIBaseView CreateItemView()
    {
        GuildBossCopyItemView item = new GuildBossCopyItemView();
        item.SetDisplayObject(GameObject.Instantiate(_rankItem));
        return item;
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        OnCopyResult(args[0] as GuildCurBossHurtVO);
    }

    private void OnInit()
    {
        _bossTime = _curBossHurtVO.RefreshTime;
        if (_timer != 0)
            TimerHeap.DelTimer(_timer);
        int interval = 1000;
        _timer = TimerHeap.AddTimer(0, interval, OnAddTime);

        if (_curBossHurtVO.mRespawnNum < _curBossHurtVO.mTotalRespawnNum)
            _amdNum.text = _curBossHurtVO.mListRespawnNeedCost[_curBossHurtVO.mRespawnNum].ToString();
        _hurtNum.text = LanguageMgr.GetLanguage(5002908) + _curBossHurtVO.mCurBossId;
        _hp.text = _curBossHurtVO.mHpPercent + "%";
        _fill.fillAmount = (float)_curBossHurtVO.mHpPercent / (float)100;
        _amd.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(SpecialItemID.Diamond).Icon);
        _bossImg.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetGuildBossConfig(_curBossHurtVO.mCurBossId).Image);

        ObjectHelper.SetSprite(_amd,_amd.sprite);
        ObjectHelper.SetSprite(_bossImg,_bossImg.sprite);

        _conObj.SetActive(_curBossHurtVO.mStageState == 1 && _curBossHurtVO.mRespawnNum != _curBossHurtVO.mTotalRespawnNum);
        _comObj.SetActive(_curBossHurtVO.mStageState == 0 || _curBossHurtVO.mStageState == 1 && _curBossHurtVO.mRespawnNum == _curBossHurtVO.mTotalRespawnNum);
        if (_curBossHurtVO.mStageState == 1 && _curBossHurtVO.mRespawnNum == _curBossHurtVO.mTotalRespawnNum)
            _fight.interactable = false;
        else
            _fight.interactable = true;
    }

    private void OnAddTime()
    {
        _timeObj.SetActive(_bossTime > 0);
        if (_bossTime > 0)
        {
            _bossTime -= 1;
            _time.text = TimeHelper.GetCountTime(_bossTime);
        }
    }

    private void ClearRecruitItem()
    {
        if (listKillItemView != null)
        {
            for (int i = 0; i < listKillItemView.Count; i++)
                listKillItemView[i].Dispose();
            listKillItemView.Clear();
            listKillItemView = null;
        }
    }

    private void OnKillReward()
    {
        DiposeChildren();
        _childrenViews = new List<UIBaseView>();
        ClearRecruitItem();
        listKillItemView = new List<GuildBossKillItemView>();
        List<ItemInfo> listInfo = new List<ItemInfo>();
        listInfo = GuildBossDataModel.Instance.GetListRankReward(_curBossHurtVO.mCurBossId);
        List<int> listMin = new List<int>();
        listMin = GuildBossDataModel.Instance.GetListMin(_curBossHurtVO.mCurBossId);
        List<int> listMax = new List<int>();
        listMax = GuildBossDataModel.Instance.GetListMax(_curBossHurtVO.mCurBossId);
        if (listInfo.Count == 0)
            return;
        GuildBossConfig cfg = GameConfigMgr.Instance.GetGuildBossConfig(_curBossHurtVO.mCurBossId);
        string[] rewards = cfg.BattleReward.Split(',');
        int itemId = 0;
        int itemCount = 0;
        ItemView view;
        for (int i = 0; i < rewards.Length; i += 2)
        {
            ItemInfo itemInfo = new ItemInfo();
            view = new ItemView();
            if (rewards.Length % 2 != 0)
                continue;
            itemId = int.Parse(rewards[i]);
            itemCount = int.Parse(rewards[i + 1]);
            itemInfo.Id = itemId;
            itemInfo.Value = itemCount;

            view = ItemFactory.Instance.CreateItemView(itemInfo, ItemViewType.RewardItem, null);
            view.mRectTransform.SetParent(_parent, false);
            AddChildren(view);
        }
        for (int i = 0; i < listInfo.Count; i++)
        {
            GameObject obj = GameObject.Instantiate(_rewardOj);
            obj.transform.SetParent(_killParent, false);
            GuildBossKillItemView killItemView = new GuildBossKillItemView();
            killItemView.SetDisplayObject(obj);
            killItemView.Show(listInfo[i], listMin[i], listMax[i]);
            listKillItemView.Add(killItemView);
        }
    }

    private void OnDis()
    {
        Hide();
    }

    private void OnReward()
    {
        _rewardObj.SetActive(true);
        _killParent.anchoredPosition = new Vector2(0, 0);
        OnKillReward();
    }

    private void OnRes()
    {
        if (HeroDataModel.Instance.mHeroInfoData.mDiamond >= _curBossHurtVO.mListRespawnNeedCost[_curBossHurtVO.mRespawnNum])
        {
            GameNetMgr.Instance.mGameServer.ReqStagePlaterRespawn();
            if (_curBossHurtVO.mListRespawnNeedCost[_curBossHurtVO.mRespawnNum] > 0)
                TDPostDataMgr.Instance.DoCostDiamond(TDCostDiamondType.BuyGuildBossRefreshCount, 1, _curBossHurtVO.mListRespawnNeedCost[_curBossHurtVO.mRespawnNum]);
        }
        else
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000055));
        }
    }

    private void OnFight()
    {
        GuildBossDataModel.Instance.RefershStageId(GameConfigMgr.Instance.GetGuildBossConfig(_curBossHurtVO.mCurBossId).StageID);
        LineupSceneMgr.Instance.ShowLineupModule(TeamType.GuildBoss, _curBossHurtVO.mCurBossId);
    }

    private void OnRewardDis()
    {
        _rewardObj.SetActive(false);
    }
}
