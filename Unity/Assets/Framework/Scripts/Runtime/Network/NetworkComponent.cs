/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/2/21 14:33:55
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using Framework;
using Runtime;
using UnityEngine;

namespace Runtime
{
    /// <summary>
    /// 网络组件
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Framework/Network")]
    public class NetworkComponent : FrameworkComponent
    {
        private INetworkManager mNetworkManager = null;
        private EventComponent mEventComponent = null;

        /// <summary>
        /// 网络频道数量
        /// </summary>
        public int NetworkChannelCount => mNetworkManager.NetworkChannelCount;

        protected override void Awake()
        {
            base.Awake();
            mNetworkManager = FrameworkEntry.GetModule<INetworkManager>();
            if (mNetworkManager == null)
            {
                Log.Error("Network manager is invalid.");
                return;
            }

            mNetworkManager.NetworkConnected += OnNetworkConnected;
            mNetworkManager.NetworkClosed += OnNetworkClosed;
            mNetworkManager.NetworkCustomError += OnNetworkCustomError;
            mNetworkManager.NetworkError += OnNetworkError;
            mNetworkManager.NetworkMissHeartBeat += OnNetworkMissHeartBeat;
        }

        private void Start()
        {
            mEventComponent = MainEntry.Helper.GetComponent<EventComponent>();
            if (mEventComponent == null)
            {
                Log.Error("Event component is invalid.");
            }
        }


        /// <summary>
        /// 是否存在网络频道
        /// </summary>
        /// <param name="name">网络频道名称</param>
        /// <returns>是否存在网络频道</returns>
        public bool HasNetworkChannel(string name)
        {
            return mNetworkManager.HasNetworkChannel(name);
        }

        /// <summary>
        /// 获取网络频道
        /// </summary>
        /// <param name="name">网络频道名称</param>
        /// <returns>网络频道</returns>
        public INetworkChannel GetNetworkChannel(string name)
        {
            return mNetworkManager.GetNetworkChannel(name);
        }

        /// <summary>
        /// 获取所有网络频道
        /// </summary>
        /// <returns>所有网络频道</returns>
        public INetworkChannel[] GetAllNetworkChannel()
        {
            return mNetworkManager.GetAllNetworkChannel();
        }

        /// <summary>
        /// 获取所有网络频道
        /// </summary>
        /// <param name="results">所有网络频道</param>
        public void GetAllNetworkChannel(List<INetworkChannel> results)
        {
            mNetworkManager.GetAllNetworkChannel(results);
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
            return mNetworkManager.CreateNetworkChannel(name, serviceType, networkChannelHelper);
        }

        /// <summary>
        /// 销毁网络频道
        /// </summary>
        /// <param name="name">网络频道名称</param>
        /// <returns>是否销毁网络频道成功</returns>
        public bool DestroyNetworkChannel(string name)
        {
            return mNetworkManager.DestroyNetworkChannel(name);
        }

        public void Connect(string ip, int port, string name = "Default", object userData = null)
        {
            var networkChannel = GetNetworkChannel(name);
            if (networkChannel == null)
            {
                networkChannel = CreateNetworkChannel(name, ServiceType.Tcp, new NetworkChannelHelper());
            }
            
            networkChannel.Connect(IPAddress.Parse(ip), port, userData);
        }

        public void Close(string name = "Default")
        {
            var networkChannel = GetNetworkChannel(name);
            networkChannel?.Close();
        }

        private void OnNetworkConnected(object sender, Framework.NetworkConnectedEventArgs e)
        {
            mEventComponent.Fire(this, NetworkConnectedEventArgs.Create(e));
        }

        private void OnNetworkClosed(object sender, Framework.NetworkClosedEventArgs e)
        {
            mEventComponent.Fire(this, NetworkClosedEventArgs.Create(e));
        }

        private void OnNetworkCustomError(object sender, Framework.NetworkCustomErrorEventArgs e)
        {
            mEventComponent.Fire(this, NetworkCustomErrorEventArgs.Create(e));
        }

        private void OnNetworkError(object sender, Framework.NetworkErrorEventArgs e)
        {
            mEventComponent.Fire(this, NetworkErrorEventArgs.Create(e));
        }

        private void OnNetworkMissHeartBeat(object sender, Framework.NetworkMissHeartBeatEventArgs e)
        {
            mEventComponent.Fire(this, NetworkMissHeartBeatEventArgs.Create(e));
        }
    }
}