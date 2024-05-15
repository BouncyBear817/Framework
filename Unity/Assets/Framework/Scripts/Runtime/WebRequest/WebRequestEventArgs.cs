/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/5/13 16:44:8
 * Description:
 * Modify Record:
 *************************************************************/

using Framework;

namespace Runtime
{
    /// <summary>
    /// Web请求开始事件
    /// </summary>
    public sealed class WebRequestStartEventArgs : BaseEventArgs
    {
        public static int EventId = typeof(WebRequestStartEventArgs).GetHashCode();

        public WebRequestStartEventArgs()
        {
            SerialId = 0;
            WebRequestUri = null;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 任务序列编号
        /// </summary>
        public int SerialId { get; private set; }

        /// <summary>
        /// Web请求地址
        /// </summary>
        public string WebRequestUri { get; private set; }

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建Web请求开始事件
        /// </summary>
        /// <param name="e">内部事件</param>
        /// <returns>Web请求开始事件</returns>
        public static WebRequestStartEventArgs Create(Framework.WebRequestStartEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<WebRequestStartEventArgs>();
            eventArgs.SerialId = e.SerialId;
            eventArgs.WebRequestUri = e.WebRequestUri;
            eventArgs.UserData = e.UserData;
            return eventArgs;
        }

        /// <summary>
        /// 清理Web请求开始事件
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            WebRequestUri = null;
            UserData = null;
        }
    }

    /// <summary>
    /// Web请求成功事件
    /// </summary>
    public sealed class WebRequestSuccessEventArgs : BaseEventArgs
    {
        public static int EventId = typeof(WebRequestSuccessEventArgs).GetHashCode();

        public WebRequestSuccessEventArgs()
        {
            SerialId = 0;
            WebRequestUri = null;
            WebResponseBytes = null;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 任务序列编号
        /// </summary>
        public int SerialId { get; private set; }

        /// <summary>
        /// Web请求地址
        /// </summary>
        public string WebRequestUri { get; private set; }

        /// <summary>
        /// Web响应的数据流
        /// </summary>
        public byte[] WebResponseBytes { get; private set; }

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建Web请求成功事件
        /// </summary>
        /// <param name="e">内部事件</param>
        /// <returns>Web请求成功事件</returns>
        public static WebRequestSuccessEventArgs Create(Framework.WebRequestSuccessEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<WebRequestSuccessEventArgs>();
            eventArgs.SerialId = e.SerialId;
            eventArgs.WebRequestUri = e.WebRequestUri;
            eventArgs.WebResponseBytes = e.WebResponseBytes;
            eventArgs.UserData = e.UserData;
            return eventArgs;
        }

        /// <summary>
        /// 清理Web请求成功事件
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            WebRequestUri = null;
            WebResponseBytes = null;
            UserData = null;
        }
    }

    /// <summary>
    /// Web请求失败事件
    /// </summary>
    public sealed class WebRequestFailureEventArgs : BaseEventArgs
    {
        public static int EventId = typeof(WebRequestFailureEventArgs).GetHashCode();

        public WebRequestFailureEventArgs()
        {
            SerialId = 0;
            WebRequestUri = null;
            ErrorMessage = null;
            UserData = null;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 任务序列编号
        /// </summary>
        public int SerialId { get; private set; }

        /// <summary>
        /// Web请求地址
        /// </summary>
        public string WebRequestUri { get; private set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        public object UserData { get; private set; }

        /// <summary>
        /// 创建Web请求失败事件
        /// </summary>
        /// <param name="e">内部事件</param>
        /// <returns>Web请求失败事件</returns>
        public static WebRequestFailureEventArgs Create(Framework.WebRequestFailureEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<WebRequestFailureEventArgs>();
            eventArgs.SerialId = e.SerialId;
            eventArgs.WebRequestUri = e.WebRequestUri;
            eventArgs.ErrorMessage = e.ErrorMessage;
            eventArgs.UserData = e.UserData;
            return eventArgs;
        }

        /// <summary>
        /// 清理Web请求失败事件
        /// </summary>
        public override void Clear()
        {
            SerialId = 0;
            WebRequestUri = null;
            ErrorMessage = null;
            UserData = null;
        }
    }
}