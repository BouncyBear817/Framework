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
    /// 资源应用开始事件
    /// </summary>
    public class ResourceApplyStartEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(ResourceApplyStartEventArgs).GetHashCode();

        public ResourceApplyStartEventArgs()
        {
            ResourcePackPath = null;
            Count = 0;
            TotalLength = 0;
        }

        /// <summary>
        /// 事件编号
        /// </summary>
        public override int Id => EventId;

        /// <summary>
        /// 资源包路径
        /// </summary>
        public string ResourcePackPath { get; private set; }

        /// <summary>
        /// 应用资源数量
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// 应用资源总大小
        /// </summary>
        public long TotalLength { get; private set; }

        /// <summary>
        /// 创建资源应用开始事件
        /// </summary>
        /// <param name="e">内部事件</param>
        /// <returns>资源应用开始事件</returns>
        public static ResourceApplyStartEventArgs Create(Framework.ResourceApplyStartEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<ResourceApplyStartEventArgs>();
            eventArgs.ResourcePackPath = e.ResourcePackPath;
            eventArgs.Count = e.Count;
            eventArgs.TotalLength = e.TotalLength;
            return eventArgs;
        }

        /// <summary>
        /// 清除资源应用开始事件
        /// </summary>
        public override void Clear()
        {
            ResourcePackPath = null;
            Count = 0;
            TotalLength = 0;
        }
    }

    /// <summary>
    /// 资源应用成功事件
    /// </summary>
    public class ResourceApplySuccessEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(ResourceApplySuccessEventArgs).GetHashCode();

        public ResourceApplySuccessEventArgs()
        {
            Name = null;
            ApplyPath = null;
            ResourcePackPath = null;
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
        /// 资源应用后存储路径
        /// </summary>
        public string ApplyPath { get; private set; }

        /// <summary>
        /// 资源包路径
        /// </summary>
        public string ResourcePackPath { get; private set; }

        /// <summary>
        /// 资源大小
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// 压缩后资源大小
        /// </summary>
        public int CompressedLength { get; private set; }

        /// <summary>
        /// 创建资源应用成功事件
        /// </summary>
        /// <param name="e">内部事件</param>
        /// <returns>资源应用成功事件</returns>
        public static ResourceApplySuccessEventArgs Create(Framework.ResourceApplySuccessEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<ResourceApplySuccessEventArgs>();
            eventArgs.Name = e.Name;
            eventArgs.ApplyPath = e.ApplyPath;
            eventArgs.ResourcePackPath = e.ResourcePackPath;
            eventArgs.Length = e.Length;
            eventArgs.CompressedLength = e.CompressedLength;
            return eventArgs;
        }

        /// <summary>
        /// 清除资源应用成功事件
        /// </summary>
        public override void Clear()
        {
            Name = null;
            ApplyPath = null;
            ResourcePackPath = null;
            Length = 0;
            CompressedLength = 0;
        }
    }

    /// <summary>
    /// 资源应用失败事件
    /// </summary>
    public class ResourceApplyFailureEventArgs : BaseEventArgs
    {
        public static readonly int EventId = typeof(ResourceApplyFailureEventArgs).GetHashCode();

        public ResourceApplyFailureEventArgs()
        {
            Name = null;
            ResourcePackPath = null;
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
        /// 资源包路径
        /// </summary>
        public string ResourcePackPath { get; private set; }

        /// <summary>
        /// 失败信息
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// 创建资源应用失败事件
        /// </summary>
        /// <param name="e">内部事件</param>
        /// <returns>资源应用失败事件</returns>
        public static ResourceApplyFailureEventArgs Create(Framework.ResourceApplyFailureEventArgs e)
        {
            var eventArgs = ReferencePool.Acquire<ResourceApplyFailureEventArgs>();
            eventArgs.Name = e.Name;
            eventArgs.ResourcePackPath = e.ResourcePackPath;
            eventArgs.ErrorMessage = e.ErrorMessage;
            return eventArgs;
        }

        /// <summary>
        /// 清除资源应用失败事件
        /// </summary>
        public override void Clear()
        {
            Name = null;
            ResourcePackPath = null;
            ErrorMessage = null;
        }
    }
}