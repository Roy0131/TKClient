using Spine.Unity;
using UnityEngine.UI;
using Framework.UI;

public class PlayerLevelUpView : UIBaseView
{
    private Text _level;
    private SkeletonGraphic _graphic;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _level = Find<Text>("LVImg/Text");
        _graphic = Find<SkeletonGraphic>("SkeletonGraphic");
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _level.text = args[0].ToString();
        _graphic.AnimationState.SetAnimation(0, "animation", false);
    }

    public override void Dispose()
    {
        _graphic = null;
        base.Dispose();
    }
}
