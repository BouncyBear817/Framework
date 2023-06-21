using System;

namespace Framework
{
    /// <summary>
    /// 管理器接口
    /// </summary>
    public interface IManager
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void Init();

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="onEvent"></param>
        /// <typeparam name="T"></typeparam>
        void RegisterEvent<T>(T eventId, OnEvent onEvent) where T : IConvertible;
        
        /// <summary>
        /// 取消注册事件
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="onEvent"></param>
        /// <typeparam name="T"></typeparam>
        void UnRegisterEvent<T>(T eventId, OnEvent onEvent) where T : IConvertible;
        
        /// <summary>
        /// 发送事件消息，限于管理器内部通信
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="param"></param>
        /// <typeparam name="T"></typeparam>
        void SendEvent<T>(T eventId, params object[] param) where T : IConvertible;
        
        /// <summary>
        /// 发送消息，用于管理器间通信
        /// </summary>
        /// <param name="msg"></param>
        void SendMsg(IMsg msg);
    }
}