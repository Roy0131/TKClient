using UnityEngine;
using UnityEngine.UI;
using System;

public class ModuleBase : UISoundViewBase
{
    public ModuleID mModuleID { get; protected set; }
    public UILayer mLayer { get; protected set; }

    protected string _modelResName;
    protected string _soundName;
    private Button _btn;
    public bool mBlNeedBackMask { get; protected set; }

    public bool mBlStack { get; protected set; }

    public string ModuleResName
    {
        get { return _modelResName; }
    }

    public ModuleBase(ModuleID id, UILayer layer)
    {
        mModuleID = id;
        mLayer = layer;
        mBlStack = true;
        mBlNeedBackMask = false;
    }

    public override void Show(params object[] args)
    {
        Action OnShow = () =>
        {
            ShowChildren(args);
            base.Show(args);
        };
        OnShowAnimator();
        if (!string.IsNullOrEmpty(_soundName) && !mBlShow)
            SoundMgr.Instance.PlayEffectSound(_soundName);
        OnShow();
        _btn = Find<Button>("BlackBack");
    }

    public override void Hide()
    {
        if (_childrenViews.Count > 0)
        {
            for (int i = 0; i < _childrenViews.Count; i++)
                _childrenViews[i].Hide();
        }

        DGHelper.DoKill(mRectTransform);
        base.Hide();
    }

    protected void LoadRes()
    {

    }

    public void InitModuleObject(GameObject uiObject)
    {
        SetDisplayObject(uiObject);
        GameUIMgr.Instance.AddModuleToStage(this);
    }

    public override void Dispose()
    {
        if (_childrenViews != null)
        {
            for (int i = _childrenViews.Count - 1; i >= 0; i--)
                _childrenViews[i].Dispose();
            _childrenViews.Clear();
            _childrenViews = null;
        }
        base.Dispose();
    }

    protected virtual void OnClose()
    {
        GameUIMgr.Instance.CloseModule(mModuleID);
    }

    protected virtual void OnShowAnimator()
    {

    }
   
    protected virtual void OnHideAnimator() { }

}
