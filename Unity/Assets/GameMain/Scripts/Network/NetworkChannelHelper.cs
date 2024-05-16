/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/2/23 10:51:31
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Framework;

public class NetworkChannelHelper : INetworkChannelHelper, IReference
{
    private const int DefaultCachedSize = 8 * 1024;
    private const int DefaultBufferSize = 64 * 1024;

    private readonly Dictionary<int, Type> mSCPacketTypes;
    private readonly MemoryStream mCachedStream;
    private INetworkChannel mNetworkChannel;

    private byte[] mCachedByte;

    public NetworkChannelHelper()
    {
        mSCPacketTypes = new Dictionary<int, Type>();
        mCachedStream = new MemoryStream(DefaultCachedSize);
        mNetworkChannel = null;
    }

    /// <summary>
    /// 判定是否为小端字节序
    /// </summary>
    public bool IsLittleEndian { get; set; }

    /// <summary>
    /// 消息包头长度
    /// </summary>
    public int PacketHeaderLength => sizeof(int) + sizeof(int);

    /// <summary>
    /// 初始化网络频道辅助器
    /// </summary>
    /// <param name="networkChannel">网络频道辅助器</param>
    public void Initialize(INetworkChannel networkChannel)
    {
        mCachedByte = new byte[PacketHeaderLength];
        mNetworkChannel = networkChannel;

        var packetBaseType = typeof(SCPacketBase);
        var packetHandlerBaseType = typeof(PacketHandlerBase);
        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes();

        for (var i = 0; i < types.Length; i++)
        {
            if (!types[i].IsClass || types[i].IsAbstract)
            {
                continue;
            }

            if (types[i].BaseType == packetBaseType)
            {
                var packetBase = Activator.CreateInstance(types[i]) as PacketBase;
                if (packetBase != null)
                {
                    var packetType = GetSCPacketType(packetBase.Id);
                    if (packetType != null)
                    {
                        Log.Warning(
                            $"Already exist packet type ({packetBase.Id.ToString()}), check ({packetType.Name}) or ({packetBase.GetType().Name}).");
                        return;
                    }

                    mSCPacketTypes.Add(packetBase.Id, types[i]);
                }
            }
            else if (types[i].BaseType == packetHandlerBaseType)
            {
                var packetHandler = Activator.CreateInstance(types[i]) as IPacketHandler;
                mNetworkChannel.RegisterHandler(packetHandler);
            }
        }

        MainEntry.Event.Subscribe(Runtime.NetworkConnectedEventArgs.EventId, OnNetworkConnected);
        MainEntry.Event.Subscribe(Runtime.NetworkClosedEventArgs.EventId, OnNetworkClosed);
        MainEntry.Event.Subscribe(Runtime.NetworkCustomErrorEventArgs.EventId, OnNetworkCustomError);
        MainEntry.Event.Subscribe(Runtime.NetworkErrorEventArgs.EventId, OnNetworkError);
        MainEntry.Event.Subscribe(Runtime.NetworkMissHeartBeatEventArgs.EventId, OnNetworkMissHeartBeat);
    }

    /// <summary>
    /// 关闭并清理网络频道辅助器
    /// </summary>
    public void Shutdown()
    {
        MainEntry.Event.UnSubscribe(Runtime.NetworkConnectedEventArgs.EventId, OnNetworkConnected);
        MainEntry.Event.UnSubscribe(Runtime.NetworkClosedEventArgs.EventId, OnNetworkClosed);
        MainEntry.Event.UnSubscribe(Runtime.NetworkCustomErrorEventArgs.EventId, OnNetworkCustomError);
        MainEntry.Event.UnSubscribe(Runtime.NetworkErrorEventArgs.EventId, OnNetworkError);
        MainEntry.Event.UnSubscribe(Runtime.NetworkMissHeartBeatEventArgs.EventId, OnNetworkMissHeartBeat);

        mSCPacketTypes.Clear();
        mCachedStream.Close();
        mNetworkChannel = null;
    }

    /// <summary>
    /// 准备进行连接
    /// </summary>
    public void PrepareForConnecting()
    {
        mNetworkChannel.Socket.ReceiveBufferSize = DefaultBufferSize;
        mNetworkChannel.Socket.SendBufferSize = DefaultBufferSize;
    }

    /// <summary>
    /// 发送心跳包
    /// </summary>
    public bool SendHeartBeat()
    {
        mNetworkChannel.Send(ReferencePool.Acquire<CSHeartBeat>());
        return true;
    }

    /// <summary>
    /// 序列化消息包
    /// </summary>
    /// <param name="packet">消息包</param>
    /// <param name="destination">要序列化的目标流</param>
    /// <typeparam name="T">消息包类型</typeparam>
    /// <returns></returns>
    public bool Serialize<T>(T packet, Stream destination) where T : Packet
    {
        var packetImp = packet as PacketBase;
        if (packetImp == null)
        {
            Log.Warning("Packet is invalid.");
            return false;
        }

        if (packetImp.PacketType != PacketType.ClientToServer)
        {
            Log.Warning("Send Packet invalid.");
            return false;
        }

        mCachedStream.SetLength(0);
        mCachedStream.Position = 0L;
        Array.Clear(mCachedByte, 0, mCachedByte.Length);
        mCachedByte.WriteTo(0, packetImp.MessageBody == null ? 0 : packetImp.MessageBody.Length, IsLittleEndian);
        mCachedByte.WriteTo(sizeof(int), packetImp.Id, IsLittleEndian);

        mCachedStream.Write(mCachedByte, 0, mCachedByte.Length);
        if (packetImp.MessageBody != null)
        {
            mCachedStream.Write(packetImp.MessageBody, 0, packetImp.MessageBody.Length);
        }

        mCachedStream.WriteTo(destination);

        Log.Info($"Network channel ({mNetworkChannel.Name}) send packet, id is {packetImp.Id}.");

        ReferencePool.Release(packet);
        return true;
    }

    /// <summary>
    /// 反序列化消息包头
    /// </summary>
    /// <param name="source">要反序列化的来源流</param>
    /// <param name="customErrorData">自定义错误信息</param>
    /// <returns>反序列化的消息包头</returns>
    public IPacketHeader DeserializePacketHeader(Stream source, out object customErrorData)
    {
        customErrorData = null;

        var packetHeader = ReferencePool.Acquire<PacketHeader>();
        if (source is MemoryStream memoryStream)
        {
            var bytes = memoryStream.GetBuffer();
            packetHeader.PacketLength = bytes.ReadTo(0, IsLittleEndian);
            packetHeader.Id = bytes.ReadTo(sizeof(int), IsLittleEndian);
            Log.Info(
                $"Network channel ({mNetworkChannel.Name}) deserialize packet header, id is {packetHeader.Id}, packet length is {packetHeader.PacketLength}.");
            return packetHeader;
        }

        return null;
    }

    /// <summary>
    /// 反序列化消息包
    /// </summary>
    /// <param name="packetHeader">消息包头</param>
    /// <param name="source">要反序列化的来源流</param>
    /// <param name="customErrorData">自定义错误信息</param>
    /// <returns>反序列化的消息包</returns>
    public Packet DeserializePacket(IPacketHeader packetHeader, Stream source, out object customErrorData)
    {
        customErrorData = null;

        var scPacketHeader = packetHeader as PacketHeader;
        if (scPacketHeader == null)
        {
            Log.Warning("Packet header is invalid.");
            return null;
        }

        PacketBase packet = null;
        if (scPacketHeader.IsValid)
        {
            var packetType = GetSCPacketType(scPacketHeader.Id);
            if (packetType != null)
            {
                packet = ReferencePool.Acquire(packetType) as PacketBase;
                if (source is MemoryStream memoryStream)
                {
                    if (packet != null)
                    {
                        packet.MessageBody = memoryStream.ToArray();
                        Log.Info(
                            $"Network channel ({mNetworkChannel.Name}) deserialize packet, packet length is {packet.MessageBody.Length}.");
                    }
                }
                else
                {
                    Log.Warning(
                        $"Network channel ({mNetworkChannel.Name}) deserialize packet for packet id ({scPacketHeader.Id}), source is null.");
                }
            }
            else
            {
                packet = ReferencePool.Acquire<SCPacketBase>();
                if (source is MemoryStream memoryStream)
                {
                    if (packet != null)
                    {
                        packet.MessageId = scPacketHeader.Id;
                        packet.MessageBody = memoryStream.ToArray();
                        Log.Info(
                            $"Network channel ({mNetworkChannel.Name}) deserialize packet, packet length is {packet.MessageBody.Length}.");
                    }
                }
                else
                {
                    Log.Warning(
                        $"Network channel ({mNetworkChannel.Name}) deserialize packet for packet id ({scPacketHeader.Id}), source is null.");
                }
            }
        }
        else
        {
            Log.Warning("Packet header is invalid.");
        }

        ReferencePool.Release(scPacketHeader);
        return packet;
    }

    private Type GetSCPacketType(int packetBaseId)
    {
        return mSCPacketTypes.GetValueOrDefault(packetBaseId);
    }

    private void OnNetworkConnected(object sender, BaseEventArgs e)
    {
        var eventArgs = e as Runtime.NetworkConnectedEventArgs;
        if (eventArgs == null || eventArgs.NetworkChannel != mNetworkChannel)
        {
            return;
        }

        Log.Info(
            $"Network channel ({eventArgs.NetworkChannel.Name}) connected, local address ({eventArgs.NetworkChannel.Socket.LocalEndPoint}), remote address ({eventArgs.NetworkChannel.Socket.RemoteEndPoint}).");
    }

    private void OnNetworkClosed(object sender, BaseEventArgs e)
    {
        var eventArgs = e as Runtime.NetworkClosedEventArgs;
        if (eventArgs == null || eventArgs.NetworkChannel != mNetworkChannel)
        {
            return;
        }

        Log.Info($"Network channel ({eventArgs.NetworkChannel.Name}) closed.");
    }

    private void OnNetworkCustomError(object sender, BaseEventArgs e)
    {
        var eventArgs = e as Runtime.NetworkCustomErrorEventArgs;
        if (eventArgs == null || eventArgs.NetworkChannel != mNetworkChannel)
        {
            return;
        }
    }

    private void OnNetworkError(object sender, BaseEventArgs e)
    {
        var eventArgs = e as Runtime.NetworkErrorEventArgs;
        if (eventArgs == null || eventArgs.NetworkChannel != mNetworkChannel)
        {
            return;
        }

        Log.Info(
            $"Network channel ({eventArgs.NetworkChannel.Name}) error, error code is ({eventArgs.ErrorCode}), error message is ({eventArgs.ErrorMessage}).");

        eventArgs.NetworkChannel.Close();
    }

    private void OnNetworkMissHeartBeat(object sender, BaseEventArgs e)
    {
        var eventArgs = e as Runtime.NetworkMissHeartBeatEventArgs;
        if (eventArgs == null || eventArgs.NetworkChannel != mNetworkChannel)
        {
            return;
        }

        Log.Info(
            $"Network channel ({eventArgs.NetworkChannel.Name}) miss heart beat ({eventArgs.MissCount}) times.");

        if (eventArgs.MissCount < 2)
        {
            return;
        }

        eventArgs.NetworkChannel.Close();
    }

    public void Clear()
    {
    }
}