using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarnivalTwoExchangeView : UIBaseView
{
    private int _activeType;
    private CarnivalDataVO _dataVO;
    private RectTransform _rect1;
    private RectTransform _rect2;
    private RectTransform _rect3;
    private GameObject _obj;
    private Button _btn;
    private Text _btnText;
    private Text _number;
    private ItemView _view1;
    private ItemView _view2;
    private ItemView _view3;

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _rect1 = Find<RectTransform>("Obj1");
        _rect2 = Find<RectTransform>("Obj2");
        _rect3 = Find<RectTransform>("Obj3");
        _btnText = Find<Text>("Buy/Text");
        _number = Find<Text>("Number");
        _btn = Find<Button>("Buy");
        _obj = Find("Text2");

        _btn.onClick.Add(OnBtn);
        _activeType = 0;
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        CarnivalDataModel.Instance.AddEvent<int>(CarnivalEvent.CarnivalItemExchange, OnCarnivalItemExchange);
        CarnivalDataModel.Instance.AddEvent<int>(CarnivalEvent.CarnivalNotify, OnCarnivalNotify);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        CarnivalDataModel.Instance.RemoveEvent<int>(CarnivalEvent.CarnivalItemExchange, OnCarnivalItemExchange);
        CarnivalDataModel.Instance.RemoveEvent<int>(CarnivalEvent.CarnivalNotify, OnCarnivalNotify);
    }

    private void OnCarnivalNotify(int id)
    {
        if (_dataVO.mId == id)
            OnExchange();
    }

    private void OnCarnivalItemExchange(int id)
    {
        if (_dataVO.mId == id)
        {
            OnExchange();
            GetItemTipMgr.Instance.ShowItemResult(_dataVO.mRewardInfo);
        }
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _activeType = int.Parse(args[0].ToString());
        _dataVO = args[1] as CarnivalDataVO;
        _btnText.text = LanguageMgr.GetLanguage(4000138);
        OnExchange();
        if (_view1 != null)
            ItemFactory.Instance.ReturnItemView(_view1);
        if (_view2 != null)
            ItemFactory.Instance.ReturnItemView(_view2);
        if (_view3 != null)
            ItemFactory.Instance.ReturnItemView(_view3);
        _obj.SetActive(_dataVO.mExchangeInfo2 != null);
        if (GameConfigMgr.Instance.GetItemConfig(_dataVO.mRewardInfo[0].Id).ItemType == 2)
            _view1 = ItemFactory.Instance.CreateItemView(_dataVO.mRewardInfo[0], ItemViewType.EquipHeroItem);
        else
            _view1 = ItemFactory.Instance.CreateItemView(_dataVO.mRewardInfo[0], ItemViewType.HeroItem);
        _view1.mRectTransform.SetParent(_rect1, false);
        if (_dataVO.mExchangeInfo1!=null)
        {
            if (GameConfigMgr.Instance.GetItemConfig(_dataVO.mExchangeInfo1.Id).ItemType == 2)
                _view2 = ItemFactory.Instance.CreateItemView(_dataVO.mExchangeInfo1, ItemViewType.EquipHeroItem);
            else
                _view2 = ItemFactory.Instance.CreateItemView(_dataVO.mExchangeInfo1, ItemViewType.HeroItem);
            _view2.mRectTransform.SetParent(_rect2, false);
        }
        if (_dataVO.mExchangeInfo2 != null)
        {
            if (GameConfigMgr.Instance.GetItemConfig(_dataVO.mExchangeInfo2.Id).ItemType == 2)
                _view3 = ItemFactory.Instance.CreateItemView(_dataVO.mExchangeInfo2, ItemViewType.EquipHeroItem);
            else
                _view3 = ItemFactory.Instance.CreateItemView(_dataVO.mExchangeInfo2, ItemViewType.HeroItem);
            _view3.mRectTransform.SetParent(_rect3, false);
        }
    }

    private void OnExchange()
    {
        if (_dataVO.mEventCount == _dataVO.mValue)
            _btn.interactable = false;
        else
            _btn.interactable = true;
        _number.text = (_dataVO.mEventCount - _dataVO.mValue) + "/" + _dataVO.mEventCount;
    }

    private void OnBtn()
    {
        if (_dataVO.mExchangeInfo1 != null)
        {
            if (_dataVO.mExchangeInfo2 != null)
            {
                if (BagDataModel.Instance.GetItemCountById(_dataVO.mExchangeInfo1.Id) >= _dataVO.mExchangeInfo1.Value &&
                    BagDataModel.Instance.GetItemCountById(_dataVO.mExchangeInfo2.Id) >= _dataVO.mExchangeInfo2.Value)
                {
                    GameNetMgr.Instance.mGameServer.ReqCarnivalItemExchange(_dataVO.mId);
                }
                else
                {
                    PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001131));
                }
            }
            else
            {
                if (BagDataModel.Instance.GetItemCountById(_dataVO.mExchangeInfo1.Id) >= _dataVO.mExchangeInfo1.Value)
                    GameNetMgr.Instance.mGameServer.ReqCarnivalItemExchange(_dataVO.mId);
                else
                    PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001131));
            }
        }
    }

    public override void Dispose()
    {
        if (_view1 != null)
            ItemFactory.Instance.ReturnItemView(_view1);
        _view1 = null;
        if (_view2 != null)
            ItemFactory.Instance.ReturnItemView(_view2);
        _view2 = null;
        if (_view3 != null)
            ItemFactory.Instance.ReturnItemView(_view3);
        _view3 = null;
        base.Dispose();
    }
}
