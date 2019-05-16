using UnityEngine;
using System;

namespace Framework.UI
{
    public enum SortObjType
    {
        Canvas,
        Particle,
    }
    public class UIEffectView : UpdateViewBase
    {
        protected string sortingLayerName = "TopLayer";

        protected SortObjType _sortType;
        protected Renderer[] _renders = null;
        private Canvas _canvas = null;

        private int _sortingOrder = 20;
        private bool _blRunAliveTime;
        private float _flAliveTime;


        private Action<UIEffectView> _playEndMethod;
        public UIEffectView(int sortOder, SortObjType sortType = SortObjType.Particle)
        {
            _sortingOrder = sortOder;
            _sortType = sortType;
        }

        public void PlayEffect(Action<UIEffectView> endMthod = null, float effectTime = -1)
        {
            Show();
            _playEndMethod = endMthod;
            _flAliveTime = effectTime;
            _blRunAliveTime = _flAliveTime > 0f;
        }

        public void StopEffect()
        {
            _playEndMethod = null;
            Hide();
        }

        public override void Update()
        {
            base.Update();
            if (mDisplayObject == null)
                return;
            if (_sortType == SortObjType.Particle)
            {
                if (_renders == null || _renders.Length == 0)
                {
                    _renders = mDisplayObject.GetComponentsInChildren<Renderer>(true);
                    if (_renders != null)
                    {
                        for (int i = 0; i < _renders.Length; i++)
                        {
                            _renders[i].sortingLayerName = sortingLayerName;
                            _renders[i].sortingOrder = _sortingOrder;
                        }
                    }
                }
            }
            else
            {
                if (_canvas == null)
                {
                    if (mDisplayObject.GetComponent<Canvas>() != null)
                        _canvas = mDisplayObject.GetComponent<Canvas>();
                    else
                        _canvas = mDisplayObject.AddComponent<Canvas>();

                    _canvas.overrideSorting = true;
                    _canvas.sortingLayerName = sortingLayerName;
                    _canvas.sortingOrder = _sortingOrder;
                }
            }
            if (_blRunAliveTime)
            {
                _flAliveTime -= Time.deltaTime;
                if (_flAliveTime <= 0f)
                {
                    if (_playEndMethod != null)
                        _playEndMethod.Invoke(this);
                    _playEndMethod = null;
                    _blEnable = false;
                }
            }
        }

        public override void Dispose()
        {
            _canvas = null;
            _renders = null;
            _playEndMethod = null;
            base.Dispose();
        }
    }
}