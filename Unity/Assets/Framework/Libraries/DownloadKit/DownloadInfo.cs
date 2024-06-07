namespace Framework
{
    /// <summary>
    /// 下载任务信息
    /// </summary>
    public struct DownloadInfo
    {
        private readonly string mDownloadPath;
        private readonly string mDownloadUri;
        private readonly string mTag;
        private readonly int mPriority;
        private readonly object mUserData;

        public DownloadInfo(string downloadPath, string downloadUri) : this()
        {
            mDownloadPath = downloadPath;
            mDownloadUri = downloadUri;
        }

        public DownloadInfo(string downloadPath, string downloadUri, string tag) : this()
        {
            mDownloadPath = downloadPath;
            mDownloadUri = downloadUri;
            mTag = tag;
        }

        public DownloadInfo(string downloadPath, string downloadUri, object userData) : this()
        {
            mDownloadPath = downloadPath;
            mDownloadUri = downloadUri;
            mUserData = userData;
        }

        public DownloadInfo(string downloadPath, string downloadUri, string tag, int priority) : this()
        {
            mDownloadPath = downloadPath;
            mDownloadUri = downloadUri;
            mTag = tag;
            mPriority = priority;
        }

        public DownloadInfo(string downloadPath, string downloadUri, string tag, object userData) : this()
        {
            mDownloadPath = downloadPath;
            mDownloadUri = downloadUri;
            mTag = tag;
            mUserData = userData;
        }

        public DownloadInfo(string downloadPath, string downloadUri, string tag, int priority,
            object userData)
        {
            mDownloadPath = downloadPath;
            mDownloadUri = downloadUri;
            mTag = tag;
            mPriority = priority;
            mUserData = userData;
        }

        /// <summary>
        /// 下载存放地址
        /// </summary>
        public string DownloadPath => mDownloadPath;

        /// <summary>
        /// 下载地址
        /// </summary>
        public string DownloadUri => mDownloadUri;

        /// <summary>
        /// 下载任务标签
        /// </summary>
        public string Tag => mTag;

        /// <summary>
        /// 下载任务优先级
        /// </summary>
        public int Priority => mPriority;

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public object UserData => mUserData;

        public override string ToString()
        {
            return
                $"{nameof(DownloadPath)}: {DownloadPath}, {nameof(DownloadUri)}: {DownloadUri}, {nameof(Tag)}: {Tag}, {nameof(Priority)}: {Priority}";
        }
    }
}