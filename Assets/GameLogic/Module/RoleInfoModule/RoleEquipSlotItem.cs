using Msg.ClientMessage;
using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

public class RoleEquipSlotItem : UIBaseView
{
    public int mEquipType { get; private set; }
    private GameObject _lockFlag;
    private Text _unlockableFlag;
    private Button _button;
    private ItemView _itemView;
    private EquipSlotStatu _status;
    private CardDataVO _vo;
    private ItemConfig _equipConfig;
    private GameObject _addFlagObj;
    private ItemInfo info;


    private int _fighting;
    public RoleEquipSlotItem(int slotIdx)
    {
        mEquipType = slotIdx;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _lockFlag = Find("LockObj");
        _unlockableFlag = Find<Text>("LockStatusText");
        _addFlagObj = Find("ImageAdd");
        _button = FindOnSelf<Button>();
        _button.onClick.Add(OnClick);

       
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _vo = args[0] as CardDataVO;
        _status = EquipSlotStatu.None;
        _unlockableFlag.gameObject.SetActive(false);
        int equipId = _vo.GetEquipIdByEquipType(mEquipType);
        _equipConfig = null;
        info = null;

        if (_itemView != null)
        {
            ItemFactory.Instance.ReturnItemView(_itemView);
            _itemView = null;
            _equipConfig = null;
        }

        if (equipId != 0)
        {
            _equipConfig = GameConfigMgr.Instance.GetItemConfig(equipId);
            _fighting = _equipConfig.BattlePower;
        }
        else
        {
            _fighting = -1;
        }
        if (mEquipType == EquipmentType.GemStone)
        {
           
            if (_vo.mCardLevel < 45)
            {
                _unlockableFlag.gameObject.SetActive(true);
                GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(EquipEvent.EquipGemLock);
                _unlockableFlag.text = LanguageMgr.GetLanguage(6001222);
                _status = EquipSlotStatu.Locked;
            }
            else
            {
                _unlockableFlag.gameObject.SetActive(false);
                if (equipId == 0)
                {
                    //_effect01.PlayEffect();
                    GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(EquipEvent.EquipGemUnLock);//播放解锁提示特效
                    //_unlockableFlag.text = LanguageMgr.GetLanguage(6001223);
                    _status = EquipSlotStatu.Unlockable;
                }
                else
                {
                    
                    GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(EquipEvent.EquipGemLock);
                    _status = EquipSlotStatu.HasEqiup;
                }
            }
        }
        else
        {
            if (equipId == 0)
                _status = EquipSlotStatu.Unlocked;
            else
                _status = EquipSlotStatu.HasEqiup;
        }
        if (_status == EquipSlotStatu.Unlockable)
        {
            _lockFlag.SetActive(true);
        }
        else
        {
            _lockFlag.SetActive(_status == EquipSlotStatu.Locked);
        }
        info = BagDataModel.Instance.GetEquipFighting(mEquipType, _fighting);
        _addFlagObj.SetActive(_equipConfig == null && info != null);
        if (_status == EquipSlotStatu.Unlockable || _status == EquipSlotStatu.Locked)
            _addFlagObj.SetActive(false);
        if (_equipConfig != null)
        {
            if (_equipConfig.ItemType == 2)
                _itemView = ItemFactory.Instance.CreateItemView(_equipConfig, ItemViewType.EquipItem, OnClickEquip);
            else
                _itemView = ItemFactory.Instance.CreateItemView(_equipConfig, ItemViewType.BagItem, OnClickEquip);
            _itemView.mRectTransform.SetParent(mRectTransform, false);
            if (_equipConfig.EquipType != 5)
                _itemView.RedSele = info != null;
            else
                _itemView.RedSele = false;
        }
    }

    private void OnClickEquip(ItemView view)
    {
        OnClick();
    }

    private void OnClick()
    {
        LogHelper.LogWarning("index:" + mEquipType);
        switch (_status)
        {
            case EquipSlotStatu.None:
            case EquipSlotStatu.Locked:
                return;
            case EquipSlotStatu.HasEqiup:
                GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(EquipEvent.ShowRoleInfoEquipTips, this);
                break;
            case EquipSlotStatu.Unlocked:
                EquipEventVO evtVO = new EquipEventVO();
                evtVO.mEquipType = mEquipType;
                evtVO.mCardDataVO = _vo;
                GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(EquipEvent.ShowEquipSelectView, evtVO);
                break;
            case EquipSlotStatu.Unlockable:
                GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(EquipEvent.EquipGemUnLockAble);
                DelayCall(0.3f,()=> GameNetMgr.Instance.mGameServer.ReqOpenLeftSlot(_vo.mCardID)) ;
                break;
        }
    }

    public override void Dispose()
    {
        if (_itemView != null)
        {
            ItemFactory.Instance.ReturnItemView(_itemView);
            _itemView = null;
        }
        _equipConfig = null;
        base.Dispose();
    }
}