using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public delegate void OnEvent(int eventId, params object[] param);
    
    /// <summary>
    /// 事件监听器，事件广播
    /// </summary>
    public class EventListener : ISingleton, IPoolable
    {
        private Dictionary<int, EventWrap> mAllEventMap = new Dictionary<int, EventWrap>();

        public static EventListener Instance => SingletonProperty<EventListener>.Instance;

        private EventListener()
        {
        }

        /// <summary>
        /// 内部类，事件包，记录一个事件对应的所有的监听
        /// </summary>
        private class EventWrap
        {
            private readonly LinkedList<OnEvent> mEventList = new LinkedList<OnEvent>();

            public bool Execute(int eventId, params object[] param)
            {
                var next = mEventList.First;

                while (next != null)
                {
                    var call = next.Value;

                    call(eventId, param);

                    next = next.Next;
                }

                return true;
            }

            public bool Add(OnEvent process)
            {
                if (!mEventList.Contains(process))
                {
                    mEventList.AddLast(process);
                    return true;
                }

                return false;
            }

            public void Remove(OnEvent process)
            {
                if (mEventList.Contains(process))
                {
                    mEventList.Remove(process);
                }
            }

            public void RemoveAll()
            {
                mEventList.Clear();
            }
        }
        
        public bool Register<T>(T msgEvent, OnEvent process) where T : IConvertible
        {
            var key = msgEvent.ToInt32(null);

            if (!mAllEventMap.TryGetValue(key, out var wrap))
            {
                wrap = new EventWrap();
                mAllEventMap.Add(key, wrap);
            }

            if (wrap.Add(process))
            {
                return true;
            }

            Logger.Warning("Already Register Same Event : " + typeof(T).Name);
            return false;
        }

        public void UnRegister<T>(T msgEvent, OnEvent process) where T : IConvertible
        {
            if (mAllEventMap.TryGetValue(msgEvent.ToInt32(null), out var wrap))
            {
                wrap.Remove(process);
            }
        }

        public void UnRegister<T>(T msgEvent) where T : IConvertible
        {
            var key = msgEvent.ToInt32(null);
            if (mAllEventMap.TryGetValue(key, out var wrap))
            {
                wrap.RemoveAll();

                mAllEventMap.Remove(key);
            }
        }

        public bool Send<T>(T msgEvent, params object[] param) where T : IConvertible
        {
            var key = msgEvent.ToInt32(null);
            if (mAllEventMap.TryGetValue(key, out var wrap))
            {
                return wrap.Execute(key, param);
            }

            Debug.LogWarning("No Register Event : " + typeof(T).Name);
            return false;
        }

        #region 事件监听器静态方法

        public static bool RegisterEvent<T>(T msgEvent, OnEvent process) where T : IConvertible
        {
            return Instance.Register<T>(msgEvent, process);
        }

        public static void UnRegisterEvent<T>(T msgEvent, OnEvent process) where T : IConvertible
        {
            Instance.UnRegister<T>(msgEvent, process);
        }

        public static void UnRegisterEvent<T>(T msgEvent) where T : IConvertible
        {
            Instance.UnRegister<T>(msgEvent);
        }

        public static bool SendEvent<T>(T msgEvent, int eventId, params object[] param) where T : IConvertible
        {
            return Instance.Send(msgEvent, eventId, param);
        }

        public static void RecycleAll()
        {
            Instance.OnRecycle();
        }

        #endregion

        public void OnRecycle()
        {
            mAllEventMap.Clear();
        }

        public void OnSingletonInit()
        {
            
        }
    }
}