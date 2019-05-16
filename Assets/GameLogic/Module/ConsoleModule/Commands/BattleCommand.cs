namespace GameConsole
{
    public class BattleCommand : ICommand
    {
        public string mCmdName { get; set; } = ConsoleName.BATTLE;

        public bool ExecCommand(string[] args = null)
        {
            //int enemyId = LocalDataMgr.ServerIndex == 0 ? 16777279 : 16777347;//16777217;
            //if (args == null || args.Length == 0)
            //{
            //    ConsoleLogger.Warning("battle command, no input enemy player id, auto fight with roboot001, id:" + enemyId);
            //}
            //else
            //{
            //    int.TryParse(args[0], out enemyId);
            //}
            //if (enemyId <= 0)
            //{
            //    enemyId = 16777245;
            //    ConsoleLogger.Error("battle command, enemy player id invalid");
            //}
            //BattleDataModel.Instance.ReqStartBattle(BattleType.Pvp, enemyId);
            return true;
        }
    }
}
