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
    }
}
