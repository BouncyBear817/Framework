/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/3/22 15:39:4
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using Framework;
using Framework.Runtime;
using UnityEngine;

/// <summary>
/// 网络连接组件
/// </summary>
[DisallowMultipleComponent]
[AddComponentMenu("Custom/NetConnector")]
public class NetConnectorComponent : FrameworkComponent
{
    private readonly Dictionary<string, INetworkChannel> mNetworkChannels =
        new Dictionary<string, INetworkChannel>();

    private NetworkChannelHelper mNetworkChannelHelper;

    [SerializeField] private bool mIsLittleEndian = false;

    [SerializeField] private DataTransferFormat mDataTransferFormat = DataTransferFormat.None;

    /// <summary>
    /// 创建网络频道
    /// </summary>
    /// <param name="name">网络频道名称</param>
    /// <returns>网络频道</returns>
    public INetworkChannel CreateTcpNetworkChannel(string name = "Default")
    {
        var networkChannel = mNetworkChannels.GetValueOrDefault(name);
        if (networkChannel == null)
        {
            mNetworkChannelHelper = ReferencePool.Acquire<NetworkChannelHelper>();
            mNetworkChannelHelper.IsLittleEndian = mIsLittleEndian;
            networkChannel = MainEntry.Network.CreateNetworkChannel(name, ServiceType.Tcp, mNetworkChannelHelper);
            mNetworkChannels.Add(name, networkChannel);
        }

        return networkChannel;
    }

    /// <summary>
    /// 连接远程主机
    /// </summary>
    /// <param name="ip">IP地址</param>
    /// <param name="port">IP端口</param>
    /// <param name="name">网络频道名称</param>
    /// <param name="userData">用户自定义数据</param>
    public void Connect(string ip, int port, string name = "Default", object userData = null)
    {
        var networkChannel = mNetworkChannels.GetValueOrDefault(name);
        if (networkChannel == null)
        {
            networkChannel = CreateTcpNetworkChannel(name);
            if (networkChannel == null)
            {
                Log.Error($"Connect failed, channel name ({name}) is null.");
                return;
            }
        }

        networkChannel.Connect(IPAddress.Parse(ip), port, userData);
    }

    /// <summary>
    /// 关闭网络频道
    /// </summary>
    /// <param name="name">网络频道名称</param>
    public void Close(string name)
    {
        var networkChannel = mNetworkChannels.GetValueOrDefault(name);
        if (networkChannel == null)
        {
            Log.Error($"Close failed, channel name ({name}) is null.");
            return;
        }

        networkChannel.Close();
    }

    /// <summary>
    /// 设置心跳间隔时长，以秒为单位
    /// </summary>
    /// <param name="name">网络频道名称</param>
    /// <param name="heartBeatInterval">心跳间隔时长，以秒为单位</param>
    public void SetHeartBeatInterval(string name, float heartBeatInterval)
    {
        var networkChannel = mNetworkChannels.GetValueOrDefault(name);
        if (networkChannel != null)
        {
            networkChannel.HeartBeatInterval = heartBeatInterval;
        }
    }

    /// <summary>
    /// 注册消息包处理函数
    /// </summary>
    /// <param name="name">网络频道名称</param>
    /// <param name="handler">网络消息包处理器</param>
    public void RegisterHandler(string name, IPacketHandler handler)
    {
        var networkChannel = mNetworkChannels.GetValueOrDefault(name);
        if (networkChannel == null)
        {
            Log.Error($"Close failed, channel name ({name}) is null.");
            return;
        }

        networkChannel.RegisterHandler(handler);
    }

    /// <summary>
    /// 注册消息包处理函数
    /// </summary>
    /// <param name="name">网络频道名称</param>
    /// <param name="id">网络消息包协议编号</param>
    /// <param name="handler">消息包处理函数</param>
    public void RegisterHandler(string name, int id, EventHandler<Packet> handler)
    {
        var networkChannel = mNetworkChannels.GetValueOrDefault(name);
        if (networkChannel == null)
        {
            Log.Error($"Close failed, channel name ({name}) is null.");
            return;
        }

        networkChannel.RegisterHandler(id, handler);
    }

    /// <summary>
    /// 向远程主机发送消息包
    /// </summary>
    /// <param name="name">网络频道名称</param>
    /// <param name="packet">消息包</param>
    public void Send(string name, Packet packet)
    {
        var networkChannel = mNetworkChannels.GetValueOrDefault(name);
        if (networkChannel == null)
        {
            Log.Error($"Send failed, channel name ({name}) is null.");
            return;
        }

        networkChannel.Send(packet);
    }

    /// <summary>
    /// 向远程主机发送消息包
    /// </summary>
    /// <param name="name">网络频道名称</param>
    /// <param name="messageId">消息编号</param>
    /// <param name="messageBody">消息内容</param>
    public void Send(string name, int messageId, byte[] messageBody)
    {
        var networkChannel = mNetworkChannels.GetValueOrDefault(name);
        if (networkChannel == null)
        {
            Log.Error($"Send failed, channel name ({name}) is null.");
            return;
        }

        var csPacket = ReferencePool.Acquire<CSPacketBase>();
        csPacket.MessageId = messageId;
        csPacket.MessageBody = messageBody;
        networkChannel.Send(csPacket);
    }

    /// <summary>
    /// 向远程主机发送消息包
    /// </summary>
    /// <param name="name">网络频道名称</param>
    /// <param name="messageId">消息编号</param>
    /// <param name="message">消息内容</param>
    public void Send(string name, int messageId, object message)
    {
        Send(name, messageId, Serialize(message));
    }

    private byte[] Serialize(object message)
    {
        switch (mDataTransferFormat)
        {
            case DataTransferFormat.Protobuf:
                return ProtobufUtil.Serialize(message);
            case DataTransferFormat.Json:
                return null;
        }

        return null;
    }
}