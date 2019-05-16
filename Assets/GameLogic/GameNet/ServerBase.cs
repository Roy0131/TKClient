using System;
using Google.Protobuf;
using Msg.ClientMessage;
using Msg.ClientMessageId;
using System.Collections.Generic;
using Framework.Core;

public abstract class ServerBase : IUpdateable, IDispose
{

    #region getter
    private Dictionary<int, NetMsgEventHandle> _dictNetHandle;

    public Dictionary<int, NetMsgEventHandle> DictNetHandle
    {
        get { return _dictNetHandle; }
    }

    protected bool _blEnable;
    public bool blEnable
    {
        get { return _blEnable; }
    }
    #endregion

    protected ByteStream _recvStream;
    protected List<NetMsgRecvData> _lstRecvDatas;
    protected List<int> _lstRequestID;
    protected GameRequestBase _curRequest = null;

    public ServerBase()
    {
        _recvStream = new ByteStream();
        _lstRecvDatas = new List<NetMsgRecvData>();
        _lstRequestID = new List<int>();
        InitNetMsgHandle();
    }

    protected virtual void InitNetMsgHandle()
    {
        _blEnable = true;
        _dictNetHandle = new Dictionary<int, NetMsgEventHandle>();
    }

    protected void RegisterNetMsgType<T>(int msgId, MessageParser msgParser, Action<T> method) where T : IMessage
    {
        if (_dictNetHandle.ContainsKey(msgId))
        {
            LogHelper.LogError("[ServerBase.RegisterNetMsgType() => msgId:" + msgId + "重复注册]");
            return;
        }
        _dictNetHandle.Add(msgId, new NetMsgEventHandle(msgId, msgParser, typeof(T), method));
    }

    protected virtual void OnDataError()
    {
        if (_curRequest != null)
        {
            _curRequest.Dispose();
            _curRequest = null;
        }
    }

    protected void ProccessOneMsg(S2C_ONE_MSG value)
    {
        _recvStream.Clear();
        byte[] bytes = value.Data.ToByteArray();
        _recvStream.AddBytes(bytes);
        while (_recvStream.BytesAvailable >= 4)
        {
            int msgId = _recvStream.ReadInt(); //消息id
            int msgLen = _recvStream.ReadInt(); //消息长度
            byte[] msgData = _recvStream.ReadBytes(msgLen);
            if (DictNetHandle.ContainsKey(msgId))
            {
                NetMsgEventHandle handle = DictNetHandle[msgId];
                NetMsgRecvData data = NetPacketHelper.Read(msgId, msgData, handle, value.MsgCode);
                if (data != null)
                    _lstRecvDatas.Add(data);
            }
            else
            {
                LogHelper.LogWarning("[ServerBase.ProccessOneMsg() <= 消息号:" + msgId + "未注册]");
            }
        }
    }

    protected Queue<C2S_ONE_MSG> _postDataPools = new Queue<C2S_ONE_MSG>();
    protected virtual void Send(MSGID msgId, ByteString data)
    {
        int id = (int)msgId;
        if (_lstRequestID.Contains(id))
            return;
        _lstRequestID.Add(id);
        C2S_ONE_MSG pd = new C2S_ONE_MSG();
        pd.MsgCode = id;
        pd.Data = data;
        _postDataPools.Enqueue(pd);
        CheckSendData();
    }

    public virtual void Update()
    {
        NetMsgRecvData recvData = null;
        while ((recvData = GetRecvDate()) != null)
        {
            recvData.Exec();
            if (_lstRequestID.Contains(recvData.mSendMsgID))
                _lstRequestID.Remove(recvData.mSendMsgID);
        }
        CheckSendData();
    }

    private void CheckSendData()
    {
        if (_curRequest != null && !_curRequest.BlEnd)
            return;
        if (_curRequest != null)
        {
            _curRequest.Dispose();
            _curRequest = null;
        }
        if (_postDataPools.Count <= 0)
        {
            _curRequest = null;
            _lstRequestID.Clear();
            return;
        }
        DoRequestSend();
    }

    protected abstract void OnReceiveData(object data);
    protected abstract void DoRequestSend();

    private NetMsgRecvData GetRecvDate()
    {
        if (_lstRecvDatas == null || _lstRecvDatas.Count == 0)
            return null;
        NetMsgRecvData data = _lstRecvDatas[0];
        _lstRecvDatas.RemoveAt(0);
        return data;
    }

    public virtual void Dispose()
    {
        if (_dictNetHandle != null)
        {
            foreach (var kv in _dictNetHandle)
                kv.Value.Dispose();
            _dictNetHandle.Clear();
            _dictNetHandle = null;
        }
        if (_lstRecvDatas != null)
        {
            _lstRecvDatas.Clear();
            _lstRecvDatas = null;
        }

        if (_lstRequestID != null)
        {
            _lstRequestID.Clear();
            _lstRequestID = null;
        }

        if (_postDataPools != null)
        {
            _postDataPools.Clear();
            _postDataPools = null;
        }
        if (_curRequest != null)
        {
            _curRequest.Dispose();
            _curRequest = null;
        }
    }
}
