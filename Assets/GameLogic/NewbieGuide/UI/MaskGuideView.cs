using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Spine.Unity;
using Framework.UI;

namespace NewBieGuide
{
    public class MaskGuideView : UIBaseView
    {
        private Material _maskMaterial;
        private RectTransform _npcObject;
        private RectTransform _dialogObject;
        private Text _dialogText;
        private DialogStyle _curStyle;
        private GuideStepDataVO _vo;
        private Vector3 _maskScreenPos;
        private int _radius = 100;
        private RectPoint _rectPoint;
        private GameObject _maskObject;
        private RectTransform _pointerRect;
        private Image _maskMatImage;

        private SkeletonGraphic _pointerGraphic;
        private GuideLineupLogic _specialLogic;
        private bool _blEnter = false;

        protected override void ParseComponent()
        {
            base.ParseComponent();

            _maskMatImage = Find<Image>("MaskImage");
            _maskMaterial = _maskMatImage.material;

            _dialogObject = Find<RectTransform>("DialogObject");
            _npcObject = Find<RectTransform>("dc");//("dc");//xinshouyindao
            _dialogText = Find<Text>("DialogObject/Text");
            _pointerRect = Find<RectTransform>("dianji");
            _pointerGraphic = Find<SkeletonGraphic>("dianji");

            RegisteSpecialLogic();
        }

        #region guide special logic
        private void RegisteSpecialLogic()
        {
            _specialLogic = new GuideLineupLogic();
            _specialLogic.OnLogicEnd = OnSpeicalEnd;
        }

        private void OnSpeicalEnd()
        {
            _maskMatImage.raycastTarget = true;
            _pointerGraphic.AnimationState.SetAnimation(0, "animation", true);
            _pointerRect.localScale = Vector3.one;
            EndStep();
        }

        private void ShowSpecialAnimation()
        {
            Transform tf = null;
            if (_vo.mSpecialLogicId != 0)
            {
                if(_vo.mSpecialLogicId == GuideSpecialID.GuideSpecial3)
                {
                    _pointerGraphic.AnimationState.SetAnimation(0, "animation3", true);
                    tf = NewBieGuideMgr.Instance.GetLineupCardTransform(3);
                    if (NewBieGuideMgr.Instance.mLineupPos <= 3)
                        _pointerRect.localScale = new Vector3(-1f, 1f, 1f);
                    else
                        _pointerRect.localScale = Vector3.one;
                }
                else
                {
                    _pointerGraphic.AnimationState.SetAnimation(0, "animation2", true);
                    int idx = _vo.mSpecialLogicId == GuideSpecialID.GuideLineUp ? 0 : 1; //_dictSpecialLogic.Add(GuideSpecialID.GuideLineUp, logicBase);
                    tf = NewBieGuideMgr.Instance.GetLineupCardTransform(idx);
                }

            }
            if (tf == null)
                return;
            RectTransform uiRect = tf as RectTransform;
            Vector3 uiScreenPos = GameUIMgr.Instance.mUICamera.WorldToScreenPoint(uiRect.position);
            Vector2 uiPos = GameUIMgr.Instance.ScreenPosToLocalPos(uiScreenPos);
            _pointerRect.gameObject.SetActive(true);
            _pointerRect.anchoredPosition = uiPos;
        }
        #endregion

        private void DoEnterLogic()
        {
            if (_vo.mBattleState == 1)
                BattleManager.Instance.PauseBattle();
            else if (_vo.mBattleState == 2)
                BattleManager.Instance.StartBattle();
            _blEnter = true;
            if (_vo.mDialogID != 0)
            {
                string dialogValue = LanguageMgr.GetLanguage(_vo.mDialogID);
                _dialogText.text = dialogValue;
                AdjustDialogPosition(_vo.mDialogStyle);
                _dialogObject.gameObject.SetActive(true);
                _npcObject.gameObject.SetActive(true);
            }
            else
            {
                _dialogObject.gameObject.SetActive(false);
            }

            if (_vo.mSpecialLogicId != 0)
            {
                _maskMatImage.raycastTarget = false;
                _specialLogic.Run(_vo.mSpecialLogicId);
                ShowSpecialAnimation();
                return;
            }
            if (_vo.mGuideMaskType == GuideMaskType.None)
            {
                ShowWaitStates();
                return;
            }   
            FillMask();
        }

        protected override void Refresh(params object[] args)
        {
            base.Refresh(args);
            _waitingTime = -1f;
            _blEnter = false;
            _vo = args[0] as GuideStepDataVO;
            if (_vo.mSignOut == 1)
                _pointerRect.rotation = Quaternion.Euler(0f, 0f, -90f);
            else if(_vo.mSignOut == 2)
                _pointerRect.rotation = Quaternion.Euler(180f, 0f, 135f);
            else
                _pointerRect.rotation = Quaternion.Euler(0f, 0f, 0f);
            MainMapMgr.Instance.Enable = _vo.mOpenMainMapDrag == 1;
            _pointerRect.gameObject.SetActive(false);
            if (_vo.mEnterCondId != 0 && !GuideCondHelper.CheckEnterCondition(_vo.mEnterCondId))
            {
                LocalDataMgr.NewBieGuildStepID = _vo.mStepID;
                GuideDataModel.Instance.SaveGuideData();
                ShowWaitStates();
            }
            else
            {
                DoEnterLogic();
            }
        }

        #region mask logic
        private void FillMask()
        {
            Vector4 center = Vector4.zero;
            Vector3 uiPos;

            Transform maskTf = NewBieGuideMgr.Instance.GetMaskTransform(_vo.mMaskID);
            if (maskTf == null && _vo.mGuideMaskType != GuideMaskType.Global)
            {
                LogHelper.LogError("[MaskGuideView.FillMask() => mask transform not found, mask id:" + _vo.mMaskID + "]");
                return;
            }
            if (_rectPoint == null)
                _rectPoint = new RectPoint();
            _maskObject = null;
            _maskMatImage.raycastTarget = true;
            if (_vo.mGuideMaskType == GuideMaskType.Circle)
            {
                if (maskTf is RectTransform)
                {
                    _maskScreenPos = GameUIMgr.Instance.mUICamera.WorldToScreenPoint(maskTf.position);
                    uiPos = GameUIMgr.Instance.ScreenPosToLocalPos(_maskScreenPos);
                }
                else
                {
                    uiPos = GameUIMgr.Instance.WorldToUIPoint(maskTf.position);
                    _maskScreenPos = Camera.main.WorldToScreenPoint(maskTf.position);
                }

                center = new Vector4(uiPos.x, uiPos.y, 0f, 0f);
                if (maskTf is RectTransform)
                    _radius = 50;
                else
                    _radius = 100;
                _maskMaterial.SetVector("_Center", center);
                //_maskMaterial.EnableKeyword("_ROUNDMODE_ROUND");
                //_maskMaterial.SetFloat("_RoundMode", 1);
                _maskMaterial.SetInt("_Radius", _radius);
                _maskMaterial.DisableKeyword("_IsRect");
                _pointerRect.gameObject.SetActive(true);
                _pointerRect.anchoredPosition = uiPos;
            }
            else
            {
                float width = 0f, height = 0f;
                if (_vo.mGuideMaskType == GuideMaskType.Rect)
                {
                    RectTransform uiRect = maskTf as RectTransform;
                    Vector3 uiScreenPos = GameUIMgr.Instance.mUICamera.WorldToScreenPoint(uiRect.position);
                    uiPos = GameUIMgr.Instance.ScreenPosToLocalPos(uiScreenPos);
                    center = new Vector4(uiPos.x, uiPos.y, 0f, 0f);

                    width = uiRect.sizeDelta.x * uiRect.localScale.x;
                    height = uiRect.sizeDelta.y * uiRect.localScale.y;
                    width /= 2f;
                    height /= 2f;
                    width += 20;
                    height += 20;
                    _rectPoint.Init(uiScreenPos.x, uiScreenPos.y, width, height);
                    _maskObject = maskTf.gameObject;
                    _pointerRect.gameObject.SetActive(true);
                    _pointerRect.anchoredPosition = uiPos;
                }
                else if (_vo.mGuideMaskType == GuideMaskType.Global)
                {
                    center = Vector4.zero;
                }
                _maskMaterial.SetVector("_Center", center);
                _maskMaterial.SetFloat("_Width", width);
                _maskMaterial.SetFloat("_Height", height);
                //_maskMaterial.EnableKeyword("_ROUNDMODE_ELLIPSE");
                //_maskMaterial.SetFloat("_RoundMode", 0);
                _maskMaterial.EnableKeyword("_IsRect");
            }
        }

        private bool _blWaiting = false;
        private float _waitingTime = -1f;
        private void OnInputClick(Vector2 pos)
        {
            pos = Input.mousePosition;
            //Debug.Log("[OnInputClick, pos:" + pos + ", maskType:" + _vo.mGuideMaskType + ", _blwaiting:" + _blWaiting + ", _blEnter:" + _blEnter + "]");
            if (!_blEnter || _vo.mGuideMaskType == GuideMaskType.None)
                return;
            if (!_blWaiting)
            {
                bool blInRange = true;
                if (_vo.mGuideMaskType == GuideMaskType.Circle)
                {
                    int tmpRadius = (int)(Vector3.Distance(_maskScreenPos, pos));
                    //Debug.LogWarning("[_maskPos:" + _maskScreenPos + ", tmpRadius:" + tmpRadius + ", _radius:" + _radius + "]");
                    blInRange = tmpRadius <= _radius;
                }
                else if (_vo.mGuideMaskType == GuideMaskType.Rect)
                {
                    blInRange = _rectPoint.InRange(pos);
                    //Debug.LogWarning("range value:" + blInRange);
                }
                if (!blInRange)
                    return;
            }
            else
            {
                //if (_waitingTime <= 0f)
                //    return;
                if (Time.deltaTime - _waitingTime < 3f)
                    return;
            }
            LocalDataMgr.NewBieGuildStepID = _vo.mStepID;
            if (_vo.mGuideMaskType == GuideMaskType.Global)
            {
                EndStep();
            }
            else
            {
                _blWaiting = true;
                _waitingTime = Time.deltaTime;
                if (_vo.mModuleId != 0)
                {
                    switch (_vo.mModuleId)
                    {
                        case 1:
                            RecruitDataModel.Instance.ReqDrawCardList();
                            break;
                        case 2:
                            HangupDataModel.Instance.ReqCampaignData();
                            break;
                        case 3:
                            GameUIMgr.Instance.OpenModule(ModuleID.Bag);
                            break;
                        case 4:
                            GameUIMgr.Instance.OpenModule(ModuleID.RoleDecompose);
                            break;
                        case 5:
                            GameUIMgr.Instance.OpenModule(ModuleID.RoleBag);
                            break;
                        case 8:
                            GameUIMgr.Instance.OpenModule(ModuleID.Attendance);
                            break;
                        case 9:
                            GameUIMgr.Instance.OpenModule(ModuleID.Equipment);
                            break;
                    }
                }
                if (_maskObject != null)
                {
                    ExecuteEvents.Execute(_maskObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
                }
                _pointerRect.gameObject.SetActive(false);
                ShowWaitStates();
                if (_vo.mEndConditionId == 0)
                    DelayCall(0.3f, EndStep);
            }
        }

        private void ShowWaitStates()
        {
            _maskMaterial.SetVector("_Center", Vector4.zero);
            _maskMaterial.SetFloat("_Width", 1332);
            _maskMaterial.SetFloat("_Height", 748);
            _maskMaterial.EnableKeyword("_IsRect");
            //_maskMaterial.EnableKeyword("_ROUNDMODE_ROUND");
            //_maskMaterial.EnableKeyword("_ROUNDMODE_ELLIPSE");
            //_maskMaterial.SetFloat("_RoundMode", 0);
            _dialogObject.gameObject.SetActive(false);
            _npcObject.gameObject.SetActive(false);
            _maskMatImage.raycastTarget = _vo.mWaitMaskStates == 1;
        }
        #endregion

        #region dialog position
        private void AdjustDialogPosition(DialogStyle style)
        {
            if (_curStyle == style)
                return;
            _curStyle = style;
            //if(GameDriver.Instance.mBlRunHot)
            //{
                //switch (_curStyle)
                //{
                //    case DialogStyle.LeftTop:
                //        _npcObject.localScale = Vector3.one;
                //        _npcObject.anchoredPosition = new Vector2(-513f, -497f);

                //        _dialogObject.anchoredPosition = new Vector2(141f, 50f);
                //        break;
                //    case DialogStyle.LeftBottom:
                //        _npcObject.localScale = Vector3.one;
                //        _npcObject.anchoredPosition = new Vector2(-513f, -497f);

                //        _dialogObject.anchoredPosition = new Vector2(141f, -276f);
                //        break;
                //    case DialogStyle.RightTop:
                //        _npcObject.localScale = new Vector3(-1f, 1f, 1f);
                //        _npcObject.anchoredPosition = new Vector2(523f, -497f);

                //        _dialogObject.anchoredPosition = new Vector2(-145f, 50f);
                //        break;
                //    case DialogStyle.RightBottom:
                //        _npcObject.localScale = new Vector3(-1f, 1f, 1f);
                //        _npcObject.anchoredPosition = new Vector2(523f, -497f);

                //        _dialogObject.anchoredPosition = new Vector2(-145f, -276f);
                //        break;
                //}
            //}
            //else
            //{
                switch (_curStyle)
                {
                    case DialogStyle.LeftTop:
                        _npcObject.localScale = Vector3.one * 2;
                        _npcObject.anchoredPosition = new Vector2(-472f, -378f);

                        _dialogObject.anchoredPosition = new Vector2(141f, 50f);
                        break;
                    case DialogStyle.LeftBottom:
                        _npcObject.localScale = Vector3.one * 2;
                        _npcObject.anchoredPosition = new Vector2(-472f, -378f);

                        _dialogObject.anchoredPosition = new Vector2(141f, -276f);
                        break;
                    case DialogStyle.RightTop:
                        _npcObject.localScale = new Vector3(-2f, 2f, 2f);
                        _npcObject.anchoredPosition = new Vector2(506f, -378f);

                        _dialogObject.anchoredPosition = new Vector2(-145f, 50f);
                        break;
                    case DialogStyle.RightBottom:
                    _npcObject.localScale = new Vector3(-2f, 2f, 2f);
                    _npcObject.anchoredPosition = new Vector2(506f, -378f);

                    _dialogObject.anchoredPosition = new Vector2(-145f, -276f);
                    break;
                }
            //}

        }
        #endregion

        protected override void AddEvent()
        {
            base.AddEvent();
            InputController.Instance.AddInputEvent(InputEventType.MouseClick, OnInputClick);
            InputController.Instance.AddInputEvent(InputEventType.MouseUp, OnInputClick);
            GameEventMgr.Instance.mGuideDispatcher.AddEvent<int>(GuideEvent.EnterCondTrigger, OnEnterTrigger);
            GameEventMgr.Instance.mGuideDispatcher.AddEvent<int>(GuideEvent.EndCondTrigger, OnEndTrigger);
        }

        private void OnEnterTrigger(int conditionId)
        {
            if (_vo.mEnterCondId == conditionId)
                DoEnterLogic();
        }

        private void OnEndTrigger(int conditionId)
        {
            if (_vo.mEndConditionId == conditionId)
                EndStep();
        }

        protected override void RemoveEvent()
        {
            base.RemoveEvent();
            InputController.Instance.RemoveInputEvent(InputEventType.MouseClick, OnInputClick);
            InputController.Instance.RemoveInputEvent(InputEventType.MouseUp, OnInputClick);
            GameEventMgr.Instance.mGuideDispatcher.RemoveEvent<int>(GuideEvent.EnterCondTrigger, OnEnterTrigger);
            GameEventMgr.Instance.mGuideDispatcher.RemoveEvent<int>(GuideEvent.EndCondTrigger, OnEndTrigger);
        }

        private void EndStep()
        {
            _blWaiting = false;
            GuideDataModel.Instance.SaveGuideData();
            GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.GuideStepComplete);
        }

        public override void Dispose()
        {
            if (_specialLogic != null)
                _specialLogic.Dispose();
            _specialLogic = null;
            _vo = null;
            _rectPoint = null;
            _pointerGraphic = null;
            base.Dispose();
        }
    }
}
