using System;
using System.Collections.Generic;

public class MailEvent
{

    #region net msg event
    /// <summary>
    /// 邮件具体内容
    /// </summary>
    public static readonly string MailDetailBack = "mailDetailBack";
    #endregion

    #region ui event
    /// <summary>
    /// 显示邮件详情 param mail id
    /// </summary>
    public static readonly string ShowMailDetail = "showMailDetail";
    #endregion

    #region Mail Delete
    /// <summary>
    /// 删除邮件 mail id
    /// </summary>
    public static readonly string DeleteMail = "deleteMail";
    #endregion

    #region Mail Send
    /// <summary>
    /// 发送邮件 mail id
    /// </summary>
    public static readonly string SendMail = "sendMail";
    #endregion

    #region Mail Attached
    /// <summary>
    /// 获取邮件附件 mail id item
    /// </summary>
    public static readonly string AttachedItem = "attachedItem";
    #endregion

    #region Mail Refresh
    /// <summary>
    /// 获取邮件附件 mail id item
    /// </summary>
    public static readonly string MailRefresh = "mailRefresh";
    #endregion

    #region Mail New
    /// <summary>
    /// 新邮件
    /// </summary>
    public static readonly string MailNew = "mailNew";
    #endregion

    #region Refresh
    /// <summary>
    /// 获取邮件列表
    /// </summary>
    public static readonly string Refresh = "refresh";
    #endregion
}