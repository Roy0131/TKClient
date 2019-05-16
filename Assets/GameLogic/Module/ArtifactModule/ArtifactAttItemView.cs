using Framework.UI;
using Msg.ClientMessage;
using UnityEngine.UI;

public class ArtifactAttItemView : UIBaseView
{
    private ItemInfo _info;
    private Text _name;
    private Text _value;


    protected override void ParseComponent()
    {
        base.ParseComponent();
        _name = Find<Text>("Name");
        _value = Find<Text>("Value");
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _info = args[0] as ItemInfo;
        AttributeConfig config;
        config = GameConfigMgr.Instance.GetAttrConfig(_info.Id);
        _name.text = LanguageMgr.GetLanguage(config.NameID);
        float value = 0;
        value = (float)_info.Value / (float)config.Divisor;
        if (config.PercentShow > 0)
            _value.text = value + "%";
        else
            _value.text = _info.Value.ToString();

    }
}
