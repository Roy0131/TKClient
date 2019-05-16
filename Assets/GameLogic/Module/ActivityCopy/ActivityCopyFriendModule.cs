using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public class ActivityCopyFriendModule : ModuleBase {

    private Button _btnClose;
    private Transform _root;

    public ActivityCopyFriendModule() : base(ModuleID.ActivityFriend, UILayer.Popup)
    {
        _modelResName = UIModuleResName.UI_ActivityFriend;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _root = Find<Transform>("Root");
        _btnClose = Find<Button>("Root/ButtonClose");
        ColliderHelper.SetButtonCollider(_btnClose.transform);

        ActivityCopyFriendView activityCopyFriendView = new ActivityCopyFriendView();
        activityCopyFriendView.SetDisplayObject(Find("Root/Content"));
        AddChildren(activityCopyFriendView);

        _btnClose.onClick.Add(OnClose);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(ActivityCopyEvent.ActivityCopyFriendClose, OnClose);
    }

    protected override void RemoveEvent()
    {
        base.AddEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(ActivityCopyEvent.ActivityCopyFriendClose, OnClose);
    }

    protected override void OnShowAnimator()
    {
        base.OnShowAnimator();
        ObjectHelper.PopAnimationLiner(_root);
    }
}
