using Framework.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactDetailView : UIBaseView
{
    private ArtifactDataVO _artifactDataVO;
    private Button _disBtn;
    private Button _recastBtn;
    private Button _levelUpBtn;
    private Image _itemIcon;
    private Image _bjImg;
    private Text _upText;
    private Text _detailName;
    private Text _grade;
    private Text _rankName;
    private List<ArtifactRankItemView> _listRankItemView;
    private GameObject _rankGrid;
    private RectTransform _parent;
    private ResCostGroup _resCostGroup;
    private List<ArtifactAttItemView> _listAttItemView;
    private GameObject _attItem;
    private RectTransform _attParent;
    private Text _skillName;
    private Text _skillDetail;
    private Image _skillIcon;
    private GameObject _kuang1;
    private GameObject _kuang2;
    private GameObject _kuang3;

    private GameObject _shenqi1;
    private UIEffectView _effect1;

    private GameObject _shenqi2;
    private UIEffectView _effect2;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _skillName = Find<Text>("Right/SkillImg/Name");
        _skillDetail = Find<Text>("Right/SkillExplain");
        _skillIcon = Find<Image>("Right/SkillImg/SkillIcon");
        _itemIcon = Find<Image>("Left/Obj");
        _bjImg = Find<Image>("Left");
        _disBtn = Find<Button>("DisBtn");
        _recastBtn = Find<Button>("Left/RecastBtn");
        _levelUpBtn = Find<Button>("Right/UpBtn");
        _upText = Find<Text>("Right/UpBtn/Text");
        _detailName = Find<Text>("Right/Detail");
        _grade = Find<Text>("Right/Grade");
        _rankName = Find<Text>("Right/Rank/Name");
        _rankGrid = Find("Right/Rank/RankGrid/item");
        _parent = Find<RectTransform>("Right/Rank/RankGrid");
        _attItem = Find("Right/AttImg/Item");
        _attParent = Find<RectTransform>("Right/AttImg");

        _kuang1 = Find("Left/Img1");
        _kuang2 = Find("Left/Img2");
        _kuang3 = Find("Left/Img3");
        _shenqi1 = Find("Left/icon_shenqi_1");
        _effect1 = CreateUIEffect(_shenqi1, UILayerSort.WindowSortBeginner + 5);
        _shenqi2 = Find("Left/icon_shenqi_2");
        _effect2 = CreateUIEffect(_shenqi2, UILayerSort.WindowSortBeginner + 3);        
        CreateFixedEffect(_itemIcon.gameObject, UILayerSort.WindowSortBeginner + 4, SortObjType.Canvas);
        //CreateFixedEffect(_recastBtn.gameObject, UILayerSort.WindowSortBeginner + 5, SortObjType.Canvas);
        //AdjustRecastButton();

        _resCostGroup = new ResCostGroup();
        _resCostGroup.SetDisplayObject(Find("Right/ResGroup"));

        _levelUpBtn.onClick.Add(OnLevelUp);
        _recastBtn.onClick.Add(OnRecast);
        _disBtn.onClick.Add(OnDis);
        ColliderHelper.SetButtonCollider(_disBtn.transform, 120, 120);
    }

    private void AdjustRecastButton()
    {
        Canvas canvas = _recastBtn.gameObject.GetComponent<Canvas>();
        if (canvas != null)
            return;
        canvas = _recastBtn.gameObject.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingLayerName = "TopLayer";
        canvas.sortingOrder = UILayerSort.WindowSortBeginner + 5;

        _recastBtn.gameObject.AddComponent<GraphicRaycaster>();
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        ArtifactDataModel.Instance.AddEvent<int>(ArtifactEvent.ArtifactReset, OnArtifactReset);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        ArtifactDataModel.Instance.RemoveEvent<int>(ArtifactEvent.ArtifactReset, OnArtifactReset);
    }

    private void OnArtifactReset(int id)
    {
        if (_artifactDataVO.mArtifactData.Id == id)
            GetItemTipMgr.Instance.ShowItemResult(_artifactDataVO.mDecomposeRes);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        AdjustRecastButton();
        _artifactDataVO = args[0] as ArtifactDataVO;
        if (_artifactDataVO.mArtifactData.Rank == 1)
        {
            _effect1.StopEffect();
            _effect2.StopEffect();
        }
        else if (_artifactDataVO.mArtifactData.Rank == 2)
        {
            _effect1.PlayEffect();
            _effect2.StopEffect();
        }
        else
        {
            _effect1.PlayEffect();
            _effect2.PlayEffect();
        }
        _kuang1.SetActive(_artifactDataVO.mArtifactData.Rank == 1);
        _kuang2.SetActive(_artifactDataVO.mArtifactData.Rank == 2);
        _kuang3.SetActive(_artifactDataVO.mArtifactData.Rank > 2);
        _detailName.text = LanguageMgr.GetLanguage(_artifactDataVO.mArtifactNameId);
        _grade.text = "Lv：" + _artifactDataVO.mArtifactData.Level + "/" + _artifactDataVO.mCurMaxLevel;
        _rankName.text = LanguageMgr.GetLanguage(5002705);
        _itemIcon.sprite = GameResMgr.Instance.LoadItemIcon("artifacticon/" + _artifactDataVO.mArtifactIcon);
        _bjImg.sprite = GameResMgr.Instance.LoadItemIcon("artifacticon/" + _artifactDataVO.mArtifactBjIcon);
        OnRankitemClear();
        _listRankItemView = new List<ArtifactRankItemView>();
        for (int i = 1; i < _artifactDataVO.mMaxRank; i++)
        {
            GameObject obj = GameObject.Instantiate(_rankGrid);
            obj.transform.SetParent(_parent, false);
            ArtifactRankItemView rankItemView = new ArtifactRankItemView();
            rankItemView.SetDisplayObject(obj);
            rankItemView.Show(i, _artifactDataVO.mArtifactData.Rank - 1);
            _listRankItemView.Add(rankItemView);
        }
        OnAttitemClear();
        _listAttItemView = new List<ArtifactAttItemView>();
        for (int i = 0; i < _artifactDataVO.mCurArtifactAtt.Count; i++)
        {
            GameObject obj = GameObject.Instantiate(_attItem);
            obj.transform.SetParent(_attParent, false);
            ArtifactAttItemView attItemView = new ArtifactAttItemView();
            attItemView.SetDisplayObject(obj);
            attItemView.Show(_artifactDataVO.mCurArtifactAtt[i]);
            _listAttItemView.Add(attItemView);
        }
        if (_artifactDataVO.mArtifactData.Rank == _artifactDataVO.mMaxRank && _artifactDataVO.mArtifactData.Level == _artifactDataVO.mCurMaxLevel)
        {
            _resCostGroup.Hide();
            _levelUpBtn.gameObject.SetActive(false);
        }
        else
        {
            _levelUpBtn.gameObject.SetActive(true);
            if (_artifactDataVO.mArtifactData.Level == _artifactDataVO.mCurMaxLevel)
            {
                _upText.text = LanguageMgr.GetLanguage(400005);
                _resCostGroup.Show(_artifactDataVO.mRankUpResCost);
            }
            else
            {
                _upText.text = LanguageMgr.GetLanguage(5002706);
                _resCostGroup.Show(_artifactDataVO.mLevelUpResCost);
            }
        }
        SkillConfig config = GameConfigMgr.Instance.GetSkillConfig(_artifactDataVO.mCurSkillId);
        _skillName.text = "Lv." + config.InnerLevel + " " + LanguageMgr.GetLanguage(config.NameID);
        string[] skillArgs = config.ShowParam.Split(',');
        _skillDetail.text = LanguageMgr.GetLanguage(config.DescrptionID, skillArgs);
        _skillIcon.sprite = GameResMgr.Instance.LoadSkillIcon(config.Icon);
    }

    private void OnRankitemClear()
    {
        if (_listRankItemView != null)
        {
            for (int i = 0; i < _listRankItemView.Count; i++)
                _listRankItemView[i].Dispose();
            _listRankItemView.Clear();
            _listRankItemView = null;
        }
    }

    private void OnAttitemClear()
    {
        if (_listAttItemView != null)
        {
            for (int i = 0; i < _listAttItemView.Count; i++)
                _listAttItemView[i].Dispose();
            _listAttItemView.Clear();
            _listAttItemView = null;
        }
    }

    private void OnLevelUp()
    {
        if (!_resCostGroup.BlEnough)
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001185));
            return;
        }
        if (_artifactDataVO.mArtifactData.Level == _artifactDataVO.mCurMaxLevel)
            GameNetMgr.Instance.mGameServer.ReqArtifactRankUp(_artifactDataVO.mArtifactData.Id);
        else
            GameNetMgr.Instance.mGameServer.ReqArtifactUp(_artifactDataVO.mArtifactData.Id);
    }

    private void OnRecast()
    {
        if (_artifactDataVO.mArtifactData.Level > 1)
            ConfirmTipsMgr.Instance.ShowConfirmTips(LanguageMgr.GetLanguage(400006), ShopRefresh);
        else
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(400007));
    }

    private void ShopRefresh(bool result, bool blShowAgain)
    {
        if (result)
        {
            GameNetMgr.Instance.mGameServer.ReqArtifactReset(_artifactDataVO.mArtifactData.Id);
        }
        else
        {

        }
    }

    private void OnDis()
    {
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(ArtifactEvent.ArtifactDetailHide);
    }

    public override void Dispose()
    {
        OnRankitemClear();
        OnAttitemClear();
        base.Dispose();
    }
}
