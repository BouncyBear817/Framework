namespace Framework
{
    /// <summary>
    /// 事件池
    /// </summary>
    /// <typeparam name="T">事件类型</typeparam>
    public sealed partial class EventPool<T> where T : BaseEventArgs
    {
        /// <summary>
        /// 事件节点
        /// </summary>
        private sealed class Event : IReference
        {
            private object mSender;
            private T mEventArgs;

            public Event()
            {
                mSender = null;
                mEventArgs = null;
            }

            /// <summary>
            /// 事件
            /// </summary>
            public object Sender => mSender;

            /// <summary>
            /// 事件参数
            /// </summary>
            public T EventArgs => mEventArgs;

            /// <summary>
            /// 创建事件
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            /// <returns></returns>
            public static Event Create(object sender, T e)
            {
                var eventNode = ReferencePool.Acquire<Event>();
                eventNode.mSender = sender;
                eventNode.mEventArgs = e;
                return eventNode;
            }

            /// <summary>
            /// 清理事件。
            /// </summary>
            public void Clear()
            {
                mSender = null;
                mEventArgs = null;
            }
        }
    }
}