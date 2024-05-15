/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/01/30 16:12:05
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using Framework;

namespace Runtime
{
    public class DefaultLogHelper : LogHelper
    {
        public DefaultLogHelper()
        {
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="logLevel">日志等级</param>
        /// <param name="message">日志内容</param>
        public override void Log(LogLevel logLevel, object message)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    UnityEngine.Debug.Log($"<color=#FFFFFF>{message}</color>");
                    break;
                case LogLevel.Info:
                    UnityEngine.Debug.Log($"<color=#00FF00>{message}</color>");
                    break;
                case LogLevel.Warning:
                    UnityEngine.Debug.LogWarning($"<color=#FFFF00>{message}</color>");
                    break;
                case LogLevel.Error:
                    UnityEngine.Debug.LogError($"<color=#FF0000>{message}</color>");
                    break;
                default:
                    throw new Exception($"<color=#FF0000>{message}</color>");
            }
        }
    }
}