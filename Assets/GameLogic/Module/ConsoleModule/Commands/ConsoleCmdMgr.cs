using System;
using System.Collections.Generic;

namespace GameConsole
{

    public class ConsoleName
    {
        public static readonly string HELP = "help";
        public static readonly string LOGIN = "login";
        public static readonly string BATTLE = "battle";
        public static readonly string TEST = "test";
    }

    public class ConsoleCmdMgr : Singleton<ConsoleCmdMgr>
    {
        private Dictionary<string, ICommand> _dictAllCommands;
        private bool _blConsoleModuleShow = false;

        public void Init()
        {
            _dictAllCommands = new Dictionary<string, ICommand>();
            _dictAllCommands.Add(ConsoleName.HELP, new HelpCommand());
            _dictAllCommands.Add(ConsoleName.BATTLE, new BattleCommand());
            _dictAllCommands.Add(ConsoleName.TEST, new CommonCommand());

            GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(UIEventDefines.CloseConsoleModule, OnCloseConsole);
            GameEventMgr.Instance.mUIEvtDispatcher.AddEvent(UIEventDefines.OpenConsoleModule, OnOpenConsole);
        }

        private void OnOpenConsole()
        {
            if (_blConsoleModuleShow)
                return;
            _blConsoleModuleShow = true;
            GameUIMgr.Instance.OpenModule(ModuleID.Console);
        }

        private void OnCloseConsole()
        {
            if (!_blConsoleModuleShow)
                return;
            _blConsoleModuleShow = false;
            GameUIMgr.Instance.CloseModule(ModuleID.Console);
        }

        public void DoShortCutConsole()
        {
            if (_blConsoleModuleShow)
                OnCloseConsole();
            else
                OnOpenConsole();
        }

        public void ExecCommand(string inputCmd)
        {
            string[] cmds = inputCmd.Split(' ');
            if (cmds.Length == 0)
            {
                ConsoleLogger.Error("input command params error!!");
                return;
            }

            string cmdName = cmds[0];
            if (!_dictAllCommands.ContainsKey(cmdName))
            {
                ConsoleLogger.Error("command name:" + cmdName + " unregistered!!");
                return;
            }
            string[] args = null;
            if (cmds.Length > 1)
            {
                args = new string[cmds.Length - 1];
                Array.Copy(cmds, 1, args, 0, args.Length);
            }
            ICommand cmd = _dictAllCommands[cmdName];
            if (cmd.ExecCommand(args))
                ConsoleLogger.Log("exec command:" + inputCmd + " success.");
            else
                ConsoleLogger.Error("exec command:" + inputCmd + " failed.");
        }
    }
}