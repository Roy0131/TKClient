using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Framework.Core;

namespace Framework.UI
{
    public abstract class UIBaseView : IDisplayObject, IDispose
    {
        public bool mBlShow { get; protected set; }
        protected List<UIBaseView> _childrenViews;
        protected List<UIEffectView> _lstFixedEffects;
        protected int _sortOrder;
        public UIBaseView()
        {
            mBlShow = false;
            _childrenViews = new List<UIBaseView>();
            _lstFixedEffects = new List<UIEffectView>();
            _delayTimerPool = new Queue<DelayTimer>();
        }

        public int SortingOrder
        {
            get { return _sortOrder; }
            set
            {
                _sortOrder = value;
            }
        }

        public void SetDisplayObject(GameObject obj)
        {
            if (obj == null)
            {
                LogHelper.LogWarning("display object is null!!");
                return;
            }
            _displayObject = obj;
            _transform = mDisplayObject.transform;
            if (_transform is RectTransform)
                _rectTransform = _transform as RectTransform;
            ParseComponent();
        }

        #region children object operate
        protected virtual void ShowChildren(params object[] args)
        {
            if (_childrenViews != null && _childrenViews.Count > 0)
            {
                for (int i = 0; i < _childrenViews.Count; i++)
                    _childrenViews[i].Show(args);
            }
        }

        protected virtual void HideChildren()
        {
            if (_childrenViews != null && _childrenViews.Count > 0)
            {
                for (int i = 0; i < _childrenViews.Count; i++)
                    _childrenViews[i].Hide();
            }
        }

        protected void AddChildren(UIBaseView children)
        {
            if (_childrenViews.Contains(children))
                return;
            _childrenViews.Add(children);
        }

        protected void RemoveChildren(UIBaseView children)
        {
            if (_childrenViews == null || !_childrenViews.Contains(children))
                return;
            _childrenViews.Remove(children);
        }

        protected virtual void DiposeChildren()
        {
            if (_childrenViews != null)
            {
                for (int i = 0; i < _childrenViews.Count; i++)
                    _childrenViews[i].Dispose();
                _childrenViews.Clear();
            }
        }
        #endregion

        #region uiEffect operate
        protected Dictionary<string, Queue<UIEffectView>> _dictEffectPool = new Dictionary<string, Queue<UIEffectView>>();

        protected void CreateFixedEffect(GameObject effectObject, int sortOrder, SortObjType sortType = SortObjType.Particle)
        {
            UIEffectView effect = new UIEffectView(sortOrder, sortType);
            effect.SetDisplayObject(effectObject);
            _lstFixedEffects.Add(effect);
        }

        protected UIEffectView CreateUIEffect(GameObject effectObject, int sortOrder, bool blNeedCloneObj = false, SortObjType sortType = SortObjType.Particle)
        {
            if (_dictEffectPool.ContainsKey(effectObject.name))
            {
                Queue<UIEffectView> queue = _dictEffectPool[effectObject.name];
                if (queue.Count > 0)
                    return queue.Dequeue();
            }
            UIEffectView effect = new UIEffectView(sortOrder, sortType);
            if (blNeedCloneObj)
                effect.SetDisplayObject(GameObject.Instantiate(effectObject));
            else
                effect.SetDisplayObject(effectObject);
            return effect;
        }

        public void ReturnUIEffect(UIEffectView effect, bool blCache = true)
        {
            if (blCache)
            {
                Queue<UIEffectView> queue;
                if (_dictEffectPool.ContainsKey(effect.mEffectName))
                {
                    queue = _dictEffectPool[effect.mEffectName];
                }
                else
                {
                    queue = new Queue<UIEffectView>();
                    _dictEffectPool.Add(effect.mEffectName, queue);
                }
                effect.StopEffect();
                ObjectHelper.AddChildToParent(effect.mTransform, mTransform, false);
                queue.Enqueue(effect);
                if (_lstFixedEffects.Contains(effect))
                    _lstFixedEffects.Remove(effect);
            }
            else
            {
                effect.Dispose();
            }
        }

        protected virtual void DisposeUIEfectPool()
        {
            if (_dictEffectPool != null)
            {
                Queue<UIEffectView> queue;
                foreach (var kv in _dictEffectPool)
                {
                    queue = kv.Value;
                    while (queue.Count > 0)
                        queue.Dequeue().Dispose();
                }
                _dictEffectPool.Clear();
                _dictEffectPool = null;
            }
            if (_lstFixedEffects != null)
            {
                for (int i = 0; i < _lstFixedEffects.Count; i++)
                    _lstFixedEffects[i].Dispose();
                _lstFixedEffects.Clear();
                _lstFixedEffects = null;
            }
        }
        #endregion

        #region delay function

        protected void DelayCall(float time, Action callMethod)
        {
            DelayTimerData tData = new DelayTimerData();
            tData.mDelayTime = time;
            tData.onMethod = callMethod;
            RunDelayCall(tData);
        }

        protected void DelayCall<T>(float time, T tParam, Action<T> callMethod)
        {
            DelayTimerData<T> tData = new DelayTimerData<T>();
            tData.mDelayTime = time;
            tData.TParamData = tParam;
            tData.onMethod = callMethod;
            RunDelayCall(tData);
        }

        protected void DelayCall<T, K>(float time, T tParam, K kParam, Action<T, K> callMethod)
        {
            DelayTimerData<T, K> tData = new DelayTimerData<T, K>();
            tData.mDelayTime = time;
            tData.TParamData = tParam;
            tData.KParamData = kParam;
            tData.onMethod = callMethod;
            RunDelayCall(tData);
        }

        private Queue<DelayTimer> _delayTimerPool;
        protected virtual void RunDelayCall(AbsDelayTimerData data)
        {
            DelayTimer timer;
            if (_delayTimerPool.Count > 0)
                timer = _delayTimerPool.Dequeue();
            else
                timer = new DelayTimer(ReturnDelayTimer);
            GameComponent.Instance.AddUpdateComponent(timer);
            timer.Start(data);
        }

        private void ReturnDelayTimer(DelayTimer timer)
        {
            GameComponent.Instance.RemoveUpdateComponent(timer);
            _delayTimerPool.Enqueue(timer);
        }
        #endregion

        public virtual void Show(params object[] args)
        {
            if (mDisplayObject == null)
            {
                LogHelper.LogWarning("show ui, but displayobject is null");
                return;
            }

            if (!mBlShow)
            {
                AddEvent();
                if (!mDisplayObject.activeSelf)
                {
                    mDisplayObject.SetActive(true);
                }
                if (_lstFixedEffects != null)
                {
                    for (int i = 0; i < _lstFixedEffects.Count; i++)
                        _lstFixedEffects[i].PlayEffect();
                }
            }
            OnShowViewAnimation();
            Refresh(args);
            mBlShow = true;
            if (_sortOrder > 0)
                AdjustSortOrder(_sortOrder);
        }

        public virtual void Hide()
        {
            if (mDisplayObject == null)
                return;
            HideChildren();
            if (mBlShow)
            {
                RemoveEvent();
                if (mDisplayObject.activeSelf)
                    mDisplayObject.SetActive(false);
                if (_lstFixedEffects != null)
                {
                    for (int i = 0; i < _lstFixedEffects.Count; i++)
                        _lstFixedEffects[i].StopEffect();
                }
            }
            mBlShow = false;
        }

        private Canvas _canvas = null;
        protected virtual void AdjustSortOrder(int order)
        {
            if (_canvas == null)
            {
                if (mDisplayObject.GetComponent<Canvas>() != null)
                {
                    _canvas = mDisplayObject.GetComponent<Canvas>();
                }
                else
                {
                    _canvas = mDisplayObject.AddComponent<Canvas>();
                    mDisplayObject.AddComponent<GraphicRaycaster>();//GraphicRaycaster caster
                }

                _canvas.overrideSorting = true;
                _canvas.sortingLayerName = "TopLayer";
                _canvas.sortingOrder = order;
            }
        }


        public virtual void Dispose()
        {
            DiposeChildren();
            RemoveEvent();
            DisposeGameObject();
            DisposeUIEfectPool();
            if (_delayTimerPool != null)
            {
                while (_delayTimerPool.Count > 0)
                    _delayTimerPool.Dequeue().Dispose();
                _delayTimerPool = null;
            }
            mBlShow = false;
            _displayObject = null;
            _transform = null;
            _childrenViews = null;
        }

        protected virtual void DisposeGameObject()
        {
            if (mDisplayObject != null)
                GameObject.Destroy(mDisplayObject);
        }

        protected GameObject Find(string name)
        {
            Transform tf = mRectTransform.Find(name);
            if (tf != null)
                return tf.gameObject;
            return null;
        }

        protected T Find<T>(string name) where T : UnityEngine.Object
        {
            GameObject obj = Find(name);
            if (obj != null)
                return obj.GetComponent<T>();
            return null;
        }

        private GameObject _displayObject;

        public GameObject mDisplayObject
        {
            get { return _displayObject; }
        }

        private RectTransform _rectTransform;
        public RectTransform mRectTransform
        {
            get { return _rectTransform; }
        }

        private Transform _transform;
        public Transform mTransform
        {
            get { return _transform; }
        }

        protected T FindOnSelf<T>() where T : UnityEngine.Object
        {
            return mDisplayObject.GetComponent<T>();
        }

        protected virtual void ParseComponent() { }
        protected virtual void AddEvent() { }
        protected virtual void RemoveEvent() { }
        protected virtual void Refresh(params object[] args)
        {
        }
        protected virtual void OnShowViewAnimation() { }

    }

}