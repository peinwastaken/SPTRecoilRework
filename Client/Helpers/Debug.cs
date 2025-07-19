using BepInEx.Logging;
using PeinRecoilRework.Config.Settings;

namespace PeinRecoilRework.Helpers
{
    public class Debug
    {
        public static ManualLogSource Logger { get; set; }
        private static bool isDebug => DebugSettings.EnableDebug.Value;

        public static void Log(string message, LogLevel logLevel = LogLevel.Info)
        {
            if (!isDebug) return;

            Logger?.Log(logLevel, message);
        }

        public static void LogInfo(string message)
        {
            Log(message, LogLevel.Info);
        }

        public static void LogWarning(string message)
        {
            Log(message, LogLevel.Warning);
        }

        public static void LogError(string message)
        {
            Log(message, LogLevel.Error);
        }
    }
}
