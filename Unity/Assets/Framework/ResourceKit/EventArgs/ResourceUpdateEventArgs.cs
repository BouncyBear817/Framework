namespace Framework
{
    /// <summary>
    /// 资源更新开始事件
    /// </summary>
    public class ResourceUpdateStartEventArgs : FrameworkEventArgs
    {
        public ResourceUpdateStartEventArgs()
        {
            Name = null;
            DownloadUrl = null;
            DownloadPath = null;
            CurrentLength = 0;
            CompressedLength = 0;
            RetryCount = 0;
        }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 资源下载地址
        /// </summary>
        public string DownloadUrl { get; private set; }

        /// <summary>
        /// 资源下载后存放路径
        /// </summary>
        public string DownloadPath { get; private set; }

        /// <summary>
        /// 当前下载大小
        /// </summary>
        public int CurrentLength { get; private set; }

        /// <summary>
        /// 压缩后大小
        /// </summary>
        public int CompressedLength { get; private set; }

        /// <summary>
        /// 已重试下载次数
        /// </summary>
        public int RetryCount { get; private set; }

        /// <summary>
        /// 创建资源更新开始事件
        /// </summary>
        /// <param name="name">资源名称</param>
        /// <param name="downloadUrl">资源下载地址</param>
        /// <param name="downloadPath">资源下载后存放路径</param>
        /// <param name="currentLength">当前下载大小</param>
        /// <param name="compressLength">压缩后大小</param>
        /// <param name="retryCount">已重试下载次数</param>
        /// <returns>资源更新开始事件</returns>
        public static ResourceUpdateStartEventArgs Create(string name, string downloadUrl, string downloadPath,
            int currentLength, int compressLength, int retryCount)
        {
            var eventArgs = ReferencePool.Acquire<ResourceUpdateStartEventArgs>();
            eventArgs.Name = name;
            eventArgs.DownloadUrl = downloadUrl;
            eventArgs.DownloadPath = downloadPath;
            eventArgs.CurrentLength = currentLength;
            eventArgs.CompressedLength = compressLength;
            eventArgs.RetryCount = retryCount;
            return eventArgs;
        }

        /// <summary>
        /// 清除资源更新开始事件
        /// </summary>
        public override void Clear()
        {
            Name = null;
            DownloadUrl = null;
            DownloadPath = null;
            CurrentLength = 0;
            CompressedLength = 0;
            RetryCount = 0;
        }
    }

    /// <summary>
    /// 资源更新改变事件
    /// </summary>
    public class ResourceUpdateChangedEventArgs : FrameworkEventArgs
    {
        public ResourceUpdateChangedEventArgs()
        {
            Name = null;
            DownloadUrl = null;
            DownloadPath = null;
            CurrentLength = 0;
            CompressedLength = 0;
        }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 资源下载地址
        /// </summary>
        public string DownloadUrl { get; private set; }

        /// <summary>
        /// 资源下载后存放路径
        /// </summary>
        public string DownloadPath { get; private set; }

        /// <summary>
        /// 当前下载大小
        /// </summary>
        public int CurrentLength { get; private set; }

        /// <summary>
        /// 压缩后大小
        /// </summary>
        public int CompressedLength { get; private set; }

        /// <summary>
        /// 创建资源更新改变事件
        /// </summary>
        /// <param name="name">资源名称</param>
        /// <param name="downloadUrl">资源下载地址</param>
        /// <param name="downloadPath">资源下载后存放路径</param>
        /// <param name="currentLength">当前下载大小</param>
        /// <param name="compressLength">压缩后大小</param>
        /// <returns>资源更新改变事件</returns>
        public static ResourceUpdateChangedEventArgs Create(string name, string downloadUrl, string downloadPath,
            int currentLength, int compressLength)
        {
            var eventArgs = ReferencePool.Acquire<ResourceUpdateChangedEventArgs>();
            eventArgs.Name = name;
            eventArgs.DownloadUrl = downloadUrl;
            eventArgs.DownloadPath = downloadPath;
            eventArgs.CurrentLength = currentLength;
            eventArgs.CompressedLength = compressLength;
            return eventArgs;
        }

        /// <summary>
        /// 清除资源更新改变事件
        /// </summary>
        public override void Clear()
        {
            Name = null;
            DownloadUrl = null;
            DownloadPath = null;
            CurrentLength = 0;
            CompressedLength = 0;
        }
    }

    /// <summary>
    /// 资源更新成功事件
    /// </summary>
    public class ResourceUpdateSuccessEventArgs : FrameworkEventArgs
    {
        public ResourceUpdateSuccessEventArgs()
        {
            Name = null;
            DownloadUrl = null;
            DownloadPath = null;
            Length = 0;
            CompressedLength = 0;
        }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 资源下载地址
        /// </summary>
        public string DownloadUrl { get; private set; }

        /// <summary>
        /// 资源下载后存放路径
        /// </summary>
        public string DownloadPath { get; private set; }

        /// <summary>
        /// 资源大小
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// 压缩后大小
        /// </summary>
        public int CompressedLength { get; private set; }

        /// <summary>
        /// 创建资源更新成功事件
        /// </summary>
        /// <param name="name">资源名称</param>
        /// <param name="downloadUrl">资源下载地址</param>
        /// <param name="downloadPath">资源下载后存放路径</param>
        /// <param name="length">资源大小</param>
        /// <param name="compressLength">压缩后大小</param>
        /// <returns>资源更新成功事件</returns>
        public static ResourceUpdateSuccessEventArgs Create(string name, string downloadUrl, string downloadPath,
            int length, int compressLength)
        {
            var eventArgs = ReferencePool.Acquire<ResourceUpdateSuccessEventArgs>();
            eventArgs.Name = name;
            eventArgs.DownloadUrl = downloadUrl;
            eventArgs.DownloadPath = downloadPath;
            eventArgs.Length = length;
            eventArgs.CompressedLength = compressLength;
            return eventArgs;
        }

        /// <summary>
        /// 清除资源更新成功事件
        /// </summary>
        public override void Clear()
        {
            Name = null;
            DownloadUrl = null;
            DownloadPath = null;
            Length = 0;
            CompressedLength = 0;
        }
    }

    /// <summary>
    /// 资源更新失败事件
    /// </summary>
    public class ResourceUpdateFailureEventArgs : FrameworkEventArgs
    {
        public ResourceUpdateFailureEventArgs()
        {
            Name = null;
            DownloadUrl = null;
            RetryCount = 0;
            TotalRetryCount = 0;
            ErrorMessage = null;
        }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 资源下载地址
        /// </summary>
        public string DownloadUrl { get; private set; }

        /// <summary>
        /// 已重试次数
        /// </summary>
        public int RetryCount { get; private set; }

        /// <summary>
        /// 设定的重试次数
        /// </summary>
        public int TotalRetryCount { get; private set; }

        /// <summary>
        /// 失败信息
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// 创建资源更新失败事件
        /// </summary>
        /// <param name="name">资源名称</param>
        /// <param name="downloadUrl">资源下载地址</param>
        /// <param name="retryCount">已重试次数</param>
        /// <param name="totalRetryCount">设定的重试次数</param>
        /// <param name="errorMessage">失败信息</param>
        /// <returns>资源更新失败事件</returns>
        public static ResourceUpdateFailureEventArgs Create(string name, string downloadUrl, int retryCount,
            int totalRetryCount, string errorMessage)
        {
            var eventArgs = ReferencePool.Acquire<ResourceUpdateFailureEventArgs>();
            eventArgs.Name = name;
            eventArgs.DownloadUrl = downloadUrl;
            eventArgs.RetryCount = retryCount;
            eventArgs.TotalRetryCount = totalRetryCount;
            eventArgs.ErrorMessage = errorMessage;
            return eventArgs;
        }

        /// <summary>
        /// 清除资源更新失败事件
        /// </summary>
        public override void Clear()
        {
            Name = null;
            DownloadUrl = null;
            RetryCount = 0;
            TotalRetryCount = 0;
            ErrorMessage = null;
        }
    }

    /// <summary>
    /// 资源更新全部完成事件
    /// </summary>
    public class ResourceUpdateAllCompleteEventArgs : FrameworkEventArgs
    {
        public ResourceUpdateAllCompleteEventArgs()
        {
        }

        /// <summary>
        /// 创建资源更新全部完成事件
        /// </summary>
        /// <returns>资源更新全部完成事件</returns>
        public static ResourceUpdateAllCompleteEventArgs Create()
        {
            return ReferencePool.Acquire<ResourceUpdateAllCompleteEventArgs>();
        }

        /// <summary>
        /// 清除资源更新全部完成事件
        /// </summary>
        public override void Clear()
        {
        }
    }
}