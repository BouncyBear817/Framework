/************************************************************
 * Unity Version: 2022.3.0f1c1
 * Author:        bear
 * CreateTime:    2024/01/05 11:21:26
 * Description:
 * Modify Record:
 *************************************************************/

using Framework;

namespace Runtime
{
    /// <summary>
    /// 资源更新开始事件
    /// </summary>
    public class ResourceUpdateStartEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(ResourceUpdateStartEventArgs).GetHashCode();

        public ResourceUpdateStartEventArgs()
        {
            Name = null;
            DownloadPath = null;
            DownloadUri = null;
            CurrentLength = 0;
            CompressedLength = 0;
            RetryCount = 0;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 资源下载后存放路径
        /// </summary>
        public string DownloadPath { get; private set; }

        /// <summary>
        /// 资源下载地址
        /// </summary>
        public string DownloadUri { get; private set; }

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
        /// <param name="e">内部事件</param>
        /// <returns>资源更新开始事件</returns>
        public static ResourceUpdateStartEventArgs Create(Framework.ResourceUpdateStartEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<ResourceUpdateStartEventArgs>();
            eventArgs.Name = e.Name;
            eventArgs.DownloadUri = e.DownloadUri;
            eventArgs.DownloadPath = e.DownloadPath;
            eventArgs.CurrentLength = e.CurrentLength;
            eventArgs.CompressedLength = e.CompressedLength;
            eventArgs.RetryCount = e.RetryCount;
            return eventArgs;
        }

        /// <summary>
        /// 清除资源更新开始事件
        /// </summary>
        public override void Clear()
        {
            Name = null;
            DownloadPath = null;
            DownloadUri = null;
            CurrentLength = 0;
            CompressedLength = 0;
            RetryCount = 0;
        }
    }

    /// <summary>
    /// 资源更新改变事件
    /// </summary>
    public class ResourceUpdateChangedEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(ResourceUpdateChangedEventArgs).GetHashCode();

        public ResourceUpdateChangedEventArgs()
        {
            Name = null;
            DownloadPath = null;
            DownloadUri = null;
            CurrentLength = 0;
            CompressedLength = 0;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 资源下载后存放路径
        /// </summary>
        public string DownloadPath { get; private set; }

        /// <summary>
        /// 资源下载地址
        /// </summary>
        public string DownloadUri { get; private set; }

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
        /// <param name="e">内部事件</param>
        /// <returns>资源更新改变事件</returns>
        public static ResourceUpdateChangedEventArgs Create(Framework.ResourceUpdateChangedEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<ResourceUpdateChangedEventArgs>();
            eventArgs.Name = e.Name;
            eventArgs.DownloadUri = e.DownloadUri;
            eventArgs.DownloadPath = e.DownloadPath;
            eventArgs.CurrentLength = e.CurrentLength;
            eventArgs.CompressedLength = e.CompressedLength;
            return eventArgs;
        }

        /// <summary>
        /// 清除资源更新改变事件
        /// </summary>
        public override void Clear()
        {
            Name = null;
            DownloadPath = null;
            DownloadUri = null;
            CurrentLength = 0;
            CompressedLength = 0;
        }
    }

    /// <summary>
    /// 资源更新成功事件
    /// </summary>
    public class ResourceUpdateSuccessEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(ResourceUpdateSuccessEventArgs).GetHashCode();

        public ResourceUpdateSuccessEventArgs()
        {
            Name = null;
            DownloadPath = null;
            DownloadUri = null;
            Length = 0;
            CompressedLength = 0;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 资源下载后存放路径
        /// </summary>
        public string DownloadPath { get; private set; }

        /// <summary>
        /// 资源下载地址
        /// </summary>
        public string DownloadUri { get; private set; }

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
        /// <param name="e">内部事件</param>
        /// <returns>资源更新成功事件</returns>
        public static ResourceUpdateSuccessEventArgs Create(Framework.ResourceUpdateSuccessEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<ResourceUpdateSuccessEventArgs>();
            eventArgs.Name = e.Name;
            eventArgs.DownloadUri = e.DownloadUri;
            eventArgs.DownloadPath = e.DownloadPath;
            eventArgs.Length = e.Length;
            eventArgs.CompressedLength = e.CompressedLength;
            return eventArgs;
        }

        /// <summary>
        /// 清除资源更新成功事件
        /// </summary>
        public override void Clear()
        {
            Name = null;
            DownloadPath = null;
            DownloadUri = null;
            Length = 0;
            CompressedLength = 0;
        }
    }

    /// <summary>
    /// 资源更新失败事件
    /// </summary>
    public class ResourceUpdateFailureEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(ResourceUpdateFailureEventArgs).GetHashCode();

        public ResourceUpdateFailureEventArgs()
        {
            Name = null;
            DownloadUri = null;
            RetryCount = 0;
            TotalRetryCount = 0;
            ErrorMessage = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 资源下载地址
        /// </summary>
        public string DownloadUri { get; private set; }

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
        /// <param name="e">内部事件</param>
        /// <returns>资源更新失败事件</returns>
        public static ResourceUpdateFailureEventArgs Create(Framework.ResourceUpdateFailureEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<ResourceUpdateFailureEventArgs>();
            eventArgs.Name = e.Name;
            eventArgs.DownloadUri = e.DownloadUri;
            eventArgs.RetryCount = e.RetryCount;
            eventArgs.TotalRetryCount = e.TotalRetryCount;
            eventArgs.ErrorMessage = e.ErrorMessage;
            return eventArgs;
        }

        /// <summary>
        /// 清除资源更新失败事件
        /// </summary>
        public override void Clear()
        {
            Name = null;
            DownloadUri = null;
            RetryCount = 0;
            TotalRetryCount = 0;
            ErrorMessage = null;
        }
    }

    /// <summary>
    /// 资源更新全部完成事件
    /// </summary>
    public class ResourceUpdateAllCompleteEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(ResourceUpdateAllCompleteEventArgs).GetHashCode();

        public ResourceUpdateAllCompleteEventArgs()
        {
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 创建资源更新全部完成事件
        /// </summary>
        /// <param name="e">内部事件</param>
        /// <returns>资源更新全部完成事件</returns>
        public static ResourceUpdateAllCompleteEventArgs Create(Framework.ResourceUpdateAllCompleteEventArgs e)
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