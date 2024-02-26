/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/01/30 16:12:05
 * Description:   
 * Modify Record: 
 *************************************************************/

using Framework;

namespace Runtime
{
    public class DefaultLogHelper : LogHelper
    {
        public DefaultLogHelper()
        {
        }

        public override void Log(LogLevel logLevel, object message)
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

        public override void Log(LogLevel logLevel, string format, params object[] args)
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