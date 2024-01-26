using System;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 下载代理辅助器基类
    /// </summary>
    public abstract class DownloadAgentHelperBase : MonoBehaviour, IDownloadAgentHelper
    {
        protected const long RangeNotSatisfiableErrorCode = 416;
        
        /// <summary>
        /// 下载代理辅助器完成事件
        /// </summary>
        public abstract event EventHandler<DownloadAgentHelperCompleteEventArgs> DownloadAgentHelperComplete;

        /// <summary>
        /// 下载代理辅助器失败事件
        /// </summary>
        public abstract event EventHandler<DownloadAgentHelperErrorEventArgs> DownloadAgentHelperError;

        /// <summary>
        /// 下载代理辅助器更新数据流事件
        /// </summary>
        public abstract event EventHandler<DownloadAgentHelperUpdateBytesEventArgs> DownloadAgentHelperUpdateBytes;

        /// <summary>
        /// 下载代理辅助器更新大小事件
        /// </summary>
        public abstract event EventHandler<DownloadAgentHelperUpdateLengthEventArgs> DownloadAgentHelperUpdateLength;

        /// <summary>
        /// 通过下载代理辅助器下载指定地址的数据
        /// </summary>
        /// <param name="downloadUri">下载地址</param>
        /// <param name="userData">自定义数据</param>
        /// <param name="fromPosition">下载数据起始位置</param>
        /// <param name="toPosition">下载数据结束位置</param>
        public abstract void Download(string downloadUri, object userData, long fromPosition = 0, long toPosition = 0);

        /// <summary>
        /// 重置下载代理辅助器
        /// </summary>
        public abstract void Reset();
    }
}