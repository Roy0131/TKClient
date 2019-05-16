using UnityEngine.UI;

namespace GameConsole
{
    public class ConsoleModule : ModuleBase
    {
        private InputField _cmdInput;
        private ScrollRect _scrRect;
        private Text _outputText;
        public ConsoleModule()
            : base(ModuleID.Console, UILayer.Popup)
        {
            _modelResName = UIModuleResName.UI_Console;
        }

        protected override void ParseComponent()
        {
            base.ParseComponent();
            _cmdInput = Find<InputField>("InputField");
            _scrRect = Find<ScrollRect>("Scroll View");
            _outputText = Find<Text>("Scroll View/Viewport/Content");
            _cmdInput.onEndEdit.Add(OnSubmit);
        }

        private void OnSubmit(string input)
        {
            string cmd = input.Trim();
            if (string.IsNullOrEmpty(cmd))
            {
                ConsoleLogger.Log("invalid input command");
                return;
            }
            ConsoleCmdMgr.Instance.ExecCommand(cmd);
            _scrRect.verticalScrollbar.value = 0;
            ClearInput();
        }

        private void ClearInput()
        {
            _cmdInput.MoveTextStart(false);
            _cmdInput.text = "";
            _cmdInput.MoveTextEnd(false);
        }

        private void OnRreshLog(string value)
        {
            _outputText.text = value;
            _scrRect.verticalNormalizedPosition = 0;
        }

        protected override void AddEvent()
        {
            base.AddEvent();
            GameEventMgr.Instance.mUIEvtDispatcher.AddEvent<string>(UIEventDefines.ConsoleLogChange, OnRreshLog);
        }

        protected override void RemoveEvent()
        {
            base.RemoveEvent();
            GameEventMgr.Instance.mUIEvtDispatcher.RemoveEvent<string>(UIEventDefines.ConsoleLogChange, OnRreshLog);
        }

        public override void Show(params object[] args)
        {
            base.Show(args);
            ClearInput();
        }
    }
}
