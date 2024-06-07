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
    /// 下载管理器
    /// </summary>
    public sealed partial class DownloadManager : FrameworkModule, IDownloadManager
    {
        private const int OneMegaBytes = 1024 * 1024;

        private readonly TaskPool<DownloadTask> mTaskPool;
        private readonly DownloadCounter mDownloadCounter;
        private int mFlushSize;
        private float mTimeout;

        private EventHandler<DownloadStartEventArgs> mDownloadStartEventHandler;
        private EventHandler<DownloadUpdateEventArgs> mDownloadUpdateEventHandler;
        private EventHandler<DownloadSuccessEventArgs> mDownloadSuccessEventHandler;
        private EventHandler<DownloadFailureEventArgs> mDownloadFailureEventHandler;

        public DownloadManager()
        {
            mTaskPool = new TaskPool<DownloadTask>();
            mDownloadCounter = new DownloadCounter(1f, 10f);
            mFlushSize = OneMegaBytes;
            mTimeout = 30f;
            mDownloadStartEventHandler = null;
            mDownloadUpdateEventHandler = null;
            mDownloadSuccessEventHandler = null;
            mDownloadFailureEventHandler = null;
        }

        /// <summary>
        /// 模块优先级
        /// </summary>
        /// <remarks>优先级较高的模块会优先轮询，关闭操作会后进行</remarks>
        public override int Priority => 5;

        /// <summary>
        /// 下载是否暂停
        /// </summary>
        public bool Paused
        {
            get => mTaskPool.Paused;
            set => mTaskPool.Paused = value;
        }

        /// <summary>
        /// 下载代理总数量
        /// </summary>
        public int TotalAgentCount => mTaskPool.TotalAgentCount;

        /// <summary>
        /// 可用下载代理数量
        /// </summary>
        public int AvailableAgentCount => mTaskPool.AvailableAgentCount;

        /// <summary>
        /// 工作中下载代理数量
        /// </summary>
        public int WorkingAgentCount => mTaskPool.WorkingAgentCount;

        /// <summary>
        /// 等待下载任务数量
        /// </summary>
        public int WaitingTaskCount => mTaskPool.WaitingTaskCount;

        /// <summary>
        /// 设置缓冲区写入磁盘的临界大小
        /// </summary>
        public int FlushSize
        {
            get => mFlushSize;
            set => mFlushSize = value;
        }

        /// <summary>
        /// 下载超时时长，以秒为单位
        /// </summary>
        public float Timeout
        {
            get => mTimeout;
            set => mTimeout = value;
        }

        /// <summary>
        /// 当前下载速度
        /// </summary>
        public float CurrentSpeed => mDownloadCounter.CurrentSpeed;

        /// <summary>
        /// 下载开始事件
        /// </summary>
        public event EventHandler<DownloadStartEventArgs> DownloadStart
        {
            add => mDownloadStartEventHandler += value;
            remove => mDownloadStartEventHandler -= value;
        }

        /// <summary>
        /// 下载更新事件
        /// </summary>
        public event EventHandler<DownloadUpdateEventArgs> DownloadUpdate
        {
            add => mDownloadUpdateEventHandler += value;
            remove => mDownloadUpdateEventHandler -= value;
        }

        /// <summary>
        /// 下载成功事件
        /// </summary>
        public event EventHandler<DownloadSuccessEventArgs> DownloadSuccess
        {
            add => mDownloadSuccessEventHandler += value;
            remove => mDownloadSuccessEventHandler -= value;
        }

        /// <summary>
        /// 下载失败事件
        /// </summary>
        public event EventHandler<DownloadFailureEventArgs> DownloadFailure
        {
            add => mDownloadFailureEventHandler += value;
            remove => mDownloadFailureEventHandler -= value;
        }

        /// <summary>
        /// 下载管理器轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝事件</param>
        /// <param name="realElapseSeconds">真实流逝事件</param>
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            mTaskPool.Update(elapseSeconds, realElapseSeconds);
            mDownloadCounter.Update(elapseSeconds, realElapseSeconds);
        }

        /// <summary>
        /// 清理下载管理器
        /// </summary>
        public override void Shutdown()
        {
            mTaskPool.Shutdown();
            mDownloadCounter.Shutdown();
        }

        /// <summary>
        /// 增加下载代理辅助器
        /// </summary>
        /// <param name="downloadAgentHelper">下载代理辅助器</param>
        public void AddDownloadAgentHelper(IDownloadAgentHelper downloadAgentHelper)
        {
            var agent = new DownloadAgent(downloadAgentHelper);
            agent.DownloadAgentStart += OnDownloadAgentStart;
            agent.DownloadAgentUpdate += OnDownloadAgentUpdate;
            agent.DownloadAgentSuccess += OnDownloadAgentSuccess;
            agent.DownloadAgentFailure += OnDownloadAgentFailure;
            
            mTaskPool.AddAgent(agent);
        }

        /// <summary>
        /// 根据任务编号获取下载任务信息
        /// </summary>
        /// <param name="serialId">任务编号</param>
        /// <returns>下载任务信息</returns>
        public TaskInfo GetDownloadInfo(int serialId)
        {
            return mTaskPool.GetTaskInfo(serialId);
        }

        /// <summary>
        /// 根据任务标签获取下载任务信息
        /// </summary>
        /// <param name="tag">任务标签</param>
        /// <returns>下载任务信息</returns>
        public TaskInfo[] GetDownloadInfos(string tag)
        {
            return mTaskPool.GetTaskInfos(tag);
        }

        /// <summary>
        /// 获取所有下载任务信息
        /// </summary>
        /// <returns>所有下载任务信息</returns>
        public TaskInfo[] GetAllDownloadInfos()
        {
            return mTaskPool.GetAllTaskInfos();
        }

        /// <summary>
        /// 获取所有下载任务信息
        /// </summary>
        /// <param name="results">所有下载任务信息</param>
        public void GetAllDownloadInfos(List<TaskInfo> results)
        {
            mTaskPool.GetAllTaskInfos(results);
        }

        /// <summary>
        /// 增加下载任务
        /// </summary>
        /// <param name="downloadInfo">下载任务信息</param>
        /// <returns>下载任务的序列编号</returns>
        public int AddDownload(DownloadInfo downloadInfo)
        {
            if (string.IsNullOrEmpty(downloadInfo.DownloadPath) || string.IsNullOrEmpty(downloadInfo.DownloadUri))
            {
                throw new Exception("Download path or uri is invalid.");
            }

            if (TotalAgentCount <= 0)
            {
                throw new Exception("You must add download agent first.");
            }

            var task = DownloadTask.Create(downloadInfo.DownloadPath, downloadInfo.DownloadUri, downloadInfo.Tag,
                downloadInfo.Priority, mFlushSize, mTimeout, downloadInfo.UserData);
            mTaskPool.AddTask(task);
            return task.SerialId;
        }

        /// <summary>
        /// 根据序列编号移除下载任务
        /// </summary>
        /// <param name="serialId">下载任务的序列编号</param>
        /// <returns>是否移除成功</returns>
        public bool RemoveDownload(int serialId)
        {
            return mTaskPool.RemoveTask(serialId);
        }

        /// <summary>
        /// 根据任务标签移除下载任务
        /// </summary>
        /// <param name="tag">任务标签</param>
        /// <returns>移除下载任务的数量</returns>
        public int RemoveDownload(string tag)
        {
            return mTaskPool.RemoveTask(tag);
        }

        /// <summary>
        /// 移除所有下载任务
        /// </summary>
        /// <returns>移除下载任务的数量</returns>
        public int RemoveAllDownloads()
        {
            return mTaskPool.RemoveAllTasks();
        }

        private void OnDownloadAgentStart(DownloadAgent sender)
        {
            if (mDownloadStartEventHandler != null)
            {
                var eventArgs = DownloadStartEventArgs.Create(sender.Task.SerialId, sender.Task.DownloadPath,
                    sender.Task.DownloadUri, sender.CurrentLength, sender.Task.UserData);
                mDownloadStartEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }

        private void OnDownloadAgentUpdate(DownloadAgent sender, int deltaLength)
        {
            mDownloadCounter.RecordDeltaLength(deltaLength);
            if (mDownloadUpdateEventHandler != null)
            {
                var eventArgs = DownloadUpdateEventArgs.Create(sender.Task.SerialId, sender.Task.DownloadPath,
                    sender.Task.DownloadUri, sender.CurrentLength, sender.Task.UserData);
                mDownloadUpdateEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }

        private void OnDownloadAgentSuccess(DownloadAgent sender, long length)
        {
            if (mDownloadSuccessEventHandler != null)
            {
                var eventArgs = DownloadSuccessEventArgs.Create(sender.Task.SerialId, sender.Task.DownloadPath,
                    sender.Task.DownloadUri, sender.CurrentLength, sender.Task.UserData);
                mDownloadSuccessEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }

        private void OnDownloadAgentFailure(DownloadAgent sender, string errorMessage)
        {
            if (mDownloadFailureEventHandler != null)
            {
                var eventArgs = DownloadFailureEventArgs.Create(sender.Task.SerialId, sender.Task.DownloadPath,
                    sender.Task.DownloadUri, errorMessage, sender.Task.UserData);
                mDownloadFailureEventHandler(this, eventArgs);
                ReferencePool.Release(eventArgs);
            }
        }
    }
}