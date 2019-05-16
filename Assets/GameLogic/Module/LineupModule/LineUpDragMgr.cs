using UnityEngine;
using UnityEngine.EventSystems;

public enum DragStatus
{
    None,
    DragCard,
    DragLineupFighter,
}

public class LineUpDragMgr : Singleton<LineUpDragMgr>
{
    private LineupFighter _dragFighter;

    private DragStatus _curDragStatus = DragStatus.None;
    private int _index;
    private RectTransform _dragRect;
    private bool _blInRange = false;
    private RectTransform _fighterViewRect;
    private RectTransform _fighterAreanRect;
    private RectTransform _cardArenaRect;
    private CardView _cardView;

    public void Init()
    {
        InputController.Instance.AddInputEvent(InputEventType.MouseDrag, OnMove);
    }

    private void OnMove(Vector2 pos)
    {
        //Debuger.LogWarning("move...");
        if (_curDragStatus == DragStatus.None)
            return;
        // transform the screen point to world point int rectangle  
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_dragRect, Input.mousePosition, GameUIMgr.Instance.mUICamera, out globalMousePos))
        {
            if (_curDragStatus == DragStatus.DragCard)
                _dragRect.position = globalMousePos;
            if (_cardView != null)
                _cardView.mRectTransform.position = globalMousePos;

            if (_fighterViewRect == null)
            {
                _fighterViewRect = GameObject.Find("FighterView").transform.Find("RawImage") as RectTransform;
                _fighterAreanRect = GameObject.Find("FighterView").transform.Find("FightArean") as RectTransform;
                _cardArenaRect = GameObject.Find("HeroCardView").transform.Find("ScrollView") as RectTransform;
            }
            Vector2 screenPos;
            //计算出鼠标相对于ui中的 rendererTexture的坐标
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_fighterViewRect, Input.mousePosition, GameUIMgr.Instance.mUICamera, out screenPos);
            //计算出的坐标还是以中间为0,0点，则需要分别减去尺寸的一半，得到以左下角为0，0的坐标，此坐标已经是3D相机的屏幕坐标了
            screenPos.x += _fighterViewRect.sizeDelta.x / 2;
            screenPos.y += _fighterViewRect.sizeDelta.y / 2;

            Vector3 p3 = RoleRTMgr.Instance.mCamera.ScreenToWorldPoint(screenPos);//LineupSceneMgr.Instance.mLineupCamera.ScreenToWorldPoint(screenPos);
            _dragFighter.UpdatePosition(p3);

            bool blInRange = RectTransformUtility.RectangleContainsScreenPoint(_fighterAreanRect, Input.mousePosition, GameUIMgr.Instance.mUICamera);
            //LogHelper.Log("blInRange:" + blInRange);
            if (_blInRange != blInRange)
            {
                _blInRange = blInRange;
                if (_curDragStatus == DragStatus.DragCard)
                    _dragRect.gameObject.SetActive(!_blInRange);
//                else if (_curDragStatus == DragStatus.DragLineupFighter)
//                    _dragFighter.mUnitRoot.gameObject.SetActive(_blInRange);
                if (_cardView != null)
                    _cardView.mDisplayObject.SetActive(!_blInRange);
            }
            
            bool blInCardView = RectTransformUtility.RectangleContainsScreenPoint(_cardArenaRect, Input.mousePosition, GameUIMgr.Instance.mUICamera);
            _dragFighter.mUnitRoot.gameObject.SetActive(!blInCardView);
        }
    }

    public void HideDragFighter()
    {
        if (_dragFighter == null)
            return;
        _dragFighter.mUnitRoot.gameObject.SetActive(false);
    }

    public CardView mDragCardView { get; set; }

    public void StartDrag(PointerEventData evtData, DragHelper dragHelper, DragStatus dragStatus)
    {
        _curDragStatus = dragStatus;
        _dragRect = dragHelper.transform as RectTransform;
        if (_curDragStatus == DragStatus.DragCard)
        {
            GameUIMgr.Instance.AddObjectToTopRoot(_dragRect);
            if (_dragFighter != null)
            {
                _dragFighter.Dispose();
                _dragFighter = null;
            }
            _dragFighter = LineupSceneMgr.Instance.CreateDragFighter(mDragCardView.mCardDataVO);
            _dragFighter.OnDrag();
            _blInRange = false;
        }
        else
        {
            string name = dragHelper.gameObject.name;
            string idxStr = name.Substring(name.Length - 1, 1);
            int idx = int.Parse(idxStr);
            LineupFighter fighter = LineupSceneMgr.Instance.GetFighterDataByIndex(idx);
            if (fighter == null)
            {
                _curDragStatus = DragStatus.None;
                return;
            }
            _index = idx;
            _curDragStatus = DragStatus.DragLineupFighter;
            _dragFighter = LineupSceneMgr.Instance.CreateDragFighter(fighter.mCardDataVO);
            _dragFighter.OnDrag();
            fighter.mUnitRoot.gameObject.SetActive(false);
            _blInRange = true;

            _cardView = CardViewFactory.Instance.CreateCardView(fighter.mCardDataVO, CardViewType.Lineup);
            GameUIMgr.Instance.AddObjectToTopRoot(_cardView.mRectTransform);
            _cardView.mDisplayObject.SetActive(false);
        }
        OnMove(Vector2.zero);
    }

    public void EndDrag()
    {
        if (_curDragStatus == DragStatus.None || _dragRect == null)
            return;
        CardDataVO vo = null;
        if (_dragFighter != null)
        {
            vo = _dragFighter.mCardDataVO;
            _dragFighter.Dispose();
            _dragFighter = null;
        }
        if (_curDragStatus == DragStatus.DragCard)
        {
            Ray ray = GameUIMgr.Instance.mUICamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] raycasts = Physics.RaycastAll(ray);
            if (raycasts != null)
            {
                int index = -1;
                string name;
                for (int i = 0; i < raycasts.Length; i++)
                {
                    if (raycasts[i].collider.gameObject.name.Contains("collider"))
                    {
                        name = raycasts[i].collider.gameObject.name;
                        string idxStr = name.Substring(name.Length - 1, 1);
                        index = int.Parse(idxStr);
                        break;
                    }
                }
                if (index != -1)
                {
                    LineupFighter fighter = LineupSceneMgr.Instance.GetFighterDataByIndex(index);
                    if (fighter != null)
                    {
                        if (!LineupSceneMgr.Instance.IsAssetFighter(fighter))
                        { 
                            if (!NewBieGuide.NewBieGuideMgr.Instance.mBlGuideForce)
                            {
                                LineupSceneMgr.Instance.RemoveFighterByIndex(index);
                                LineupSceneMgr.Instance.LineupCreateNewFighter(index, vo);
                                GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.GuideLineupFighter, index);
                            }
                        }
                    }
                    else
                    {
                        
                        if (LineupSceneMgr.Instance.OnDragCount() < LineupSceneMgr.Instance.OnGetMaxRole())
                        {
                            LineupSceneMgr.Instance.LineupCreateNewFighter(index, vo);
                            GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.GuideLineupFighter, index);
                        }
                        else
                        {
                            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001256) + LineupSceneMgr.Instance.OnGetMaxRole());
                        }
                    }
                }
            }
            mDragCardView = null;
        }
        else
        {
            if (!_blInRange)
            {
                //remove fighter;
                if (!NewBieGuide.NewBieGuideMgr.Instance.mBlGuideForce)
                    LineupSceneMgr.Instance.RemoveFighterByIndex(_index);
                else
                {
                    LineupFighter fighter = LineupSceneMgr.Instance.GetFighterDataByIndex(_index);
                    if (fighter != null)
                        fighter.mUnitRoot.gameObject.SetActive(true);
                }
            }
            else
            {
                Ray ray = GameUIMgr.Instance.mUICamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] raycasts = Physics.RaycastAll(ray);
                if (raycasts != null)
                {
                    int index = -1;
                    string name;
                    for (int i = 0; i < raycasts.Length; i++)
                    {
                        if (raycasts[i].collider.gameObject.name.Contains("collider"))
                        {
                            name = raycasts[i].collider.gameObject.name;
                            string idxStr = name.Substring(name.Length - 1, 1);
                            index = int.Parse(idxStr);
                            break;
                        }
                    }
                    if (index != -1)
                    {
                        if (index != _index)
                        {
                            _dragRect.gameObject.SetActive(true);
                            LineupSceneMgr.Instance.ChangLineFighter(_index, index);
                            _dragRect = null;
                            _curDragStatus = DragStatus.None;
                            if (_cardView != null)
                            {
                                CardViewFactory.Instance.ReturnCardView(_cardView);
                                _cardView = null;
                            }
                            GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(UIEventDefines.LineupChangeIndex);
                            GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.GuideChangeFighter, _index);
                            return;
                        }
                    }

                }
                LineupFighter fighter = LineupSceneMgr.Instance.GetFighterDataByIndex(_index);
                fighter.mUnitRoot.gameObject.SetActive(true);
            }
        }
        _dragRect.gameObject.SetActive(true);
        _dragRect = null;
        _curDragStatus = DragStatus.None;
        if (_cardView != null)
        {
            CardViewFactory.Instance.ReturnCardView(_cardView);
            _cardView = null;
        }
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(UIEventDefines.LineupChangeIndex);
    }

    public void Dispose()
    {
        if (_dragFighter != null)
        {
            _dragFighter.mUnitRoot.gameObject.SetActive(false);
            _dragFighter = null;
        }
        if (_dragRect != null)
        {
            _dragRect.gameObject.SetActive(false);
            _dragRect = null;
        }
        InputController.Instance.RemoveInputEvent(InputEventType.MouseDrag, OnMove);
    }
}
