using System;

namespace Framework
{
    /// <summary>
    /// 日志助手，可继承复写
    /// </summary>
    public abstract class LogHelper : ILogHelper
    {
        public virtual void Log(LogLevel logLevel, object message)
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

        public virtual void Log(LogLevel logLevel, string format, params object[] args)
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