namespace Framework
{
    /// <summary>
    /// 日志助手，可继承复写
    /// </summary>
    public abstract class LogHelper : ILogHelper
    {
        public abstract void Log(LogLevel logLevel, object message);

        public abstract void Log(LogLevel logLevel, string format, params object[] args);
    }
}