// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/2/20 11:32:53
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine.Events;

namespace Framework
{
    /// <summary>
    /// 网络管理器
    /// </summary>
    public sealed partial class NetworkManager : FrameworkModule, INetworkManager
    {
        private abstract class NetworkChannelBase : INetworkChannel, IDisposable
        {
            private const float DefaultHeartBeatInterval = 30f;

            private readonly string mName;
            protected readonly Queue<Packet> mSendPacketPool;
            protected readonly EventPool<Packet> mReceivePacketPool;
            protected readonly INetworkChannelHelper mNetworkChannelHelper;
            protected AddressFamily mAddressFamily;
            protected bool mResetHeartBeatElapseSecondsWhenReceivePacket;
            protected float mHeartBeatInterval;
            protected Socket mSocket;
            protected SendState mSendState;
            protected ReceiveState mReceiveState;
            protected HeartBeatState mHeartBeatState;
            protected int mSentPacketCount;
            protected int mReceivedPacketCount;
            protected bool mActive;
            private bool mDisposed;

            public Action<NetworkChannelBase, object> NetworkChannelConnected;
            public Action<NetworkChannelBase> NetworkChannelClosed;
            public Action<NetworkChannelBase, object> NetworkChannelCustomError;
            public Action<NetworkChannelBase, NetworkErrorCode, SocketError, string> NetworkChannelError;
            public Action<NetworkChannelBase, int> NetworkChannelMissHeartBeat;

            public NetworkChannelBase(string name, INetworkChannelHelper networkChannelHelper)
            {
                mName = name ?? string.Empty;
                mNetworkChannelHelper = networkChannelHelper;

                mSendPacketPool = new Queue<Packet>();
                mReceivePacketPool = new EventPool<Packet>(EventPoolMode.Default);
                mAddressFamily = AddressFamily.Unknown;
                mResetHeartBeatElapseSecondsWhenReceivePacket = false;
                mHeartBeatInterval = DefaultHeartBeatInterval;
                mSocket = null;
                mSendState = new SendState();
                mReceiveState = new ReceiveState();
                mHeartBeatState = new HeartBeatState();
                mSentPacketCount = 0;
                mReceivedPacketCount = 0;
                mActive = false;
                mDisposed = false;

                NetworkChannelConnected = null;
                NetworkChannelClosed = null;
                NetworkChannelCustomError = null;
                NetworkChannelError = null;
                NetworkChannelMissHeartBeat = null;
                
                networkChannelHelper.Initialize(this);
            }


            /// <summary>
            /// 网络频道名称
            /// </summary>
            public string Name => mName;

            /// <summary>
            /// 网络频道用的socket
            /// </summary>
            public Socket Socket => mSocket;

            /// <summary>
            /// socket是否已连接
            /// </summary>
            public bool Connected
            {
                get
                {
                    if (mSocket != null)
                    {
                        return mSocket.Connected;
                    }

                    return false;
                }
            }

            /// <summary>
            /// 网络服务类型
            /// </summary>
            public abstract ServiceType ServiceType { get; }

            /// <summary>
            /// 网络地址类型
            /// </summary>
            public AddressFamily AddressFamily => mAddressFamily;

            /// <summary>
            /// 要发送的消息包数量
            /// </summary>
            public int SendPacketCount => mSendPacketPool.Count;

            /// <summary>
            /// 已发送的消息包数量
            /// </summary>
            public int SentPacketCount => mSentPacketCount;

            /// <summary>
            /// 要接收的消息包数量
            /// </summary>
            public int ReceivePacketCount => mReceivePacketPool.EventCount;

            /// <summary>
            /// 已接收的消息包数量
            /// </summary>
            public int ReceivedPacketCount => mReceivedPacketCount;

            /// <summary>
            /// 当收到消息包时是否重置心跳流逝时间
            /// </summary>
            public bool ResetHeartBeatElapseSecondsWhenReceivePacket
            {
                get => mResetHeartBeatElapseSecondsWhenReceivePacket;
                set => mResetHeartBeatElapseSecondsWhenReceivePacket = value;
            }

            /// <summary>
            /// 丢失心跳的次数
            /// </summary>
            public int MissHeartBeatCount => mHeartBeatState.MissHeartBeatCount;

            /// <summary>
            /// 心跳间隔时长，以秒为单位
            /// </summary>
            public float HeartBeatInterval
            {
                get => mHeartBeatInterval;
                set => mHeartBeatInterval = value;
            }

            /// <summary>
            /// 心跳等待时长，以秒为单位
            /// </summary>
            public float HeartBeatElapseSeconds => mHeartBeatState.HeartBeatElapseSeconds;

            public virtual void Update(float elapseSeconds, float realElapseSeconds)
            {
                if (mSocket == null || !mActive)
                {
                    return;
                }

                ProcessSend();
                ProcessReceive();
                if (mSocket == null || !mActive)
                {
                    return;
                }

                mReceivePacketPool.Update(elapseSeconds, realElapseSeconds);

                if (mHeartBeatInterval > 0f)
                {
                    var sendHeartBeat = false;
                    var missHeartBeatCount = 0;
                    lock (mHeartBeatState)
                    {
                        if (mSocket == null || !mActive)
                        {
                            return;
                        }

                        mHeartBeatState.HeartBeatElapseSeconds += realElapseSeconds;
                        if (mHeartBeatState.HeartBeatElapseSeconds >= mHeartBeatInterval)
                        {
                            sendHeartBeat = true;
                            missHeartBeatCount = mHeartBeatState.MissHeartBeatCount;
                            mHeartBeatState.HeartBeatElapseSeconds = 0f;
                            mHeartBeatState.MissHeartBeatCount++;
                        }
                    }

                    if (sendHeartBeat && mNetworkChannelHelper.SendHeartBeat())
                    {
                        if (missHeartBeatCount > 0)
                        {
                            NetworkChannelMissHeartBeat?.Invoke(this, missHeartBeatCount);
                        }
                    }
                }
            }

            public virtual void Shutdown()
            {
                Close();
                mReceivePacketPool.Shutdown();
                mNetworkChannelHelper.Shutdown();
            }

            /// <summary>
            /// 注册消息包处理函数
            /// </summary>
            /// <param name="handler">消息包处理函数</param>
            public void RegisterHandler(IPacketHandler handler)
            {
                if (handler == null)
                {
                    throw new Exception("Packet handler is invalid.");
                }

                mReceivePacketPool.Subscribe(handler.Id, handler.Handle);
            }

            /// <summary>
            /// 设置默认事件处理函数
            /// </summary>
            /// <param name="handler"></param>
            public void SetDefaultHandler(EventHandler<Packet> handler)
            {
                mReceivePacketPool.SetDefaultHandler(handler);
            }

            /// <summary>
            /// 连接远程主机
            /// </summary>
            /// <param name="ipAddress">IP地址</param>
            /// <param name="port">IP端口</param>
            public virtual void Connect(IPAddress ipAddress, int port, object userData = null)
            {
                if (mSocket != null)
                {
                    Close();
                    mSocket = null;
                }

                switch (ipAddress.AddressFamily)
                {
                    case System.Net.Sockets.AddressFamily.InterNetwork:
                        mAddressFamily = AddressFamily.IPv4;
                        break;
                    case System.Net.Sockets.AddressFamily.InterNetworkV6:
                        mAddressFamily = AddressFamily.IPv6;
                        break;
                    default:
                        var errorMessage = $"Not support address family ({ipAddress.AddressFamily}).";
                        if (NetworkChannelError != null)
                        {
                            NetworkChannelError(this, NetworkErrorCode.AddressFamilyError, SocketError.Success,
                                errorMessage);
                            return;
                        }

                        throw new Exception(errorMessage);
                }

                mSendState.Reset();
                mReceiveState.PrepareForPacketHeader(mNetworkChannelHelper.PacketHeaderLength);
            }

            /// <summary>
            /// 关闭网络频道
            /// </summary>
            public void Close()
            {
                lock (this)
                {
                    if (mSocket == null)
                    {
                        return;
                    }

                    mActive = false;

                    try
                    {
                        mSocket.Shutdown(SocketShutdown.Both);
                    }
                    catch
                    {
                        // ignored
                    }
                    finally
                    {
                        mSocket.Close();
                        mSocket = null;
                        NetworkChannelClosed?.Invoke(this);
                    }

                    mSentPacketCount = 0;
                    mReceivedPacketCount = 0;

                    lock (mSendPacketPool)
                    {
                        mSendPacketPool.Clear();
                    }

                    mReceivePacketPool.Clear();

                    lock (mHeartBeatState)
                    {
                        mHeartBeatState.Reset(true);
                    }
                }
            }

            /// <summary>
            /// 向远程主机发送消息包
            /// </summary>
            /// <param name="packet">消息包</param>
            /// <typeparam name="T">消息包类型</typeparam>
            public void Send<T>(T packet) where T : Packet
            {
                if (mSocket == null)
                {
                    var errorMessage = "You must connect first.";
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.SendError, SocketError.Success, errorMessage);
                        return;
                    }

                    throw new Exception(errorMessage);
                }

                if (!mActive)
                {
                    var errorMessage = "Socket is not active.";
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.SendError, SocketError.Success, errorMessage);
                        return;
                    }

                    throw new Exception(errorMessage);
                }

                if (packet == null)
                {
                    var errorMessage = "Packet is invalid.";
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.SendError, SocketError.Success, errorMessage);
                        return;
                    }

                    throw new Exception(errorMessage);
                }

                lock (mSendPacketPool)
                {
                    mSendPacketPool.Enqueue(packet);
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            private void Dispose(bool disposing)
            {
                if (mDisposed)
                {
                    return;
                }

                if (disposing)
                {
                    Close();
                    mSendState.Dispose();
                    mReceiveState.Dispose();
                }

                mDisposed = true;
            }

            protected virtual bool ProcessSend()
            {
                if (mSendState.MemoryStream.Length > 0 || mSendPacketPool.Count <= 0)
                {
                    return false;
                }

                while (mSendPacketPool.Count > 0)
                {
                    Packet packet = null;
                    lock (mSendPacketPool)
                    {
                        packet = mSendPacketPool.Dequeue();
                    }

                    bool serializeResult = false;
                    try
                    {
                        serializeResult = mNetworkChannelHelper.Serialize(packet, mSendState.MemoryStream);
                    }
                    catch (Exception e)
                    {
                        mActive = false;
                        if (NetworkChannelError != null)
                        {
                            var socketException = e as SocketException;
                            NetworkChannelError(this, NetworkErrorCode.SerializeError,
                                socketException != null ? socketException.SocketErrorCode : SocketError.Success,
                                e.ToString());
                            return false;
                        }

                        throw;
                    }

                    if (!serializeResult)
                    {
                        var errorMessage = "Serialized packet is failed.";
                        if (NetworkChannelError != null)
                        {
                            NetworkChannelError(this, NetworkErrorCode.SerializeError, SocketError.Success,
                                errorMessage);
                            return false;
                        }

                        throw new Exception(errorMessage);
                    }
                }

                mSendState.MemoryStream.Position = 0L;
                return true;
            }

            protected virtual void ProcessReceive()
            {
            }

            protected virtual bool ProcessPacketHeader()
            {
                try
                {
                    var packetHeader =
                        mNetworkChannelHelper.DeserializePacketHeader(mReceiveState.MemoryStream,
                            out var customErrorData);
                    if (customErrorData != null)
                    {
                        NetworkChannelCustomError(this, customErrorData);
                    }

                    if (packetHeader == null)
                    {
                        var errorMessage = "Packet header is invalid.";
                        if (NetworkChannelError != null)
                        {
                            NetworkChannelError(this, NetworkErrorCode.DeserializePacketHeaderError,
                                SocketError.Success, errorMessage);
                            return false;
                        }

                        throw new Exception(errorMessage);
                    }

                    mReceiveState.PrepareForPacket(packetHeader);
                    if (packetHeader.PacketLength <= 0)
                    {
                        var processSuccess = ProcessPacket();
                        mReceivedPacketCount++;
                        return processSuccess;
                    }
                }
                catch (Exception e)
                {
                    mActive = false;
                    if (NetworkChannelError != null)
                    {
                        var socketException = e as SocketException;
                        NetworkChannelError(this, NetworkErrorCode.DeserializePacketHeaderError,
                            socketException != null ? socketException.SocketErrorCode : SocketError.Success,
                            e.ToString());
                        return false;
                    }

                    throw;
                }

                return true;
            }

            protected virtual bool ProcessPacket()
            {
                lock (mHeartBeatState)
                {
                    mHeartBeatState.Reset(mResetHeartBeatElapseSecondsWhenReceivePacket);
                }

                try
                {
                    var packet = mNetworkChannelHelper.DeserializePacket(mReceiveState.PacketHeader,
                        mReceiveState.MemoryStream, out var customErrorData);
                    if (customErrorData != null)
                    {
                        NetworkChannelCustomError(this, customErrorData);
                    }

                    if (packet != null)
                    {
                        mReceivePacketPool.Fire(this, packet);
                    }

                    mReceiveState.PrepareForPacketHeader(mNetworkChannelHelper.PacketHeaderLength);
                }
                catch (Exception e)
                {
                    mActive = false;
                    if (NetworkChannelError != null)
                    {
                        var socketException = e as SocketException;
                        NetworkChannelError(this, NetworkErrorCode.DeserializePacketError,
                            socketException != null ? socketException.SocketErrorCode : SocketError.Success,
                            e.ToString());
                        return false;
                    }

                    throw;
                }

                return true;
            }
        }
    }
}