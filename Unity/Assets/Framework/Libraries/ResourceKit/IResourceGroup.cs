using System.Collections.Generic;
namespace Framework
{
    /// <summary>
    /// 资源组接口
    /// </summary>
    public interface IResourceGroup
    {
        /// <summary>
        /// 资源组名称
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// 资源组是否准备完毕
        /// </summary>
        bool Ready { get; }
        
        /// <summary>
        /// 资源组中已准备完成资源数量
        /// </summary>
        int ReadyCount { get; }
        
        /// <summary>
        /// 资源组中已准备完成资源大小
        /// </summary>
        long ReadyLength { get; }
        
        /// <summary>
        /// 资源组中已准备完成资源压缩后大小
        /// </summary>
        long ReadyCompressedLength { get; }
        
        /// <summary>
        /// 资源组中包含资源数量
        /// </summary>
        int TotalCount { get; }
        
        /// <summary>
        /// 资源组中包含资源总大小
        /// </summary>
        long TotalLength { get; }
        
        /// <summary>
        /// 资源组中包含资源压缩后总大小
        /// </summary>
        long TotalCompressedLength { get; }
        
        /// <summary>
        /// 资源组的完成进度
        /// </summary>
        float Progress { get; }

        /// <summary>
        /// 资源组包含的资源名称列表
        /// </summary>
        /// <returns>资源名称列表</returns>
        string[] GetResourceNames();

        /// <summary>
        /// 资源组包含的资源名称列表
        /// </summary>
        /// <param name="results">资源名称列表</param>
        void GetResourceNames(List<string> results);
    }
}