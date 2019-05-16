namespace NewBieGuide
{
    public abstract class NewBieStepBase
    {
        public GuideType mGuideType { get; protected set; }
        public int mGuildID { get; protected set; }
    
        public NewBieStepBase()
        {

        }

        public void Start()
        {
            OnStart();
            AddEvent();
        }

        public void End()
        {
            RemoveEvent();
        }

        protected virtual void AddEvent()
        {

        }

        protected virtual void RemoveEvent()
        {

        }
        
        protected abstract void OnStart();
    }
}
