using UnityEngine.UI;
using Msg.ClientMessage;
using System.Collections.Generic;
using NewBieGuide;
using Framework.UI;

public class RoleFuncView : UIBaseView
{
    enum RoleInfoType
    {
        RoleInfoBasic,
        RoleEquip,
        Rankup,
    }

    private Toggle _infoToggle;
    private Toggle _equipToggle;
    private Toggle _rankUpToggle;
    private ToggleGroup _toggleGroup;

    private RoleInfoType _curType;

    private RoleInfoView _roleInfoView;
    private RoleEquipView _roleEquipView;
    private RoleFusionView _roleFusionView;

    private RoleAdvancedView _roleAdvancedView;
    private RoleSurmountView _roleSurmountView;

    private CardDataVO _curCardData;
    private UIBaseView _curShowView;

    private bool _blHeroRole;
    private RoleVO _roleVO;


    protected override void ParseComponent()
	{
        base.ParseComponent();

        _curType = RoleInfoType.RoleInfoBasic;

        _toggleGroup = Find<ToggleGroup>("ToggleGroup");
        _infoToggle = Find<Toggle>("ToggleGroup/Tog1");
        _equipToggle = Find<Toggle>("ToggleGroup/Tog2");
        _rankUpToggle = Find<Toggle>("ToggleGroup/Tog3");

        _roleInfoView = new RoleInfoView();
        _roleInfoView.SetDisplayObject(Find("HeroInfo"));

        _roleEquipView = new RoleEquipView();
        _roleEquipView.SetDisplayObject(Find("HeroEquipment"));

        _roleFusionView = new RoleFusionView();
        _roleFusionView.SetDisplayObject(Find("HeroFusion"));

        _roleAdvancedView = new RoleAdvancedView();
        _roleAdvancedView.SetDisplayObject(Find("Advanced"));

        _roleSurmountView = new RoleSurmountView();
        _roleSurmountView.SetDisplayObject(Find("Surmount"));

        _infoToggle.onValueChanged.Add((bool value) => { if (value) OnChangeType(RoleInfoType.RoleInfoBasic); });
        _equipToggle.onValueChanged.Add((bool value) => { if (value) OnChangeType(RoleInfoType.RoleEquip); });
        _rankUpToggle.onValueChanged.Add((bool value) => { if (value) OnChangeType(RoleInfoType.Rankup); });
        _curShowView = _roleInfoView;
        _roleInfoView.mDisplayObject.SetActive(true);
        _roleEquipView.mDisplayObject.SetActive(false);
        _roleFusionView.mDisplayObject.SetActive(false);
        _blHeroRole = true;

        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.HeroEquipment, _equipToggle.transform);
    }

	protected override void AddEvent()
	{
        base.AddEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<CardDataVO>(UIEventDefines.RoleInfoChangeRole, OnChangeRole);
        HeroDataModel.Instance.AddEvent<S2CRoleFusionResponse>(HeroEvent.CardFusionBack, OnFusion);
        HeroDataModel.Instance.AddEvent<List<int>>(HeroEvent.CardDataRefresh, OnCardRefresh);
        HeroDataModel.Instance.AddEvent<int>(HeroEvent.Advanced, OnAdvanced);
        GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<int,int, List<ItemInfo>>(HeroEvent.Surmount, OnSurmount);
    }

	protected override void RemoveEvent()
	{
        base.RemoveEvent();
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<CardDataVO>(UIEventDefines.RoleInfoChangeRole, OnChangeRole);
        HeroDataModel.Instance.RemoveEvent<S2CRoleFusionResponse>(HeroEvent.CardFusionBack, OnFusion);
        HeroDataModel.Instance.RemoveEvent<List<int>>(HeroEvent.CardDataRefresh, OnCardRefresh);
        HeroDataModel.Instance.RemoveEvent<int>(HeroEvent.Advanced, OnAdvanced);
        GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<int,int, List<ItemInfo>>(HeroEvent.Surmount, OnSurmount);
    }

    private void OnAdvanced(int newCardId)
    {
       DelayCall(2f,() => _roleAdvancedView.Show(HeroDataModel.Instance.GetCardDataByCardId(newCardId)));
    }

    private void OnSurmount(int newCardId,int fusionId, List<ItemInfo> listInfo)
    {
        DelayCall(2f, () => _roleSurmountView.Show(HeroDataModel.Instance.GetCardDataByCardId(newCardId), fusionId));
        if (listInfo.Count > 0)
            DelayCall(2f, () => GetItemTipMgr.Instance.ShowItemResult(listInfo));
    }

    private void OnCardRefresh(List<int> value)
    {
        if (_curCardData != null && value.Contains(_curCardData.mCardID))
            OnRefreshRole(_curCardData);
    }

    private void OnFusion(S2CRoleFusionResponse value)
    {
        _infoToggle.isOn = true;
        List<ItemInfo> listInfo = new List<ItemInfo>();
        listInfo.AddRange(value.GetItems);
        GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent(HeroEvent.Surmount, value.RoleId, value.FusionId, listInfo);
    }

    private void OnChangeRole(CardDataVO vo)
    {
        OnRefreshRole(vo);
    }

    private void OnRefreshRole(CardDataVO vo)
    {
        _curCardData = vo;
        if (vo.BlTopRank)
        {
            if (_curType == RoleInfoType.Rankup)
            {
                _curCardData = vo;
                _infoToggle.isOn = true;
                _rankUpToggle.gameObject.SetActive(false);
                _curShowView.Hide();
                _curShowView = _roleInfoView;
            }
            else
            {
                if (_rankUpToggle.gameObject.activeSelf)
                    _rankUpToggle.gameObject.SetActive(false);
            }
        }
        else
        {
            if (!_rankUpToggle.gameObject.activeSelf)
                _rankUpToggle.gameObject.SetActive(true);
        }
        _curShowView.Show(vo, _roleVO.mCardDetailType);
    }

    public override void Hide()
    {
        if (_curShowView != null)
            _curShowView.Hide();
        _infoToggle.isOn = true;
        base.Hide();
    }

    protected override void Refresh(params object[] args)
	{
        base.Refresh(args);
        _roleVO = args[0] as RoleVO;
        CardDataVO vo = _roleVO.mCardDataVO;
        if (vo.BlEntityCard != _blHeroRole)
        {
            _blHeroRole = vo.BlEntityCard;
            if(_blHeroRole)
            {
                _toggleGroup.gameObject.SetActive(true);
            }
            else
            {
                _toggleGroup.gameObject.SetActive(false);
                if(_curType != RoleInfoType.RoleInfoBasic)
                {
                    _curCardData = vo;
                    _infoToggle.isOn = true;
                    return;
                }
            }
        }
        OnRefreshRole(vo);
	}

	private void OnChangeType(RoleInfoType type)
    {
        if (_curType == type)
            return;
        _curType = type;
        if (_curShowView != null)
            _curShowView.Hide();
        switch(_curType)
        {
            case RoleInfoType.RoleInfoBasic:
                _curShowView = _roleInfoView;
                break;
            case RoleInfoType.RoleEquip:
                _curShowView = _roleEquipView;
                break;
            case RoleInfoType.Rankup:
                _curShowView = _roleFusionView;
                break;
        }
        OnRefreshRole(_curCardData);
    }

	public override void Dispose()
	{
        if(_roleInfoView != null)
        {
            _roleInfoView.Dispose();
            _roleInfoView = null;
        }
        if(_roleEquipView != null)
        {
            _roleEquipView.Dispose();
            _roleEquipView = null;
        }
        if(_roleFusionView != null)
        {
            _roleFusionView.Dispose();
            _roleFusionView = null;
        }
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.HeroEquipment);
        base.Dispose();
	}

}