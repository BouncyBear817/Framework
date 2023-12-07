
namespace Framework
{
    /// <summary>
    /// 日志记录类
    /// </summary>
    public static class Log
    {
        private static ILogHelper mLogHelper = null;

        /// <summary>
        /// 设置日志辅助器
        /// </summary>
        /// <param name="logHelper"></param>
        public static void SetLogHelper(ILogHelper logHelper)
        {
            mLogHelper = logHelper;
        }

        /// <summary>
        /// 打印调试级别日志，用于记录调试类信息
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Debug(object message)
        {
            mLogHelper?.Log(LogLevel.Debug, message);
        }

        /// <summary>
        /// 打印调试级别日志，用于记录调试类信息
        /// </summary>
        /// <param name="format">日志内容，符合格式字符串</param>
        /// <param name="args">格式参数</param>
        public static void Debug(string format, params object[] args)
        {
            mLogHelper?.Log(LogLevel.Debug, format, args);
        }

        /// <summary>
        /// 打印信息级别日志，用于记录程序正常运行下的日志
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Info(object message)
        {
            mLogHelper?.Log(LogLevel.Info, message);
        }

        /// <summary>
        /// 打印信息级别日志，用于记录程序正常运行下的日志
        /// </summary>
        /// <param name="format">日志内容，符合格式字符串</param>
        /// <param name="args">格式参数</param>
        public static void Info(string format, params object[] args)
        {
            mLogHelper?.Log(LogLevel.Info, format, args);
        }

        /// <summary>
        /// 打印警告级别日志，常用于异常情况
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Warning(object message)
        {
            mLogHelper?.Log(LogLevel.Warning, message);
        }

        /// <summary>
        /// 打印警告级别日志，常用于异常情况
        /// </summary>
        /// <param name="format">日志内容，符合格式字符串</param>
        /// <param name="args">格式参数</param>
        public static void Warning(string format, params object[] args)
        {
            mLogHelper?.Log(LogLevel.Warning, format, args);
        }

        /// <summary>
        /// 打印错误级别日志，常用于功能逻辑错误情况
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Error(object message)
        {
            mLogHelper?.Log(LogLevel.Error, message);
        }

        /// <summary>
        /// 打印错误级别日志，常用于功能逻辑错误情况
        /// </summary>
        /// <param name="format">日志内容，符合格式字符串</param>
        /// <param name="args">格式参数</param>
        public static void Error(string format, params object[] args)
        {
            mLogHelper?.Log(LogLevel.Error, format, args);
        }

    }
}

