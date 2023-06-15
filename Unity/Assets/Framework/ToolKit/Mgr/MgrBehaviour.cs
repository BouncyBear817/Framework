using System;

namespace Framework
{
    public abstract class MgrBehaviour : MgrBaseBehaviour, IManager
    {
        private EventListener mEventListener = NonPublicObjectPool<EventListener>.Instance.Allocate();
        
        public abstract int ManagerId { get; }

        public override IManager Manager => this;

        public void Init()
        {
        }

        public void RegisterEvent<T>(T eventId, OnEvent onEvent) where T : IConvertible
        {
            mEventListener.Register(eventId, onEvent);
        }

        public void UnRegisterEvent<T>(T eventId, OnEvent onEvent) where T : IConvertible
        {
            mEventListener.UnRegister(eventId, onEvent);
        }
        
        /// <summary>
        /// 发送事件消息：常用于一个manager内部
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="param"></param>
        /// <typeparam name="T"></typeparam>
        public void SendEvent<T>(T eventId, params object[] param) where T : IConvertible
        {
            var id = eventId.ToInt32(null);
            Process(id, param);
        }
        
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        public void SendMsg(IMsg msg)
        {
            //如果managerId一致，转为内部消息
            if (msg.ManagerId == ManagerId)
            {
                SendEvent(msg.EventId, msg);
            }
            else
            {
                MsgCenter.Instance.SendMsg(msg);
            }
        }
        
        protected override void ProcessMsg(int eventId, Msg msg)
        {
            mEventListener?.Send(eventId, msg);
        }
        
        protected override void ProcessEvent(int eventId, params object[] param)
        {
            mEventListener?.Send(eventId, param);
        }
        
        protected override void OnBeforeDestroy()
        {
            mEventListener?.OnRecycle();
        }
    }
}