using UnityEngine;
using System.Collections;

public class RenderUnitEffect : EffectBase
{
    private SceneRenderUnit _unit;
    public RenderUnitEffect(SceneRenderUnit unit)
    {
        _unit = unit;
    }
}
