namespace GameConsole
{
    public class HelpCommand : ICommand
    {
        public string mCmdName { get; set; } = ConsoleName.HELP;
        private string _helpLog = "all commands:" +
                                  "\n\t\thelp ---- 查看所以命令, 无参数, 示例:help" +
                                  "\n\t\tlogin ---- 登陆游戏, 参数为登陆账号, 示例:login lhgame" +
                                  "\n\t\tbattle ---- 开始战斗, 参数为对战玩家playerid, 非玩家账号, 示例: battle 100001, 如果不输入，默认为roboot001机器人" +
                                  "\n\t\ttest ---- 测试指令，示例: test fight_stage 关卡ID";

        public bool ExecCommand(string[] args = null)
        {
            ConsoleLogger.Warning(_helpLog);
            return true;
        }
    }
}
