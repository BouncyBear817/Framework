/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/01/19 11:15:53
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 下载管理器接口
    /// </summary>
    public interface IDownloadManager
    {
        /// <summary>
        /// 下载是否暂停
        /// </summary>
        bool Paused { get; set; }

        /// <summary>
        /// 下载代理总数量
        /// </summary>
        int TotalAgentCount { get; }

        /// <summary>
        /// 可用下载代理数量
        /// </summary>
        int AvailableAgentCount { get; }

        /// <summary>
        /// 工作中下载代理数量
        /// </summary>
        int WorkingAgentCount { get; }

        /// <summary>
        /// 等待下载任务数量
        /// </summary>
        int WaitingTaskCount { get; }

        /// <summary>
        /// 设置缓冲区写入磁盘的临界大小
        /// </summary>
        int FlushSize { get; set; }

        /// <summary>
        /// 下载超时时长，以秒为单位
        /// </summary>
        float Timeout { get; set; }

        /// <summary>
        /// 当前下载速度
        /// </summary>
        float CurrentSpeed { get; }

        /// <summary>
        /// 下载开始事件
        /// </summary>
        event EventHandler<DownloadStartEventArgs> DownloadStart;

        /// <summary>
        /// 下载更新事件
        /// </summary>
        event EventHandler<DownloadUpdateEventArgs> DownloadUpdate;

        /// <summary>
        /// 下载成功事件
        /// </summary>
        event EventHandler<DownloadSuccessEventArgs> DownloadSuccess;

        /// <summary>
        /// 下载失败事件
        /// </summary>
        event EventHandler<DownloadFailureEventArgs> DownloadFailure;

        /// <summary>
        /// 增加下载代理辅助器
        /// </summary>
        /// <param name="downloadAgentHelper">下载代理辅助器</param>
        void AddDownloadAgentHelper(IDownloadAgentHelper downloadAgentHelper);

        /// <summary>
        /// 根据任务编号获取下载任务信息
        /// </summary>
        /// <param name="serialId">任务编号</param>
        /// <returns>下载任务信息</returns>
        TaskInfo GetDownloadInfo(int serialId);

        /// <summary>
        /// 根据任务标签获取下载任务信息
        /// </summary>
        /// <param name="tag">任务标签</param>
        /// <returns>下载任务信息</returns>
        TaskInfo[] GetDownloadInfos(string tag);

        /// <summary>
        /// 获取所有下载任务信息
        /// </summary>
        /// <returns>所有下载任务信息</returns>
        TaskInfo[] GetAllDownloadInfos();

        /// <summary>
        /// 获取所有下载任务信息
        /// </summary>
        /// <param name="results">所有下载任务信息</param>
        void GetAllDownloadInfos(List<TaskInfo> results);

        /// <summary>
        /// 增加下载任务
        /// </summary>
        /// <param name="downloadInfo">下载任务信息</param>
        /// <returns>下载任务的序列编号</returns>
        int AddDownload(DownloadInfo downloadInfo);

        /// <summary>
        /// 根据序列编号移除下载任务
        /// </summary>
        /// <param name="serialId">下载任务的序列编号</param>
        /// <returns>是否移除成功</returns>
        bool RemoveDownload(int serialId);

        /// <summary>
        /// 根据任务标签移除下载任务
        /// </summary>
        /// <param name="tag">任务标签</param>
        /// <returns>移除下载任务的数量</returns>
        int RemoveDownload(string tag);

        /// <summary>
        /// 移除所有下载任务
        /// </summary>
        /// <returns>移除下载任务的数量</returns>
        int RemoveAllDownloads();
    }
}