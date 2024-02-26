using System;
using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;

namespace Framework
{
    public abstract class MgrBaseBehaviour : MonoBehaviour
    {
        protected bool ReceiveMsgInActive = true;

        private List<int> mCacheEventIds = new List<int>();

        protected void Process(int eventId, params object[] param)
        {
            if (ReceiveMsgInActive && gameObject.activeInHierarchy || !ReceiveMsgInActive)
            {
                if (param[0] is IMsg msg)
                {
                    ProcessMsg(eventId, msg as Msg);
                    msg.Processed = true;
                    msg.OnRecycle();
                }
                else
                {
                    ProcessEvent(eventId, param);
                }
            }
        }
        
        /// <summary>
        /// 用于接受外部消息（manager外）
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="msg"></param>
        protected virtual void ProcessMsg(int eventId, Msg msg)
        {
        }
        
        /// <summary>
        /// 用于接收内部消息（manager内）
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="param"></param>
        protected virtual void ProcessEvent(int eventId, params object[] param)
        {
        }

        public abstract IManager Manager { get; }

        protected void RegisterEvent<T>(T eventId) where T : IConvertible
        {
            mCacheEventIds.Add(eventId.ToInt32(null));
            Manager.RegisterEvent(eventId, Process);
        }

        protected void UnRegisterEvent<T>(T eventId) where T : IConvertible
        {
            mCacheEventIds.Remove(eventId.ToInt32(null));
            Manager.UnRegisterEvent(eventId, Process);
        }

        protected void UnRegisterAllEvent()
        {
            if (mCacheEventIds != null)
            {
                mCacheEventIds.ForEach(eventId => { Manager.UnRegisterEvent(eventId, Process); });
            }
        }

        public void Send<T>(T eventId, params object[] param) where T : IConvertible
        {
            Manager.SendEvent(eventId, param);
        }

        public void Send(IMsg msg)
        {
            Manager.SendMsg(msg);
        }

        protected virtual void OnDestroy()
        {
            if (Application.isPlaying)
            {
                OnBeforeDestroy();
                UnRegisterAllEvent();
            }
        }
        
        protected virtual void OnBeforeDestroy() {}
    }
}