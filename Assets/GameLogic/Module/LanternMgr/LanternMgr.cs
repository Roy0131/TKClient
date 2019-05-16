using UnityEngine;
using System;

public class LanternMgr : Singleton<LanternMgr>
{
    public LanternView _lanternView { get; private set; }

    public void ShowLantern(string notice)
    {

        if (_lanternView == null)
        {
            Action<GameObject> OnObjectLoaded = (uiObject) =>
            {
                _lanternView = new LanternView();
                _lanternView.SetDisplayObject(uiObject);
                GameUIMgr.Instance.AddObjectToTopRoot(_lanternView.mRectTransform);
                _lanternView.Show(notice);
            };
            GameResMgr.Instance.LoadUIObjectAsync(SingletonResName.UILantern, OnObjectLoaded);
        }
        else
        {
            _lanternView.Show(notice);
        }
    }

    public void LanternHide()
    {
        _lanternView.Hide();
    }
}
