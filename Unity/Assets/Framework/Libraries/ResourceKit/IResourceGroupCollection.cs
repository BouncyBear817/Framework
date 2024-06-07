// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/21 14:52:28
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 资源组集合接口
    /// </summary>
    public interface IResourceGroupCollection
    {
        /// <summary>
        /// 资源组集合是否准备完毕
        /// </summary>
        bool Ready { get; }

        /// <summary>
        /// 资源组集合内含资源数量
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        /// 资源组集合内已准备完成的资源数量
        /// </summary>
        int ReadyCount { get; }

        /// <summary>
        /// 资源组集合内含资源数量的总大小
        /// </summary>
        long TotalLength { get; }

        /// <summary>
        /// 资源组集合内含资源数量的压缩后总大小
        /// </summary>
        long TotalCompressedLength { get; }

        /// <summary>
        /// 资源组集合内已准备完成的资源数量的大小
        /// </summary>
        long ReadyLength { get; }

        /// <summary>
        /// 资源组集合内已准备完成的资源数量的压缩后大小
        /// </summary>
        long ReadyCompressedLength { get; }

        /// <summary>
        /// 资源组集合的完成进度
        /// </summary>
        float Progress { get; }

        /// <summary>
        /// 获取资源组集合包含的资源组列表
        /// </summary>
        /// <returns>资源组列表</returns>
        IResourceGroup[] GetResourceGroups();

        /// <summary>
        /// 获取资源组集合包含的资源组名称列表
        /// </summary>
        /// <returns>资源组名称列表</returns>
        string[] GetResourceNames();

        /// <summary>
        /// 获取资源组集合包含的资源组名称列表
        /// </summary>
        /// <param name="results">资源组名称列表</param>
        void GetResourceNames(List<string> results);
    }
}