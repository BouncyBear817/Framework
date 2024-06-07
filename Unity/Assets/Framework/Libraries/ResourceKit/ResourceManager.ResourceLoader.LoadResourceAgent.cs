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
        private sealed partial class ResourceLoader
        {
            /// <summary>
            /// 加载资源代理
            /// </summary>
            private sealed class LoadResourceAgent : ITaskAgent<LoadResourceTaskBase>
            {
                private static readonly Dictionary<string, string> sCachedResourceNames =
                    new Dictionary<string, string>();

                private static readonly HashSet<string> sLoadingAssetNames = new HashSet<string>();
                private static readonly HashSet<string> sLoadingResourceNames = new HashSet<string>();

                private readonly ILoadResourceAgentHelper mAgentHelper;
                private readonly IResourceHelper mResourceHelper;
                private readonly ResourceLoader mResourceLoader;
                private readonly string mReadOnlyPath;
                private readonly string mReadWritePath;
                private readonly DecryptResourceCallback mDecryptResourceCallback;
                private LoadResourceTaskBase mTask;

                public LoadResourceAgent(ILoadResourceAgentHelper agentHelper, IResourceHelper resourceHelper,
                    ResourceLoader resourceLoader, string readOnlyPath, string readWritePath,
                    DecryptResourceCallback decryptResourceCallback)
                {
                    if (agentHelper == null)
                    {
                        throw new Exception("Load resource agent helper is invalid.");
                    }

                    if (resourceHelper == null)
                    {
                        throw new Exception("Resource helper is invalid.");
                    }

                    if (resourceLoader == null)
                    {
                        throw new Exception("Resource loader is invalid.");
                    }

                    if (decryptResourceCallback == null)
                    {
                        throw new Exception("Decrypt resource callback is invalid.");
                    }

                    mAgentHelper = agentHelper;
                    mResourceHelper = resourceHelper;
                    mResourceLoader = resourceLoader;
                    mReadOnlyPath = readOnlyPath;
                    mReadWritePath = readWritePath;
                    mDecryptResourceCallback = decryptResourceCallback;
                    mTask = null;
                }

                /// <summary>
                /// 加载资源代理辅助器
                /// </summary>
                public ILoadResourceAgentHelper AgentHelper => mAgentHelper;

                /// <summary>
                /// 任务
                /// </summary>
                public LoadResourceTaskBase Task => mTask;

                /// <summary>
                /// 初始化任务代理
                /// </summary>
                public void Initialize()
                {
                    mAgentHelper.LoadResourceAgentHelperUpdate += OnLoadResourceAgentHelperUpdate;
                    mAgentHelper.LoadResourceAgentHelperReadFileComplete += OnLoadResourceAgentHelperReadFileComplete;
                    mAgentHelper.LoadResourceAgentHelperReadBytesComplete += OnLoadResourceAgentHelperReadBytesComplete;
                    mAgentHelper.LoadResourceAgentHelperParseBytesComplete +=
                        OnLoadResourceAgentHelperParseBytesComplete;
                    mAgentHelper.LoadResourceAgentHelperLoadComplete += OnLoadResourceAgentHelperLoadComplete;
                    mAgentHelper.LoadResourceAgentHelperError += OnLoadResourceAgentHelperError;
                }

                public void Update(float elapseSeconds, float realElapseSeconds)
                {
                }

                /// <summary>
                /// 关闭并清理任务代理
                /// </summary>
                public void Shutdown()
                {
                    StopAndReset();

                    mAgentHelper.LoadResourceAgentHelperUpdate -= OnLoadResourceAgentHelperUpdate;
                    mAgentHelper.LoadResourceAgentHelperReadFileComplete -= OnLoadResourceAgentHelperReadFileComplete;
                    mAgentHelper.LoadResourceAgentHelperReadBytesComplete -= OnLoadResourceAgentHelperReadBytesComplete;
                    mAgentHelper.LoadResourceAgentHelperParseBytesComplete -=
                        OnLoadResourceAgentHelperParseBytesComplete;
                    mAgentHelper.LoadResourceAgentHelperLoadComplete -= OnLoadResourceAgentHelperLoadComplete;
                    mAgentHelper.LoadResourceAgentHelperError -= OnLoadResourceAgentHelperError;
                }

                /// <summary>
                /// 开始处理任务
                /// </summary>
                /// <param name="task">任务</param>
                /// <returns>开始处理任务的状态</returns>
                public StartTaskStatus Start(LoadResourceTaskBase task)
                {
                    if (task == null)
                    {
                        throw new Exception("Task is invalid.");
                    }

                    mTask = task;
                    mTask.StartTime = DateTime.UtcNow;
                    var resourceInfo = mTask.ResourceInfo;

                    if (!resourceInfo.Ready)
                    {
                        mTask.StartTime = default(DateTime);
                        return StartTaskStatus.HasToWait;
                    }

                    if (sLoadingAssetNames.Contains(mTask.AssetName))
                    {
                        mTask.StartTime = default(DateTime);
                        return StartTaskStatus.HasToWait;
                    }

                    if (!mTask.IsScene)
                    {
                        var assetObject = mResourceLoader.mAssetPool.Spawn(mTask.AssetName);
                        if (assetObject != null)
                        {
                            OnAssetObjectReady(assetObject);
                        }
                    }

                    foreach (var dependencyAssetName in mTask.GetDependencyAssetNames())
                    {
                        if (!mResourceLoader.mAssetPool.CanSpawn(dependencyAssetName))
                        {
                            mTask.StartTime = default(DateTime);
                            return StartTaskStatus.HasToWait;
                        }
                    }

                    var resourceName = resourceInfo.ResourceName.Name;
                    if (sLoadingResourceNames.Contains(resourceName))
                    {
                        mTask.StartTime = default(DateTime);
                        return StartTaskStatus.HasToWait;
                    }

                    sLoadingAssetNames.Add(mTask.AssetName);

                    var resourceObject = mResourceLoader.mResourcePool.Spawn(resourceName);
                    if (resourceObject != null)
                    {
                        mTask.LoadMain(this, resourceObject);
                        return StartTaskStatus.CanResume;
                    }

                    sLoadingResourceNames.Add(resourceName);

                    if (!sCachedResourceNames.TryGetValue(resourceName, out var fullPath))
                    {
                        var path1 = resourceInfo.StorageInReadOnly ? mReadOnlyPath : mReadWritePath;
                        var path2 = resourceInfo.UseFileSystem
                            ? resourceInfo.FileSystemName
                            : resourceInfo.ResourceName.FullName;
                        fullPath = Utility.Path.GetRegularPath(Path.Combine(path1, path2));
                        sCachedResourceNames.Add(resourceName, fullPath);
                    }

                    if (resourceInfo.LoadType == LoadType.LoadFromFile)
                    {
                        if (resourceInfo.UseFileSystem)
                        {
                            var fileSystem = mResourceLoader.mResourceManager.GetFileSystem(resourceInfo.FileSystemName,
                                resourceInfo.StorageInReadOnly);
                            mAgentHelper.ReadFile(fileSystem, resourceInfo.ResourceName.FullName);
                        }
                        else
                        {
                            mAgentHelper.ReadFile(fullPath);
                        }
                    }
                    else if (resourceInfo.LoadType == LoadType.LoadFromMemory ||
                             resourceInfo.LoadType == LoadType.LoadFromMemoryAndQuickDecrypt ||
                             resourceInfo.LoadType == LoadType.LoadFormMemoryDecrypt)
                    {
                        if (resourceInfo.UseFileSystem)
                        {
                            var fileSystem = mResourceLoader.mResourceManager.GetFileSystem(resourceInfo.FileSystemName,
                                resourceInfo.StorageInReadOnly);
                            mAgentHelper.ReadBytes(fileSystem, resourceInfo.ResourceName.FullName);
                        }
                        else
                        {
                            mAgentHelper.ReadBytes(fullPath);
                        }
                    }
                    else
                    {
                        throw new Exception($"Resource load type ({resourceInfo.LoadType}) is not supported.");
                    }

                    return StartTaskStatus.CanResume;
                }

                /// <summary>
                /// 停止正在处理的任务并重置任务代理
                /// </summary>
                public void StopAndReset()
                {
                    mAgentHelper.Reset();
                    mTask = null;
                }

                public static void Clear()
                {
                    sCachedResourceNames.Clear();
                    sLoadingAssetNames.Clear();
                    sLoadingResourceNames.Clear();
                }

                private void OnAssetObjectReady(AssetObject assetObject)
                {
                    mAgentHelper.Reset();
                    var asset = assetObject.Target;
                    if (mTask.IsScene)
                    {
                        mResourceLoader.mSceneToAssetMap.Add(mTask.AssetName, asset);
                    }

                    mTask.OnLoadAssetSuccess(this, asset, (float)(DateTime.UtcNow - mTask.StartTime).TotalSeconds);
                    mTask.Done = true;
                }

                private void OnLoadResourceAgentHelperUpdate(object sender, LoadResourceAgentHelperUpdateEventArgs e)
                {
                    mTask.OnLoadAssetUpdate(this, e.Type, e.Progress);
                }

                private void OnLoadResourceAgentHelperReadFileComplete(object sender,
                    LoadResourceAgentHelperReadFileCompleteEventArgs e)
                {
                    var resourceObject = ResourceObject.Create(mTask.ResourceInfo.ResourceName.Name, e.Resource,
                        mResourceHelper, mResourceLoader);
                    mResourceLoader.mResourcePool.Register(resourceObject, true);
                    sLoadingResourceNames.Remove(mTask.ResourceInfo.ResourceName.Name);
                    mTask.LoadMain(this, resourceObject);
                }

                private void OnLoadResourceAgentHelperReadBytesComplete(object sender,
                    LoadResourceAgentHelperReadBytesCompleteEventArgs e)
                {
                    var bytes = e.Bytes;
                    var resourceInfo = mTask.ResourceInfo;
                    if (resourceInfo.LoadType == LoadType.LoadFormMemoryDecrypt ||
                        resourceInfo.LoadType == LoadType.LoadFromMemoryAndQuickDecrypt)
                    {
                        mDecryptResourceCallback(bytes, 0, bytes.Length, resourceInfo.ResourceName.Name,
                            resourceInfo.ResourceName.Variant, resourceInfo.ResourceName.Variant,
                            resourceInfo.StorageInReadOnly, resourceInfo.FileSystemName, (byte)resourceInfo.LoadType,
                            resourceInfo.Length, resourceInfo.CompressedLength);
                    }

                    mAgentHelper.ParseBytes(bytes);
                }

                private void OnLoadResourceAgentHelperParseBytesComplete(object sender,
                    LoadResourceAgentHelperParseBytesCompleteEventArgs e)
                {
                    var resourceObject = ResourceObject.Create(mTask.ResourceInfo.ResourceName.Name, e.Resource,
                        mResourceHelper, mResourceLoader);
                    mResourceLoader.mResourcePool.Register(resourceObject, true);
                    sLoadingResourceNames.Remove(mTask.ResourceInfo.ResourceName.Name);
                    mTask.LoadMain(this, resourceObject);
                }

                private void OnLoadResourceAgentHelperLoadComplete(object sender,
                    LoadResourceAgentHelperLoadCompleteEventArgs e)
                {
                    AssetObject assetObject = null;
                    if (mTask.IsScene)
                    {
                        assetObject = mResourceLoader.mAssetPool.Spawn(mTask.AssetName);
                    }

                    if (assetObject == null)
                    {
                        var dependencyAssets = mTask.GetDependencyAssets();
                        assetObject = AssetObject.Create(mTask.AssetName, e.Asset, dependencyAssets,
                            mTask.ResourceObject.Target, mResourceHelper, mResourceLoader);
                        mResourceLoader.mAssetPool.Register(assetObject, true);
                        mResourceLoader.mAssetToResourceMap.Add(e.Asset, mTask.ResourceObject.Target);
                        foreach (var dependencyAsset in dependencyAssets)
                        {
                            if (mResourceLoader.mAssetToResourceMap.TryGetValue(dependencyAsset,
                                    out var dependencyResource))
                            {
                                mTask.ResourceObject.AddDependencyResource(dependencyResource);
                            }
                            else
                            {
                                throw new Exception("Can not find dependency resource.");
                            }
                        }
                    }

                    sLoadingAssetNames.Remove(mTask.AssetName);
                    OnAssetObjectReady(assetObject);
                }

                private void OnLoadResourceAgentHelperError(object sender, LoadResourceAgentHelperErrorEventArgs e)
                {
                    mAgentHelper.Reset();
                    mTask.OnLoadAssetFailure(this, e.Status, e.ErrorMessage);
                    sLoadingAssetNames.Remove(mTask.AssetName);
                    sLoadingResourceNames.Remove(mTask.ResourceInfo.ResourceName.Name);
                    mTask.Done = true;
                }
            }
        }
    }
}