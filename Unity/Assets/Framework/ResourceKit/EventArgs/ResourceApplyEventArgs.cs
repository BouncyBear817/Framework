namespace Framework
{
    /// <summary>
    /// 资源应用开始事件
    /// </summary>
    public class ResourceApplyStartEventArgs : FrameworkEventArgs
    {
        public ResourceApplyStartEventArgs()
        {
            ResourcePackPath = null;
            Count = 0;
            TotalLength = 0;
        }
        
        /// <summary>
        /// 资源包路径
        /// </summary>
        public string  ResourcePackPath { get; private set; }
        
        /// <summary>
        /// 应用资源数量
        /// </summary>
        public int Count { get; private set; }
        
        /// <summary>
        /// 应用资源总大小
        /// </summary>
        public int TotalLength { get; private set; }
        
        /// <summary>
        /// 创建资源应用开始事件
        /// </summary>
        /// <param name="resourcePackPath">资源包路径</param>
        /// <param name="count">应用资源数量</param>
        /// <param name="totalCount">应用资源总大小</param>
        /// <returns>资源应用开始事件</returns>
        public static ResourceApplyStartEventArgs Create(string resourcePackPath, int count, int totalCount)
        {
            var eventArgs = ReferencePool.Acquire<ResourceApplyStartEventArgs>();
            eventArgs.ResourcePackPath = resourcePackPath;
            eventArgs.Count = count;
            eventArgs.TotalLength = totalCount;
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
    public class ResourceApplySuccessEventArgs : FrameworkEventArgs
    {
        public ResourceApplySuccessEventArgs()
        {
            Name = null;
            ApplyPath = null;
            ResourcePackPath = null;
            Length = 0;
            CompressedLength = 0;
        }
        
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
        /// <param name="name">资源名称</param>
        /// <param name="applyPath">资源应用后存储路径</param>
        /// <param name="resourcePackPath">资源包路径</param>
        /// <param name="length">资源大小</param>
        /// <param name="compressedLength">压缩后资源大小</param>
        /// <returns>资源应用成功事件</returns>
        public static ResourceApplySuccessEventArgs Create(string name, string applyPath, string resourcePackPath, int length, int compressedLength)
        {
            var eventArgs = ReferencePool.Acquire<ResourceApplySuccessEventArgs>();
            eventArgs.Name = name;
            eventArgs.ApplyPath = applyPath;
            eventArgs.ResourcePackPath = resourcePackPath;
            eventArgs.Length = length;
            eventArgs.CompressedLength = compressedLength;
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
    public class ResourceApplyFailureEventArgs : FrameworkEventArgs
    {
        public ResourceApplyFailureEventArgs()
        {
            Name = null;
            ResourcePackPath = null;
            ErrorMessage = null;
        }
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
        /// <param name="name">资源名称</param>
        /// <param name="resourcePackPath">资源包路径</param>
        /// <param name="errorMessage">失败信息</param>
        /// <returns>资源应用失败事件</returns>
        public static ResourceApplyFailureEventArgs Create(string name, string resourcePackPath, string errorMessage)
        {
            var eventArgs = ReferencePool.Acquire<ResourceApplyFailureEventArgs>();
            eventArgs.Name = name;
            eventArgs.ResourcePackPath = resourcePackPath;
            eventArgs.ErrorMessage = errorMessage;
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