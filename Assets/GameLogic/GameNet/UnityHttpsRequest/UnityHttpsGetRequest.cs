using LitJson;
using UnityEngine.Networking;
using System;

public class UnityHttpsGetRequest : GameRequestBase
{
    public UnityHttpsGetRequest(string url, Action<object> onCompleteMethod, Action onErrMethod = null)
        : base(url, onCompleteMethod, onErrMethod)
    {

    }

    protected override UnityWebRequestAsyncOperation DoSend()
    {
        UnityWebRequest req = UnityWebRequest.Get(_url);
        return req.SendWebRequest();
    }

    protected override void DoParseData()
    {
        if (string.IsNullOrEmpty(_webRequestAsync.webRequest.downloadHandler.text))
        {
            LogHelper.LogError("[UnityHttpsGetRequest.DoParseData() => get data was empty!!!]");
            CheckReSend();
            return;
        }
        JsonData jd = JsonMapper.ToObject(_webRequestAsync.webRequest.downloadHandler.text);
        if (_onCompleteMethod != null)
            _onCompleteMethod.Invoke(jd);
        Dispose();
    }
}