using System;

namespace Framework
{
    public class LoggerHelper : ILogHelper
    {
        public void Log(LogLevel logLevel, object message)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    UnityEngine.Debug.Log(message);
                    break;
                case LogLevel.Info:
                    UnityEngine.Debug.Log(message);
                    break;
                case LogLevel.Warning:
                    UnityEngine.Debug.LogWarning(message);
                    break;
                case LogLevel.Error:
                    UnityEngine.Debug.LogError(message);
                    break;
            }
        }

        public void Log(LogLevel logLevel, string format, params object[] args)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    UnityEngine.Debug.LogFormat(format, args);
                    break;
                case LogLevel.Info:
                    UnityEngine.Debug.LogFormat(format, args);
                    break;
                case LogLevel.Warning:
                    UnityEngine.Debug.LogWarningFormat(format,  args);
                    break;
                case LogLevel.Error:
                    UnityEngine.Debug.LogErrorFormat(format, args);
                    break;
            }
        }
    }
}