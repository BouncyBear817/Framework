// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/2/20 15:58:1
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Net;
using System.Net.Sockets;

namespace Framework
{
    /// <summary>
    /// 网络管理器
    /// </summary>
    public sealed partial class NetworkManager : FrameworkModule, INetworkManager
    {
        private sealed class TcpWithSyncReceiveNetworkChannel : NetworkChannelBase
        {
            private readonly AsyncCallback mConnectCallback;
            private readonly AsyncCallback mSendCallback;

            public TcpWithSyncReceiveNetworkChannel(string name, INetworkChannelHelper networkChannelHelper) : base(
                name,
                networkChannelHelper)
            {
                mConnectCallback = ConnectCallback;
                mSendCallback = SendCallback;
            }

            /// <summary>
            /// 网络服务类型
            /// </summary>
            public override ServiceType ServiceType => ServiceType.TcpWithSyncReceive;

            /// <summary>
            /// 连接远程主机
            /// </summary>
            /// <param name="ipAddress">IP地址</param>
            /// <param name="port">IP端口</param>
            /// <param name="userData">自定义数据</param>
            public override void Connect(IPAddress ipAddress, int port, object userData = null)
            {
                base.Connect(ipAddress, port, userData);
                mSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                if (mSocket == null)
                {
                    var errorMessage = "Initialize network channel failure.";
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.SocketError, SocketError.Success,
                            errorMessage);
                        return;
                    }

                    throw new Exception(errorMessage);
                }

                mNetworkChannelHelper.PrepareForConnecting();
                ConnectAsync(ipAddress, port, userData);
            }

            protected override bool ProcessSend()
            {
                if (base.ProcessSend())
                {
                    SendAsync();
                    return true;
                }

                return false;
            }

            protected override void ProcessReceive()
            {
                base.ProcessReceive();
                while (mSocket.Available > 0)
                {
                    if (!ReceiveAsync())
                    {
                        break;
                    }
                }
            }

            private void ConnectAsync(IPAddress ipAddress, int port, object userData)
            {
                try
                {
                    mSocket.BeginConnect(ipAddress, port, mConnectCallback, new ConnectState(mSocket, userData));
                }
                catch (Exception e)
                {
                    if (NetworkChannelError != null)
                    {
                        var socketException = e as SocketException;
                        NetworkChannelError(this, NetworkErrorCode.ConnectError,
                            socketException != null ? socketException.SocketErrorCode : SocketError.Success,
                            e.ToString());
                        return;
                    }

                    throw;
                }
            }

            private void ConnectCallback(IAsyncResult ar)
            {
                var socketUserData = ar.AsyncState as ConnectState;
                try
                {
                    socketUserData?.Socket.EndConnect(ar);
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
                catch (Exception e)
                {
                    mActive = false;
                    if (NetworkChannelError != null)
                    {
                        var socketException = e as SocketException;
                        NetworkChannelError(this, NetworkErrorCode.ConnectError,
                            socketException != null ? socketException.SocketErrorCode : SocketError.Success,
                            e.ToString());
                        return;
                    }

                    throw;
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

                if (socketUserData != null)
                    NetworkChannelConnected?.Invoke(this, socketUserData.UserData);

                mActive = true;
            }


            private void SendAsync()
            {
                try
                {
                    mSocket.BeginSend(mSendState.MemoryStream.GetBuffer(), (int)mSendState.MemoryStream.Position,
                        (int)(mSendState.MemoryStream.Length - mSendState.MemoryStream.Position), SocketFlags.None,
                        mSendCallback, mSocket);
                }
                catch (Exception e)
                {
                    mActive = false;
                    if (NetworkChannelError != null)
                    {
                        var socketException = e as SocketException;
                        NetworkChannelError(this, NetworkErrorCode.SendError,
                            socketException != null ? socketException.SocketErrorCode : SocketError.Success,
                            e.ToString());
                        return;
                    }

                    throw;
                }
            }

            private void SendCallback(IAsyncResult ar)
            {
                var socket = ar.AsyncState as Socket;
                if (socket == null || !socket.Connected)
                {
                    return;
                }

                int bytesSent = 0;
                try
                {
                    bytesSent = socket.EndSend(ar);
                }
                catch (Exception e)
                {
                    mActive = false;
                    if (NetworkChannelError != null)
                    {
                        var socketException = e as SocketException;
                        NetworkChannelError(this, NetworkErrorCode.SendError,
                            socketException != null ? socketException.SocketErrorCode : SocketError.Success,
                            e.ToString());
                        return;
                    }

                    throw;
                }

                mSendState.MemoryStream.Position += bytesSent;
                if (mSendState.MemoryStream.Position < mSendState.MemoryStream.Length)
                {
                    SendAsync();
                    return;
                }

                mSentPacketCount++;
                mSendState.Reset();
            }

            private bool ReceiveAsync()
            {
                try
                {
                    var bytesReceived = mSocket.Receive(mReceiveState.MemoryStream.GetBuffer(),
                        (int)mReceiveState.MemoryStream.Position,
                        (int)(mReceiveState.MemoryStream.Length - mReceiveState.MemoryStream.Position),
                        SocketFlags.None);
                    if (bytesReceived <= 0)
                    {
                        Close();
                        return false;
                    }

                    mReceiveState.MemoryStream.Position += bytesReceived;
                    if (mReceiveState.MemoryStream.Position < mReceiveState.MemoryStream.Length)
                    {
                        return false;
                    }

                    mReceiveState.MemoryStream.Position = 0L;

                    var processSuccess = false;
                    if (mReceiveState.PacketHeader != null)
                    {
                        processSuccess = ProcessPacket();
                        mReceivedPacketCount++;
                    }
                    else
                    {
                        processSuccess = ProcessPacketHeader();
                    }

                    return processSuccess;
                }
                catch (Exception e)
                {
                    mActive = false;
                    if (NetworkChannelError != null)
                    {
                        var socketException = e as SocketException;
                        NetworkChannelError(this, NetworkErrorCode.ReceiveError,
                            socketException != null ? socketException.SocketErrorCode : SocketError.Success,
                            e.ToString());
                        return false;
                    }

                    throw;
                }
            }
        }
    }
}