/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/5/13 11:18:18
 * Description:
 * Modify Record:
 *************************************************************/

namespace Framework
{
    /// <summary>
    /// Web请求代理辅助器完成事件
    /// </summary>
    public sealed class WebRequestAgentHelperCompleteEventArgs : FrameworkEventArgs
    {
        public WebRequestAgentHelperCompleteEventArgs()
        {
            WebResponseBytes = null;
        }

        /// <summary>
        /// Web响应的数据流
        /// </summary>
        public byte[] WebResponseBytes { get; private set; }

        /// <summary>
        /// 创建Web请求代理辅助器完成事件
        /// </summary>
        /// <param name="webResponseBytes">Web响应的数据流</param>
        /// <returns>Web请求代理辅助器完成事件</returns>
        public static WebRequestAgentHelperCompleteEventArgs Create(byte[] webResponseBytes)
        {
            var eventArgs = ReferencePool.Acquire<WebRequestAgentHelperCompleteEventArgs>();
            eventArgs.WebResponseBytes = webResponseBytes;
            return eventArgs;
        }

        /// <summary>
        /// 清理Web请求代理辅助器完成事件
        /// </summary>
        public override void Clear()
        {
            WebResponseBytes = null;
        }
    }

    /// <summary>
    /// Web请求代理辅助器错误事件
    /// </summary>
    public sealed class WebRequestAgentHelperErrorEventArgs : FrameworkEventArgs
    {
        public WebRequestAgentHelperErrorEventArgs()
        {
            ErrorMessage = null;
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// 创建Web请求代理辅助器错误事件
        /// </summary>
        /// <param name="errorMessage">Web请求错误信息</param>
        /// <returns>Web请求代理辅助器错误事件</returns>
        public static WebRequestAgentHelperErrorEventArgs Create(string errorMessage)
        {
            var eventArgs = ReferencePool.Acquire<WebRequestAgentHelperErrorEventArgs>();
            eventArgs.ErrorMessage = errorMessage;
            return eventArgs;
        }

        /// <summary>
        /// 清理Web请求代理辅助器错误事件
        /// </summary>
        public override void Clear()
        {
            ErrorMessage = null;
        }
    }

    /// <summary>
    /// Web请求开始事件
    /// </summary>
    public sealed class WebRequestStartEventArgs : FrameworkEventArgs
    {
        public WebRequestStartEventArgs()
        {
            SerialId = 0;
            WebRequestUri = null;
            UserData = null;
        }

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
        /// <param name="serialId">任务序列编号</param>
        /// <param name="webRequestUri">Web请求地址</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>Web请求开始事件</returns>
        public static WebRequestStartEventArgs Create(int serialId, string webRequestUri, object userData)
        {
            var eventArgs = ReferencePool.Acquire<WebRequestStartEventArgs>();
            eventArgs.SerialId = serialId;
            eventArgs.WebRequestUri = webRequestUri;
            eventArgs.UserData = userData;
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
    public sealed class WebRequestSuccessEventArgs : FrameworkEventArgs
    {
        public WebRequestSuccessEventArgs()
        {
            SerialId = 0;
            WebRequestUri = null;
            WebResponseBytes = null;
            UserData = null;
        }

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
        /// <param name="serialId">任务序列编号</param>
        /// <param name="webRequestUri">Web请求地址</param>
        /// <param name="webResponseBytes">Web响应的数据流</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>Web请求成功事件</returns>
        public static WebRequestSuccessEventArgs Create(int serialId, string webRequestUri, byte[] webResponseBytes,
            object userData)
        {
            var eventArgs = ReferencePool.Acquire<WebRequestSuccessEventArgs>();
            eventArgs.SerialId = serialId;
            eventArgs.WebRequestUri = webRequestUri;
            eventArgs.WebResponseBytes = webResponseBytes;
            eventArgs.UserData = userData;
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
    public sealed class WebRequestFailureEventArgs : FrameworkEventArgs
    {
        public WebRequestFailureEventArgs()
        {
            SerialId = 0;
            WebRequestUri = null;
            ErrorMessage = null;
            UserData = null;
        }

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
        /// <param name="serialId">任务序列编号</param>
        /// <param name="webRequestUri">Web请求地址</param>
        /// <param name="errorMessage">错误信息</param>
        /// <param name="userData">用户自定义数据</param>
        /// <returns>Web请求失败事件</returns>
        public static WebRequestFailureEventArgs Create(int serialId, string webRequestUri, string errorMessage,
            object userData)
        {
            var eventArgs = ReferencePool.Acquire<WebRequestFailureEventArgs>();
            eventArgs.SerialId = serialId;
            eventArgs.WebRequestUri = webRequestUri;
            eventArgs.ErrorMessage = errorMessage;
            eventArgs.UserData = userData;
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