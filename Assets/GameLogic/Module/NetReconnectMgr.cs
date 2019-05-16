
using UnityEngine;
using UnityEngine.UI;

public class NetReconnectMgr : Singleton<NetReconnectMgr>
{
    public GameObject _reconnectObject;
    private bool _blShow = false;
    public void ShowRecconect()
    {
        if (_blShow)
            return;
        if (_reconnectObject == null)
        {
            _reconnectObject = GameResMgr.Instance.LoadUIObjectSync(SingletonResName.UIRecconetTips);
            _reconnectObject.transform.SetParent(GameUIMgr.Instance.mGuideRoot, false);
            _reconnectObject.transform.SetAsLastSibling();
        }
        _blShow = true;
        _reconnectObject.transform.Find("Text").GetComponent<Text>().text = LanguageMgr.GetLanguage(6001270);
        _reconnectObject.SetActive(true);
        OnSendLogin();
    }

    private uint _key = 0;
    private void OnLoginFailed()
    {
        if (_key != 0)
            TimerHeap.DelTimer(_key);
        _key = TimerHeap.AddTimer(5000, 0, OnSendLogin);
    }

    private void OnSendLogin()
    {
        LoginHelper.ReLogin(LocalDataMgr.PlayerAccount, LocalDataMgr.Password, LocalDataMgr.LoginChannel, OnLoginFailed, false);
    }

    public void HideReconnect()
    {
        if (_key != 0)
            TimerHeap.DelTimer(_key);
        _key = 0;
        if (!_blShow)
            return;
        _blShow = false;
        if (_reconnectObject == null)
            return;
        _reconnectObject.gameObject.SetActive(false);
    }
}