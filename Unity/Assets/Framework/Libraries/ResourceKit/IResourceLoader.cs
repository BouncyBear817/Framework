// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/22 16:49:2
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 加载资源管理器接口
    /// </summary>
    public interface IResourceLoader
    {
        /// <summary>
        /// 资源对象池自动释放可释放对象的间隔秒数
        /// </summary>
        public float AssetAutoReleaseInterval { get; set; }

        /// <summary>
        /// 资源对象池的容量
        /// </summary>
        public int AssetCapacity { get; set; }

        /// <summary>
        /// 资源对象池的优先级
        /// </summary>
        public int AssetPriority { get; set; }

        /// <summary>
        /// 资源对象池自动释放可释放对象的间隔秒数
        /// </summary>
        public float ResourceAutoReleaseInterval { get; set; }

        /// <summary>
        /// 资源对象池的容量
        /// </summary>
        public int ResourceCapacity { get; set; }

        /// <summary>
        /// 资源对象池的优先级
        /// </summary>
        public int ResourcePriority { get; set; }

        /// <summary>
        /// 设置对象池管理器
        /// </summary>
        /// <param name="objectPoolManager">对象池管理器</param>
        public void SetObjectPoolManager(IObjectPoolManager objectPoolManager);

        /// <summary>
        /// 设置加载资源代理辅助器
        /// </summary>
        /// <param name="loadResourceAgentHelper">加载资源代理辅助器</param>
        /// <param name="resourceHelper">资源辅助器</param>
        /// <param name="readOnlyPath">资源只读区路径</param>
        /// <param name="readWritePath">资源读写区路径</param>
        /// <param name="decryptResourceCallback">解密资源回调函数</param>
        public void AddLoadResourceAgentHelper(ILoadResourceAgentHelper loadResourceAgentHelper,
            IResourceHelper resourceHelper, string readOnlyPath, string readWritePath,
            DecryptResourceCallback decryptResourceCallback);

        /// <summary>
        /// 检查资源是否存在
        /// </summary>
        /// <param name="assetName">资源名称</param>
        /// <returns>资源是否存在</returns>
        public HasAssetResult HasAsset(string assetName);

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="assetName">资源名称</param>
        /// <param name="assetType">资源类型</param>
        /// <param name="priority">资源优先级</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集</param>
        /// <param name="userData">用户自定义数据</param>
        public void LoadAsset(string assetName, Type assetType, int priority, LoadAssetCallbacks loadAssetCallbacks,
            object userData);

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="asset">资源对象</param>
        public void UnloadAsset(object asset);

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <param name="priority">场景资源优先级</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集</param>
        /// <param name="userData">用户自定义数据</param>
        public void LoadScene(string sceneAssetName, int priority, LoadSceneCallbacks loadSceneCallbacks,
            object userData);

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调函数集</param>
        /// <param name="userData">用户自定义数据</param>
        public void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData);

        /// <summary>
        /// 获取二进制资源路径
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <returns>二进制资源路径</returns>
        /// <remarks>此方法适用于二进制资源存储在磁盘中（非文件系统中）</remarks>
        public string GetBinaryPath(string binaryAssetName);

        /// <summary>
        /// 获取二进制资源路径
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="storageInReadOnly">二进制资源是否存储于只读区内</param>
        /// <param name="storageInFileSystem">二进制资源是否存储于文件系统中</param>
        /// <param name="relativePath">文件系统相对于只读区或读写区的相对路径</param>
        /// <param name="fileName">二进制资源在文件系统中的名称（仅用于存储在文件系统中，否则为空）</param>
        /// <returns>是否成功获取二进制资源路径</returns>
        public bool GetBinaryPath(string binaryAssetName, out bool storageInReadOnly, out bool storageInFileSystem,
            out string relativePath, out string fileName);

        /// <summary>
        /// 获取二进制资源长度
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <returns>二进制资源长度</returns>
        public int GetBinaryLength(string binaryAssetName);

        /// <summary>
        /// 加载二进制资源
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="loadBinaryCallbacks">加载二进制资源回调函数</param>
        /// <param name="userData">用户自定义数据</param>
        public void LoadBinary(string binaryAssetName, LoadBinaryCallbacks loadBinaryCallbacks, object userData);

        /// <summary>
        /// 从文件系统加载二进制资源
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <returns>存储二进制资源的二进制流</returns>
        public byte[] LoadBinaryFromFileSystem(string binaryAssetName);

        /// <summary>
        /// 从文件系统加载二进制资源
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="buffer">存储二进制资源的二进制流</param>
        /// <param name="startIndex">二进制流的起始位置</param>
        /// <param name="length">二进制流的长度</param>
        /// <returns>加载的字节数</returns>
        public int LoadBinaryFromFileSystem(string binaryAssetName, byte[] buffer, int startIndex, int length);

        /// <summary>
        /// 从文件系统加载二进制资源片段
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="offset">加载片段的偏移</param>
        /// <param name="length">加载片段的长度</param>
        /// <returns>存储二进制资源片段的二进制流</returns>
        public byte[] LoadBinarySegmentFromFileSystem(string binaryAssetName, int offset, int length);

        /// <summary>
        /// 从文件系统加载二进制资源片段
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="offset">加载片段的偏移</param>
        /// <param name="buffer">存储二进制资源的二进制流</param>
        /// <param name="startIndex">加载片段的起始位置</param>
        /// <param name="length">加载片段的长度</param>
        /// <returns>加载片段的字节数</returns>
        public int LoadBinarySegmentFromFileSystem(string binaryAssetName, int offset, byte[] buffer, int startIndex,
            int length);

        /// <summary>
        /// 获取所有加载资源的任务信息
        /// </summary>
        /// <returns>所有加载资源的任务信息</returns>
        public TaskInfo[] GetAllLoadAssetInfos();

        /// <summary>
        /// 获取所有加载资源的任务信息
        /// </summary>
        /// <param name="results">所有加载资源的任务信息</param>
        public void GetAllLoadAssetInfos(List<TaskInfo> results);
    }
}