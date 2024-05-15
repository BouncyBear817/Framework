namespace Framework
{
    /// <summary>
    /// 日志记录类
    /// </summary>
    public static class Log
    {
        private static ILogHelper sLogHelper = null;

        /// <summary>
        /// 设置日志辅助器
        /// </summary>
        /// <param name="logHelper"></param>
        public static void SetLogHelper(ILogHelper logHelper)
        {
            sLogHelper = logHelper;
        }

        /// <summary>
        /// 打印调试级别日志，用于记录调试类信息
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Debug(object message)
        {
            sLogHelper?.Log(LogLevel.Debug, message);
        }

        /// <summary>
        /// 打印调试级别日志，用于记录调试类信息
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Debug(string message)
        {
            sLogHelper?.Log(LogLevel.Debug, message);
        }

        /// <summary>
        /// 打印调试级别日志，用于记录调试类信息
        /// </summary>
        /// <param name="format">日志内容，符合格式字符串</param>
        /// <param name="args">格式参数</param>
        public static void Debug(string format, params object[] args)
        {
            sLogHelper?.Log(LogLevel.Debug, string.Format(format, args));
        }

        /// <summary>
        /// 打印信息级别日志，用于记录程序正常运行下的日志
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Info(object message)
        {
            sLogHelper?.Log(LogLevel.Info, message);
        }

        /// <summary>
        /// 打印信息级别日志，用于记录程序正常运行下的日志
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Info(string message)
        {
            sLogHelper?.Log(LogLevel.Info, message);
        }

        /// <summary>
        /// 打印信息级别日志，用于记录程序正常运行下的日志
        /// </summary>
        /// <param name="format">日志内容，符合格式字符串</param>
        /// <param name="args">格式参数</param>
        public static void Info(string format, params object[] args)
        {
            sLogHelper?.Log(LogLevel.Info, string.Format(format, args));
        }

        /// <summary>
        /// 打印警告级别日志，常用于局部功能逻辑错误
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Warning(object message)
        {
            sLogHelper?.Log(LogLevel.Warning, message);
        }

        /// <summary>
        /// 打印警告级别日志，常用于局部功能逻辑错误
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Warning(string message)
        {
            sLogHelper?.Log(LogLevel.Warning, message);
        }

        /// <summary>
        /// 打印警告级别日志，常用于局部功能逻辑错误
        /// </summary>
        /// <param name="format">日志内容，符合格式字符串</param>
        /// <param name="args">格式参数</param>
        public static void Warning(string format, params object[] args)
        {
            sLogHelper?.Log(LogLevel.Warning, string.Format(format, args));
        }

        /// <summary>
        /// 打印错误级别日志，常用于功能逻辑错误情况
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Error(object message)
        {
            sLogHelper?.Log(LogLevel.Error, message);
        }

        /// <summary>
        /// 打印错误级别日志，常用于功能逻辑错误情况
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Error(string message)
        {
            sLogHelper?.Log(LogLevel.Error, message);
        }

        /// <summary>
        /// 打印错误级别日志，常用于功能逻辑错误情况
        /// </summary>
        /// <param name="format">日志内容，符合格式字符串</param>
        /// <param name="args">格式参数</param>
        public static void Error(string format, params object[] args)
        {
            sLogHelper?.Log(LogLevel.Error, string.Format(format, args));
        }

        /// <summary>
        /// 打印严重错误级别日志，常用于功能逻辑错误情况
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Fatal(object message)
        {
            sLogHelper?.Log(LogLevel.Fatal, message);
        }

        /// <summary>
        /// 打印严重错误级别日志，常用于功能逻辑错误情况
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Fatal(string message)
        {
            sLogHelper?.Log(LogLevel.Fatal, message);
        }

        /// <summary>
        /// 打印严重错误级别日志，常用于游戏崩溃或异常时使用
        /// </summary>
        /// <param name="format">日志内容，符合格式字符串</param>
        /// <param name="args">格式参数</param>
        public static void Fatal(string format, params object[] args)
        {
            sLogHelper?.Log(LogLevel.Fatal, string.Format(format, args));
        }
    }
}