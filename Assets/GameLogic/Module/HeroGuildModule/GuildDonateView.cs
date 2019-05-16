using System;
using Msg.ClientMessage;
using UnityEngine;
using Framework.UI;

public class GuildDonateView : UILoopBaseView<GuildDonateVO>
{
    private GameObject _noObj;
    private GameObject _itemObject;
    private RectTransform _contentRect;
    protected override void ParseComponent()
    {
        base.ParseComponent();
        _noObj = Find("No");
        _itemObject = Find("ScrollView/Content/DonateItem");
        InitScrollRect("ScrollView");
        _contentRect = Find<RectTransform>("ScrollView/Content");
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GuildDataModel.Instance.AddEvent(GuildEvent.GuildDonateListRefresh, OnRefreshDonate);
        GuildDataModel.Instance.AddEvent<bool>(GuildEvent.GuildDonateRefresh, OnDonateRefresh);
        GuildDataModel.Instance.AddEvent<int,int>(GuildEvent.GuildDonateReward, OnDonateReward);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GuildDataModel.Instance.RemoveEvent(GuildEvent.GuildDonateListRefresh, OnRefreshDonate);
        GuildDataModel.Instance.RemoveEvent<bool>(GuildEvent.GuildDonateRefresh, OnDonateRefresh);
        GuildDataModel.Instance.RemoveEvent<int,int>(GuildEvent.GuildDonateReward, OnDonateReward);
    }

    private void OnDonateReward(int id,int num)
    {
        GuildDonateConfig guildCfg = GameConfigMgr.Instance.GetGuildDonateConfig(id);
        string[] rewardItem = guildCfg.DonateRewardItem.Split(',');
        ItemInfo info = new ItemInfo();
        info.Id =Convert.ToInt32(rewardItem[0]);
        info.Value = Convert.ToInt32(rewardItem[1]);
        RewardTipsMgr.Instance.ShowTips(info);
    }

    private void OnDonateRefresh(bool over)
    {
        if (over)
            OnRefreshDonate();
    }

    private void OnRefreshDonate()
    {
        _lstDatas = GuildDataModel.Instance.mlstDonateDatas;
        _lstDatas.Sort((a, b) => a.mAskTime.CompareTo(b.mAskTime));
        _loopScrollRect.ClearCells();
        _noObj.SetActive(_lstDatas.Count == 0);
        if (_lstDatas.Count == 0)
            return;
        _loopScrollRect.totalCount = _lstDatas.Count;
        _loopScrollRect.RefillCells();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        GuildDataModel.Instance.ReqGuildDonateData();
    }

    protected override UIBaseView CreateItemView()
    {
        GuildDonateItem item = new GuildDonateItem();
        item.SetDisplayObject(GameObject.Instantiate(_itemObject));
        return item;
    }
}