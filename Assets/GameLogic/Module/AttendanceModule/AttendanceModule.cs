using Framework.UI;
using Msg.ClientMessage;
using NewBieGuide;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttendanceModule : ModuleBase
{
    private Button _disBtn;
    private List<SignItemView> _listSignItemView;
    private Button _signBtn;
    private Button _help;
    private SignDataVO _curSignDataVO;
    private Text _signNum;
    private Text _signTime;
    private uint _timer = 0;
    private int _signTimes = 0;
    private Transform _root;


    public AttendanceModule()
       : base(ModuleID.Attendance, UILayer.Window)
    {
        _modelResName = UIModuleResName.UI_Attendance;
    }

    protected override void ParseComponent()
    {
        base.ParseComponent();
        _root = Find<Transform>("SignObj");
        _disBtn = Find<Button>("Btn_Back");
        _signBtn = Find<Button>("SignObj/SignIn/Sign");
        _help = Find<Button>("SignObj/SignIn/Help");
        _signNum = Find<Text>("SignObj/SignIn/SignNum");
        _signTime = Find<Text>("SignObj/SignIn/Sign/Text");

        _signBtn.onClick.Add(OnSign);
        _help.onClick.Add(OnHelp);

        _listSignItemView = new List<SignItemView>();
        for (int i = 0; i < 30; i++)
        {
            SignItemView signItemView = new SignItemView();
            signItemView.SetDisplayObject(Find("SignObj/Sign/SignGroup/Reward" + (i + 1)));
            _listSignItemView.Add(signItemView);
        }

        _disBtn.onClick.Add(OnClose);
        
        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.BoonDisBtn, _disBtn.transform);
        NewBieGuideMgr.Instance.RegistMaskTransform(NewBieMaskID.BoonCheck, _signBtn.transform);

        ColliderHelper.SetButtonCollider(_disBtn.transform, 120, 120);
    }

    protected override void AddEvent()
    {
        base.AddEvent();
        WelfareDataModel.Instance.AddEvent<SignDataVO>(WelfareEvent.SignData, OnSignData);
        WelfareDataModel.Instance.AddEvent<List<ItemInfo>>(WelfareEvent.SignAward, OnSignAward);
    }

    protected override void RemoveEvent()
    {
        base.RemoveEvent();
        WelfareDataModel.Instance.RemoveEvent<SignDataVO>(WelfareEvent.SignData, OnSignData);
        WelfareDataModel.Instance.RemoveEvent<List<ItemInfo>>(WelfareEvent.SignAward, OnSignAward);
    }

    private void OnSignAward(List<ItemInfo> listInfo)
    {
        GetItemTipMgr.Instance.ShowItemResult(listInfo);
        OnSignChang();
    }

    private void OnSignData(SignDataVO signVO)
    {
        _curSignDataVO = signVO;
        OnSignChang();
        DelayCall(0.5f, () => GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.EndCondTrigger, NewBieGuide.EndConditionConst.BoonOpen));
    }

    private void OnSignChang()
    {
        _signTimes = _curSignDataVO.SignTime;
        if (_timer != 0)
            TimerHeap.DelTimer(_timer);
        int interval = 1000;
        _timer = TimerHeap.AddTimer(0, interval, OnAddTime);
        _signNum.text = LanguageMgr.GetLanguage(5007204) + " " + _curSignDataVO.mMinIndex % 30 + "/30";
        
        for (int i = 0; i < _listSignItemView.Count; i++)
            _listSignItemView[i].Show(_curSignDataVO.mListSignConfig[i], _curSignDataVO.mMinIndex, _curSignDataVO.mMaxIndex);
        if (_curSignDataVO.mMinIndex == _curSignDataVO.mMaxIndex)
            _signBtn.interactable = false;
        else
            _signBtn.interactable = true;
    }

    private void OnAddTime()
    {
        if (_signTimes > 0)
        {
            _signTimes -= 1;
            if (_curSignDataVO.mMinIndex == _curSignDataVO.mMaxIndex)
                _signTime.text = TimeHelper.GetCountTime(_signTimes);
            else
                _signTime.text = LanguageMgr.GetLanguage(5003110);
        }
        else
        {
            GameNetMgr.Instance.mGameServer.ReqSignData();
        }
    }

    private void OnSign()
    {
        GameNetMgr.Instance.mGameServer.ReqSignAward(_curSignDataVO.mMinIndex + 1);
    }

    private void OnHelp()
    {
        HelpTipsMgr.Instance.ShowTIps(HelpType.SignInHelp);
    }

    protected override void Refresh(params object[] args)
    {
        base.Refresh(args);
        WelfareDataModel.Instance.ReqSignData();
    }

    public override void Dispose()
    {
        base.Dispose();
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.BoonDisBtn);
        NewBieGuideMgr.Instance.UnRegistMaskTransform(NewBieMaskID.BoonCheck);
    }

    protected override void OnShowAnimator()
    {
        base.OnShowAnimator();
        ObjectHelper.PopAnimationLiner(_root);
    }
}
