/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/01/19 11:15:53
 * Description:   
 * Modify Record: 
 *************************************************************/

namespace Framework
{
    /// <summary>
    /// 下载开始事件
    /// </summary>
    public sealed class DownloadStartEventArgs : FrameworkEventArgs
    {
        public DownloadStartEventArgs()
        {
            SerialId = 0;
            DownloadPath = null;
            DownloadUri = null;
            CurrentLength = 0L;
            UserData = null;
        }

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
        /// <param name="serialId">下载任务的序列编号</param>
        /// <param name="downloadPath">下载任务的存放地址</param>
        /// <param name="downloadUri">下载地址</param>
        /// <param name="currentLength">下载任务的大小</param>
        /// <param name="userData">下载任务的自定义数据</param>
        /// <returns>下载开始事件</returns>
        public static DownloadStartEventArgs Create(int serialId, string downloadPath, string downloadUri, long currentLength, object userData)
        {
            var eventArgs = ReferencePool.Acquire<DownloadStartEventArgs>();
            eventArgs.SerialId = serialId;
            eventArgs.DownloadPath = downloadPath;
            eventArgs.DownloadUri = downloadUri;
            eventArgs.CurrentLength = currentLength;
            eventArgs.UserData = userData;
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
    public sealed class DownloadUpdateEventArgs : FrameworkEventArgs
    {
        public DownloadUpdateEventArgs()
        {
            SerialId = 0;
            DownloadPath = null;
            DownloadUri = null;
            CurrentLength = 0L;
            UserData = null;
        }

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
        /// <param name="serialId">下载任务的序列编号</param>
        /// <param name="downloadPath">下载任务的存放地址</param>
        /// <param name="downloadUri">下载地址</param>
        /// <param name="currentLength">下载任务的大小</param>
        /// <param name="userData">下载任务的自定义数据</param>
        /// <returns>下载更新事件</returns>
        public static DownloadUpdateEventArgs Create(int serialId, string downloadPath, string downloadUri, long currentLength, object userData)
        {
            var eventArgs = ReferencePool.Acquire<DownloadUpdateEventArgs>();
            eventArgs.SerialId = serialId;
            eventArgs.DownloadPath = downloadPath;
            eventArgs.DownloadUri = downloadUri;
            eventArgs.CurrentLength = currentLength;
            eventArgs.UserData = userData;
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
    public sealed class DownloadSuccessEventArgs : FrameworkEventArgs
    {
        public DownloadSuccessEventArgs()
        {
            SerialId = 0;
            DownloadPath = null;
            DownloadUri = null;
            CurrentLength = 0L;
            UserData = null;
        }

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
        /// <param name="serialId">下载任务的序列编号</param>
        /// <param name="downloadPath">下载任务的存放地址</param>
        /// <param name="downloadUri">下载地址</param>
        /// <param name="currentLength">下载任务的大小</param>
        /// <param name="userData">下载任务的自定义数据</param>
        /// <returns>下载成功事件</returns>
        public static DownloadSuccessEventArgs Create(int serialId, string downloadPath, string downloadUri, long currentLength, object userData)
        {
            var eventArgs = ReferencePool.Acquire<DownloadSuccessEventArgs>();
            eventArgs.SerialId = serialId;
            eventArgs.DownloadPath = downloadPath;
            eventArgs.DownloadUri = downloadUri;
            eventArgs.CurrentLength = currentLength;
            eventArgs.UserData = userData;
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
    public sealed class DownloadFailureEventArgs : FrameworkEventArgs
    {
        public DownloadFailureEventArgs()
        {
            SerialId = 0;
            DownloadPath = null;
            DownloadUri = null;
            ErrorMessage = null;
            UserData = null;
        }

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
        /// <param name="serialId">下载任务的序列编号</param>
        /// <param name="downloadPath">下载任务的存放地址</param>
        /// <param name="downloadUri">下载地址</param>
        /// <param name="errorMessage">下载任务的错误信息</param>
        /// <param name="userData">下载任务的自定义数据</param>
        /// <returns>下载失败事件</returns>
        public static DownloadFailureEventArgs Create(int serialId, string downloadPath, string downloadUri, string errorMessage, object userData)
        {
            var eventArgs = ReferencePool.Acquire<DownloadFailureEventArgs>();
            eventArgs.SerialId = serialId;
            eventArgs.DownloadPath = downloadPath;
            eventArgs.DownloadUri = downloadUri;
            eventArgs.ErrorMessage = errorMessage;
            eventArgs.UserData = userData;
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