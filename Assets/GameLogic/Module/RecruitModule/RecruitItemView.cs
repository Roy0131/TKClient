using UnityEngine;
using UnityEngine.UI;
using NewBieGuide;
using Framework.UI;

public class RecruitItemView : UIBaseView
{
    private Text _NumText;
    private Text _timeText;
    private Text _oneText;
    private Text _tenText;
    private Text _callText;
    private Image _artcleImg;
    private Image _oneImg;
    private Image _tenImg;
    private Button _oneBtn;
    private Button _tenBtn;
    private GameObject _timeObj;
    private GameObject _callImgObj;
    private GameObject _callTextObj;
    private GameObject _freeTextObj;
    public RecruitDataVO mCurRecruitDataVO { get; private set; }

    private uint _time = 0;
    private int _curTime = 0;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _NumText = Find<Text>("ImgBj/NumText");
        _timeText = Find<Text>("TimeText");
        _oneText = Find<Text>("CallBtn/Image/Text");
        _tenText = Find<Text>("CallTenBtn/Image/Text");
        _callText = Find<Text>("CallBtn/Text");
        _artcleImg = Find<Image>("ImgBj/ArtcleImg");
        _oneImg = Find<Image>("CallBtn/Image");
        _tenImg = Find<Image>("CallTenBtn/Image");
        _oneBtn = Find<Button>("CallBtn");
        _tenBtn = Find<Button>("CallTenBtn");
        _timeObj = Find("TimeText");
        _callImgObj = Find("CallBtn/Image");
        _callTextObj = Find("CallBtn/Text");
        _freeTextObj = Find("CallBtn/Free");

        _oneBtn.onClick.Add(OnOne);
        _tenBtn.onClick.Add(OnTen);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        mCurRecruitDataVO = args[0] as RecruitDataVO;
        UnRegisteMaskObject();
        OnItem();
        RegisteMaskObject();
    }

    private void RegisteMaskObject()
    {
        if (mCurRecruitDataVO.mRecruitIndex == 0)
            NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.RecruitNormal, _oneBtn.transform);
        else if (mCurRecruitDataVO.mRecruitIndex == 1)
            NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.RecruitAdvanceBtn, _oneBtn.transform);
    }

    private void UnRegisteMaskObject()
    {
        if (mCurRecruitDataVO.mRecruitIndex == 0)
            NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.RecruitNormal);
        else if (mCurRecruitDataVO.mRecruitIndex == 1)
            NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.RecruitAdvanceBtn);
    }

    private void OnAddTime()
    {
        if (mCurRecruitDataVO.mRecruitIndex != 2)
        {
            _timeObj.SetActive(_curTime > 0);
            _callImgObj.SetActive(_curTime > 0);
            _callTextObj.SetActive(_curTime > 0);
            _freeTextObj.SetActive(_curTime <= 0);
            if (_curTime > 0)
            {
                _curTime -= 1;
                _timeText.text = (LanguageMgr.GetLanguage(5002504)+ " <color=#A5FD47>" + TimeHelper.GetCountTime(_curTime) + "</color>");
            }
        }
    }

    private void OnItem()
    {
        _curTime = mCurRecruitDataVO.LastTime;
        if (_time != 0)
            TimerHeap.DelTimer(_time);
        int interval = 1000;
        _time = TimerHeap.AddTimer(0, interval, OnAddTime);

        _callText.text = LanguageMgr.GetLanguage(5002505);
        _NumText.text = BagDataModel.Instance.GetItemCountById(mCurRecruitDataVO.mArticleId).ToString();
        _artcleImg.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(mCurRecruitDataVO.mArticleId).UIIcon);

        ObjectHelper.SetSprite(_artcleImg,_artcleImg.sprite);
        _oneText.text = mCurRecruitDataVO.mOneCont.ToString();
        _tenText.text = mCurRecruitDataVO.mTenCont.ToString();
        _oneImg.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(mCurRecruitDataVO.mOneId).UIIcon);
        _tenImg.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(mCurRecruitDataVO.mTenId).UIIcon);

        ObjectHelper.SetSprite(_oneImg,_oneImg.sprite);
        ObjectHelper.SetSprite(_tenImg,_tenImg.sprite);
    }

    private void OnOne()
    {
        string str = "";
        if (mCurRecruitDataVO.mOneId== SpecialItemID.Diamond)
            str = LanguageMgr.GetLanguage(4000055);
        else if(mCurRecruitDataVO.mOneId == SpecialItemID.FriendShipPoint)
            str = LanguageMgr.GetLanguage(6001181);
        else if(mCurRecruitDataVO.mOneId == SpecialItemID.Vulgar)
            str = LanguageMgr.GetLanguage(6001182);
        else if(mCurRecruitDataVO.mOneId == SpecialItemID.High)
            str = LanguageMgr.GetLanguage(6001183);
        if (mCurRecruitDataVO.mRecruitIndex == 0 && _curTime <= 0)
        {
            GameNetMgr.Instance.mGameServer.ReqDrawCard(mCurRecruitDataVO.mRecruitIndex * 2 + 1);
        }
        else if(mCurRecruitDataVO.mRecruitIndex == 1 && _curTime <= 0)
        {
            GameNetMgr.Instance.mGameServer.ReqDrawCard(mCurRecruitDataVO.mRecruitIndex * 2 + 1);
        }
        else if (mCurRecruitDataVO.mOneId != 3)
        {
            if (BagDataModel.Instance.GetItemCountById(mCurRecruitDataVO.mOneId) >= mCurRecruitDataVO.mOneCont)
                GameNetMgr.Instance.mGameServer.ReqDrawCard(mCurRecruitDataVO.mRecruitIndex * 2 + 1);
            else
                PopupTipsMgr.Instance.ShowTips(str);
        }
        else
        {
            if (HeroDataModel.Instance.mHeroInfoData.mDiamond >= mCurRecruitDataVO.mOneCont)
            {
                GameNetMgr.Instance.mGameServer.ReqDrawCard(mCurRecruitDataVO.mRecruitIndex * 2 + 1);
                TDPostDataMgr.Instance.DoCostDiamond(TDCostDiamondType.BuyRecruitOneCount, 1, mCurRecruitDataVO.mOneCont);
            }
            else
            {
                PopupTipsMgr.Instance.ShowTips(str);
            }
        }
    }
     
    private void OnTen()
    {
        string str = "";
        if (mCurRecruitDataVO.mTenId == SpecialItemID.Diamond)
            str = LanguageMgr.GetLanguage(4000055);
        else if (mCurRecruitDataVO.mTenId == SpecialItemID.FriendShipPoint)
            str = LanguageMgr.GetLanguage(6001181);
        else if (mCurRecruitDataVO.mTenId == SpecialItemID.Vulgar)
            str = LanguageMgr.GetLanguage(6001182);
        else if (mCurRecruitDataVO.mTenId == SpecialItemID.High)
            str = LanguageMgr.GetLanguage(6001183);
        if (mCurRecruitDataVO.mTenId == 3)
        {
            if (HeroDataModel.Instance.mHeroInfoData.mDiamond >= mCurRecruitDataVO.mTenCont)
            {
                GameNetMgr.Instance.mGameServer.ReqDrawCard(mCurRecruitDataVO.mRecruitIndex * 2 + 2);
                TDPostDataMgr.Instance.DoCostDiamond(TDCostDiamondType.BuyRecruitTenCount, 1, mCurRecruitDataVO.mTenCont);
            }
            else
            {
                PopupTipsMgr.Instance.ShowTips(str);
            }
        }
        else
        {
            if (BagDataModel.Instance.GetItemCountById(mCurRecruitDataVO.mTenId) >= mCurRecruitDataVO.mTenCont)
                GameNetMgr.Instance.mGameServer.ReqDrawCard(mCurRecruitDataVO.mRecruitIndex * 2 + 2);
            else
                PopupTipsMgr.Instance.ShowTips(str);
        }
    }
}
