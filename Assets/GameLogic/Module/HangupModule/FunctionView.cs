using UnityEngine.UI;
using Framework.UI;

public class FunctionView : UIBaseView
{
    private Text _featureName;
    private Image _featureImg;
    private Button _planelBtn;
    //private UIEffectView _effect;
    //private UIEffectView _effect02;
    //private UIEffectView _effect03;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _featureName = Find<Text>("NewFeature/FeatureName");
        _featureImg = Find<Image>("NewFeature/FeatureImg");
        _planelBtn = Find<Button>("Panel");
        CreateFixedEffect(Find("fx_ui_shengjijiesuo"),UILayerSort.TopSortBeginner + 1);
        CreateFixedEffect(Find("NewFeature"), UILayerSort.TopSortBeginner + 2, SortObjType.Canvas);
        //_effect02 = CreateUIEffect(Find("NewFeature/FeatureName"),UILayerSort.PopupSortBeginner + 2);
        //_effect03 = CreateUIEffect(Find("NewFeature/FeatureImg"),UILayerSort.PopupSortBeginner + 2);
        _planelBtn.onClick.Add(Hide);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        SystemUnlockConfig cfg = args[0] as SystemUnlockConfig;
        //_effect.PlayEffect();
        //_effect02.PlayEffect();
        //_effect03.PlayEffect();
        _featureName.text = LanguageMgr.GetLanguage(cfg.NameID);
        _featureImg.sprite = GameResMgr.Instance.LoadItemIcon(cfg.ClientGuildShowIcon);
        ObjectHelper.SetSprite(_featureImg,_featureImg.sprite);
        HeroDataModel.Instance.mHeroInfoData.OnConfigClear();
    }
}
