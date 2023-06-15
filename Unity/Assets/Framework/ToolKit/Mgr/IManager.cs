using System;

namespace Framework
{
    public interface IManager
    {
        void Init();

        void RegisterEvent<T>(T eventId, OnEvent onEvent) where T : IConvertible;
        
        void UnRegisterEvent<T>(T eventId, OnEvent onEvent) where T : IConvertible;
        
        void SendEvent<T>(T eventId, params object[] param) where T : IConvertible;

        void SendMsg(IMsg msg);
    }
}