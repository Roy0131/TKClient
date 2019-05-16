using System.Collections.Generic;

namespace GameConsole
{
    public enum ConsoleLogType
    {
        log,
        warning,
        error,
    }
    public class ConsoleLogger
    {
        private static List<string> _allLogs = new List<string>();
        private static string _logBuffers = "";
        public static void Log(string value)
        {
            PushLog("<color=#00ff00>" + value + "</color>");
        }

        public static void Warning(string value)
        {
            PushLog("<color=#ffffff>" + value + "</color>");
        }

        public static void Error(string value)
        {
            PushLog("<color=#ff0000>" + value + "</color>");
        }

        public static void PushLog(string value)
        {
            if (_allLogs.Count > 200)
            {
                _logBuffers = "";
                _allLogs.RemoveRange(0, _allLogs.Count - 50);
                for (int i = 0; i < _allLogs.Count; i++)
                    _logBuffers += (_allLogs[i] + "\n");
            }
            else
            {
                _logBuffers += (value + "\n");
            }
            GameEventMgr.Instance.mUIEvtDispatcher.DispathEvent<string>(UIEventDefines.ConsoleLogChange, _logBuffers);
        }
    }
}
