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
        /// 资源常量
        /// </summary>
        ResourceConstant ResourceConstant { get; }

        /// <summary>
        /// 资源数量
        /// </summary>
        int AssetCount { get; }

        string Variant { get; }

        /// <summary>
        /// 资源对象池信息
        /// </summary>
        ObjectPoolInfo AssetObjectPoolInfo { get; set; }

        /// <summary>
        /// 资源数量
        /// </summary>
        int ResourceCount { get; }

        /// <summary>
        /// 资源组数量
        /// </summary>
        int ResourceGroupCount { get; }

        /// <summary>
        /// 资源对象池信息
        /// </summary>
        ObjectPoolInfo ResourceObjectPoolInfo { get; set; }

        event EventHandler<ResourceVerifyStartEventArgs> ResourceVerifyStart;

        event EventHandler<ResourceVerifySuccessEventArgs> ResourceVerifySuccess;

        event EventHandler<ResourceVerifyFailureEventArgs> ResourceVerifyFailure;

        event EventHandler<ResourceUpdateStartEventArgs> ResourceUpdateStart;

        event EventHandler<ResourceUpdateChangedEventArgs> ResourceUpdateChanged;

        event EventHandler<ResourceUpdateSuccessEventArgs> ResourceUpdateSuccess;

        event EventHandler<ResourceUpdateFailureEventArgs> ResourceUpdateFailure;

        event EventHandler<ResourceUpdateAllCompleteEventArgs> ResourceUpdateAllComplete;

        event EventHandler<ResourceApplyStartEventArgs> ResourceApplyStart;

        event EventHandler<ResourceApplySuccessEventArgs> ResourceApplySuccess;

        event EventHandler<ResourceApplyFailureEventArgs> ResourceApplyFailure;

        public void SetObjectPoolManager(IObjectPoolManager objectPoolManager);

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
        /// <param name="verifyResourceCompleteCallback">校验资源完成回调函数</param>
        void VerifyResource(int verifyResourceLengthPerFrame,
            VerifyResourceCompleteCallback verifyResourceCompleteCallback);

        /// <summary>
        /// 使用可更新模式并检查资源
        /// </summary>
        /// <param name="ignoreOtherVariant">是否忽略其他变体的资源，若否则移除其它变体的资源</param>
        /// <param name="checkResourceCompleteCallback">检查资源完成回调函数</param>
        void CheckResource(bool ignoreOtherVariant, CheckResourceCompleteCallback checkResourceCompleteCallback);

        /// <summary>
        /// 使用可更新模式并应用资源
        /// </summary>
        /// <param name="resourcePackPath">资源包路径</param>
        /// <param name="applyResourceCompleteCallback">应用资源完成回调函数</param>
        void ApplyResource(string resourcePackPath, ApplyResourceCompleteCallback applyResourceCompleteCallback);

        /// <summary>
        /// 使用可更新模式并更新资源
        /// </summary>
        /// <param name="updateResourceCompleteCallback">更新资源完成回调函数</param>
        void UpdateResource(UpdateResourceCompleteCallback updateResourceCompleteCallback);

        /// <summary>
        /// 停止更新资源
        /// </summary>
        void StopUpdateResource();

        /// <summary>
        /// 校验资源包
        /// </summary>
        /// <param name="resourcePackPath">资源包路径</param>
        void VerifyResourcePack(string resourcePackPath);

        TaskInfo[] GetAllLoadAssetInfos();

        void GetAllLoadAssetInfos(List<TaskInfo> results);

        HasAssetResult HasAsset(string assetName);

        void LoadAsset(LoadAssetInfo loadAssetInfo, LoadAssetCallbacks loadAssetCallbacks);

        void UnloadAsset(object asset);

        void LoadScene(LoadSceneInfo loadSceneInfo, LoadSceneCallbacks loadSceneCallbacks);

        void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData = null);

        string GetBinaryPath(string binaryAssetName);

        bool GetBinaryPath(string binaryAssetName, out bool storageInReadOnly, out bool storageInFileSystem, out string relativePath, out string fileName);

        int GetBinaryLength(string binaryAssetName);

        void LoadBinary(string binaryAssetName, LoadBinaryCallbacks loadBinaryCallbacks, object userData = null);

        byte[] LoadBinaryFromFileSystem(string binaryAssetName);

        int LoadBinaryFromFileSystem(string binaryAssetName, byte[] buffer, int startIndex = 0);
        
        int LoadBinaryFromFileSystem(string binaryAssetName, byte[] buffer, int startIndex, int length);

        bool HasResourceGroup(string resourceGroupName);
        IResourceGroup GetResourceGroup(string resourceGroupName = null);

        IResourceGroup[] GetResourceGroups();

        void GetResourceGroups(List<IResourceGroup> results);
    }
}