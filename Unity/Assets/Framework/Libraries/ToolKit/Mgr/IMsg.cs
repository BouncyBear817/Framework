using System;

namespace Framework
{
    public interface IMsg
    {
        int EventId { get; set; }
        
        int ManagerId { get; set; }
        
        bool Processed { get; set; }

        void OnRecycle();
    }
    
    public class Msg : IMsg, IPoolable
    {
        public int EventId { get; set; }
        public int ManagerId { get; set; }
        public bool Processed { get; set; }
        void IMsg.OnRecycle()
        {
            ObjectPool<Msg>.Instance.Recycle(this);
        }

        public static Msg Allocate<T>(T eventId) where T : IConvertible
        {
            var msg = ObjectPool<Msg>.Instance.Allocate();
            msg.EventId = eventId.ToInt32(null);

            return msg;
        }

        void IPoolable.OnRecycle()
        {
            Processed = false;
        }
    }
}