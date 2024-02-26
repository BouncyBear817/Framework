using System;

namespace Framework
{
    public interface IEventListener
    {
        void Init();

        bool RegisterEvent<T>(T msgEvent, OnEvent process) where T : IConvertible;

        void UnRegisterEvent<T>(T msgEvent) where T : IConvertible;

        bool SendEvent<T>(T msgEvent, params object[] param) where T : IConvertible;
    }
}