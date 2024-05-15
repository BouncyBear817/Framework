/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/2/19 16:6:14
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Net;
using System.Net.Sockets;

namespace Framework
{
    /// <summary>
    /// 网络频道接口
    /// </summary>
    public interface INetworkChannel
    {
        /// <summary>
        /// 网络频道名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 网络频道用的socket
        /// </summary>
        Socket Socket { get; }

        /// <summary>
        /// socket是否已连接
        /// </summary>
        bool Connected { get; }

        /// <summary>
        /// 网络服务类型
        /// </summary>
        ServiceType ServiceType { get; }

        /// <summary>
        /// 网络地址类型
        /// </summary>
        AddressFamily AddressFamily { get; }

        /// <summary>
        /// 要发送的消息包数量
        /// </summary>
        int SendPacketCount { get; }

        /// <summary>
        /// 已发送的消息包数量
        /// </summary>
        int SentPacketCount { get; }

        /// <summary>
        /// 要接收的消息包数量
        /// </summary>
        int ReceivePacketCount { get; }

        /// <summary>
        /// 已接收的消息包数量
        /// </summary>
        int ReceivedPacketCount { get; }

        /// <summary>
        /// 当收到消息包时是否重置心跳流逝时间
        /// </summary>
        bool ResetHeartBeatElapseSecondsWhenReceivePacket { get; set; }

        /// <summary>
        /// 丢失心跳的次数
        /// </summary>
        int MissHeartBeatCount { get; }

        /// <summary>
        /// 心跳间隔时长，以秒为单位
        /// </summary>
        float HeartBeatInterval { get; set; }

        /// <summary>
        /// 心跳等待时长，以秒为单位
        /// </summary>
        float HeartBeatElapseSeconds { get; }

        /// <summary>
        /// 注册消息包处理函数
        /// </summary>
        /// <param name="handler">消息包处理函数</param>
        void RegisterHandler(IPacketHandler handler);

        /// <summary>
        /// 注册消息包处理函数
        /// </summary>
        /// <param name="id">网络消息包协议编号</param>
        /// <param name="handler">消息包处理函数</param>
        public void RegisterHandler(int id, EventHandler<Packet> handler);

        /// <summary>
        /// 设置默认事件处理函数
        /// </summary>
        /// <param name="handler"></param>
        void SetDefaultHandler(EventHandler<Packet> handler);

        /// <summary>
        /// 连接远程主机
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <param name="port">IP端口</param>
        /// <param name="userData">自定义数据</param>
        void Connect(IPAddress ipAddress, int port, object userData = null);

        /// <summary>
        /// 关闭网络频道
        /// </summary>
        void Close();

        /// <summary>
        /// 向远程主机发送消息包
        /// </summary>
        /// <param name="packet">消息包</param>
        /// <typeparam name="T">消息包类型</typeparam>
        void Send<T>(T packet) where T : Packet;
    }
}