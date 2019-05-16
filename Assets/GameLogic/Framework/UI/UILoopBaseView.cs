using System.Collections.Generic;
using UnityEngine;

namespace Framework.UI
{
    public abstract class UILoopBaseView<M> : UIBaseView
    {
        protected RLoopScrollRect _loopScrollRect;
        protected Queue<UIBaseView> _uiViewPools;
        protected List<UIBaseView> _lstShowViews;
        protected List<M> _lstDatas;
        protected List<UIBaseView> _lstInterfaceViews;

        public UILoopBaseView()
        {
            _uiViewPools = new Queue<UIBaseView>();
            _lstShowViews = new List<UIBaseView>();
            _lstDatas = new List<M>();
            _lstInterfaceViews = new List<UIBaseView>();
        }

        protected void InitScrollRect(string name)
        {
            _loopScrollRect = Find<RLoopScrollRect>(name);
            _loopScrollRect.mCreateNewFunc = OnCreateNewItem;
            _loopScrollRect.mReturnAct = OnReturnItem;

        }

        private RectTransform OnCreateNewItem(int idx, bool blHead)
        {
            //Debuger.LogWarning("create new item, idx:" + idx + ", blHead:" + blHead);
            UIBaseView view = CreateNewItemView(idx);
            if (view == null)
                return null;
            if (blHead)
                _lstInterfaceViews.Insert(0, view);
            else
                _lstInterfaceViews.Add(view);
            return view.mRectTransform;
        }

        private void OnReturnItem(int idx)
        {
            if (idx < 0 || idx > _lstInterfaceViews.Count - 1)
            {
                LogHelper.LogWarning("[UILoopBaseView.OnReturnItem() => idx:" + idx + " invalid!!!]");
                return;
            }
            //Debuger.Log("return item, idx:" + idx);
            
            RetItemView(_lstInterfaceViews[idx]);
            _lstInterfaceViews.RemoveAt(idx);
        }

        public virtual UIBaseView CreateNewItemView(int idx)
        {
            UIBaseView view;
            if (_uiViewPools.Count > 0)
                view = _uiViewPools.Dequeue();
            else
                view = CreateItemView();
            SetItemData(view, idx);
            _lstShowViews.Add(view);
            return view;
        }

        protected virtual void SetItemData(UIBaseView view, int idx)
        {
            view.Show(_lstDatas[idx]);
        }

        protected void RefreshLoopView(List<M> value)
        {
            _lstDatas = value;
            _loopScrollRect.ClearCells();
            if (_lstDatas == null || _lstDatas.Count == 0)
                return;
            _loopScrollRect.totalCount = _lstDatas.Count;
            _loopScrollRect.RefillCells();
        }

        protected abstract UIBaseView CreateItemView();

        public virtual void RetItemView(UIBaseView view)
        {
            _uiViewPools.Enqueue(view);
            view.Hide();
            view.mRectTransform.SetParent(mRectTransform, false);
            _lstShowViews.Remove(view);
        }

        public override void Dispose()
        {
            if (_loopScrollRect != null)
            {
                _loopScrollRect.ClearCells();
                _loopScrollRect = null;
            }
            if (_uiViewPools != null && _uiViewPools.Count > 0)
            {
                while (_uiViewPools.Count > 0)
                    _uiViewPools.Dequeue().Dispose();
                _uiViewPools.Clear();
                _uiViewPools = null;
            }
            if (_lstShowViews != null)
            {
                _lstShowViews.Clear();
                _lstShowViews = null;
            }
            base.Dispose();
        }
    }
}