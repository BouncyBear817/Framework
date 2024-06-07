// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/22 14:16:59
//  * Description:
//  * Modify Record:
//  *************************************************************/

namespace Framework
{
    /// <summary>
    /// 加载资源代理辅助器更新事件
    /// </summary>
    public sealed class LoadResourceAgentHelperUpdateEventArgs : FrameworkEventArgs
    {
        public LoadResourceAgentHelperUpdateEventArgs()
        {
            Type = LoadResourceProgress.Unknown;
            Progress = 0f;
        }

        /// <summary>
        /// 进度类型
        /// </summary>
        public LoadResourceProgress Type { get; private set; }

        /// <summary>
        /// 进度
        /// </summary>
        public float Progress { get; private set; }

        /// <summary>
        /// 创建加载资源代理辅助器更新事件
        /// </summary>
        /// <param name="type">进度类型</param>
        /// <param name="progress">进度</param>
        /// <returns>加载资源代理辅助器更新事件</returns>
        public static LoadResourceAgentHelperUpdateEventArgs Create(LoadResourceProgress type, float progress)
        {
            var eventArgs = ReferencePool.Acquire<LoadResourceAgentHelperUpdateEventArgs>();
            eventArgs.Type = type;
            eventArgs.Progress = progress;
            return eventArgs;
        }

        /// <summary>
        /// 清理加载资源代理辅助器更新事件
        /// </summary>
        public override void Clear()
        {
            Type = LoadResourceProgress.Unknown;
            Progress = 0f;
        }
    }

    /// <summary>
    /// 加载资源代理辅助器读取资源文件完成事件
    /// </summary>
    public sealed class LoadResourceAgentHelperReadFileCompleteEventArgs : FrameworkEventArgs
    {
        public LoadResourceAgentHelperReadFileCompleteEventArgs()
        {
            Resource = null;
        }

        /// <summary>
        /// 加载对象
        /// </summary>
        public object Resource { get; private set; }

        /// <summary>
        /// 创建加载资源代理辅助器读取资源文件完成事件
        /// </summary>
        /// <param name="resource">加载对象</param>
        /// <returns>加载资源代理辅助器读取资源文件完成事件</returns>
        public static LoadResourceAgentHelperReadFileCompleteEventArgs Create(object resource)
        {
            var eventArgs = ReferencePool.Acquire<LoadResourceAgentHelperReadFileCompleteEventArgs>();
            eventArgs.Resource = resource;
            return eventArgs;
        }

        /// <summary>
        /// 清理加载资源代理辅助器读取资源文件完成事件
        /// </summary>
        public override void Clear()
        {
            Resource = null;
        }
    }

    /// <summary>
    /// 加载资源代理辅助器读取资源二进制流完成事件
    /// </summary>
    public sealed class LoadResourceAgentHelperReadBytesCompleteEventArgs : FrameworkEventArgs
    {
        public LoadResourceAgentHelperReadBytesCompleteEventArgs()
        {
            Bytes = null;
        }

        /// <summary>
        /// 加载资源的二进制流
        /// </summary>
        public byte[] Bytes { get; private set; }

        /// <summary>
        /// 创建加载资源代理辅助器读取资源二进制流完成事件
        /// </summary>
        /// <param name="bytes">资源的二进制流</param>
        /// <returns>加载资源代理辅助器读取资源二进制流完成事件</returns>
        public static LoadResourceAgentHelperReadBytesCompleteEventArgs Create(byte[] bytes)
        {
            var eventArgs = ReferencePool.Acquire<LoadResourceAgentHelperReadBytesCompleteEventArgs>();
            eventArgs.Bytes = bytes;
            return eventArgs;
        }

        /// <summary>
        /// 清理加载资源代理辅助器读取资源二进制流完成事件
        /// </summary>
        public override void Clear()
        {
            Bytes = null;
        }
    }

    /// <summary>
    /// 加载资源代理辅助器将资源二进制流转换为加载对象事件
    /// </summary>
    public sealed class LoadResourceAgentHelperParseBytesCompleteEventArgs : FrameworkEventArgs
    {
        public LoadResourceAgentHelperParseBytesCompleteEventArgs()
        {
            Resource = null;
        }

        /// <summary>
        /// 加载对象
        /// </summary>
        public object Resource { get; private set; }

        /// <summary>
        /// 创建加载资源代理辅助器将资源二进制流转换为加载对象事件
        /// </summary>
        /// <param name="resource">加载对象</param>
        /// <returns>加载资源代理辅助器将资源二进制流转换为加载对象事件</returns>
        public static LoadResourceAgentHelperParseBytesCompleteEventArgs Create(object resource)
        {
            var eventArgs = ReferencePool.Acquire<LoadResourceAgentHelperParseBytesCompleteEventArgs>();
            eventArgs.Resource = resource;
            return eventArgs;
        }

        /// <summary>
        /// 清理加载资源代理辅助器将资源二进制流转换为加载对象事件
        /// </summary>
        public override void Clear()
        {
            Resource = null;
        }
    }

    /// <summary>
    /// 加载资源代理辅助器加载资源完成事件
    /// </summary>
    public sealed class LoadResourceAgentHelperLoadCompleteEventArgs : FrameworkEventArgs
    {
        public LoadResourceAgentHelperLoadCompleteEventArgs()
        {
            Asset = null;
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        public object Asset { get; private set; }

        /// <summary>
        /// 创建加载资源代理辅助器加载资源完成事件
        /// </summary>
        /// <param name="resource">加载资源</param>
        /// <returns>加载资源代理辅助器加载资源完成事件</returns>
        public static LoadResourceAgentHelperLoadCompleteEventArgs Create(object resource)
        {
            var eventArgs = ReferencePool.Acquire<LoadResourceAgentHelperLoadCompleteEventArgs>();
            eventArgs.Asset = resource;
            return eventArgs;
        }

        /// <summary>
        /// 清理加载资源代理辅助器加载资源完成事件
        /// </summary>
        public override void Clear()
        {
            Asset = null;
        }
    }

    /// <summary>
    /// 加载资源代理辅助器错误事件
    /// </summary>
    public sealed class LoadResourceAgentHelperErrorEventArgs : FrameworkEventArgs
    {
        public LoadResourceAgentHelperErrorEventArgs()
        {
            Status = LoadResourceStatus.Success;
            ErrorMessage = null;
        }

        /// <summary>
        /// 加载资源状态
        /// </summary>
        public LoadResourceStatus Status { get; private set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// 创建加载资源代理辅助器错误事件
        /// </summary>
        /// <param name="status"></param>
        /// <param name="errorMessage"></param>
        /// <returns>加载资源代理辅助器错误事件</returns>
        public static LoadResourceAgentHelperErrorEventArgs Create(LoadResourceStatus status, string errorMessage)
        {
            var eventArgs = ReferencePool.Acquire<LoadResourceAgentHelperErrorEventArgs>();
            eventArgs.Status = status;
            eventArgs.ErrorMessage = errorMessage;
            return eventArgs;
        }

        /// <summary>
        /// 清理加载资源代理辅助器错误事件
        /// </summary>
        public override void Clear()
        {
            Status = LoadResourceStatus.Success;
            ErrorMessage = null;
        }
    }
}