/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/3/22 15:39:4
 * Description:
 * Modify Record:
 *************************************************************/

using System.Collections.Generic;
using System.Net;
using Framework;
using UnityEngine;

namespace Runtime
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Custom/NetConnector")]
    public class NetConnectorComponent : FrameworkComponent
    {
        private readonly Dictionary<string, INetworkChannel> mNetworkChannels =
            new Dictionary<string, INetworkChannel>();

        private NetworkChannelHelper mNetworkChannelHelper;

        [SerializeField] private bool mIsLittleEndian = false;

        [SerializeField] private DataTransferFormat mDataTransferFormat = DataTransferFormat.None;

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

        public void Connect(string ip, int port, string name = "Default", object userData = null)
        {
            var networkChannel = mNetworkChannels.GetValueOrDefault(name);
            if (networkChannel == null)
            {
                Log.Error($"Connect failed, channel name ({name}) is null.");
                return;
            }

            networkChannel.Connect(IPAddress.Parse(ip), port, userData);
        }

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

        public void SetHeartBeatInterval(string name, float heartBeatInterval)
        {
            var networkChannel = mNetworkChannels.GetValueOrDefault(name);
            if (networkChannel != null)
            {
                networkChannel.HeartBeatInterval = heartBeatInterval;
            }
        }

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
}