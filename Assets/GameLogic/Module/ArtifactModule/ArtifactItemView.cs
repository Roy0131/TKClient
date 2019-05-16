using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactItemView : UIBaseView
{
    private ArtifactDataVO _artifactDataVO;
    private Text _detailName;
    private Text _unlockName;
    private Text _unlockNum;
    private Text _unlockConditionName;
    private Text _unlockLevel;
    private Text _unlockVIP;
    private Image _unlockImg;
    private Image _bjImg;
    private Image _itemIcon;
    private Button _detailBtn;
    private Button _previewBtn;
    private GameObject _unlock;
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
        _detailName = Find<Text>("Btn/Text");
        _unlockName = Find<Text>("Btn/Img/Text");
        _unlockNum = Find<Text>("Btn/Img/Num");
        _unlockConditionName = Find<Text>("Unlock/Name");
        _unlockLevel = Find<Text>("Unlock/Level");
        _unlockVIP = Find<Text>("Unlock/VIP");
        _unlockImg = Find<Image>("Btn/Img");
        _bjImg = Find<Image>("BJ");
        _itemIcon = Find<Image>("Obj");
        _detailBtn = Find<Button>("Btn");
        _previewBtn = Find<Button>("Preview");
        ColliderHelper.SetButtonCollider(_previewBtn.transform);
        _unlock = Find("Unlock");

        _kuang1 = Find("Kuang/Img1");
        _kuang2 = Find("Kuang/Img2");
        _kuang3 = Find("Kuang/Img3");
        _shenqi1 = Find("icon_shenqi_1");
        _effect1 = CreateUIEffect(_shenqi1, UILayerSort.WindowSortBeginner + 2);
        _shenqi2 = Find("icon_shenqi_2");
        _effect2 = CreateUIEffect(_shenqi2, UILayerSort.WindowSortBeginner);
        CreateFixedEffect(_itemIcon.gameObject, UILayerSort.WindowSortBeginner + 1,SortObjType.Canvas);
        CreateFixedEffect(_unlock.gameObject, UILayerSort.WindowSortBeginner + 2, SortObjType.Canvas);

        _detailBtn.onClick.Add(OnDetail);
        _previewBtn.onClick.Add(OnPreView);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        ArtifactDataModel.Instance.AddEvent<int>(ArtifactEvent.ArtifactUnlock, OnArtifactUnlock);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        ArtifactDataModel.Instance.RemoveEvent<int>(ArtifactEvent.ArtifactUnlock, OnArtifactUnlock);
    }

    private void OnArtifactUnlock(int id)
    {
        if (_artifactDataVO.mArtifactData.Id == id)
        {
            _unlockImg.gameObject.SetActive(_artifactDataVO.mArtifactData.Level == 0);
            _detailName.gameObject.SetActive(_artifactDataVO.mArtifactData.Level > 0);
        }
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
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
        _unlockConditionName.text = LanguageMgr.GetLanguage(400012);
        _unlockLevel.text = LanguageMgr.GetLanguage(5002734) + " " + _artifactDataVO.mUnlockLevel.ToString();
        _unlockVIP.text = "VIP：" + _artifactDataVO.mUnlockVIPLevel.ToString();

        _unlockNum.text = UnitChange.GetUnitNum(_artifactDataVO.mUnlockInfo.Value);
        _unlockImg.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(_artifactDataVO.mUnlockInfo.Id).UIIcon);
        ObjectHelper.SetSprite(_unlockImg, _unlockImg.sprite);
        _itemIcon.sprite = GameResMgr.Instance.LoadItemIcon("artifacticon/" + _artifactDataVO.mArtifactIcon);
        _bjImg.sprite = GameResMgr.Instance.LoadItemIcon("artifacticon/" + _artifactDataVO.mArtifactBjIcon);

        _unlockImg.gameObject.SetActive(_artifactDataVO.mArtifactData.Level == 0);
        _detailName.gameObject.SetActive(_artifactDataVO.mArtifactData.Level > 0);
        _unlock.SetActive(_artifactDataVO.mArtifactData.Level == 0 && HeroDataModel.Instance.mHeroInfoData.mLevel < _artifactDataVO.mUnlockLevel
            || _artifactDataVO.mArtifactData.Level == 0 && HeroDataModel.Instance.mHeroInfoData.mVipLevel < _artifactDataVO.mUnlockVIPLevel);
        _unlockLevel.gameObject.SetActive(_artifactDataVO.mArtifactData.Level == 0 && HeroDataModel.Instance.mHeroInfoData.mLevel < _artifactDataVO.mUnlockLevel);
        _unlockVIP.gameObject.SetActive(_artifactDataVO.mArtifactData.Level == 0 && HeroDataModel.Instance.mHeroInfoData.mVipLevel < _artifactDataVO.mUnlockVIPLevel);
    }

    private void OnDetail()
    {
        if (_artifactDataVO.mArtifactData.Level > 0)
        {
            GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(ArtifactEvent.ArtifactDetailShow, _artifactDataVO);
        }
        else
        {
            if (!_unlock.activeSelf && BagDataModel.Instance.GetItemCountById(_artifactDataVO.mUnlockInfo.Id) >= _artifactDataVO.mUnlockInfo.Value)
                GameNetMgr.Instance.mGameServer.ReqArtifactUnlock(_artifactDataVO.mArtifactData.Id);
            else if (_unlock.activeSelf)
                PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(400008));
            else
                PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001185));
        }
    }

    private void OnPreView()
    {
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(ArtifactEvent.ArtifactPreViewShow, _artifactDataVO);
    }
}
