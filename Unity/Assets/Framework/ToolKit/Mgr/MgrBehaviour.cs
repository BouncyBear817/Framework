using System;
using UnityEngine;

namespace Framework
{
    public abstract class MgrBehaviour : MgrBaseBehaviour
    {
        private readonly EventListener mEventListener = NonPublicObjectPool<EventListener>.Instance.Allocate();

        public bool RegisterEvent<T>(T msgEvent, OnEvent process) where T : IConvertible
        {
            return mEventListener.Register(msgEvent, process);
        }

        public void UnRegisterEvent<T>(T msgEvent) where T : IConvertible
        {
            mEventListener.UnRegister(msgEvent);
        }

        public bool SendEvent<T>(T msgEvent, params object[] param) where T : IConvertible
        {
            SendMsg(Msg.Allocate(msgEvent));
            return true;
        }

        public void SendMsg(IMsg msg)
        {
            Process(msg.EventId, msg);
        }

        protected override void ProcessMsg(int eventId, Msg msg)
        {
            mEventListener.Send(eventId, msg);
        }
        
        
    }
}