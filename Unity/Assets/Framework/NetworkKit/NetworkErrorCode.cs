/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/2/19 17:17:45
 * Description:
 * Modify Record:
 *************************************************************/

namespace Framework
{
    /// <summary>
    /// 网络错误码
    /// </summary>
    public enum NetworkErrorCode : byte
    {
        /// <summary>
        /// 未知错误
        /// </summary>
        Unknown = 0,
        
        /// <summary>
        /// 地址族错误
        /// </summary>
        AddressFamilyError,
        
        /// <summary>
        /// socket错误
        /// </summary>
        SocketError,
        
        /// <summary>
        /// 连接错误
        /// </summary>
        ConnectError,
        
        /// <summary>
        /// 发送错误
        /// </summary>
        SendError,
        
        /// <summary>
        /// 接收错误
        /// </summary>
        ReceiveError,
        
        /// <summary>
        /// 序列化错误
        /// </summary>
        SerializeError,
        
        /// <summary>
        /// 反序列化消息包头错误
        /// </summary>
        DeserializePacketHeaderError,
        
        /// <summary>
        /// 反序列化消息包错误
        /// </summary>
        DeserializePacketError
    }
}