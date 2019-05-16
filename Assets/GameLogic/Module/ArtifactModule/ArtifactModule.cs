using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactModule : ModuleBase
{
    private Transform _root;
    private Text _goldNum;
    private Text _attText;
    private Image _goldImg;
    private Button _disBtn;
    private Button _help;
    private Button _attBtn;
    private Button _goldBtn;
    private ItemResGroup _resGroup;
    private ArtifactView _artifactView;
    private ArtifactDetailView _artifactDetailView;
    private ArtifactAttView _artifactAttView;
    private ArtifactPreView _artifactPreView;
    private int _index;
    private RectTransform _resources;

    public ArtifactModule()
        : base(ModuleID.Artifact, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_Artifact;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _root = Find<Transform>("ArtifactShow");
        _goldNum = Find<Text>("Resources/Gold/GoldNum");
        _attText = Find<Text>("AttBtn/Text");
        _goldImg = Find<Image>("Resources/Gold/GoldImg");
        _disBtn = Find<Button>("DisBtn");
        _help = Find<Button>("Help");
        _attBtn = Find<Button>("AttBtn");
        _goldBtn = Find<Button>("Resources/Gold/GoldBtn");
        _resources = Find<RectTransform>("Resources");

        _resGroup = new ItemResGroup();
        _resGroup.SetDisplayObject(Find("Resources/ResGroup"));

        _artifactView = new ArtifactView();
        _artifactView.SetDisplayObject(Find("ArtifactShow"));

        _artifactDetailView = new ArtifactDetailView();
        _artifactDetailView.SetDisplayObject(Find("ArtifactDetail"));

        _artifactAttView = new ArtifactAttView();
        _artifactAttView.SetDisplayObject(Find("ArtifactAtt"));
        _artifactAttView.SortingOrder = UILayerSort.WindowSortBeginner + 3;

        _artifactPreView = new ArtifactPreView();
        _artifactPreView.SetDisplayObject(Find("ArtifactPreview"));
        _artifactPreView.SortingOrder = UILayerSort.WindowSortBeginner + 3;

        _help.onClick.Add(OnHelp);
        _attBtn.onClick.Add(OnAtt);
        _goldBtn.onClick.Add(OnGold);
        _disBtn.onClick.Add(OnClose);
        ColliderHelper.SetButtonCollider(_disBtn.transform, 120, 120);
        ColliderHelper.SetButtonCollider(_help.transform, 120, 120);
        ColliderHelper.SetButtonCollider(_goldBtn.transform);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        HeroDataModel.Instance.AddEvent(HeroEvent.HeroInfoChange, OnGoldRefresh);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<ArtifactDataVO>(ArtifactEvent.ArtifactDetailShow, OnDetailShow);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<ArtifactDataVO>(ArtifactEvent.ArtifactPreViewShow, OnPreView);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(ArtifactEvent.ArtifactDetailHide, OnDetailHide);
        ArtifactDataModel.Instance.AddEvent(ArtifactEvent.ArtifactData,OnArtifactData);
        ArtifactDataModel.Instance.AddEvent<ArtifactDataVO>(ArtifactEvent.ArtifactRefresh, OnArtifactRefresh);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        HeroDataModel.Instance.RemoveEvent(HeroEvent.HeroInfoChange, OnGoldRefresh);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<ArtifactDataVO>(ArtifactEvent.ArtifactDetailShow, OnDetailShow);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<ArtifactDataVO>(ArtifactEvent.ArtifactPreViewShow, OnPreView);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(ArtifactEvent.ArtifactDetailHide, OnDetailHide);
        ArtifactDataModel.Instance.RemoveEvent(ArtifactEvent.ArtifactData, OnArtifactData);
        ArtifactDataModel.Instance.RemoveEvent<ArtifactDataVO>(ArtifactEvent.ArtifactRefresh, OnArtifactRefresh);
    }

    private void OnPreView(ArtifactDataVO artifactDataVO)
    {
        _artifactPreView.Show(artifactDataVO);
    }

    private void OnArtifactRefresh(ArtifactDataVO artifactDataVO)
    {
        SoundMgr.Instance.PlayEffectSound("UI_Hero_up");
        _artifactDetailView.Show(artifactDataVO);
    }

    private void OnArtifactData()
    {
        _artifactDetailView.Hide();
        _artifactAttView.Hide();
        _attBtn.gameObject.SetActive(true);
        _resources.anchoredPosition = new Vector2(0f, -50f);
        _artifactView.Show(_index);
    }

    private void OnDetailShow(ArtifactDataVO artifactDataVO)
    {
        _artifactView.Hide();
        _attBtn.gameObject.SetActive(false);
        _resources.anchoredPosition = new Vector2(70f, -50f);
        for (int i = 0; i < ArtifactDataModel.Instance.mListArtifactVO.Count; i++)
        {
            if (ArtifactDataModel.Instance.mListArtifactVO[i] == artifactDataVO)
            {
                if ((i + 1) % 3 == 0)
                    _index = (i + 1) / 3 - 1;
                else
                    _index = ((i + 1) / 3);
            }
        }
        _artifactDetailView.Show(artifactDataVO);
    }

    private void OnDetailHide()
    {
        _artifactDetailView.Hide();
        _attBtn.gameObject.SetActive(true);
        _resources.anchoredPosition = new Vector2(0f, -50f);
        _artifactView.Show(_index);
    }

    private void OnGoldRefresh()
    {
        _goldNum.text = UnitChange.GetUnitNum(HeroDataModel.Instance.mHeroInfoData.mGold);
        _goldImg.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(SpecialItemID.Gold).UIIcon);

        ObjectHelper.SetSprite(_goldImg, _goldImg.sprite);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _resGroup.Show(SpecialItemID.VibratingGold, SpecialItemID.Ullr);
        _index = 0;
        _attText.text = LanguageMgr.GetLanguage(400002);
        ArtifactDataModel.Instance.ReqArtifactData();
        OnGoldRefresh();
    }

    private void OnHelp()
    {
        HelpTipsMgr.Instance.ShowTIps(HelpType.ArtifactHelp);
    }

    private void OnAtt()
    {
        if (ArtifactDataModel.Instance.mAllArtifactAtt != null && ArtifactDataModel.Instance.mAllArtifactAtt.Count > 0)
            _artifactAttView.Show();
        else
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(400009));
    }

    private void OnGold()
    {
        GameUIMgr.Instance.OpenModule(ModuleID.Gold);
    }

    public override void Hide()
    {
        base.Hide();
        if (_artifactView != null)
            _artifactView.Hide();
        if (_artifactDetailView != null)
            _artifactDetailView.Hide();
        if (_artifactAttView != null)
            _artifactAttView.Hide();
    }

    public override void Dispose()
    {
        base.Dispose();
        if (_artifactView != null)
        {
            _artifactView.Dispose();
            _artifactView = null;
        }
        if (_artifactDetailView != null)
        {
            _artifactDetailView.Dispose();
            _artifactDetailView = null;
        }
        if (_artifactAttView != null)
        {
            _artifactAttView.Dispose();
            _artifactAttView = null;
        }
    }

    protected override void OnShowAnimator()
    {
        base.OnShowAnimator();
        ObjectHelper.PopAnimationLiner(_root);
    }
}
