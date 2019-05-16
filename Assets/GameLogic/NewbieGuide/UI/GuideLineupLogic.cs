namespace NewBieGuide
{
    public class GuideLineupLogic : SpecialLogicBase
    {
        protected override void OnRun()
        {
            base.OnRun();
            GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.LineupButtonStatusChange, false);
        }

        protected override void AddEvent()
        {
            base.AddEvent();
            if(_logicID == GuideSpecialID.GuideSpecial3)
                GameEventMgr.Instance.mGuideDispatcher.AddEvent<int>(GuideEvent.GuideChangeFighter, OnLineupFighter);
            else
                GameEventMgr.Instance.mGuideDispatcher.AddEvent<int>(GuideEvent.GuideLineupFighter, OnLineupFighter);
        }

        protected override void RemoveEvent()
        {
            base.RemoveEvent();
            if (_logicID == GuideSpecialID.GuideSpecial3)
                GameEventMgr.Instance.mGuideDispatcher.RemoveEvent<int>(GuideEvent.GuideChangeFighter, OnLineupFighter);
            else
                GameEventMgr.Instance.mGuideDispatcher.RemoveEvent<int>(GuideEvent.GuideLineupFighter, OnLineupFighter);
        }

        private void OnLineupFighter(int index)
        {
            if (_logicID == GuideSpecialID.GuideLineUp2)
            {
                if (LineupSceneMgr.Instance.GetBattleFighterCount() < 2)
                    return;
            }
            LogHelper.LogWarning("index:" + index);
            GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.LineupButtonStatusChange, true);
            OnEnd();
        }

    }
}
