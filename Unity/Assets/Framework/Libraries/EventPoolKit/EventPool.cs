using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 事件池
    /// </summary>
    /// <typeparam name="T">事件类型</typeparam>
    public sealed partial class EventPool<T> where T : BaseEventArgs
    {
        private readonly MultiDictionary<int, EventHandler<T>> mEventHandlers;
        private readonly Queue<Event> mEvents;

        private readonly EventPoolMode mEventPoolMode;
        private EventHandler<T> mDefaultEventHandler;

        public EventPool(EventPoolMode eventPoolMode)
        {
            mEventHandlers = new MultiDictionary<int, EventHandler<T>>();
            mEvents = new Queue<Event>();
            mEventPoolMode = eventPoolMode;
            mDefaultEventHandler = null;
        }

        /// <summary>
        /// 事件处理函数的数量
        /// </summary>
        public int EventHandlerCount => mEventHandlers.Count;

        /// <summary>
        /// 事件数量
        /// </summary>
        public int EventCount => mEvents.Count;

        /// <summary>
        /// 框架模块轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间</param>
        /// <param name="realElapseSeconds">真实流逝时间</param>
        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            lock (mEvents)
            {
                while (mEvents.Count > 0)
                {
                    var eventNode = mEvents.Dequeue();
                    HandledEvent(eventNode.Sender, eventNode.EventArgs);
                    ReferencePool.Release(eventNode);
                }
            }
        }

        /// <summary>
        /// 关闭并清理框架模块
        /// </summary>
        public void Shutdown()
        {
            Clear();
            mEventHandlers.Clear();
            mDefaultEventHandler = null;
        }

        /// <summary>
        /// 清理事件。
        /// </summary>
        public void Clear()
        {
            lock (mEvents)
            {
                mEvents.Clear();
            }
        }

        /// <summary>
        /// 事件处理函数的数量，指定id
        /// </summary>
        /// <param name="id">事件编号</param>
        /// <returns>事件处理函数的数量</returns>
        public int Count(int id)
        {
            return mEventHandlers.TryGetValue(id, out var linkedList) ? linkedList.Count : 0;
        }

        /// <summary>
        /// 检查是否存在事件处理函数
        /// </summary>
        /// <param name="id">事件编号</param>
        /// <param name="handler">事件处理函数</param>
        /// <returns>是否存在事件处理函数</returns>
        public bool Check(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new Exception("Event handler is invalid.");
            }

            return mEventHandlers.Contains(id, handler);
        }

        /// <summary>
        /// 订阅事件处理函数
        /// </summary>
        /// <param name="id">事件编号</param>
        /// <param name="handler">事件处理函数</param>
        public void Subscribe(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new Exception("Event handler is invalid.");
            }

            if (!mEventHandlers.Contains(id))
            {
                mEventHandlers.Add(id, handler);
            }
            else if ((mEventPoolMode & EventPoolMode.AllowMultiHandler) != EventPoolMode.AllowMultiHandler)
            {
                throw new Exception($"Event ({id}) not allow multi handler.");
            }
            else if ((mEventPoolMode & EventPoolMode.AllowDuplicateHandler) != EventPoolMode.AllowDuplicateHandler &&
                     Check(id, handler))
            {
                throw new Exception($"Event ({id}) not allow duplicate handler.");
            }
            else
            {
                mEventHandlers.Add(id, handler);
            }
        }

        /// <summary>
        /// 取消订阅事件处理函数
        /// </summary>
        /// <param name="id">事件编号</param>
        /// <param name="handler">事件处理函数</param>
        public void UnSubscribe(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new Exception("Event handler is invalid.");
            }

            if (!mEventHandlers.Remove(id, handler))
            {
                throw new Exception($"Event ({id}) not exists handler that needed to be removed.");
            }
        }

        /// <summary>
        /// 设置默认事件处理函数
        /// </summary>
        /// <param name="handler">事件处理函数</param>
        public void SetDefaultHandler(EventHandler<T> handler)
        {
            mDefaultEventHandler = handler;
        }

        /// <summary>
        /// 抛出事件，线程安全
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件参数</param>
        public void Fire(object sender, T e)
        {
            if (e == null)
            {
                throw new Exception($"Event is invalid.");
            }

            var eventNode = Event.Create(sender, e);
            lock (mEvents)
            {
                mEvents.Enqueue(eventNode);
            }
        }

        /// <summary>
        /// 抛出事件，线程不安全
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件参数</param>
        public void FireNow(object sender, T e)
        {
            if (e == null)
            {
                throw new Exception($"Event is invalid.");
            }

            HandledEvent(sender, e);
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件参数</param>
        private void HandledEvent(object sender, T e)
        {
            if (mEventHandlers.TryGetValue(e.Id, out var linkedList))
            {
                var current = linkedList.First;
                while (current != null)
                {
                    current.Value(sender, e);
                    current = current.Next;
                }
            }
            else if (mDefaultEventHandler != null)
            {
                mDefaultEventHandler(sender, e);
            }

            ReferencePool.Release(e);
        }
    }
}