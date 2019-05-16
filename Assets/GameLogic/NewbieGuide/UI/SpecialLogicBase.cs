using Framework.Core;
using System;

namespace NewBieGuide
{
    public class SpecialLogicBase : IDispose
    {
        public Action OnLogicEnd { get; set; }
        protected int _logicID;

        public void Run(int specialId)
        {
            _logicID = specialId;
            OnRun();
            AddEvent();
        }

        protected virtual void OnRun()
        {

        }

        protected virtual void AddEvent()
        {

        }

        protected virtual void RemoveEvent()
        {

        }
         

        protected virtual void OnEnd()
        {
            RemoveEvent();
            if (OnLogicEnd != null)
                OnLogicEnd.Invoke();
        }

        public virtual void Dispose()
        {
            RemoveEvent();
            OnLogicEnd = null;
        }
    }
}
