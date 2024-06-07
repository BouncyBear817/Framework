// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/22 14:16:13
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;

namespace Framework
{
    /// <summary>
    /// 加载资源代理辅助器接口
    /// </summary>
    public interface ILoadResourceAgentHelper
    {
        /// <summary>
        /// 加载资源代理辅助器更新事件
        /// </summary>
        event EventHandler<LoadResourceAgentHelperUpdateEventArgs> LoadResourceAgentHelperUpdate;

        /// <summary>
        /// 加载资源代理辅助器读取资源文件完成事件
        /// </summary>
        event EventHandler<LoadResourceAgentHelperReadFileCompleteEventArgs> LoadResourceAgentHelperReadFileComplete;

        /// <summary>
        /// 加载资源代理辅助器读取资源二进制流完成事件
        /// </summary>
        event EventHandler<LoadResourceAgentHelperReadBytesCompleteEventArgs> LoadResourceAgentHelperReadBytesComplete;

        /// <summary>
        /// 加载资源代理辅助器将资源二进制流转换为加载对象事件
        /// </summary>
        event EventHandler<LoadResourceAgentHelperParseBytesCompleteEventArgs>
            LoadResourceAgentHelperParseBytesComplete;

        /// <summary>
        /// 加载资源代理辅助器加载资源完成事件
        /// </summary>
        event EventHandler<LoadResourceAgentHelperLoadCompleteEventArgs> LoadResourceAgentHelperLoadComplete;

        /// <summary>
        /// 加载资源代理辅助器错误事件
        /// </summary>
        event EventHandler<LoadResourceAgentHelperErrorEventArgs> LoadResourceAgentHelperError;

        /// <summary>
        /// 读取资源文件
        /// </summary>
        /// <param name="fullPath">资源完整路径</param>
        void ReadFile(string fullPath);

        /// <summary>
        /// 读取资源文件
        /// </summary>
        /// <param name="fileSystem">资源的文件系统</param>
        /// <param name="name">资源名称</param>
        void ReadFile(IFileSystem fileSystem, string name);

        /// <summary>
        /// 读取资源二进制流
        /// </summary>
        /// <param name="fullPath">资源完整路径</param>
        void ReadBytes(string fullPath);

        /// <summary>
        /// 读取资源二进制流
        /// </summary>
        /// <param name="fileSystem">资源的文件系统</param>
        /// <param name="name">资源名称</param>
        void ReadBytes(IFileSystem fileSystem, string name);

        /// <summary>
        /// 将资源二进制流转换为加载对象
        /// </summary>
        /// <param name="bytes">资源二进制流</param>
        void ParseBytes(byte[] bytes);

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="resource">资源</param>
        /// <param name="assetName">资源名称</param>
        /// <param name="assetType">资源类型</param>
        /// <param name="isScene">资源是否为场景</param>
        void LoadAsset(object resource, string assetName, Type assetType, bool isScene);

        /// <summary>
        /// 重置加载资源代理辅助器
        /// </summary>
        void Reset();
    }
}