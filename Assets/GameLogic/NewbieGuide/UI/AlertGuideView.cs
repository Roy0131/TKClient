using UnityEngine;
using UnityEngine.UI;
using Framework.UI;

namespace NewBieGuide
{
    public class AlertGuideView : UIBaseView
    {
        private GameObject _nameGuideObject;
        private InputField _nameInputField;
        private Button _nameOkBtn;

        private GameObject _abilityObject;
        private Button _abilityBtn1;
        private Button _abilityBtn2;
        private Button _abilityBtn3;
        private Text _Title;
        private Text _abilityText1;
        private Text _abilityText2;
        private Text _abilityText3;

        private GameObject _dialogObject;
        private Button _dialogBtn;

        private GuideStepDataVO _vo;

        protected override void ParseComponent()
        {
            base.ParseComponent();

            _nameGuideObject = Find("NameGuide");
            _nameInputField = Find<InputField>("NameGuide/InputField");
            _nameOkBtn = Find<Button>("NameGuide/Button");

            _abilityObject = Find("AbilityGuide");
            _abilityBtn1 = Find<Button>("AbilityGuide/Button0");
            _abilityBtn2 = Find<Button>("AbilityGuide/Button1");
            _abilityBtn3 = Find<Button>("AbilityGuide/Button2");
            _Title = Find<Text>("AbilityGuide/Title");
            _abilityText1 = Find<Text>("AbilityGuide/Button0/Text");
            _abilityText2 = Find<Text>("AbilityGuide/Button1/Text");
            _abilityText3 = Find<Text>("AbilityGuide/Button2/Text");

            _dialogObject = Find("DialogGuide");
            _dialogBtn = Find<Button>("DialogGuide/Button");

            _abilityBtn1.onClick.Add(OnClick);
            _abilityBtn2.onClick.Add(OnClick);
            _abilityBtn3.onClick.Add(OnClick);
            _dialogBtn.onClick.Add(OnClick);

            _nameOkBtn.onClick.Add(OnInputName);
        }

        protected override void AddEvent()
        {
            base.AddEvent();
            GameEventMgr.Instance.mGuideDispatcher.AddEvent<int>(GuideEvent.EnterCondTrigger, OnEnterTrigger);
        }

        protected override void RemoveEvent()
        {
            base.RemoveEvent();
            GameEventMgr.Instance.mGuideDispatcher.RemoveEvent<int>(GuideEvent.EnterCondTrigger, OnEnterTrigger);
        }

        private bool _blStarted;
        private void OnEnterTrigger(int enterCondID)
        {
            if (enterCondID == _vo.mEnterCondId)
                OnEnterShow();
        }

        private void OnEnterShow()
        {
            _blStarted = true;
            if (_vo.mGuideType == GuideType.NameUIGuide)
                _nameGuideObject.SetActive(true);
            else if (_vo.mGuideType == GuideType.AbilityGuide || _vo.mGuideType == GuideType.GoalGuide)
                _abilityObject.SetActive(true);
            else if (_vo.mGuideType == GuideType.DialogGuide)
                _dialogObject.SetActive(true);
            switch (_vo.mGuideType)
            {
                case GuideType.ModuleGuide:
                    break;
                case GuideType.NameUIGuide:
                    break;
                case GuideType.AbilityGuide:
                    _Title.text = LanguageMgr.GetLanguage(3011003);
                    _abilityText1.text = LanguageMgr.GetLanguage(3011005);
                    _abilityText2.text = LanguageMgr.GetLanguage(3011006);
                    _abilityText3.text = LanguageMgr.GetLanguage(3011007);
                    break;
                case GuideType.GoalGuide:
                    _Title.text = LanguageMgr.GetLanguage(3011014);
                    _abilityText1.text = LanguageMgr.GetLanguage(3011015);
                    _abilityText2.text = LanguageMgr.GetLanguage(3011016);
                    _abilityText3.text = LanguageMgr.GetLanguage(3011017);
                    break;
            }
        }

        protected override void Refresh(params object[] args)
        {
            base.Refresh(args);

            _nameInputField.text = "";
            _blStarted = false;
            _vo = args[0] as GuideStepDataVO;
            _nameGuideObject.SetActive(false);
            _abilityObject.SetActive(false);
            _dialogObject.SetActive(false);
            if (_vo.mEnterCondId != 0)
                return;
            OnEnterShow();
            LocalDataMgr.NewBieGuildStepID = _vo.mStepID;
            GuideDataModel.Instance.SaveGuideData();
        }

        private void OnInputName()
        {
            if (!_blStarted)
                return;
            if(string.IsNullOrWhiteSpace(_nameInputField.text))
            {
                PopupTipsMgr.Instance.ShowTips("Please input your name!!!");
                return;
            }
            GameNetMgr.Instance.mGameServer.ReqPlayerChangeName(_nameInputField.text);
            OnClick();
        }

        private void OnClick()
        {
            if (!_blStarted)
                return;
            GameEventMgr.Instance.mGuideDispatcher.DispathEvent(GuideEvent.GuideStepComplete);
        }

        public override void Dispose()
        {
            _vo = null;
            base.Dispose();
        }
    }
}
