// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/2/20 11:30:57
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Framework
{
    /// <summary>
    /// 网络管理器
    /// </summary>
    public sealed partial class NetworkManager : FrameworkModule, INetworkManager
    {
        private readonly Dictionary<string, NetworkChannelBase> mNetworkChannels;

        private EventHandler<NetworkConnectedEventArgs> mNetworkConnectedEventHandler;
        private EventHandler<NetworkClosedEventArgs> mNetworkClosedEventHandler;
        private EventHandler<NetworkCustomErrorEventArgs> mNetworkCustomErrorEventHandler;
        private EventHandler<NetworkErrorEventArgs> mNetworkErrorEventHandler;
        private EventHandler<NetworkMissHeartBeatEventArgs> mNetworkMissHeartBeatEventHandler;

        public NetworkManager()
        {
            mNetworkChannels = new Dictionary<string, NetworkChannelBase>(StringComparer.Ordinal);
            mNetworkConnectedEventHandler = null;
            mNetworkClosedEventHandler = null;
            mNetworkCustomErrorEventHandler = null;
            mNetworkErrorEventHandler = null;
            mNetworkMissHeartBeatEventHandler = null;
        }

        /// <summary>
        /// 网络频道数量
        /// </summary>
        public int NetworkChannelCount => mNetworkChannels.Count;

        /// <summary>
        /// 网络连接成功事件
        /// </summary>
        public event EventHandler<NetworkConnectedEventArgs> NetworkConnected
        {
            add => mNetworkConnectedEventHandler += value;
            remove => mNetworkConnectedEventHandler -= value;
        }

        /// <summary>
        /// 网络连接关闭事件
        /// </summary>
        public event EventHandler<NetworkClosedEventArgs> NetworkClosed
        {
            add => mNetworkClosedEventHandler += value;
            remove => mNetworkClosedEventHandler -= value;
        }

        /// <summary>
        /// 网络自定义错误事件
        /// </summary>
        public event EventHandler<NetworkCustomErrorEventArgs> NetworkCustomError
        {
            add => mNetworkCustomErrorEventHandler += value;
            remove => mNetworkCustomErrorEventHandler -= value;
        }

        /// <summary>
        /// 网络错误事件
        /// </summary>
        public event EventHandler<NetworkErrorEventArgs> NetworkError
        {
            add => mNetworkErrorEventHandler += value;
            remove => mNetworkErrorEventHandler -= value;
        }

        /// <summary>
        /// 网络丢失心跳包事件
        /// </summary>
        public event EventHandler<NetworkMissHeartBeatEventArgs> NetworkMissHeartBeat
        {
            add => mNetworkMissHeartBeatEventHandler += value;
            remove => mNetworkMissHeartBeatEventHandler -= value;
        }

        /// <summary>
        /// 框架模块轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间</param>
        /// <param name="realElapseSeconds">真实流逝时间</param>
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var (_, networkChannel) in mNetworkChannels)
            {
                networkChannel.Update(elapseSeconds, realElapseSeconds);
            }
        }

        /// <summary>
        /// 关闭并清理框架模块
        /// </summary>
        public override void Shutdown()
        {
            foreach (var (_, networkChannel) in mNetworkChannels)
            {
                networkChannel.NetworkChannelConnected -= OnNetworkChannelConnected;
                networkChannel.NetworkChannelClosed -= OnNetworkChannelClosed;
                networkChannel.NetworkChannelCustomError -= OnNetworkChannelCustomError;
                networkChannel.NetworkChannelError -= OnNetworkChannelError;
                networkChannel.NetworkChannelMissHeartBeat -= OnNetworkChannelMissHeartBeat;
                networkChannel.Shutdown();
            }

            mNetworkChannels.Clear();
        }

        /// <summary>
        /// 是否存在网络频道
        /// </summary>
        /// <param name="name">网络频道名称</param>
        /// <returns>是否存在网络频道</returns>
        public bool HasNetworkChannel(string name)
        {
            return mNetworkChannels.ContainsKey(name ?? string.Empty);
        }

        /// <summary>
        /// 获取网络频道
        /// </summary>
        /// <param name="name">网络频道名称</param>
        /// <returns>网络频道</returns>
        public INetworkChannel GetNetworkChannel(string name)
        {
            return mNetworkChannels.GetValueOrDefault(name ?? string.Empty);
        }

        /// <summary>
        /// 获取所有网络频道
        /// </summary>
        /// <returns>所有网络频道</returns>
        public INetworkChannel[] GetAllNetworkChannel()
        {
            var index = 0;
            var results = new INetworkChannel[mNetworkChannels.Count];
            foreach (var (_, networkChannel) in mNetworkChannels)
            {
                results[index++] = networkChannel;
            }

            return results;
        }

        /// <summary>
        /// 获取所有网络频道
        /// </summary>
        /// <param name="results">所有网络频道</param>
        public void GetAllNetworkChannel(List<INetworkChannel> results)
        {
            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            foreach (var (_, networkChannel) in mNetworkChannels)
            {
                results.Add(networkChannel);
            }
        }

        /// <summary>
        /// 创建网络频道
        /// </summary>
        /// <param name="name">网络频道名称</param>
        /// <param name="serviceType">网络服务类型</param>
        /// <param name="networkChannelHelper">网络频道辅助器</param>
        /// <returns>网络频道</returns>
        public INetworkChannel CreateNetworkChannel(string name, ServiceType serviceType,
            INetworkChannelHelper networkChannelHelper)
        {
            if (networkChannelHelper == null)
            {
                throw new Exception("Network channel helper is invalid.");
            }

            if (networkChannelHelper.PacketHeaderLength < 0)
            {
                throw new Exception("Packet header length is invalid.");
            }

            if (HasNetworkChannel(name))
            {
                throw new Exception($"Already exist network channel ({name ?? string.Empty}).");
            }

            NetworkChannelBase networkChannel = null;
            switch (serviceType)
            {
                case ServiceType.Tcp:
                    networkChannel = new TcpNetworkChannel(name, networkChannelHelper);
                    break;
                case ServiceType.TcpWithSyncReceive:
                    networkChannel = new TcpWithSyncReceiveNetworkChannel(name, networkChannelHelper);
                    break;
                default:
                    throw new Exception($"Not supported service type ({serviceType}).");
            }

            networkChannel.NetworkChannelConnected += OnNetworkChannelConnected;
            networkChannel.NetworkChannelClosed += OnNetworkChannelClosed;
            networkChannel.NetworkChannelCustomError += OnNetworkChannelCustomError;
            networkChannel.NetworkChannelError += OnNetworkChannelError;
            networkChannel.NetworkChannelMissHeartBeat += OnNetworkChannelMissHeartBeat;

            mNetworkChannels.Add(name, networkChannel);

            return networkChannel;
        }

        /// <summary>
        /// 销毁网络频道
        /// </summary>
        /// <param name="name">网络频道名称</param>
        /// <returns>是否销毁网络频道成功</returns>
        public bool DestroyNetworkChannel(string name)
        {
            if (mNetworkChannels.TryGetValue(name ?? string.Empty, out var networkChannel))
            {
                networkChannel.NetworkChannelConnected -= OnNetworkChannelConnected;
                networkChannel.NetworkChannelClosed -= OnNetworkChannelClosed;
                networkChannel.NetworkChannelCustomError -= OnNetworkChannelCustomError;
                networkChannel.NetworkChannelError -= OnNetworkChannelError;
                networkChannel.NetworkChannelMissHeartBeat -= OnNetworkChannelMissHeartBeat;
                networkChannel.Shutdown();
                return mNetworkChannels.Remove(name ?? string.Empty);
            }

            return false;
        }

        private void OnNetworkChannelConnected(NetworkChannelBase networkChannel, object userData)
        {
            if (mNetworkConnectedEventHandler != null)
            {
                var eventArgs = NetworkConnectedEventArgs.Create(networkChannel, userData);
                mNetworkConnectedEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }

        private void OnNetworkChannelClosed(NetworkChannelBase networkChannel)
        {
            if (mNetworkClosedEventHandler != null)
            {
                var eventArgs = NetworkClosedEventArgs.Create(networkChannel);
                mNetworkClosedEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }

        private void OnNetworkChannelCustomError(NetworkChannelBase networkChannel, object customErrorData)
        {
            if (mNetworkCustomErrorEventHandler != null)
            {
                var eventArgs = NetworkCustomErrorEventArgs.Create(networkChannel, customErrorData);
                mNetworkCustomErrorEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }

        private void OnNetworkChannelError(NetworkChannelBase networkChannel, NetworkErrorCode errorCode,
            SocketError socketErrorCode, string errorMessage)
        {
            if (mNetworkErrorEventHandler != null)
            {
                var eventArgs = NetworkErrorEventArgs.Create(networkChannel, errorCode, socketErrorCode, errorMessage);
                mNetworkErrorEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }

        private void OnNetworkChannelMissHeartBeat(NetworkChannelBase networkChannel, int missCount)
        {
            if (mNetworkMissHeartBeatEventHandler != null)
            {
                var eventArgs = NetworkMissHeartBeatEventArgs.Create(networkChannel, missCount);
                mNetworkMissHeartBeatEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }
    }
}