using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public class FusionMatItem : UIBaseView
{
    private Image _icon;
    private Image _subscript;
    private Text _countText;
    private ImageGray _gray;
    private Button _button;
    private GameObject mRedPointObject;
    private CardRarityView _rarityView;
    public FusionMatDataVO mMatDataVO { get; private set; }
    protected override void ParseComponent()
    {
        base.ParseComponent();
        _icon = Find<Image>("Icon");
        _subscript = Find<Image>("Subscript");
        _countText = Find<Text>("Count");
        _gray = _icon.GetComponent<ImageGray>();
        _button = Find<Button>("Button");
        mRedPointObject = Find("RedDot");

        _rarityView = new CardRarityView();
        _rarityView.SetDisplayObject(Find("StarGroup"));

        _button.onClick.Add(OnShowSelectMat);
    }

    private void OnShowSelectMat()
    {
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(UIEventDefines.OpenFusionMatSelect, mMatDataVO.mIndex);
    }

    private bool EqualsTableId(CardDataVO vo)
    {
        return mMatDataVO.mCardTableId == 0 ? true : vo.mCardTableId == mMatDataVO.mCardTableId;
    }

    private bool EqualsCamp(CardDataVO vo)
    {
        return mMatDataVO.mCampCond == 0 ? true : vo.mCardConfig.Camp == mMatDataVO.mCampCond;
    }

    private bool EqualsType(CardDataVO vo)
    {
        return mMatDataVO.mTypeCond == 0 ? true : vo.mCardConfig.Type == mMatDataVO.mTypeCond;
    }

    private bool EqualsStar(CardDataVO vo)
    {
        return mMatDataVO.mStarCond == 0 ? true : vo.mCardConfig.Rarity == mMatDataVO.mStarCond;
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        HeroDataModel.Instance.AddEvent<S2CRoleFusionResponse>(HeroEvent.CardFusionBack, OnFusionBack);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        HeroDataModel.Instance.RemoveEvent<S2CRoleFusionResponse>(HeroEvent.CardFusionBack, OnFusionBack);
    }

    private void OnFusionBack(S2CRoleFusionResponse value)
    {
        OnRedPoint();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        FusionMatDataVO vo = args[0] as FusionMatDataVO;
        if (vo == mMatDataVO)
            return;
        mMatDataVO = vo;
        OnRedPoint();
        if (mMatDataVO.mType == 2)
        {
            _rarityView.Show(mMatDataVO.mStarShow);
            _icon.sprite = GameResMgr.Instance.LoadCardIcon(mMatDataVO.mDefIcon);
            ObjectHelper.SetSprite(_icon,_icon.sprite);
            _subscript.sprite = GameResMgr.Instance.LoadItemIcon(mMatDataVO.mSubscript);
            ObjectHelper.SetSprite(_subscript,_subscript.sprite);
        }
        else
        {
            if (mMatDataVO.mCardTableId > 0)
            {
                CardConfig cfg = GameConfigMgr.Instance.GetCardConfig(mMatDataVO.mCardTableId * 100 + 1);
                _rarityView.Show(cfg.Rarity);
                _icon.sprite = GameResMgr.Instance.LoadCardIcon(cfg.Icon);
                ObjectHelper.SetSprite(_icon,_icon.sprite);
                _subscript.sprite = GameResMgr.Instance.LoadItemIcon("campicon/cardtype" + cfg.Type);
                ObjectHelper.SetSprite(_subscript,_subscript.sprite);
            }
            else
            {
                _rarityView.Show(mMatDataVO.mStarCond);
                _icon.sprite = GameResMgr.Instance.LoadCardIcon(mMatDataVO.mDefIcon);
                ObjectHelper.SetSprite(_icon,_icon.sprite);
                if (mMatDataVO.mTypeCond > 0)
                {
                    _subscript.sprite = GameResMgr.Instance.LoadItemIcon("campicon/cardtype" + mMatDataVO.mTypeCond);
                    ObjectHelper.SetSprite(_subscript, _subscript.sprite);
                }
                else
                {
                    _subscript.sprite = GameResMgr.Instance.LoadItemIcon("campicon/camp" + mMatDataVO.mCampCond);
                    ObjectHelper.SetSprite(_subscript,_subscript.sprite);
                }
            }
        }
        RefreshStatus();
    }

    private void OnRedPoint()
    {
        int num = 0;
        List<CardDataVO> lstCards = HeroDataModel.Instance.mAllCards;
        for (int i = 0; i < lstCards.Count; i++)
        {
            if (lstCards[i].mCardID == mMatDataVO.mMainCardId)
                continue;
            if (!EqualsTableId(lstCards[i]))
                continue;
            if (!EqualsCamp(lstCards[i]))
                continue;
            if (!EqualsType(lstCards[i]))
                continue;
            if (!EqualsStar(lstCards[i]))
                continue;
            num++;
        }
        if (num >= mMatDataVO.mMatNum)
            mRedPointObject.SetActive(true);
        else
            mRedPointObject.SetActive(false);
    }

    public void RefreshStatus()
    {
        _countText.text = mMatDataVO.mlstMatIds.Count + "/" + mMatDataVO.mMatNum;
        if (mMatDataVO.BlMatEnough)
            _gray.SetNormal();
        else
            _gray.SetGray();
    }

    public override void Dispose()
    {
        mMatDataVO = null;
        if (_rarityView != null)
            _rarityView.Dispose();
        _rarityView = null;
        base.Dispose();
    }
}
