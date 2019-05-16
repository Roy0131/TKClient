using System;
using System.IO;
using Google.Protobuf;

public class NetMsgEventHandle
{
    public Type mMsgType { get; private set; }
    public MessageParser mMsgParser { get; private set; }
    public Delegate mCallBack { get; private set; }
    public int mMsgId { get; private set; }

    public NetMsgEventHandle(int msgId, MessageParser parser, Type msgType, Delegate method)
    {
        mMsgType = msgType;
        mMsgParser = parser;
        mMsgId = msgId;
        mCallBack = method;
    }

    public void Dispose()
    {
        mMsgType = null;
        mMsgParser = null;
        mCallBack = null;
    }
}

public class NetMsgRecvData
{
    public Delegate mCallBack;
    public object mData;
    public int mSendMsgID;

    public NetMsgRecvData(Delegate method, object data, int id)
    {
        mCallBack = method;
        mData = data;
        mSendMsgID = id;
    }

    public void Exec()
    {
        //LogHelper.Log(mCallBack.Method.Name);
        if (mCallBack != null)
            mCallBack.Method.Invoke(mCallBack.Target, new object[] { mData });
        mCallBack = null;
    }
}

public class HttpFrom
{
    private string _url;
    private string _datas;

    public HttpFrom(string url)
    {
        _url = url;
        _datas = "";
    }

    public void AddValue(string key, string value)
    {
        if (string.IsNullOrEmpty(_datas))
            _datas = key + "=" + value;
        else
            _datas += ("&" + key + "=" + value);
    }

    public void AddValue(string key, int value)
    {
        AddValue(key, value.ToString());
    }

    public void AddValue(string key, float value)
    {
        AddValue(key, value.ToString());
    }

    public override string ToString()
    {
        return _url + "?" + _datas;
    }
}

public class NetPacketHelper
{
    public static NetMsgRecvData Read(int msgId, byte[] bts, NetMsgEventHandle handle, int sendMsgID)
    {
        MemoryStream ms = new MemoryStream(bts);
        ms.SetLength(bts.Length);

        NetMsgRecvData data = null;
        try
        {
            IMessage md = handle.mMsgParser.ParseFrom(bts);
            data = new NetMsgRecvData(handle.mCallBack, md, sendMsgID);
        }
        catch (Exception ex)
        {
            LogHelper.Log("[NetPacketHelper.Read() => 反序列化数据出错，ex:" + ex + "]");
        }
        ms.Close();
        return data;
    }
}