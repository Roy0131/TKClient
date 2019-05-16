using Framework.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpeditionView : UIBaseView
{
    private ExoeditionDataVO _exoeditionDataVO;
    private Text _stageName;
    private Text _playerName;
    private Text _playerLevel;
    private Text _playerPower;
    private Text _rewardText;
    private Text _warningName;
    private Text _warningText;
    private Image _playerHead;
    private Button _disBtn;
    private Button _bulletBtn;
    private GameObject _warning;

    private RectTransform _parent;
    private List<ItemView> _listItemView;

    private RawImage _roleImage;
    private List<GameObject> _lstRoleFlags;
    private List<GameObject> _lstHpObj;


    protected override void ParseComponent()
    {
        base.ParseComponent();
        _stageName = Find<Text>("TextLayer");
        _playerName = Find<Text>("LeftSide/NameImg1/Name");
        _playerLevel = Find<Text>("LeftSide/NameImg1/Level");
        _playerPower = Find<Text>("LeftSide/NameImg1/WarImg/Text");
        _rewardText = Find<Text>("LeftSide/NameImg2/Reward");
        _warningName = Find<Text>("LeftSide/Warning/WarningName");
        _warningText = Find<Text>("LeftSide/Warning/WarningText");
        _playerHead = Find<Image>("LeftSide/NameImg1/heroDeta");
        _disBtn = Find<Button>("Btn_Back");
        ColliderHelper.SetButtonCollider(_disBtn.transform);
        _bulletBtn = Find<Button>("RightSide/ButtonBattle");
        _parent = Find<RectTransform>("LeftSide/RewardGrid");
        _warning = Find("LeftSide/Warning");

        _roleImage = Find<RawImage>("RightSide/RoleRoot/RawImage");
        _lstRoleFlags = new List<GameObject>();
        for (int i = 1; i <= 9; i++)
            _lstRoleFlags.Add(Find("RightSide/RoleRoot/t" + i + "/ImageHave"));
        _lstHpObj = new List<GameObject>();
        for (int i = 1; i <= 9; i++)
            _lstHpObj.Add(Find("RightSide/RoleRoot/HPObj" + i));

        _disBtn.onClick.Add(Hide);
        _bulletBtn.onClick.Add(OnBullet);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _roleImage.gameObject.SetActive(false);
        _warningName.text = LanguageMgr.GetLanguage(5001103);
        _warningText.text = LanguageMgr.GetLanguage(5003408);
        RoleCreate();
        _exoeditionDataVO = args[0] as ExoeditionDataVO;
        ExpeditionConfig cfg = GameConfigMgr.Instance.GetExpeditionConfig(ExpeditionDataModel.Instance.mCurStage + 1);
        _warning.gameObject.SetActive(cfg.StageType == 2);
        _rewardText.text = LanguageMgr.GetLanguage(5001704);
        _stageName.text = LanguageMgr.GetLanguage(6001146) + " " + (ExpeditionDataModel.Instance.mCurStage + 1);
        _playerName.text = _exoeditionDataVO.mPlayerName;
        _playerLevel.text = _exoeditionDataVO.mPlayerLevel.ToString();
        _playerPower.text = _exoeditionDataVO.mPlayerPower.ToString();
        if (_exoeditionDataVO.mPlayerHead > 0)
            _playerHead.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(_exoeditionDataVO.mPlayerHead).Icon);
        OnItemView();
        _listItemView = new List<ItemView>();
        for (int i = 0; i < _exoeditionDataVO.mListInfo.Count; i++)
        {
            ItemView view = ItemFactory.Instance.CreateItemView(_exoeditionDataVO.mListInfo[i], ItemViewType.BagItem);
            view.mRectTransform.SetParent(_parent, false);
            _listItemView.Add(view);
        }
    }

    private void OnItemView()
    {
        if (_listItemView != null && _listItemView.Count > 0)
        {
            for (int i = 0; i < _listItemView.Count; i++)
                ItemFactory.Instance.ReturnItemView(_listItemView[i]);
        }
    }

    private void OnBullet()
    {
        LineupSceneMgr.Instance.ShowLineupModule(TeamType.Expedition);
        ExpeditionDataModel.Instance.OnCurCfgId(ExpeditionDataModel.Instance.mCurStage);
    }

    private List<HpBarView> _lstHpBarView;
    private void RoleCreate()
    {
        ClearHpBar();
        _lstHpBarView = new List<HpBarView>();
        for (int i = 0; i < 9; i++)
            _lstRoleFlags[i].SetActive(false);
        for (int i = 0; i < 9; i++)
            _lstHpObj[i].SetActive(false);
        List<ExpeditionEnemyRole> listEnemyRole = ExpeditionDataModel.Instance.mExoeditionDataVO.mListEnemyRole;
        Dictionary<int, string> monsters = new Dictionary<int, string>();
        int slot;
        for (int i = 0; i < listEnemyRole.Count; i++)
        {
            CardConfig cardCfg = GameConfigMgr.Instance.GetCardConfig(listEnemyRole[i].TableId * 100 + listEnemyRole[i].Rank);
            slot = listEnemyRole[i].Position;
            _lstRoleFlags[slot].SetActive(true);
            _lstHpObj[slot].SetActive(true);
            monsters.Add(slot + 1, cardCfg.Model);

            ModelConfig modelConfig = GameConfigMgr.Instance.GetModelConfig(cardCfg.Model);
            if (modelConfig == null)
            {
                LogHelper.LogError("model config not found, model name:" + cardCfg.Model);
                continue;
            }
            float flModelHeight = (float)modelConfig.Height;
            HpBarView hpView = BattleUIMgr.CreateUIFighterHpbar(listEnemyRole[i].HpPercent, listEnemyRole[i].Level, cardCfg.Type);
            hpView.mRectTransform.SetParent(_lstHpObj[slot].transform, false);
            hpView.mRectTransform.localScale = new Vector3(-1f, 1f, 1f);
            GameUIMgr.Instance.ChildAddToParent(hpView.mRectTransform, _lstHpObj[slot].transform as RectTransform);
            hpView.mRectTransform.anchoredPosition = new Vector2(0, flModelHeight - 20);
            _lstHpBarView.Add(hpView);
        }
        RoleRTMgr.Instance.ShowRoleRTLogic(RoleRTType.BeforeBattle, monsters, true);
        if (!_roleImage.gameObject.activeSelf)
            _roleImage.gameObject.SetActive(true);
        _roleImage.texture = RoleRTMgr.Instance.GetRoleRTImage();
    }

    private void ClearHpBar()
    {
        if (_lstHpBarView == null)
            return;
        for (int i = 0; i < _lstHpBarView.Count; i++)
        {
            _lstHpBarView[i].Dispose();
            _lstHpBarView[i] = null;
        }
        _lstHpBarView.Clear();
        _lstHpBarView = null;
    }

    public override void Hide()
    {
        base.Hide();
        ClearHpBar();
        RoleRTMgr.Instance.Hide(RoleRTType.BeforeBattle);
    }

    public override void Dispose()
    {
        if (_lstRoleFlags != null)
        {
            _lstRoleFlags.Clear();
            _lstRoleFlags = null;
        }
        OnItemView();
        base.Dispose();
    }
}
