namespace Framework
{
    public sealed partial class DownloadManager : FrameworkModule, IDownloadManager
    {
        /// <summary>
        /// 下载任务
        /// </summary>
        private sealed class DownloadTask : TaskBase
        {
            private static int sSerial = 0;

            private DownloadTaskStatus mStatus;
            private string mDownloadPath;
            private string mDownloadUri;
            private int mFlushSize;
            private float mTimeout;

            public DownloadTask()
            {
                mStatus = DownloadTaskStatus.Todo;
                mDownloadPath = null;
                mDownloadUri = null;
                mFlushSize = 0;
                mTimeout = 0f;
            }

            /// <summary>
            /// 下载任务状态
            /// </summary>
            public DownloadTaskStatus Status
            {
                get => mStatus;
                set => mStatus = value;
            }

            /// <summary>
            /// 下载后存放路径
            /// </summary>
            public string DownloadPath => mDownloadPath;

            /// <summary>
            /// 下载地址
            /// </summary>
            public string DownloadUri => mDownloadUri;

            /// <summary>
            /// 将缓冲区写入磁盘的临界大小
            /// </summary>
            public int FlushSize => mFlushSize;

            /// <summary>
            /// 下载超时时长，以秒为单位
            /// </summary>
            public float Timeout => mTimeout;

            /// <summary>
            /// 下载任务描述
            /// </summary>
            public override string Description => mDownloadPath;

            /// <summary>
            /// 创建下载任务
            /// </summary>
            /// <param name="downloadPath">下载后存放路径</param>
            /// <param name="downloadUri">下载地址</param>
            /// <param name="tag">任务标签</param>
            /// <param name="priority">任务优先级</param>
            /// <param name="flushSize">将缓冲区写入磁盘的临界大小</param>
            /// <param name="timeout">下载超时时长，以秒为单位</param>
            /// <param name="userData">自定义数据</param>
            /// <returns>下载任务</returns>
            public static DownloadTask Create(string downloadPath, string downloadUri, string tag, int priority,
                int flushSize, float timeout, object userData)
            {
                var task = ReferencePool.Acquire<DownloadTask>();
                task.Initialize(++sSerial, tag, priority, userData);
                task.mDownloadPath = downloadPath;
                task.mDownloadUri = downloadUri;
                task.mFlushSize = flushSize;
                task.mTimeout = timeout;
                return task;
            }

            /// <summary>
            /// 清理下载任务
            /// </summary>
            public override void Clear()
            {
                base.Clear();
                mStatus = DownloadTaskStatus.Todo;
                mDownloadPath = null;
                mDownloadUri = null;
                mFlushSize = 0;
                mTimeout = 0f;
            }
        }
    }
}