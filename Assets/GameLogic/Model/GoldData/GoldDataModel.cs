using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Msg.ClientMessage;

public class GoldDataModel : ModelDataBase<GoldDataModel>
{
    private const string GoldData = "goldData";
    public List<GoldDataVO> mAllGold { get; private set; }
    private int _time;

    public void ReqDoldData()
    {
        if (CheckNeedRequest(GoldData))
            GameNetMgr.Instance.mGameServer.ReqGoldData();
        else
            DispathEvent(GoldEvent.GoldData, mAllGold);
    }

    private void OnGoldData(S2CGoldHandDataResponse value)
    {
        _time = (int)Time.realtimeSinceStartup + value.RemainRefreshSeconds;
        if (mAllGold != null)
            mAllGold.Clear();
        mAllGold = new List<GoldDataVO>();
        GoldDataVO vo;
        for (int i = 0; i < 3; i++)
        {
            vo = new GoldDataVO();
            vo.GetGoldID(value.LeftNums[i].Id, value.LeftNums[i].Value);
            mAllGold.Add(vo);
        }
        DispathEvent(GoldEvent.GoldData, mAllGold);
    }

    public int GoldTime
    {
        get { return _time - (int)Time.realtimeSinceStartup; }
    }

    private void OnGoldTou(S2CTouchGoldResponse value)
    {
        for (int i = 0; i < mAllGold.Count; i++)
        {
            if (mAllGold[i].mGoldIndex == value.Type)
                mAllGold[i].OnLeftNum();
        }
        GoldState();
        DispathEvent(GoldEvent.GoldTou, value.GetGold);
    }

    private void GoldState()
    {
        for (int i = 0; i < mAllGold.Count; i++)
        {
            if (mAllGold[i].mGoldIndex == 1)
            {
                if (mAllGold[i].mLeftNum <= 0)
                    RedPointDataModel.Instance.SetRedPointDataState(RedPointEnum.GoldHand, false);
            }
        }  
    }

    public static void DoGoldTou(S2CTouchGoldResponse value)
    {
        Instance.OnGoldTou(value);
    }

    public static void DoGoldData(S2CGoldHandDataResponse value)
    {
        Instance.OnGoldData(value);
    }

    protected override void DoClearData()
    {
        base.DoClearData();
        if (mAllGold != null)
            mAllGold.Clear();
    }
}
