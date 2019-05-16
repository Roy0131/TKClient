using Framework.UI;
using UnityEngine.UI;

public class ArtifactAllAttItemView : UIBaseView
{
    private Text _attName;
    private Text _attValue;


    protected override void ParseComponent()
    {
        base.ParseComponent();
        _attName = Find<Text>("AttName");
        _attValue = Find<Text>("AttValue");
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        AttributeConfig config;
        config = GameConfigMgr.Instance.GetAttrConfig(int.Parse(args[0].ToString()));
        _attName.text = LanguageMgr.GetLanguage(config.NameID);
        float value = 0;
        value = (float)int.Parse(args[1].ToString()) / (float)config.Divisor;
        if (config.PercentShow > 0)
            _attValue.text = value + "%";
        else
            _attValue.text = args[1].ToString();
    }
}
