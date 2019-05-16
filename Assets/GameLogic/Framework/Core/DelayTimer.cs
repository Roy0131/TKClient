using System;

namespace Framework.Core
{
    public class DelayTimer : IUpdateable, IDispose
    {
        private bool _blEnable;
        private float _flTime;
        private AbsDelayTimerData _timerData;
        private Action<DelayTimer> _onEnd;
        public DelayTimer(Action<DelayTimer> onEnd)
        {
            _onEnd = onEnd;
        }

        public void Start(AbsDelayTimerData data)
        {
            _timerData = data;
            _flTime = _timerData.mDelayTime;
            _blEnable = true;
        }

        public void Update()
        {
            if (_flTime <= 0f)
            {
                OnEnd();
                return;
            }
            _flTime -= UnityEngine.Time.deltaTime;
        }

        private void OnEnd()
        {
            _timerData.DoMethod();
            _blEnable = false;
            _timerData = null;
            if (_onEnd != null)
                _onEnd.Invoke(this);
        }

        public bool blEnable
        {
            get { return _blEnable; }
        }

        public void Dispose()
        {
            _onEnd = null;
            _timerData = null;
            _blEnable = false;
        }
    }

    public abstract class AbsDelayTimerData
    {
        public float mDelayTime;
        public virtual void DoMethod()
        {

        }
    }

    public class DelayTimerData : AbsDelayTimerData
    {
        public Action onMethod;

        public override void DoMethod()
        {
            if (onMethod != null)
                onMethod.Invoke();
        }
    }

    public class DelayTimerData<T> : AbsDelayTimerData
    {
        public Action<T> onMethod;

        public T TParamData { get; set; }
        public override void DoMethod()
        {
            if (onMethod != null)
                onMethod.Invoke(TParamData);
        }
    }

    public class DelayTimerData<T, K> : AbsDelayTimerData
    {
        public Action<T, K> onMethod;
        public T TParamData { get; set; }
        public K KParamData { get; set; }

        public override void DoMethod()
        {
            if (onMethod != null)
                onMethod.Invoke(TParamData, KParamData);
        }
    }
}

