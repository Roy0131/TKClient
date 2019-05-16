public static class LogHelper
{
    public static void Log(object value)
    {
        Debuger.Log(value);
    }

    public static void LogWarning(object value)
    {
        Debuger.LogWarning(value);
    }

    public static void LogError(object value)
    {
        Debuger.LogError(value);
    }
}