using Framework.UI;
using UnityEngine;
using UnityEngine.UI;

public class GuildBossHurtView : UILoopBaseView<GuildBossHurtVO>
{
    private Button _disBtn;
    private GameObject _rankItem;
    private int _bossId;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _disBtn = Find<Button>("Hurt/CloseBtn");
        _rankItem = Find("Hurt/Panel_Scroll/KnapsackPanel/RankItem");
        InitScrollRect("Hurt/Panel_Scroll");

        _disBtn.onClick.Add(OnDis);

        ColliderHelper.SetButtonCollider(_disBtn.transform, 120, 120);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GuildBossDataModel.Instance.AddEvent<int>(GuildEvent.ReqBossDmg, OnBossDmg);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GuildBossDataModel.Instance.RemoveEvent<int>(GuildEvent.ReqBossDmg, OnBossDmg);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        GameNetMgr.Instance.mGameServer.ReqStageDamage(int.Parse(args[0].ToString()));
    }

    private void OnBossDmg(int bossId)
    {
        _bossId = bossId;
        _lstDatas = GuildBossDataModel.Instance.HurtVO(bossId);
        _loopScrollRect.ClearCells();
        if (_lstDatas == null)
            return;
        _loopScrollRect.totalCount = _lstDatas.Count;
        _loopScrollRect.RefillCells();
    }

    protected override UIBaseView CreateItemView()
    {
        GuildBossHurtItemView item = new GuildBossHurtItemView();
        item.SetDisplayObject(GameObject.Instantiate(_rankItem));
        return item;
    }

    protected override void SetItemData(UIBaseView view, int idx)
    {
        view.Show(_bossId, _lstDatas[idx]);
    }

    private void OnDis()
    {
        Hide();
    }
}
