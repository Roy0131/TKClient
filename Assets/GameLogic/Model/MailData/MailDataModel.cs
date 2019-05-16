using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Msg.ClientMessage;
using System.Linq;

public class MailTypeConst
{
    public const int SYSTEM = 1;//系统邮件
    public const int PLAYERS = 2;//玩家邮件
    public const int GUILD = 3;//联盟邮件
}

public class MailDataModel : ModelDataBase<MailDataModel>
{
    private const string Maildata = "maildata";
    private Dictionary<int, MailDataVO> mAllMails = new Dictionary<int, MailDataVO>();

    public void ReqMailList()
    {
        if (CheckNeedRequest(Maildata))
            GameNetMgr.Instance.mGameServer.ReqMailList(); //req mail list function
        else
            Instance.DispathEvent(MailEvent.Refresh);
    }

    private void OnListMail(S2CMailListResponse value)
    {
        Instance.mAllMails.Clear();
        MailDataVO vo;
        for(int i = 0; i < value.Mails.Count; i++)
        {
            vo = new MailDataVO();
            vo.InitData(value.Mails[i]);
            mAllMails.Add(vo.mMailBasicData.Id,vo);
        }
        Instance.AddLastReqTime(Maildata);
        Instance.DispathEvent(MailEvent.Refresh);
    }

    private void OnDetaMail(S2CMailDetailResponse value)
    {
        for (int i = 0; i < value.Mails.Count; i++)
        {
            mAllMails[value.Mails[i].Id].RefreshData();
            if (mAllMails.ContainsKey(value.Mails[i].Id))
                mAllMails[value.Mails[i].Id].SetMailDetailData(value.Mails[i]);
        }
        OnRedPoint();
        DispathEvent(MailEvent.MailDetailBack);
    }

    public List<MailDataVO> GetMailDataVOType(int mailType)
    {
        List<MailDataVO> result = new List<MailDataVO>();
        foreach (MailDataVO vo in mAllMails.Values)
        {
            if (mailType== MailTypeConst.SYSTEM)
            {
                if (vo.mMailBasicData.Type == MailTypeConst.SYSTEM)
                    result.Add(vo);
            }
            else
            {
                if (vo.mMailBasicData.Type != MailTypeConst.SYSTEM)
                    result.Add(vo);
            }
        }
        return result;
    }

    private void OnDeleteMail(S2CMailDeleteResponse value)
    {
        List<int> listMailID = new List<int>();
        for (int i = 0; i < value.MailIds.Count; i++)
        {
            if (!listMailID.Contains(value.MailIds[i]))
                listMailID.Add(value.MailIds[i]);
            if (mAllMails.ContainsKey(value.MailIds[i]))
                mAllMails.Remove(value.MailIds[i]);
        }
        DispathEvent(MailEvent.DeleteMail,listMailID);
    }

    private void OnSendMail(S2CMailSendResponse value)
    {
        DispathEvent(MailEvent.SendMail,value.MailId);
    }

    private void OnAttachedMail(S2CMailGetAttachedItemsResponse value)
    {
        for (int i = 0; i < value.MailIds.Count; i++)
        {
            mAllMails[value.MailIds[i]].RefreshAttached();
        }
        List<ItemInfo> listInfo = new List<ItemInfo>();
        listInfo.AddRange(value.AttachedItems);
        List<int> listID = new List<int>();
        listID.AddRange(value.MailIds);
        OnRedPoint();
        MailEventVO vo = new MailEventVO();
        vo.mlstItems = listInfo;
        vo.mlstIds = listID;
        DispathEvent(MailEvent.AttachedItem, vo);//, listID, listInfo);
    }

    private void OnMailNew(S2CMailsNewNotify value)
    {
        RedPointDataModel.Instance.SetRedPointDataState(RedPointEnum.Mail, true);
        MailDataVO vo;
        for (int i = 0; i < value.Mails.Count; i++)
        {
            vo = new MailDataVO();
            vo.InitData(value.Mails[i]);
            mAllMails.Add(vo.mMailBasicData.Id, vo);
        }
        DispathEvent(MailEvent.MailNew);
    }

    private void OnRedPoint()
    {
        foreach (MailDataVO vo in mAllMails.Values)
        {
            if (vo.mMailBasicData.IsRead == false)
                return;
        }
        RedPointDataModel.Instance.SetRedPointDataState(RedPointEnum.Mail, false);
    }

    //获取邮件列表
    public static void DoListMail(S2CMailListResponse value)
    {
        Instance.OnListMail(value);
    }

    //发送邮件
    public static void DoSendMail(S2CMailSendResponse value)
    {
        Instance.OnSendMail(value);
    }
    //请求邮件具体内容
    public static void DoDetaMail(S2CMailDetailResponse value)
    {
        Instance.OnDetaMail(value);
    }
    //获取邮件附加物品
    public static void DoGetAttachedItem(S2CMailGetAttachedItemsResponse value)
    {
        Instance.OnAttachedMail(value);
    }
    //删除邮件
    public static void DoDelete(S2CMailDeleteResponse value)
    {
        Instance.OnDeleteMail(value);
    }

    public static void DoMailNew(S2CMailsNewNotify value)
    {
        Instance.OnMailNew(value);
    }

    protected override void DoClearData()
    {
        base.DoClearData();
        if (mAllMails != null)
            mAllMails.Clear();
    }
}
