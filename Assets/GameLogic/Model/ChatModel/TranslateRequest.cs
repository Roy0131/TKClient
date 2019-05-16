using UnityEngine;
using System.Collections;
using Framework.Core;
using UnityEngine.Networking;
using System;

public class TranslateRequest : UpdateBase
{
    private UnityWebRequestAsyncOperation _webRequestAsync;

    private Action<string> _onEnd;
    private float _flTimeOut = 10f;

    public TranslateRequest(Action<string> value)
    {
        _onEnd = value;
    }

    public void StartSend(string txt, string target)
    {
        string value = "text=" + txt + "&" + "target=" + target;
        //WWWForm form = new WWWForm();
        //form.AddField("text", txt);
        //form.AddField("target", target);
        LitJson.JsonData jsonData = new LitJson.JsonData();
        jsonData["text"] = txt;
        jsonData["target"] = target;
        byte[] dataToPut = System.Text.Encoding.UTF8.GetBytes(value);

        //UnityWebRequest webRequest = UnityWebRequest.Put(GameConst.TranslateUrl, dataToPut);
        //_webRequestAsync = webRequest.SendWebRequest();

        UnityWebRequest req = new UnityWebRequest(GameConst.TranslateUrl, "POST");
        //req.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        byte[] postBytes = System.Text.Encoding.Default.GetBytes(jsonData.ToJson());
        req.SetRequestHeader("Content-Type", "application/json");
        req.uploadHandler = new UploadHandlerRaw(postBytes);
        req.downloadHandler = new DownloadHandlerBuffer();
        _webRequestAsync = req.SendWebRequest();
        Initialize();
    }

    public override void Update()
    {
        base.Update();
        if (_webRequestAsync == null)
            return;
        if(_webRequestAsync.isDone)
        {
            if (_webRequestAsync.webRequest.isNetworkError || _webRequestAsync.webRequest.isHttpError)
                OnError();
            else
                OnEnd();
        }else
        {
            if (_flTimeOut <= 0.01f)
                OnError();
            else
                _flTimeOut -= Time.deltaTime;
        }
    }

    private void OnEnd()
    {
        byte[] data = _webRequestAsync.webRequest.downloadHandler.data;
        string value = System.Text.UTF8Encoding.UTF8.GetString(data);//_webRequestAsync.webRequest.downloadHandler.text;
        LogHelper.Log("!finised" + value);
        if (_onEnd != null)
            _onEnd(value);
        Remove();
    }

    protected override void Remove()
    {
        _onEnd = null;
        if (_webRequestAsync != null && _webRequestAsync.webRequest != null)
            _webRequestAsync.webRequest.Abort();
        _webRequestAsync = null;
        base.Remove();
    }

    private void OnError()
    {
        if (_onEnd != null)
            _onEnd(null);
        Remove();
    }
}
