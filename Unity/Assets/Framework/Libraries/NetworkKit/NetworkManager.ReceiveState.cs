// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/2/20 14:35:55
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.IO;

namespace Framework
{
    /// <summary>
    /// 网络管理器
    /// </summary>
    public sealed partial class NetworkManager : FrameworkModule, INetworkManager
    {
        private sealed class ReceiveState : IDisposable
        {
            private const int DefaultBufferLength = 64 * 1024;

            private MemoryStream mMemoryStream;
            private IPacketHeader mPacketHeader;
            private bool mDisposed;

            public ReceiveState()
            {
                mMemoryStream = new MemoryStream(DefaultBufferLength);
                mPacketHeader = null;
                mDisposed = false;
            }

            public MemoryStream MemoryStream => mMemoryStream;

            public IPacketHeader PacketHeader => mPacketHeader;

            public void PrepareForPacketHeader(int packetHeaderLength)
            {
                Reset(packetHeaderLength, null);
            }

            public void PrepareForPacket(IPacketHeader packetHeader)
            {
                if (packetHeader == null)
                {
                    throw new Exception("Packet header is invalid.");
                }

                Reset(packetHeader.PacketLength, packetHeader);
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
                    if (mMemoryStream != null)
                    {
                        mMemoryStream.Dispose();
                        mMemoryStream = null;
                    }
                }

                mDisposed = true;
            }

            private void Reset(int targetLength, IPacketHeader packetHeader)
            {
                if (targetLength < 0)
                {
                    throw new Exception("Target length is invalid.");
                }

                mMemoryStream.Position = 0L;
                mMemoryStream.SetLength(targetLength);
                mPacketHeader = packetHeader;
            }
        }
    }
}