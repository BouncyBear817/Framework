using System;

namespace Framework
{
    public interface IMsg
    {
        int EventId { get; set; }
        
        bool Processed { get; set; }
    }
    
    public class Msg : IMsg, IPoolable
    {
        public int EventId { get; set; }
        public bool Processed { get; set; }

        public static Msg Allocate<T>(T eventId) where T : IConvertible
        {
            var msg = ObjectPool<Msg>.Instance.Allocate();
            msg.EventId = eventId.ToInt32(null);

            return msg;
        }
        
        public void OnRecycle()
        {
            Processed = false;
        }
    }
}