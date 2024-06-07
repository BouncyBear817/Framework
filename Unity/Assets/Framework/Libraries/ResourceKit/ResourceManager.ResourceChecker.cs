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
                                var resourcePath =
                                    Utility.Path.GetRegularPath(Path.Combine(mResourceManager.mReadWritePath,
                                        resourceFullName));
                                if (checkInfo.NeedMoveToDisk)
                                {
                                    //TODO：need file system
                                }
                            }
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
                            break;
                        default:
                            throw new Exception($"Check resources ({resourceName}) error with unknown status.");
                    }
                }
            }

            private void OnLoadUpdatableVersionListSuccess(string fileUri, byte[] bytes, float duration,
                object userData)
            {
            }

            private void OnLoadUpdatableVersionListFailure(string fileUri, string errormessage, object userData)
            {
            }

            private void OnLoadReadOnlyVersionListSuccess(string fileUri, byte[] bytes, float duration, object userData)
            {
            }

            private void OnLoadReadOnlyVersionListFailure(string fileUri, string errormessage, object userData)
            {
            }

            private void OnLoadReadWriteVersionListSuccess(string fileUri, byte[] bytes, float duration,
                object userData)
            {
            }

            private void OnLoadReadWriteVersionListFailure(string fileUri, string errormessage, object userData)
            {
            }
        }
    }
}