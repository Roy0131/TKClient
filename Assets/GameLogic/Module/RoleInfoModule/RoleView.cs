using Framework.UI;
using NewBieGuide;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleView : UIBaseView
{
    private Button _shareBtn;
    private Button _lockBtn;
    private Text _roleName;

    private Button _nextBtn;
    private Button _preBtn;
    private Image _campIcon;
    private RawImage _roleImage;

    private ItemResGroup _resGroup;
    private CardRarityView _rarityView;
    private Button _backBtn;

    private CardDataVO _curCardVO;
    private List<CardDataVO> _allCardDatas;
    
    private int _curIndex;
    private int _totalCardCount;
    private bool _blHeroRole = true;
    private int _curCampType;

    private GameObject _lockObj;
    private GameObject _unLockObj;

    private bool _isShare;

    protected override void ParseComponent()
	{
        base.ParseComponent();

        _shareBtn = Find<Button>("ButtonShare");
        _lockBtn = Find<Button>("ButtonLock");
        _roleName = Find<Text>("Grid/Name");

        _nextBtn = Find<Button>("ButtonRight");
        _preBtn = Find<Button>("ButtonLeft");
        _backBtn = Find<Button>("Btn_Back");
        _campIcon = Find<Image>("Grid/ImageCamp");

        _lockObj = Find("ButtonLock/Lock");
        _unLockObj = Find("ButtonLock/UnlockLock");

        _resGroup = new ItemResGroup();
        _resGroup.SetDisplayObject(Find("ResRoot/uiResGroup"));

        _roleImage = Find<RawImage>("ImageBottom/Role");

        _rarityView = new CardRarityView();
        _rarityView.SetDisplayObject(Find("cardStars/uiStarGroup"));

        _shareBtn.onClick.Add(OnShare);
        _lockBtn.onClick.Add(OnLock);
        _nextBtn.onClick.Add(OnNextRole);
        _preBtn.onClick.Add(OnPreRole);
        _backBtn.onClick.Add(OnExit);
        _blHeroRole = true;
        _isShare = false;

        ColliderHelper.SetButtonCollider(_shareBtn.transform);
        ColliderHelper.SetButtonCollider(_lockBtn.transform);
        ColliderHelper.SetButtonCollider(_nextBtn.transform);
        ColliderHelper.SetButtonCollider(_preBtn.transform);
        ColliderHelper.SetButtonCollider(_backBtn.transform);

        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.HeroInfoDisBtn, _backBtn.transform);
    }

	protected override void Refresh(params object[] args)
	{
        base.Refresh(args);
        RoleVO roleVO = args[0] as RoleVO;
        CardDataVO vo = roleVO.mCardDataVO;
        if (roleVO.mCardDetailType== CardDetailConst.HeroInfo)
            _resGroup.Show(SpecialItemID.Gold, SpecialItemID.HeroExp);
        else
            _resGroup.Hide();
        _allCardDatas = null;
        _curCampType = 0;
        //if (args.Length == 3)
        //{
        //    _allCardDatas = args[1] as List<CardDataVO>;
        //    _curCampType = int.Parse(args[2].ToString());
        //}
        if (roleVO.mLstCardDatas != null)
            _allCardDatas = roleVO.mLstCardDatas;
        if (roleVO.mCampType > 0)
            _curCampType = roleVO.mCampType;
        if (vo.BlEntityCard != _blHeroRole)
        {
            _blHeroRole = vo.BlEntityCard;
            _lockBtn.gameObject.SetActive(_blHeroRole);
            _shareBtn.gameObject.SetActive(_blHeroRole);
        }
        if (_allCardDatas != null)
        {
            _curIndex = _allCardDatas.IndexOf(vo);
            _totalCardCount = _allCardDatas.Count;
            _nextBtn.gameObject.SetActive(true);
            _preBtn.gameObject.SetActive(true);
        }
        else
        {
            _nextBtn.gameObject.SetActive(false);
            _preBtn.gameObject.SetActive(false);
        }

        ShowRole(vo);
        OnSwitch();

    }

    private void OnSwitch()
    {
        if (_allCardDatas != null)
        {
            _preBtn.gameObject.SetActive(_curIndex > 0);
            _nextBtn.gameObject.SetActive(_curIndex < _allCardDatas.Count - 1);
        }
    }

    private void ShowRole(CardDataVO vo)
    {
        _rarityView.Show(vo.mCardConfig.Rarity);
        _unLockObj.SetActive(vo.mBlLock);
        _lockObj.SetActive(!vo.mBlLock);
        RoleRTMgr.Instance.ShowRoleRTLogic(RoleRTType.RoleInfo, vo.mCardConfig);
        if (!_roleImage.gameObject.activeSelf)
            _roleImage.gameObject.SetActive(true);
        _roleImage.texture = RoleRTMgr.Instance.GetRoleRTImage();
        if (_curCardVO == vo)
            return;
        _curCardVO = vo;
        _roleName.text = LanguageMgr.GetLanguage(_curCardVO.mCardConfig.Name);
        _campIcon.sprite = GameResMgr.Instance.LoadCampIcon(vo.mCardConfig.Camp);
        ObjectHelper.SetSprite(_campIcon,_campIcon.sprite);
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(UIEventDefines.RoleInfoChangeRole, vo);
    }

    private void OnCardRefresh(List<int> value)
    {
        if (_curCardVO == null)
            return;
        if (value.Contains(_curCardVO.mCardID))
        {
            _rarityView.Show(_curCardVO.mCardConfig.Rarity);
            _campIcon.sprite = GameResMgr.Instance.LoadCampIcon(_curCardVO.mCardConfig.Camp);
            ObjectHelper.SetSprite(_campIcon,_campIcon.sprite);
            _unLockObj.SetActive(_curCardVO.mBlLock);
            _lockObj.SetActive(!_curCardVO.mBlLock);
        }
    }

    private void OnAddCard(List<int> value)
    {
        RefreshAllCardData();
    }

    private void OnRemoveCard(List<int> value)
    {
        RefreshAllCardData();
    }

    private void RefreshAllCardData()
    {
        List<CardDataVO> value = HeroDataModel.Instance.mAllCards;
        List<CardDataVO> result = new List<CardDataVO>();
        if (_curCampType != 0)
        {
            for (int i = 0; i < value.Count; i++)
            {
                if (value[i].mCardConfig.Camp == _curCampType)
                    result.Add(value[i]);
            }
        }
        else
        {
            result = value;
        }

        CardDataVO vo = HeroDataModel.Instance.GetCardDataByCardId(_curCardVO.mCardID);
        _allCardDatas = result;
        _curIndex = _allCardDatas.IndexOf(vo);
        _totalCardCount = _allCardDatas.Count;
        _curCardVO = null;
        ShowRole(vo);
    }

	protected override void AddEvent()
	{
        base.AddEvent();
        HeroDataModel.Instance.AddEvent<List<int>>(HeroEvent.CardDataRefresh, OnCardRefresh);
        HeroDataModel.Instance.AddEvent<List<int>>(HeroEvent.HeroAddCard, OnAddCard);
        HeroDataModel.Instance.AddEvent<List<int>>(HeroEvent.HeroRemoveCard, OnRemoveCard);
        ChatModel.Instance.AddEvent<int>(ChatEvent.ChatRefresh, OnChatDataRefresh);
    }


    protected override void RemoveEvent()
	{
        base.RemoveEvent();
        HeroDataModel.Instance.RemoveEvent<List<int>>(HeroEvent.CardDataRefresh, OnCardRefresh);
        HeroDataModel.Instance.RemoveEvent<List<int>>(HeroEvent.HeroAddCard, OnAddCard);
        HeroDataModel.Instance.RemoveEvent<List<int>>(HeroEvent.HeroRemoveCard, OnRemoveCard);
        ChatModel.Instance.RemoveEvent<int>(ChatEvent.ChatRefresh, OnChatDataRefresh);
    }

    private void OnChatDataRefresh(int channel)
    {
        if (_isShare)
        {
            PopupTipsMgr.Instance.ShowTips(LanguageMgr.GetLanguage(4000117));
            _isShare = false;
        }
    }

    private void OnLock()
    {
        GameNetMgr.Instance.mGameServer.ReqRoleLock(_curCardVO.mCardID, !_curCardVO.mBlLock);
    }

    private void OnPreRole()
    {
        if (_curIndex <= 0)
            return;
        _curIndex--;
        ShowRole(_allCardDatas[_curIndex]);
        OnSwitch();
    }

    private void OnNextRole()
    {
        if (_curIndex >= _allCardDatas.Count - 1)
            return;
        _curIndex++;
        ShowRole(_allCardDatas[_curIndex]);
        OnSwitch();
    }

    private void OnExit()
    {
        RoleRTMgr.Instance.Hide(RoleRTType.RoleInfo);
        GameUIMgr.Instance.CloseModule(ModuleID.RoleInfo);
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(HeroEvent.HeroShop);
    }

    private void OnShare()
    {
        string content = _curCardVO.mCardConfig.ID + "*" + _curCardVO.mCardRank + "*" + _curCardVO.mCardLevel + "*" +
            _curCardVO.mBattlePower + "*" + _curCardVO.GetAttriByType(AttributesType.HP) + "*" +
            _curCardVO.GetAttriByType(AttributesType.ATTACK) + "*" + _curCardVO.GetAttriByType(AttributesType.DEFENSE);
        GameNetMgr.Instance.mGameServer.ReqChat(ChatChannelConst.World, content);
        _isShare = true;
    }

	public override void Dispose()
	{
        if(_resGroup != null)
        {
            _resGroup.Dispose();
            _resGroup = null;
        }

        if(_rarityView != null)
        {
            _rarityView.Dispose();
            _rarityView = null;
        }
        _curCardVO = null;
        _allCardDatas = null;
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.HeroInfoDisBtn);
        base.Dispose();
	}
}