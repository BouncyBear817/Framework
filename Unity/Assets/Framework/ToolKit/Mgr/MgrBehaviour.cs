using System;
using UnityEngine;

namespace Framework
{
    public abstract class MgrBehaviour : MgrBaseBehaviour, IManager
    {
        private EventListener mEventListener = NonPublicObjectPool<EventListener>.Instance.Allocate();

        public void Init()
        {
        }

        public void RegisterEvent<T>(T msgEvent, OnEvent onEvent) where T : IConvertible
        {
            mEventListener.Register(msgEvent, onEvent);
        }

        public void UnRegisterEvent<T>(T msgEvent, OnEvent onEvent) where T : IConvertible
        {
            mEventListener.UnRegister(msgEvent, onEvent);
        }

        public override IManager Manager => this;

        public void SendEvent<T>(T msgEvent, params object[] param) where T : IConvertible
        {
            mEventListener.Send(msgEvent, param);
        }

        public void SendMsg(IMsg msg)
        {
            Process(msg.EventId, msg);
        }
    }
}