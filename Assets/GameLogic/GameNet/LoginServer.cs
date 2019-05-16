using Google.Protobuf;
using Msg.ClientMessage;
using Msg.ClientMessageId;

public class LoginServer : ServerBase
{
    protected override void InitNetMsgHandle()
    {
        base.InitNetMsgHandle();

        RegisterNetMsgType<S2CLoginResponse>((int)MSGID.S2CLoginResponse, S2CLoginResponse.Parser, LoginHelper.OnLoginResponse);
        RegisterNetMsgType<S2CSelectServerResponse>((int)MSGID.S2CSelectServerResponse, S2CSelectServerResponse.Parser, LoginHelper.OnSelectedResponse);
        RegisterNetMsgType<S2CSetLoginPasswordResponse>((int)MSGID.S2CSetLoginPasswordResponse, S2CSetLoginPasswordResponse.Parser, LoginHelper.OnSetPsdResponse);
        RegisterNetMsgType<S2CGuestBindNewAccountResponse>((int)MSGID.S2CGuestBindNewAccountResponse, S2CGuestBindNewAccountResponse.Parser, LoginHelper.OnBindAccResponse);
    }

    public void ReqBindAccount(int sid, string oldAcc, string oldPsd, string newAcc, string newPsd, int channel)
    {
        C2SGuestBindNewAccountRequest req = new C2SGuestBindNewAccountRequest();
        req.ServerId = sid;
        req.Account = oldAcc;
        req.Password = oldPsd;
        req.NewAccount = newAcc;
        req.NewPassword = newPsd;
        req.NewChannel = GameLoginType.GetChannelValue(channel);
        Send(MSGID.C2SGuestBindNewAccountRequest, req.ToByteString());
    }

    public void ReqSetLoginPsd(string acc, string oldPsd, string newPsd)
    {
        C2SSetLoginPasswordRequest req = new C2SSetLoginPasswordRequest();
        req.Account = acc;
        req.Password = oldPsd;
        req.NewPassword = newPsd;
        Send(MSGID.C2SSetLoginPasswordRequest, req.ToByteString());
    }

    public void ReqSelectServer(string acc, string token, int sid)
    {
        C2SSelectServerRequest req = new C2SSelectServerRequest();
        req.Acc = acc;
        req.Token = token;
        req.ServerId = sid;
        Send(MSGID.C2SSelectServerRequest, req.ToByteString());
    }

    public void ReqLogin(string acc, string psd, int channel)
    {
        C2SLoginRequest req = new C2SLoginRequest();
        req.Acc = acc;
        req.Password = psd;
        req.ClientOS = FileConst.RunPlatform;
        req.Channel = GameLoginType.GetChannelValue(channel);
        req.IsAppleVerifyUse = !GameDriver.Instance.mBlRunHot;
        Send(MSGID.C2SLoginRequest, req.ToByteString());
    }

    protected override void DoRequestSend()
    {
        _curRequest = new LoginPostRequest(GameNetMgr.Instance.mGameLoginUrl, OnReceiveData, OnDataError);
        C2S_ONE_MSG pbData = _postDataPools.Dequeue();
        _curRequest.InitPostData(pbData.ToByteArray());
        _curRequest.Send();
    }

    protected override void OnDataError()
    {
        base.OnDataError();
        if (_lstRequestID != null)
            _lstRequestID.Clear();
        LoginHelper.DoLoginNetError(-1);
    }

    protected override void OnReceiveData(object value)
    {
        S2C_ONE_MSG pbMsg = value as S2C_ONE_MSG;
        if (DictNetHandle.ContainsKey(pbMsg.MsgCode))
        {
            NetMsgEventHandle handle = DictNetHandle[pbMsg.MsgCode];
            NetMsgRecvData data = NetPacketHelper.Read(pbMsg.MsgCode, pbMsg.Data.ToByteArray(), handle, _lstRequestID[0]);
            if (data != null)
                _lstRecvDatas.Add(data);
        }
        else
        {
            LogHelper.LogWarning("[ServerBase.ProccessOneMsg() <= 消息号:" + pbMsg.MsgCode + "未注册]");
        }
        //ProccessOneMsg(data as S2C_ONE_MSG);
    }
}

