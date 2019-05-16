using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ItemViewType
{
    None,
    RewardItem,
    BagItem,
    EquipItem,
    EquipRewardItem,
    ShopItem,
    HeroItem,
    EquipHeroItem,
    HurtItem,
}

public class ItemDataVO : DataBaseVO
{
    public ItemConfig mItemConfig { get; private set; }
    private ItemInfo _itemInfoData;
    public int mCount { get; private set; }

    protected override void OnInitData<T>(T value)
    {
        if (value.GetType() == typeof(ItemConfig))
        {
            mItemConfig = value as ItemConfig;
            mCount = 1;
        }
        else
        {
            _itemInfoData = value as ItemInfo;
            mItemConfig = GameConfigMgr.Instance.GetItemConfig(_itemInfoData.Id);
            mCount = _itemInfoData.Value;
        }
    }

    public void RefreshCount(int value)
    {
        mCount = value;
    }

    public override void Dispose()
    {
        base.Dispose();
        _itemInfoData = null;
        mItemConfig = null;
    }
}

public class ItemFactory : Singleton<ItemFactory>
{
    private Queue<ItemView> _lstItemViewPools = new Queue<ItemView>();
    
    public ItemView CreateItemView<T>(T data, ItemViewType type, Action<ItemView> OnClickMethod = null)
    {
        ItemDataVO vo = new ItemDataVO();
        vo.InitData(data);

        ItemView view = GetItemView(type, OnClickMethod);
        view.Show(vo);
        view.mRectTransform.anchoredPosition = Vector2.zero;
        view.mRectTransform.anchorMax = Vector2.one * 0.5f;
        view.mRectTransform.anchorMin = Vector2.one * 0.5f;
        return view;
    }
    
    public void ReturnItemView(ItemView view)
    {
        view.mRectTransform.localScale = Vector3.one;
        view.Hide();
        _lstItemViewPools.Enqueue(view);
        GameUIMgr.Instance.AddObjectToTopRoot(view.mRectTransform);
    }

    private GameObject _itemObject;
    private ItemView GetItemView(ItemViewType type, Action<ItemView> OnClickMethod)
    {
        ItemView view;
        if (_lstItemViewPools.Count > 0)
        {
            view = _lstItemViewPools.Dequeue();
        }
        else
        {
            if (_itemObject == null)
                _itemObject = GameResMgr.Instance.LoadUIObjectSync(SingletonResName.UIItem, false);
            view = new ItemView();
            view.SetDisplayObject(GameObject.Instantiate(_itemObject));
        }
        view.SetParamters(type, OnClickMethod);
        return view;
    }
}