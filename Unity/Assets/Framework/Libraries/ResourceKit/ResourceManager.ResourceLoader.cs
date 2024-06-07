// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/21 16:4:39
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;
using System.IO;

namespace Framework
{
    public sealed partial class ResourceManager : FrameworkModule, IResourceManager
    {
        /// <summary>
        /// 资源加载器
        /// </summary>
        private sealed partial class ResourceLoader : IResourceLoader
        {
            private const int CachedBytesLength = 4;

            private readonly ResourceManager mResourceManager;
            private readonly TaskPool<LoadResourceTaskBase> mTaskPool;
            private readonly Dictionary<object, int> mAssetDependencyCount;
            private readonly Dictionary<object, int> mResourceDependencyCount;
            private readonly Dictionary<object, object> mAssetToResourceMap;
            private readonly Dictionary<string, object> mSceneToAssetMap;
            private readonly LoadBytesCallbacks mLoadBytesCallbacks;
            private readonly byte[] mCachedHashBytes;
            private IObjectPool<AssetObject> mAssetPool;
            private IObjectPool<ResourceObject> mResourcePool;

            public ResourceLoader(ResourceManager resourceManager)
            {
                mResourceManager = resourceManager;
                mTaskPool = new TaskPool<LoadResourceTaskBase>();
                mAssetDependencyCount = new Dictionary<object, int>();
                mResourceDependencyCount = new Dictionary<object, int>();
                mAssetToResourceMap = new Dictionary<object, object>();
                mSceneToAssetMap = new Dictionary<string, object>(StringComparer.Ordinal);
                mLoadBytesCallbacks = new LoadBytesCallbacks(OnLoadBinarySuccess, OnLoadBinaryFailure);
                mCachedHashBytes = new byte[CachedBytesLength];
                mAssetPool = null;
                mResourcePool = null;
            }


            public int TotalAgentCount => mTaskPool.TotalAgentCount;

            public int AvailableAgentCount => mTaskPool.AvailableAgentCount;

            public int WorkingAgentCount => mTaskPool.WorkingAgentCount;

            public int WaitingTaskCount => mTaskPool.WaitingTaskCount;

            /// <summary>
            /// 资源对象池自动释放可释放对象的间隔秒数
            /// </summary>
            public float AssetAutoReleaseInterval
            {
                get => mAssetPool.AutoReleaseInterval;
                set => mAssetPool.AutoReleaseInterval = value;
            }

            /// <summary>
            /// 资源对象池的容量
            /// </summary>
            public int AssetCapacity
            {
                get => mAssetPool.Capacity;
                set => mAssetPool.Capacity = value;
            }

            /// <summary>
            /// 资源对象池的优先级
            /// </summary>
            public int AssetPriority
            {
                get => mAssetPool.Priority;
                set => mAssetPool.Priority = value;
            }

            /// <summary>
            /// 资源对象池自动释放可释放对象的间隔秒数
            /// </summary>
            public float ResourceAutoReleaseInterval
            {
                get => mResourcePool.AutoReleaseInterval;
                set => mResourcePool.AutoReleaseInterval = value;
            }

            /// <summary>
            /// 资源对象池的容量
            /// </summary>
            public int ResourceCapacity
            {
                get => mResourcePool.Capacity;
                set => mResourcePool.Capacity = value;
            }

            /// <summary>
            /// 资源对象池的优先级
            /// </summary>
            public int ResourcePriority
            {
                get => mResourcePool.Priority;
                set => mResourcePool.Priority = value;
            }

            /// <summary>
            /// 加载资源器轮询
            /// </summary>
            /// <param name="elapseSeconds">逻辑流逝时间</param>
            /// <param name="realElapseSeconds">真实流逝时间</param>
            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                mTaskPool.Update(elapseSeconds, realElapseSeconds);
            }

            public void Shutdown()
            {
                mTaskPool.Shutdown();
                mAssetDependencyCount.Clear();
                mResourceDependencyCount.Clear();
                mAssetToResourceMap.Clear();
                mSceneToAssetMap.Clear();
                LoadResourceAgent.Clear();
            }

            /// <summary>
            /// 设置对象池管理器
            /// </summary>
            /// <param name="objectPoolManager">对象池管理器</param>
            public void SetObjectPoolManager(IObjectPoolManager objectPoolManager)
            {
                if (objectPoolManager == null)
                {
                    throw new Exception("Object pool manager is invalid.");
                }

                mAssetPool = objectPoolManager.SpawnObjectPool<AssetObject>(new ObjectPoolInfo("Asset Pool"));
                mResourcePool = objectPoolManager.SpawnObjectPool<ResourceObject>(new ObjectPoolInfo("Resource Pool"));
            }

            /// <summary>
            /// 设置加载资源代理辅助器
            /// </summary>
            /// <param name="loadResourceAgentHelper">加载资源代理辅助器</param>
            /// <param name="resourceHelper">资源辅助器</param>
            /// <param name="readOnlyPath">资源只读区路径</param>
            /// <param name="readWritePath">资源读写区路径</param>
            /// <param name="decryptResourceCallback">解密资源回调函数</param>
            public void AddLoadResourceAgentHelper(ILoadResourceAgentHelper loadResourceAgentHelper,
                IResourceHelper resourceHelper,
                string readOnlyPath, string readWritePath, DecryptResourceCallback decryptResourceCallback)
            {
                if (mAssetPool == null || mResourcePool == null)
                {
                    throw new Exception("You must set object pool manager first.");
                }

                var agent = new LoadResourceAgent(loadResourceAgentHelper, resourceHelper, this, readOnlyPath,
                    readWritePath, decryptResourceCallback);
                mTaskPool.AddAgent(agent);
            }

            /// <summary>
            /// 检查资源是否存在
            /// </summary>
            /// <param name="assetName">资源名称</param>
            /// <returns>资源是否存在</returns>
            public HasAssetResult HasAsset(string assetName)
            {
                var resourceInfo = GetResourceInfo(assetName);
                if (resourceInfo == null)
                {
                    return HasAssetResult.NotExist;
                }

                if (!resourceInfo.Ready && mResourceManager.mResourceMode != ResourceMode.UpdatableWhilePlaying)
                {
                    return HasAssetResult.NotReady;
                }

                if (resourceInfo.UseFileSystem)
                {
                    return resourceInfo.IsLoadFromBinary
                        ? HasAssetResult.BinaryOnFileSystem
                        : HasAssetResult.AssetOnFileSystem;
                }
                else
                {
                    return resourceInfo.IsLoadFromBinary ? HasAssetResult.BinaryOnDisk : HasAssetResult.AssetOnDisk;
                }
            }

            /// <summary>
            /// 加载资源
            /// </summary>
            /// <param name="assetName">资源名称</param>
            /// <param name="assetType">资源类型</param>
            /// <param name="priority">资源优先级</param>
            /// <param name="loadAssetCallbacks">加载资源回调函数集</param>
            /// <param name="userData">用户自定义数据</param>
            public void LoadAsset(string assetName, Type assetType, int priority, LoadAssetCallbacks loadAssetCallbacks,
                object userData)
            {
                if (!CheckAsset(assetName, out var resourceInfo, out var dependencyAssetNames))
                {
                    var errorMessage = $"Can not load asset ({assetName}).";
                    var status = resourceInfo != null && !resourceInfo.Ready
                        ? LoadResourceStatus.NotReady
                        : LoadResourceStatus.NotExist;
                    if (loadAssetCallbacks.LoadAssetFailureCallback != null)
                    {
                        loadAssetCallbacks.LoadAssetFailureCallback(assetName, status, errorMessage, userData);
                        return;
                    }

                    throw new Exception(errorMessage);
                }

                if (resourceInfo.IsLoadFromBinary)
                {
                    var errorMessage = $"Can not load asset ({assetName}) which is a binary asset.";
                    if (loadAssetCallbacks.LoadAssetFailureCallback != null)
                    {
                        loadAssetCallbacks.LoadAssetFailureCallback(assetName, LoadResourceStatus.TypeError,
                            errorMessage, userData);
                        return;
                    }

                    throw new Exception(errorMessage);
                }

                var loadAssetTask = LoadAssetTask.Create(assetName, assetType, priority, resourceInfo,
                    dependencyAssetNames, loadAssetCallbacks, userData);
                foreach (var dependencyAssetName in dependencyAssetNames)
                {
                    if (!LoadDependencyAsset(assetName, priority, loadAssetTask, userData))
                    {
                        var errorMessage =
                            $"Can not load dependency asset ({dependencyAssetName}) when load asset ({assetName}).";
                        if (loadAssetCallbacks.LoadAssetFailureCallback != null)
                        {
                            loadAssetCallbacks.LoadAssetFailureCallback(assetName, LoadResourceStatus.TypeError,
                                errorMessage, userData);
                            return;
                        }

                        throw new Exception(errorMessage);
                    }
                }

                mTaskPool.AddTask(loadAssetTask);
                if (!resourceInfo.Ready)
                {
                    mResourceManager.UpdateResource(resourceInfo.ResourceName);
                }
            }

            /// <summary>
            /// 卸载资源
            /// </summary>
            /// <param name="asset">资源对象</param>
            public void UnloadAsset(object asset)
            {
                mAssetPool.UnSpawn(asset);
            }

            /// <summary>
            /// 加载场景
            /// </summary>
            /// <param name="sceneAssetName">场景资源名称</param>
            /// <param name="priority">场景资源优先级</param>
            /// <param name="loadSceneCallbacks">加载场景回调函数集</param>
            /// <param name="userData">用户自定义数据</param>
            public void LoadScene(string sceneAssetName, int priority, LoadSceneCallbacks loadSceneCallbacks,
                object userData)
            {
                if (!CheckAsset(sceneAssetName, out var resourceInfo, out var dependencyAssetNames))
                {
                    var errorMessage = $"Can not load scene asset ({sceneAssetName}).";
                    var status = resourceInfo != null && !resourceInfo.Ready
                        ? LoadResourceStatus.NotReady
                        : LoadResourceStatus.NotExist;
                    if (loadSceneCallbacks.LoadSceneFailureCallback != null)
                    {
                        loadSceneCallbacks.LoadSceneFailureCallback(sceneAssetName, status, errorMessage, userData);
                        return;
                    }

                    throw new Exception(errorMessage);
                }

                if (resourceInfo.IsLoadFromBinary)
                {
                    var errorMessage = $"Can not load scene asset ({sceneAssetName}) which is a binary asset.";
                    if (loadSceneCallbacks.LoadSceneFailureCallback != null)
                    {
                        loadSceneCallbacks.LoadSceneFailureCallback(sceneAssetName, LoadResourceStatus.TypeError,
                            errorMessage, userData);
                        return;
                    }

                    throw new Exception(errorMessage);
                }

                var loadSceneTask = LoadSceneTask.Create(sceneAssetName, priority, resourceInfo,
                    dependencyAssetNames, loadSceneCallbacks, userData);
                foreach (var dependencyAssetName in dependencyAssetNames)
                {
                    if (!LoadDependencyAsset(sceneAssetName, priority, loadSceneTask, userData))
                    {
                        var errorMessage =
                            $"Can not load dependency asset ({dependencyAssetName}) when load scene asset ({sceneAssetName}).";
                        if (loadSceneCallbacks.LoadSceneFailureCallback != null)
                        {
                            loadSceneCallbacks.LoadSceneFailureCallback(sceneAssetName, LoadResourceStatus.TypeError,
                                errorMessage, userData);
                            return;
                        }

                        throw new Exception(errorMessage);
                    }
                }

                mTaskPool.AddTask(loadSceneTask);
                if (!resourceInfo.Ready)
                {
                    mResourceManager.UpdateResource(resourceInfo.ResourceName);
                }
            }

            /// <summary>
            /// 卸载场景
            /// </summary>
            /// <param name="sceneAssetName">场景资源名称</param>
            /// <param name="unloadSceneCallbacks">卸载场景回调函数集</param>
            /// <param name="userData">用户自定义数据</param>
            public void UnloadScene(string sceneAssetName, UnloadSceneCallbacks unloadSceneCallbacks, object userData)
            {
                if (mResourceManager.mResourceHelper == null)
                {
                    throw new Exception("Resource helper is invalid.");
                }

                if (mSceneToAssetMap.TryGetValue(sceneAssetName, out var asset))
                {
                    mSceneToAssetMap.Remove(sceneAssetName);
                    mAssetPool.UnSpawn(asset);
                    mAssetPool.ReleaseObject(asset);
                }
                else
                {
                    throw new Exception($"Can not find asset of scene ({sceneAssetName}).");
                }

                mResourceManager.mResourceHelper.UnloadScene(sceneAssetName, unloadSceneCallbacks, userData);
            }

            /// <summary>
            /// 获取二进制资源路径
            /// </summary>
            /// <param name="binaryAssetName">二进制资源名称</param>
            /// <returns>二进制资源路径</returns>
            /// <remarks>此方法适用于二进制资源存储在磁盘中（非文件系统中）</remarks>
            public string GetBinaryPath(string binaryAssetName)
            {
                var resourceInfo = GetResourceInfo(binaryAssetName);
                if (resourceInfo == null || !resourceInfo.Ready || !resourceInfo.IsLoadFromBinary ||
                    resourceInfo.UseFileSystem)
                {
                    return null;
                }

                var path = resourceInfo.StorageInReadOnly
                    ? mResourceManager.mReadOnlyPath
                    : mResourceManager.mReadWritePath;
                return Utility.Path.GetRegularPath(Path.Combine(path, resourceInfo.ResourceName.FullName));
            }

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
                out string relativePath, out string fileName)
            {
                storageInReadOnly = false;
                storageInFileSystem = false;
                relativePath = null;
                fileName = null;

                var resourceInfo = GetResourceInfo(binaryAssetName);
                if (resourceInfo == null || !resourceInfo.Ready || !resourceInfo.IsLoadFromBinary)
                {
                    return false;
                }

                storageInReadOnly = resourceInfo.StorageInReadOnly;
                if (resourceInfo.UseFileSystem)
                {
                    storageInFileSystem = true;
                    relativePath = $"{resourceInfo.FileSystemName}.{DefaultExtension}";
                    fileName = resourceInfo.ResourceName.FullName;
                }
                else
                {
                    relativePath = resourceInfo.ResourceName.FullName;
                }

                return true;
            }

            /// <summary>
            /// 获取二进制资源长度
            /// </summary>
            /// <param name="binaryAssetName">二进制资源名称</param>
            /// <returns>二进制资源长度</returns>
            public int GetBinaryLength(string binaryAssetName)
            {
                var resourceInfo = GetResourceInfo(binaryAssetName);
                if (resourceInfo == null || !resourceInfo.Ready || !resourceInfo.IsLoadFromBinary)
                {
                    return -1;
                }

                return resourceInfo.Length;
            }

            /// <summary>
            /// 加载二进制资源
            /// </summary>
            /// <param name="binaryAssetName">二进制资源名称</param>
            /// <param name="loadBinaryCallbacks">加载二进制资源回调函数</param>
            /// <param name="userData">用户自定义数据</param>
            public void LoadBinary(string binaryAssetName, LoadBinaryCallbacks loadBinaryCallbacks, object userData)
            {
                var resourceInfo = GetResourceInfo(binaryAssetName);
                if (resourceInfo == null)
                {
                    var errorMessage = $"Can not load binary asset ({binaryAssetName}) which is not exist.";
                    if (loadBinaryCallbacks.LoadBinaryFailureCallback != null)
                    {
                        loadBinaryCallbacks.LoadBinaryFailureCallback(binaryAssetName, LoadResourceStatus.NotExist,
                            errorMessage, userData);
                        return;
                    }

                    throw new Exception(errorMessage);
                }

                if (!resourceInfo.Ready)
                {
                    var errorMessage = $"Can not load binary asset ({binaryAssetName}) which is not ready.";
                    if (loadBinaryCallbacks.LoadBinaryFailureCallback != null)
                    {
                        loadBinaryCallbacks.LoadBinaryFailureCallback(binaryAssetName, LoadResourceStatus.NotReady,
                            errorMessage, userData);
                        return;
                    }

                    throw new Exception(errorMessage);
                }

                if (!resourceInfo.IsLoadFromBinary)
                {
                    var errorMessage = $"Can not load binary asset ({binaryAssetName}) which is not a binary asset.";
                    if (loadBinaryCallbacks.LoadBinaryFailureCallback != null)
                    {
                        loadBinaryCallbacks.LoadBinaryFailureCallback(binaryAssetName, LoadResourceStatus.TypeError,
                            errorMessage, userData);
                        return;
                    }

                    throw new Exception(errorMessage);
                }

                if (resourceInfo.UseFileSystem)
                {
                    loadBinaryCallbacks.LoadBinarySuccessCallback?.Invoke(binaryAssetName,
                        LoadBinaryFromFileSystem(binaryAssetName), 0f, userData);
                }
                else
                {
                    var path = Utility.Path.GetRemotePath(Path.Combine(
                        resourceInfo.StorageInReadOnly
                            ? mResourceManager.mReadOnlyPath
                            : mResourceManager.mReadWritePath, resourceInfo.ResourceName.FullName));
                    mResourceManager.mResourceHelper.LoadBytes(path, mLoadBytesCallbacks,
                        LoadBinaryInfo.Create(binaryAssetName, resourceInfo, loadBinaryCallbacks, userData));
                }
            }

            /// <summary>
            /// 从文件系统加载二进制资源
            /// </summary>
            /// <param name="binaryAssetName">二进制资源名称</param>
            /// <returns>存储二进制资源的二进制流</returns>
            public byte[] LoadBinaryFromFileSystem(string binaryAssetName)
            {
                var resourceInfo = GetResourceInfo(binaryAssetName);
                if (resourceInfo == null)
                {
                    throw new Exception(
                        $"Can not load binary asset ({binaryAssetName}) from file system which is not exist.");
                }

                if (!resourceInfo.Ready)
                {
                    throw new Exception(
                        $"Can not load binary asset ({binaryAssetName}) from file system which is not ready.");
                }

                if (!resourceInfo.IsLoadFromBinary)
                {
                    throw new Exception(
                        $"Can not load binary asset ({binaryAssetName}) from file system which is not a binary asset.");
                }

                if (!resourceInfo.UseFileSystem)
                {
                    throw new Exception(
                        $"Can not load binary asset ({binaryAssetName}) from file system which is not use file system.");
                }

                var fileSystem =
                    mResourceManager.GetFileSystem(resourceInfo.FileSystemName, resourceInfo.StorageInReadOnly);
                var bytes = fileSystem.ReadFile(resourceInfo.ResourceName.FullName);
                if (bytes == null)
                {
                    return null;
                }

                if (resourceInfo.LoadType == LoadType.LoadFromBinaryDecrypt ||
                    resourceInfo.LoadType == LoadType.LoadFromBinaryAndQuickDecrypt)
                {
                    var decryptResourceCallback =
                        mResourceManager.mDecryptResourceCallback ?? DefaultDecryptResourceCallback;
                    decryptResourceCallback(bytes, 0, bytes.Length, resourceInfo.ResourceName.Name,
                        resourceInfo.ResourceName.Variant, resourceInfo.ResourceName.Extension,
                        resourceInfo.StorageInReadOnly, resourceInfo.FileSystemName, (byte)resourceInfo.LoadType,
                        resourceInfo.Length, resourceInfo.HashCode);
                }

                return bytes;
            }


            /// <summary>
            /// 从文件系统加载二进制资源
            /// </summary>
            /// <param name="binaryAssetName">二进制资源名称</param>
            /// <param name="buffer">存储二进制资源的二进制流</param>
            /// <param name="startIndex">二进制流的起始位置</param>
            /// <param name="length">二进制流的长度</param>
            /// <returns>加载的字节数</returns>
            public int LoadBinaryFromFileSystem(string binaryAssetName, byte[] buffer, int startIndex, int length)
            {
                var resourceInfo = GetResourceInfo(binaryAssetName);
                if (resourceInfo == null)
                {
                    throw new Exception(
                        $"Can not load binary asset ({binaryAssetName}) from file system which is not exist.");
                }

                if (!resourceInfo.Ready)
                {
                    throw new Exception(
                        $"Can not load binary asset ({binaryAssetName}) from file system which is not ready.");
                }

                if (!resourceInfo.IsLoadFromBinary)
                {
                    throw new Exception(
                        $"Can not load binary asset ({binaryAssetName}) from file system which is not a binary asset.");
                }

                if (!resourceInfo.UseFileSystem)
                {
                    throw new Exception(
                        $"Can not load binary asset ({binaryAssetName}) from file system which is not use file system.");
                }

                var fileSystem =
                    mResourceManager.GetFileSystem(resourceInfo.FileSystemName, resourceInfo.StorageInReadOnly);
                var bytesRead = fileSystem.ReadFile(resourceInfo.ResourceName.FullName, buffer, startIndex, length);
                if (resourceInfo.LoadType == LoadType.LoadFromBinaryDecrypt ||
                    resourceInfo.LoadType == LoadType.LoadFromBinaryAndQuickDecrypt)
                {
                    var decryptResourceCallback =
                        mResourceManager.mDecryptResourceCallback ?? DefaultDecryptResourceCallback;
                    decryptResourceCallback(buffer, startIndex, bytesRead, resourceInfo.ResourceName.Name,
                        resourceInfo.ResourceName.Variant, resourceInfo.ResourceName.Extension,
                        resourceInfo.StorageInReadOnly, resourceInfo.FileSystemName, (byte)resourceInfo.LoadType,
                        resourceInfo.Length, resourceInfo.HashCode);
                }

                return bytesRead;
            }

            /// <summary>
            /// 从文件系统加载二进制资源片段
            /// </summary>
            /// <param name="binaryAssetName">二进制资源名称</param>
            /// <param name="offset">加载片段的偏移</param>
            /// <param name="length">加载片段的长度</param>
            /// <returns>存储二进制资源片段的二进制流</returns>
            public byte[] LoadBinarySegmentFromFileSystem(string binaryAssetName, int offset, int length)
            {
                var resourceInfo = GetResourceInfo(binaryAssetName);
                if (resourceInfo == null)
                {
                    throw new Exception(
                        $"Can not load binary asset ({binaryAssetName}) from file system which is not exist.");
                }

                if (!resourceInfo.Ready)
                {
                    throw new Exception(
                        $"Can not load binary asset ({binaryAssetName}) from file system which is not ready.");
                }

                if (!resourceInfo.IsLoadFromBinary)
                {
                    throw new Exception(
                        $"Can not load binary asset ({binaryAssetName}) from file system which is not a binary asset.");
                }

                if (!resourceInfo.UseFileSystem)
                {
                    throw new Exception(
                        $"Can not load binary asset ({binaryAssetName}) from file system which is not use file system.");
                }

                var fileSystem =
                    mResourceManager.GetFileSystem(resourceInfo.FileSystemName, resourceInfo.StorageInReadOnly);
                var bytes = fileSystem.ReadFileSegment(resourceInfo.ResourceName.FullName, offset, length);
                if (bytes == null)
                {
                    return null;
                }

                if (resourceInfo.LoadType == LoadType.LoadFromBinaryDecrypt ||
                    resourceInfo.LoadType == LoadType.LoadFromBinaryAndQuickDecrypt)
                {
                    var decryptResourceCallback =
                        mResourceManager.mDecryptResourceCallback ?? DefaultDecryptResourceCallback;
                    decryptResourceCallback(bytes, 0, bytes.Length, resourceInfo.ResourceName.Name,
                        resourceInfo.ResourceName.Variant, resourceInfo.ResourceName.Extension,
                        resourceInfo.StorageInReadOnly, resourceInfo.FileSystemName, (byte)resourceInfo.LoadType,
                        resourceInfo.Length, resourceInfo.HashCode);
                }

                return bytes;
            }

            /// <summary>
            /// 从文件系统加载二进制资源片段
            /// </summary>
            /// <param name="binaryAssetName">二进制资源名称</param>
            /// <param name="offset">加载片段的偏移</param>
            /// <param name="buffer">存储二进制资源的二进制流</param>
            /// <param name="startIndex">加载片段的起始位置</param>
            /// <param name="length">加载片段的长度</param>
            /// <returns>加载片段的字节数</returns>
            public int LoadBinarySegmentFromFileSystem(string binaryAssetName, int offset, byte[] buffer,
                int startIndex, int length)
            {
                var resourceInfo = GetResourceInfo(binaryAssetName);
                if (resourceInfo == null)
                {
                    throw new Exception(
                        $"Can not load binary asset ({binaryAssetName}) from file system which is not exist.");
                }

                if (!resourceInfo.Ready)
                {
                    throw new Exception(
                        $"Can not load binary asset ({binaryAssetName}) from file system which is not ready.");
                }

                if (!resourceInfo.IsLoadFromBinary)
                {
                    throw new Exception(
                        $"Can not load binary asset ({binaryAssetName}) from file system which is not a binary asset.");
                }

                if (!resourceInfo.UseFileSystem)
                {
                    throw new Exception(
                        $"Can not load binary asset ({binaryAssetName}) from file system which is not use file system.");
                }

                var fileSystem =
                    mResourceManager.GetFileSystem(resourceInfo.FileSystemName, resourceInfo.StorageInReadOnly);
                var bytesRead = fileSystem.ReadFileSegment(resourceInfo.ResourceName.FullName, offset, buffer,
                    startIndex, length);
                if (resourceInfo.LoadType == LoadType.LoadFromBinaryDecrypt ||
                    resourceInfo.LoadType == LoadType.LoadFromBinaryAndQuickDecrypt)
                {
                    var decryptResourceCallback =
                        mResourceManager.mDecryptResourceCallback ?? DefaultDecryptResourceCallback;
                    decryptResourceCallback(buffer, startIndex, bytesRead, resourceInfo.ResourceName.Name,
                        resourceInfo.ResourceName.Variant, resourceInfo.ResourceName.Extension,
                        resourceInfo.StorageInReadOnly, resourceInfo.FileSystemName, (byte)resourceInfo.LoadType,
                        resourceInfo.Length, resourceInfo.HashCode);
                }

                return bytesRead;
            }

            /// <summary>
            /// 获取所有加载资源的任务信息
            /// </summary>
            /// <returns>所有加载资源的任务信息</returns>
            public TaskInfo[] GetAllLoadAssetInfos()
            {
                return mTaskPool.GetAllTaskInfos();
            }

            /// <summary>
            /// 获取所有加载资源的任务信息
            /// </summary>
            /// <param name="results">所有加载资源的任务信息</param>
            public void GetAllLoadAssetInfos(List<TaskInfo> results)
            {
                mTaskPool.GetAllTaskInfos(results);
            }

            private bool LoadDependencyAsset(string assetName, int priority, LoadResourceTaskBase loadResourceTask,
                object userData)
            {
                if (string.IsNullOrEmpty(assetName))
                {
                    return false;
                }

                if (loadResourceTask == null)
                {
                    throw new Exception("Load Resource Task is invalid.");
                }

                if (!CheckAsset(assetName, out var resourceInfo, out var dependencyAssetNames))
                {
                    return false;
                }

                if (resourceInfo.IsLoadFromBinary)
                {
                    return false;
                }

                var loadDependencyTask = LoadDependencyAssetTask.Create(assetName, priority, resourceInfo,
                    dependencyAssetNames, loadResourceTask, userData);
                foreach (var dependencyAssetName in dependencyAssetNames)
                {
                    if (!LoadDependencyAsset(dependencyAssetName, priority, loadResourceTask, userData))
                    {
                        return false;
                    }
                }

                mTaskPool.AddTask(loadDependencyTask);
                if (resourceInfo.Ready)
                {
                    mResourceManager.UpdateResource(resourceInfo.ResourceName);
                }

                return true;
            }

            private ResourceInfo GetResourceInfo(string assetName)
            {
                if (string.IsNullOrEmpty(assetName))
                {
                    throw new Exception("Asset name is invalid.");
                }

                var assetInfo = mResourceManager.GetAssetInfo(assetName);
                if (assetInfo == null)
                {
                    return null;
                }

                return mResourceManager.GetResourceInfo(assetInfo.ResourceName);
            }

            private bool CheckAsset(string assetName, out ResourceInfo resourceInfo, out string[] dependencyAssetNames)
            {
                resourceInfo = null;
                dependencyAssetNames = null;

                if (string.IsNullOrEmpty(assetName))
                {
                    return false;
                }

                var assetInfo = mResourceManager.GetAssetInfo(assetName);
                if (assetInfo == null)
                {
                    return false;
                }

                resourceInfo = mResourceManager.GetResourceInfo(assetInfo.ResourceName);
                if (resourceInfo == null)
                {
                    return false;
                }

                dependencyAssetNames = assetInfo.DependencyAssetNames;
                return mResourceManager.mResourceMode == ResourceMode.UpdatableWhilePlaying ? true : resourceInfo.Ready;
            }

            private void DefaultDecryptResourceCallback(byte[] bytes, int startIndex, int count, string name,
                string variant, string extension, bool storageInReadOnly, string filesystem, byte loadType, int length,
                int hashCode)
            {
                Utility.Converter.GetBytes(hashCode, mCachedHashBytes);
                switch ((LoadType)loadType)
                {
                    case LoadType.LoadFromMemoryAndQuickDecrypt:
                    case LoadType.LoadFromBinaryAndQuickDecrypt:
                        Utility.Encryption.GetQuickSelfXorBytes(bytes, mCachedHashBytes);
                        break;
                    case LoadType.LoadFormMemoryDecrypt:
                    case LoadType.LoadFromBinaryDecrypt:
                        Utility.Encryption.GetSelfXorBytes(bytes, mCachedHashBytes);
                        break;
                    default:
                        throw new Exception("Not supported load type when decrypt resource.");
                }

                Array.Clear(mCachedHashBytes, 0, CachedBytesLength);
            }

            private void OnLoadBinarySuccess(string fileUri, byte[] bytes, float duration, object userData)
            {
                var loadBinaryInfo = userData as LoadBinaryInfo;
                if (loadBinaryInfo == null)
                {
                    throw new Exception("Load binary info in invalid.");
                }

                var resourceInfo = loadBinaryInfo.ResourceInfo;
                if (resourceInfo.LoadType == LoadType.LoadFromBinaryDecrypt ||
                    resourceInfo.LoadType == LoadType.LoadFromBinaryAndQuickDecrypt)
                {
                    var decryptResourceCallback =
                        mResourceManager.mDecryptResourceCallback ?? DefaultDecryptResourceCallback;
                    decryptResourceCallback(bytes, 0, bytes.Length, resourceInfo.ResourceName.Name,
                        resourceInfo.ResourceName.Variant, resourceInfo.ResourceName.Extension,
                        resourceInfo.StorageInReadOnly, resourceInfo.FileSystemName, (byte)resourceInfo.LoadType,
                        resourceInfo.Length, resourceInfo.HashCode);
                }

                loadBinaryInfo.LoadBinaryCallbacks.LoadBinarySuccessCallback?.Invoke(loadBinaryInfo.BinaryAssetName,
                    bytes, duration, loadBinaryInfo.UserData);
                ReferencePool.Release(loadBinaryInfo);
            }

            private void OnLoadBinaryFailure(string fileUri, string errorMessage, object userData)
            {
                var loadBinaryInfo = userData as LoadBinaryInfo;
                if (loadBinaryInfo == null)
                {
                    throw new Exception("Load binary info in invalid.");
                }

                loadBinaryInfo.LoadBinaryCallbacks.LoadBinaryFailureCallback?.Invoke(loadBinaryInfo.BinaryAssetName,
                    LoadResourceStatus.AssetError, errorMessage, userData);
                ReferencePool.Release(loadBinaryInfo);
            }
        }
    }
}