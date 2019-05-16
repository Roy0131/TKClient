using UnityEngine;
using System;

public class PlayerLevelUpMgr : Singleton<PlayerLevelUpMgr>
{
    private PlayerLevelUpView _playerLevelUpView;

    public void ShowLevelUp(int Level)
    {

        if (_playerLevelUpView == null)
        {
            Action<GameObject> OnObjectLoaded = (uiObject) =>
            {
                _playerLevelUpView = new PlayerLevelUpView();
                _playerLevelUpView.SetDisplayObject(uiObject);
                GameUIMgr.Instance.AddObjectToTopRoot(_playerLevelUpView.mRectTransform);
                _playerLevelUpView.Show(Level);
            };
            GameResMgr.Instance.LoadUIObjectAsync(SingletonResName.UILevelUp, OnObjectLoaded);
        }
        else
        {
            _playerLevelUpView.Show(Level);
        }
    }

    public void LevelUpHide()
    {
        _playerLevelUpView.Hide();
    }
}
