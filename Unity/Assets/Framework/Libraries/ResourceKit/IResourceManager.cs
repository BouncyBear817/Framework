/************************************************************
 * Unity Version: 2022.3.0f1c1
 * Author:        bear
 * CreateTime:    2024/01/05 11:21:26
 * Description:
 * Modify Record:
 *************************************************************/

using System;
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// 资源管理器接口
    /// </summary>
    public interface IResourceManager
    {
        /// <summary>
        /// 只读区地址
        /// </summary>
        string ReadOnlyPath { get; }

        /// <summary>
        /// 读写区地址
        /// </summary>
        string ReadWritePath { get; }

        /// <summary>
        /// 资源模式
        /// </summary>
        ResourceMode ResourceMode { get; }

        /// <summary>
        /// 当前变体
        /// </summary>
        string CurrentVariant { get; }

        /// <summary>
        /// 当前资源适用的版号
        /// </summary>
        string ApplicableVersion { get; }

        /// <summary>
        /// 当前内部资源版本号
        /// </summary>
        int InternalResourceVersion { get; }

        /// <summary>
        /// 资源更新下载地址
        /// </summary>
        string UpdatePrefixUri { get; set; }

        /// <summary>
        /// 每更新多少字节的资源，重新生成一次版本资源列表
        /// </summary>
        int GenerateReadWriteVersionListLength { get; set; }

        /// <summary>
        /// 正在应用资源包路径
        /// </summary>
        string ApplyingResourcePackPath { get; }

        /// <summary>
        /// 等待应用资源的数量
        /// </summary>
        int ApplyingWaitingCount { get; }

        /// <summary>
        /// 资源更新重试次数
        /// </summary>
        int UpdateRetryCount { get; set; }

        /// <summary>
        /// 正在更新的资源组
        /// </summary>
        IResourceGroup UpdatingResourceGroup { get; }

        /// <summary>
        /// 等待更新资源的数量
        /// </summary>
        int UpdateWaitingCount { get; }

        /// <summary>
        /// 使用时下载的等待更新资源的数量
        /// </summary>
        int UpdateWaitingWhilePlayingCount { get; }

        /// <summary>
        /// 候选更新资源的数量
        /// </summary>
        int UpdateCandidateCount { get; }

        /// <summary>
        /// 加载资源代理总数量
        /// </summary>
        int LoadTotalAgentCount { get; }

        /// <summary>
        /// 可用的加载资源代理的数量
        /// </summary>
        int LoadAvailableAgentCount { get; }

        /// <summary>
        /// 工作中加载资源代理的数量
        /// </summary>
        int LoadWorkingAgentCount { get; }

        /// <summary>
        /// 等待加载资源任务的数量
        /// </summary>
        int LoadWaitingTaskCount { get; }

        /// <summary>
        /// 资源数量
        /// </summary>
        int AssetCount { get; }

        /// <summary>
        /// 资源对象池自动释放可释放对象的间隔秒数
        /// </summary>
        float AssetAutoReleaseInterval { get; set; }

        /// <summary>
        /// 资源对象池的容量
        /// </summary>
        int AssetCapacity { get; set; }

        /// <summary>
        /// 资源对象池的优先级
        /// </summary>
        int AssetPriority { get; set; }

        /// <summary>
        /// 资源数量
        /// </summary>
        int ResourceCount { get; }

        /// <summary>
        /// 资源组数量
        /// </summary>
        int ResourceGroupCount { get; }

        /// <summary>
        /// 资源对象池自动释放可释放对象的间隔秒数
        /// </summary>
        float ResourceAutoReleaseInterval { get; set; }

        /// <summary>
        /// 资源对象池的容量
        /// </summary>
        int ResourceCapacity { get; set; }

        /// <summary>
        /// 资源对象池的优先级
        /// </summary>
        int ResourcePriority { get; set; }

        /// <summary>
        /// 单机模式版本资源列表序列化器
        /// </summary>
        PackageVersionListSerializer PackageVersionListSerializer { get; }

        /// <summary>
        /// 可更新模式版本资源列表序列化器
        /// </summary>
        UpdatableVersionListSerializer UpdatableVersionListSerializer { get; }

        /// <summary>
        /// 本地只读区版本资源列表序列化器
        /// </summary>
        ReadOnlyVersionListSerializer ReadOnlyVersionListSerializer { get; }

        /// <summary>
        /// 本地读写区版本资源序列化器
        /// </summary>
        ReadWriteVersionListSerializer ReadWriteVersionListSerializer { get; }

        /// <summary>
        /// 资源包版本资源列表序列化器
        /// </summary>
        ResourcePackVersionListSerializer ResourcePackVersionListSerializer { get; }

        /// <summary>
        /// 资源校验开始事件
        /// </summary>
        event EventHandler<ResourceVerifyStartEventArgs> ResourceVerifyStart;

        /// <summary>
        /// 资源校验成功事件
        /// </summary>
        event EventHandler<ResourceVerifySuccessEventArgs> ResourceVerifySuccess;

        /// <summary>
        /// 资源校验失败事件
        /// </summary>
        event EventHandler<ResourceVerifyFailureEventArgs> ResourceVerifyFailure;

        /// <summary>
        /// 资源更新开始事件
        /// </summary>
        event EventHandler<ResourceUpdateStartEventArgs> ResourceUpdateStart;

        /// <summary>
        /// 资源更新改变事件
        /// </summary>
        event EventHandler<ResourceUpdateChangedEventArgs> ResourceUpdateChanged;

        /// <summary>
        /// 资源更新成功事件
        /// </summary>
        event EventHandler<ResourceUpdateSuccessEventArgs> ResourceUpdateSuccess;

        /// <summary>
        /// 资源更新失败事件
        /// </summary>
        event EventHandler<ResourceUpdateFailureEventArgs> ResourceUpdateFailure;

        /// <summary>
        /// 资源更新全部完成事件
        /// </summary>
        event EventHandler<ResourceUpdateAllCompleteEventArgs> ResourceUpdateAllComplete;

        /// <summary>
        /// 资源应用开始事件
        /// </summary>
        event EventHandler<ResourceApplyStartEventArgs> ResourceApplyStart;

        /// <summary>
        /// 资源应用成功事件
        /// </summary>
        event EventHandler<ResourceApplySuccessEventArgs> ResourceApplySuccess;

        /// <summary>
        /// 资源应用失败事件
        /// </summary>
        event EventHandler<ResourceApplyFailureEventArgs> ResourceApplyFailure;

        /// <summary>
        /// 设置只读区路径
        /// </summary>
        /// <param name="readOnlyPath">只读区路径</param>
        void SetReadOnlyPath(string readOnlyPath);

        /// <summary>
        /// 设置读写区路径
        /// </summary>
        /// <param name="readWritePath">读写区路径</param>
        void SetReadWritePath(string readWritePath);

        /// <summary>
        /// 设置资源模式
        /// </summary>
        /// <param name="resourceMode">资源模式</param>
        void SetResourceMode(ResourceMode resourceMode);

        /// <summary>
        /// 设置当前变体
        /// </summary>
        /// <param name="currentVariant">当前变体</param>
        void SetCurrentVariant(string currentVariant);

        /// <summary>
        /// 设置对象池管理器
        /// </summary>
        /// <param name="objectPoolManager">对象池管理器</param>
        public void SetObjectPoolManager(IObjectPoolManager objectPoolManager);

        /// <summary>
        /// 设置文件系统管理器
        /// </summary>
        /// <param name="fileSystemManager">文件系统管理器</param>
        void SetFileSystemManager(IFileSystemManager fileSystemManager);

        /// <summary>
        /// 设置下载管理器
        /// </summary>
        /// <param name="downloadManager">下载管理器</param>
        void SetDownloadManager(IDownloadManager downloadManager);

        /// <summary>
        /// 设置解密资源回调函数
        /// </summary>
        /// <param name="decryptResourceCallback">解密资源回调函数</param>
        void SetDecryptResourceCallback(DecryptResourceCallback decryptResourceCallback);

        /// <summary>
        /// 加载资源辅助器
        /// </summary>
        /// <param name="resourceHelper">资源辅助器</param>
        void SetResourceHelper(IResourceHelper resourceHelper);

        /// <summary>
        /// 增加加载资源代理辅助器
        /// </summary>
        /// <param name="loadResourceAgentHelper">加载资源代理辅助器</param>
        void AddLoadResourceAgentHelper(ILoadResourceAgentHelper loadResourceAgentHelper);

        /// <summary>
        /// 使用单机模式并初始化资源
        /// </summary>
        /// <param name="initResourcesCompleteCallback">使用单机模式并初始化资源完成时回调</param>
        void InitResources(InitResourcesCompleteCallback initResourcesCompleteCallback);

        /// <summary>
        /// 使用可更新模式并检查版本资源列表
        /// </summary>
        /// <param name="latestInternalResourceVersion">最新的内部资源版本号</param>
        /// <returns>检查版本资源列表结果</returns>
        CheckVersionListResult CheckVersionList(int latestInternalResourceVersion);

        /// <summary>
        /// 使用可更新模式并更新版本资源列表
        /// </summary>
        /// <param name="versionListInfo">版本资源列表信息</param>
        /// <param name="updateVersionListCallbacks">更新版本资源列表回调函数集</param>
        void UpdateVersionList(VersionListInfo versionListInfo, UpdateVersionListCallbacks updateVersionListCallbacks);

        /// <summary>
        /// 使用可更新模式并校验资源
        /// </summary>
        /// <param name="verifyResourceLengthPerFrame">每帧校验资源大小，以字节为单位</param>
        /// <param name="verifyResourcesCompleteCallback">校验资源完成回调函数</param>
        void VerifyResource(int verifyResourceLengthPerFrame,
            VerifyResourcesCompleteCallback verifyResourcesCompleteCallback);

        /// <summary>
        /// 使用可更新模式并检查资源
        /// </summary>
        /// <param name="ignoreOtherVariant">是否忽略其他变体的资源，若否则移除其它变体的资源</param>
        /// <param name="checkResourcesCompleteCallback">使用可更新模式并检查资源完成时的回调函数</param>
        void CheckResource(bool ignoreOtherVariant, CheckResourcesCompleteCallback checkResourcesCompleteCallback);

        /// <summary>
        /// 使用可更新模式并应用资源
        /// </summary>
        /// <param name="resourcePackPath">资源包路径</param>
        /// <param name="applyResourcesCompleteCallback">使用可更新模式并应用资源完成时的回调函数</param>
        void ApplyResource(string resourcePackPath, ApplyResourcesCompleteCallback applyResourcesCompleteCallback);

        /// <summary>
        /// 使用可更新模式并更新资源
        /// </summary>
        /// <param name="updateResourcesCompleteCallback">使用可更新模式并更新资源完成时的回调函数</param>
        void UpdateResources(UpdateResourcesCompleteCallback updateResourcesCompleteCallback);

        /// <summary>
        /// 使用可更新模式并更新指定资源组的资源
        /// </summary>
        /// <param name="resourceGroupName">资源组名称</param>
        /// <param name="updateResourcesCompleteCallback">使用可更新模式并更新资源完成时的回调函数</param>
        void UpdateResources(string resourceGroupName, UpdateResourcesCompleteCallback updateResourcesCompleteCallback);

        /// <summary>
        /// 停止更新资源
        /// </summary>
        void StopUpdateResource();

        /// <summary>
        /// 校验资源包
        /// </summary>
        /// <param name="resourcePackPath">资源包路径</param>
        /// <returns>是否成功校验资源包</returns>
        bool VerifyResourcePack(string resourcePackPath);

        /// <summary>
        /// 获取所有加载资源任务的信息
        /// </summary>
        /// <returns>所有加载资源任务的信息</returns>
        TaskInfo[] GetAllLoadAssetInfos();

        /// <summary>
        /// 获取所有加载资源任务的信息
        /// </summary>
        /// <param name="results">所有加载资源任务的信息</param>
        void GetAllLoadAssetInfos(List<TaskInfo> results);

        /// <summary>
        /// 检查资源是否存在
        /// </summary>
        /// <param name="assetName">资源名称</param>
        /// <returns>检查资源是否存在的结果</returns>
        HasAssetResult HasAsset(string assetName);

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="loadAssetInfo">加载资源的信息</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集</param>
        void LoadAsset(LoadAssetInfo loadAssetInfo, LoadAssetCallbacks loadAssetCallbacks);

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="asset">资源</param>
        void UnloadAsset(object asset);

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="loadSceneInfo">加载场景信息</param>
        /// <param name="loadSceneCallbacks">加载场景回调函数集</param>
        void LoadScene(LoadSceneInfo loadSceneInfo, LoadSceneCallbacks loadSceneCallbacks);

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称</param>
        /// <param name="unloadSceneCallbacks">卸载场景回调函数集</param>
        /// <param name="userData">用户自定义数据</param>
        void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData = null);

        /// <summary>
        /// 获取二进制资源的实际路径
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <returns>二进制资源的实际路径</returns>
        /// <remarks>此方法仅适用于二进制资源存储在磁盘（非文件系统）中，否则为空</remarks>
        string GetBinaryPath(string binaryAssetName);

        /// <summary>
        /// 获取二进制资源的实际路径
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="storageInReadOnly">二进制资源是否存储在只读区内</param>
        /// <param name="storageInFileSystem">二进制资源是否存储在文件系统内</param>
        /// <param name="relativePath">文件系统相对于只读区或读写区的相对路径</param>
        /// <param name="fileName">二进制资源在文件系统中的名称（仅用于存储在文件系统中，否则为空）</param>
        /// <returns>是否成功获取二进制资源的实际路径</returns>
        bool GetBinaryPath(string binaryAssetName, out bool storageInReadOnly, out bool storageInFileSystem,
            out string relativePath, out string fileName);

        /// <summary>
        /// 获取二进制资源的长度
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <returns>二进制资源的长度</returns>
        int GetBinaryLength(string binaryAssetName);

        /// <summary>
        /// 加载二进制资源
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="loadBinaryCallbacks">加载二进制资源回调函数</param>
        /// <param name="userData">用户自定义数据</param>
        void LoadBinary(string binaryAssetName, LoadBinaryCallbacks loadBinaryCallbacks, object userData = null);

        /// <summary>
        /// 从文件系统中加载二进制资源
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <returns>存储加载二进制资源的二进制流</returns>
        byte[] LoadBinaryFromFileSystem(string binaryAssetName);

        /// <summary>
        /// 从文件系统中加载二进制资源
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="buffer">存储加载二进制资源的二进制流</param>
        /// <returns>实际加载了多少字节</returns>
        int LoadBinaryFromFileSystem(string binaryAssetName, byte[] buffer);

        /// <summary>
        /// 从文件系统中加载二进制资源
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="buffer">存储加载二进制资源的二进制流</param>
        /// <param name="startIndex">二进制流的起始位置</param>
        /// <returns>实际加载了多少字节</returns>
        int LoadBinaryFromFileSystem(string binaryAssetName, byte[] buffer, int startIndex);

        /// <summary>
        /// 从文件系统中加载二进制资源
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="buffer">存储加载二进制资源的二进制流</param>
        /// <param name="startIndex">二进制流的起始位置</param>
        /// <param name="length">二进制流的长度</param>
        /// <returns>实际加载了多少字节</returns>
        int LoadBinaryFromFileSystem(string binaryAssetName, byte[] buffer, int startIndex, int length);

        /// <summary>
        /// 从文件系统中加载二进制资源的片段
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="length">加载片段的长度</param>
        /// <returns>存储加载二进制资源片段内容的二进制流</returns>
        byte[] LoadBinarySegmentFromFileSystem(string binaryAssetName, int length);

        /// <summary>
        /// 从文件系统中加载二进制资源的片段
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="offset">加载片段的偏移</param>
        /// <param name="length">加载片段的长度</param>
        /// <returns>存储加载二进制资源片段内容的二进制流</returns>
        byte[] LoadBinarySegmentFromFileSystem(string binaryAssetName, int offset, int length);

        /// <summary>
        /// 从文件系统中加载二进制资源的片段
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="buffer">存储加载二进制资源片段内容的二进制流</param>
        /// <returns>实际加载了多少字节</returns>
        int LoadBinarySegmentFromFileSystem(string binaryAssetName, byte[] buffer);

        /// <summary>
        /// 从文件系统中加载二进制资源的片段
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="buffer">存储加载二进制资源片段内容的二进制流</param>
        /// <param name="length">加载片段的长度</param>
        /// <returns>实际加载了多少字节</returns>
        int LoadBinarySegmentFromFileSystem(string binaryAssetName, byte[] buffer, int length);

        /// <summary>
        /// 从文件系统中加载二进制资源的片段
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="buffer">存储加载二进制资源片段内容的二进制流</param>
        /// <param name="startIndex">二进制流的起始位置</param>
        /// <param name="length">加载片段的长度</param>
        /// <returns>实际加载了多少字节</returns>
        int LoadBinarySegmentFromFileSystem(string binaryAssetName, byte[] buffer, int startIndex, int length);

        /// <summary>
        /// 从文件系统中加载二进制资源的片段
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="offset">加载片段的偏移</param>
        /// <param name="buffer">存储加载二进制资源片段内容的二进制流</param>
        /// <returns>实际加载了多少字节</returns>
        int LoadBinarySegmentFromFileSystem(string binaryAssetName, int offset, byte[] buffer);

        /// <summary>
        /// 从文件系统中加载二进制资源的片段
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="offset">加载片段的偏移</param>
        /// <param name="buffer">存储加载二进制资源片段内容的二进制流</param>
        /// <param name="length">加载片段的长度</param>
        /// <returns>实际加载了多少字节</returns>
        int LoadBinarySegmentFromFileSystem(string binaryAssetName, int offset, byte[] buffer, int length);

        /// <summary>
        /// 从文件系统中加载二进制资源的片段
        /// </summary>
        /// <param name="binaryAssetName">二进制资源名称</param>
        /// <param name="offset">加载片段的偏移</param>
        /// <param name="buffer">存储加载二进制资源片段内容的二进制流</param>
        /// <param name="startIndex">二进制流的起始位置</param>
        /// <param name="length">加载片段的长度</param>
        /// <returns>实际加载了多少字节</returns>
        int LoadBinarySegmentFromFileSystem(string binaryAssetName, int offset, byte[] buffer, int startIndex,
            int length);

        /// <summary>
        /// 检查资源组是否存在
        /// </summary>
        /// <param name="resourceGroupName">资源组名称</param>
        /// <returns>资源组是否存在</returns>
        bool HasResourceGroup(string resourceGroupName);

        /// <summary>
        /// 获取指定资源组
        /// </summary>
        /// <param name="resourceGroupName">资源组名称</param>
        /// <returns>指定资源组</returns>
        IResourceGroup GetResourceGroup(string resourceGroupName = null);

        /// <summary>
        /// 获取所有资源组
        /// </summary>
        /// <returns>所有资源组</returns>
        IResourceGroup[] GetAllResourceGroups();

        /// <summary>
        /// 获取所有资源组
        /// </summary>
        /// <param name="results">所有资源组</param>
        void GetAllResourceGroups(List<IResourceGroup> results);

        /// <summary>
        /// 获取资源组集合
        /// </summary>
        /// <param name="resourceGroupNames">资源组名称集合</param>
        /// <returns>资源组集合</returns>
        IResourceGroupCollection GetResourceGroupCollection(params string[] resourceGroupNames);

        /// <summary>
        /// 获取资源组集合
        /// </summary>
        /// <param name="resourceGroupNames">资源组名称集合</param>
        /// <returns>资源组集合</returns>
        IResourceGroupCollection GetResourceGroupCollection(List<string> resourceGroupNames);
    }
}