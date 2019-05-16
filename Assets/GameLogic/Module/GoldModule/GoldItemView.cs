using Framework.UI;
using UnityEngine;
using UnityEngine.UI;


public class GoldItemView : UIBaseView
{
    private Text _goldNum;
    private Text _amdNum;
    private Image _amdImg;
    private Button _drawBtn;
    private GameObject _drawObj;
    private GameObject _amdImgObj;
    private GameObject _glodDrawObj;
    private GoldDataVO mGoldDataVO;
    private ImageGray _gray;
    public GameObject mRedObject { get; private set; }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _goldNum = Find<Text>("GoldBjImg/GoldNum");
        _amdNum = Find<Text>("DrawBtn/AmdImg/AmdNum");
        _amdImg = Find<Image>("DrawBtn/AmdImg");
        _drawBtn = Find<Button>("DrawBtn");
        _gray = Find<ImageGray>("DrawBtn/AmdImg");
        _drawObj = Find("DrawBtn/Draw");
        _amdImgObj = Find("DrawBtn/AmdImg");
        _glodDrawObj = Find("DrawBtn/GlodDraw");
        mRedObject = Find("DrawBtn/RedPoint");

        _drawBtn.onClick.Add(OnGold);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        mGoldDataVO = args[0] as GoldDataVO;
        if (mGoldDataVO.mGoldIndex == 1)
            RedPointTipsMgr.Instance.RedPointBindObject(RedPointEnum.GoldHand, mRedObject);
        OnGoldItem();
    }

    private void OnGoldItem()
    {
        if (mGoldDataVO.mLeftNum <= 0)
        {
            _gray.SetGray();
            _drawBtn.interactable = false;
        }
        else
        {
            _gray.SetNormal();
            _drawBtn.interactable = true;
        }
        _goldNum.text = mGoldDataVO.mGold.ToString();
        _amdNum.text = mGoldDataVO.mCon.ToString();
        _amdImg.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(SpecialItemID.Diamond).UIIcon);
        ObjectHelper.SetSprite(_amdImg,_amdImg.sprite);
        _drawObj.SetActive(mGoldDataVO.mGoldIndex != 1);
        _amdImgObj.SetActive(mGoldDataVO.mGoldIndex != 1);
        _glodDrawObj.SetActive(mGoldDataVO.mGoldIndex == 1);
    }

    private void OnGold()
    {
        if (mGoldDataVO.mGoldIndex == 1)
        {
            GameNetMgr.Instance.mGameServer.ReqGoldTou(mGoldDataVO.mGoldIndex);
        }
        else
        {
            if (HeroDataModel.Instance.mHeroInfoData.mDiamond >= mGoldDataVO.mCon)
            {
                TDPostDataMgr.Instance.DoCostDiamond(TDCostDiamondType.BuyGoldCount, 1, mGoldDataVO.mCon);
                GameNetMgr.Instance.mGameServer.ReqGoldTou(mGoldDataVO.mGoldIndex);
            }
            else
            {
                PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000055));
            }
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        if (mGoldDataVO.mGoldIndex == 1)
            RedPointTipsMgr.Instance.RedPointUnBindObject(RedPointEnum.GoldHand, mRedObject);
    }
}
