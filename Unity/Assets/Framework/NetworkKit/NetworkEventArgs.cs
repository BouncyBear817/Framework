/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/2/19 17:7:35
 * Description:
 * Modify Record:
 *************************************************************/

using System.Net.Sockets;

namespace Framework
{
    /// <summary>
    /// 网络连接成功事件
    /// </summary>
    public sealed class NetworkConnectedEventArgs : FrameworkEventArgs
    {
        public NetworkConnectedEventArgs()
        {
            NetworkChannel = null;
            UserData = null;
        }

        /// <summary>
        /// 网络频道
        /// </summary>
        public INetworkChannel NetworkChannel { get; private set; }

        /// <summary>
        /// 自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建网络连接成功事件
        /// </summary>
        /// <param name="networkChannel">网络频道</param>
        /// <param name="userData">自定义数据</param>
        /// <returns>网络连接成功事件</returns>
        public static NetworkConnectedEventArgs Create(INetworkChannel networkChannel, object userData)
        {
            var eventArgs = ReferencePool.Acquire<NetworkConnectedEventArgs>();
            eventArgs.NetworkChannel = networkChannel;
            eventArgs.UserData = userData;
            return eventArgs;
        }

        /// <summary>
        /// 清理网络连接成功事件
        /// </summary>
        public override void Clear()
        {
            NetworkChannel = null;
            UserData = null;
        }
    }

    /// <summary>
    /// 网络连接失败事件
    /// </summary>
    public sealed class NetworkClosedEventArgs : FrameworkEventArgs
    {
        public NetworkClosedEventArgs()
        {
            NetworkChannel = null;
        }

        /// <summary>
        /// 网络频道
        /// </summary>
        public INetworkChannel NetworkChannel { get; private set; }

        /// <summary>
        /// 创建网络连接失败事件
        /// </summary>
        /// <param name="networkChannel">网络频道</param>
        /// <returns>网络连接失败事件</returns>
        public static NetworkClosedEventArgs Create(INetworkChannel networkChannel)
        {
            var eventArgs = ReferencePool.Acquire<NetworkClosedEventArgs>();
            eventArgs.NetworkChannel = networkChannel;
            return eventArgs;
        }

        /// <summary>
        /// 清理网络连接失败事件
        /// </summary>
        public override void Clear()
        {
            NetworkChannel = null;
        }
    }

    /// <summary>
    /// 自定义网络错误事件
    /// </summary>
    public sealed class NetworkCustomErrorEventArgs : FrameworkEventArgs
    {
        public NetworkCustomErrorEventArgs()
        {
            NetworkChannel = null;
            CustomErrorData = null;
        }

        /// <summary>
        /// 网络频道
        /// </summary>
        public INetworkChannel NetworkChannel { get; private set; }

        /// <summary>
        /// 自定义错误数据
        /// </summary>
        public object CustomErrorData { get; private set; }

        /// <summary>
        /// 创建自定义网络错误事件
        /// </summary>
        /// <param name="networkChannel">网络频道</param>
        /// <param name="customErrorData">自定义错误数据</param>
        /// <returns>自定义网络错误事件</returns>
        public static NetworkCustomErrorEventArgs Create(INetworkChannel networkChannel, object customErrorData)
        {
            var eventArgs = ReferencePool.Acquire<NetworkCustomErrorEventArgs>();
            eventArgs.NetworkChannel = networkChannel;
            eventArgs.CustomErrorData = customErrorData;
            return eventArgs;
        }

        /// <summary>
        /// 清理自定义网络错误事件
        /// </summary>
        public override void Clear()
        {
            NetworkChannel = null;
            CustomErrorData = null;
        }
    }

    /// <summary>
    /// 网络错误事件
    /// </summary>
    public sealed class NetworkErrorEventArgs : FrameworkEventArgs
    {
        public NetworkErrorEventArgs()
        {
            NetworkChannel = null;
            ErrorCode = NetworkErrorCode.Unknown;
            SocketErrorCode = SocketError.Success;
            ErrorMessage = null;
        }

        /// <summary>
        /// 网络频道
        /// </summary>
        public INetworkChannel NetworkChannel { get; private set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public NetworkErrorCode ErrorCode { get; private set; }

        /// <summary>
        /// socket错误码
        /// </summary>
        public SocketError SocketErrorCode { get; private set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// 创建网络错误事件
        /// </summary>
        /// <param name="networkChannel">网络频道</param>
        /// <param name="errorCode">错误码</param>
        /// <param name="socketErrorCode">socket错误码</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>网络错误事件</returns>
        public static NetworkErrorEventArgs Create(INetworkChannel networkChannel, NetworkErrorCode errorCode,
            SocketError socketErrorCode, string errorMessage)
        {
            var eventArgs = ReferencePool.Acquire<NetworkErrorEventArgs>();
            eventArgs.NetworkChannel = networkChannel;
            eventArgs.ErrorCode = errorCode;
            eventArgs.SocketErrorCode = socketErrorCode;
            eventArgs.ErrorMessage = errorMessage;
            return eventArgs;
        }

        /// <summary>
        /// 清理网络错误事件
        /// </summary>
        public override void Clear()
        {
            NetworkChannel = null;
            ErrorCode = NetworkErrorCode.Unknown;
            SocketErrorCode = SocketError.Success;
            ErrorMessage = null;
        }
    }

    /// <summary>
    /// 网络丢失心跳包事件
    /// </summary>
    public sealed class NetworkMissHeartBeatEventArgs : FrameworkEventArgs
    {
        public NetworkMissHeartBeatEventArgs()
        {
            NetworkChannel = null;
            MissCount = 0;
        }

        /// <summary>
        /// 网络频道
        /// </summary>
        public INetworkChannel NetworkChannel { get; private set; }

        /// <summary>
        /// 心跳包丢失次数
        /// </summary>
        public int MissCount { get; private set; }

        /// <summary>
        /// 创建网络丢失心跳包事件
        /// </summary>
        /// <param name="networkChannel">网络频道</param>
        /// <param name="missCount">心跳包丢失次数</param>
        /// <returns>网络丢失心跳包事件</returns>
        public static NetworkMissHeartBeatEventArgs Create(INetworkChannel networkChannel, int missCount)
        {
            var eventArgs = ReferencePool.Acquire<NetworkMissHeartBeatEventArgs>();
            eventArgs.NetworkChannel = networkChannel;
            eventArgs.MissCount = missCount;
            return eventArgs;
        }

        /// <summary>
        /// 清理网络丢失心跳包事件
        /// </summary>
        public override void Clear()
        {
            NetworkChannel = null;
            MissCount = 0;
        }
    }
}