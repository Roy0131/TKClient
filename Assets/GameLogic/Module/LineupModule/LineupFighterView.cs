using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Framework.UI;

public class LineupFighterView : UIBaseView {

    private Text _text;
    private Button _btnBattle;
    private RectTransform _rectBattle;
    private Text _btnBattleText;
    private RawImage _fighterImage;
    private List<GameObject> _lstPosCollider;
    private Text _textNum;
    private Text _textTitle;

    private List<GameObject> _lstFighterFlag;
    private List<UIEffectView> _listEffect;

    protected override void ParseComponent()
    {
        base.ParseComponent();

        _text = Find<Text>("Imagefighting/Textfighting");
        _btnBattle = Find<Button>("ButtonFight");
        _rectBattle = Find<RectTransform>("ButtonFight");
        _btnBattleText = Find<Text>("ButtonFight/Text");
        _fighterImage = Find<RawImage>("RawImage");
        CreateFixedEffect(_fighterImage.gameObject, UILayerSort.PopupSortBeginner + 1, SortObjType.Canvas);

        _textNum = Find<Text>("TextNum");
        _textTitle = Find<Text>("Text");
        _lstPosCollider = new List<GameObject>();
        _lstFighterFlag = new List<GameObject>();
        _listEffect = new List<UIEffectView>();
        GameObject itemObj;
        DragHelper dragComponet;
        Dictionary<int, CardDataVO> pveCards = HeroDataModel.Instance.GetTeamCardVO(LineupSceneMgr.Instance.mLineupTeamType);
        bool tmpValue = false;
        for (int i = 0; i < 9; i++)
        {
            itemObj = Find("SetGrid/collider" + i);
            dragComponet = itemObj.AddComponent<DragHelper>();
            dragComponet.mDragMethod = OnDragMethod;
            _lstPosCollider.Add(itemObj);

            UIEffectView effect = CreateUIEffect(Find("SetGrid/collider" + i + "/fx_a_select"), UILayerSort.PopupSortBeginner);
            _listEffect.Add(effect);

            _lstFighterFlag.Add(Find("SetGrid/collider" + i + "/selected"));
            if (pveCards.ContainsKey(i))
            {
                _lstFighterFlag[i].SetActive(true);
                //_listEffect[i].StopEffect();
                if (!tmpValue)
                    NewBieGuide.NewBieGuideMgr.Instance.mLineupPos = i;
                tmpValue = true;
            }   
        }
        _btnBattle.onClick.Add(GoBattle);
        _textTitle.text = LanguageMgr.GetLanguage(5002927);
        NewBieGuide.NewBieGuideMgr.Instance.RegistMaskTransform(NewBieGuide.NewBieMaskID.LineUpBattleBtn, _btnBattle.transform);
    }

    private void OnMouseUp(Vector2 pos)
    {
        LineUpDragMgr.Instance.EndDrag();
    }

    private void OnDragMethod(DragEventType evtType, PointerEventData evtData, DragHelper dragHelper)
    {
        if (evtData.button != PointerEventData.InputButton.Left)
            return;
        switch(evtType)
        {
            case DragEventType.MouseDown:
                LineUpDragMgr.Instance.StartDrag(evtData, dragHelper, DragStatus.DragLineupFighter);
                break;
        }
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(UIEventDefines.LineupChangeIndex, OnLineUpChange);
        InputController.Instance.AddInputEvent(InputEventType.MouseUp, OnMouseUp);
        GameEventMgr.Instance.mGuideDispatcher.AddEvent<bool>(GuideEvent.LineupButtonStatusChange, OnRefreshBtnStatus);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(ArtifactEvent.ArtifactEffect, OnLineUpChange);
    }

    private void OnRefreshBtnStatus(bool enable)
    {
        _btnBattle.enabled = enable;
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(UIEventDefines.LineupChangeIndex, OnLineUpChange);
        InputController.Instance.RemoveInputEvent(InputEventType.MouseUp, OnMouseUp);
        GameEventMgr.Instance.mGuideDispatcher.RemoveEvent<bool>(GuideEvent.LineupButtonStatusChange, OnRefreshBtnStatus);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent(ArtifactEvent.ArtifactEffect, OnLineUpChange);
    }

    private void OnLineUpChange()
    {
        NewBieGuide.NewBieGuideMgr.Instance.UnRegisteLineupCardTransform(3);
        LineupFighter fighter;
        bool tmpValue = false;
        for (int i = 0; i < 9; i++)
        {
            fighter = LineupSceneMgr.Instance.GetFighterDataByIndex(i);
            _lstFighterFlag[i].SetActive(fighter != null);

            if (FunctionUnlock.IsUnlock(FunctionType.Artifact, true))
            {
                if (LocalDataMgr.GetArtifactSele(LineupSceneMgr.Instance.mLineupTeamType) == 0)
                {
                    _listEffect[i].StopEffect();
                }
                else
                {
                    if (fighter == null)
                    {
                        _listEffect[i].StopEffect();
                    }
                    else
                    {
                        _listEffect[i].PlayEffect();
                        ArtifactUnlockConfig artifactUnlockCfg = GameConfigMgr.Instance.GetArtifactUnlockConfig(LocalDataMgr.GetArtifactSele(LineupSceneMgr.Instance.mLineupTeamType));
                        foreach (Transform child in _listEffect[i].mTransform)
                        {
                            if (child.gameObject.name == artifactUnlockCfg.SelectFx)
                                child.gameObject.SetActive(true);
                            else
                                child.gameObject.SetActive(false);
                        }
                    }
                }
            }
            else
            {
                _listEffect[i].StopEffect();
            }
            if (fighter != null && !tmpValue)
            {
                NewBieGuide.NewBieGuideMgr.Instance.RegisteLineupCardTransform(3, _lstFighterFlag[i].transform);
                NewBieGuide.NewBieGuideMgr.Instance.mLineupPos = i;
                tmpValue = true;
            }
        }
        _text.text = LineupSceneMgr.Instance.GetTeamBattlePower().ToString();
        _textNum.text =LineupSceneMgr.Instance.OnDragCount()+ "/"+LineupSceneMgr.Instance.OnGetMaxRole();
    }

    protected override void Refresh(params object[] args)
	{
        base.Refresh(args);
        if (LineupSceneMgr.Instance.mLineupTeamType == TeamType.Defense || LineupSceneMgr.Instance.mLineupTeamType == TeamType.FriendBossAssist)
            _btnBattleText.text = LanguageMgr.GetLanguage(5003015);
        else
            _btnBattleText.text = LanguageMgr.GetLanguage(5001506);
        OnLineUpChange();
        InitLineUp();
        if (!_fighterImage.gameObject.activeSelf)
            _fighterImage.gameObject.SetActive(true);
        _fighterImage.texture = RoleRTMgr.Instance.GetRoleRTImage();
    }

    private void InitLineUp()
    {
        if (LineupSceneMgr.Instance.mLineupTeamType == TeamType.FriendBossAssist)
        {
            _rectBattle.anchoredPosition = new Vector3(0f, -297f, 0f);
            for (int i = 0; i < _lstPosCollider.Count; i++)
                _lstPosCollider[i].gameObject.SetActive(false);
            _lstPosCollider[4].gameObject.SetActive(true);
        }
        else
        {
            _rectBattle.anchoredPosition = new Vector3(180f, -297f, 0f);
            for (int i = 0; i < _lstPosCollider.Count; i++)
                _lstPosCollider[i].gameObject.SetActive(true);
        }
    }

	//点击按钮切换战斗界面
	private void GoBattle()
    {
        SoundMgr.Instance.PlayEffectSound("UI_btn_battle");
        LineupSceneMgr.Instance.DoBattle();
    }
}
