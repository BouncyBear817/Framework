using System;

namespace Framework
{
    /// <summary>
    /// 事件管理器
    /// </summary>
    public sealed class EventManager : FrameworkModule, IEventManager
    {
        private readonly EventPool<BaseEventArgs> mEventPool;

        public EventManager()
        {
            mEventPool = new EventPool<BaseEventArgs>(EventPoolMode.AllowNoHandler | EventPoolMode.AllowMultiHandler);
        }

        public override int Priority => 7;

        /// <summary>
        /// 事件处理函数的数量
        /// </summary>
        public int EventHandlerCount => mEventPool.EventHandlerCount;

        /// <summary>
        /// 事件数量
        /// </summary>
        public int EventCount => mEventPool.EventCount;


        /// <summary>
        /// 事件处理函数的数量，指定id
        /// </summary>
        /// <param name="id">事件编号</param>
        /// <returns>事件处理函数的数量</returns>
        public int Count(int id)
        {
            return mEventPool.Count(id);
        }

        /// <summary>
        /// 检查是否存在事件处理函数
        /// </summary>
        /// <param name="id">事件编号</param>
        /// <param name="handler">事件处理函数</param>
        /// <returns>是否存在事件处理函数</returns>
        public bool Check(int id, EventHandler<BaseEventArgs> handler)
        {
            return mEventPool.Check(id, handler);
        }

        /// <summary>
        /// 订阅事件处理函数
        /// </summary>
        /// <param name="id">事件编号</param>
        /// <param name="handler">事件处理函数</param>
        public void Subscribe(int id, EventHandler<BaseEventArgs> handler)
        {
            mEventPool.Subscribe(id, handler);
        }

        /// <summary>
        /// 取消订阅事件处理函数
        /// </summary>
        /// <param name="id">事件编号</param>
        /// <param name="handler">事件处理函数</param>
        public void UnSubscribe(int id, EventHandler<BaseEventArgs> handler)
        {
            mEventPool.UnSubscribe(id, handler);
        }

        /// <summary>
        /// 设置默认事件处理函数
        /// </summary>
        /// <param name="handler">事件处理函数</param>
        public void SetDefaultHandler(EventHandler<BaseEventArgs> handler)
        {
            mEventPool.SetDefaultHandler(handler);
        }

        /// <summary>
        /// 抛出事件，线程安全
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件参数</param>
        public void Fire(object sender, BaseEventArgs e)
        {
            mEventPool.Fire(sender, e);
        }

        /// <summary>
        /// 抛出事件，线程不安全
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件参数</param>
        public void FireNow(object sender, BaseEventArgs e)
        {
            mEventPool.FireNow(sender, e);
        }

        /// <summary>
        /// 框架模块轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间</param>
        /// <param name="realElapseSeconds">真实流逝时间</param>
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            mEventPool.Update(elapseSeconds, realElapseSeconds);
        }

        /// <summary>
        /// 关闭并清理框架模块
        /// </summary>
        public override void Shutdown()
        {
            mEventPool.Shutdown();
        }
    }
}