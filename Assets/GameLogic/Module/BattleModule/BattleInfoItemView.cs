using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public class BattleInfoItemView : UIBaseView
{
    private Image _bar;
    private CardView _cardView;
    private FighterStatisticVO _vo;
    private Text _numText;

    protected override void ParseComponent()
    {
        base.ParseComponent();

        _bar = Find<Image>("ImageBarBack/ImageBar");
        _numText = Find<Text>("TextNum");
    }

	protected override void Refresh(params object[] args)
	{
        base.Refresh(args);
        _vo = args[0] as FighterStatisticVO;
	}

    public void ShowStaticData(bool blDamage)
    {
        float flPer = 0f;
        if(blDamage)
        {
            int totalDamage = _vo.mBlHero ? BattleDataModel.Instance.mHeroTotalDamage : BattleDataModel.Instance.mTargetTotalDamage;
            flPer = (float)_vo.mDamageCount / (float)totalDamage;
            _numText.text = _vo.mDamageCount.ToString();
        }
        else
        {
            int totalHeal = _vo.mBlHero ? BattleDataModel.Instance.mHeroTotalHeal : BattleDataModel.Instance.mTargetTotalHeal;
            if (totalHeal <= 0)
                flPer = 0f;
            else
                flPer = (float)_vo.mHealCount / (float)totalHeal;
            _numText.text = _vo.mHealCount.ToString();

        }
        _bar.fillAmount = flPer;

        if (_cardView == null)
        {
            CardConfig cfg = GameConfigMgr.Instance.GetCardConfig(_vo.mItemConfigId);
            CardDataVO vo = new CardDataVO(cfg.ID, 1, _vo.mLevel);
            _cardView = CardViewFactory.Instance.CreateCardView(vo, CardViewType.None);
            _cardView.mRectTransform.SetParent(mRectTransform, false);
            _cardView.mRectTransform.localScale = Vector3.one * 0.8f;
            _cardView.mRectTransform.anchoredPosition = new Vector2(-171f, 0f);
        }
    }

	public override void Hide()
	{
        base.Hide();
        if (_cardView != null)
        {
            _cardView.mRectTransform.localScale = Vector3.one * 1.3f;
            CardViewFactory.Instance.ReturnCardView(_cardView);
            _cardView = null;
        }
        _vo = null;
	}

	public override void Dispose()
	{
        if (_cardView != null)
        {
            CardViewFactory.Instance.ReturnCardView(_cardView);
            _cardView = null;
        }
        _vo = null;
        base.Dispose();
	}
}