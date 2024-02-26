/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/2/19 16:54:17
 * Description:
 * Modify Record:
 *************************************************************/

using System.IO;

namespace Framework
{
    /// <summary>
    /// 网络频道辅助器接口
    /// </summary>
    public interface INetworkChannelHelper
    {
        /// <summary>
        /// 消息包头长度
        /// </summary>
        int PacketHeaderLength {get;}
        
        /// <summary>
        /// 初始化网络频道辅助器
        /// </summary>
        /// <param name="networkChannel">网络频道辅助器</param>
        void Initialize(INetworkChannel networkChannel);

        /// <summary>
        /// 关闭并清理网络频道辅助器
        /// </summary>
        void Shutdown();

        /// <summary>
        /// 准备进行连接
        /// </summary>
        void PrepareForConnecting();

        /// <summary>
        /// 发送心跳包
        /// </summary>
        bool SendHeartBeat();

        /// <summary>
        /// 序列化消息包
        /// </summary>
        /// <param name="packet">消息包</param>
        /// <param name="destination">要序列化的目标流</param>
        /// <typeparam name="T">消息包类型</typeparam>
        /// <returns></returns>
        bool Serialize<T>(T packet, Stream destination) where T : Packet;

        /// <summary>
        /// 反序列化消息包头
        /// </summary>
        /// <param name="source">要反序列化的来源流</param>
        /// <param name="customErrorData">自定义错误信息</param>
        /// <returns>反序列化的消息包头</returns>
        IPacketHeader DeserializePacketHeader(Stream source, out object customErrorData);

        /// <summary>
        /// 反序列化消息包
        /// </summary>
        /// <param name="packetHeader">消息包头</param>
        /// <param name="source">要反序列化的来源流</param>
        /// <param name="customErrorData">自定义错误信息</param>
        /// <returns>反序列化的消息包</returns>
        Packet DeserializePacket(IPacketHeader packetHeader, Stream source, out object customErrorData);
    }
}