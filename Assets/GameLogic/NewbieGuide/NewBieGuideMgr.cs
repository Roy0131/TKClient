using UnityEngine;
using System.Collections.Generic;

namespace NewBieGuide
{
    public class NewBieGuideMgr : Singleton<NewBieGuideMgr>
    {
        public bool mBlGuideForce { get; set; }
        private Dictionary<int, Transform> _dictMaskTransform;
        private Dictionary<int, Transform> _dictLineupCardTransform;
        private GuideDataVO _curDataVO;
        private bool _blFinished;

        private int _guideIndex;
        private int _stepIndex;

        public int mLineupPos { get; set; }
        public void Init()
        {
            _blInited = false;
            GuideDataModel.Instance.Init();
            _dictMaskTransform = new Dictionary<int, Transform>();
            _dictLineupCardTransform = new Dictionary<int, Transform>();
            AddEvent();
        }

        public void ResetGuideData()
        {
            _blInited = false;
        }

        private bool CheckGuideIdAndStepID(int guideIdx, int stepIdx)
        {
            _guideIndex = guideIdx;
            _stepIndex = stepIdx;
            if (GuideDataModel.Instance.CheckIndexValid(guideIdx))
            {
                GuideDataVO vo = GuideDataModel.Instance.GetGuideDataVO(guideIdx);
                if(vo.CheckIndexValid(stepIdx))
                {
                    GuideStepDataVO stepvo = vo.GetStepVO(stepIdx);
                    if (stepIdx < vo.mKeyStepId - 1)
                        return false;
                    else
                        return CheckGuideIdAndStepID(guideIdx + 1, 0);
                }
                else
                {
                    return CheckGuideIdAndStepID(guideIdx + 1, 0);
                }
            }
            return true;
        }

        private void AddEvent()
        {
            GameEventMgr.Instance.mGuideDispatcher.AddEvent(GuideEvent.GuideStepComplete, OnStepComplete);
            GameEventMgr.Instance.mGlobalDispatcher.AddEvent(GameEventMgr.GEventClearAllData, ResetGuideData);
        }

        private bool _blStarted;
        public void OnEnterGame()
        {
            GameUIMgr.Instance.LockMainMapDrag(false);
            if (!GameDriver.Instance.mShowGuide)
                return;
            if (_blInited)
                return;
            _blInited = true;
            _blFinished = CheckGuideIdAndStepID(LocalDataMgr.NewBieGuideID - 1, LocalDataMgr.NewBieGuildStepID - 1);
            if (_blFinished)
            {
                LogHelper.LogWarning("[NewBieGuideMgr.Init() => newbie guide was finished!!!]");
                //GuideDataModel.Instance.Dispose();
                GuideAllEnd();
                return;
            }
            _stepIndex = 0;
            _curDataVO = GuideDataModel.Instance.GetGuideDataVO(_guideIndex);
            GuideStepDataVO stepVO = _curDataVO.GetStepVO(_stepIndex);
            LocalDataMgr.NewBieGuideID = _guideIndex + 1;
            CheckCameraStatue();
            //Debuger.LogWarning("[NewBiewGuideMgr.OnEnterGame() => guideIndex:" + _guideIndex + ", stepIndex:" + _stepIndex + "]");
            GuideUIMgr.Instance.Show(stepVO);
            TDPostDataMgr.Instance.DoNewBieStart(LocalDataMgr.NewBieGuideID, _stepIndex + 1);
        }

        private void CheckCameraStatue()
        {
            if (_curDataVO.mCameraType == 0)
                return;
            MainMapMgr.Instance.SetGuideCameraType(_curDataVO.mCameraType);
        }

        private void OnStepComplete()
        {
            _stepIndex++;
            if(!_curDataVO.CheckIndexValid(_stepIndex))
            {
                _guideIndex++;
                if(!GuideDataModel.Instance.CheckIndexValid(_guideIndex))
                {
                    LocalDataMgr.NewBieGuideID = _guideIndex + 2;
                    LocalDataMgr.NewBieGuildStepID = 1;
                    GuideAllEnd();
                    mBlGuideForce = false;
                    GuideDataModel.Instance.SaveGuideData();
                    LogHelper.LogWarning("[NewBieGuideMgr.OnStepComplete() => all guide was complete!!!]");
                    return;
                }
                _stepIndex = 0;
                LocalDataMgr.NewBieGuideID = _guideIndex + 1;
                LocalDataMgr.NewBieGuildStepID = 1;
                _curDataVO = GuideDataModel.Instance.GetGuideDataVO(_guideIndex);
                CheckCameraStatue();
                GuideDataModel.Instance.SaveGuideData();
            }
            GuideStepDataVO vo = _curDataVO.GetStepVO(_stepIndex);
            int tmp = 5;
            if(vo.mCondition != 0)
            {
                while (GuideCondHelper.CheckCondition(vo.mCondition))
                {
                    _stepIndex = vo.mJumpNextId - 1;
                    vo = _curDataVO.GetStepVO(_stepIndex);
                    if (tmp <= 0)
                        break;
                    tmp--;
                }
            }
            //Debuger.LogWarning("[NewBiewGuideMgr.OnStepEnd() => guideIndex:" + _guideIndex + ", stepIndex:" + _stepIndex + "]");
            GuideUIMgr.Instance.Show(vo);
            TDPostDataMgr.Instance.DoNewBieStart(LocalDataMgr.NewBieGuideID, _stepIndex + 1);
        }

        private void RemoveEvent()
        {
            //HeroDataModel.Instance.RemoveEvent(HeroEvent.HeroEnterGameFinished, OnEnterGame);
            GameEventMgr.Instance.mGuideDispatcher.RemoveEvent(GuideEvent.GuideStepComplete, OnStepComplete);
        }

        #region mask transform
        public void RegistMaskTransform(int maskID, Transform transform)
        {
            //if (_blFinished)
            //    return;
            if (_dictMaskTransform.ContainsKey(maskID))
            {
                LogHelper.LogWarning("[NewBieGuideMgr.RegistMaskTransfrom() => registe mask transform repeated, maskId:" + maskID + "]");
                //return;
                _dictMaskTransform.Remove(maskID);
            }
            _dictMaskTransform.Add(maskID, transform);
        }

        public void UnRegistMaskTransform(int maskID)
        {
            //if (_blFinished)
            //    return;
            if (!_dictMaskTransform.ContainsKey(maskID))
            {
                LogHelper.LogWarning("[NewBieGuideMgr.UnRegistMaskTransform() => mask transform unregisted!!!]");
                return;
            }
            _dictMaskTransform.Remove(maskID);
        }

        public Transform GetMaskTransform(int maskID)
        {
            //if (_blFinished)
            //    return null;
            if (_dictMaskTransform.ContainsKey(maskID))
                return _dictMaskTransform[maskID];
            return null;
        }
        #endregion;

        #region lineup card transform
        public void RegisteLineupCardTransform(int pos, Transform transform)
        {
            //if (_blFinished)
            //    return;
            if (_dictLineupCardTransform.ContainsKey(pos))
                return;
            _dictLineupCardTransform.Add(pos, transform);
        }

        public void UnRegisteLineupCardTransform(int pos)
        {
            //if (_blFinished)
            //    return;
            if (_dictLineupCardTransform.ContainsKey(pos))
                _dictLineupCardTransform.Remove(pos);
        }

        public Transform GetLineupCardTransform(int pos)
        {
            //if (_blFinished)
            //    return null;
            if (_dictLineupCardTransform.ContainsKey(pos))
                return _dictLineupCardTransform[pos];
            return null;
        }
        #endregion

        private void GuideAllEnd()
        {
            //RemoveEvent();
            //_curDataVO = null;
            //if (_dictMaskTransform != null)
            //{
            //    _dictMaskTransform.Clear();
            //    _dictMaskTransform = null;
            //}
            //if (_dictLineupCardTransform != null)
            //{
            //    _dictLineupCardTransform.Clear();
            //    _dictLineupCardTransform = null;
            //}
            GuideUIMgr.Instance.Dispose();
            //GuideDataModel.Instance.Dispose();
            //_blFinished = true;
        }
    }
}