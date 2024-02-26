/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/01/19 11:15:53
 * Description:
 * Modify Record:
 *************************************************************/

using System;

namespace Framework
{
    /// <summary>
    /// 下载代理辅助器完成事件
    /// </summary>
    public sealed class DownloadAgentHelperCompleteEventArgs : FrameworkEventArgs
    {
        public DownloadAgentHelperCompleteEventArgs()
        {
            Length = 0L;
        }

        /// <summary>
        /// 下载的数据大小
        /// </summary>
        public long Length { get; private set; }

        /// <summary>
        /// 创建下载代理辅助器完成事件
        /// </summary>
        /// <param name="length">下载的数据大小</param>
        /// <returns>下载代理辅助器完成事件</returns>
        /// <exception cref="Exception"></exception>
        public static DownloadAgentHelperCompleteEventArgs Create(long length)
        {
            if (length < 0L)
            {
                throw new Exception("Length is invalid.");
            }

            var eventArgs = ReferencePool.Acquire<DownloadAgentHelperCompleteEventArgs>();
            eventArgs.Length = length;
            return eventArgs;
        }

        /// <summary>
        /// 清除下载代理辅助器完成事件
        /// </summary>
        public override void Clear()
        {
            Length = 0L;
        }
    }

    /// <summary>
    /// 下载代理辅助器错误事件
    /// </summary>
    public sealed class DownloadAgentHelperErrorEventArgs : FrameworkEventArgs
    {
        public DownloadAgentHelperErrorEventArgs()
        {
            IsDeleteDownloading = false;
            ErrorMessage = null;
        }

        /// <summary>
        /// 是否删除正在下载的文件
        /// </summary>
        public bool IsDeleteDownloading { get; private set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// 创建下载代理辅助器错误事件
        /// </summary>
        /// <param name="isDeleteDownloading">是否删除正在下载的文件</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>下载代理辅助器错误事件</returns>
        public static DownloadAgentHelperErrorEventArgs Create(bool isDeleteDownloading, string errorMessage)
        {
            var eventArgs = ReferencePool.Acquire<DownloadAgentHelperErrorEventArgs>();
            eventArgs.IsDeleteDownloading = isDeleteDownloading;
            eventArgs.ErrorMessage = errorMessage;
            return eventArgs;
        }

        /// <summary>
        /// 清除下载代理辅助器错误事件
        /// </summary>
        public override void Clear()
        {
            IsDeleteDownloading = false;
            ErrorMessage = null;
        }
    }

    /// <summary>
    /// 下载代理辅助器更新数据流事件
    /// </summary>
    public sealed class DownloadAgentHelperUpdateBytesEventArgs : FrameworkEventArgs
    {
        public DownloadAgentHelperUpdateBytesEventArgs()
        {
            Bytes = null;
            Offset = 0;
            Length = 0;
        }

        /// <summary>
        /// 下载的数据流
        /// </summary>
        public byte[] Bytes { get; private set; }

        /// <summary>
        /// 数据流的偏移
        /// </summary>
        public int Offset { get; private set; }

        /// <summary>
        /// 数据流的大小
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// 创建下载代理辅助器更新数据流事件
        /// </summary>
        /// <param name="bytes">下载的数据流</param>
        /// <param name="offset">数据流的偏移</param>
        /// <param name="length">数据流的大小</param>
        /// <returns>下载代理辅助器更新数据流事件</returns>
        /// <exception cref="Exception"></exception>
        public static DownloadAgentHelperUpdateBytesEventArgs Create(byte[] bytes, int offset, int length)
        {
            if (bytes == null)
            {
                throw new Exception("Bytes is invalid.");
            }

            if (offset < 0 || offset >= bytes.Length)
            {
                throw new Exception("Offset is invalid.");
            }

            if (length <= 0 || offset + length > bytes.Length)
            {
                throw new Exception("Length is invalid.");
            }

            var eventArgs = ReferencePool.Acquire<DownloadAgentHelperUpdateBytesEventArgs>();
            eventArgs.Bytes = bytes;
            eventArgs.Offset = offset;
            eventArgs.Length = length;
            return eventArgs;
        }

        /// <summary>
        /// 清除下载代理辅助器更新数据流事件
        /// </summary>
        public override void Clear()
        {
            Bytes = null;
            Offset = 0;
            Length = 0;
        }
    }

    /// <summary>
    /// 下载代理辅助器更新数据大小事件
    /// </summary>
    public sealed class DownloadAgentHelperUpdateLengthEventArgs : FrameworkEventArgs
    {
        public DownloadAgentHelperUpdateLengthEventArgs()
        {
            DeltaLength = 0;
        }

        /// <summary>
        /// 下载的增量数据大小
        /// </summary>
        public int DeltaLength { get; private set; }

        /// <summary>
        /// 创建下载代理辅助器更新数据大小事件
        /// </summary>
        /// <param name="deltaLength"></param>
        /// <returns>下载代理辅助器更新数据大小事件</returns>
        /// <exception cref="Exception"></exception>
        public static DownloadAgentHelperUpdateLengthEventArgs Create(int deltaLength)
        {
            if (deltaLength <= 0)
            {
                throw new Exception("Delta length is invalid.");
            }

            var eventArgs = ReferencePool.Acquire<DownloadAgentHelperUpdateLengthEventArgs>();
            eventArgs.DeltaLength = deltaLength;
            return eventArgs;
        }

        /// <summary>
        /// 清除下载代理辅助器更新数据大小事件
        /// </summary>
        public override void Clear()
        {
            DeltaLength = 0;
        }
    }
}