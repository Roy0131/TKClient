using Framework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarnivalTwoRechargeView : UIBaseView
{
    private int _activeType;
    private CarnivalDataVO _dataVO;
    private Text _text1;
    private Text _text2;
    private RectTransform _parent;
    private ItemView _view;
    private bool isGray;


    protected override void ParseComponent()
    {
        base.ParseComponent();
        _text1 = Find<Text>("Text1");
        _text2 = Find<Text>("Text2");
        _parent = Find<RectTransform>("ItemObj");
        
        _activeType = 0;
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        CarnivalDataModel.Instance.AddEvent<int>(CarnivalEvent.CarnivalNotify, OnCarnivalNotify);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        CarnivalDataModel.Instance.RemoveEvent<int>(CarnivalEvent.CarnivalNotify, OnCarnivalNotify);
    }

    private void OnCarnivalNotify(int id)
    {
        OnInit();
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _activeType = int.Parse(args[0].ToString());
        _dataVO = args[1] as CarnivalDataVO;
        _text1.text = LanguageMgr.GetLanguage(5007621, _dataVO.mParam1);
        OnInit();
    }

    private void OnInit()
    {
        if (_dataVO.mValue >= _dataVO.mEventCount)
        {
            isGray = true;
            _text2.text = "(" + _dataVO.mParam1 + "/" + _dataVO.mParam1 + ")";
        }
        else
        {
            isGray = false;
            _text2.text = "(" + _dataVO.mValues + "/" + _dataVO.mParam1 + ")";
        }
        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        for (int i = 0; i < _dataVO.mRewardInfo.Count; i++)
        {
            if (GameConfigMgr.Instance.GetItemConfig(_dataVO.mRewardInfo[i].Id).ItemType == 2)
                _view = ItemFactory.Instance.CreateItemView(_dataVO.mRewardInfo[i], ItemViewType.EquipHeroItem);
            else
                _view = ItemFactory.Instance.CreateItemView(_dataVO.mRewardInfo[i], ItemViewType.HeroItem);
            _view.mRectTransform.SetParent(_parent, false);
            if (isGray)
                _view.SetGray();
            else
                _view.SetNormal();
        }
    }

    public override void Dispose()
    {
        if (_view != null)
            ItemFactory.Instance.ReturnItemView(_view);
        _view = null;
        base.Dispose();
    }
}
