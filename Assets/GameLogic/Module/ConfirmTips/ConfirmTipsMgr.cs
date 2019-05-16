using UnityEngine;
using System;

public class ConfirmTipsMgr : Singleton<ConfirmTipsMgr>
{
    private ConfirmTipsView _confirmTips;

    private void InitConfirmView()
    {
        if (_confirmTips == null)
        {
            _confirmTips = new ConfirmTipsView();
            GameObject obj = GameResMgr.Instance.LoadUIObjectSync(SingletonResName.UIConfirm);
            _confirmTips.SetDisplayObject(obj);
            _confirmTips.mRectTransform.SetParent(GameUIMgr.Instance.mGuideRoot, false);
        }
        _confirmTips.mRectTransform.SetAsLastSibling();
    }

    public void ShowConfirmTips(string content, Action<bool, bool> callBack, bool showAgain = false)
    {
        InitConfirmView();
        _confirmTips.SetParam(callBack, showAgain);
        _confirmTips.Show(content);
    }

    public void HideConfirmTips()
    {
        if (_confirmTips == null)
            return;
        _confirmTips.Hide();
    }
}
