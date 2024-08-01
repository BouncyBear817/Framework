/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/01/30 16:12:05
 * Description:   
 * Modify Record: 
 *************************************************************/

using Framework;

namespace Framework.Runtime
{
    /// <summary>
    /// 下载开始事件
    /// </summary>
    public sealed class DownloadStartEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(DownloadStartEventArgs).GetHashCode();

        public DownloadStartEventArgs()
        {
            SerialId = 0;
            DownloadPath = null;
            DownloadUri = null;
            CurrentLength = 0L;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 下载任务的序列编号
        /// </summary>
        public int SerialId { get; private set; }

        /// <summary>
        /// 下载任务的存放地址
        /// </summary>
        public string DownloadPath { get; private set; }

        /// <summary>
        /// 下载地址
        /// </summary>
        public string DownloadUri { get; private set; }

        /// <summary>
        /// 下载任务的大小
        /// </summary>
        public long CurrentLength { get; private set; }

        /// <summary>
        /// 下载任务的自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建下载开始事件
        /// </summary>
        /// <param name="e">内部事件</param>
        /// <returns>下载开始事件</returns>
        public static DownloadStartEventArgs Create(Framework.DownloadStartEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<DownloadStartEventArgs>();
            eventArgs.SerialId = e.SerialId;
            eventArgs.DownloadPath = e.DownloadPath;
            eventArgs.DownloadUri = e.DownloadUri;
            eventArgs.CurrentLength = e.CurrentLength;
            eventArgs.UserData = e.UserData;
            return eventArgs;
        }

        /// <summary>
        /// 清除下载开始事件
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            DownloadPath = null;
            DownloadUri = null;
            CurrentLength = 0L;
            UserData = null;
        }
    }

    /// <summary>
    /// 下载更新事件
    /// </summary>
    public sealed class DownloadUpdateEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(DownloadUpdateEventArgs).GetHashCode();

        public DownloadUpdateEventArgs()
        {
            SerialId = 0;
            DownloadPath = null;
            DownloadUri = null;
            CurrentLength = 0L;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 下载任务的序列编号
        /// </summary>
        public int SerialId { get; private set; }

        /// <summary>
        /// 下载任务的存放地址
        /// </summary>
        public string DownloadPath { get; private set; }

        /// <summary>
        /// 下载地址
        /// </summary>
        public string DownloadUri { get; private set; }

        /// <summary>
        /// 下载任务的大小
        /// </summary>
        public long CurrentLength { get; private set; }

        /// <summary>
        /// 下载任务的自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建下载更新事件
        /// </summary>
        /// <param name="e">内部事件</param>
        /// <returns>下载更新事件</returns>
        public static DownloadUpdateEventArgs Create(Framework.DownloadUpdateEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<DownloadUpdateEventArgs>();
            eventArgs.SerialId = e.SerialId;
            eventArgs.DownloadPath = e.DownloadPath;
            eventArgs.DownloadUri = e.DownloadUri;
            eventArgs.CurrentLength = e.CurrentLength;
            eventArgs.UserData = e.UserData;
            return eventArgs;
        }

        /// <summary>
        /// 清除下载更新事件
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            DownloadPath = null;
            DownloadUri = null;
            CurrentLength = 0L;
            UserData = null;
        }
    }

    /// <summary>
    /// 下载成功事件
    /// </summary>
    public sealed class DownloadSuccessEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(DownloadSuccessEventArgs).GetHashCode();

        public DownloadSuccessEventArgs()
        {
            SerialId = 0;
            DownloadPath = null;
            DownloadUri = null;
            CurrentLength = 0L;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 下载任务的序列编号
        /// </summary>
        public int SerialId { get; private set; }

        /// <summary>
        /// 下载任务的存放地址
        /// </summary>
        public string DownloadPath { get; private set; }

        /// <summary>
        /// 下载地址
        /// </summary>
        public string DownloadUri { get; private set; }

        /// <summary>
        /// 下载任务的大小
        /// </summary>
        public long CurrentLength { get; private set; }

        /// <summary>
        /// 下载任务的自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建下载成功事件
        /// </summary>
        /// <param name="e">内部事件</param>
        /// <returns>下载成功事件</returns>
        public static DownloadSuccessEventArgs Create(Framework.DownloadSuccessEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<DownloadSuccessEventArgs>();
            eventArgs.SerialId = e.SerialId;
            eventArgs.DownloadPath = e.DownloadPath;
            eventArgs.DownloadUri = e.DownloadUri;
            eventArgs.CurrentLength = e.CurrentLength;
            eventArgs.UserData = e.UserData;
            return eventArgs;
        }

        /// <summary>
        /// 清除下载成功事件
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            DownloadPath = null;
            DownloadUri = null;
            CurrentLength = 0L;
            UserData = null;
        }
    }

    /// <summary>
    /// 下载失败事件
    /// </summary>
    public sealed class DownloadFailureEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(DownloadFailureEventArgs).GetHashCode();

        public DownloadFailureEventArgs()
        {
            SerialId = 0;
            DownloadPath = null;
            DownloadUri = null;
            ErrorMessage = null;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 下载任务的序列编号
        /// </summary>
        public int SerialId { get; private set; }

        /// <summary>
        /// 下载任务的存放地址
        /// </summary>
        public string DownloadPath { get; private set; }

        /// <summary>
        /// 下载地址
        /// </summary>
        public string DownloadUri { get; private set; }

        /// <summary>
        /// 下载任务的错误信息
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// 下载任务的自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建下载失败事件
        /// </summary>
        /// <param name="e">内部事件</param>
        /// <returns>下载失败事件</returns>
        public static DownloadFailureEventArgs Create(Framework.DownloadFailureEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<DownloadFailureEventArgs>();
            eventArgs.SerialId = e.SerialId;
            eventArgs.DownloadPath = e.DownloadPath;
            eventArgs.DownloadUri = e.DownloadUri;
            eventArgs.ErrorMessage = e.ErrorMessage;
            eventArgs.UserData = e.UserData;
            return eventArgs;
        }

        /// <summary>
        /// 清除下载失败事件
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            DownloadPath = null;
            DownloadUri = null;
            ErrorMessage = null;
            UserData = null;
        }
    }
}