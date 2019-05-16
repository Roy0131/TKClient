using System.Collections.Generic;

namespace GameConsole
{
    public class CommonCommand : ICommand
    {
        public string mCmdName { get; set; } = ConsoleName.TEST;
        public bool ExecCommand(string[] args = null)
        {
            if (args == null || args.Length == 0)
            {
                ConsoleLogger.Warning("test command, not command input!!!");
                return false;
            }

            string cmd = args[0];
            IList<string> p = new List<string>();
            for (int i = 1; i < args.Length; i++)
                p.Add(args[i]);
            GameNetMgr.Instance.mGameServer.ReqTestCommand(cmd, p);
            return true;
        }
    }
}