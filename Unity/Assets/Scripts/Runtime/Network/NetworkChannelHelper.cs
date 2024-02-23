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

namespace Runtime
{
    public class NetworkChannelHelper : INetworkChannelHelper
    {
        private const int DefaultCachedSize = 8 * 1024;
        private const int DefaultBufferSize = 64 * 1024;

        private readonly Dictionary<int, Type> mSCPacketTypes;
        private readonly MemoryStream mCachedStream;
        private INetworkChannel mNetworkChannel;

        public NetworkChannelHelper()
        {
            mSCPacketTypes = new Dictionary<int, Type>();
            mCachedStream = new MemoryStream(DefaultCachedSize);
            mNetworkChannel = null;
        }

        public int PacketHeaderLength => sizeof(int);

        public void Initialize(INetworkChannel networkChannel)
        {
            mNetworkChannel = networkChannel;

            var packetBaseType = typeof(SCPacketBase);
            var packetHandlerBaseType = typeof(PacketHandlerBase);
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();

            for (int i = 0; i < types.Length; i++)
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

            MainEntry.Event.Subscribe(NetworkConnectedEventArgs.EventId, OnNetworkConnected);
            MainEntry.Event.Subscribe(NetworkClosedEventArgs.EventId, OnNetworkClosed);
            MainEntry.Event.Subscribe(NetworkCustomErrorEventArgs.EventId, OnNetworkCustomError);
            MainEntry.Event.Subscribe(NetworkErrorEventArgs.EventId, OnNetworkError);
            MainEntry.Event.Subscribe(NetworkMissHeartBeatEventArgs.EventId, OnNetworkMissHeartBeat);
        }

        public void Shutdown()
        {
            MainEntry.Event.UnSubscribe(NetworkConnectedEventArgs.EventId, OnNetworkConnected);
            MainEntry.Event.UnSubscribe(NetworkClosedEventArgs.EventId, OnNetworkClosed);
            MainEntry.Event.UnSubscribe(NetworkCustomErrorEventArgs.EventId, OnNetworkCustomError);
            MainEntry.Event.UnSubscribe(NetworkErrorEventArgs.EventId, OnNetworkError);
            MainEntry.Event.UnSubscribe(NetworkMissHeartBeatEventArgs.EventId, OnNetworkMissHeartBeat);

            mSCPacketTypes.Clear();
            mCachedStream.Close();
            mNetworkChannel = null;
        }

        public void PrepareForConnecting()
        {
            mNetworkChannel.Socket.ReceiveBufferSize = DefaultBufferSize;
            mNetworkChannel.Socket.SendBufferSize = DefaultBufferSize;
        }

        public bool SendHeartBeat()
        {
            mNetworkChannel.Send(ReferencePool.Acquire<CSHeartBeat>());
            return true;
        }

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
            
            mCachedStream.SetLength(mCachedStream.Capacity);
            mCachedStream.Position = 0L;

            return true;
        }

        public IPacketHeader DeserializePacketHeader(Stream source, out object customErrorData)
        {
            throw new System.NotImplementedException();
        }

        public Packet DeserializePacket(IPacketHeader packetHeader, Stream source, out object customErrorData)
        {
            throw new System.NotImplementedException();
        }

        private Type GetSCPacketType(int packetBaseId)
        {
            return mSCPacketTypes.GetValueOrDefault(packetBaseId);
        }

        private void OnNetworkConnected(object sender, BaseEventArgs e)
        {
            var eventArgs = e as NetworkConnectedEventArgs;
            if (eventArgs == null || eventArgs.NetworkChannel != mNetworkChannel)
            {
                return;
            }
        }

        private void OnNetworkClosed(object sender, BaseEventArgs e)
        {
        }

        private void OnNetworkCustomError(object sender, BaseEventArgs e)
        {
        }

        private void OnNetworkError(object sender, BaseEventArgs e)
        {
        }

        private void OnNetworkMissHeartBeat(object sender, BaseEventArgs e)
        {
        }
    }
}