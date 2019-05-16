using UnityEngine;
using System;
using Spine.Unity;

public class GrayComponent : IDisposable
{

    #region gray shader
    private static Shader _grayShader = null;
    public static Shader mGrayShader
    {
        get
        {
            if (_grayShader == null)
                _grayShader = Shader.Find("IHGame/RoleGray");
            return _grayShader;
        }
    }
    #endregion

    private Material _grayMat;
    private Material _mainMat;
    //private MeshRenderer _render;
    private SkeletonGraphic _skeleton;
    private bool _blGray;
    public GrayComponent(SkeletonGraphic graphic)
    {
        _grayMat = new Material(GrayComponent.mGrayShader);
        _skeleton = graphic;
        _mainMat = _skeleton.material;
        _blGray = false;
    }

    public void SetGray()
    {
        if (_blGray)
            return;
        _blGray = true;
        _skeleton.material = _grayMat;
    }

    public void SetNormal()
    {
        if (!_blGray)
            return;
        _blGray = false;
        _skeleton.material = _mainMat;
    }

    public void Dispose()
    {
        _grayMat = null;
        _skeleton = null;
        _mainMat = null;
        _blGray = false;
    }
}
