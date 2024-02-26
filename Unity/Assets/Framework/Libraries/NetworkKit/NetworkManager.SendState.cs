// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/2/20 14:35:36
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
        private sealed class SendState : IDisposable
        {
            private const int DefaultBufferLength = 64 * 1024;

            private MemoryStream mMemoryStream;
            private bool mDisposed;

            public SendState()
            {
                mMemoryStream = new MemoryStream(DefaultBufferLength);
                mDisposed = false;
            }

            public MemoryStream MemoryStream => mMemoryStream;

            public void Reset()
            {
                mMemoryStream.Position = 0L;
                mMemoryStream.SetLength(0L);
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
        }
    }
}