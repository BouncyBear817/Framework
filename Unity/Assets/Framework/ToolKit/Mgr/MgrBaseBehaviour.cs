using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public abstract class MgrBaseBehaviour : MonoBehaviour
    {
        protected bool ReceiveMsgInActive = true;

        private List<ushort> mCacheEventIds = new List<ushort>();

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
            }
        }

        protected virtual void ProcessMsg(int eventId, Msg msg)
        {
        }

        public abstract IManager Manager { get; }

        protected void RegisterEvent<T>(T eventId) where T : IConvertible
        {
            mCacheEventIds.Add(eventId.ToUInt16(null));
            Manager.RegisterEvent(eventId, Process);
        }

        protected void UnRegisterEvent<T>(T eventId) where T : IConvertible
        {
            mCacheEventIds.Remove(eventId.ToUInt16(null));
            Manager.UnRegisterEvent(eventId, Process);
        }

        public void SendEvent<T>(T eventId, params object[] param) where T : IConvertible
        {
            Manager.SendEvent(eventId, param);
        }

        public void SendMsg(IMsg msg)
        {
            Manager.SendMsg(msg);
        }

        protected virtual void OnDestroy()
        {
            if (Application.isPlaying)
            {
                mCacheEventIds.ForEach(eventId =>
                {
                    Manager.UnRegisterEvent(eventId, Process);
                });
                mCacheEventIds.Clear();
            }
        }
    }
}