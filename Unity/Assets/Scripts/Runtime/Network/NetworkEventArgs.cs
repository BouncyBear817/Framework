/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/2/21 14:29:35
 * Description:
 * Modify Record:
 *************************************************************/

using System.Net.Sockets;
using Framework;

namespace Runtime
{
    /// <summary>
    /// 网络连接成功事件
    /// </summary>
    public sealed class NetworkConnectedEventArgs : BaseEventArgs
    {
        private static readonly int sEventId = typeof(NetworkConnectedEventArgs).GetHashCode();

        public NetworkConnectedEventArgs()
        {
            NetworkChannel = null;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => sEventId;

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
        /// <param name="e">内部事件</param>
        /// <returns>网络连接成功事件</returns>
        public static NetworkConnectedEventArgs Create(Framework.NetworkConnectedEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<NetworkConnectedEventArgs>();
            eventArgs.NetworkChannel = e.NetworkChannel;
            eventArgs.UserData = e.UserData;
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
    public sealed class NetworkClosedEventArgs : BaseEventArgs
    {
        private static readonly int sEventId = typeof(NetworkConnectedEventArgs).GetHashCode();

        public NetworkClosedEventArgs()
        {
            NetworkChannel = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => sEventId;

        /// <summary>
        /// 网络频道
        /// </summary>
        public INetworkChannel NetworkChannel { get; private set; }

        /// <summary>
        /// 创建网络连接失败事件
        /// </summary>
        /// <param name="e">内部事件</param>
        /// <returns>网络连接失败事件</returns>
        public static NetworkClosedEventArgs Create(Framework.NetworkClosedEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<NetworkClosedEventArgs>();
            eventArgs.NetworkChannel = e.NetworkChannel;
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
    public sealed class NetworkCustomErrorEventArgs : BaseEventArgs
    {
        private static readonly int sEventId = typeof(NetworkConnectedEventArgs).GetHashCode();

        public NetworkCustomErrorEventArgs()
        {
            NetworkChannel = null;
            CustomErrorData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => sEventId;

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
        /// <param name="e">内部事件</param>
        /// <returns>自定义网络错误事件</returns>
        public static NetworkCustomErrorEventArgs Create(Framework.NetworkCustomErrorEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<NetworkCustomErrorEventArgs>();
            eventArgs.NetworkChannel = e.NetworkChannel;
            eventArgs.CustomErrorData = e.CustomErrorData;
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
    public sealed class NetworkErrorEventArgs : BaseEventArgs
    {
        private static readonly int sEventId = typeof(NetworkConnectedEventArgs).GetHashCode();

        public NetworkErrorEventArgs()
        {
            NetworkChannel = null;
            ErrorCode = NetworkErrorCode.Unknown;
            SocketErrorCode = SocketError.Success;
            ErrorMessage = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => sEventId;

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
        /// <param name="e">内部事件</param>
        /// <returns>网络错误事件</returns>
        public static NetworkErrorEventArgs Create(Framework.NetworkErrorEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<NetworkErrorEventArgs>();
            eventArgs.NetworkChannel = e.NetworkChannel;
            eventArgs.ErrorCode = e.ErrorCode;
            eventArgs.SocketErrorCode = e.SocketErrorCode;
            eventArgs.ErrorMessage = e.ErrorMessage;
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
    public sealed class NetworkMissHeartBeatEventArgs : BaseEventArgs
    {
        private static readonly int sEventId = typeof(NetworkConnectedEventArgs).GetHashCode();

        public NetworkMissHeartBeatEventArgs()
        {
            NetworkChannel = null;
            MissCount = 0;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => sEventId;

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
        /// <param name="e">内部事件</param>
        /// <returns>网络丢失心跳包事件</returns>
        public static NetworkMissHeartBeatEventArgs Create(Framework.NetworkMissHeartBeatEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<NetworkMissHeartBeatEventArgs>();
            eventArgs.NetworkChannel = e.NetworkChannel;
            eventArgs.MissCount = e.MissCount;
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