using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Msg.ClientMessage;

public class MailDataVO : DataBaseVO
{
    public MailBasicData mMailBasicData { get; private set; }

    public MailDetail mMailDetailData { get; private set; }

    protected override void OnInitData<T>(T value)
    {
        mMailBasicData = value as MailBasicData;
    }

    public void SetMailDetailData(MailDetail value)
    {
        mMailDetailData = value;
    }

    public void RefreshAttached()
    {
        mMailBasicData.IsRead = true;
        mMailBasicData.IsGetAttached = true;
    }

    public void RefreshData()
    {
        mMailBasicData.IsRead = true;
    }
}
