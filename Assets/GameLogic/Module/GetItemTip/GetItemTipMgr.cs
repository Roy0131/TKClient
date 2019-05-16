using System.Collections.Generic;
using Msg.ClientMessage;
using UnityEngine;

public class GetItemTipMgr : Singleton<GetItemTipMgr>
{
    private Queue<GetItemView> _itemViewPool = new Queue<GetItemView>();
    private Queue<GetItemView> _queItemView = new Queue<GetItemView>();
    private GetItemView _curShowItemView = null;
    private GetItemView GetItemView()
    {
        if (_itemViewPool.Count > 0)
            return _itemViewPool.Dequeue();
        GetItemView itemView = new GetItemView();
        GameObject obj = GameResMgr.Instance.LoadUIObjectSync(SingletonResName.UIGetItem);
        itemView.SetDisplayObject(obj);
        return itemView;
    }

    public void ShowRoles(List<ItemInfo> value, int level = 1)
    {
        GetItemView view = GetItemView();
        view.ShowResult(value, level);
        if (_curShowItemView != null)
            _queItemView.Enqueue(view);
        else
            ShowItemView(view);
    }

    public void ShowRoleResult(int roleId)
    {
        GetItemView view = GetItemView();
        view.ShowRoleResult(roleId);
        if (_curShowItemView != null)
            _queItemView.Enqueue(view);
        else
            ShowItemView(view);
    }

    public void ShowItemResult(IList<ItemInfo> value)
    {
        if (value == null || value.Count == 0)
            return;
        GetItemView view = GetItemView();
        view.ShowItemResult(value);
        if (_curShowItemView != null)
            _queItemView.Enqueue(view);
        else
            ShowItemView(view);
    }

    private void ShowItemView(GetItemView view)
    {
        _curShowItemView = view;
        _curShowItemView.Show();
        GameUIMgr.Instance.AddObjectToTopRoot(_curShowItemView.mRectTransform);
        _curShowItemView.mRectTransform.SetAsFirstSibling();
    }

    public void CloseItemView(GetItemView view)
    {
        view.Hide();
        _itemViewPool.Enqueue(view);
        _curShowItemView = null;
        if (_queItemView.Count > 0)
            ShowItemView(_queItemView.Dequeue());
    }
}