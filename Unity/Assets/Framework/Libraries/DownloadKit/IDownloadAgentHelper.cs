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
    /// 下载代理辅助器接口
    /// </summary>
    public interface IDownloadAgentHelper
    {
        /// <summary>
        /// 下载代理辅助器完成事件
        /// </summary>
        event EventHandler<DownloadAgentHelperCompleteEventArgs> DownloadAgentHelperComplete;

        /// <summary>
        /// 下载代理辅助器失败事件
        /// </summary>
        event EventHandler<DownloadAgentHelperErrorEventArgs> DownloadAgentHelperError;

        /// <summary>
        /// 下载代理辅助器更新数据流事件
        /// </summary>
        event EventHandler<DownloadAgentHelperUpdateBytesEventArgs> DownloadAgentHelperUpdateBytes;

        /// <summary>
        /// 下载代理辅助器更新大小事件
        /// </summary>
        event EventHandler<DownloadAgentHelperUpdateLengthEventArgs> DownloadAgentHelperUpdateLength;

        /// <summary>
        /// 通过下载代理辅助器下载指定地址的数据
        /// </summary>
        /// <param name="downloadUri">下载地址</param>
        /// <param name="userData">自定义数据</param>
        /// <param name="fromPosition">下载数据起始位置</param>
        /// <param name="toPosition">下载数据结束位置</param>
        void Download(string downloadUri, object userData, long fromPosition = 0L, long toPosition = 0L);

        /// <summary>
        /// 重置下载代理辅助器
        /// </summary>
        void Reset();
    }
}