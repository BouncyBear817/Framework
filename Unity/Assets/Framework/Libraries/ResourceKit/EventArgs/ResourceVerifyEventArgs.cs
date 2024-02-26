/************************************************************
* Unity Version: 2022.3.0f1c1
* Author:        bear
* CreateTime:    2024/01/05 11:21:26
* Description:   
* Modify Record: 
*************************************************************/

namespace Framework
{
    /// <summary>
    /// 资源校验开始事件
    /// </summary>
    public class ResourceVerifyStartEventArgs : FrameworkEventArgs
    {
        public ResourceVerifyStartEventArgs()
        {
            Count = 0;
            TotalLength = 0L;
        }

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
        /// <param name="count">要校验的资源数量</param>
        /// <param name="totalLength">要校验的资源总大小</param>
        /// <returns>资源校验开始事件</returns>
        public static ResourceVerifyStartEventArgs Create(int count, int totalLength)
        {
            var eventArgs = ReferencePool.Acquire<ResourceVerifyStartEventArgs>();
            eventArgs.Count = count;
            eventArgs.TotalLength = totalLength;
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
    public class ResourceVerifySuccessEventArgs : FrameworkEventArgs
    {
        public ResourceVerifySuccessEventArgs()
        {
            Name = null;
            Length = 0;
        }

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
        /// <param name="name">资源名称</param>
        /// <param name="length">资源大小</param>
        /// <returns>资源校验成功事件</returns>
        public static ResourceVerifySuccessEventArgs Create(string name, int length)
        {
            var eventArgs = ReferencePool.Acquire<ResourceVerifySuccessEventArgs>();
            eventArgs.Name = name;
            eventArgs.Length = length;
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
    public class ResourceVerifyFailureEventArgs : FrameworkEventArgs
    {
        public ResourceVerifyFailureEventArgs()
        {
            Name = null;
        }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 创建资源校验失败事件
        /// </summary>
        /// <param name="name">资源名称</param>
        /// <returns>资源校验失败事件</returns>
        public static ResourceVerifyFailureEventArgs Create(string name)
        {
            var eventArgs = ReferencePool.Acquire<ResourceVerifyFailureEventArgs>();
            eventArgs.Name = name;
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