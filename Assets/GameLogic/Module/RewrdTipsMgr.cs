using Framework.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardTipsMgr : Singleton<RewardTipsMgr>
{
    #region init TipsView
    private Queue<RewardTipsView> _uiPools = new Queue<RewardTipsView>();
    private Queue<ItemInfo> _tipsPool = new Queue<ItemInfo>();
    public RewardTipsView mCurShowTips { get; private set; } = null;
    #endregion

    private RewardTipsView GetTipsView()
    {
        RewardTipsView view;
        if (_uiPools.Count > 0)
        {
            view = _uiPools.Dequeue();
        }
        else
        {
            view = new RewardTipsView();
            view.SetDisplayObject(GameResMgr.Instance.LoadUIObjectSync(SingletonResName.UIRewardTips));
        }
        GameUIMgr.Instance.AddObjectToTopRoot(view.mRectTransform);
        view.mRectTransform.localPosition = Vector2.zero;
        return view;
    }

    public void ShowTips(ItemInfo info)
    {
        if (mCurShowTips != null)
        {
            if (info == mCurShowTips.mItemInfo)
                return;
            if (_tipsPool.Contains(info))
                return;
            _tipsPool.Enqueue(info);
        }
        else
        {
            Show(info);
        }
    }

    public void ShowTips(int id, params object[] args)
    {
        int value = id;
        if (value == 0)
            return;
        ShowTips(value);
    }

    private void Show(ItemInfo value)
    {
        mCurShowTips = GetTipsView();
        mCurShowTips.Show(value);
    }

    public void ReturnView(RewardTipsView view)
    {
        mCurShowTips = null;
        _uiPools.Enqueue(view);
        view.Hide();
        if (_tipsPool.Count <= 0)
            return;
        Show(_tipsPool.Dequeue());
    }
}

public class RewardTipsView : UIBaseView
{
    private static Vector2 _targetPos = new Vector2(0, 200f);

    private Text _text;
    private Color _defaultColor;
    private Image _backGround;
    private ItemView _itemView;

    public ItemInfo mItemInfo { get; private set; }
    protected override void ParseComponent()
    {
        base.ParseComponent();
        _text = Find<Text>("Root/Label");
        _backGround = Find<Image>("BackGround");
        _defaultColor = _text.color;
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        mItemInfo = args[0] as ItemInfo;
        if (_itemView != null)
            ItemFactory.Instance.ReturnItemView(_itemView);
        _itemView = ItemFactory.Instance.CreateItemView(mItemInfo, ItemViewType.RewardItem);
        _text.text = LanguageMgr.GetLanguage(_itemView.mItemDataVO.mItemConfig.NameID);
        GameUIMgr.Instance.ChildAddToParent(_itemView.mRectTransform, Find<RectTransform>("Root/Image"));

        _text.color = _defaultColor;
        _backGround.color = new Color(0.2f, 0.15f, 0.3f, 0.6f);

        _itemView._backImg.color = Color.white;
        _itemView._itemIcon.color = Color.white;
        _itemView._countText.color = Color.white;
        _itemView._itemKind.color = Color.white;
        //_image.color = Color.white;
        //mRectTransform.DOAnchorPos(_targetPos, 0.8f).onComplete = DoAlphaEnd;
        DGHelper.DoAnchorPos(mRectTransform, _targetPos, 0.8f, 0, DoAlphaEnd);
    }

    private void DoAlphaEnd()
    {
        DGHelper.DoKill(mRectTransform);
        DGHelper.DoKill(_text);
        DGHelper.DoKill(_backGround);

        DGHelper.DoKill(_itemView._backImg);
        DGHelper.DoKill(_itemView._itemIcon);
        DGHelper.DoKill(_itemView._countText);
        DGHelper.DoKill(_itemView._itemKind);
        RewardTipsMgr.Instance.ReturnView(this);
    }

    public override void Dispose()
    {
        base.Dispose();
        if (_itemView != null)
            ItemFactory.Instance.ReturnItemView(_itemView);
        _itemView = null;
    }
}