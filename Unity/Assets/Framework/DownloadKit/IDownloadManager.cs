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
    public interface IDownloadManager
    {
        bool Pause { get; set; }
        
        int TotalAgentCount { get; }
        
        int FreeAgentCount { get; }
        
        int WorkingAgentCount { get; }
        
        int FlushSize { get; set; }
        
        float Timeout { get; set; }
        
        float CurrentSpeed { get; }

        event EventHandler<DownloadStartEventArgs> DownloadStart;
        event EventHandler<DownloadUpdateEventArgs> DownloadUpdate;
        event EventHandler<DownloadSuccessEventArgs> DownloadSuccess;
        event EventHandler<DownloadFailureEventArgs> DownloadFailure;

        void AddDownloadAgentHelper(IDownloadAgentHelper downloadAgentHelper);

        TaskInfo GetDownloadInfo(int serialId);

        TaskInfo[] GetDownloadInfos(string tag);

        void GetDownloadInfos(string tag, List<TaskInfo> results);

        TaskInfo[] GetDownloadInfos();

        void GetDownloadInfos(List<TaskInfo> results);

        int AddDownload(DownloadInfo downloadInfo);

        bool RemoveDownload(int serialId);

        int RemoveDownload(string tag);

        int RemoveAllDownloads();
    }
}