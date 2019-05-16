using UnityEngine;
using System;

public class MailSendMgr : Singleton<MailSendMgr>
{
    private MailSendView _mailSend;

    public void ShowMailSend(int recriverId, string sendName, int mailType = 2)
    {
        if (_mailSend == null)
        {
            Action<GameObject> OnObjectLoaded = (uiObject) =>
            {
                _mailSend = new MailSendView();
                _mailSend.SetDisplayObject(uiObject);
                GameUIMgr.Instance.AddObjectToTopRoot(_mailSend.mRectTransform);
                _mailSend.Show(recriverId, sendName, mailType);
            };

            GameResMgr.Instance.LoadUIObjectAsync(SingletonResName.UIMailSend, OnObjectLoaded);
        }
        else
        {
            _mailSend.Show(recriverId, sendName, mailType);
        }
    }
}
