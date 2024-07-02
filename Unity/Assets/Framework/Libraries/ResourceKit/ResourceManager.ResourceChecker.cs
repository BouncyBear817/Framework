using System;
using System.Collections.Generic;
using System.IO;

namespace Framework
{
    public sealed partial class ResourceManager : FrameworkModule, IResourceManager
    {
        /// <summary>
        /// 资源检查器
        /// </summary>
        private sealed partial class ResourceChecker
        {
            private readonly ResourceManager mResourceManager;
            private readonly Dictionary<ResourceName, CheckInfo> mCheckInfos;
            private string mCurrentVariant;
            private bool mIgnoreOtherVariant;
            private bool mUpdatableVersionListReady;
            private bool mReadOnlyVersionListReady;
            private bool mReadWriteVersionListReady;

            public Action<ResourceName, string, LoadType, int, int, int, int> ResourceNeedUpdate;
            public Action<int, int, int, long, long> ResourceCheckComplete;

            public ResourceChecker(ResourceManager resourceManager)
            {
                mResourceManager = resourceManager;
                mCheckInfos = new Dictionary<ResourceName, CheckInfo>();
                mCurrentVariant = null;
                mIgnoreOtherVariant = false;
                mUpdatableVersionListReady = false;
                mReadOnlyVersionListReady = false;
                mReadWriteVersionListReady = false;

                ResourceNeedUpdate = null;
                ResourceCheckComplete = null;
            }

            public void Shutdown()
            {
                mCheckInfos.Clear();
            }

            public void CheckResources(string currentVariant, bool ignoreOtherVariant)
            {
                if (mResourceManager.mResourceHelper == null)
                {
                    throw new Exception("Resource helper is invalid.");
                }

                if (string.IsNullOrEmpty(mResourceManager.mReadOnlyPath))
                {
                    throw new Exception("Read-only path is invalid.");
                }

                if (string.IsNullOrEmpty(mResourceManager.mReadWritePath))
                {
                    throw new Exception("Read-write path is invalid.");
                }

                mCurrentVariant = currentVariant;
                mIgnoreOtherVariant = ignoreOtherVariant;

                mResourceManager.mResourceHelper.LoadBytes(
                    Utility.Path.GetRemotePath(Path.Combine(mResourceManager.mReadWritePath,
                        RemoteVersionListFileName)),
                    new LoadBytesCallbacks(OnLoadUpdatableVersionListSuccess, OnLoadUpdatableVersionListFailure), null);
                mResourceManager.mResourceHelper.LoadBytes(
                    Utility.Path.GetRemotePath(Path.Combine(mResourceManager.mReadOnlyPath,
                        LocalVersionListFileName)),
                    new LoadBytesCallbacks(OnLoadReadOnlyVersionListSuccess, OnLoadReadOnlyVersionListFailure), null);
                mResourceManager.mResourceHelper.LoadBytes(
                    Utility.Path.GetRemotePath(Path.Combine(mResourceManager.mReadWritePath,
                        LocalVersionListFileName)),
                    new LoadBytesCallbacks(OnLoadReadWriteVersionListSuccess, OnLoadReadWriteVersionListFailure), null);
            }

            private void SetCachedFileSystemName(ResourceName resourceName, string fileSystemName)
            {
                GetOrAddCheckInfo(resourceName).SetCachedFileSystemName(fileSystemName);
            }

            /// <summary>
            /// 设置资源在远程版本中的信息
            /// </summary>
            /// <param name="resourceName">资源名称</param>
            /// <param name="loadType">资源加载方式</param>
            /// <param name="length">资源大小</param>
            /// <param name="hashCode">资源哈希值</param>
            /// <param name="compressedLength">压缩后资源大小</param>
            /// <param name="compressedHashCode">压缩后资源哈希值</param>
            /// <exception cref="Exception"></exception>
            private void SetRemoteVersionInfo(ResourceName resourceName, LoadType loadType, int length, int hashCode,
                int compressedLength,
                int compressedHashCode)
            {
                GetOrAddCheckInfo(resourceName)
                    .SetRemoteVersionInfo(loadType, length, hashCode, compressedLength, compressedHashCode);
            }

            /// <summary>
            /// 设置资源在只读区的信息
            /// </summary>
            /// <param name="resourceName">资源名称</param>
            /// <param name="loadType">资源加载方式</param>
            /// <param name="length">资源大小</param>
            /// <param name="hashCode">资源哈希值</param>
            /// <exception cref="Exception"></exception>
            private void SetReadOnlyInfo(ResourceName resourceName, LoadType loadType, int length, int hashCode)
            {
                GetOrAddCheckInfo(resourceName).SetReadOnlyInfo(loadType, length, hashCode);
            }

            /// <summary>
            /// 设置资源在读写区的信息
            /// </summary>
            /// <param name="resourceName">资源名称</param>
            /// <param name="loadType">资源加载方式</param>
            /// <param name="length">资源大小</param>
            /// <param name="hashCode">资源哈希值</param>
            /// <exception cref="Exception"></exception>
            private void SetReadWriteInfo(ResourceName resourceName, LoadType loadType, int length, int hashCode)
            {
                GetOrAddCheckInfo(resourceName).SetReadWriteInfo(loadType, length, hashCode);
            }

            private CheckInfo GetOrAddCheckInfo(ResourceName resourceName)
            {
                if (mCheckInfos.TryGetValue(resourceName, out var checkInfo))
                {
                    return checkInfo;
                }

                checkInfo = new CheckInfo(resourceName);
                mCheckInfos.Add(resourceName, checkInfo);
                return checkInfo;
            }

            private void RefreshCheckInfoStatus()
            {
                if (!mUpdatableVersionListReady || !mReadOnlyVersionListReady || !mReadWriteVersionListReady)
                {
                    return;
                }

                var movedCount = 0;
                var removedCount = 0;
                var updateCount = 0;
                var updateTotalLength = 0L;
                var updateTotalCompressedLength = 0L;
                foreach (var (resourceName, checkInfo) in mCheckInfos)
                {
                    checkInfo.RefreshStatus(mCurrentVariant, mIgnoreOtherVariant);
                    switch (checkInfo.Status)
                    {
                        case CheckInfo.CheckStatus.StorageInReadOnly:
                            mResourceManager.mResourceInfos.Add(resourceName, new ResourceInfo(checkInfo));
                            break;
                        case CheckInfo.CheckStatus.StorageInReadWrite:
                        {
                            if (checkInfo.NeedMoveToDisk || checkInfo.NeedMoveToFileSystem)
                            {
                                movedCount++;
                                var resourceFullName = checkInfo.ResourceName.FullName;
                                var resourcePath = Utility.Path.GetRegularPath(Path.Combine(mResourceManager.mReadWritePath, resourceFullName));
                                if (checkInfo.NeedMoveToDisk)
                                {
                                    var fileSystem = mResourceManager.GetFileSystem(checkInfo.ReadWriteFileSystemName, false);
                                    if (fileSystem.SaveAsFile(resourceFullName, resourcePath))
                                    {
                                        throw new Exception($"Save as file ({resourceFullName}) to ({resourcePath}) from file system ({fileSystem.FullPath}) error.");
                                    }

                                    fileSystem.DeleteFile(resourceFullName);
                                }

                                if (checkInfo.NeedMoveToFileSystem)
                                {
                                    var fileSystem = mResourceManager.GetFileSystem(checkInfo.FileSystemName, false);
                                    if (fileSystem.WriteFile(resourceFullName, resourcePath))
                                    {
                                        throw new Exception($"Write resource ({resourceFullName}) to file system ({fileSystem.FullPath}) error.");
                                    }

                                    if (File.Exists(resourcePath))
                                    {
                                        File.Delete(resourcePath);
                                    }
                                }
                            }

                            mResourceManager.mResourceInfos.Add(checkInfo.ResourceName,
                                new ResourceInfo(checkInfo.ResourceName, checkInfo.FileSystemName, checkInfo.LoadType, checkInfo.Length, checkInfo.HashCode,
                                    checkInfo.CompressedLength, false, true));
                            mResourceManager.mReadWriteResourceInfos.Add(checkInfo.ResourceName,
                                new ReadWriteResourceInfo(checkInfo.FileSystemName, checkInfo.LoadType, checkInfo.Length, checkInfo.HashCode));
                        }
                            break;
                        case CheckInfo.CheckStatus.Update:
                        {
                            mResourceManager.mResourceInfos.Add(resourceName, new ResourceInfo(checkInfo));
                            updateCount++;
                            updateTotalLength += checkInfo.Length;
                            updateTotalCompressedLength += checkInfo.CompressedLength;
                            if (ResourceNeedUpdate != null)
                            {
                                ResourceNeedUpdate(checkInfo.ResourceName, checkInfo.FileSystemName, checkInfo.LoadType,
                                    checkInfo.Length, checkInfo.HashCode, checkInfo.CompressedLength,
                                    checkInfo.CompressedHashCode);
                            }
                        }
                            break;
                        case CheckInfo.CheckStatus.Unavailable:
                        case CheckInfo.CheckStatus.Disuse:
                            //Do nothing
                            break;
                        default:
                            throw new Exception($"Check resources ({resourceName}) error with unknown status.");
                    }

                    if (checkInfo.NeedRemove)
                    {
                        removedCount++;
                        if (checkInfo.ReadWriteUseFileSystem)
                        {
                            var fileSystem = mResourceManager.GetFileSystem(checkInfo.ReadWriteFileSystemName, false);
                            fileSystem.DeleteFile(checkInfo.ResourceName.FullName);
                        }
                        else
                        {
                            var resourcePath = Utility.Path.GetRegularPath(Path.Combine(mResourceManager.mReadWritePath, checkInfo.ResourceName.FullName));
                            if (File.Exists(resourcePath))
                            {
                                File.Delete(resourcePath);
                            }
                        }
                    }
                }

                if (movedCount > 0 || removedCount > 0)
                {
                    RemoveEmptyFileSystems();
                    Utility.Path.RemoveEmptyDirectory(mResourceManager.mReadWritePath);
                }

                ResourceCheckComplete?.Invoke(movedCount, removedCount, updateCount, updateTotalLength, updateTotalCompressedLength);
            }

            private void RemoveEmptyFileSystems()
            {
                List<string> removedFileSystemNames = null;
                foreach (var (name, fileSystem) in mResourceManager.mReadWriteFileSystems)
                {
                    if (fileSystem.FileCount <= 0)
                    {
                        removedFileSystemNames ??= new List<string>();

                        mResourceManager.mFileSystemManager.DestroyFileSystem(fileSystem, true);
                        removedFileSystemNames.Add(name);
                    }
                }

                if (removedFileSystemNames != null)
                {
                    foreach (var fileSystemName in removedFileSystemNames)
                    {
                        mResourceManager.mReadWriteFileSystems.Remove(fileSystemName);
                    }
                }
            }

            private void OnLoadUpdatableVersionListSuccess(string fileUri, byte[] bytes, float duration, object userData)
            {
                if (mUpdatableVersionListReady)
                {
                    throw new Exception("Updatable version list has been parsed.");
                }

                MemoryStream memoryStream = null;
                try
                {
                    memoryStream = new MemoryStream(bytes, false);
                    var versionList = mResourceManager.mUpdatableVersionListSerializer.Deserialize(memoryStream);
                    if (!versionList.IsValid)
                    {
                        throw new Exception("Deserialize updatable version list failure.");
                    }

                    var assets = versionList.Assets;
                    var resources = versionList.Resources;
                    var fileSystems = versionList.FileSystems;
                    var resourceGroups = versionList.ResourceGroups;

                    mResourceManager.mApplicableVersion = versionList.ApplicableVersion;
                    mResourceManager.mInternalResourceVersion = versionList.InternalResourceVersion;
                    mResourceManager.mAssetInfos = new Dictionary<string, AssetInfo>(assets.Length, StringComparer.Ordinal);
                    mResourceManager.mResourceInfos = new Dictionary<ResourceName, ResourceInfo>(resources.Length, new ResourceNameComparer());
                    mResourceManager.mReadWriteResourceInfos = new SortedDictionary<ResourceName, ReadWriteResourceInfo>(new ResourceNameComparer());

                    var defaultResourceGroup = mResourceManager.GetOrAddResourceGroup(string.Empty);

                    foreach (var fileSystem in fileSystems)
                    {
                        var resourceIndexes = fileSystem.ResourceIndexes;
                        foreach (var resourceIndex in resourceIndexes)
                        {
                            var resource = resources[resourceIndex];
                            if (resource.Variant != null && resource.Variant != mCurrentVariant)
                            {
                                continue;
                            }

                            SetCachedFileSystemName(new ResourceName(resource.Name, resource.Variant, resource.Extension), fileSystem.Name);
                        }
                    }

                    foreach (var resource in resources)
                    {
                        if (resource.Variant != null && resource.Variant != mCurrentVariant)
                        {
                            continue;
                        }

                        var resourceName = new ResourceName(resource.Name, resource.Variant, resource.Extension);
                        var assetIndexes = resource.AssetIndexes;
                        foreach (var assetIndex in assetIndexes)
                        {
                            var asset = assets[assetIndex];
                            var dependencyAssetIndexes = asset.DependencyAssetIndexes;
                            var index = 0;
                            var dependencyAssetNames = new string[dependencyAssetIndexes.Length];
                            foreach (var dependencyAssetIndex in dependencyAssetIndexes)
                            {
                                dependencyAssetNames[index++] = assets[dependencyAssetIndex].Name;
                            }

                            mResourceManager.mAssetInfos.Add(asset.Name, new AssetInfo(asset.Name, resourceName, dependencyAssetNames));
                        }

                        SetRemoteVersionInfo(resourceName, (LoadType)resource.LoadType, resource.Length, resource.HashCode, resource.CompressedLength, resource.CompressedHashCode);
                        defaultResourceGroup.AddResource(resourceName, resource.Length, resource.CompressedLength);
                    }

                    foreach (var resourceGroup in resourceGroups)
                    {
                        var group = mResourceManager.GetOrAddResourceGroup(resourceGroup.Name);
                        var resourceIndexes = resourceGroup.ResourceIndexes;
                        foreach (var resourceIndex in resourceIndexes)
                        {
                            var resource = resources[resourceIndex];
                            if (resource.Variant != null && resource.Variant != mCurrentVariant)
                            {
                                continue;
                            }

                            group.AddResource(new ResourceName(resource.Name, resource.Variant, resource.Extension), resource.Length, resource.CompressedLength);
                        }
                    }

                    mUpdatableVersionListReady = true;
                    RefreshCheckInfoStatus();
                }
                catch (Exception e)
                {
                    throw new Exception($"Parse updatable version list with e ({e}).");
                }
                finally
                {
                    if (memoryStream != null)
                    {
                        memoryStream.Dispose();
                        memoryStream = null;
                    }
                }
            }

            private void OnLoadUpdatableVersionListFailure(string fileUri, string errormessage, object userData)
            {
                errormessage = string.IsNullOrEmpty(errormessage) ? "<Empty>" : errormessage;
                throw new Exception($"Updatable version list ({fileUri}) is invalid, error message is ({errormessage})");
            }

            private void OnLoadReadOnlyVersionListSuccess(string fileUri, byte[] bytes, float duration, object userData)
            {
                if (mReadOnlyVersionListReady)
                {
                    throw new Exception("Read-only version list has been parsed.");
                }

                MemoryStream memoryStream = null;
                try
                {
                    memoryStream = new MemoryStream(bytes, false);
                    var versionList = mResourceManager.mReadOnlyVersionListSerializer.Deserialize(memoryStream);
                    if (!versionList.IsValid)
                    {
                        throw new Exception("Deserialize read-only version list failure.");
                    }

                    var resources = versionList.Resources;
                    var fileSystems = versionList.FileSystems;

                    foreach (var fileSystem in fileSystems)
                    {
                        var resourceIndexes = fileSystem.ResourceIndexes;
                        foreach (var resourceIndex in resourceIndexes)
                        {
                            var resource = resources[resourceIndex];
                            SetCachedFileSystemName(new ResourceName(resource.Name, resource.Variant, resource.Extension), fileSystem.Name);
                        }
                    }

                    foreach (var resource in resources)
                    {
                        SetReadOnlyInfo(new ResourceName(resource.Name, resource.Variant, resource.Extension), (LoadType)resource.LoadType, resource.Length, resource.HashCode);
                    }

                    mReadOnlyVersionListReady = true;
                    RefreshCheckInfoStatus();
                }
                catch (Exception e)
                {
                    throw new Exception($"Parse read-only version list with e ({e}).");
                }
                finally
                {
                    if (memoryStream != null)
                    {
                        memoryStream.Dispose();
                        memoryStream = null;
                    }
                }
            }

            private void OnLoadReadOnlyVersionListFailure(string fileUri, string errormessage, object userData)
            {
                if (mReadOnlyVersionListReady)
                {
                    throw new Exception("Read-only version list has been parsed.");
                }

                mReadOnlyVersionListReady = true;
                RefreshCheckInfoStatus();
            }

            private void OnLoadReadWriteVersionListSuccess(string fileUri, byte[] bytes, float duration, object userData)
            {
                if (mReadWriteVersionListReady)
                {
                    throw new Exception("Read-write version list has been parsed.");
                }

                MemoryStream memoryStream = null;
                try
                {
                    memoryStream = new MemoryStream(bytes, false);
                    var versionList = mResourceManager.mReadWriteVersionListSerializer.Deserialize(memoryStream);
                    if (!versionList.IsValid)
                    {
                        throw new Exception("Deserialize read-write version list failure.");
                    }

                    var resources = versionList.Resources;
                    var fileSystems = versionList.FileSystems;

                    foreach (var fileSystem in fileSystems)
                    {
                        var resourceIndexes = fileSystem.ResourceIndexes;
                        foreach (var resourceIndex in resourceIndexes)
                        {
                            var resource = resources[resourceIndex];
                            SetCachedFileSystemName(new ResourceName(resource.Name, resource.Variant, resource.Extension), fileSystem.Name);
                        }
                    }

                    foreach (var resource in resources)
                    {
                        SetReadWriteInfo(new ResourceName(resource.Name, resource.Variant, resource.Extension), (LoadType)resource.LoadType, resource.Length, resource.HashCode);
                    }

                    mReadWriteVersionListReady = true;
                    RefreshCheckInfoStatus();
                }
                catch (Exception e)
                {
                    throw new Exception($"Parse read-write version list with e ({e}).");
                }
                finally
                {
                    if (memoryStream != null)
                    {
                        memoryStream.Dispose();
                        memoryStream = null;
                    }
                }
            }

            private void OnLoadReadWriteVersionListFailure(string fileUri, string errormessage, object userData)
            {
                if (mReadWriteVersionListReady)
                {
                    throw new Exception("Read-write version list has been parsed.");
                }

                mReadWriteVersionListReady = true;
                RefreshCheckInfoStatus();
            }
        }
    }
}