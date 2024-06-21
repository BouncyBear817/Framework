/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/01/30 16:12:04
 * Description:   
 * Modify Record: 
 *************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Framework.Runtime
{
    /// <summary>
    /// 下载组件
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Framework/Download")]
    public sealed class DownloadComponent : FrameworkComponent
    {
        private const int DefaultPriority = 0;
        private const int OneMegaBytes = 1024 * 1024;

        private IDownloadManager mDownloadManager;
        private EventComponent mEventComponent;

        [SerializeField] private Transform mInstanceRoot = null;

        [SerializeField] private string mDownloadAgentHelperTypeName =
            "Framework.UnityWebRequestDownloadAgentHelper";

        [SerializeField] private DownloadAgentHelperBase mCustomDownloadAgentHelper = null;

        [SerializeField] private int mDownloadAgentHelperCount = 3;

        [SerializeField] private float mTimeout = 30f;

        [SerializeField] private int mFlushSize = OneMegaBytes;

        /// <summary>
        /// 下载是否暂停
        /// </summary>
        public bool Paused
        {
            get => mDownloadManager.Paused;
            set => mDownloadManager.Paused = value;
        }

        /// <summary>
        /// 下载代理总数量
        /// </summary>
        public int TotalAgentCount => mDownloadManager.TotalAgentCount;

        /// <summary>
        /// 可用下载代理数量
        /// </summary>
        public int AvailableAgentCount => mDownloadManager.AvailableAgentCount;

        /// <summary>
        /// 工作中下载代理数量
        /// </summary>
        public int WorkingAgentCount => mDownloadManager.WorkingAgentCount;

        /// <summary>
        /// 等待下载任务数量
        /// </summary>
        public int WaitingTaskCount => mDownloadManager.WaitingTaskCount;

        /// <summary>
        /// 设置缓冲区写入磁盘的临界大小
        /// </summary>
        public int FlushSize
        {
            get => mDownloadManager.FlushSize;
            set => mDownloadManager.FlushSize = mFlushSize = value;
        }

        /// <summary>
        /// 下载超时时长，以秒为单位
        /// </summary>
        public float Timeout
        {
            get => mDownloadManager.Timeout;
            set => mDownloadManager.Timeout = mTimeout = value;
        }

        /// <summary>
        /// 当前下载速度
        /// </summary>
        public float CurrentSpeed => mDownloadManager.CurrentSpeed;

        protected override void Awake()
        {
            base.Awake();
            mDownloadManager = FrameworkEntry.GetModule<IDownloadManager>();
            if (mDownloadManager == null)
            {
                Log.Fatal("Download manager is invalid.");
                return;
            }

            mDownloadManager.DownloadStart += OnDownloadStart;
            mDownloadManager.DownloadUpdate += OnDownloadUpdate;
            mDownloadManager.DownloadSuccess += OnDownloadSuccess;
            mDownloadManager.DownloadFailure += OnDownloadFailure;
            mDownloadManager.Timeout = mTimeout;
            mDownloadManager.FlushSize = mFlushSize;
        }

        private void Start()
        {
            mEventComponent = MainEntryHelper.GetComponent<EventComponent>();
            if (mEventComponent == null)
            {
                Log.Fatal("Event component is invalid.");
                return;
            }

            if (mInstanceRoot == null)
            {
                mInstanceRoot = new GameObject("Download Agent Instances").transform;
                mInstanceRoot.SetParent(transform);
                mInstanceRoot.localScale = Vector3.zero;
                ;
            }

            for (var i = 0; i < mDownloadAgentHelperCount; i++)
            {
                AddDownloadAgentHelper(i);
            }
        }

        /// <summary>
        /// 根据任务编号获取下载任务信息
        /// </summary>
        /// <param name="serialId">任务编号</param>
        /// <returns>下载任务信息</returns>
        public TaskInfo GetDownloadInfo(int serialId)
        {
            return mDownloadManager.GetDownloadInfo(serialId);
        }

        /// <summary>
        /// 根据任务标签获取下载任务信息
        /// </summary>
        /// <param name="tag">任务标签</param>
        /// <returns>下载任务信息</returns>
        public TaskInfo[] GetDownloadInfos(string tag)
        {
            return mDownloadManager.GetDownloadInfos(tag);
        }

        /// <summary>
        /// 获取所有下载任务信息
        /// </summary>
        /// <returns>所有下载任务信息</returns>
        public TaskInfo[] GetAllDownloadInfos()
        {
            return mDownloadManager.GetAllDownloadInfos();
        }

        /// <summary>
        /// 获取所有下载任务信息
        /// </summary>
        /// <param name="results">所有下载任务信息</param>
        public void GetAllDownloadInfos(List<TaskInfo> results)
        {
            mDownloadManager.GetAllDownloadInfos(results);
        }

        /// <summary>
        /// 增加下载任务
        /// </summary>
        /// <param name="downloadInfo">下载任务信息</param>
        /// <returns>下载任务的序列编号</returns>
        public int AddDownload(DownloadInfo downloadInfo)
        {
            return mDownloadManager.AddDownload(downloadInfo);
        }

        /// <summary>
        /// 根据序列编号移除下载任务
        /// </summary>
        /// <param name="serialId">下载任务的序列编号</param>
        /// <returns>是否移除成功</returns>
        public bool RemoveDownload(int serialId)
        {
            return mDownloadManager.RemoveDownload(serialId);
        }

        /// <summary>
        /// 根据任务标签移除下载任务
        /// </summary>
        /// <param name="tag">任务标签</param>
        /// <returns>移除下载任务的数量</returns>
        public int RemoveDownload(string tag)
        {
            return mDownloadManager.RemoveDownload(tag);
        }

        /// <summary>
        /// 移除所有下载任务
        /// </summary>
        /// <returns>移除下载任务的数量</returns>
        public int RemoveAllDownloads()
        {
            return mDownloadManager.RemoveAllDownloads();
        }

        /// <summary>
        /// 增加下载代理辅助器
        /// </summary>
        /// <param name="index">下载代理辅助器索引</param>
        private void AddDownloadAgentHelper(int index)
        {
            var helper = Helper.CreateHelper(mDownloadAgentHelperTypeName, mCustomDownloadAgentHelper, index);
            if (helper == null)
            {
                Log.Error("Can not create download agent helper.");
                return;
            }

            helper.name = $"Download Agent Helper - {index}";
            var helperTransform = helper.transform;
            helperTransform.SetParent(mInstanceRoot);
            helperTransform.localScale = Vector3.zero;

            mDownloadManager.AddDownloadAgentHelper(helper);
        }

        private void OnDownloadStart(object sender, Framework.DownloadStartEventArgs e)
        {
            Log.Info($"Download start, info : {e.ToString()}");
            mEventComponent.Fire(this, DownloadStartEventArgs.Create(e));
        }

        private void OnDownloadUpdate(object sender, Framework.DownloadUpdateEventArgs e)
        {
            mEventComponent.Fire(this, DownloadUpdateEventArgs.Create(e));
        }

        private void OnDownloadSuccess(object sender, Framework.DownloadSuccessEventArgs e)
        {
            Log.Info($"Download success, info : {e.ToString()}");
            mEventComponent.Fire(this, DownloadSuccessEventArgs.Create(e));
        }

        private void OnDownloadFailure(object sender, Framework.DownloadFailureEventArgs e)
        {
            Log.Warning($"Download failure, info : {e.ToString()}");
            mEventComponent.Fire(this, DownloadFailureEventArgs.Create(e));
        }
    }
}