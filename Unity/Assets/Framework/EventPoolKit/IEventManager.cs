using System;

namespace Framework
{
    /// <summary>
    /// 事件管理器接口
    /// </summary>
    public interface IEventManager
    {
        /// <summary>
        /// 事件处理函数的数量
        /// </summary>
        public int EventHandlerCount { get; }
        
        /// <summary>
        /// 事件数量
        /// </summary>
        public int EventCount { get; }

        /// <summary>
        /// 事件处理函数的数量，指定id
        /// </summary>
        /// <param name="id">事件编号</param>
        /// <returns>事件处理函数的数量</returns>
        int Count(int id);

        /// <summary>
        /// 检查是否存在事件处理函数
        /// </summary>
        /// <param name="id">事件编号</param>
        /// <param name="handler">事件处理函数</param>
        /// <returns>是否存在事件处理函数</returns>
        bool Check(int id, EventHandler<BaseEventArgs> handler);
        
        /// <summary>
        /// 订阅事件处理函数
        /// </summary>
        /// <param name="id">事件编号</param>
        /// <param name="handler">事件处理函数</param>
        void Subscribe(int id, EventHandler<BaseEventArgs> handler);
        
        /// <summary>
        /// 取消订阅事件处理函数
        /// </summary>
        /// <param name="id">事件编号</param>
        /// <param name="handler">事件处理函数</param>
        void UnSubscribe(int id, EventHandler<BaseEventArgs> handler);
        
        /// <summary>
        /// 设置默认事件处理函数
        /// </summary>
        /// <param name="handler">事件处理函数</param>
        void SetDefaultHandler(EventHandler<BaseEventArgs> handler);
        
        /// <summary>
        /// 抛出事件，线程安全
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件参数</param>
        void Fire(object sender, BaseEventArgs e);
        
        /// <summary>
        /// 抛出事件，线程不安全
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件参数</param>
        void FireNow(object sender, BaseEventArgs e);
    }
}