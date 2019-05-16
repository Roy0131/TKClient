using Framework.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalentView : UIBaseView
{
    private Text _name;
    private Text _talentName;
    private Text _expendNum;
    private Text _curAtt;
    private Text _wardAtt;
    private Text _frontText;
    private Image _expendImg;
    private Image _talentImg;
    private Image _talentSkillImg;
    private Button _talentUp;
    private Button _reset;
    private RectTransform _groupRect;
    private GameObject _front;
    private GameObject _wardAttObj;
    private GameObject _imgObj;
    private GameObject _expendObj;

    private TalentVO _talentVO;
    private List<GameObject> listLine;

    private List<TalentItemView> _listTalentItemView;
    private TalentItemView _curItemView;
    private TalentItemView _talentItemView;
    private int _talentType = 0;
    private int _talentCount = 0;
    private int _talentNum = 0;
    private int _curTalentNum = 0;

    private ImageGray _gray;
    private ImageGray _grays;


    protected override void ParseComponent()
    {
        base.ParseComponent();
        _name = Find<Text>("Name");
        _talentName = Find<Text>("Details/TalentName");
        _expendNum = Find<Text>("Details/Expend/Text");
        _curAtt = Find<Text>("Details/Descrption/Group/CurAtt");
        _wardAtt = Find<Text>("Details/Descrption/Group/WardAtt");
        _frontText = Find<Text>("Details/Front");
        _expendImg = Find<Image>("Details/Expend/Image");
        _talentImg = Find<Image>("Details/Talent");
        _gray = Find<ImageGray>("Details/Talent");
        _talentSkillImg = Find<Image>("Details/Talent/Skill");
        _grays = Find<ImageGray>("Details/Talent/Skill");
        _talentUp = Find<Button>("Details/TalentUp");
        _reset = Find<Button>("Reset");
        _groupRect = Find<RectTransform>("Details/Descrption/Group");
        _front = Find("Details/Front");
        _wardAttObj = Find("Details/Descrption/Group/WardAtt");
        _imgObj = Find("Details/Descrption/Group/Obj/Img");
        _expendObj = Find("Details/Expend");

        TalentItemView item;
        _listTalentItemView = new List<TalentItemView>();
        for (int i = 1; i <= 10; i++)
        {
            GameObject obj = Find("Talent/Talent" + i);
            item = new TalentItemView(i);
            item.SetDisplayObject(obj);
            _listTalentItemView.Add(item);
        }

        listLine = new List<GameObject>();
        for (int i = 0; i < 9; i++)
        {
            GameObject obj = Find("Guide/Line" + (i + 1));
            listLine.Add(obj);
        }

        _talentUp.onClick.Add(OnTalentUp);
        _reset.onClick.Add(OnReset);

        ColliderHelper.SetButtonCollider(_reset.transform);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        TalentDataModel.Instance.AddEvent(TalentEvent.TalentDate, OnTalentDate);
        TalentDataModel.Instance.AddEvent(TalentEvent.TalentUp, OnUpTalent);
        TalentDataModel.Instance.AddEvent<List<int>>(TalentEvent.TalentReset, OnTalentReset);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<TalentItemView>(TalentEvent.Click, OnClick);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<int,bool>(TalentEvent.TalentUnlocked, OnUnlocked);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        TalentDataModel.Instance.RemoveEvent(TalentEvent.TalentDate, OnTalentDate);
        TalentDataModel.Instance.RemoveEvent(TalentEvent.TalentUp, OnUpTalent);
        TalentDataModel.Instance.RemoveEvent<List<int>>(TalentEvent.TalentReset, OnTalentReset);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<TalentItemView>(TalentEvent.Click, OnClick);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<int,bool>(TalentEvent.TalentUnlocked, OnUnlocked);
    }

    private void OnUnlocked(int index,bool isGray)
    {
        if (index > 0)
            listLine[index - 1].SetActive(!isGray);
    }

    private void OnTalentReset(List<int> listItem)
    {
        List<ItemInfo> _listItemInfo = new List<ItemInfo>();
        for (int i = 0; i < listItem.Count; i += 2)
        {
            ItemInfo _itemInfo = new ItemInfo();
            _itemInfo.Id = listItem[i];
            _itemInfo.Value = listItem[i + 1];
            _listItemInfo.Add(_itemInfo);
        }
        GetItemTipMgr.Instance.ShowItemResult(_listItemInfo);
        OnTalentChange();
        OnClick(_listTalentItemView[0]);
    }

    private void OnTalentDate()
    {
        OnTalentChange();
        OnClick(_listTalentItemView[0]);
    }

    private void OnUpTalent()
    {
        OnTalentChange();
        OnClick(_curItemView);
    }

    private void OnClick(TalentItemView itemView)
    {
        _curItemView = itemView;
        _talentVO = itemView._talentVO;
        _curTalentNum = _talentVO.mTalentBaseID;
        if (_talentItemView != null)
            _talentItemView.BlSelected = false;
        _talentItemView = itemView;
        _talentItemView.BlSelected = true;
        if (_talentVO.mTalengPreSkill > 0 && TalentDataModel.Instance.GetTalentVO(_talentVO.mTalentType, _talentVO.mTalengPreSkill).mTalentLevel 
            < _talentVO.mTalentPreSkillLevel&& _talentVO.mTalentLevel < _talentVO.mTalentMaxLevel)
        {
            _front.SetActive(true);
            _talentUp.interactable = false;
        }
        else
        {
            _front.SetActive(false);
            _talentUp.interactable = true;
        }
        _talentName.text = LanguageMgr.GetLanguage(_talentVO.mTalentNameId);
        _expendNum.text = _talentVO.mUpConNum.ToString();
        _frontText.text = LanguageMgr.GetLanguage(620001, _talentVO.mTalentPreSkillLevel);
        _expendImg.sprite = GameResMgr.Instance.LoadItemIcon(GameConfigMgr.Instance.GetItemConfig(_talentVO.mUpConId).UIIcon);
        ObjectHelper.SetSprite(_expendImg,_expendImg.sprite);
        //if (_talentVO.mTalentBaseID < 12)
        //{
        //    _talentImg.sprite = GameResMgr.Instance.LoadItemIcon("" + (_talentVO.mTalentBaseID - 10));
        //}
        //else
        //{
        //    _talentImg.sprite = GameResMgr.Instance.LoadItemIcon("" + (_talentVO.mTalentBaseID - 10));
        //}

        TalentConfig cfg = GameConfigMgr.Instance.GetTalentConfig(_talentVO.mTalentBaseID * 100 + _talentVO.mTalentLevel);
        TalentConfig cfg1;
        if (_talentVO.mBlLearned&& _talentVO.mTalentLevel < _talentVO.mTalentMaxLevel)
            cfg1 = GameConfigMgr.Instance.GetTalentConfig(_talentVO.mTalentBaseID * 100 + _talentVO.mTalentLevel + 1);
        else
            cfg1 = GameConfigMgr.Instance.GetTalentConfig(_talentVO.mTalentBaseID * 100 + _talentVO.mTalentLevel);
        string[] talent = cfg.ShowParam.Split(',');
        string[] talent1 = cfg1.ShowParam.Split(',');
        object[] param = new object[talent.Length];
        object[] param1 = new object[talent1.Length];
        if (_talentVO.mBlLearned)
        {
            for (int i = 0; i < param.Length; i++)
            {
                param[i] = talent[i];
                param1[i] = talent1[i];
            }
        }
        else
        {
            for (int i = 0; i < param.Length; i++)
            {
                param[i] = 0;
                 param1[i] = talent[i];
            }
        }
        _curAtt.text = LanguageMgr.GetLanguage(cfg.DescrptionID, param);
        _wardAtt.text= LanguageMgr.GetLanguage(cfg.DescrptionID, param1);
        if (_talentVO.mTalentLevel == _talentVO.mTalentMaxLevel)
        {
            //_curAttRect.anchoredPosition = new Vector2(234, -30);
            _wardAttObj.SetActive(false);
            _imgObj.SetActive(false);
            _talentUp.interactable = false;
            _expendObj.SetActive(false);
        }
        else
        {
            //_curAttRect.anchoredPosition = new Vector2(234, 60);
            _wardAttObj.SetActive(true);
            _imgObj.SetActive(true);
            _expendObj.SetActive(true);
        }
        _groupRect.anchoredPosition = new Vector2(0, 0);

        _talentImg.sprite = GameResMgr.Instance.LoadItemIcon(cfg.PanelIcon);
        _talentSkillImg.sprite = GameResMgr.Instance.LoadItemIcon(cfg.Icon);

        ObjectHelper.SetSprite(_talentImg,_talentImg.sprite);
        ObjectHelper.SetSprite(_talentSkillImg,_talentSkillImg.sprite);
        if (itemView.mIsGray)
        {
            _gray.SetGray();
            _grays.SetGray();
        }
        else
        {
            _gray.SetNormal();
            _grays.SetNormal();
        }
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        _talentType = int.Parse(args[0].ToString());
        _talentCount = _talentType == TalentTypeConst.Basis ? 7 : 10;
        _talentNum = _talentType == TalentTypeConst.Basis ? 1 : 11;
        OnTalentChange();
        OnClick(_listTalentItemView[0]);
        if (_talentType == TalentTypeConst.Basis)
            _name.text = LanguageMgr.GetLanguage(6001189);
        else
            _name.text = LanguageMgr.GetLanguage(5003146);
    }

    private void OnTalentChange()
    {
        for (int i = 0; i < _talentCount; i++)
            _listTalentItemView[i].Show(TalentDataModel.Instance.GetTalentVO(_talentType, i + _talentNum), i);
        for (int j = _talentCount; j < 10; j++)
            _listTalentItemView[j].Hide();
        for (int i = 0; i < 3; i++)
        {
            GameObject obj1 = Find("SkillSlots/Slot" + (i + 8));
            obj1.SetActive(_talentType != TalentTypeConst.Basis);
            GameObject obj2 = Find("Guide/Guide" + (i + 7));
            obj2.SetActive(_talentType != TalentTypeConst.Basis);
            GameObject obj4 = Find("Talent/Talent" + (i + 8));
            obj4.SetActive(_talentType != TalentTypeConst.Basis);
            GameObject obj3 = Find("Guide/Line" + (i + 7));
            if (_talentType == TalentTypeConst.Basis)
                obj3.SetActive(false);
        }
    }

    private void OnTalentUp()
    {
        string str = "";
        if (_talentVO.mTalentType== TalentTypeConst.Basis)
            str = LanguageMgr.GetLanguage(6001191);
        else if(_talentVO.mTalentType == TalentTypeConst.Guild)
            str = LanguageMgr.GetLanguage(6001192);
        if (BagDataModel.Instance.GetItemCountById(_talentVO.mUpConId) >= _talentVO.mUpConNum)
            GameNetMgr.Instance.mGameServer.ReqTalentUp(_talentVO.mTalentBaseID);
        else
            PopupTipsMgr.Instance.ShowTips(str);
    }

    private void OnReset()
    {
        if (TalentDataModel.Instance.GetTalentNum(_talentType) == 0)
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(6001193));
        else
            ConfirmTipsMgr.Instance.ShowConfirmTips(LanguageMgr.GetLanguage(6001194, GameConst.TalentReset), AlertBack);
    }

    private void AlertBack(bool result, bool blShowAgain)
    {
        if (result)
        {
            if (HeroDataModel.Instance.mHeroInfoData.mDiamond >= GameConst.TalentReset)
            {
                GameNetMgr.Instance.mGameServer.ReqTalentReset(_talentVO.mTalentType);
                TDPostDataMgr.Instance.DoCostDiamond(TDCostDiamondType.BuyTalentRefreshCount, 1, GameConst.TalentReset);
            }
            else
            {
                PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000055));
            }
        }
        else
        {

        }
    }
}
