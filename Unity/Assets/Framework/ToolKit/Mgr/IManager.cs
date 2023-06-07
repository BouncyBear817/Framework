using System;

namespace Framework
{
    public interface IManager
    {
        void Init();

        void RegisterEvent<T>(T msgEvent, OnEvent onEvent) where T : IConvertible;
        
        void UnRegisterEvent<T>(T msgEvent, OnEvent onEvent) where T : IConvertible;
        
        void SendEvent<T>(T msgEvent, params object[] param) where T : IConvertible;

        void SendMsg(IMsg msg);
    }
}