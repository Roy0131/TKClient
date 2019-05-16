using Spine.Unity;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using Msg.ClientMessage;
using Framework.UI;

public class GoldModule : ModuleBase
{
    private Button _closeButton;
    private SkeletonGraphic _skeleSpine;
    private Transform _root;
    private Text _Time;
    private RectTransform _parent;
    private List<GoldDataVO> _listGoldVO;
    private List<GoldItemView> _listGoldItemView;

    private uint _timer = 0;
    private int _goldTime = 0;

    public GoldModule()
        : base(ModuleID.Gold, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_Gold;
        _soundName = UIModuleSoundName.GoldSoundName;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _closeButton = Find<Button>("Root/Btn_Back");
        _skeleSpine = Find<SkeletonGraphic>("Root/dianjinshou");
        _root = Find<Transform>("Root");
        _Time = Find<Text>("Root/GoldObj/TimeText");

        _closeButton.onClick.Add(OnClose);
        CreateFixedEffect(_skeleSpine.gameObject, UILayerSort.PopupSortBeginner + 1, SortObjType.Canvas);

        ColliderHelper.SetButtonCollider(_closeButton.transform, 120, 120);
        OnGoldInit();
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GoldDataModel.Instance.AddEvent<List<GoldDataVO>>(GoldEvent.GoldData, OnGoldData);
        GoldDataModel.Instance.AddEvent<int>(GoldEvent.GoldTou, OnGoldTou);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GoldDataModel.Instance.RemoveEvent<List<GoldDataVO>>(GoldEvent.GoldData, OnGoldData);
        GoldDataModel.Instance.RemoveEvent<int>(GoldEvent.GoldTou, OnGoldTou);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        GoldDataModel.Instance.ReqDoldData();
        _skeleSpine.AnimationState.SetAnimation(0, "animation2", false);

         _skeleSpine.AnimationState.AddAnimation(0, "animation", true,2f);
    }

    private void OnGoldData(List<GoldDataVO> listGoldVO)
    {
        _listGoldVO = listGoldVO;
        OnGoldChange();
        _goldTime = GoldDataModel.Instance.GoldTime;
        if (_timer != 0)
            TimerHeap.DelTimer(_timer);
        int interval = 1000;
        _timer = TimerHeap.AddTimer(0, interval, OnAddTime);
    }

    private void OnAddTime()
    {
        if (_goldTime > 0)
        {
            _goldTime -= 1;
            _Time.text = (LanguageMgr.GetLanguage(5001710) + "<color=#A5FD47>" + TimeHelper.GetCountTime(_goldTime) + "</color>");
        }
        else
        {
            _Time.text = LanguageMgr.GetLanguage(5001317);
        }
    }

    private void OnGoldTou(int goldNum)
    {
        List<ItemInfo> _listItemInfo = new List<ItemInfo>();
        ItemInfo _itemInfo = new ItemInfo();
        _itemInfo.Id = SpecialItemID.Gold;
        _itemInfo.Value = goldNum;
        _listItemInfo.Add(_itemInfo);
        GetItemTipMgr.Instance.ShowItemResult(_listItemInfo);
    }

    private void ClearGoldItem()
    {
        if (_listGoldItemView != null)
        {
            for (int i = 0; i < _listGoldItemView.Count; i++)
                _listGoldItemView[i].Dispose();
            _listGoldItemView.Clear();
            _listGoldItemView = null;
        }
    }

    private void OnGoldInit()
    {
        _listGoldItemView = new List<GoldItemView>();
        for (int i = 0; i < 3; i++)
        {
            GameObject obj = Find("Root/GoldObj/Panel_Scroll/KnapsackPanel/Gold" + (i + 1));
            GoldItemView goldItemView = new GoldItemView();
            goldItemView.SetDisplayObject(obj);
            _listGoldItemView.Add(goldItemView);
        }
    }

    private void OnGoldChange()
    {
        if (_listGoldVO == null || _listGoldVO.Count == 0)
            return;
        _listGoldVO.Sort((x, y) => x.mGoldIndex.CompareTo(y.mGoldIndex));
        for (int i = 0; i < _listGoldVO.Count; i++)
            _listGoldItemView[i].Show(_listGoldVO[i]);
    }

    public override void Dispose()
    {
        ClearGoldItem();
        if (_listGoldVO != null)
        {
            _listGoldVO.Clear();
            _listGoldVO = null;
        }
        base.Dispose();
    }

    protected override void OnShowAnimator()
    {
        base.OnShowAnimator();
        ObjectHelper.PopAnimationBack(_root);
    }
}