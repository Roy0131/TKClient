
using Msg.ClientMessage;
using System;

public class LoginPostRequest : GameRequestBase
{
    public LoginPostRequest(string url, Action<object> onComplete, Action onError)
        : base(url, onComplete, onError)
    {

    }

    protected override void DoParseData()
    {
        if (string.IsNullOrEmpty(_webRequestAsync.webRequest.downloadHandler.text))
        {
            LogHelper.LogError("[LoginPostRequest.DoParseData() => get data was empty!!!]");
            CheckReSend();
            return;
        }
        byte[] data = _webRequestAsync.webRequest.downloadHandler.data;
        S2C_ONE_MSG pbMsgData = S2C_ONE_MSG.Parser.ParseFrom(data);
        _status = HttpsStatus.None;
        if (pbMsgData.ErrorCode < NetErrorCode.None)
        {
            LoginHelper.DoLoginNetError(pbMsgData.ErrorCode);
            //if (_onErrMethod != null)
                //_onErrMethod.Invoke();
        }
        else
        {
            if (_onCompleteMethod != null)
                _onCompleteMethod.Invoke(pbMsgData);
        }
    }
}