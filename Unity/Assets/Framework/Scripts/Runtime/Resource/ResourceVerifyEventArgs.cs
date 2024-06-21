/************************************************************
 * Unity Version: 2022.3.0f1c1
 * Author:        bear
 * CreateTime:    2024/01/05 11:21:26
 * Description:
 * Modify Record:
 *************************************************************/

using Framework;

namespace Framework.Runtime
{
    /// <summary>
    /// 资源校验开始事件
    /// </summary>
    public class ResourceVerifyStartEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(ResourceVerifyStartEventArgs).GetHashCode();

        public ResourceVerifyStartEventArgs()
        {
            Count = 0;
            TotalLength = 0L;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 要校验的资源数量
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// 要校验的资源总大小
        /// </summary>
        public long TotalLength { get; private set; }

        /// <summary>
        /// 创建资源校验开始事件
        /// </summary>
        /// <param name="e">内部事件</param>
        /// <returns>资源校验开始事件</returns>
        public static ResourceVerifyStartEventArgs Create(Framework.ResourceVerifyStartEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<ResourceVerifyStartEventArgs>();
            eventArgs.Count = e.Count;
            eventArgs.TotalLength = e.TotalLength;
            return eventArgs;
        }

        /// <summary>
        /// 清除资源校验开始事件
        /// </summary>
        public override void Clear()
        {
            Count = 0;
            TotalLength = 0L;
        }
    }

    /// <summary>
    /// 资源校验成功事件
    /// </summary>
    public class ResourceVerifySuccessEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(ResourceVerifySuccessEventArgs).GetHashCode();

        public ResourceVerifySuccessEventArgs()
        {
            Name = null;
            Length = 0;
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
        /// 资源大小
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// 创建资源校验成功事件
        /// </summary>
        /// <param name="e">内部事件</param>
        /// <returns>资源校验成功事件</returns>
        public static ResourceVerifySuccessEventArgs Create(Framework.ResourceVerifySuccessEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<ResourceVerifySuccessEventArgs>();
            eventArgs.Name = e.Name;
            eventArgs.Length = e.Length;
            return eventArgs;
        }

        /// <summary>
        /// 清除资源校验成功事件
        /// </summary>
        public override void Clear()
        {
            Name = null;
            Length = 0;
        }
    }

    /// <summary>
    /// 资源校验失败事件
    /// </summary>
    public class ResourceVerifyFailureEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(ResourceVerifyFailureEventArgs).GetHashCode();

        public ResourceVerifyFailureEventArgs()
        {
            Name = null;
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
        /// 创建资源校验失败事件
        /// </summary>
        /// <param name="e">内部事件</param>
        /// <returns>资源校验失败事件</returns>
        public static ResourceVerifyFailureEventArgs Create(Framework.ResourceVerifyFailureEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<ResourceVerifyFailureEventArgs>();
            eventArgs.Name = e.Name;
            return eventArgs;
        }

        /// <summary>
        /// 清除资源校验失败事件
        /// </summary>
        public override void Clear()
        {
            Name = null;
        }
    }
}