using Msg.ClientMessage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExploreGroupModule : ModuleBase
{
    private Button _btnClose;//关闭按钮
    private ExploreGroupView _exploreGroupView;
    private GameObject _content;

    public ExploreGroupModule() : base(ModuleID.ExploreGroup, UILayer.Popup)
    {
        _modelResName = UIModuleResName.UI_ExploreGroup;
    }
    
    protected override void ParseComponent()
    {
        base.ParseComponent();
        _btnClose = Find<Button>("ButtonBack");
        ColliderHelper.SetButtonCollider(_btnClose.transform);
        _content = Find("Content");

        _exploreGroupView = new ExploreGroupView();
        _exploreGroupView.SetDisplayObject(_content);
        AddChildren(_exploreGroupView);

        _btnClose.onClick.Add(OnClose);

    }
    protected override void AddEvent()
    {
        base.AddEvent();
        ExploreDataModel.Instance.AddEvent<List<int>>(ExploreEvent.ExploreStart, OnStart);
    }
    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        ExploreDataModel.Instance.RemoveEvent<List<int>>(ExploreEvent.ExploreStart, OnStart);
    }

    private void OnStart(List<int> listId)
    {
        OnClose();
    }
}

    
