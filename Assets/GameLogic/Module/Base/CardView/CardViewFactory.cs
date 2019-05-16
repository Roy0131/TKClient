using System.Collections.Generic;
using UnityEngine;
using System;

public enum CardViewType
{
    None,
    ConfigCard,
    Lineup,
    Common,
    FusionMat,
    Decompose,
    HeroCall,
}

public class CardViewFactory : Singleton<CardViewFactory>
{
    private Queue<CardView> _lstCardViewPools = new Queue<CardView>();


    public CardView CreateCardView(int tableId, CardViewType type, Action<CardView> OnClickMethod = null)
    {
        CardDataVO vo = new CardDataVO(tableId);
        return CreateCardView(vo, type, OnClickMethod);
    }


    public CardView CreateCardView(CardDataVO vo, CardViewType type, Action<CardView> OnClickMethod = null)
    {
        CardView view = GetView(type, OnClickMethod);
        view.Show(vo);
        view.mRectTransform.anchoredPosition = Vector2.zero;
        view.mRectTransform.anchorMax = Vector2.one * 0.5f;
        view.mRectTransform.anchorMin = Vector2.one * 0.5f;
        return view;
    }

    public void ReturnCardView(CardView view)
    {
        view.mRectTransform.localScale = Vector3.one * 1.3f;
        view.ReturnCardView();
        _lstCardViewPools.Enqueue(view);
        GameUIMgr.Instance.AddObjectToTopRoot(view.mRectTransform);
    }

    private GameObject _cardItemObject;

    private CardView GetView(CardViewType type, Action<CardView> OnClickMethod = null)
    {
        CardView view;
        if(_lstCardViewPools.Count > 0)
        {
            view = _lstCardViewPools.Dequeue();
        }
        else
        {
            if (_cardItemObject == null)
                _cardItemObject = GameResMgr.Instance.LoadUIObjectSync(SingletonResName.UICardItem, false);
            view = new CardView();
            view.SetDisplayObject(GameObject.Instantiate(_cardItemObject));
        }
        view.SetParamters(type, OnClickMethod);
        return view;
    }
}