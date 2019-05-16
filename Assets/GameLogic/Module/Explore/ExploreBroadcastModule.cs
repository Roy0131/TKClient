using UnityEngine.UI;

public class ExploreBroadcastModule : ModuleBase
{
    private Button _btnClose;
    //private ExploreBroadcastView _view;
    public ExploreBroadcastModule() : base(ModuleID.ExploreBroadcast, UILayer.Popup)
    {
        _modelResName = UIModuleResName.UI_ExploreBroadcast;
    }
    protected override void AddEvent()
    {
        base.AddEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(ExploreEvent.ExploreTaskClear, OnClose);
    }
    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(ExploreEvent.ExploreTaskClear, OnClose);
    }
    protected override void ParseComponent()
    {
        base.ParseComponent();
        _btnClose = Find<Button>("ButtonBack");
        ColliderHelper.SetButtonCollider(_btnClose.transform);
        _btnClose.onClick.Add(OnClose);
        ExploreBroadcastView view = new ExploreBroadcastView();
        view.SetDisplayObject(Find("Content"));
        AddChildren(view);
    }
}
