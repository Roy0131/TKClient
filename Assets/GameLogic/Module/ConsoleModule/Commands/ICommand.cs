
namespace GameConsole
{
    public interface ICommand
    {
        string mCmdName { get; set; }
        bool ExecCommand(string[] args = null);
    }
}