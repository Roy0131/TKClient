using NewBieGuide;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public class RoleEquipView : UIBaseView
{
    private Button _wearEquip;
    private Button _takeoffEquip;

    private List<RoleEquipSlotItem> _lstEquipSlots;
    private CardDataVO _vo;
    private UIEffectView _effect01;
    private UIEffectView _effect02;
    private RectTransform _parent;
    private List<GameObject> _gameObjects;

    protected override void ParseComponent()
    {
        base.ParseComponent();

        _wearEquip = Find<Button>("ButtonUp");
        _takeoffEquip = Find<Button>("ButtonDown");
        _parent = Find<RectTransform>("Attris");
        _wearEquip.onClick.Add(OnWearEquip);
        _takeoffEquip.onClick.Add(OnTakeOffEquip);

        _lstEquipSlots = new List<RoleEquipSlotItem>();
        RoleEquipSlotItem item;
        for (int i = 1; i <= 6; i++)
        {
            item = new RoleEquipSlotItem(i);
            item.SetDisplayObject(Find("Equip/EquipSlot" + i));
            _lstEquipSlots.Add(item);
        }
        _effect01 = CreateUIEffect(Find("Equip/fx_ui_jiesuotishi"), UILayerSort.WindowSortBeginner);
        _effect02 = CreateUIEffect(Find("Equip/fx_ui_jiesuo"), UILayerSort.WindowSortBeginner);

        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.HeroEquipSlot, _lstEquipSlots[0].mTransform);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<RoleEquipSlotItem>(EquipEvent.ShowRoleInfoEquipTips, OnEquipClick);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<int>(EquipEvent.RoleEquipTipsFun1Called, OnFun1Method);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<int>(EquipEvent.RoleEquipTipsFun2Called, OnFun2Method);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(EquipEvent.EquipGemUnLock,OnUnLock);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(EquipEvent.EquipGemUnLockAble, OnUnLockAble);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(EquipEvent.EquipGemLock, OnLock);
        HeroDataModel.Instance.AddEvent<List<int>>(HeroEvent.CardDataRefresh, OnChangeRole);

    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<RoleEquipSlotItem>(EquipEvent.ShowRoleInfoEquipTips, OnEquipClick);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<int>(EquipEvent.RoleEquipTipsFun1Called, OnFun1Method);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<int>(EquipEvent.RoleEquipTipsFun2Called, OnFun2Method);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(EquipEvent.EquipGemUnLock, OnUnLock);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(EquipEvent.EquipGemUnLockAble, OnUnLockAble);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(EquipEvent.EquipGemLock, OnLock);
        HeroDataModel.Instance.RemoveEvent<List<int>>(HeroEvent.CardDataRefresh, OnChangeRole);
    }
    private void OnLock()
    {
        _effect01.StopEffect();
        _effect02.StopEffect();
    }
    private void OnUnLock()
    {
        _effect02.StopEffect();
        _effect01.PlayEffect();
    }
    private void OnUnLockAble()
    {
        _effect01.StopEffect();
        _effect02.PlayEffect();
    }
    private RoleEquipSlotItem FindEquipSlot(int equipType)
    {
        int index = equipType - 1;
        if (index < 0 || index >= _lstEquipSlots.Count)
        {
            LogHelper.LogWarning("[RoleEquipView.FindEquipSlot() => equipType:" + equipType + " error!!]");
            return null;
        }
        return _lstEquipSlots[index];
    }

    private void OnFun1Method(int equipType)
    {
        RoleEquipSlotItem item = FindEquipSlot(equipType);
        if (item == null)
            return;
        if (item.mEquipType == EquipmentType.GemStone)
        {
            //convert;
            EquipFunDataVO vo = new EquipFunDataVO();
            vo.mRoleId = _vo.mCardID;
            vo.mEquipSlot = item.mEquipType;
            vo.mItemId = _vo.GetEquipIdByEquipType(item.mEquipType);
            vo.mUpGradeType = ItemUpGradeType.GemStone_Convert;
            GameUIMgr.Instance.OpenModule(ModuleID.EquipFunc, vo);
        }
        else
        {
            GameNetMgr.Instance.mGameServer.ReqTakeoffEquip(_vo.mCardID, item.mEquipType);
        }
    }

    private void OnFun2Method(int equipType)
    {
        RoleEquipSlotItem item = FindEquipSlot(equipType);
        if (item == null)
            return;
        if (item.mEquipType == EquipmentType.Artifact || item.mEquipType == EquipmentType.GemStone)
        {
            //upGrade;
            EquipFunDataVO vo = new EquipFunDataVO();
            vo.mRoleId = _vo.mCardID;
            vo.mEquipSlot = item.mEquipType;
            vo.mItemId = _vo.GetEquipIdByEquipType(item.mEquipType);
            vo.mUpGradeType = item.mEquipType == EquipmentType.GemStone ? ItemUpGradeType.GemStone_UpGrade : ItemUpGradeType.Artifact_UpGrade;
            GameUIMgr.Instance.OpenModule(ModuleID.EquipFunc, vo);
        }
        else
        {
            EquipEventVO evtVO = new EquipEventVO();
            evtVO.mEquipType = equipType;
            evtVO.mCardDataVO = _vo;
            GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(EquipEvent.ShowEquipSelectView, evtVO);
        }
    }

    private void OnEquipClick(RoleEquipSlotItem item)
    {
        ItemTipsMgr.Instance.ShowRoleEquipTips(_vo, item.mEquipType);
    }

    private void OnChangeRole(List<int> listId)
    {
        OnClear();
        List<string> listAttris = new List<string>();
        for (int i = 0; i < GameConst.AttrListShow.Count; i++)
        {
            AttributeConfig cfg = GameConfigMgr.Instance.GetAttrConfig(GameConst.AttrListShow[i]);
            if (_vo.Comparison(GameConst.AttrListShow[i]) != 0)
            {
                if (_vo.Comparison(GameConst.AttrListShow[i]) > 0)
                {
                    if (cfg.PercentShow > 0)
                        listAttris.Add(LanguageMgr.GetLanguage(cfg.NameID) + " + " + (float)_vo.Comparison(GameConst.AttrListShow[i]) / cfg.Divisor + "%");
                    else
                        listAttris.Add(LanguageMgr.GetLanguage(cfg.NameID) + " + " + _vo.Comparison(GameConst.AttrListShow[i]));
                }
                else
                {
                    if (cfg.PercentShow > 0)
                        listAttris.Add(LanguageMgr.GetLanguage(cfg.NameID) + " - " + (float)-_vo.Comparison(GameConst.AttrListShow[i]) / cfg.Divisor + "%");
                    else
                        listAttris.Add(LanguageMgr.GetLanguage(cfg.NameID) + " - " + -_vo.Comparison(GameConst.AttrListShow[i]));
                }
            }
        }
        _gameObjects = new List<GameObject>();
        for (int i = 0; i < listAttris.Count; i++)
        {
            GameObject obj = GameObject.Instantiate(Find("Attris/Text"));
            obj.transform.SetParent(_parent, false);
            obj.SetActive(true);
            obj.GetComponent<Text>().text = listAttris[i];
            _gameObjects.Add(obj);
        }
        DelayCall(0.6f, OnClear);
    }

    private void OnClear()
    {
        if (_gameObjects != null)
        {
            for (int i = 0; i < _gameObjects.Count; i++)
               GameObject.Destroy(_gameObjects[i]);
            _gameObjects.Clear();
        }
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _vo = args[0] as CardDataVO;
        for (int i = 0; i < _lstEquipSlots.Count; i++)
            _lstEquipSlots[i].Show(args);
       
    }

    private void OnWearEquip()
    {
        GameNetMgr.Instance.mGameServer.ReqWearEquipByOneKey(_vo.mCardID);
    }

    private void OnTakeOffEquip()
    {
        GameNetMgr.Instance.mGameServer.ReqTakeOffEquipByOnKey(_vo.mCardID);
    }

    public override void Dispose()
    {
        if (_lstEquipSlots != null)
        {
            for (int i = 0; i < _lstEquipSlots.Count; i++)
                _lstEquipSlots[i].Dispose();
            _lstEquipSlots.Clear();
            _lstEquipSlots = null;
        }
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.HeroEquipSlot);
        base.Dispose();
    }
}