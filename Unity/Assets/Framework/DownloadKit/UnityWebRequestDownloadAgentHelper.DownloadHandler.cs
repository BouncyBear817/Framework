using System;
using UnityEngine.Networking;

namespace Framework
{
    public partial class UnityWebRequestDownloadAgentHelper : DownloadAgentHelperBase, IDisposable
    {
        private sealed class DownloadHandler : DownloadHandlerScript
        {
            private readonly UnityWebRequestDownloadAgentHelper mOwner;

            public DownloadHandler(UnityWebRequestDownloadAgentHelper owner)
            {
                mOwner = owner;
            }

            protected override bool ReceiveData(byte[] data, int dataLength)
            {
                if (mOwner != null && mOwner.mUnityWebRequest != null && dataLength > 0)
                {
                    var updateBytesEventArgs = DownloadAgentHelperUpdateBytesEventArgs.Create(data, 0, dataLength);
                    mOwner.mDownloadAgentHelperUpdateBytes(this, updateBytesEventArgs);
                    ReferencePool.Release(updateBytesEventArgs);
                    
                    var updateLengthEventArgs = DownloadAgentHelperUpdateLengthEventArgs.Create(dataLength);
                    mOwner.mDownloadAgentHelperUpdateLength(this, updateLengthEventArgs);
                    ReferencePool.Release(updateLengthEventArgs);
                }
                
                return base.ReceiveData(data, dataLength);
            }
        }
    }
}