/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/2/20 11:20:18
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 网络管理器接口
    /// </summary>
    public interface INetworkManager
    {
        /// <summary>
        /// 网络频道数量
        /// </summary>
        int NetworkChannelCount { get; }

        /// <summary>
        /// 网络连接成功事件
        /// </summary>
        event EventHandler<NetworkConnectedEventArgs> NetworkConnected;

        /// <summary>
        /// 网络连接关闭事件
        /// </summary>
        event EventHandler<NetworkClosedEventArgs> NetworkClosed;

        /// <summary>
        /// 网络自定义错误事件
        /// </summary>
        event EventHandler<NetworkCustomErrorEventArgs> NetworkCustomError;

        /// <summary>
        /// 网络错误事件
        /// </summary>
        event EventHandler<NetworkErrorEventArgs> NetworkError;

        /// <summary>
        /// 网络丢失心跳包事件
        /// </summary>
        event EventHandler<NetworkMissHeartBeatEventArgs> NetworkMissHeartBeat;

        /// <summary>
        /// 是否存在网络频道
        /// </summary>
        /// <param name="name">网络频道名称</param>
        /// <returns>是否存在网络频道</returns>
        bool HasNetworkChannel(string name);

        /// <summary>
        /// 获取网络频道
        /// </summary>
        /// <param name="name">网络频道名称</param>
        /// <returns>网络频道</returns>
        INetworkChannel GetNetworkChannel(string name);

        /// <summary>
        /// 获取所有网络频道
        /// </summary>
        /// <returns>所有网络频道</returns>
        INetworkChannel[] GetAllNetworkChannel();

        /// <summary>
        /// 获取所有网络频道
        /// </summary>
        /// <param name="results">所有网络频道</param>
        void GetAllNetworkChannel(List<INetworkChannel> results);

        /// <summary>
        /// 创建网络频道
        /// </summary>
        /// <param name="name">网络频道名称</param>
        /// <param name="serviceType">网络服务类型</param>
        /// <param name="networkChannelHelper">网络频道辅助器</param>
        /// <returns>网络频道</returns>
        INetworkChannel CreateNetworkChannel(string name, ServiceType serviceType,
            INetworkChannelHelper networkChannelHelper);

        /// <summary>
        /// 销毁网络频道
        /// </summary>
        /// <param name="name">网络频道名称</param>
        /// <returns>是否销毁网络频道成功</returns>
        bool DestroyNetworkChannel(string name);
    }
}