namespace Framework
{
    /// <summary>
    /// 日志记录器接口
    /// </summary>
    public interface ILogHelper
    {
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="logLevel">日志等级</param>
        /// <param name="message">日志内容</param>
        void Log(LogLevel logLevel, object message);

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="logLevel">日志等级</param>
        /// <param name="format">日志内容，复合格式字符串</param>
        /// <param name="args">参数格式</param>
        void Log(LogLevel logLevel, string format, params object[] args);
    }
}
