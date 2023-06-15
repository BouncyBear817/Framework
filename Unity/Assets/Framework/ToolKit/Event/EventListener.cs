using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public delegate void OnEvent(int key, params object[] param);

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

            public bool Execute(int key, params object[] param)
            {
                var next = mEventList.First;

                while (next != null)
                {
                    var call = next.Value;

                    call(key, param);

                    next = next.Next;
                }

                return true;
            }

            public bool Add(OnEvent onEvent)
            {
                if (!mEventList.Contains(onEvent))
                {
                    mEventList.AddLast(onEvent);
                    return true;
                }

                return false;
            }

            public void Remove(OnEvent onEvent)
            {
                if (mEventList.Contains(onEvent))
                {
                    mEventList.Remove(onEvent);
                }
            }

            public void RemoveAll()
            {
                mEventList.Clear();
            }
        }
        
        public bool Register<T>(T eventId, OnEvent onEvent) where T : IConvertible
        {
            var id = eventId.ToInt32(null);

            if (!mAllEventMap.TryGetValue(id, out var wrap))
            {
                wrap = new EventWrap();
                mAllEventMap.Add(id, wrap);
            }

            if (wrap.Add(onEvent))
            {
                return true;
            }

            Logger.Warning("Already Register Same Event : " + typeof(T).Name);
            return false;
        }
        
        /// <summary>
        /// 卸载指定的事件监听
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="process"></param>
        /// <typeparam name="T"></typeparam>
        public void UnRegister<T>(T eventId, OnEvent process) where T : IConvertible
        {
            var id = eventId.ToInt32(null);
            
            if (mAllEventMap.TryGetValue(id, out var wrap))
            {
                wrap.Remove(process);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventId"></param>
        /// <typeparam name="T"></typeparam>
        public void UnRegister<T>(T eventId) where T : IConvertible
        {
            var id = eventId.ToInt32(null);
            if (mAllEventMap.TryGetValue(id, out var wrap))
            {
                wrap.RemoveAll();

                mAllEventMap.Remove(id);
            }
        }

        public bool Send<T>(T eventId, params object[] param) where T : IConvertible
        {
            var id = eventId.ToInt32(null);
            if (mAllEventMap.TryGetValue(id, out var wrap))
            {
                // 默认onEvent中key为eventId
                return wrap.Execute(id, param);
            }

            Debug.LogWarning("No Register Event : " + typeof(T).Name);
            return false;
        }

        #region 事件监听器静态方法

        public static bool RegisterEvent<T>(T eventId, OnEvent process) where T : IConvertible
        {
            return Instance.Register<T>(eventId, process);
        }

        public static void UnRegisterEvent<T>(T eventId, OnEvent process) where T : IConvertible
        {
            Instance.UnRegister<T>(eventId, process);
        }

        public static void UnRegisterEvent<T>(T eventId) where T : IConvertible
        {
            Instance.UnRegister<T>(eventId);
        }

        public static bool SendEvent<T>(T eventId, params object[] param) where T : IConvertible
        {
            return Instance.Send(eventId, param);
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