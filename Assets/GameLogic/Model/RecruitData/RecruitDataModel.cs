using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Msg.ClientMessage;

public class RecruitDataModel : ModelDataBase<RecruitDataModel>
{
    public List<RecruitDataVO> mAllRecruits { get; private set; }
    public List<int> rewardId { get; private set; }

    protected override void AddEvent()
    {
        base.AddEvent();
        BagDataModel.Instance.AddEvent<List<int>>(BagEvent.BagItemRefresh, OnRefresh);
        HeroDataModel.Instance.AddEvent(HeroEvent.HeroInfoChange, OnRefresh);
    }

    private void OnRefresh()
    {
        if (mAllRecruits == null)
            return;
        for (int i = 0; i < mAllRecruits.Count; i++)
            mAllRecruits[i].GetRecruitID(i);
        DispathEvent(RecruitEvent.HeroRefreshs);
    }

    private void OnRefresh(List<int> listId)
    {
        if (mAllRecruits == null)
            return;
        for (int i = 0; i < mAllRecruits.Count; i++)
            mAllRecruits[i].GetRecruitID(i);
        DispathEvent(RecruitEvent.BagRefresh);
    }

    public void ReqDrawCardList()
    {
        if (mAllRecruits == null)
            GameNetMgr.Instance.mGameServer.ReqDrawData();
        else
            GameUIMgr.Instance.OpenModule(ModuleID.Recruit);
    }

    private void OnDrawData(S2CDrawDataResponse value)
    {
        mAllRecruits = new List<RecruitDataVO>();
        RecruitDataVO vo;
        for (int i = 0; i < 3; i++)
        {
            vo = new RecruitDataVO();
            vo.GetRecruitID(i);
            for (int j = 0; j < value.FreeDrawRemainSeconds.Count; j++)
            {
                if (value.FreeDrawRemainSeconds[j].Id == (i * 2 + 1))
                    vo.GetTime(value.FreeDrawRemainSeconds[j].Value);
            }
            mAllRecruits.Add(vo);
        }
        GameUIMgr.Instance.OpenModule(ModuleID.Recruit);
    }

    public RecruitDataVO GetDrawCardIdVO(int drawId)
    {
        if (drawId >= 0 && drawId < mAllRecruits.Count)
            return mAllRecruits[drawId];
        return null;
    }

    private void OnDrawCard(S2CDrawCardResponse value)
    {
        List<int> tabId = new List<int>();
        rewardId = new List<int>();
        for (int i = 0; i < value.RoleTableId.Count / 2; i++)
        {
            tabId.Add(value.RoleTableId[i * 2]);
        }
        rewardId.AddRange(value.RoleTableId);
        //tabId.AddRange(value.RoleTableId);
        if (value.DrawType == 1 && value.IsFreeDraw)
            RedPointDataModel.Instance.SetRedPointDataState(RedPointEnum.NormalDraw, false);
        if (value.DrawType == 3 && value.IsFreeDraw)
            RedPointDataModel.Instance.SetRedPointDataState(RedPointEnum.AdvanceDraw, false);
        DispathEvent(RecruitEvent.DrawCard, value.DrawType, tabId, value.IsFreeDraw);
    }

    public static void DoDrawData(S2CDrawDataResponse value)
    {
        Instance.OnDrawData(value);
    }

    public static void DoDrawCard(S2CDrawCardResponse value)
    {
        Instance.OnDrawCard(value);
    }

    protected override void DoClearData()
    {
        base.DoClearData();
        if (mAllRecruits != null)
        {
            mAllRecruits.Clear();
            mAllRecruits = null;
        }
    }
}
