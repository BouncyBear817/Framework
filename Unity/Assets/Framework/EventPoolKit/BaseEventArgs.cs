namespace Framework
{
    /// <summary>
    /// 事件基类
    /// </summary>
    public abstract class BaseEventArgs : FrameworkEventArgs
    {
        /// <summary>
        /// 事件编号
        /// </summary>
        public abstract int Id { get; }
    }
}