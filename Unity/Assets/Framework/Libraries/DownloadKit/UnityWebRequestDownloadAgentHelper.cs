using System;
using UnityEngine.Networking;

namespace Framework
{
    public partial class UnityWebRequestDownloadAgentHelper : DownloadAgentHelperBase, IDisposable
    {
        private const int CachedBytesLength = 0x1000;
        private readonly byte[] mCachedBytes = new byte[CachedBytesLength];

        private UnityWebRequest mUnityWebRequest = null;
        private bool mDisposed = false;

        private EventHandler<DownloadAgentHelperCompleteEventArgs> mDownloadAgentHelperComplete = null;
        private EventHandler<DownloadAgentHelperErrorEventArgs> mDownloadAgentHelperError = null;
        private EventHandler<DownloadAgentHelperUpdateBytesEventArgs> mDownloadAgentHelperUpdateBytes = null;
        private EventHandler<DownloadAgentHelperUpdateLengthEventArgs> mDownloadAgentHelperUpdateLength = null;

        /// <summary>
        /// 下载代理辅助器完成事件
        /// </summary>
        public override event EventHandler<DownloadAgentHelperCompleteEventArgs> DownloadAgentHelperComplete
        {
            add => mDownloadAgentHelperComplete += value;
            remove => mDownloadAgentHelperComplete -= value;
        }

        /// <summary>
        /// 下载代理辅助器失败事件
        /// </summary>
        public override event EventHandler<DownloadAgentHelperErrorEventArgs> DownloadAgentHelperError
        {
            add => mDownloadAgentHelperError += value;
            remove => mDownloadAgentHelperError -= value;
        }

        /// <summary>
        /// 下载代理辅助器更新数据流事件
        /// </summary>
        public override event EventHandler<DownloadAgentHelperUpdateBytesEventArgs> DownloadAgentHelperUpdateBytes
        {
            add => mDownloadAgentHelperUpdateBytes += value;
            remove => mDownloadAgentHelperUpdateBytes -= value;
        }

        /// <summary>
        /// 下载代理辅助器更新大小事件
        /// </summary>
        public override event EventHandler<DownloadAgentHelperUpdateLengthEventArgs> DownloadAgentHelperUpdateLength
        {
            add => mDownloadAgentHelperUpdateLength += value;
            remove => mDownloadAgentHelperUpdateLength -= value;
        }

        /// <summary>
        /// 通过下载代理辅助器下载指定地址的数据
        /// </summary>
        /// <param name="downloadUri">下载地址</param>
        /// <param name="userData">自定义数据</param>
        /// <param name="fromPosition">下载数据起始位置</param>
        /// <param name="toPosition">下载数据结束位置</param>
        public override void Download(string downloadUri, object userData, long fromPosition = 0, long toPosition = 0)
        {
            if (mDownloadAgentHelperUpdateBytes == null || mDownloadAgentHelperUpdateLength == null)
            {
                throw new Exception("Download agent helper handler is invalid.");
            }

            mUnityWebRequest = UnityWebRequest.Get(downloadUri);
            if (fromPosition > 0)
            {
                var range = $"bytes={fromPosition}-";
                if (toPosition > 0 && toPosition > fromPosition)
                {
                    range = $"bytes={fromPosition}-{toPosition}";
                }

                mUnityWebRequest.SetRequestHeader("Range", range);
            }

            mUnityWebRequest.downloadHandler = new DownloadHandler(this);
            mUnityWebRequest.SendWebRequest();
        }

        /// <summary>
        /// 重置下载代理辅助器
        /// </summary>
        public override void Reset()
        {
            if (mUnityWebRequest != null)
            {
                mUnityWebRequest.Abort();
                mUnityWebRequest.Dispose();
                mUnityWebRequest = null;
            }

            Array.Clear(mCachedBytes, 0, CachedBytesLength);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (mDisposed)
            {
                return;
            }

            if (disposing)
            {
                if (mUnityWebRequest != null)
                {
                    mUnityWebRequest.Dispose();
                    mUnityWebRequest = null;
                }
            }

            mDisposed = true;
        }

        private void Update()
        {
            if (mUnityWebRequest == null || !mUnityWebRequest.isDone)
            {
                return;
            }

            var isError = mUnityWebRequest.result != UnityWebRequest.Result.Success;
            if (isError)
            {
                var eventArgs = DownloadAgentHelperErrorEventArgs.Create(
                    mUnityWebRequest.responseCode == RangeNotSatisfiableErrorCode, mUnityWebRequest.error);
                mDownloadAgentHelperError(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
            else
            {
                var eventArgs = DownloadAgentHelperCompleteEventArgs.Create((long)mUnityWebRequest.downloadedBytes);
                mDownloadAgentHelperComplete(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }
    }
}