using UnityEngine;
using System;

public class FunctionMgr : Singleton<FunctionMgr>
{    
    private FunctionView _functionView;
    public void ShowFeature(SystemUnlockConfig cfg)
    {
        if (cfg.NameID > 0)
        {
            if (_functionView == null)
            {
                Action<GameObject> OnObjectLoaded = (uiObject) =>
                {
                    _functionView = new FunctionView();
                    _functionView.SetDisplayObject(uiObject);
                    GameUIMgr.Instance.AddObjectToTopRoot(_functionView.mRectTransform);
                    _functionView.Show(cfg);
                };

                GameResMgr.Instance.LoadUIObjectAsync(SingletonResName.UIFunction, OnObjectLoaded);
            }
            else
            {
                _functionView.Show(cfg);
            }
        }
    }
}
